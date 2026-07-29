namespace DMC.Models
{
    /// <summary>
    /// 部门信息：1：deptId 主键/ 2：parentId 父级/ 3：deptName 名称/ 4：deptType 部门类型（0集团，1院，2所，3专业）没有值就是专业下面的
    /// </summary>
    public class DeptInfoModel
    {
        /// <summary>
        /// 1：deptId 主键
        /// </summary>
        public string deptId { get; set; }
        /// <summary>
        /// 2：parentId 父级
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 3：deptName 名称
        /// </summary>
        public string deptName { get; set; }
        /// <summary>
        /// 4：deptType 部门类型（0集团，1院，2所，3专业）没有值就是专业下面的
        /// </summary>
        public string deptType { get; set; }
    }
}
