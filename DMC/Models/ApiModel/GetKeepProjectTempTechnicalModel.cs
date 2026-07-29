namespace DMC.Models
{
    /// <summary>
    /// 获得归档项目临时技术资料
    /// </summary>
    public class GetKeepProjectTempTechnicalModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string filePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 技术资料名称
        /// </summary>
        public string technicalName { get; set; }
        /// <summary>
        /// 0未读  1已读
        /// </summary>
        public int read { get; set; }
        /// <summary>
        /// 0非必填  1必填
        /// </summary>
        public string must { get; set;}
    }
}
