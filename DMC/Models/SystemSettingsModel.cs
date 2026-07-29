namespace DMC.Models
{
    /// <summary>
    /// 系统设置：/ 1：ServiceAddress 服务地址/ 2：ServiceProt 服务端口/ 3：MqttServiceAddress MQTT服务地址/ 4：MqttServiceProt 消息服务端口/ 5：StartupAutomatically 开机自启动
    /// </summary>
    public class SystemSettingsModel
    {
        /// <summary>
        /// 1：ServiceAddress 服务地址
        /// </summary>
        public string ServiceAddress { get; set; }
        /// <summary>
        /// 2：ServiceProt 服务端口
        /// </summary>
        public int ServiceProt { get; set; }
        /// <summary>
        /// 3：MqttServiceAddress MQTT服务地址
        /// </summary>
        public string MqttServiceAddress { get; set; }
        /// <summary>
        /// 4：MqttServiceProt 消息服务端口
        /// </summary>
        public int MqttServiceProt { get; set; }
        /// <summary>
        /// 5：StartupAutomatically 开机自启动
        /// </summary>
        public bool StartupAutomatically { get; set; }
    }
}
