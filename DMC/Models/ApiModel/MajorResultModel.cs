using System.Collections.Generic;

namespace DMC.Models
{
    /// <summary>
    /// 返回的专业相关信息1：majorId 主键/2：MajorName 专业名称/3：projectMajorId项目专业主键/4：template 项目人员列表
    /// </summary>
    public class MajorResultModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string majorId { get; set; }
        /// <summary>
        /// 专业名称
        /// </summary>
        public string MajorName { get; set; }
        /// <summary>
        /// 项目专业主键
        /// </summary>
        public string projectMajorId { get; set; }
        /// <summary>
        /// 项目人员列表
        /// </summary>
        public List<GetProjectLevelUserModel> template { get; set; }
    }
}
