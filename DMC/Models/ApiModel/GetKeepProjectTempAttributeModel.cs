namespace DMC.Models
{
    /// <summary>
    /// 获得归档项目临时字段:/ 1:id/ 2:oneStartTime 可行性研究开始时间/ 3:oneEndTime 可行性研究结束时间/ 4:twoStartTime 前期工作开始时间/ 5:twoEndTime 前期工作结束时间/ 6:threeStartTime 初步设计开始时间/ 7:threeEndTime 初步设计结束时间/ 8:fourStartTime 施工图开始时间/ 9:fourEndTime 施工图结束时间/ 10:projectId 项目ID/ 11:other 其他/ 12:remarks 这是一个备注/ 13:userId 创建用户Id
    /// </summary>
    public class GetKeepProjectTempAttributeModel
    {
        /// <summary>
        /// 1:id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 2:oneStartTime 可行性研究开始时间
        /// </summary>
        public string oneStartTime { get; set; }
        /// <summary>
        /// 3:oneEndTime 可行性研究结束时间
        /// </summary>
        public string oneEndTime { get; set; }
        /// <summary>
        /// 4:twoStartTime 前期工作开始时间
        /// </summary>
        public string twoStartTime { get; set; }
        /// <summary>
        /// 5:twoEndTime 前期工作结束时间
        /// </summary>
        public string twoEndTime { get; set; }
        /// <summary>
        /// 6:threeStartTime 初步设计开始时间
        /// </summary>
        public string threeStartTime { get; set; }
        /// <summary>
        /// 7:threeEndTime 初步设计结束时间
        /// </summary>
        public string threeEndTime { get; set; }
        /// <summary>
        /// 8:fourStartTime 施工图开始时间
        /// </summary>
        public string fourStartTime { get; set; }
        /// <summary>
        /// 9:fourEndTime 施工图结束时间
        /// </summary>
        public string fourEndTime { get; set; }
        /// <summary>
        /// 10:projectId 项目ID
        /// </summary>
        public string projectId { get; set; }
        /// <summary>
        /// 11:other 其他
        /// </summary>
        public string other { get; set; }
        /// <summary>
        /// 12:remarks 这是一个备注
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 13:userId 创建用户Id
        /// </summary>
        public string userId { get; set; }
    }
}
