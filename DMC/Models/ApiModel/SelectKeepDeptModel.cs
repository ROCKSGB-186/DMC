namespace DMC.Models
{
    /// <summary>
    /// 查询归档目录层级/ 1：Id/ 2:parentId 父id/ 3:name 名称/ 4：createTime 创建时间/ 5：ancestors值/ 6：status 状态/ 7：identifier 标识符
    /// </summary>
    public class SelectKeepDeptModel
    {
        /// <summary>
        /// 1：Id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 2:parentId 父id
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 3:name 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 4：createTime 创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 5：ancestors值
        /// </summary>
        public string ancestors { get; set; }
        /// <summary>
        /// 6：status 状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 7：identifier 标识符
        /// </summary>
        public string identifier { get; set; }
    }
}
