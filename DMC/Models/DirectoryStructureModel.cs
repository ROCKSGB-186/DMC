namespace DMC.Models
{
    /// <summary>
    /// 文件夹结构：1、ParentId：父ID/2、PrimaryKey：主键/3、Name：名/4、Type：类型1文件夹2文件/5、fileUpload：上传文件参数
    /// </summary>
    public class DirectoryStructureModel
    {
        /// <summary>
        /// 1：ParentId 父Id
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 2：PrimaryKey 主键
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 2：Name 名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 3：Type 1文件夹，2文件
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 4：fileUpload 上传文件参数
        /// </summary>
        public FileUploadModel fileUpload { get; set; }
    }
}
