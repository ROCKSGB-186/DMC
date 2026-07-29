namespace DMC.Models
{
    /// <summary>
    /// 获得项目文件历史版本
    /// </summary>
    public class GetProjectFileLogListModel
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string details { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string filePath { get; set; }
    }
}
