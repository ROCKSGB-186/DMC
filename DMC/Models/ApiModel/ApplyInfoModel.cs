using System.Collections.Generic;

namespace DMC.Models
{
    /// <summary>
    /// 审批详情:1:applyXh 序号/2：appName 流程类型名/3：userDpt 用户部门/4：remark 备注/5：userId 发起人Id/6:userName 用户名/ 7:nodeList 节点List/ 8:NAME 流程标题/9：processtype_id 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 /10：result 审批状态： 0进行中 1已通过 -1未通过/10-1：resultName 审批状态： 0进行中 1已通过 -1未通过/11：createTime 提交时间/12：resultTime 最后审批时间/13 :fileids 要是按文件夹发起的就传文件夹id，按项目就传项目id，文件就传文件id，购物车就不用传/14：days 流程天数/15 ：id 主键/16 ：proId 项目id/17：proName 项目名称/ 18：annex_id 打印份数/ 19:fileType 文件来源0 项目区 1归档区/20: guiId 归档使用/21:downUser 下载人主键/22:money 出版订单金额/ 23 :FoldedAll 折A1/ 24: FileAll 文件总数/ 25:applyUser 负责人/ 26:resultRemark 备注/ 27：nodeName 节点名
    /// </summary>
    public class ApplyInfoModel
    {
        /// <summary>
        ///1:applyXh 序号
        /// </summary>
        public string applyXh { get; set; }
        /// <summary>
        /// 2:appName 流程类型名
        /// </summary>
        public string appName { get; set; }
        /// <summary>
        /// 3:userDept 用户部门
        /// </summary>
        public string userDept { get; set; }
        /// <summary>
        /// 4:remark 流程发起时备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 5:userId 发起人id
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 6:userName 用户名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 7:nodeList 节点List( 1:nodeName 节点名称// 2:result 审批状态：0进行中 1已通过 -1未通过/ 3:applyUser 负责人/ 4:sum 2就代表我审批了/ 5:id 主键/ 6:resultTime 最后审批时间/ 7:resultRemark 备注)
        /// </summary>
        public List<NodeListItem> nodeList { get; set; }
        /// <summary>
        /// 8:NAME 流程标题
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 9:processtype_id 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 
        /// </summary>
        public string processtype_id { get; set; }
        /// <summary>
        /// 10: result 审批状态： 0进行中 1已通过 -1未通过
        /// </summary>
        public int result { get; set; }
        /// <summary>
        /// 10-1: resultName 审批状态： 0进行中 1已通过 -1未通过
        /// </summary>
        public string resultName { get; set; }
        /// <summary>
        /// 11 : createTime 提交时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 12:resultTime 最后审批时间
        /// </summary>
        public string resultTime { get; set; }
        /// <summary>
        /// 13:fileids 要是按文件夹发起的就传文件夹id，按项目就传项目id，文件就传文件id，购物车就不用传
        /// </summary>
        public string fileids { get; set; }
        /// <summary>
        /// 14：days 流程天数
        /// </summary>
        public string days { get; set; }
        /// <summary>
        /// 15 ：id 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 16 ：proId 项目id
        /// </summary>
        public string proId { get; set; }
        /// <summary>
        /// 17：proName 项目名称
        /// </summary>
        public string proName { get; set; }
        /// <summary>
        /// 18：annex_id 打印份数
        /// </summary>
        public int annex_id { get; set; }
        /// <summary>
        /// 19:fileType 文件来源0 项目区 1归档区
        /// </summary>
        public int fileType { get; set; }
        /// <summary>
        ///20: guiId 归档使用
        /// </summary>
        public string guiId { get; set; }
        /// <summary>
        ///21:downUser 下载人主键
        /// </summary>
        public string downUser { get; set; }
        /// <summary>
        ///22:money 出版订单金额
        /// </summary>
        public string money { get; set; }
        /// <summary>
        /// 23 :FoldedAll 折A1
        /// </summary>
        public double FoldedAll { get; set; }
        /// <summary>
        /// 24: FileAll 文件总数
        /// </summary>
        public int FileAll { get; set; }
        /// <summary>
        /// 25:applyUser 负责人
        /// </summary>
        public string applyUser {  get; set; }
        /// <summary>
        /// 26:resultRemark 备注
        /// </summary>
        public string resultRemark { get; set; }
        /// <summary>
        /// 27：nodeName 节点名
        /// </summary>
        public string nodeName {  get; set; }

    }
    /// <summary>
    /// 1:nodeName 节点名称// 2:result 审批状态：0进行中 1已通过 -1未通过/ 3:applyUser 负责人/ 4:sum 2就代表我审批了/ 5:id 主键/ 6:resultTime 最后审批时间/ 7:resultRemark 备注
    /// </summary>
    public class NodeListItem
    {
        /// <summary>
        /// 1:nodeName 节点名称
        /// </summary>
        public string nodeName { get; set; }
        /// <summary>
        /// 2:result 审批状态：0进行中 1已通过 -1未通过
        /// </summary>
        public int result { get; set; }
        /// <summary>
        /// 3:applyUser 负责人
        /// </summary>
        public string applyUser { get; set; }
        /// <summary>
        /// 4:sum 2就代表我审批了
        /// </summary>
        public int sum { get; set; }
        /// <summary>
        /// 5:id 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 6:resultTime 最后审批时间
        /// </summary>
        public string resultTime { get; set; }
        /// <summary>
        /// 7:resultRemark 备注
        /// </summary>
        public string resultRemark { get; set; }
    }
}
