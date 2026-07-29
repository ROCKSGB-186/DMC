using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DMC.Helper;
using DMC.Models;

/// <summary>
/// 项目层级结构服务类 - 专门用于解析ancestors字段构建层级结构
/// </summary>
public class ProjectHierarchyService
{
    /// <summary>
    /// 项目层级节点模型 - 用于表示项目结构树中的每一个节点
    /// </summary>
    public class ProjectHierarchyNode
    {
        /// <summary>
        /// 节点ID - 对应数据库中的id字段
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 节点名称 - 对应数据库中的name字段
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 节点类型 - 0项目 1阶段 2专业 3子项 4文件夹 5文件
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 父节点引用
        /// </summary>
        public ProjectHierarchyNode Parent { get; set; }

        /// <summary>
        /// 子节点集合 - 存储所有下级节点
        /// </summary>
        public List<ProjectHierarchyNode> Children { get; set; } = new List<ProjectHierarchyNode>();

        /// <summary>
        /// 关联的原始数据 - 存储从数据库获取的原始记录
        /// </summary>
        public GetKeepProjectDirModel OriginalData { get; set; }

        /// <summary>
        /// 获取完整路径字符串 - 例如："项目名称/阶段名称/专业名称/文件名称"
        /// </summary>
        public string FullPath
        {
            get
            {
                var pathParts = new List<string>();
                var currentNode = this;

                // 从当前节点向上遍历到根节点
                while (currentNode != null)
                {
                    pathParts.Insert(0, currentNode.Name); // 插入到列表开头
                    currentNode = currentNode.Parent;
                }

                return string.Join("/", pathParts);
            }
        }

        /// <summary>
        /// 节点级别 - 根节点为0，其子节点为1，依此类推
        /// </summary>
        public int Level
        {
            get
            {
                int level = 0;
                var currentNode = this.Parent;
                while (currentNode != null)
                {
                    level++;
                    currentNode = currentNode.Parent;
                }
                return level;
            }
        }
    }

    /// <summary>
    /// 根据项目ID和过滤条件获取项目的完整层级结构（基于ancestors字段）
    /// </summary>
    /// <param name="projectId">项目ID</param>
    /// <param name="fileTypeFilter">文件类型过滤条件，如".pdf"</param>
    /// <param name="startTime">开始时间</param>
    /// <param name="endTime">结束时间</param>
    /// <returns>项目层级结构的根节点</returns>
    public ProjectHierarchyNode GetProjectHierarchy(string projectId, string fileTypeFilter = "pdf", string startTime = "", string endTime = "")
    {
        // 1. 获取项目的所有相关数据（包括所有层级的节点）
        var allProjectData = GetAllProjectRelatedData(projectId, fileTypeFilter, startTime, endTime);

        // 2. 构建基于ancestors的层级结构
        var rootNode = BuildHierarchyFromAncestors(allProjectData, projectId);

        return rootNode;
    }

    /// <summary>
    /// 一次性获取项目的所有相关数据
    /// </summary>
    /// <param name="projectId">项目ID</param>
    /// <param name="fileTypeFilter">文件类型过滤</param>
    /// <param name="startTime">开始时间</param>
    /// <param name="endTime">结束时间</param>
    /// <returns>项目相关的所有数据列表</returns>
    private List<GetKeepProjectDirModel> GetAllProjectRelatedData(string projectId, string fileTypeFilter, string startTime, string endTime)
    {
        List<GetKeepProjectDirModel> result = new List<GetKeepProjectDirModel>();

        DataTable dataTable;
        if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
        {
            // 使用时间范围查询
            dataTable = SQLiteDataBase.GetDataFromMysql(
                "qz_keep_project",
                "name",
                "create_time",
                projectId,
                fileTypeFilter,
                startTime,
                endTime);
        }
        else
        {
            // 只按项目ID查询
            dataTable = SQLiteDataBase.GetDataFromMysql(
                "qz_keep_project",
                "name",
                "create_time",
                projectId,
                fileTypeFilter,
                "1900-01-01 00:00:00",  // 最早时间
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); // 当前时间
        }

        if (dataTable.Rows.Count == 0)
            return result;

        // 将DataTable转换为实体列表
        foreach (DataRow row in dataTable.Rows)
        {
            // 如果有文件类型过滤，检查文件名是否匹配
            string fileName = row["name"].ToString();
            if (!string.IsNullOrEmpty(fileTypeFilter) && !fileName.EndsWith(fileTypeFilter, StringComparison.OrdinalIgnoreCase))
            {
                continue; // 跳过不符合条件的记录
            }

            var model = new GetKeepProjectDirModel
            {
                id = row["id"].ToString(),
                name = row["name"].ToString(),
                type = Convert.ToInt32(row["type"]),
                parentId = row["parent_id"].ToString(),
                ancestors = row["ancestors"].ToString(),
                createTime = row["create_time"].ToString(),
                folded = row["folded"]?.ToString() ?? "0",
                projectId = row["project_id"].ToString(),
                // 根据需要添加其他字段...
            };

            result.Add(model);
        }

        return result;
    }

