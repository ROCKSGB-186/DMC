namespace DMC.Models
{
    /// <summary>
    /// 返回项目阶段相关信息：/1：ID /2：StageName 项目阶段名称 /3：projectStageId 项目阶段id
    /// </summary>
    public class ProjectStageResultModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 项目阶段名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 项目阶段id
        /// </summary>
        public string projectStageId { get; set; }
    }
}
