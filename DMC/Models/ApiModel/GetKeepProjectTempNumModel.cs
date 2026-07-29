namespace DMC.Models
{
    /// <summary>
    /// 获得临时外审图纸数量
    /// </summary>
    public class GetKeepProjectTempNumModel
    {
        /// <summary>
        /// 折A1
        /// </summary>
        public double foldedNum { get; set; }
        /// <summary>
        /// 文件总数
        /// </summary>
        public int fileNum { get; set; }
    }
}