    /// <summary>
    /// 基于ancestors字段构建层级结构
    /// </summary>
    /// <param name="allProjectData">所有项目数据</param>
    /// <param name="projectId">项目ID</param>
    /// <returns>构建好的层级树根节点</returns>
    private ProjectHierarchyNode BuildHierarchyFromAncestors(List<GetKeepProjectDirModel> allProjectData, string projectId)
    {
        // 创建所有节点的字典，便于快速查找
        var nodeDict = new Dictionary<string, ProjectHierarchyNode>();

        // 创建数据字典，便于访问原始数据
        var dataDict = allProjectData.ToDictionary(x => x.id, x => x);

        // 第一步：创建所有节点
        foreach (var data in allProjectData)
        {
            if (!nodeDict.ContainsKey(data.id))
            {
                var node = CreateNode(data);
                nodeDict[data.id] = node;
            }
        }

        Console.WriteLine($"共创建了 {nodeDict.Count} 个节点");

        // 第二步：基于ancestors字段建立层级关系
        int relationshipCount = 0;

        // 按照ancestors长度排序，确保先处理路径上层级较高的节点
        var sortedData = allProjectData.OrderBy(x => ParseAncestors(x.ancestors).Count).ToList();

        foreach (var data in sortedData)
        {
            var currentNode = nodeDict[data.id];

            // 解析ancestors字段，获取祖先节点ID列表
            var ancestorIds = ParseAncestors(data.ancestors);

            Console.WriteLine($"处理节点: {data.id} ({data.name}), ancestors count: {ancestorIds.Count}, ancestors: [{string.Join(", ", ancestorIds)}]");

            if (ancestorIds.Count > 0)
            {
                // 直接父节点是ancestors列表中的最后一个ID
                string directParentId = ancestorIds.Last();

                Console.WriteLine($"  - 候选直接父节点ID: {directParentId}");

                // 检查父节点是否在当前数据集中
                if (nodeDict.ContainsKey(directParentId))
                {
                    var parentNode = nodeDict[directParentId];

                    // 建立父子关系
                    currentNode.Parent = parentNode;

                    // 避免重复添加
                    if (!parentNode.Children.Any(c => c.Id == currentNode.Id))
                    {
                        parentNode.Children.Add(currentNode);
                        relationshipCount++;
                        Console.WriteLine($"  - 成功建立父子关系: {parentNode.Name}({parentNode.Id}) -> {currentNode.Name}({currentNode.Id})");
                    }
                    else
                    {
                        Console.WriteLine($"  - 父子关系已存在: {parentNode.Name}({parentNode.Id}) -> {currentNode.Name}({currentNode.Id})");
                    }
                }
                else
                {
                    Console.WriteLine($"  - 父节点ID {directParentId} 不在当前数据集中，无法建立父子关系");

                    // 尝试从数据库中单独获取父节点信息
                    var parentData = GetNodeById(directParentId);
                    if (parentData != null)
                    {
                        // 创建父节点
                        if (!nodeDict.ContainsKey(directParentId))
                        {
                            var parentNode = CreateNode(parentData);
                            nodeDict[directParentId] = parentNode;

                            // 建立父子关系
                            currentNode.Parent = parentNode;

                            if (!parentNode.Children.Any(c => c.Id == currentNode.Id))
                            {
                                parentNode.Children.Add(currentNode);
                                relationshipCount++;
                                Console.WriteLine($"  - 成功创建并建立父子关系: {parentNode.Name}({parentNode.Id}) -> {currentNode.Name}({currentNode.Id})");
                            }
                        }
                        else
                        {
                            var parentNode = nodeDict[directParentId];
                            currentNode.Parent = parentNode;

                            if (!parentNode.Children.Any(c => c.Id == currentNode.Id))
                            {
                                parentNode.Children.Add(currentNode);
                                relationshipCount++;
                                Console.WriteLine($"  - 成功建立父子关系: {parentNode.Name}({parentNode.Id}) -> {currentNode.Name}({currentNode.Id})");
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"  - 该节点没有祖先，可能是根节点: {data.name}");
            }
        }

        Console.WriteLine($"总共建立了 {relationshipCount} 个父子关系");

        // 第三步：找到项目根节点
        if (nodeDict.ContainsKey(projectId))
        {
            Console.WriteLine($"找到项目根节点: {nodeDict[projectId].Name}");
            Console.WriteLine($"项目根节点的子节点数量: {nodeDict[projectId].Children.Count}");
            return nodeDict[projectId];
        }
        else
        {
            // 如果项目ID不在查询结果中，找到顶级节点
            var rootNode = FindTopMostNode(nodeDict);
            if (rootNode != null)
            {
                Console.WriteLine($"项目ID {projectId} 不在查询结果中，使用顶级节点 {rootNode.Name}({rootNode.Id}) 作为根节点");
                return rootNode;
            }
            else
            {
                throw new ArgumentException($"无法找到项目 {projectId} 的层级结构");
            }
        }
    }

    /// <summary>
    /// 根据ID从数据库获取单个节点信息
    /// </summary>
    /// <param name="nodeId">节点ID</param>
    /// <returns>节点信息</returns>
    private GetKeepProjectDirModel GetNodeById(string nodeId)
    {
        try
        {
            var dataTable = SQLiteDataBase.GetDataFromMysql("qz_keep_project", "id", nodeId);
            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new GetKeepProjectDirModel
                {
                    id = row["id"].ToString(),
                    name = row["name"].ToString(),
                    type = Convert.ToInt32(row["type"]),
                    parentId = row["parent_id"].ToString(),
                    ancestors = row["ancestors"].ToString(),
                    createTime = row["create_time"].ToString(),
                    folded = row["folded"]?.ToString() ?? "0",
                    projectId = row["project_id"].ToString(),
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"获取节点 {nodeId} 信息时出错: {ex.Message}");
        }
        return null;
    }

    /// <summary>
    /// 解析ancestors字段
    /// </summary>
    /// <param name="ancestors">ancestors字段内容</param>
    /// <returns>祖先节点ID列表</returns>
    private List<string> ParseAncestors(string ancestors)
    {
        if (string.IsNullOrEmpty(ancestors))
            return new List<string>();

        // 分割ancestors字符串得到ID列表，并去除空值和重复值
        var ids = ancestors.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(id => id.Trim())
                          .Where(id => !string.IsNullOrEmpty(id))
                          .Distinct()
                          .ToList();

        return ids;
    }

    /// <summary>
    /// 查找最顶级的节点
    /// </summary>
    /// <param name="nodeDict">节点字典</param>
    /// <returns>最顶级的节点</returns>
    private ProjectHierarchyNode FindTopMostNode(Dictionary<string, ProjectHierarchyNode> nodeDict)
    {
        // 查找没有父节点的节点
        foreach (var node in nodeDict.Values)
        {
            if (node.Parent == null)
            {
                return node;
            }
        }

        // 如果所有节点都有父节点，返回第一个节点
        return nodeDict.Values.FirstOrDefault();
    }

    /// <summary>
    /// 根据原始数据创建层级节点
    /// </summary>
    /// <param name="data">原始数据模型</param>
    /// <returns>层级节点对象</returns>
    private ProjectHierarchyNode CreateNode(GetKeepProjectDirModel data)
    {
        return new ProjectHierarchyNode
        {
            Id = data.id,
            Name = data.name,
            Type = data.type,
            OriginalData = data
        };
    }

    /// <summary>
    /// 获取特定类型的所有节点（如所有文件类型的节点）
    /// </summary>
    /// <param name="rootNode">根节点</param>
    /// <param name="targetType">目标类型（如5表示文件）</param>
    /// <returns>符合条件的节点列表</returns>
    public List<ProjectHierarchyNode> GetNodesByType(ProjectHierarchyNode rootNode, int targetType)
    {
        var result = new List<ProjectHierarchyNode>();
        CollectNodesByType(rootNode, targetType, result);
        return result;
    }

    /// <summary>
    /// 递归收集指定类型的所有节点
    /// </summary>
    /// <param name="node">当前节点</param>
    /// <param name="targetType">目标类型</param>
    /// <param name="result">结果列表</param>
    private void CollectNodesByType(ProjectHierarchyNode node, int targetType, List<ProjectHierarchyNode> result)
    {
        if (node.Type == targetType)
        {
            result.Add(node);
        }

        // 递归遍历子节点
        foreach (var child in node.Children)
        {
            CollectNodesByType(child, targetType, result);
        }
    }

    /// <summary>
    /// 获取所有叶子节点（文件节点）
    /// </summary>
    /// <param name="rootNode">根节点</param>
    /// <returns>所有叶子节点列表</returns>
    public List<ProjectHierarchyNode> GetAllLeafNodes(ProjectHierarchyNode rootNode)
    {
        var leafNodes = new List<ProjectHierarchyNode>();
        CollectLeafNodes(rootNode, leafNodes);
        return leafNodes;
    }

    /// <summary>
    /// 递归收集所有叶子节点
    /// </summary>
    /// <param name="node">当前节点</param>
    /// <param name="leafNodes">叶子节点列表</param>
    private void CollectLeafNodes(ProjectHierarchyNode node, List<ProjectHierarchyNode> leafNodes)
    {
        // 如果当前节点没有子节点，它是叶子节点
        if (node.Children.Count == 0)
        {
            leafNodes.Add(node);
        }
        else
        {
            // 递归处理子节点
            foreach (var child in node.Children)
            {
                CollectLeafNodes(child, leafNodes);
            }
        }
    }
}

// 使用示例
public class AncestorBasedHierarchyUsageExample
{
    /// <summary>
    /// 示例：如何使用基于ancestors的层级结构服务
    /// </summary>
    public void ExampleUsage()
    {
        var service = new ProjectHierarchyService();

        // 获取项目层级结构
        var hierarchyRoot = service.GetProjectHierarchy(
            "WBS2023110800000016",  // 项目ID
            "pdf",                  // 文件类型过滤
            "2023-01-01",          // 开始时间
            "2023-12-31"           // 结束时间
        );

        // 获取所有文件类型的节点
        var allFiles = service.GetNodesByType(hierarchyRoot, 5); // 5代表文件

        Console.WriteLine("=== 所有文件及其完整路径 ===");
        // 输出每个文件的完整路径
        foreach (var fileNode in allFiles)
        {
            Console.WriteLine($"文件: {fileNode.Name}");
            Console.WriteLine($"完整路径: {fileNode.FullPath}");
            Console.WriteLine($"所在层级: {fileNode.Level}");
            Console.WriteLine($"A1数量: {fileNode.OriginalData?.folded}");
            Console.WriteLine("---");
        }

        // 获取所有叶子节点（真正的文件节点）
        var allLeafNodes = service.GetAllLeafNodes(hierarchyRoot);
        Console.WriteLine($"\n总共找到 {allLeafNodes.Count} 个文件");
    }
}