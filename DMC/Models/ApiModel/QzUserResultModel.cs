namespace DMC.Models
{
    /// <summary>
    /// 返回的机构人员信息/ 1：userName 账户/ 2：realName 姓名/ 3：deptName 部门名称/ 4：id 主键id 
    /// </summary>
    public class QzUserResultModel
    {
        /// <summary>
        /// 1：userName 账户
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 2：realName 姓名
        /// </summary>
        public string realName { get; set; }
        /// <summary>
        /// 3：deptName 部门名称
        /// </summary>
        public string deptName { get; set; }
        /// <summary>
        /// 4：id 主键id 
        /// </summary>
        public string id { get; set; }
    }
}
