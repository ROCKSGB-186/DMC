namespace DMC.Models
{
    /// <summary>
    /// 获取流程列表/1：id 主键/2：name 名称/3：processtypeid 流程类型 0签名 5签章 1出版 2下载 3归档 4其他 6签名签章
    /// </summary>
    public class ApprovalListModel
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
        /// 流程类型 0签名 5签章 1出版 2下载 3归档 4其他 6签名签章processtypeid
        /// </summary>
        public string processtypeId { get; set; }
    }
}
