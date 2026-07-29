namespace DMC.Models
{
    /// <summary>
    /// 项目文件树结构/1、id /2、name,名称 /3、parentId,上级id /4、proType, 类型（0项目，1阶段，2专业，3子项，4文件夹，5文件）/5、deptType 部门类型（0集团，1院，2所，3专业）没有值就是专业下面的/6、projectId 项目ID
    /// </summary>
    public class ProjectFileTreeViewModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 上级ID
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 类型0项目，1阶段，2专业，3子项，4文件夹，5文件
        /// </summary>
        public int proType { get; set; }
        /// <summary>
        /// 部门类型（0集团，1院，2所，3专业）没有值就是专业下面的
        /// </summary>
        public string deptType { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string projectId { get; set; }
    }
}
