namespace DMC.Models
{
    /// <summary>
    /// 消息列表分页
    /// </summary>
    public class MyMessageModel
    {
        // <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string proName { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 读取时间
        /// </summary>
        public string readTime { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public int isRead { get; set; }
        /// <summary>
        /// 链接类型
        /// </summary>
        public string jumpType { get; set; }
        /// <summary>
        /// 链接id
        /// </summary>
        public string jumpId { get; set; }
        /// <summary>
        /// 链接标题
        /// </summary>
        public string jumpTitle { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string userId { get; set; }
    }
}
