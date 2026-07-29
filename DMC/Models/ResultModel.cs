namespace DMC.Models
{
    /// <summary>
    /// 结果类:1:msg 返回的文字描述/ 2:code 状态码200成功/ 3:data 返回的数据/ 4:total 条数
    /// </summary>
    public class ResultModel<T>
    {
        /// <summary>
        ///1:msg 返回的文字描述
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 2:code 状态码200成功
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 3:data 返回的数据
        /// </summary>
        public T data { get; set; }
        /// <summary>
        /// 4:total 条数
        /// </summary>
        public int total { get; set; }
    }
}
