using System.Collections.Generic;

namespace DMC.Models
{
    /// <summary>
    /// 文件上传类型模型
    /// </summary></c>
    public class FileUploadModel
    {
        /// <summary>
        /// 归档技术资料名称表Id
        /// </summary>
        public string technicalId { get; set; }
        /// <summary>
        /// 归档项目临时属性ID
        /// </summary>
        public string tempAttributeId { get; set; }
        /// <summary>
        /// 上级id
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 是否为pdf文件 0是 1不是
        /// </summary>
        public string isPdf { get; set; }
        /// <summary>
        /// //文件类型
        /// </summary>
        public string fileTypeId { get; set; }
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
        /// pdf页数详情
        /// </summary>
        public List<PageInfoItem> pageInfo { get; set; }
    }

    /// <summary>
    /// 文件上传类型模型
    /// </summary></c>
    public class FileUploadInto
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public double FileSize { get; set; }
        /// <summary>
        /// //文件类型
        /// </summary>
        public string FileTypeId { get; set; }
       
    }



    public class PageInfoItem
    {
        /// <summary>
        /// 哪一页
        /// </summary>
        public string page { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public string width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public string height { get; set; }
        /// <summary>
        /// 图幅名
        /// </summary>
        public string frameName { get; set; }
        /// <summary>
        /// 折合A1
        /// </summary>
        public string folded { get; set; }
    }
}
