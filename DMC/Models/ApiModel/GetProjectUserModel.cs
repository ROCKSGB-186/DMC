using System.Collections.Generic;

namespace DMC.Models
{
    /// <summary>
    /// 获得整个项目的人员列表：1：name 专业名/2：roleList 人员与角色列表:（1：userList 用户列表 /2：roleName 角色名）
    /// </summary>
    public class GetProjectUserModel
    {
        /// <summary>
        /// 专业名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 人员与角色列表
        /// </summary>
        public List<RoleListItem> roleList { get; set; }
    }
    /// <summary>
    /// 用户角色与用户列表：1：userList 用户列表 /2：roleName 角色名
    /// </summary>
    public class RoleListItem
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public List<string> userList { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string roleName { get; set; }
    }
}
