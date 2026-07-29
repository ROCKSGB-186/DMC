using System.Collections.Generic;

namespace DMC.Models
{
    /// <summary>
    /// 项目阶段:1:ID/ 2:StageName 阶段名称/ 3:projectStageId 项目阶段id
    /// </summary>
    public class ProjectStageViewModel
    {
        /// <summary>
        /// 1:ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 2:StageName 阶段名称
        /// </summary>
        public string StageName { get; set; }
        /// <summary>
        /// 3:projectStageId 项目阶段id
        /// </summary>
        public string projectStageId { get; set; }
    }

    /// <summary>
    /// 项目专业: 1：stageId 隶属阶段id/ 2: ID/ 3: MajorName 专业名称/ 4: projectMajorId项目专业id/ 5: 慢板列表 List<GetProjectLevelUserModel> template
    /// </summary>
    public class ProjectMajorViewModel
    {
        /// <summary>
        /// 1：stageId 隶属阶段id
        /// </summary>
        public string stageId { get; set; }
        /// <summary>
        /// 2: ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 3: MajorName 专业名称
        /// </summary>
        public string MajorName { get; set; }
        /// <summary>
        /// 4: projectMajorId项目专业id
        /// </summary>
        public string projectMajorId { get; set; }
        /// <summary>
        /// 5: template 慢板列表
        /// </summary>
        public List<GetProjectLevelUserModel> template { get; set; }
    }

    /// <summary>
    /// 项目人员：1：stageId隶属阶段id/2:majorId隶属专业id/3:ID/4:UserName人员名称
    /// </summary>
    public class ProjectUserViewModel
    {
        /// <summary>
        /// 1：stageId隶属阶段id
        /// </summary>
        public string stageId { get; set; }
        /// <summary>
        /// 2:majorId隶属专业id
        /// </summary>
        public string majorId { get; set; }
        /// <summary>
        /// 3:ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 4:UserName人员名称
        /// </summary>
        public string UserName { get; set; }
    }

    /// <summary>
    /// 项目人员角色/ 1:stageId 隶属阶段id/ 2:majorId 隶属专业id/ 3:userId 隶属人员id/ 4:ID/ 5:name 名称
    /// </summary>
    public class ProjectUserRoleViewModel
    {
        /// <summary>
        /// 1:stageId 隶属阶段id
        /// </summary>
        public string stageId { get; set; }
        /// <summary>
        /// 2:majorId 隶属专业id
        /// </summary>
        public string majorId { get; set; }
        /// <summary>
        /// 3:userId 隶属人员id
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 4:ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 5:name 名称
        /// </summary>
        public string name { get; set; }
    }
}
