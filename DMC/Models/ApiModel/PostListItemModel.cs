using System.Collections.Generic;

namespace DMC.Models
{
    /// <summary>
    /// 返回的岗位类/ 1：postId 主键/ 2: postName 名子/ 3:List<QzMenusItemModel> qzMenus 权限菜单
    /// </summary>
    public class PostListItemModel
    {
        /// <summary>
        /// 1：postId 主键
        /// </summary>
        public string postId { get; set; }
        /// <summary>
        /// 2: postName 名子
        /// </summary>
        public string postName { get; set; }
        /// <summary>
        /// 3:List<QzMenusItemModel> qzMenus 权限菜单
        /// </summary>
        public List<QzMenusItemModel> qzMenus { get; set; }
    }
}
