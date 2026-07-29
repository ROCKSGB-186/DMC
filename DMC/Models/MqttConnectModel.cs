namespace DMC.Models
{
    /// <summary>
    /// Mqtt中心：/1：ServerUrl 服务器IP/2：Port 服务器端口/3：ClientId 客户端ID/4:Password 选项 - 开启登录 - 密码/5:UserName 选项 - 开启登录 - 用户名
    /// </summary>
    public class MqttConnectModel
    {
        /// <summary>
        ///1：ServerUrl 服务器IP
        /// </summary>
        public string ServerUrl { get; set; }
        /// <summary>
        ///2：Port 服务器端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        ///3：ClientId 客户端ID
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        ///4:Password 选项 - 开启登录 - 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        ///5:UserName 选项 - 开启登录 - 用户名
        /// </summary>
        public string UserName { get; set; }
    }
}
