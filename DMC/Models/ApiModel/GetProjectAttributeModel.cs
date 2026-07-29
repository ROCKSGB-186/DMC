using System.Collections.Generic;

namespace DMC.Models
{
    /// <summary>
    /// 获得项目属性:1：id主键/2：name项目名称/3：userId创建人id/4：parentId父级id/5：status项目状态（0正常 1停用 2未发布 3删除 4迭代）/6：identifier项目编码/7：unit建筑单位/8：proType项目类型/9：realName总负责人/10：govern项目经理/11：governName项目经理姓名/12：customList自定义属性
    /// </summary>
    public class GetProjectAttributeModel
    {
        /// <summary>
        /// 1：id 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 2：name 项目名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 3：userId 创建人id
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 4：parentId 父级id
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 5：status 状态（0正常 1停用 2未发布 3删除 4迭代）
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 6：identifier 项目编码
        /// </summary>
        public string identifier { get; set; }
        /// <summary>
        /// 7：unit 建筑单位
        /// </summary>
        public string unit { get; set; }
        /// <summary>
        /// 8：proType 项目类型
        /// </summary>
        public string proType { get; set; }
        /// <summary>
        /// 9：realName 总负责人
        /// </summary>
        public string realName { get; set; }
        /// <summary>
        /// 10：govern 项目经理
        /// </summary>
        public string govern { get; set; }
        /// <summary>
        /// 11：governName 项目经理姓名
        /// </summary>
        public string governName { get; set; }
        /// <summary>
        /// 12：customList 自定义属性
        /// </summary>
        public List<CustomInfo> customList { get; set; }
    }
    /// <summary>
    /// 自定义信息/ 1：id 主键/ 2：name 自定义属性名/ 3：custom 自定义属性字段/ 4：status 状态/ 5：content 内容
    /// </summary>
    public class CustomInfo
    {
        /// <summary>
        /// 1：id 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 2：name 自定义属性名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 3：custom 自定义属性字段
        /// </summary>
        public string custom { get; set; }
        /// <summary>
        /// 4：status 状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 5：content 内容
        /// </summary>
        public string content { get; set; }
    }
    
  
}
