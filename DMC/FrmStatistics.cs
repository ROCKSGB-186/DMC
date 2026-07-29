using DMC.Helper;
using DMC.Models;
using iTextSharp.text;
using Mysqlx.Crud;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static DMC.SystemTempData;

namespace DMC
{
    public partial class FrmStatistics : BaseForm
    {
        /// <summary>
        /// 初始化窗体
        /// </summary>
        public FrmStatistics()
        {
            InitializeComponent();
            //双缓冲，解决闪烁问题
            dataGridView_流程统计.DoubleBufferedDataGirdView(true);
            dataGridView_流程统计.AutoGenerateColumns = false;
            dataGridView_流程统计.RowCount = 15;
            //滚动条到底时的事件；
            //dataGridView_流程统计.RegistScrollToEndEvent(dataGrid_OnScrollToEnd);  
            // 在初始化时启用自定义绘制
            //EnableCustomCellPainting();
            comboBox_流程统计项目名称.Items.Clear();
            comboBox_流程统计项目名称.Items.Add("00全部项目");
            comboBox_流程统计项目名称.SelectedItem = "00全部项目";
            comboBox_流程类型.Items.Clear();
            comboBox_流程类型.Items.Add("00全部流程");
            comboBox_流程类型.SelectedItem = "00全部流程";
            comboBox_流程人员.Items.Clear();
            comboBox_流程人员.Items.Add("00全部人员");
            comboBox_流程人员.SelectedItem = "00全部人员";
            // 为dataGridView_目录添加行数变化事件处理程序
            dataGridView_目录.RowsAdded += DataGridView_目录_RowsAdded;
            dataGridView_目录.RowsRemoved += DataGridView_目录_RowsRemoved;
            ComboboxSelect();
        }

        /// <summary>
        /// 修改后的查询按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_查询_Click(object sender, EventArgs e)
        {
            try
            {
                Splasher.Show(typeof(FrmLoading));
                    //获取开始时间
            DateTime startTime = DateTime.Now;
                //显示时间
                label_Time.Text = startTime.ToString();
                //获取时间间隔计算时间差
                var startAndEndDate = dateTimePicker_截至.Value.Date - dateTimePicker_开始.Value.Date;
                var startDateTime = dateTimePicker_开始.Value.ToString("yyyy-M-d 00:00:00");//开始时间
                var endDateTime = dateTimePicker_截至.Value.ToString("yyyy-M-d 23:59:59");//结束时间

                // 判断当前是在哪个Tab页面
                if (TabControl_统计.SelectedTab == tabPage_项目目录)
                {
                    // 在项目目录页面，执行新的层级结构查询
                    HandleProjectCatalogQuery(startDateTime, endDateTime);
                }
                else
                {
                    // 其他页面保持原有逻辑
                    string subType = "";
                    queryApply.type = subType;
                    ///获取项目列表文件来源0 项目区 1归档区
                    if (radioButton_项目文件区.Checked) { QueryInfos.fileType = 0; } else if (radioButton_档案区.Checked) { QueryInfos.fileType = 1; }

                    if (startAndEndDate.TotalDays >= 180 && startAndEndDate.TotalDays <= 269)
                    {
                        var dialogResult = MessageBox.Show("您要查询的时间已超过半年，用时可能要需要 3 - 5 分钟！！您是否继续查询？", "查询时长提示：", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            //读取所有项目列表
                            LoadMysql_Apply_Project_UserList(startDateTime, endDateTime);
                        }
                    }
                    else if (startAndEndDate.TotalDays >= 270 && startAndEndDate.TotalDays <= 360)
                    {
                        var dialogResult = MessageBox.Show("您要查询的日期已接近一年，用时可能要需要5 - 8 分钟！！您是否继续查询？", "查询时长提示：", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            ///读取所有项目列表
                            LoadMysql_Apply_Project_UserList(startDateTime, endDateTime);
                        }
                    }
                    else
                    {
                        //读取所有项目列表
                        LoadMysql_Apply_Project_UserList(startDateTime, endDateTime);
                    }
                }

                //记录结束时间
                DateTime endTime = DateTime.Now;
                //计算时间差
                TimeSpan elapsedTime = endTime - startTime;
                label_Time.Text = elapsedTime.ToString();
                Splasher.Close();
            }
            catch (Exception ex)
            {
                Splasher.Close();
                MessageBox.Show("查询时报了如下错误:" + ex.Message);
            }
            
            
        }

        private static string installPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        public static string DMC_SQLiteDBFilePath = Path.Combine(installPath, "DMC_SQLiteDB.db");
        DataTable qz_approvalList = new DataTable();

        /// <summary>
        /// 项目List:1、id; 2、名称Name 3、值Value
        /// </summary>
        private List<List<ProjectPropertyModel>> ProjectPropertieListS = new List<List<ProjectPropertyModel>>();

        /// <summary>
        /// 文件夹架构List：1、ParentId：父ID /2、PrimaryKey：主键 /3、Name：名 /4、Type：类型1文件夹2文件 /5、fileUpload：上传文件参数
        /// </summary>
        private List<DirectoryStructureModel> DirectoryStructureList = null;

        /// <summary>
        /// 查询流程相关内容类型：查询流程相关内容类型：1：processtypeid：流程类型：（0签名签章 1出版 2下载 3归档 4其他，不传就是查询所有）；2：type审批状态（0我发起的 1待审批 2已审批，不传就是查询所有）；3：pageNum 页数；4：pageSize 要查询的条数；5：proName 项目名称；6：userName 发起人；7：startTime 开始时间；8：endTime 结束时间  
        /// </summary>
        private QueryApply queryApply = null;

        /// <summary>
        /// 获取到审批流程信息列表List /1：applyXh序号; /2：proName项目名称; /3：processtypeId流程类型 0签名 5签章 1出版 2下载 3归档 4其他 6签名签章; /4： userName用户名称; /5：NAME流程标题; /6：result审批状态0进行中 1已通过 -1未通过; /7：createTime创建时间; /8：lastTime最后审批时间; /9： remark备注; /10： id主键ID; 
        /// </summary>
        private List<ApplyInfoModel> ResultDataItemList = new List<ApplyInfoModel>();

        /// <summary>
        /// 文件列表查询条件:1/fileType,文件来源0 项目区 1归档区;2/type，发起类型 0购物车 1文件夹 2项目 3文件 /3：fileIds,流id列表用，分割/4：parentId, 上级ID/5：tab 是否获得未归档的文件 0未归档 1全部 默认传1，就出版和下载都传 0  剩下的都传1
        /// </summary>
        private QueryApprovalProjectStructure QueryInfos = new QueryApprovalProjectStructure()
        {

            fileType = 0, //文件来源0 项目区 1归档区
            type = 3,    //发起类型 0购物车 1文件夹 2项目 3文件
            fileIds = "",  //流id列表  用  ，分割
            parentId = "0",  //上级ID
            applyId = "", //审批详情主键Id
            tab = "1"  // 是否获得未归档的文件 0未归档 1全部 默认传1，就出版和下载都传 0  剩下的都传1
        };

        /// <summary>
        /// 项目Info临时变量
        /// </summary>
        private List<ApplyInfoModel> projectDataTest = new List<ApplyInfoModel>();

        /// <summary>
        /// 存储ApplyListModel对象的列表                                                                               
        /// </summary>
        private List<ApplyListModel> applyStatisticsListTemp = new List<ApplyListModel>();

        /// <summary>
        /// 初始化一个读线下流程详情列表文件的变量；
        /// </summary>
        private List<ApplyInfoModel> applyInfoTempFile = new List<ApplyInfoModel>();

        /// <summary>
        /// 存储用户对象的列表
        /// </summary>
        private static List<projectDeptModel> statisticsUserInfoListTemp = null;

        /// <summary>
        /// 项目的阶段、专业、角色、人员信息
        /// </summary>
        //private static List<projectDeptModel> projectUserInfoList = null;

        /// <summary>
        /// 组织架构下的所有用户集合
        /// </summary>
        private static List<QzUserResultModel> deptUserListTemp = new List<QzUserResultModel>();

        /// <summary>
        /// 项目用户数据列表
        /// </summary>
        private static List<ProjectUserDataList> userDataLists = null;
        private static ProjectUserDataList userDataTemp = null;

        /// <summary>
        /// 查询档案项目文件
        /// </summary>
        private QueryKeepProjectFile queryInfo = new QueryKeepProjectFile()
        {
            pageNum = 1,
            pageSize = 100

        };
        /// <summary>
        /// 树状架构节点点击事件
        /// </summary>
        private bool treeViewNodeMouseClick = true;

        string comboBoxApply = "00全部流程";
        string comboBoxProject = "00全部项目";
        string comboBoxUser = "00全部人员";

        /// <summary>
        /// 设定每次查询服务器的条数
        /// </summary>
        private int queryApplyPageSize = 10;

        /// <summary>
        /// 默认加载窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmStatistics_Load(object sender, EventArgs e)
        {
            #region 加载所有事务流程
            queryApply = new QueryApply()
            {
                //processtypeId = type.ToString(),//判断ID显示流程内容
                //type = "1",
                pageNum = 1,
                pageSize = queryApplyPageSize
            };
            #endregion
            // 加载树状架构
            treeView_Archive_Load();
        }

        #region 树状架构加载

        /// <summary>
        /// 树状架构加载
        /// </summary>
        private void treeView_Archive_Load()
        {
            // 建立档案组织架构变量
            var keepDeptData = new List<SelectKeepDeptModel>();
            //获取组织架构数据加载归档目录层级
            if (HttpGet(AppGlobalModel.SelectKeepDept + "?parentId=0", ref keepDeptData))
            {
                foreach (var keepDeptItem in keepDeptData)
                {
                    TreeNode treeNode = new TreeNode();
                    //根目录名称
                    treeNode.Text = keepDeptItem.name;//根目录名称
                    treeNode.Tag = keepDeptItem;//根目录名称
                    treeView_Archive.Nodes.Add(treeNode);//添加根节点
                }
            }
            else
            {
                this.Close();
            }
            // 对所有子节点进行排序  
            FrmArchiveManage.SortTreeViewNodes(treeView_Archive.Nodes);

            // 默认展开所有节点  
            treeView_Archive.ExpandAll();
        }

        /// <summary>
        /// 双击打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        private void treeView_ProjectFile_Load()
        {
            //加载组织架构
            if (AppGlobalModel.DeptList != null && AppGlobalModel.DeptList.Any())
            {
                foreach (var item in AppGlobalModel.DeptList.Where(o => o.parentId == "0"))
                {
                    var projectFileTreeView = new ProjectFileTreeViewModel();
                    projectFileTreeView.id = item.deptId;
                    projectFileTreeView.name = item.deptName;
                    projectFileTreeView.deptType = item.deptType;
                    projectFileTreeView.proType = -1;
                    projectFileTreeView.parentId = item.parentId;

                    TreeNode projectFileTreeNode = new TreeNode();
                    //根目录名称
                    projectFileTreeNode.Text = projectFileTreeView.name;
                    projectFileTreeNode.Tag = projectFileTreeView;
                    treeView_Archive.Nodes.Add(projectFileTreeNode);

                    LoadTreeView(projectFileTreeNode);
                }
            }
        }

        /// <summary>
        /// 加载组织架构
        /// </summary>
        /// <param name="treeNode">架构结点</param>
        private void LoadTreeView(TreeNode treeNode)
        {
            // 获取父节点 ID  
            var parentId = ((ProjectFileTreeViewModel)treeNode.Tag).id;

            // 遍历所有子节点，判断是否属于当前节点的子节点  
            foreach (var item in AppGlobalModel.DeptList.Where(o => o.parentId == parentId))
            {
                // 跳过非法节点  
                if (item.deptType == null || item.deptType == "3")
                {
                    continue;
                }

                // 创建新的项目树视图模型  
                var projectFileTreeView = new ProjectFileTreeViewModel
                {
                    id = item.deptId,
                    name = item.deptName,
                    deptType = item.deptType,
                    proType = -1,
                    parentId = item.parentId
                };

                // 创建新的树节点  
                TreeNode root = new TreeNode
                {
                    // 设置树节点名称  
                    Text = projectFileTreeView.name,
                    Tag = projectFileTreeView
                };

                // 将新节点添加到当前节点的子节点列表中  
                treeNode.Nodes.Add(root);

                // 递归加载子节点  
                LoadTreeView(root);
            }

            // 默认展开所有节点  
            treeView_Archive.ExpandAll();
            // 选择第一个节点  

            // 确保TreeView有节点  
            if (treeView_Archive.Nodes.Count > 0)
            {
                // 选中第一个节点  
                treeView_Archive.SelectedNode = treeView_Archive.Nodes[0];
                treeView_Archive.Focus();
                treeView_Archive.SelectedNode.Expand();

                // 将滚动条移动到最顶层  
                treeView_Archive.Nodes[0].EnsureVisible();
            }
        }

        #endregion

        #region 新方法查询人员相关

        /// <summary>
        /// 人员统计详情模型
        /// </summary>
        private class PersonnelStatisticsDetail
        {
            public string ProjectNo { get; set; }        // 项目编号
            public string ProjectName { get; set; }      // 项目名称
            public string UserName { get; set; }         // 用户姓名
            public string UserDepartment { get; set; }   // 用户部门
            public string StageName { get; set; }        // 阶段名称
            public string MajorName { get; set; }        // 专业名称
            public string SubProjectName { get; set; }   // 子项名称
            public string FileName { get; set; }         // 文件名称
            public int FileCount { get; set; }           // 文件数量
            public string Folded { get; set; }           // 折合A1数
            public string UserId { get; set; }           // 用户ID
            public string RoleName { get; set; }         // 角色名称
        }

