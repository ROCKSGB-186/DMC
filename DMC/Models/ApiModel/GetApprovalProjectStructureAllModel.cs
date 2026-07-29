namespace DMC.Models
{
    /// <summary>
    /// 获得审批项目文件汇总信息:/1:FoldedAll,折A1数量 /2:FileAll，文件总数；
    /// </summary>
    public class GetApprovalProjectStructureAllModel
    {
        /// <summary>
        /// 折A1
        /// </summary>
        public double FoldedAll { get; set; }
        /// <summary>
        /// 文件总数
        /// </summary>
        public int FileAll { get; set; }
    }
}
