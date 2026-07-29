namespace DMC.Models
{
    /// <summary>
    /// 获取项目类型列表:/1:dictLabel 名称/ 2：dictValue 主键
    /// </summary>
    public class GetProTypeListModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string dictLabel { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string dictValue { get; set; }
    }
}