        /// <summary>
        /// 获取人员统计详情
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        /// <param name="userId">用户ID（可选，如果不指定则返回所有用户）</param>
        /// <returns>人员统计详情列表</returns>
        private List<PersonnelStatisticsDetail> GetPersonnelStatisticsDetails(string projectId, string startDateTime, string endDateTime, string userId = null)
        {
            // 创建人员统计详情列表
            var details = new List<PersonnelStatisticsDetail>();
            // 获取所有项目数据（包括文件信息）
            var allProjectData = new DataTable();
            if (radioButton_项目文件区.Checked)
            {
                // 从项目表获取数据
                allProjectData = SQLiteDataBase.GetDataFromMysql(
                   "qz_project",
                   "name",
                   "create_time",
                   projectId,
                   "pdf",
                   startDateTime,
                   endDateTime);
            }
            else if (radioButton_档案区.Checked)
            {
                // 从档案表获取数据
                allProjectData = SQLiteDataBase.GetDataFromMysql(
                   "qz_keep_project",
                   "name",
                   "create_time",
                   projectId,
                   "pdf",
                   startDateTime,
                   endDateTime);
            }

            // 遍历所有文件，构建详细信息
            foreach (DataRow row in allProjectData.Rows)
            {
                var fileData = new GetKeepProjectDirModel
                {
                    id = row["id"].ToString(),
                    name = row["name"].ToString(),
                    type = Convert.ToInt32(row["type"]),
                    parentId = row["parent_id"].ToString(),
                    ancestors = row["ancestors"].ToString(),
                    createTime = row["create_time"].ToString(),
                    folded = row["folded"]?.ToString() ?? "0",
                    projectId = row["project_id"].ToString(),
                    userId = row["user_id"].ToString(), // 用户ID
                };

                // 如果指定了用户ID且当前文件不属于该用户，则跳过
                if (!string.IsNullOrEmpty(userId) && fileData.userId != userId)
                {
                    continue;
                }

                // 如果不是文件类型（type != 5），跳过
                if (fileData.type != 5) continue;

                // 解析ancestors字段，获取完整的层级信息
                var ancestorIds = ParseAncestors(fileData.ancestors);

                // 获取各层级节点信息
                var pathInfo = GetUserPathInfoFromAncestors(ancestorIds, fileData);

                // 获取用户信息
                var userInfo = GetUserInfoById(fileData.userId);

                // 创建详情记录
                var detail = new PersonnelStatisticsDetail
                {
                    ProjectNo = pathInfo.ProjectNo ?? "无",
                    ProjectName = pathInfo.ProjectName ?? "无",
                    UserName = userInfo?.realName ?? "未知用户",
                    UserDepartment = userInfo?.deptName ?? "未知部门",
                    StageName = pathInfo.StageName ?? "无",
                    MajorName = pathInfo.MajorName ?? "无",
                    SubProjectName = pathInfo.SubProjectName ?? "无",
                    FileName = fileData.name,
                    FileCount = 1, // 每个记录代表一个文件
                    Folded = fileData.folded,
                    UserId = fileData.userId,
                    RoleName = pathInfo.RoleName ?? "普通用户"
                };

                details.Add(detail);
            }

            return details;
        }

