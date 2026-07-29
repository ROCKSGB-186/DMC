namespace DMC.Models
{
    /// <summary>
    /// 返回的权限类/1：menuId 菜单Id/ 2: menuName 菜单名/ 3：parentId 父Id/ 4：orderNum / 5：prems 参数
    /// </summary>
    public class QzMenusItemModel
    {
        /// <summary>
        /// 1：menuId 菜单Id
        /// </summary>
        public string menuId { get; set; }
        /// <summary>
        /// 2: menuName 菜单名
        /// </summary>
        public string menuName { get; set; }
        /// <summary>
        /// 3：parentId 父Id
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 4：orderNum 
        /// </summary>
        public int orderNum { get; set; }
        /// <summary>
        /// 5：prems 参数
        /// </summary>
        public string prems { get; set; }
    }
}
