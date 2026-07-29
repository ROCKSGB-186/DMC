namespace DMC.Models
{
    /// <summary>
    /// 客户端获得版本信息
    /// </summary>
    public class GetVersionModel
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 下载地址
        /// </summary>
        public string downloadUrl { get; set; }
        /// <summary>
        /// 更新状态0不更新1强制更新
        /// </summary>
        public int updateType { get; set; }
    }
}