        /// <summary>
        /// 根据ancestors获取用户相关的路径信息
        /// </summary>
        /// <param name="ancestorIds">祖先ID列表</param>
        /// <param name="fileData">文件数据</param>
        /// <returns>路径信息</returns>
        private PersonnelStatisticsDetail GetUserPathInfoFromAncestors(List<string> ancestorIds, GetKeepProjectDirModel fileData)
        {
            var pathInfo = new PersonnelStatisticsDetail();

            if (ancestorIds.Count == 0) return pathInfo;

            // 获取项目信息（通常是ancestors的第二个ID，第一个是系统根）
            if (ancestorIds.Count >= 2)
            {
                var projectInfo = GetProjectInfoById(ancestorIds[1]); // 第二个ID是项目ID
                if (projectInfo != null)
                {
                    pathInfo.ProjectNo = projectInfo.identifier ?? "无";
                    pathInfo.ProjectName = projectInfo.name ?? "无";
                }
            }

            // 获取阶段信息（通常是第三个ID）
            if (ancestorIds.Count >= 3)
            {
                var stageInfo = GetNodeInfoById(ancestorIds[2]);
                if (stageInfo != null)
                {
                    pathInfo.StageName = stageInfo.name ?? "无";
                }
            }

            // 获取子项信息（根据示例，子项可能在ancestors的第5个位置）
            if (ancestorIds.Count >= 5)
            {
                var subProjectInfo = GetNodeInfoById(ancestorIds[4]); // 第5个ID是子项ID
                if (subProjectInfo != null)
                {
                    pathInfo.SubProjectName = subProjectInfo.name ?? "无";
                }
            }

            // 获取专业信息（根据示例，专业可能在ancestors的第6个位置）
            if (ancestorIds.Count >= 6)
            {
                var majorInfo = GetNodeInfoById(ancestorIds[5]); // 第6个ID是专业ID
                if (majorInfo != null)
                {
                    pathInfo.MajorName = majorInfo.name ?? "无";
                }
            }

            return pathInfo;
        }

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        private QzUserResultModel GetUserInfoById(string userId)
        {
            try
            {
                // 从用户表获取用户信息
                var userDataTable = SQLiteDataBase.GetDataFromMysql("qz_user", "id", userId);

                if (userDataTable.Rows.Count > 0)
                {
                    var row = userDataTable.Rows[0];
                    return new QzUserResultModel
                    {
                        id = row["id"].ToString(),
                        realName = row["real_name"]?.ToString() ?? row["name"]?.ToString() ?? "未知用户",
                        deptName = row["dept_name"]?.ToString() ?? "未知部门"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取用户信息时出错: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// 在dataGridView_人员统计中填充人员统计数据
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        /// <param name="userId">用户ID（可选）</param>
        private void PopulatePersonnelStatisticsDataGrid(string projectId, string startDateTime, string endDateTime, string userId = null)
        {
            try
            {
                // 1. 获取人员统计详情
                var personnelDetails = GetPersonnelStatisticsDetails(projectId, startDateTime, endDateTime, userId);

                // 2. 根据当前的选择进行分组和排序
                var groupedDetails = personnelDetails
                    .GroupBy(detail => detail.UserId) // 按用户ID分组
                    .Select(group => new
                    {
                        UserId = group.Key,
                        UserName = group.First().UserName,
                        UserDepartment = group.First().UserDepartment,
                        TotalFileCount = group.Sum(d => d.FileCount),
                        TotalFolded = group.Sum(d =>
                        {
                            decimal.TryParse(d.Folded, out decimal result);
                            return result;
                        }),
                        Files = group.ToList()
                    })
                    .OrderByDescending(g => g.TotalFileCount); // 按文件数量降序排列

                // 3. 清空现有数据
                dataGridView_人员统计.Rows.Clear();

                // 4. 填充分组后的数据
                foreach (var group in groupedDetails)
                {
                    // 添加汇总行
                    var summaryRow = new DataGridViewRow();
                    summaryRow.Cells.Add(new DataGridViewTextBoxCell { Value = group.UserName }); // 用户姓名
                    summaryRow.Cells.Add(new DataGridViewTextBoxCell { Value = group.UserDepartment }); // 用户部门
                    summaryRow.Cells.Add(new DataGridViewTextBoxCell { Value = group.Files.Count }); // 文件数量
                    summaryRow.Cells.Add(new DataGridViewTextBoxCell { Value = group.TotalFolded.ToString("F2") }); // A1数量
                    summaryRow.Cells.Add(new DataGridViewTextBoxCell { Value = group.Files.FirstOrDefault()?.ProjectName ?? "无" }); // 项目名称
                    summaryRow.Cells.Add(new DataGridViewTextBoxCell { Value = group.Files.FirstOrDefault()?.StageName ?? "无" }); // 阶段
                    summaryRow.Cells.Add(new DataGridViewTextBoxCell { Value = group.Files.FirstOrDefault()?.MajorName ?? "无" }); // 专业
                    summaryRow.Cells.Add(new DataGridViewTextBoxCell { Value = group.Files.FirstOrDefault()?.SubProjectName ?? "无" }); // 子项

                    dataGridView_人员统计.Rows.Add(summaryRow);
                }

                // 5. 设置DataGridView样式
                dataGridView_人员统计.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
                dataGridView_人员统计.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                // 6. 显示总计信息
                UpdatePersonnelStatisticsSummaryLabels(personnelDetails);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"填充人员统计数据时发生错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 更新人员统计汇总标签
        /// </summary>
        /// <param name="details">人员统计详情列表</param>
        private void UpdatePersonnelStatisticsSummaryLabels(List<PersonnelStatisticsDetail> details)
        {
            int totalFileCount = details.Sum(d => d.FileCount);
            decimal totalFolded = details.Sum(d =>
            {
                decimal.TryParse(d.Folded, out decimal result);
                return result;
            });

            label_合计.Text = totalFileCount.ToString();
            label_合计A1.Text = totalFolded.ToString("F2"); // 保留两位小数
        }

        /// <summary>
        /// 处理人员统计查询
        /// </summary>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        private void HandlePersonnelStatisticsQuery(string startDateTime, string endDateTime)
        {
            try
            {
                // 1. 获取选中节点的项目ID
                string projectId = GetSelectedProjectId();
                if (string.IsNullOrEmpty(projectId))
                {
                    MessageBox.Show("请先在左侧树形结构中选择一个项目！");
                    return;
                }

                // 2. 获取选中的用户（如果有的话）
                string selectedUserId = GetSelectedUserId();

                // 3. 填充人员统计数据到DataGridView
                PopulatePersonnelStatisticsDataGrid(projectId, startDateTime, endDateTime, selectedUserId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询人员统计时发生错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 获取选中的用户ID
        /// </summary>
        /// <returns>用户ID，如果未选中有效用户则返回空字符串</returns>
        private string GetSelectedUserId()
        {
            // 从下拉框获取选中的用户名
            if (comboBox_人员统计人名.SelectedItem != null)
            {
                string selectedUserName = comboBox_人员统计人名.SelectedItem.ToString();

                // 根据用户名查找用户ID
                try
                {
                    var userDataTable = SQLiteDataBase.GetDataFromMysql("qz_user", "real_name", selectedUserName);
                    if (userDataTable.Rows.Count > 0)
                    {
                        return userDataTable.Rows[0]["id"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"获取用户ID时出错: {ex.Message}");
                }
            }

            return string.Empty;
        }
               

        #endregion


        #region 新方法查询文件数据后显示到表格中

        /// <summary>
        /// 项目目录详情模型
        /// </summary>
        private class ProjectCatalogDetail
        {
            public string ProjectNo { get; set; }        // 项目编号
            public string ProjectName { get; set; }      // 项目名称
            public string ConstructionUnit { get; set; } // 建设单位
            public string ProjectManager { get; set; }   // 项目经理
            public string StageName { get; set; }        // 阶段名称
            public string MajorName { get; set; }        // 专业名称
            public string SubProjectName { get; set; }   // 子项名称
            public string FileName { get; set; }         // 文件名称
            public int FileCount { get; set; }           // 文件数量
            public string Folded { get; set; }           // 折合A1数
            public string FilePath { get; set; }         // 文件路径（用于内部处理）
        }

        /// <summary>
        /// 获取项目目录详情
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        /// <returns>项目目录详情列表</returns>
        private List<ProjectCatalogDetail> GetProjectDetailsForCatalog(string projectId, string startDateTime, string endDateTime)
        {
            var details = new List<ProjectCatalogDetail>();

            // 1. 获取项目基本信息
            var projectInfo = GetProjectInfoById(projectId);
            if (projectInfo == null)
            {
                throw new ArgumentException($"项目ID {projectId} 不存在");
            }
            var allProjectData = new DataTable();
            if (radioButton_项目文件区.Checked)
            {
                // 2. 获取该项目下的所有文件（包括各级子节点）
                allProjectData = SQLiteDataBase.GetDataFromMysql(
                   "qz_project",
                   "name",
                   "create_time",
                   projectId,
                   "pdf",
                   startDateTime,
                   endDateTime);
            }
            else if (radioButton_档案区.Checked)
            {
                // 2. 获取该项目下的所有文件（包括各级子节点）
                allProjectData = SQLiteDataBase.GetDataFromMysql(
                   "qz_keep_project",
                   "name",
                   "create_time",
                   projectId,
                   "pdf",
                   startDateTime,
                   endDateTime);
            }



            // 3. 遍历所有文件，构建详细信息
            foreach (DataRow row in allProjectData.Rows)
            {
                var fileData = new GetKeepProjectDirModel
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

                // 如果不是文件类型（type != 5），跳过
                if (fileData.type != 5) continue;

                // 解析ancestors字段，获取完整的层级信息
                var ancestorIds = ParseAncestors(fileData.ancestors);

                // 获取各层级节点信息
                var pathInfo = GetPathInfoFromAncestors(ancestorIds, fileData);

                // 创建详情记录
                var detail = new ProjectCatalogDetail
                {
                    ProjectNo = pathInfo.ProjectNo ?? "无",
                    ProjectName = pathInfo.ProjectName ?? "无",
                    ConstructionUnit = pathInfo.ConstructionUnit ?? "无",
                    ProjectManager = pathInfo.ProjectManager ?? "无",
                    StageName = pathInfo.StageName ?? "无",
                    MajorName = pathInfo.MajorName ?? "无",
                    SubProjectName = pathInfo.SubProjectName ?? "无",
                    FileName = fileData.name,
                    FileCount = 1, // 每个记录代表一个文件
                    Folded = fileData.folded,
                    FilePath = fileData.id
                };

                details.Add(detail);
            }

            return details;
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

            // 分割ancestors字符串得到ID列表，并去除空值
            var ids = ancestors.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(id => id.Trim())
                              .Where(id => !string.IsNullOrEmpty(id))
                              .ToList();

            return ids;
        }

        /// <summary>
        /// 根据ancestors获取路径信息
        /// </summary>
        /// <param name="ancestorIds">祖先ID列表</param>
        /// <param name="fileData">文件数据</param>
        /// <returns>路径信息</returns>
        private ProjectCatalogDetail GetPathInfoFromAncestors(List<string> ancestorIds, GetKeepProjectDirModel fileData)
        {
            var pathInfo = new ProjectCatalogDetail();

            if (ancestorIds.Count == 0) return pathInfo;

            // 从数据库中获取各层级节点信息
            // 根据您的层级结构：系统根 -> 项目 -> 阶段 -> 归档文件 -> 子项 -> 专业 -> 文件区 -> 文件类型 -> 文件
            // 通常是：[系统根ID, 项目ID, 阶段ID, 归档文件ID, 子项ID, 专业ID, 文件区ID, 文件类型ID]

            // 获取项目信息（通常是ancestors的第二个ID，第一个是系统根）
            if (ancestorIds.Count >= 2)
            {
                var projectInfo = GetProjectInfoById(ancestorIds[1]); // 第二个ID是项目ID
                if (projectInfo != null)
                {
                    pathInfo.ProjectNo = projectInfo.identifier ?? "无";
                    pathInfo.ProjectName = projectInfo.name ?? "无";
                    pathInfo.ConstructionUnit = projectInfo.unit ?? "无";
                    // 项目经理信息可能需要从其他表获取
                    pathInfo.ProjectManager = "暂无";
                }
            }

            // 获取阶段信息（通常是第三个ID）
            if (ancestorIds.Count >= 3)
            {
                var stageInfo = GetNodeInfoById(ancestorIds[2]);
                if (stageInfo != null)
                {
                    pathInfo.StageName = stageInfo.name ?? "无";
                }
            }

            // 获取子项信息（根据您的示例，子项可能在ancestors的第5个位置）
            if (ancestorIds.Count >= 5)
            {
                var subProjectInfo = GetNodeInfoById(ancestorIds[4]); // 第5个ID是子项ID
                if (subProjectInfo != null)
                {
                    pathInfo.SubProjectName = subProjectInfo.name ?? "无";
                }
            }

            // 获取专业信息（根据您的示例，专业可能在ancestors的第6个位置）
            if (ancestorIds.Count >= 6)
            {
                var majorInfo = GetNodeInfoById(ancestorIds[5]); // 第6个ID是专业ID
                if (majorInfo != null)
                {
                    pathInfo.MajorName = majorInfo.name;
                }
            }

            return pathInfo;
        }

        /// <summary>
        /// 根据ID获取项目信息
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns>项目信息</returns>
        private GetKeepProjectDirModel GetProjectInfoById(string projectId)
        {
            try
            {
                var dataTable = new DataTable();
                if (radioButton_项目文件区.Checked)
                {
                    dataTable = SQLiteDataBase.GetDataFromMysql("qz_project", "id", projectId);
                }
                else if (radioButton_档案区.Checked)
                {
                    dataTable = SQLiteDataBase.GetDataFromMysql("qz_keep_project", "id", projectId);
                }

                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    return new GetKeepProjectDirModel
                    {
                        id = row["id"].ToString(),
                        name = row["name"].ToString(),
                        identifier = row["identifier"]?.ToString(),
                        unit = row["unit"]?.ToString(),
                        type = Convert.ToInt32(row["type"]),
                        parentId = row["parent_id"].ToString(),
                        ancestors = row["ancestors"].ToString(),
                        createTime = row["create_time"].ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取项目信息时出错: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// 根据ID获取节点信息（通用方法）
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        /// <returns>节点信息</returns>
        private GetKeepProjectDirModel GetNodeInfoById(string nodeId)
        {
            try
            {
                var dataTable = new DataTable();
                if (radioButton_项目文件区.Checked)
                {
                    dataTable = SQLiteDataBase.GetDataFromMysql("qz_project", "id", nodeId);
                }
                else if (radioButton_档案区.Checked)
                {
                    dataTable = SQLiteDataBase.GetDataFromMysql("qz_keep_project", "id", nodeId);
                }
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
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取节点信息时出错: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// 更新汇总标签
        /// </summary>
        /// <param name="details">项目目录详情列表</param>
        private void UpdateSummaryLabels(List<ProjectCatalogDetail> details)
        {
            int totalFileCount = details.Sum(d => d.FileCount);
            decimal totalFolded = details.Sum(d =>
            {
                decimal.TryParse(d.Folded, out decimal result);
                return result;
            });

            label_合计.Text = totalFileCount.ToString();
            label_合计A1.Text = totalFolded.ToString("F2"); // 保留两位小数
            label_总出版A1.Visible = false; // 根据需要显示或隐藏
        }

        /// <summary>
        /// 修改后的项目目录查询方法
        /// </summary>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        private void HandleProjectCatalogQuery(string startDateTime, string endDateTime)
        {
            try
            {
                // 1. 获取选中节点的项目ID
                string projectId = GetSelectedProjectId();
                if (string.IsNullOrEmpty(projectId))
                {
                    MessageBox.Show("请先在左侧树形结构中选择一个项目！");
                    return;
                }

                // 2. 填充项目目录数据到DataGridView
                PopulateProjectCatalogDataGrid(projectId, startDateTime, endDateTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询项目目录时发生错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 获取选中节点的项目ID
        /// </summary>
        /// <returns>项目ID，如果未选中有效项目则返回空字符串</returns>
        private string GetSelectedProjectId()
        {
            if (treeView_Archive.SelectedNode == null)
            {
                return string.Empty;
            }

            // 根据不同的节点类型获取项目ID
            var nodeTag = treeView_Archive.SelectedNode.Tag;

            if (nodeTag is ProjectFileTreeViewModel projectNode)
            {
                // 如果是项目文件树节点
                if (projectNode.proType == 0) // 0表示项目
                {
                    return projectNode.id;
                }
                // 如果是项目下的节点，尝试获取项目ID
                else if (!string.IsNullOrEmpty(projectNode.projectId))
                {
                    return projectNode.projectId;
                }
            }
            else if (nodeTag is GetKeepProjectDirModel keepProjectNode)
            {
                // 如果是归档项目节点
                if (keepProjectNode.type == 0) // 0表示项目
                {
                    return keepProjectNode.id;
                }
                // 如果是项目下的节点，返回关联的项目ID
                else if (!string.IsNullOrEmpty(keepProjectNode.projectId))
                {
                    return keepProjectNode.projectId;
                }
            }
            else if (nodeTag is SelectKeepDeptModel deptNode)
            {
                // 如果是部门节点，直接返回ID（假设部门ID就是项目ID）
                return deptNode.id;
            }

            return string.Empty;
        }

        /// <summary>
        /// 收集需要显示在目录中的节点（通常是子项级别及以下的节点）
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <param name="collectedNodes">收集到的节点列表</param>
        /// <returns>收集到的节点列表</returns>
        private List<ProjectHierarchyService.ProjectHierarchyNode> CollectDisplayNodes(
            ProjectHierarchyService.ProjectHierarchyNode node,
            List<ProjectHierarchyService.ProjectHierarchyNode> collectedNodes)
        {
            // 如果当前节点是子项级别（type=3）或更低级别（文件夹/文件），则添加到结果中
            if (node.Type >= 3 && node.Type <= 5) // 3子项 4文件夹 5文件
            {
                collectedNodes.Add(node);
            }

            // 递归处理子节点
            foreach (var childNode in node.Children)
            {
                CollectDisplayNodes(childNode, collectedNodes);
            }

            return collectedNodes;
        }

        /// <summary>
        /// 获取节点的文件数量
        /// </summary>
        /// <param name="node">项目层级节点</param>
        /// <returns>文件数量</returns>
        private int GetFileCount(ProjectHierarchyService.ProjectHierarchyNode node)
        {
            // 如果是文件节点(type=5)，返回1
            if (node.Type == 5)
            {
                return 1;
            }
            // 如果是子项或文件夹节点，递归计算子节点中的文件数量
            else if (node.Type == 3 || node.Type == 4)
            {
                int count = 0;
                foreach (var child in node.Children)
                {
                    if (child.Type == 5) // 文件
                    {
                        count++;
                    }
                    else
                    {
                        count += GetFileCount(child);
                    }
                }
                return count;
            }
            return 0;
        }

        /// <summary>
        /// 获取节点的A1数量
        /// </summary>
        /// <param name="node">项目层级节点</param>
        /// <returns>A1数量</returns>
        private double GetA1Count(ProjectHierarchyService.ProjectHierarchyNode node)
        {
            // 如果是文件节点，从原始数据获取A1数量
            if (node.Type == 5 && node.OriginalData != null)
            {
                double.TryParse(node.OriginalData.folded, out double result);
                return result;
            }
            // 如果是子项或文件夹节点，累加子节点的A1数量
            else if (node.Type == 3 || node.Type == 4)
            {
                double sum = 0;
                foreach (var child in node.Children)
                {
                    sum += GetA1Count(child);
                }
                return sum;
            }
            return 0;
        }

        /// <summary>
        /// 单元格合并实用工具类
        /// </summary>
        private class CellMerger
        {
            private DataGridView dataGridView;
            private List<int> mergeColumns;

            public CellMerger(DataGridView dataGridView, List<int> mergeColumns)
            {
                this.dataGridView = dataGridView;
                this.mergeColumns = mergeColumns;
            }

            /// <summary>
            /// 合并单元格
            /// </summary>
            public void MergeCells()
            {
                if (dataGridView.Rows.Count == 0) return;

                // 为需要合并的列计算合并信息
                foreach (int colIndex in mergeColumns)
                {
                    CalculateMergeInfo(colIndex);
                }

                // 添加绘制事件
                dataGridView.CellPainting += DataGridView_CellPainting;
            }

            /// <summary>
            /// 计算合并信息
            /// </summary>
            private void CalculateMergeInfo(int colIndex)
            {
                if (dataGridView.Rows.Count == 0) return;

                // 为每个单元格标记是否应该隐藏
                for (int rowIndex = 0; rowIndex < dataGridView.Rows.Count; rowIndex++)
                {
                    dataGridView[colIndex, rowIndex].Tag = null; // 重置标记
                }

                for (int rowIndex = 0; rowIndex < dataGridView.Rows.Count - 1; rowIndex++)
                {
                    var currentCell = dataGridView[colIndex, rowIndex];
                    var nextCell = dataGridView[colIndex, rowIndex + 1];

                    var currentValue = currentCell.Value?.ToString();
                    var nextValue = nextCell.Value?.ToString();

                    if (currentValue == nextValue && !string.IsNullOrEmpty(currentValue))
                    {
                        // 标记下一个单元格为隐藏状态
                        nextCell.Tag = "hidden";
                    }
                }
            }

            /// <summary>
            /// 单元格绘制事件
            /// </summary>
            private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                var cell = dataGridView[e.ColumnIndex, e.RowIndex];

                // 如果单元格被标记为隐藏，则不绘制
                if (cell.Tag != null && cell.Tag.ToString() == "hidden")
                {
                    e.Handled = true;
                    return;
                }

                // 检查是否需要绘制合并单元格
                if (ShouldDrawMergedCell(e.ColumnIndex, e.RowIndex))
                {
                    DrawMergedCell(e);
                    e.Handled = true;
                }
                else
                {
                    // 正常绘制单元格
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                    // 绘制边框
                    using (Pen pen = new Pen(dataGridView.GridColor))
                    {
                        e.Graphics.DrawRectangle(pen, e.CellBounds);
                    }
                }
            }

            /// <summary>
            /// 检查是否需要绘制合并单元格
            /// </summary>
            private bool ShouldDrawMergedCell(int colIndex, int rowIndex)
            {
                if (rowIndex >= dataGridView.Rows.Count) return false;

                var currentValue = dataGridView[colIndex, rowIndex].Value?.ToString();
                if (string.IsNullOrEmpty(currentValue)) return false;

                // 检查是否与下一行的值相同
                if (rowIndex < dataGridView.Rows.Count - 1)
                {
                    var nextValue = dataGridView[colIndex, rowIndex + 1].Value?.ToString();
                    return currentValue == nextValue;
                }

                return false;
            }

            /// <summary>
            /// 绘制合并单元格
            /// </summary>
            private void DrawMergedCell(DataGridViewCellPaintingEventArgs e)
            {
                var cell = dataGridView[e.ColumnIndex, e.RowIndex];
                var currentValue = cell.Value?.ToString();

                // 计算合并的行数
                int span = 1;
                for (int i = e.RowIndex + 1; i < dataGridView.Rows.Count; i++)
                {
                    var nextValue = dataGridView[e.ColumnIndex, i].Value?.ToString();
                    if (nextValue == currentValue)
                    {
                        span++;
                    }
                    else
                    {
                        break;
                    }
                }

                // 创建合并矩形
                System.Drawing.Rectangle mergedRect = new System.Drawing.Rectangle(
                    e.CellBounds.X,
                    e.CellBounds.Y,
                    e.CellBounds.Width - 1, // 减1避免重叠
                    e.CellBounds.Height * span - 1 // 减1避免重叠
                );

                // 绘制背景
                using (SolidBrush bgBrush = new SolidBrush(cell.Style.BackColor))
                {
                    e.Graphics.FillRectangle(bgBrush, mergedRect);
                }

                // 绘制文本
                TextRenderer.DrawText(
                    e.Graphics,
                    currentValue,
                    cell.InheritedStyle.Font,
                    mergedRect,
                    cell.InheritedStyle.ForeColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.WordBreak
                );

                // 绘制边框
                using (Pen borderPen = new Pen(dataGridView.GridColor))
                {
                    e.Graphics.DrawRectangle(borderPen, mergedRect);
                }
            }
        }

        /// <summary>
        /// 存储合并信息的字段
        /// </summary>
        private Dictionary<string, int> mergeSpans = new Dictionary<string, int>();

        /// <summary>
        /// 在dataGridView_目录中填充项目层级数据（带排序和单元格合并）
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        private void PopulateProjectCatalogDataGrid(string projectId, string startDateTime, string endDateTime)
        {
            try
            {
                // 移除之前的事件处理器（如果存在）
                dataGridView_目录.CellPainting -= DataGridView_CellPainting;

                // 1. 获取项目目录详情
                var projectDetails = GetProjectDetailsForCatalog(projectId, startDateTime, endDateTime);

                // 2. 按子项/工序排序，然后在每个子项/工序内按专业排序
                var sortedDetails = projectDetails
                    .OrderBy(detail => detail.SubProjectName ?? "")  // 首先按子项/工序排序
                    .ThenBy(detail => detail.MajorName ?? "")       // 然后在子项/工序内按专业排序
                    .ThenBy(detail => detail.FileName ?? "")        // 最后按文件名排序（保持同一专业内的文件有序）
                    .ToList();

                // 3. 清空现有数据
                dataGridView_目录.Rows.Clear();

                // 4. 遍历排序后的详情，填充到DataGridView（包括序号）
                for (int i = 0; i < sortedDetails.Count; i++)
                {
                    var detail = sortedDetails[i];

                    // 添加值数组到DataGridView（9列：序号 + 8个数据列）
                    dataGridView_目录.Rows.Add(
                   (i + 1).ToString("D2"),   // 序号列（01开始）
                   detail.ProjectNo,          // 项目编号
                   detail.ProjectName,        // 项目名称
                   detail.ConstructionUnit ?? "无", // 建设单位（如果为空则显示"无"）
                   detail.ProjectManager ?? "无",   // 项目经理（如果为空则显示"无"）
                   detail.StageName,          // 阶段
                   detail.SubProjectName,     // 子项
                   detail.MajorName,          // 专业
                   detail.FileName,           // 文件名称
                   detail.FileCount,          // 文件数量
                   detail.Folded              // 折合A1数
                           );
                 }

                // 5. 设置序号列的标题和样式
                if (dataGridView_目录.Columns.Count > 0)
                {
                    dataGridView_目录.Columns[0].HeaderText = "序号";
                    dataGridView_目录.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView_目录.Columns[0].Width = 50; // 设置合适的宽度
                }

                // 6. 设置DataGridView样式
                dataGridView_目录.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
                dataGridView_目录.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                // 7. 显示总计信息
                UpdateSummaryLabels(sortedDetails);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"填充项目目录数据时发生错误：{ex.Message}");
            }
        }
        /// <summary>
        /// DataGridView行数改变时重新设置序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_目录_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // 重新设置序号列的值
            for (int i = 0; i < dataGridView_目录.Rows.Count; i++)
            {
                if (dataGridView_目录.Rows[i].IsNewRow) continue;

                dataGridView_目录.Rows[i].Cells[0].Value = (i + 1).ToString("D2");
            }
        }

        /// <summary>
        /// DataGridView行数改变时重新设置序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_目录_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // 重新设置序号列的值
            for (int i = 0; i < dataGridView_目录.Rows.Count; i++)
            {
                if (dataGridView_目录.Rows[i].IsNewRow) continue;

                dataGridView_目录.Rows[i].Cells[0].Value = (i + 1).ToString("D2");
            }
        }

        /// <summary>
        /// 单元格绘制事件
        /// </summary>
        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // 检查当前单元格是否是被合并隐藏的单元格
            string hideKey = $"{e.ColumnIndex}_{e.RowIndex}";
            if (mergeSpans.ContainsKey(hideKey) && mergeSpans[hideKey] == -1)
            {
                // 不绘制这个单元格，只绘制背景
                e.PaintBackground(e.ClipBounds, true);
                e.Handled = true;
                return;
            }

            // 检查当前单元格是否是合并的起始单元格
            string key = $"{e.ColumnIndex}_{e.RowIndex}";
            if (mergeSpans.ContainsKey(key))
            {
                int span = mergeSpans[key];

                // 获取合并矩形
                var cellRect = dataGridView_目录.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                var mergedRect = new System.Drawing.Rectangle(
                    cellRect.X,
                    cellRect.Y,
                    cellRect.Width,
                    cellRect.Height * span
                );

                // 绘制背景
                using (SolidBrush bgBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    e.Graphics.FillRectangle(bgBrush, mergedRect);
                }

                // 绘制边框
                using (Pen borderPen = new Pen(dataGridView_目录.GridColor))
                {
                    e.Graphics.DrawRectangle(borderPen, mergedRect);
                }

                // 绘制文本
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.FormatFlags = StringFormatFlags.NoWrap;

                    using (Brush textBrush = new SolidBrush(e.CellStyle.ForeColor))
                    {
                        e.Graphics.DrawString(
                            e.Value?.ToString() ?? "",
                            e.CellStyle.Font,
                            textBrush,
                            mergedRect,
                            sf
                        );
                    }
                }

                e.Handled = true;
            }
            else
            {
                // 正常绘制单元格
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
            }
        }
        #endregion

        /// <summary>
        /// 存储项目列表  
        /// </summary>
        private static List<List<ProjectPropertyModel>> statisticsProjectListFileTemp = new List<List<ProjectPropertyModel>>();

        /// <summary>
        /// 存储项目列表  
        /// </summary>
        private static string comboboxBefselect = null;

        /// <summary>
        /// 统计
        /// </summary>
        private void LoadMysql_Apply_Project_UserList(string startDateTime, string endDateTime)
        {
            try
            {
                Splasher.Show(typeof(FrmLoading));

                if (dataGridView_流程统计.CanSelect)
                {
                    dataGridView_流程统计.Rows.Clear();
                    label_合计A1.Text = "0";
                    label_合计.Text = "0";
                    if (comboBox_流程类型.Text == "00全部流程" || comboBox_流程类型.SelectedItem.ToString() == "00全部流程")
                    {
                        var dialogResult = MessageBox.Show("请选择流程类型！", "流程类型选择：", MessageBoxButtons.OK);

                        if (dialogResult == DialogResult.OK) return;
                    }
                    else
                    {
                        //流程类型变量
                        var approvalItemStr = new ApplyItemDataList();
                        //流程类型list
                        var approvalListStr = new List<ApplyItemDataList>();
                        foreach (DataRow qz_approvalRowItem in qz_approvalList.Rows)
                        {
                            approvalItemStr.applyId = qz_approvalRowItem["id"].ToString();
                            approvalItemStr.applyName = qz_approvalRowItem["Name"].ToString();
                            //存入流程类型
                            approvalListStr.Add(approvalItemStr);
                            approvalItemStr = new ApplyItemDataList();
                        }
                        //如果是第一次加载则创建本地文件，并读取服务器写入本地文件
                        if (applyStatisticsListTemp.Count == 0 || comboboxBefselect != comboBox_流程类型.SelectedItem.ToString() || Convert.ToDateTime(applyStatisticsListTemp[applyStatisticsListTemp.Count - 1].createTime).Date > dateTimePicker_开始.Value.Date)
                        {
                            SystemTempData.CreateEmptyApplyStatisticsListJsonFile();
                            SystemTempData.CreateEmptyApplyInfoJsonFile();

                            comboboxBefselect = comboBox_流程类型.SelectedItem.ToString();
                            var applyIdIndex = approvalListStr.FindIndex(approval => approval.applyName == comboBox_流程类型.SelectedItem.ToString());

                            //调取服务器数据写入本地文件
                            SearchMysqlApplyList($"{approvalListStr[applyIdIndex].applyId}");
                            //SQLiteDataBase.SearchTableFromSQLite($"{approvalListStr[applyIdIndex].applyId}");
                            applyStatisticsListTemp.Clear();
                            //调用读取本地文件方法赋值这个变量；5
                            SystemTempData.LoadApplyStatisticsListFromJson(ref applyStatisticsListTemp);

                            foreach (var applyInfoItem in applyStatisticsListTemp)
                            {
                                SystemTempData.Read_Mysql_ApplyStatisticsInfoHttpDatas(applyInfoItem.id);
                            }
                            //再次加载本地流程信息
                            SystemTempData.LoadApplyInfoDataFromJson(ref applyInfoTempFile);
                        }

                        ///循环拿到的流程列表每个审批流程
                        ResultDataItemList.Clear();
                        foreach (var applyInfoTempFileItem in applyInfoTempFile)
                        {
                            if (applyInfoTempFileItem.id != null && Convert.ToDateTime(applyInfoTempFileItem.createTime).Date >= dateTimePicker_开始.Value.Date && Convert.ToDateTime(applyInfoTempFileItem.createTime).Date <= dateTimePicker_截至.Value.Date)
                            {
                                ResultDataItemList.Add(applyInfoTempFileItem);
                            }
                        }
                        ///把查询的数据绑定到流程统计表里  result
                        dataGridView_流程统计.DataSource = new SortableBindingList<ApplyInfoModel>(ResultDataItemList);
                        //写入文件数量
                        fileNumber(ResultDataItemList);
                        ///把项目、流程、人员添加到下拉菜单内
                        ComboboxApplyData(ResultDataItemList);
                    }
                }
                else if (dataGridView_项目统计.CanSelect)
                {
                    #region new
                    //创建空项目统计列表
                    CreateEmptyStatisticsProjectListJsonFile();
                    //清理项目列表内数据
                    ProjectPropertieListS.Clear();
                    //清理所有行
                    dataGridView_项目统计.Rows.Clear();
                    //清理本地文件项目列表缓存文件
                    statisticsProjectListFileTemp.Clear();

                    int fileNumber = 0;//文件数量
                    double folded = 0;//A1数量
                    string projectName = comboBox_项目统计项目名.Text;//项目名称

                    if (projectName == "" || projectName == "00全部项目")//如果没有选择项目
                    {
                        Read_Project_Attribute_Info_Http_Mysql_Datas("qz_project", "create_time", startDateTime, endDateTime, ref fileNumber, ref folded);
                        //读取本地项目列表文件
                        SystemTempData.LoadStatisticsProjectInfoPropertyListDataFromJson(ref statisticsProjectListFileTemp);
                        ///绑定数据 5
                        dataGridView_项目统计.DoubleBufferedDataGirdView(true);

                        dataGridView_项目统计.AutoGenerateColumns = false;
                        //绑定数据绑定数据到表格
                        BindDataToGridView(statisticsProjectListFileTemp);
                        ///项目名称添加到下拉菜单内把项目、人员添加到下拉菜单内
                        ComboboxProjectData(statisticsProjectListFileTemp);
                    }
                    else
                    {
                        //读取项目列表选择了项目名称
                        Read_One_Project_Attribute_Info_Http_Mysql_Datas("qz_project", "create_time", startDateTime, endDateTime, projectName, ref fileNumber, ref folded);
                        //读取本地项目列表文件
                        SystemTempData.LoadStatisticsProjectInfoPropertyListDataFromJson(ref statisticsProjectListFileTemp);
                        //绑定数据绑定数据到表格
                        dataGridView_项目统计.DoubleBufferedDataGirdView(true);

                        dataGridView_项目统计.AutoGenerateColumns = false;
                        //绑定数据
                        BindDataToGridView_User_Project(statisticsProjectListFileTemp);

                        ComboboxProjectData(statisticsProjectListFileTemp);
                    }
                    #endregion

                    label_合计.Text = fileNumber.ToString();
                    label_合计A1.Text = folded.ToString();
                    label_总出版A1.Visible = false;

                    //fileNumber(ProjectPropertieListS);
                }
                else if (dataGridView_人员统计.CanSelect)
                {
                    // 当切换到人员统计标签页时，可能需要加载人员列表
                    try
                    {
                        // 清空现有选项
                        comboBox_人员统计人名.Items.Clear();
                        comboBox_人员统计人名.Items.Add("00全部人员");

                        // 从数据库获取所有用户
                        var userDataTable = SQLiteDataBase.GetDataFromMysql("qz_user", "status", "0"); // 假设status为0表示激活用户

                        foreach (DataRow row in userDataTable.Rows)
                        {
                            string userName = row["real_name"]?.ToString() ?? row["name"]?.ToString();
                            if (!string.IsNullOrEmpty(userName) && !comboBox_人员统计人名.Items.Contains(userName))
                            {
                                comboBox_人员统计人名.Items.Add(userName);
                            }
                        }
                        //把人员添加到下拉菜单内
                        HandlePersonnelStatisticsQuery(startDateTime, endDateTime);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"加载人员列表时出错: {ex.Message}");
                    }

                    #region 老方法人员统计


                    //if (comboBox_人员统计人名.SelectedItem != null)
                    //{
                    //    comboBoxUser = comboBox_人员统计人名.SelectedItem.ToString();
                    //}
                    //if (comboBoxUser == "00全部人员")
                    //{
                    //    MessageBox.Show("请在提交用户下拉菜单中选择要查询的用户！");
                    //    return;
                    //}
                    //userDataLists = new List<ProjectUserDataList>();
                    //userDataTemp = new ProjectUserDataList();
                    //#region 拿到人员列表
                    ////清理临时变量；
                    //statisticsUserInfoListTemp = new List<projectDeptModel>();
                    //projectUserInfoList = new List<projectDeptModel>();

                    //if (deptUserListTemp.Count == 0)
                    //{
                    //    //读取部门用户列表
                    //    LoadDetpUserListFromJson(ref deptUserListTemp);
                    //}
                    ////用户Id
                    //var userId = deptUserListTemp[deptUserListTemp.FindIndex(o => o.realName == comboBoxUser)].id;
                    //int fileNumberS = 0;//文件数量
                    //double folderS = 0;//折A1

                    //// 自定义格式字符串
                    ////string format = "yyyy-M-d HH:mm:ss";
                    //string stattTimeformat = dateTimePicker_开始.Value.ToString("yyyy-MM-dd 00:00:00");
                    //string endTimeFormat = dateTimePicker_截至.Value.ToString("yyyy-MM-dd 23:59:59");
                    ////读本地的MySql数据库
                    //Read_Mysql_ProjectUserAllInfoList(userId, stattTimeformat, endTimeFormat, ref fileNumberS, ref folderS);
                    ////读取本地项目列表文件
                    //LoadProjectUserInfoListDataFromJson(ref statisticsUserInfoListTemp);

                    ////循环项目
                    //foreach (var deptItem in statisticsUserInfoListTemp)
                    //{
                    //    foreach (var projectItem in deptItem.projectInfoList)
                    //    {
                    //        foreach (var stageItem in projectItem.projectStageList)//循环项目中的阶段
                    //        {
                    //            foreach (var majroItem in stageItem.projectMajroList)//循环阶段中的专业
                    //            {
                    //                foreach (var subProjectItem in majroItem.subProjectList)//循环专业中的子项
                    //                {
                    //                    foreach (var roleItem in subProjectItem.projectRoleList)//循环子项中的角色
                    //                    {
                    //                        //roleItem.projectUserListModel[0].projectUserName;
                    //                        if (roleItem.projectRoleName == "设计人")
                    //                        {
                    //                            userDataTemp.projectNo = projectItem.projectNo;
                    //                            userDataTemp.projectName = projectItem.projectName;
                    //                            userDataTemp.stageName = stageItem.projectStageName;
                    //                            userDataTemp.majroName = majroItem.projectMajroName;
                    //                            userDataTemp.subProjectName = subProjectItem.subProjectName;
                    //                            userDataTemp.roleName = roleItem.projectRoleName;
                    //                            userDataTemp.userName = roleItem.projectUserListModel[0].projectUserName;
                    //                            userDataTemp.fileNumber = roleItem.projectUserListModel[0].subProjectFileNumberS[0].fileNumber;
                    //                            userDataTemp.A1SizeNumber = roleItem.projectUserListModel[0].subProjectFileNumberS[0].A1SizeNumber;
                    //                            userDataLists.Add(userDataTemp);
                    //                            userDataTemp = new ProjectUserDataList();
                    //                        }
                    //                        else
                    //                        {
                    //                            userDataTemp.projectNo = projectItem.projectNo;
                    //                            userDataTemp.projectName = projectItem.projectName;
                    //                            userDataTemp.stageName = stageItem.projectStageName;
                    //                            userDataTemp.majroName = majroItem.projectMajroName;
                    //                            userDataTemp.subProjectName = subProjectItem.subProjectName;
                    //                            userDataTemp.roleName = roleItem.projectRoleName;
                    //                            userDataTemp.userName = roleItem.projectUserListModel[0].projectUserName;
                    //                            userDataTemp.fileNumber = 0;
                    //                            userDataTemp.A1SizeNumber = 0;
                    //                            userDataLists.Add(userDataTemp);
                    //                            userDataTemp = new ProjectUserDataList();
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    //#endregion
                    /////把查询的数据绑定到流程统计表里 
                    //dataGridView_人员统计.Rows.Clear();
                    //if (userDataLists.Count != 0)
                    //{
                    //    dataGridView_人员统计.DataSource = new SortableBindingList<ProjectUserDataList>(userDataLists);
                    //    dataGridView_人员统计.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    //}
                    //ComboboxUserData(userDataLists);

                    //label_合计.Text = fileNumberS.ToString();
                    //label_合计A1.Text = folderS.ToString();
                    //label_总出版A1.Visible = false;
                    #endregion
                }
                else if (dataGridView_目录.CanSelect)
                {
                    //创建空项目统计列表
                    CreateEmptyStatisticsProjectListJsonFile();
                    //清理项目列表内数据
                    ProjectPropertieListS.Clear();
                    //清理所有行
                    dataGridView_目录.Rows.Clear();
                    //清理本地文件项目列表缓存文件
                    statisticsProjectListFileTemp.Clear();

                    // 自定义格式字符串
                    int fileNumber = 0;//文件数量
                    double folded = 0;//A1数量
                    try
                    {
                        string projectName = null;
                        string projectId = null;

                        if (radioButton_档案区.Checked)
                        {

                            //string tableName, string columName, string startTime, string endTime, string projectName, string projectId, ref int fileNumber, ref double folded
                            //读取项目属性信息
                            Read_Project_Attribute_Info_Http_Mysql_Datas("qz_keep_project", "create_time", startDateTime, endDateTime, $"{projectName}", $"{projectId}", ref fileNumber, ref folded);
                            //读取本地项目列表文件
                            SystemTempData.LoadStatisticsProjectInfoPropertyListDataFromJson(ref statisticsProjectListFileTemp);
                            //绑定数据
                            dataGridView_目录.DoubleBufferedDataGirdView(true);

                            dataGridView_目录.AutoGenerateColumns = false;
                            //绑定数据绑定数据到表格
                            BindDataToGridView_ProjectCatalog(statisticsProjectListFileTemp);
                            ///项目名称添加到下拉菜单内把项目、人员添加到下拉菜单内
                            ComboboxProjectData(statisticsProjectListFileTemp);
                        }
                        else if (radioButton_项目文件区.Checked)
                        {
                            //读取项目属性信息
                            Read_Project_Attribute_Info_Http_Mysql_Datas("qz_project", "create_time", startDateTime, endDateTime, $"{projectName}", $"{projectId}", ref fileNumber, ref folded);
                            //读取本地项目列表文件
                            SystemTempData.LoadStatisticsProjectInfoPropertyListDataFromJson(ref statisticsProjectListFileTemp);
                            //绑定数据
                            dataGridView_目录.DoubleBufferedDataGirdView(true);

                            dataGridView_目录.AutoGenerateColumns = false;
                            //绑定数据绑定数据到表格
                            BindDataToGridView_ProjectCatalog(statisticsProjectListFileTemp);
                            ///项目名称添加到下拉菜单内把项目、人员添加到下拉菜单内
                            ComboboxProjectData(statisticsProjectListFileTemp);
                        }

                    }
                    catch
                    {

                    }



                    label_合计.Text = fileNumber.ToString();
                    label_合计A1.Text = folded.ToString();
                    label_总出版A1.Visible = false;
                }
                Splasher.Close();

            }
            catch
            {
                Splasher.Close();
            }

        }

        /// <summary>
        /// 绑定人员中的：人员的信息到下拉菜单内
        /// </summary>
        /// 
        private void ComboboxUserData(List<ProjectUserDataList> ProjectPropertieListS)
        {
            if (ProjectPropertieListS.Count != 0)
            {
                comboBox_人员统计项目.Items.Clear();
                comboBox_人员统计项目.Items.Add("00全部项目");
                foreach (var projectPropertieItem in ProjectPropertieListS)
                {
                    string projectName = projectPropertieItem.projectName;
                    if (!comboBox_人员统计项目.Items.Contains(projectName))//判断在combobox集合中，是不是有了这个projectName的字符串
                        comboBox_人员统计项目.Items.Add(projectName);
                }
                // 进行排序
                List<string> comboBox项目名称 = new List<string>(comboBox_人员统计项目.Items.Cast<string>());
                comboBox项目名称.Sort();
                // 重新设置排序后的数据
                comboBox_人员统计项目.Items.Clear();
                comboBox_人员统计项目.Items.AddRange(comboBox项目名称.ToArray());
            }
        }

        /// <summary>
        /// 绑定项目中的、人员的信息到下拉菜单内
        /// </summary>
        private void ComboboxProjectData(List<List<ProjectPropertyModel>> ProjectPropertieListS)
        {
            if (ProjectPropertieListS.Count != 0)
            {
                comboBox_项目统计项目名.Items.Clear();
                comboBox_项目统计项目名.Items.Add("00全部项目");
                //comboBox_项目统计项目名.SelectedIndex = 0;
                comboBox_项目人员.Items.Clear();
                comboBox_项目人员.Items.Add("00全部人员");
                //comboBox_项目人员.SelectedIndex = 0;
                foreach (var projectPropertieItem in ProjectPropertieListS)
                {
                    string projectName = projectPropertieItem[2].Value;
                    if (!comboBox_项目统计项目名.Items.Contains(projectName))//判断在combobox集合中，是不是有了这个projectName的字符串
                    {
                        comboBox_项目统计项目名.Items.Add(projectName);
                    }

                    string userName = projectPropertieItem[5].Value;
                    if (!comboBox_项目人员.Items.Contains(userName))//判断在combobox集合中，是不是有了这个userNam的字符串
                        comboBox_项目人员.Items.Add(userName);
                }
                // 进行排序
                List<string> comboBox项目统计项目名 = new List<string>(comboBox_项目统计项目名.Items.Cast<string>());
                comboBox项目统计项目名.Sort();
                // 重新设置排序后的数据
                comboBox_项目统计项目名.Items.Clear();
                comboBox_项目统计项目名.Items.AddRange(comboBox项目统计项目名.ToArray());
                // 进行排序
                List<string> comboBox项目人员 = new List<string>(comboBox_项目人员.Items.Cast<string>());
                comboBox项目人员.Sort();
                // 重新设置排序后的数据
                comboBox_项目人员.Items.Clear();
                comboBox_项目人员.Items.AddRange(comboBox项目人员.ToArray());
                // 进行排序

            }
        }

        /// <summary>
        /// 绑定流程的：项目、人员的信息到下拉菜单内
        /// </summary>
        private void ComboboxApplyData(List<ApplyInfoModel> ProjectPropertieListS)
        {
            if (ProjectPropertieListS.Count != 0)
            {
                comboBox_流程统计项目名称.Items.Clear();
                comboBox_流程统计项目名称.Items.Add("00全部项目");
                comboBox_流程统计项目名称.SelectedIndex = 0;
                comboBox_流程类型.Items.Clear();
                comboBox_流程类型.Items.Add("00全部流程");
                //流程类型加入下拉菜单
                ComboboxSelect();
                comboBox_流程类型.SelectedIndex = 0;
                comboBox_流程人员.Items.Clear();
                comboBox_流程人员.Items.Add("00全部人员");
                comboBox_流程人员.SelectedIndex = 0;

                foreach (var projectPropertieItem in ProjectPropertieListS)
                {
                    string projectName = projectPropertieItem.proName;
                    if (!comboBox_流程统计项目名称.Items.Contains(projectName))//判断在combobox集合中，是不是有了这个projectName的字符串
                        comboBox_流程统计项目名称.Items.Add(projectName);

                    //string appName = projectPropertieItem.appName;
                    //if (!comboBox_流程类型.Items.Contains(appName))//判断在combobox集合中，是不是有了这个processTypeId的字符串
                    //    comboBox_流程类型.Items.Add(appName);

                    string userName = projectPropertieItem.userName;

                    if (userName != null && !comboBox_流程人员.Items.Contains(userName))//判断在combobox集合中，是不是有了这个userNam的字符串
                        comboBox_流程人员.Items.Add(userName);
                }
                // 进行排序
                List<string> comboBox项目名称 = new List<string>(comboBox_流程统计项目名称.Items.Cast<string>());
                comboBox项目名称.Sort();
                // 重新设置排序后的数据
                comboBox_流程统计项目名称.Items.Clear();
                comboBox_流程统计项目名称.Items.AddRange(comboBox项目名称.ToArray());

                //// 进行排序
                //List<string> comboBox流程类型 = new List<string>(comboBox_流程类型.Items.Cast<string>());
                //comboBox流程类型.Sort();
                //// 重新设置排序后的数据
                //comboBox_流程类型.Items.Clear();
                //comboBox_流程类型.Items.AddRange(comboBox流程类型.ToArray());

                // 进行排序
                List<string> comboBox提交用户 = new List<string>(comboBox_流程人员.Items.Cast<string>());
                comboBox提交用户.Sort();
                // 重新设置排序后的数据
                comboBox_流程人员.Items.Clear();
                comboBox_流程人员.Items.AddRange(comboBox提交用户.ToArray());
            }
        }

        /// <summary> 
        /// 获取指定文件夹下所有子目录及文件函数 
        /// </summary> 
        /// <param name="theDir">指定目录</param> 
        /// <param name="nLevel">默认起始值,调用时,一般为0</param> 
        /// <returns></returns> 
        public void DirectoryInfo(DirectoryInfo theDir, string nLevel, TreeNode node)//递归目录 文件 
        {
            var dirModel = new DirectoryStructureModel();
            dirModel.ParentId = nLevel;
            dirModel.PrimaryKey = Guid.NewGuid().ToString();
            dirModel.Name = theDir.Name.ToString();
            dirModel.Type = 1;
            DirectoryStructureList.Add(dirModel);

            FileInfo[] fileInfo = theDir.GetFiles(); //目录下的文件 
            foreach (FileInfo fInfo in fileInfo)
            {
                var fileModel = new DirectoryStructureModel();
                fileModel.ParentId = dirModel.PrimaryKey;
                fileModel.Name = fInfo.FullName;
                fileModel.Type = 2;
                DirectoryStructureList.Add(fileModel);
            }

            DirectoryInfo[] subDirectories = theDir.GetDirectories();//获得目录 
            foreach (DirectoryInfo dirinfo in subDirectories)
            {
                DirectoryInfo(dirinfo, dirModel.PrimaryKey, node);
            }
        }

        /// <summary>
        /// 全部项目统计绑定数据
        /// </summary>
        /// <param name="ProjectPropertyListS"></param>
        private void BindDataToGridView(List<List<ProjectPropertyModel>> ProjectPropertyListS)
        {
            dataGridView_项目统计.DataSource = null;
            // 清空现有的行
            dataGridView_项目统计.Rows.Clear();
            // 逐个项目填充行
            foreach (var projectPropertyListItem in ProjectPropertyListS)
            {
                //行变量
                var row = new DataGridViewRow();

                // 填充前12列固定的属性值
                foreach (var property in projectPropertyListItem)
                {
                    int columnIndex;
                    // 检查前12个属性（使用Id来作索引）
                    if (int.TryParse(property.No, out columnIndex) && columnIndex >= 0 && columnIndex <= 14)
                    {
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = property.Value });
                    }
                    else
                    {
                        //// 处理自定义属性，动态添加到DataGridView
                        //if (!dataGridView_项目统计.Columns.Contains(property.Id))
                        //{
                        //    dataGridView_项目统计.Columns.Add(property.Id, property.Name); // 添加自定义属性为新列
                        //}
                        ////int customColumnIndex = dataGridView_项目统计.Columns[property.Id].Index; // 获取该列索引
                        //row.Cells.Add(new DataGridViewTextBoxCell { Value = property.Value });

                    }
                }

                // 将行添加到DataGridView中
                dataGridView_项目统计.Rows.Add(row);
                dataGridView_项目统计.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
                dataGridView_项目统计.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
        }

        /// <summary>
        /// 项目目录绑定数据
        /// </summary>
        /// <param name="ProjectPropertyListS"></param>
        private void BindDataToGridView_ProjectCatalog(List<List<ProjectPropertyModel>> ProjectPropertyListS)
        {
            LoadProjectUserInfoListDataFromJson(ref statisticsUserInfoListTemp);
            dataGridView_目录.DataSource = null;
            // 清空现有的行
            dataGridView_目录.Rows.Clear();

            foreach (var deptItem in statisticsUserInfoListTemp)//循环部门
            {
                foreach (var projectItem in deptItem.projectInfoList)//循环部门中的项目
                {
                    foreach (var stageItem in projectItem.projectStageList)//循环项目中的阶段
                    {
                        foreach (var majroItem in stageItem.projectMajroList)//循环阶段中的专业
                        {
                            foreach (var subProjectItem in majroItem.subProjectList)//循环专业中的子项
                            {
                                //行变量
                                var row = new DataGridViewRow();
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectItem.projectNo });//项目编号
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectItem.projectName });//项目名称
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectItem.constructUnit });//建设单位
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectItem.projectManager });//项目经理
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = stageItem.projectStageName });//项目阶段名称
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = majroItem.projectMajroName });//项目专业名称
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = subProjectItem.subProjectName }); //子项目名称
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = subProjectItem.fileNumber });//文件数量
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = subProjectItem.A1SizeNumber });//A1数量

                                // 将行添加到DataGridView中
                                dataGridView_目录.Rows.Add(row);
                                dataGridView_目录.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
                                dataGridView_目录.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 项目人员统计绑定数据
        /// </summary>
        /// <param name="ProjectPropertyListS"></param>
        private void BindDataToGridView_User_Project(List<List<ProjectPropertyModel>> ProjectPropertyListS)
        {
            LoadProjectUserInfoListDataFromJson(ref statisticsUserInfoListTemp);
            dataGridView_项目统计.DataSource = null;
            // 清空现有的行
            dataGridView_项目统计.Rows.Clear();

            foreach (var deptItem in statisticsUserInfoListTemp)//循环部门
            {
                foreach (var projectItem in deptItem.projectInfoList)//循环部门中的项目
                {
                    foreach (var stageItem in projectItem.projectStageList)//循环项目中的阶段
                    {
                        foreach (var majroItem in stageItem.projectMajroList)//循环阶段中的专业
                        {
                            foreach (var subProjectItem in majroItem.subProjectList)//循环专业中的子项
                            {
                                //行变量
                                var row = new DataGridViewRow();
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = deptItem.projectDeptName });//部门名称
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectItem.projectNo });//项目编号
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectItem.projectName });//项目名称
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectItem.constructUnit });//建设单位
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectItem.projectType });//项目类型
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectItem.founder });//项目创建人
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectItem.projectManager });//项目经理
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectItem.createTime });//创建时间
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectItem.projectStatus });//项目状态
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = stageItem.projectStageName });//项目阶段名称
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = majroItem.projectMajroName });//项目专业名称
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = subProjectItem.subProjectName }); //子项目名称
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = subProjectItem.fileNumber });
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = subProjectItem.A1SizeNumber });


                                // 将行添加到DataGridView中
                                dataGridView_项目统计.Rows.Add(row);
                                dataGridView_项目统计.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
                                dataGridView_项目统计.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 项目筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxProjectSelect_Click(object sender, EventArgs e)
        {
            projectDataTest.Clear();
            selectProjectUserApply();

        }

        /// <summary>
        /// 下拉菜单选择时的筛选
        /// </summary>
        private void selectProjectUserApply()
        {
            if (dataGridView_流程统计.CanSelect)
            {
                if (comboBox_流程统计项目名称.SelectedItem != null)
                {
                    comboBoxProject = comboBox_流程统计项目名称.SelectedItem.ToString();
                }
                if (comboBox_流程类型.SelectedItem != null)
                {
                    comboBoxApply = comboBox_流程类型.SelectedItem.ToString();
                }
                if (comboBox_流程人员.SelectedItem != null)
                {
                    comboBoxUser = comboBox_流程人员.SelectedItem.ToString();
                }
            }
            else if (dataGridView_项目统计.CanSelect)
            {
                if (comboBox_项目统计项目名.SelectedItem != null)
                {
                    comboBoxProject = comboBox_项目统计项目名.SelectedItem.ToString();
                }
                if (comboBox_项目人员.SelectedItem != null)
                {
                    comboBoxUser = comboBox_项目人员.SelectedItem.ToString();
                }
            }
            else if (dataGridView_人员统计.CanSelect)
            {
                if (comboBox_人员统计项目.SelectedItem != null)
                {
                    comboBoxProject = comboBox_人员统计项目.SelectedItem.ToString();
                }
                if (comboBox_人员统计人名.SelectedIndex != 0)
                {
                    comboBoxUser = comboBox_人员统计人名.SelectedItem.ToString();
                }
            }

            if (comboBoxProject != "00全部项目" && comboBoxUser != "00全部人员" && comboBoxApply != "00全部流程")
            {
                if (dataGridView_流程统计.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = ResultDataItemList?.Where(o => o.proName.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = projectDataTest?.Where(o => o.userName.Contains(comboBoxUser)).ToList();
                    //流程筛选
                    var applyDataTest = userDataTest?.Where(o => o.appName.Contains(comboBoxApply)).ToList();
                    //绑定数据
                    dataGridView_流程统计.DataSource = new SortableBindingList<ApplyInfoModel>(applyDataTest);
                    //计数
                    fileNumber(applyDataTest);
                }
                else if (dataGridView_项目统计.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = statisticsProjectListFileTemp?.Where(o => o[2].Value.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = projectDataTest?.Where(o => o[5].Name.Contains(comboBoxUser)).ToList();

                    //绑定数据
                    BindDataToGridView(userDataTest);
                    fileNumber(userDataTest);
                }
                else if (dataGridView_人员统计.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = userDataLists?.Where(o => o.projectName.Contains(comboBoxProject)).ToList();
                    if (projectDataTest.Count != 0)
                    {
                        dataGridView_人员统计.DataSource = new SortableBindingList<ProjectUserDataList>(projectDataTest);
                    }
                    fileNumber(projectDataTest);
                }
                else if (dataGridView_目录.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = statisticsProjectListFileTemp?.Where(o => o[2].Value.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = projectDataTest?.Where(o => o[5].Name.Contains(comboBoxUser)).ToList();
                    //绑定数据
                    BindDataToGridView_ProjectCatalog(userDataTest);
                    fileNumber(userDataTest);
                }

            }
            else if (comboBoxProject != "00全部项目" && comboBoxUser != "00全部人员" && comboBoxApply == "00全部流程")
            {
                if (dataGridView_流程统计.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = ResultDataItemList?.Where(o => o.proName.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = ResultDataItemList?.Where(o => o.userName.Contains(comboBoxUser)).ToList();

                    //绑定数据
                    dataGridView_流程统计.DataSource = new SortableBindingList<ApplyInfoModel>(userDataTest);
                    //计数
                    fileNumber(userDataTest);
                }
                else if (dataGridView_项目统计.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = statisticsProjectListFileTemp?.Where(o => o[2].Value.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = projectDataTest?.Where(o => o[5].Value.Contains(comboBoxUser)).ToList();

                    //绑定数据
                    BindDataToGridView(userDataTest);
                    //dataGridView_项目统计.DataSource = new SortableBindingList<List<ProjectPropertyModel>>(userDataTest);
                    fileNumber(userDataTest);

                }
                else if (dataGridView_人员统计.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = userDataLists?.Where(o => o.projectName.Contains(comboBoxProject)).ToList();
                    if (projectDataTest != null)
                    {
                        dataGridView_人员统计.DataSource = new SortableBindingList<ProjectUserDataList>(projectDataTest);
                    }

                    fileNumber(projectDataTest);
                }
                else if (dataGridView_目录.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = statisticsProjectListFileTemp?.Where(o => o[2].Value.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = projectDataTest?.Where(o => o[5].Name.Contains(comboBoxUser)).ToList();
                    //绑定数据
                    BindDataToGridView_ProjectCatalog(userDataTest);
                    fileNumber(userDataTest);
                }
            }
            else if (comboBoxProject != "00全部项目" && comboBoxUser == "00全部人员" && comboBoxApply == "00全部流程")
            {
                if (dataGridView_流程统计.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = ResultDataItemList?.Where(o => o.proName.Contains(comboBoxProject)).ToList();

                    //绑定数据
                    dataGridView_流程统计.DataSource = new SortableBindingList<ApplyInfoModel>(projectDataTest);
                    //计数
                    fileNumber(projectDataTest);
                }
                else if (dataGridView_项目统计.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = statisticsProjectListFileTemp?.Where(o => o[2].Value.Contains(comboBoxProject)).ToList();

                    int fileNumber = 0;//文件数量
                    double folded = 0;//A1数量
                    string projectName = comboBox_项目统计项目名.Text;//项目名称
                    var startDateTime = dateTimePicker_开始.Value.ToString("yyyy-M-d 00:00:00");//开始时间
                                                                                              //var endDateTime = dateTime.AddDays(1);
                    var endDateTime = dateTimePicker_截至.Value.ToString("yyyy-M-d 23:59:59");//结束时间
                    Read_One_Project_Attribute_Info_Http_Mysql_Datas("qz_project", "create_time", startDateTime, endDateTime, projectName, ref fileNumber, ref folded);
                    //读取本地项目列表文件
                    SystemTempData.LoadStatisticsProjectInfoPropertyListDataFromJson(ref statisticsProjectListFileTemp);

                    dataGridView_项目统计.DoubleBufferedDataGirdView(true);

                    dataGridView_项目统计.AutoGenerateColumns = false;

                    BindDataToGridView_User_Project(statisticsProjectListFileTemp);

                    ComboboxProjectData(statisticsProjectListFileTemp);
                }
                else if (dataGridView_人员统计.CanSelect)
                {
                    //用户筛选
                    var projectDataTest = userDataLists?.Where(o => o.projectName.Contains(comboBoxProject)).ToList();
                    if (projectDataTest != null)
                    {
                        dataGridView_人员统计.DataSource = new SortableBindingList<ProjectUserDataList>(projectDataTest);
                    }

                    fileNumber(projectDataTest);
                }
                else if (dataGridView_目录.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = statisticsProjectListFileTemp?.Where(o => o[2].Value.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = projectDataTest?.Where(o => o[5].Name.Contains(comboBoxUser)).ToList();
                    //绑定数据
                    BindDataToGridView_ProjectCatalog(userDataTest);
                    fileNumber(userDataTest);
                }
            }
            else if (comboBoxProject != "00全部项目" && comboBoxUser == "00全部人员" && comboBoxApply != "00全部流程")
            {
                if (dataGridView_流程统计.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = ResultDataItemList?.Where(o => o.proName.Contains(comboBoxProject)).ToList();
                    //流程筛选
                    var applyDataTest = projectDataTest?.Where(o => o.appName.Contains(comboBoxApply)).ToList();
                    //绑定数据
                    dataGridView_流程统计.DataSource = new SortableBindingList<ApplyInfoModel>(applyDataTest);
                    //计数
                    fileNumber(applyDataTest);
                }
                else if (dataGridView_项目统计.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = ProjectPropertieListS?.Where(o => o[2].Value.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = projectDataTest?.Where(o => o[4].Value.Contains(comboBoxUser)).ToList();

                    //绑定数据
                    BindDataToGridView(userDataTest);
                    fileNumber(userDataTest);

                }
                else if (dataGridView_人员统计.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = userDataLists?.Where(o => o.projectName.Contains(comboBoxProject)).ToList();
                    if (projectDataTest != null)
                    {
                        dataGridView_人员统计.DataSource = new SortableBindingList<ProjectUserDataList>(projectDataTest);
                    }

                    fileNumber(projectDataTest);
                }
                else if (dataGridView_目录.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = statisticsProjectListFileTemp?.Where(o => o[2].Value.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = projectDataTest?.Where(o => o[5].Name.Contains(comboBoxUser)).ToList();
                    //绑定数据
                    BindDataToGridView_ProjectCatalog(userDataTest);
                    fileNumber(userDataTest);
                }
            }
            else if (comboBoxProject == "00全部项目" && comboBoxUser == "00全部人员" && comboBoxApply == "00全部流程")
            {
                if (dataGridView_流程统计.CanSelect)
                {
                    //绑定数据
                    dataGridView_流程统计.DataSource = new SortableBindingList<ApplyInfoModel>(ResultDataItemList);
                    //计数
                    fileNumber(ResultDataItemList);
                }
                else if (dataGridView_项目统计.CanSelect)
                {
                    //绑定数据

                    BindDataToGridView(statisticsProjectListFileTemp);
                    fileNumber(statisticsProjectListFileTemp);

                }
                else if (dataGridView_人员统计.CanSelect)
                {
                    if (userDataLists != null)
                    {
                        dataGridView_人员统计.DataSource = new SortableBindingList<ProjectUserDataList>(userDataLists);
                    }

                    fileNumber(userDataLists);
                }
                else if (dataGridView_目录.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = statisticsProjectListFileTemp?.Where(o => o[2].Value.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = projectDataTest?.Where(o => o[5].Name.Contains(comboBoxUser)).ToList();
                    //绑定数据
                    BindDataToGridView_ProjectCatalog(userDataTest);
                    fileNumber(userDataTest);
                }
            }
            else if (comboBoxProject == "00全部项目" && comboBoxUser != "00全部人员" && comboBoxApply == "00全部流程")
            {
                if (dataGridView_流程统计.CanSelect)
                {
                    //用户筛选
                    List<ApplyInfoModel> userDataTest = ResultDataItemList?.Where(o => o.userName.Contains(comboBoxUser)).ToList();
                    //绑定数据
                    dataGridView_流程统计.DataSource = new SortableBindingList<ApplyInfoModel>(userDataTest);
                    //计数
                    fileNumber(userDataTest);
                }
                else if (dataGridView_项目统计.CanSelect)
                {
                    //用户筛选
                    var userDataTest = statisticsProjectListFileTemp?.Where(o => o[5].Value.Contains(comboBoxUser)).ToList();

                    //绑定数据
                    BindDataToGridView(userDataTest);
                    fileNumber(userDataTest);
                }
                else if (dataGridView_人员统计.CanSelect)
                {
                    if (userDataLists != null)
                    {
                        //项目筛选
                        var userDataTest = userDataLists?.Where(o => o.userName.Contains(comboBoxUser)).ToList();
                        if (userDataTemp != null && userDataTest.Count != 0)
                        {
                            dataGridView_人员统计.DataSource = new SortableBindingList<ProjectUserDataList>(userDataTest);
                            fileNumber(projectDataTest);
                        }
                        else
                        {
                            MessageBox.Show($"很抱歉，没找到{comboBoxUser}所在的项目！");
                        }
                    }
                }
                else if (dataGridView_目录.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = statisticsProjectListFileTemp?.Where(o => o[2].Value.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = projectDataTest?.Where(o => o[5].Name.Contains(comboBoxUser)).ToList();
                    //绑定数据
                    BindDataToGridView_ProjectCatalog(userDataTest);
                    fileNumber(userDataTest);
                }
            }
            else if (comboBoxProject == "00全部项目" && comboBoxUser != "00全部人员" && comboBoxApply != "00全部流程")
            {
                if (dataGridView_流程统计.CanSelect)
                {
                    //用户筛选
                    var userDataTest = ResultDataItemList?.Where(o => o.userName.Contains(comboBoxUser)).ToList();
                    //流程筛选
                    var applyDataTest = userDataTest?.Where(o => o.appName.Contains(comboBoxApply)).ToList();
                    //绑定数据
                    dataGridView_流程统计.DataSource = new SortableBindingList<ApplyInfoModel>(applyDataTest);
                    //计数
                    fileNumber(applyDataTest);
                }
                else if (dataGridView_项目统计.CanSelect)
                {

                    //用户筛选
                    var userDataTest = statisticsProjectListFileTemp?.Where(o => o[4].Value.Contains(comboBoxUser)).ToList();
                    //绑定数据
                    BindDataToGridView(userDataTest);
                    fileNumber(userDataTest);
                }
                else if (dataGridView_人员统计.CanSelect)
                {
                    //人员筛选
                    var projectDataTest = userDataLists?.Where(o => o.projectName.Contains(comboBoxProject)).ToList();
                    if (projectDataTest != null)
                    {
                        dataGridView_人员统计.DataSource = new SortableBindingList<ProjectUserDataList>(projectDataTest);
                    }
                    fileNumber(projectDataTest);
                }
                else if (dataGridView_目录.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = statisticsProjectListFileTemp?.Where(o => o[2].Value.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = projectDataTest?.Where(o => o[5].Name.Contains(comboBoxUser)).ToList();
                    //绑定数据
                    BindDataToGridView_ProjectCatalog(userDataTest);
                    fileNumber(userDataTest);
                }
            }
            else if (comboBoxProject == "00全部项目" && comboBoxUser == "00全部人员" && comboBoxApply != "00全部流程")
            {
                if (dataGridView_流程统计.CanSelect)
                {
                    //流程筛选
                    var applyDataTest = ResultDataItemList?.Where(o => o.appName.Contains(comboBoxApply)).ToList();
                    //绑定数据
                    dataGridView_流程统计.DataSource = new SortableBindingList<ApplyInfoModel>(applyDataTest);
                    //计数
                    fileNumber(applyDataTest);
                }
            }
            else if (comboBoxProject == "00全部项目" && comboBoxUser == "00全部人员" && comboBoxApply == "00全部流程")
            {
                if (dataGridView_流程统计.CanSelect)
                {

                    //绑定数据
                    dataGridView_流程统计.DataSource = new SortableBindingList<ApplyInfoModel>(ResultDataItemList);
                    //计数
                    fileNumber(ResultDataItemList);
                }
                else if (dataGridView_项目统计.CanSelect)
                {
                    //绑定数据
                    BindDataToGridView(statisticsProjectListFileTemp);
                    fileNumber(statisticsProjectListFileTemp);
                }
                else if (dataGridView_人员统计.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = userDataLists?.Where(o => o.projectName.Contains(comboBoxProject)).ToList();
                    if (projectDataTest.Count != 0)
                    {
                        dataGridView_人员统计.DataSource = new SortableBindingList<ProjectUserDataList>(projectDataTest);
                    }
                    fileNumber(projectDataTest);
                }
                else if (dataGridView_目录.CanSelect)
                {
                    //项目筛选
                    var projectDataTest = statisticsProjectListFileTemp?.Where(o => o[2].Value.Contains(comboBoxProject)).ToList();
                    //用户筛选
                    var userDataTest = projectDataTest?.Where(o => o[5].Name.Contains(comboBoxUser)).ToList();
                    //绑定数据
                    BindDataToGridView_ProjectCatalog(userDataTest);
                    fileNumber(userDataTest);
                }
            }
        }

        /// <summary>
        /// 流程筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxProcessTypeIdSelect_Click(object sender, EventArgs e)
        {
            selectProjectUserApply();
        }

        /// <summary>
        /// 人员筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxUserNameSelect_Click(object sender, EventArgs e)
        {
            if (comboBox_人员统计项目.SelectedItem != null)
            {
                comboBox_人员统计项目.SelectedIndex = 0;
                comboBoxProject = comboBox_人员统计项目.SelectedItem.ToString();
            }
            if (dataGridView_人员统计.CanSelect)
            {
                if (comboBoxUser != comboBox_人员统计人名.SelectedItem.ToString())
                {
                    return;
                }
            }
            selectProjectUserApply();
        }

        /// <summary>
        /// 把统计出来的文件数量写入到label上
        /// </summary>
        /// <param name="processDataTest">筛选后的数据</param>
        private void fileNumber(List<ApplyInfoModel> processDataTest)
        {
            label_合计.Text = "0";
            label_合计A1.Text = "0";
            label_总出版A1.Text = "0";
            int labelFileAll = 0;
            double labelFoldedAll = 0;
            double SumFoldeAll = 0;
            foreach (var item in processDataTest)
            {
                labelFileAll = item.FileAll + labelFileAll;
                labelFoldedAll = item.FoldedAll + labelFoldedAll;
                SumFoldeAll = item.FoldedAll * item.annex_id + SumFoldeAll;
            }
            label_合计.Text = labelFileAll.ToString();
            label_合计A1.Text = labelFoldedAll.ToString();
            label_总出版A1.Text = SumFoldeAll.ToString();
        }

        /// <summary>
        /// 把统计出来的文件数量写入到label上
        /// </summary>
        /// <param name="processDataTest">筛选后的数据</param>
        private void fileNumber(List<List<ProjectPropertyModel>> processDataTest)
        {
            label_合计.Text = "0";
            label_合计A1.Text = "0";
            label_总出版A1.Text = "0";
            int labelFileAll = 0;
            double labelFoldedAll = 0;
            double SumFoldeAll = 0;
            foreach (var item in processDataTest)
            {
                labelFileAll = Convert.ToInt32(item[12].Value) + labelFileAll;
                labelFoldedAll = Convert.ToDouble(item[13].Value) + labelFoldedAll;
                SumFoldeAll = labelFoldedAll;
            }
            label_合计.Text = labelFileAll.ToString();
            label_合计A1.Text = labelFoldedAll.ToString();
            label_总出版A1.Text = SumFoldeAll.ToString();
        }

        /// <summary>
        /// 把统计出来的文件数量写入到label上
        /// </summary>
        /// <param name="processDataTest">筛选后的数据</param>
        private void fileNumber(List<ProjectUserDataList> processDataTest)
        {
            if (processDataTest != null)
            {
                label_合计.Text = "0";
                label_合计A1.Text = "0";
                label_总出版A1.Text = "0";
                int labelFileAll = 0;
                double labelFoldedAll = 0;
                double SumFoldeAll = 0;
                foreach (var item in processDataTest)
                {
                    labelFileAll = item.fileNumber + labelFileAll;
                    labelFoldedAll = item.A1SizeNumber + labelFoldedAll;
                    SumFoldeAll = labelFoldedAll;
                }
                label_合计.Text = labelFileAll.ToString();
                label_合计A1.Text = labelFoldedAll.ToString();
                label_总出版A1.Text = SumFoldeAll.ToString();
            }

        }

        /// <summary>
        /// 表格第一列显示的序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
                                             e.RowBounds.Location.Y,
                                             dataGridView_流程统计.RowHeadersWidth - 8,
                                             e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView_流程统计.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView_流程统计.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// 打开流程详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                DataGridView dataGridView = (DataGridView)sender;
                var apply = ((BindingList<ApplyInfoModel>)dataGridView.DataSource)[e.RowIndex];
                var frm = new FrmApprovalInfo(apply.id);//打开流程详情
                frm.Show();
            }
        }

        /// <summary>
        /// 导出表格按键  ExcelContent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {

            //拿到当前时间；
            DateTime dateTime = DateTime.Now;
            //存盘路径与存盘文件名；
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Excel File|*.xlsx",
                FileName = dateTime.ToString("yyyyMMdd_hhmm") + "统计"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog.FileName == null) return;
                //Splasher.Show(typeof(FrmLoading));
                if (dataGridView_项目统计.CanSelect)
                {
                    EPPlusExportDataGridviewToExcel(dataGridView_项目统计, saveFileDialog);
                }
                else if (dataGridView_流程统计.CanSelect)
                {
                    EPPlusExportDataGridviewToExcel(dataGridView_流程统计, saveFileDialog);
                }
                else if (dataGridView_人员统计.CanSelect)
                {
                    EPPlusExportDataGridviewToExcel(dataGridView_人员统计, saveFileDialog);
                }
                else if (dataGridView_目录.CanSelect)
                {
                    EPPlusExportDataGridviewToExcel(dataGridView_目录, saveFileDialog);
                }
                //Splasher.Close();
            }

        }

        /// <summary>
        /// EPPlus导出表格方法
        /// </summary>
        /// <param name="dataGridView">传入进来的数据原</param>
        /// <param name="saveFileDialog">文件路径与存盘的文件名</param>
        private void EPPlusExportDataGridviewToExcel(DataGridView dataGridView, SaveFileDialog saveFileDialog)
        {
            ///创建一个Excel包
            FileInfo fileInfo = new FileInfo(saveFileDialog.FileName);
            ///
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = null;
                if (dataGridView_项目统计.CanSelect)
                {
                    worksheet = package.Workbook.Worksheets.Add("项目统计");
                }
                else if (dataGridView_流程统计.CanSelect)
                {
                    worksheet = package.Workbook.Worksheets.Add("流程统计");
                }
                else if (dataGridView_人员统计.CanSelect)
                {
                    worksheet = package.Workbook.Worksheets.Add("人员统计");
                }
                else if (dataGridView_目录.CanSelect)
                {
                    worksheet = package.Workbook.Worksheets.Add("项目文件目录");
                }

                int i = 0;
                // 将 DataGridView 的 HeaderText 作为 DataTable 的列名
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    i++;
                    worksheet.Cells[1, i].Value = column.HeaderText; // 使用表头名
                }
                //行数计数
                int rows = 0;
                // 添加 DataGridView 的数据到 DataTable
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.IsNewRow) continue; // 忽略新行
                    var values = new object[dataGridView.Columns.Count];
                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    {
                        worksheet.Cells[rows + 2, j + 1].Value = row.Cells[j].Value;
                    }
                    rows++;
                }
                //设置行高为20；
                worksheet.DefaultRowHeight = 24;
                //设置列宽为自动宽度；
                worksheet.Cells.AutoFitColumns();
                //设置第一行表头为加粗字体
                worksheet.Row(1).Style.Font.Bold = true;
                //设置第一行文字字号为13
                worksheet.Row(1).Style.Font.Size = 13;
                //设置第一行填充背景色；
                worksheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(1, 200, 200, 200);//设置单元格背景色
                //worksheet.Row(1).Height = 26;//设置第一行行高为26；
                //设置单元格边框
                worksheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                //保存表格；
                package.Save();
                MessageBox.Show("导出表格成功");

            }
        }

        /// <summary>
        /// 下拉菜单选择项
        /// </summary>
        public void ComboboxSelect()
        {
            qz_approvalList = SQLiteDataBase.SearchTableFromSQLite("qz_approval");

            foreach (DataRow item in qz_approvalList.Rows)
            {
                //qz_approvalStr.Add(item["name"].ToString());
                comboBox_流程类型.Items.Add(item["name"].ToString());
            }
            //项目筛选
            var projectDataTest = ResultDataItemList?.Where(o => o.proName.Contains(comboBoxProject)).ToList();
            if (projectDataTest.Count != 0)
            {
                ComboboxApplyData(projectDataTest);
            }
        }

        /// <summary>
        /// 统计页面选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 统计_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataGridView_流程统计.CanSelect)
            {
                comboBox_流程类型.SelectedIndex = 0;
                comboBox_流程统计项目名称.SelectedIndex = 0;
                comboBox_流程类型.Items.Clear();
                ComboboxSelect();
            }
            else if (dataGridView_项目统计.CanSelect)
            {
                //comboBox_项目统计项目名.Items.Clear();
                //comboBox_项目统计项目名.Items.Add("00全部项目");
                //comboBox_项目统计项目名.SelectedIndex = 0;
                //comboBox_项目人员.Items.Clear();
                //comboBox_项目人员.Items.Add("00全部人员");
                //comboBox_项目人员.SelectedIndex = 0;
                //项目筛选
                //var projectDataTest = ResultDataItemList?.Where(o => o.proName.Contains(comboBoxProject)).ToList();
                //ComboboxProjectData(projectDataTest);
                //projectListFileTemp.Clear();
                //LoadProjectListDataFromJson(ref projectListFileTemp);
                //if (comboBox_项目统计项目名.Items.Count != projectListFileTemp.Count)
                //{
                //    foreach (var project in projectListFileTemp)
                //    {
                //        comboBox_项目统计项目名.Items.Add(project.identifier + "-" + project.name);
                //        // 进行排序
                //        List<string> items = new List<string>(comboBox_项目统计项目名.Items.Cast<string>());
                //        items.Sort();

                //        // 重新设置排序后的数据
                //        comboBox_项目统计项目名.Items.Clear();
                //        comboBox_项目统计项目名.Items.AddRange(items.ToArray());
                //    }
                //}
            }
            else if (dataGridView_人员统计.CanSelect)
            {
                comboBox_人员统计项目.Items.Clear();
                comboBox_人员统计项目.Items.Add("00全部项目");
                comboBox_人员统计项目.SelectedIndex = 0;
                comboBox_人员统计人名.Items.Clear();
                comboBox_人员统计人名.Items.Add("00全部人员");
                comboBox_人员统计人名.SelectedIndex = 0;

                deptUserListTemp.Clear();
                LoadDetpUserListFromJson(ref deptUserListTemp);
                if (comboBox_人员统计人名.Items.Count != deptUserListTemp.Count)
                {
                    comboBox_人员统计人名.Items.Clear();

                    foreach (var userItem in deptUserListTemp)
                    {
                        if (!comboBox_人员统计人名.Items.Contains(userItem.realName))
                        {
                            comboBox_人员统计人名.Items.Add(userItem.realName);
                        }
                    }
                }
            }
        }

        #region 树节点事件

        private static object selectedNodeObject;

        /// <summary>
        /// 树节点选择前事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Archive_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView_Archive.SelectedNode != null)
            {
                //将上一个选中的节点背景色还原（原先没有颜色）
                treeView_Archive.SelectedNode.BackColor = Color.Empty;
                //还原前景色
                treeView_Archive.SelectedNode.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// 树节点点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Archive_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (radioButton_项目文件区.Checked == true)
            {
                try
                {
                    treeView_Archive.SelectedNode = e.Node;//选中这个节点
                    var level = e.Node.Level;//获取节点的层级
                    treeView_Archive.SelectedNode = e.Node;
                    //让选中项背景色呈现高亮
                    treeView_Archive.SelectedNode.BackColor = SystemColors.Highlight;
                    //前景色为白色
                    treeView_Archive.SelectedNode.ForeColor = Color.White;
                    if (e.Button == MouseButtons.Left)//鼠标左键
                    {
                        label1.Text = $"文件数量：0";
                        //ProjectFileTreeViewModel 项目文件树结构/1、id /2、name,名称 /3、parentId,上级id /4、proType, 类型（0项目，1阶段，2专业，3子项，4文件夹，5文件）/5、deptType 部门类型（0集团，1院，2所，3专业）没有值就是专业下面的/6、projectId 项目ID
                        var selectInfo = (ProjectFileTreeViewModel)e.Node.Tag;
                        //判断是选定的是不是机构中的 所 层级或是不是-1文件夹级；
                        if (selectInfo.deptType == "2" || selectInfo.deptType == "-1")
                        {
                            //如是所，那么获取到这个所下面的项目列表，如果是项目，那获取这个项目的id： 返回给url字符串；
                            var url = selectInfo.deptType == "2" ? AppGlobalModel.GetProjectList + $"?deptId={selectInfo.id}&table={(AppGlobalModel.OverallSituationMenu.Exists(o => o == "profile:all:list") ? 0 : 1)}" : AppGlobalModel.GetProjectLevelDetails + $"?parentId={selectInfo.id}";
                            //resultData是所有项目属性数据列表 或 是项目的属性数据 或 是专业文件夹数据；
                            var resultData = new List<ProjectResultModel>();
                            if (HttpGet(url, ref resultData))
                            {
                                e.Node.Nodes.Clear();
                                foreach (var item in resultData)
                                {
                                    LoadProjectTreeView(e.Node, item);
                                }
                                if (resultData.Count != 0)
                                {
                                    // 对所有子节点进行排序  
                                    SortTreeViewNodes(e.Node.Nodes);
                                }
                            }
                        }
                        if (treeViewNodeMouseClick)
                        {
                            e.Node.Expand();
                        }
                    }
                    treeViewNodeMouseClick = true;
                    Splasher.Close();
                }
                catch
                {
                    Splasher.Close();
                }
            }
            else if (radioButton_档案区.Checked == true)
            {
                try
                {
                    // 记录选中的节点
                    selectedNodeObject = e.Node.Tag;

                    // 设置选中节点的视觉效果
                    treeView_Archive.SelectedNode = e.Node;
                    treeView_Archive.SelectedNode.BackColor = SystemColors.Highlight;
                    treeView_Archive.SelectedNode.ForeColor = Color.White;

                    label1.Text = $"文件数量：0";
                    label2.Text = $"总A1数量：0   A1";

                    if (e.Button == MouseButtons.Left && treeViewNodeMouseClick)
                    {
                        e.Node.Nodes.Clear();
                        //LoadKeepProjectDir(queryInfo.parentId);
                    }
                    //如果点击的是部门
                    if (selectedNodeObject is SelectKeepDeptModel)
                    {
                        //部门
                        var deptInfo = (SelectKeepDeptModel)selectedNodeObject;
                        //如果点击的是部门
                        if (e.Button == MouseButtons.Left && treeViewNodeMouseClick)
                        {
                            //加载档案部门
                            LoadKeepDept(deptInfo.id);
                        }
                        //查询部门
                        queryInfo.parentId = deptInfo.id;
                    }
                    else
                    {
                        //项目
                        var deptInfo = (GetKeepProjectDirModel)selectedNodeObject;
                        //如果点击的是项目
                        queryInfo.parentId = deptInfo.id;
                    }
                    //如果点击的是项目
                    if (e.Button == MouseButtons.Left && treeViewNodeMouseClick)
                    {
                        //加载项目
                        LoadKeepProjectDir(queryInfo.parentId);
                    }
                    if (e.Node.Nodes.Count != 0)
                    {
                        // 对所有子节点进行排序
                        SortTreeViewNodes(e.Node.Nodes);
                    }
                    treeViewNodeMouseClick = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载档案区节点时发生错误：{ex.Message}");
                    Splasher.Close();
                }
            }
        }

        /// <summary>
        /// 加载项目
        /// </summary>
        /// <param name="treeNode"></param>
        private void LoadProjectTreeView(TreeNode treeNode, ProjectResultModel data)
        {
            // 创建新的项目树视图模型  
            var projectFileTreeView = new ProjectFileTreeViewModel
            {
                id = data.id,
                name = data.type == 0 ? data.identifier + "-" + data.name : data.name,
                deptType = "-1",
                proType = data.type,
                parentId = data.parentId,
                projectId = data.projectId
            };

            // 创建新的树节点  
            TreeNode root = new TreeNode
            {
                // 设置树节点名称  
                Text = projectFileTreeView.name,
                Tag = projectFileTreeView
            };

            // 将新节点添加到当前节点的子节点列表中  
            treeNode.Nodes.Add(root);

            // 对所有子节点进行排序  
            SortTreeViewNodes(treeNode.Nodes);

        }

        /// <summary>
        /// 树节点离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Archive_Leave(object sender, EventArgs e)
        {
            if (treeView_Archive.SelectedNode != null)
            {
                //将上一个选中的节点背景色还原（原先没有颜色）
                treeView_Archive.SelectedNode.BackColor = SystemColors.Highlight;
                //前景色为白色
                treeView_Archive.SelectedNode.ForeColor = Color.White;
            }
        }

        /// <summary>
        /// 折叠节点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Archive_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            treeViewNodeMouseClick = false;
        }

        /// <summary>
        /// 加载档案部门
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <param name="parentNode">父节点</param>
        private void LoadKeepDept(string parentId, TreeNode parentNode = null)
        {
            try
            {
                // 获取部门列表
                var resultData = new List<SelectKeepDeptModel>();
                // 发送请求 返回部门列表                

                if (HttpGet(AppGlobalModel.SelectKeepDept + "?parentId=" + parentId, ref resultData))
                {
                    foreach (var item in resultData)
                    {
                        TreeNode root = new TreeNode();
                        //根目录名称
                        root.Text = item.name;
                        root.Tag = item;

                        if (parentId == "0")
                        {
                            treeView_Archive.Nodes.Add(root);
                        }
                        else
                        {
                            treeView_Archive.SelectedNode.Nodes.Add(root);
                        }
                    }

                    if (parentId == "0")
                    {
                        treeView_Archive.ExpandAll();
                    }
                    else
                    {
                        treeView_Archive.SelectedNode.Expand();
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载档案部门时发生错误：{ex.Message}");
            }

        }

        /// <summary>
        /// 加载档案项目目录
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <param name="parentNode">父节点</param>
        private void LoadKeepProjectDir(string parentId, TreeNode parentNode = null)
        {
            //建立归档区项目目录
            var keepProjectResultData = new List<GetKeepProjectDirModel>();
            if (HttpGet(AppGlobalModel.GetKeepProjectDir + "?parentId=" + parentId, ref keepProjectResultData))
            {
                //遍历项目
                foreach (var keepProjectItem in keepProjectResultData)
                {
                    //创建新的树节点
                    TreeNode node = new TreeNode();
                    //根目录名称
                    node.Text = keepProjectItem.name;
                    //节点标签
                    node.Tag = keepProjectItem;

                    //if (parentNode == null)
                    //{
                    //    treeView_Archive.Nodes.Add(node);
                    //}
                    //else
                    //{
                    //    parentNode.Nodes.Add(node);
                    //}
                    treeView_Archive.SelectedNode.Nodes.Add(node);
                }
                treeView_Archive.SelectedNode.Expand();
            }

        }

        /// <summary>
        /// 项目排序
        /// </summary>
        /// <param name="nodes"></param>
        public static void SortTreeViewNodes(TreeNodeCollection nodes)
        {
            // 将节点转换为列表，进行排序后替换原节点  
            var nodeList = nodes.Cast<TreeNode>().ToList();
            nodeList.Sort(new TreeNodeComparer()); // 使用自定义比较器  
            nodes.Clear(); // 清空当前节点集合  
            foreach (var node in nodeList)
            {
                nodes.Add(node); // 重新添加排序后的节点  
            }

            // 递归排序所有子节点  
            foreach (TreeNode node in nodeList)
            {
                SortTreeViewNodes(node.Nodes);
            }
        }

        /// <summary>
        /// 排序比较
        /// </summary>
        private class TreeNodeComparer : IComparer<TreeNode>
        {
            public int Compare(TreeNode x, TreeNode y)
            {
                // 按照节点名称排序  
                return string.Compare(x.Text, y.Text);
            }
        }
        #endregion

        /// <summary>
        /// 项目人员综合数据
        /// </summary>
        private class ProjectUserDataList
        {
            /// <summary>
            /// 项目编号
            /// </summary>
            public string projectNo { get; set; }
            /// <summary>
            /// 项目名称
            /// </summary>
            public string projectName { get; set; }
            /// <summary>
            /// 阶段名称
            /// </summary>
            public string stageName { get; set; }
            /// <summary>
            /// 专业名称
            /// </summary>
            public string majroName { get; set; }
            /// <summary>
            /// 角色名称
            /// </summary>
            public string roleName { get; set; }
            /// <summary>
            /// 用户名称
            /// </summary>
            public string userName { get; set; }
            /// <summary>
            /// 子项名称
            /// </summary>
            public string subProjectName { get; set; }
            /// <summary>
            /// 文件数量
            /// </summary>
            public int fileNumber { get; set; }
            /// <summary>
            /// 折A1数量
            /// </summary>
            public double A1SizeNumber { get; set; }
        }

        /// <summary>
        /// 项目人员综合数据
        /// </summary>
        private class ApplyItemDataList
        {
            /// <summary>
            /// 项目编号
            /// </summary>
            public string applyName { get; set; }
            /// <summary>
            /// 项目名称
            /// </summary>
            public string applyId { get; set; }
            /// <summary>
            /// 阶段名称
            /// </summary>
            public string stageName { get; set; }

        }

        /// <summary>
        /// 搜索Mysql内的流程列表
        /// </summary>
        private void SearchMysqlApplyList(string applyType)
        {
            // 自定义格式字符串
            string format = "yyyy-M-d HH:mm:ss";
            var startDateTime = dateTimePicker_开始.Value.ToString(format);
            var endDateTime = dateTimePicker_截至.Value.ToString(format);

            Read_Mysql_ApplyListHttpDatas($"{applyType}", startDateTime, endDateTime);
        }

        private void radioButton_项目文件区_CheckedChanged(object sender, EventArgs e)
        {
            treeView_Archive.Nodes.Clear();
            // 加载树状架构
            treeView_ProjectFile_Load();
        }

        private void radioButton_档案区_CheckedChanged(object sender, EventArgs e)
        {
            treeView_Archive.Nodes.Clear();
            // 加载树状架构
            treeView_Archive_Load();
        }

    }
}
