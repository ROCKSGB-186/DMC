using System;

namespace DMC.Models
{
    /// <summary>
    /// 获得归档项目层级
    /// </summary>
    public class GetKeepProjectDirModel
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string upDataTime { get; set; }
        //public DateTime? upDataTime { get; set; }  // 或者 string 类型
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 技术资料
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string identifier { get; set; }
        /// <summary>
        /// 建设单位
        /// </summary>
        public string unit { get; set; }
        /// <summary>
        /// 上级id
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 祖籍列表
        /// </summary>
        public string ancestors { get; set; }
        /// <summary>
        /// 类型 0项目 1阶段 2专业 3子项 4文件夹 5文件
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 文件id
        /// </summary>
        public int fileId { get; set; }
        /// <summary>
        /// 状态（0正常 1停用 2未发布 3删除）
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public string projectId { get; set; }
        /// <summary>
        /// 阶段或专业id
        /// </summary>
        public string varargsId { get; set; }
        /// <summary>
        /// 图幅名称
        /// </summary>
        public string frameName { get; set; }
        /// <summary>
        /// 折合A1数
        /// </summary>
        public string folded { get; set; }
        /// <summary>
        /// pdf总页数
        /// </summary>
        public string pageAll { get; set; }
        /// <summary>
        /// 文件类型id
        /// </summary>
        public string fileTypeId { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string filePath { get; set; }
        /// <summary>
        /// 归档技术资料名称表Id
        /// </summary>
        public string technicalId { get; set; }
        /// <summary>
        /// 文件夹或文件类型  0正常 1技术资料 2外审图纸 3归档信息
        /// </summary>
        public int dirType { get; set; }
        /// <summary>
        /// 审批同意用户ID
        /// </summary>
        public string resultId { get; set; }
        /// <summary>
        /// 审批同意用户名
        /// </summary>
        public string resultName { get; set; }
        /// <summary>
        /// 专业名
        /// </summary>
        public string majorName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uuName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string guName { get; set; }
        /// <summary>
        /// 图纸文件
        /// </summary>
        public string fileTypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string describe { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom7 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom8 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom9 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom10 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom11 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom12 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom13 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom15 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom16 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom17 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom18 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom19 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom20 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom21 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom22 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom23 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom24 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom25 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom26 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom27 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom28 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom29 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom30 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom31 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom32 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom33 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom34 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom35 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom36 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom37 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom38 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom39 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom40 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom41 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom42 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom43 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom44 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom45 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom46 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom47 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom48 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom49 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom50 { get; set; }
        /// <summary>
        /// 是否选择
        /// </summary>
        public bool isCheck { get; set; }
    }
}
