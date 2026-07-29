namespace DMC.Models
{
    /// <summary>
    /// 用户配置实体类 1:Name 用户名/2：PassWord 密码
    /// </summary>
    public class UserConfigModel
    {
        /// <summary>
        ///1：Name 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 2：Password 密码
        /// </summary>
        public string PassWord { get; set; }
    }
}
