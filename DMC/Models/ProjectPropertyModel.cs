namespace DMC.Models
{
    /// <summary>
    /// 项目属性模型:0、No序号；1、id； 2、Name名称 3、Value值
    /// </summary>
    public class ProjectPropertyModel
    {
        /// <summary>
        /// 0：No 序号
        /// </summary>
        public string No {  get; set; }
        /// <summary>
        /// 1:Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 2:Name 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 3:Value 值
        /// </summary>
        public string Value { get; set; }
    }
}
