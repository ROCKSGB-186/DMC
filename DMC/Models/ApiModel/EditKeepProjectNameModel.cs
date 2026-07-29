namespace DMC.Models
{
    /// <summary>
    /// 归档修改名称
    /// </summary>
    public class EditKeepProjectNameModel
    {
        /// <summary>
        /// 父级id
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string newName { get; set; }
        /// <summary>
        /// 类型 0归档区目录 1归档项目区目录 2文件
        /// </summary>
        public string type { get; set; }
    }
}
