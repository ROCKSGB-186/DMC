namespace DMC.Models
{
    /// <summary>
    /// 整形的KeyValue模型：1：KeyValueIntModel 构造函数/ 2：Key 键/ 3：Value 值
    /// </summary>
    public class KeyValueIntModel
    {
        /// <summary>
        /// 1：KeyValueIntModel 构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public KeyValueIntModel(int key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
        /// <summary>
        /// 2：Key 键
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// 3：Value 值
        /// </summary>
        public string Value { get; set; }
    }
}
