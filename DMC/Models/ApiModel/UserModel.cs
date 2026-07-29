namespace DMC.Models
{
    /// <summary>
    /// 返回的用户类/ 1:createTime 创建时间/ 2:updateTime 更新时间/ 3:remark/ 4:id 用户ID/ 5:userName 用户账户/ 6:nickName 昵称/ 7:password 密码/ 8:realName 真实名称/ 9：sex 性别/ 10：birthday 生日/ 11：email/ 12：phone 电话/ 13：loginDate 登录时间/ 14：token/ 15：status 状态/ 16：uuid/ 17：deptName 用户部门
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// 1:createTime 创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 2:updateTime 更新时间
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 3:remark
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 4:id 用户ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 5:userName 用户账户
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 6:nickName 昵称
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 7:password 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 8:realName 真实名称
        /// </summary>
        public string realName { get; set; }
        /// <summary>
        /// 9：sex 性别
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 10：birthday 生日
        /// </summary>
        public string birthday { get; set; }
        /// <summary>
        /// 11：email
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 12：phone 电话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 13：loginDate 登录时间
        /// </summary>
        public string loginDate { get; set; }
        /// <summary>
        /// 14：token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 15：status 状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 16：uuid
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// 17：deptName 用户部门
        /// </summary>
        public string deptName { get; set; }
    }
}
