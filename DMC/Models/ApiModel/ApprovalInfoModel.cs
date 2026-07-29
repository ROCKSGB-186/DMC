using System.Collections.Generic;

namespace DMC.Models
{
    /// <summary>
    /// 获取流程信息 /1：id 流程主键 /2：processtypeid 流程类型 0签名 5签章 1出版 2下载 3归档 4其他 6签名签章 /3：qzApprovalNodeList 签章审批节点列表
    /// </summary>
    public class ApprovalInfoModel
    {
        /// <summary>
        /// 流程主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 流程类型 0签名 5签章 1出版 2下载 3归档 4其他 6签名签章processtypeid
        /// </summary>
        public string processtypeId { get; set; }
        /// <summary>
        /// 签章审批节点列表
        /// </summary>
        public List<QzApprovalNodeListItem> qzApprovalNodeList { get; set; }
    }
    /// <summary>
    /// 获取签名签章流程细节/1：id 主键/2：name 节点名称/3: nodeType 节点类型1签名，2签章，3不签/4:userList 用户列表/5: sealList 章列表/6: defaultSealList 默认章列表/7:sort 种类
    /// </summary>
    public class QzApprovalNodeListItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 节点类型1签名，2签章，3不签
        /// </summary>
        public int nodeType { get; set; }
        /// <summary>
        /// 用户列表
        /// </summary>
        public List<UserListItem> userList { get; set; }
        /// <summary>
        /// 章列表
        /// </summary>
        public List<SealListItem> sealList { get; set; }
        /// <summary>
        /// 默认章列表
        /// </summary>
        public List<SealListItem> defaultSealList { get; set; }
        /// <summary>
        /// 种类
        /// </summary>
        public int sort { get; set; }
    }
    /// <summary>
    /// 用户ListItem /1:id 主键/2：realName 姓名
    /// </summary>
    public class UserListItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string realName { get; set; }
    }
    /// <summary>
    /// 章listItem/1：id 主键/2：章名称/3：章到期时间
    /// </summary>
    public class SealListItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 章名称
        /// </summary>
        public string sealname { get; set; }
        /// <summary>
        /// 章到期时间
        /// </summary>
        public string endtime { get; set; }
    }
}
