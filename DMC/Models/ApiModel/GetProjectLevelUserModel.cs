using System.Collections.Generic;

namespace DMC.Models
{
    /// <summary>
    /// 获得项目层级人员：/ 1：userId 用户主键/ 2:userName 账户/ 3:realName 姓名/ 4:userRoleList 角色列表list
    /// </summary>
    public class GetProjectLevelUserModel
    {
        /// <summary>
        /// 1：userId 用户主键
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 2:userName 账户
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 3:realName 姓名
        /// </summary>
        public string realName { get; set; }
        /// <summary>
        /// 4:userRoleList 角色列表
        /// </summary>
        public List<UserRoleModel> userRoleList { get; set; }
    }

    public class UserRoleModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string roleId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string roleName { get; set; }
    }
}
