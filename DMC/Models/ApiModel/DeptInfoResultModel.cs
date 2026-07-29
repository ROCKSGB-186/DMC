namespace DMC.Models
{
    /// <summary>
    /// 返回组织架构：
    /// 1：deptId\主键
    /// 2：parentId父级 
    /// 3：deptName公司名称 
    /// 4：deptType\部门类型（0集团，1院，2所，3专业）没有值就是专业下面的 
    /// 5：uudi
    /// </summary>
    public class DeptInfoResultModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string deptId { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public string parentId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string deptName { get; set; }

        /// <summary>
        /// 部门类型（0集团，1院，2所，3专业）没有值就是专业下面的
        /// </summary>
        public string deptType { get; set; }

        /// <summary>
        /// uuid
        /// </summary>
        public string uuid { get; set; }
    }
}
