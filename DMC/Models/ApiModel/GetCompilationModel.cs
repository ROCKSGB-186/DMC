namespace DMC.Models
{
    /// <summary>
    /// 获得编研
    /// </summary>
    public class GetCompilationModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public string projectId { get; set; }
        /// <summary>
        /// 档案类型
        /// </summary>
        public string archivesType { get; set; }
        /// <summary>
        /// 档案号
        /// </summary>
        public string archivesNum { get; set; }
        /// <summary>
        /// 图纸卷柜号
        /// </summary>
        public string blueprintCabinet { get; set; }
        /// <summary>
        /// 图纸归档人
        /// </summary>
        public string blueprintUser { get; set; }
        /// <summary>
        /// 资料卷柜号
        /// </summary>
        public string materialCabinet { get; set; }
        /// <summary>
        /// 资料归档人
        /// </summary>
        public string materialUser { get; set; }
        /// <summary>
        /// 文件类别
        /// </summary>
        public string fileType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 案卷号
        /// </summary>
        public int records { get; set; }
        /// <summary>
        /// 项目内容摘要
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 编制日期
        /// </summary>
        public string formationTime { get; set; }
        /// <summary>
        /// 项目地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 保管期限
        /// </summary>
        public string safekeep { get; set; }
        /// <summary>
        /// 保密级别
        /// </summary>
        public string secrecy { get; set; }
    }
}
