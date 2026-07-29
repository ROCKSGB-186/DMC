namespace DMC.Models
{
    /// <summary>
    /// 获得归档技术资料详细列的名称列表
    /// </summary>
    public class GetKeepTechnicalNameListModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int must { get; set; }
        /// <summary>
        /// 模板地址
        /// </summary>
        public string filePath { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int sort { get; set; }
        /// <summary>
        /// 本地文件
        /// </summary>
        public string localFile { get; set; }
        /// <summary>
        /// 上传文件参数
        /// </summary>
        public FileUploadModel fileUpload { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string localFilePath { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public int rowNo { get; set;}
        /// <summary>
        /// 专业
        /// </summary>
        public string major { get; set;}
    }
}
