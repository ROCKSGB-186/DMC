namespace DMC.Models
{
    /// <summary>
    /// 添加归档项目属性
    /// </summary>
    public class AddKeepProjectAttributeModel
    {
        /// <summary>
        /// 可行性研究开始时间
        /// </summary>
        public string oneStartTime { get; set; }
        /// <summary>
        /// 可行性研究结束时间
        /// </summary>
        public string oneEndTime { get; set; }
        /// <summary>
        /// 前期工作开始时间
        /// </summary>
        public string twoStartTime { get; set; }
        /// <summary>
        /// 前期工作结束时间
        /// </summary>
        public string twoEndTime { get; set; }
        /// <summary>
        /// 初步设计开始时间
        /// </summary>
        public string threeStartTime { get; set; }
        /// <summary>
        /// 初步设计结束时间
        /// </summary>
        public string threeEndTime { get; set; }
        /// <summary>
        /// 施工图开始时间
        /// </summary>
        public string fourStartTime { get; set; }
        /// <summary>
        /// 施工图结束时间
        /// </summary>
        public string fourEndTime { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public string projectId { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public string other { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }
    }
}
