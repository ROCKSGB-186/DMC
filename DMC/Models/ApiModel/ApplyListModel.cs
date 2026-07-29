namespace DMC.Models
{
    /// <summary>
    /// 审批列表查询 /1：applyXh 序号; /2：proName 项目名称; /3：processtypeId 流程类型 0签名 5签章 1出版 2下载 3归档 4其他 6签名签章; /4： userName 用户名称; /5：NAME 流程标题; /6：result 审批状态0进行中 1已通过 -1未通过; /7：createTime 创建时间; /8：lastTime 最后审批时间; /9： remark 备注; /10： id 主键ID;
    /// </summary>
    public class ApplyListModel
    {
        /// <summary>
        ///1:applyXh 序号
        /// </summary>
        public string applyXh { get; set; }
        /// <summary>
        ///2:proName 项目名称
        /// </summary>
        public string proName { get; set; }
        /// <summary>
        ///3:processtypeId 流程类型 0签名 5签章 1出版 2下载 3归档 4其他 6签名签章
        /// </summary>
        public string processtypeId { get; set; }
        /// <summary>
        ///4:userName 用户名称
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        ///5:NAME 流程标题
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        ///6:result 审批状态0进行中 1已通过 -1未通过
        /// </summary>
        public int result { get; set; }
        /// <summary>
        ///7:createTime 创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        ///8:lastTime 最后审批时间
        /// </summary>
        public string lastTime { get; set; }
        /// <summary>
        ///9:remark 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        ///10:id 主键ID
        /// </summary>
        public string id { get; set; }
      
       
    }
}
