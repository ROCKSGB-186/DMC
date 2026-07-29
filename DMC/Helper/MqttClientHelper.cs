using DMC.Models;
using MQTTnet;
using MQTTnet.Client;

using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Publishing;
using MQTTnet.Client.Receiving;

using MQTTnet.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMC.Helper
{
    /// <summary>
    /// Mqtt客户端帮助类
    /// </summary>
    public class MqttClientHelper
    {
        public event Action<object, MqttApplicationMessage> CallbackMessageReceived = null;

        private IMqttClient mqttClient = null;
        private IMqttClientOptions options = null;
        public MqttConnectModel connectModel = null;
        /// <summary>
        /// 保留
        /// </summary>
        private bool Retained = false;
        /// <summary>
        /// 错误信息计数
        /// </summary>
        private Dictionary<string, int> dictErrorCount = null;
        /// <summary>
        /// 错误次数
        /// </summary>
        private int errorCount = 0;

        public MqttClientHelper()
        {
            dictErrorCount = new Dictionary<string, int>();
        }

        #region 连接
        /// <summary>
        /// 连接
        /// </summary>
        public void Connect(MqttConnectModel connectModel)
        {
            try
            {
                this.connectModel = connectModel;

                options = new MqttClientOptionsBuilder()
                    .WithTcpServer(connectModel.ServerUrl, connectModel.Port)
                    .WithCredentials(connectModel.UserName, connectModel.Password)
                    .WithClientId(connectModel.ClientId)
                    .WithCleanSession(false)
                    .Build();

                var factory = new MqttFactory();
                mqttClient = factory.CreateMqttClient();    
                mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(new Action<MqttClientConnectedEventArgs>(Connected));
                mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(new Action<MqttClientDisconnectedEventArgs>(Disconnected));
                mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(new Action<MqttApplicationMessageReceivedEventArgs>(MqttApplicationMessageReceived));
                MqttClientConnectResult result = mqttClient.ConnectAsync(options).GetAwaiter().GetResult();
                if (result.ResultCode == MqttClientConnectResultCode.Success)
                {
                    LogHelper.WriteLocalLog(this,$"初始连接成功：{JsonConvert.SerializeObject(connectModel)}", "MqttClientHelper");
                }
                else
                {
                    LogHelper.WriteLocalLog(this, $"初始连接失败：{result.ResultCode}，{JsonConvert.SerializeObject(connectModel)}", "MqttClientHelper");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalErrorLog(this, ex.Message, "MqttClientHelper");
            }
        }
        #endregion

        #region 连接状态
        /// <summary>
        /// 连接状态
        /// </summary>
        /// <returns></returns>
        public bool GetConnected()
        {
            return mqttClient.IsConnected;
        }
        #endregion

        #region 连接服务器并按标题订阅内容
        /// <summary>
        /// 连接服务器并按标题订阅内容
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private void Connected(MqttClientConnectedEventArgs e)
        {
            try
            {
                List<MqttTopicFilter> listTopic = new List<MqttTopicFilter>();
                if (!listTopic.Any())
                {
                    var item = new MqttTopicFilterBuilder()
                                   .WithTopic($"app/{AppGlobalModel.UseInfo.id}")
                                   .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                                   .WithExactlyOnceQoS()
                                   .Build();

                    listTopic.Add(item);
                }
                var result = mqttClient.SubscribeAsync(listTopic.ToArray()).GetAwaiter().GetResult();

                LogHelper.WriteLocalLog(this, $"订阅结果：{JsonConvert.SerializeObject(result)}", "MqttClientHelper");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalErrorLog(this, ex.Message, "MqttClientHelper");
            }
        }
        #endregion

        #region 失去连接触发事件
        /// <summary>
        /// 失去连接触发事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private void Disconnected(MqttClientDisconnectedEventArgs e)
        {
            try
            {
                if (e.ClientWasConnected && errorCount == 2)
                {
                    LogHelper.WriteLocalErrorLog(this, "已有客户端连接：" + connectModel.ClientId + "。异常信息：" + e.Exception, "MqttClientHelper");
                }
                else
                {
                    string logTxt = string.Format("客户端是否连接：{0}；客户端认证结果：{1}；异常信息：{2}", e.ClientWasConnected, JsonConvert.SerializeObject(e.ConnectResult), e.Exception);
                    WriteLog(logTxt);
                    //等待10秒重新连接
                    Task.Delay(TimeSpan.FromSeconds(3)).Wait();
                    mqttClient.ReconnectAsync().Wait();
                    if (mqttClient.IsConnected)
                    {
                        errorCount = 0;
                        WriteLog("连接成功：" + connectModel.ClientId, false);
                    }
                    else
                    {
                        errorCount++;
                        WriteLog("连接失败：" + connectModel.ClientId, false);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("出现异常：{0}", ex.Message));
            }
        }
        #endregion

        #region 接收消息触发事件
        /// <summary>
        /// 接收消息触发事件:3:时时接收消息第二步
        /// </summary>
        /// <param name="e"></param>
        private void MqttApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                //调用第三步
                CallbackMessageReceived?.Invoke(this, e.ApplicationMessage);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalErrorLog(this, ex.Message, "MqttClientHelper");
            }
        }
        #endregion

        #region 发布
        public bool Publish(string topic, string message)
        {
            try
            {
                if (mqttClient == null) { return false; }
                if (mqttClient.IsConnected == false)
                {
                    LogHelper.WriteLocalErrorLog(this, "连接失败：" + connectModel.ClientId, "MqttClientHelper");
                    return false;
                }
                else
                {
                    MqttApplicationMessageBuilder mamb = new MqttApplicationMessageBuilder()
                        .WithTopic(topic)
                        .WithPayload(message)
                        .WithRetainFlag(Retained)
                        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                        .WithExactlyOnceQoS();

                    var result = mqttClient.PublishAsync(mamb.Build()).GetAwaiter().GetResult();

                    if (result.ReasonCode == MqttClientPublishReasonCode.Success)
                    {
                        return true;
                    }
                    else
                    {
                        LogHelper.WriteLocalErrorLog(this, JsonConvert.SerializeObject(result), "MqttClientHelper");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalErrorLog(this, ex.Message, "MqttClientHelper");
                return false;
            }
        }
        #endregion

        #region 写入日志
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="isErrorLog">是否错误日志</param>
        public void WriteLog(string logContent, bool isErrorLog = true)
        {
            try
            {
                string singStr = logContent;
                if (dictErrorCount.ContainsKey(singStr))
                {
                    dictErrorCount[singStr] += 1;
                }
                else
                {
                    dictErrorCount.Add(singStr, 1);
                }
                if (dictErrorCount[singStr] > 5)
                {
                    dictErrorCount[singStr] = 0;
                    logContent = string.Format("5次，{0}", logContent);
                    if (isErrorLog)
                    {
                        LogHelper.WriteLocalErrorLog(this, logContent, "MqttClientHelper");
                    }
                    else
                    {
                        LogHelper.WriteLocalLog(this, logContent, "MqttClientHelper");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalErrorLog(this, ex.Message, "MqttClientHelper");
            }
        }
        #endregion
    }
}
