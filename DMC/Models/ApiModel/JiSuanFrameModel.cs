namespace DMC.Models
{
    /// <summary>
    /// 获取图幅列表/ 1：id 主键/ 2：name 名称/ 3：maxH 最大高度/ 4：minH 最小高度/ 5：maxW 最大宽度/ 6：minX 最小宽度/ 7：folded 折合A1
    /// </summary>
    public class JiSuanFrameModel
    {
        /// <summary>
        /// 1：id 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 2：name 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 3：maxH 最大高度
        /// </summary>
        public int maxH { get; set; }
        /// <summary>
        /// 4：minH 最小高度
        /// </summary>
        public int minH { get; set; }
        /// <summary>
        /// 5：maxW 最大宽度
        /// </summary>
        public int maxW { get; set; }
        /// <summary>
        /// 6：minX 最小宽度
        /// </summary>
        public int minW { get; set; }
        /// <summary>
        /// 7：folded 折合A1
        /// </summary>
        public string folded { get; set; }
    }
}
