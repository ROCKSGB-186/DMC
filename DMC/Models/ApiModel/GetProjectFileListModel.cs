namespace DMC.Models
{
    /// <summary>
    /// 获得项目文件列表：1：createTime 创建时间/ 2：updateTime 更新上传时间/ 3：id 主键/ 4：name 名称/ 5：userId 上传人ID/ 6：parentId 父级ID/ 7：projectId 项目id/ 8：frameName 图幅名称/ 9：folded 折合A1/ 10：pageAll 总页数/ 11：fileTypeId 文件类型/ 12：filePath 文件路径/ 13：userName 上传人账号/ 14：realName 上传人姓名/ 15：fileTypeName 文件类型名称/ 16：majorName 专业名称/ 17：isCheck 是否选择/  18：processtypeName 流程类型名称/ 19：ancestors 记录文件在数据库里的路径/ 20：type 类型/ 21：fileId 文件类型id/ 22：status 状态（0正常 1停用 2未发布 3删除 4迭代）/ 23 is_show 是否可在客户端显示 0显示 1不显示
    public class GetProjectFileListModel
    {
        /// <summary>
        /// 1：createTime 创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 2：updateTime 更新上传时间
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 3：id 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 4：name 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 5：userId 上传人ID
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 6：parentId 父级ID
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 7：projectId 项目id
        /// </summary>
        public string projectId { get; set; }
        /// <summary>
        /// 8：frameName 图幅名称
        /// </summary>
        public string frameName { get; set; }
        /// <summary>
        /// 9：folded 折合A1
        /// </summary>
        public string folded { get; set; }
        /// <summary>
        /// 10：pageAll 总页数
        /// </summary>
        public string pageAll { get; set; }
        /// <summary>
        /// 11：fileTypeId 文件类型
        /// </summary>
        public string fileTypeId { get; set; }
        /// <summary>
        /// 12：filePath 文件路径
        /// </summary>
        public string filePath { get; set; }
        /// <summary>
        /// 13：userName 上传人账号
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 14：realName 上传人姓名
        /// </summary>
        public string realName { get; set; }
        /// <summary>
        /// 15：fileTypeName 文件类型名称
        /// </summary>
        public string fileTypeName { get; set; }
        /// <summary>
        /// 16：majorName 专业名称
        /// </summary>
        public string majorName { get; set; }
        /// <summary>
        /// 17：isCheck 是否选择
        /// </summary>
        public bool isCheck { get; set; }
        /// <summary>
        ///  18：processtypeName 流程类型名称
        /// </summary>
        public string processtypeName { get; set; }
        /// <summary>
        /// 19：ancestors 记录文件在数据库里的路径
        /// </summary>
        public string ancestors { get; set; }
        /// <summary>
        /// 20：type 类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 21：fileId 文件类型id
        /// </summary>
        public int fileId { get; set; }
        /// <summary>
        /// 22：status 状态（0正常 1停用 2未发布 3删除 4迭代）
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 23 is_show 是否可在客户端显示 0显示 1不显示
        /// </summary>
        public int is_show { get; set; }
    }
}
