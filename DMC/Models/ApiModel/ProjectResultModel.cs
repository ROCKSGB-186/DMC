namespace DMC.Models
{
    /// <summary>
    /// 返回项目信息/1:createTime 创建时间 /2:id 项目id /3:name 项目名称 /4:userId 创建用户id /5:parentId 上级ID /6:type 类型0项目，1阶段，2专业，3子项，4文件夹，5文件 /7:status 0正常，1停用，2未发布，3删除 /8:projectId 项目ID /9:varargsId 原始字典数据id /10:identifier 项目编号 /11:unit 建筑单位 /12:userName 创建人 /13:IsChecked 选择状态 /14:parentList 祖籍列表
    /// </summary>
    public class ProjectResultModel
    {
        /// <summary>
        ///1 创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        ///2 项目id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        ///3 项目名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        ///4 创建用户id
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        ///5 上级ID
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        ///6 类型0项目，1阶段，2专业，3子项，4文件夹，5文件
        /// </summary>
        public int type { get; set; }
        /// <summary>
        ///7 0正常，1停用，2未发布，3删除
        /// </summary>
        public int status { get; set; }
        /// <summary>
        ///8 项目ID
        /// </summary>
        public string projectId { get; set; }
        /// <summary>
        ///9 原始字典数据id
        /// </summary>
        public string varargsId { get; set; }
        /// <summary>
        ///10 项目编号
        /// </summary>
        public string identifier { get; set; }
        /// <summary>
        ///11 建筑单位
        /// </summary>
        public string unit { get; set; }
        /// <summary>
        ///12 创建人
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        ///13 选择状态
        /// </summary>
        public bool IsChecked { get; set; }
        /// <summary>
        ///14 祖籍列表
        /// </summary>
        public string parentList { get; set; }
        /// <summary>
        ///15 是否已归档 0未归档 1已归档
        /// </summary>
        public int is_documentation { get; set; }
        /// <summary>
        ///16 项目类型 1工业建筑 2民用建筑 3其它
        /// </summary>
        public string projectType { get; set; }
    }
}
