using DMC.Helper;
using DMC.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Drawing.Text;

namespace DMC
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public partial class FrmSystemSettings : BaseForm
    {
        //private List<SystemSettingsModel> systemSettingsModel = null;
        
        public FrmSystemSettings()
        {
            InitializeComponent();
        }

        private void FrmSystemSettings_Load(object sender, EventArgs e)
        {
            var systemSettings = ConfigurationManager.AppSettings["SystemSettings"];
            //判断是不是加载数据为空；
            if (systemSettings != "")
            {
                //读取保存的系统设置；
              
                var systemSettingsModel = JsonConvert.DeserializeObject<SystemSettingsModel>(systemSettings);
                textBox1.Text = systemSettingsModel.ServiceAddress;
                textBox2.Text = systemSettingsModel.ServiceProt.ToString();
                textBox3.Text = systemSettingsModel.MqttServiceAddress;
                textBox4.Text = systemSettingsModel.MqttServiceProt.ToString();
            }
            //var systemSettings = ConfigurationManager.AppSettings["SystemSettings"];

            ////判断是不是加载数据为空；
            //if (systemSettings != "")
            //{
            //    //读取保存的系统设置,序列化字段后赋值给systemSettingsModel
            //    var systemSettingsModel = JsonConvert.DeserializeObject<SystemSettingsModel>(systemSettings);
            //    //赋值给系统IP
            //    AppGlobalModel.ServiceAddress = systemSettingsModel.ServiceAddress;
            //    //系统端口
            //    AppGlobalModel.ServiceProt = systemSettingsModel.ServiceProt;
            //    //消息IP
            //    AppGlobalModel.MqttServiceAddress = systemSettingsModel.MqttServiceAddress;
            //    //消息端口
            //    AppGlobalModel.MqttServiceProt = systemSettingsModel.MqttServiceProt;
            //}
            //textBox1.Text = AppGlobalModel.ServiceAddress;
            //textBox2.Text = AppGlobalModel.ServiceProt == 0 ? "" : AppGlobalModel.ServiceProt.ToString();
            //textBox3.Text = AppGlobalModel.MqttServiceAddress;
            //textBox4.Text = AppGlobalModel.MqttServiceProt == 0 ? "" : AppGlobalModel.MqttServiceProt.ToString();
            //checkBox1.Checked = AppGlobalModel.StartupAutomatically;
            this.Size = new System.Drawing.Size(800, 460);
        }
        #region 简化方法 窗体移动,直接变化Left、Top
        private Point originLocation;

        private void WindowMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                #region 通过Left、Top计算直接+=变化即可
                Left += e.Location.X - originLocation.X;
                Top += e.Location.Y - originLocation.Y;
                #endregion
            }
        }

        private void WindowMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originLocation = e.Location;
            }
        }
        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
          private void button1_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text.Trim()))
            {
                ShowErrorMsg("请输入服务器IP！");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text.Trim()))
            {
                ShowErrorMsg("请输入服务端口！");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox3.Text.Trim()))
            {
                ShowErrorMsg("请输入消息服务器IP！");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox4.Text.Trim()))
            {
                ShowErrorMsg("请输入消息服务端口！");
                return;
            }

            var settingInfo = new SystemSettingsModel();
            settingInfo.ServiceAddress = textBox1.Text.Trim();
            if (textBox2.Text.Trim() == null) textBox2.Text = "8888";
            settingInfo.ServiceProt = Convert.ToInt32(textBox2.Text.Trim());
            settingInfo.MqttServiceAddress = textBox3.Text.Trim();
            if (textBox4.Text.Trim() == null) textBox4.Text = "1884";
            settingInfo.MqttServiceProt = Convert.ToInt32(textBox4.Text.Trim());
            settingInfo.StartupAutomatically = checkBox1.Checked;
            IPAddress IP;
            bool flag = IPAddress.TryParse(settingInfo.ServiceAddress, out IP);

            if (!flag)
            {
                ShowErrorMsg("请输入正确的服务器IP！");
                return;
            }

            flag = IPAddress.TryParse(settingInfo.MqttServiceAddress, out IP);

            if (!flag)
            {
                ShowErrorMsg("请输入正确的消息服务器IP！");
                return;
            }

            try
            {
                string execPath = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
                if (checkBox1.Checked)
                {
                    rk2.SetValue("DmcExec", execPath);
                    LogHelper.WriteLocalLog(this, string.Format("[注册表操作]添加注册表键值：path = {0}, key = {1}, value = {2} 成功", rk2.Name, "TuniuAutoboot", execPath));
                }
                else
                {
                    rk2.DeleteValue("DmcExec", false);
                    LogHelper.WriteLocalLog(this, string.Format("[注册表操作]删除注册表键值：path = {0}, key = {1} 成功", rk2.Name, "TuniuAutoboot"));
                }
                rk2.Close();
                rk.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMsg(string.Format("[注册表操作]向注册表写开机启动信息失败, Exception: {0}", ex.Message));
                return;
            }

            ConfigHelper.SaveConfigInfo("SystemSettings", JsonConvert.SerializeObject(settingInfo));
            AppGlobalModel.ServiceAddress = settingInfo.ServiceAddress;
            AppGlobalModel.ServiceProt = settingInfo.ServiceProt;
            AppGlobalModel.MqttServiceAddress = settingInfo.MqttServiceAddress;
            AppGlobalModel.MqttServiceProt = settingInfo.MqttServiceProt;
            AppGlobalModel.StartupAutomatically = settingInfo.StartupAutomatically;

            DialogResult = DialogResult.OK;
        }
        

        private void button2_Click_1(object sender, EventArgs e)
        {
            // 验证用户输入
            if (string.IsNullOrWhiteSpace(textBox1.Text.Trim()))
            {
                ShowErrorMsg("请输入服务器IP！");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text.Trim()))
            {
                ShowErrorMsg("请输入服务端口！");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox3.Text.Trim()))
            {
                ShowErrorMsg("请输入消息服务器IP！");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox4.Text.Trim()))
            {
                ShowErrorMsg("请输入消息服务端口！");
                return;
            }

            // 初始化设置对象
            var settingInfo = new SystemSettingsModel
            {
                ServiceAddress = textBox1.Text.Trim(),
                ServiceProt = string.IsNullOrEmpty(textBox2.Text.Trim()) ? 8888 : int.Parse(textBox2.Text.Trim()),
                MqttServiceAddress = textBox3.Text.Trim(),
                MqttServiceProt = string.IsNullOrEmpty(textBox4.Text.Trim()) ? 1884 : int.Parse(textBox4.Text.Trim()),
                StartupAutomatically = checkBox1.Checked
            };

            // 验证IP格式
            if (!IPAddress.TryParse(settingInfo.ServiceAddress, out _))
            {
                ShowErrorMsg("请输入正确的服务器IP！");
                return;
            }

            if (!IPAddress.TryParse(settingInfo.MqttServiceAddress, out _))
            {
                ShowErrorMsg("请输入正确的消息服务器IP！");
                return;
            }

            // 注册表操作
            try
            {
                ManageRegistrySetting(settingInfo.StartupAutomatically);
            }
            catch (Exception ex)
            {
                ShowErrorMsg($"[注册表操作]向注册表写开机启动信息失败, Exception: {ex.Message}");
                return;
            }

            // 保存配置到文件
            try
            {
                ConfigHelper.SaveConfigInfo("SystemSettings", JsonConvert.SerializeObject(settingInfo));
            }
            catch (Exception ex)
            {
                ShowErrorMsg($"配置保存失败: {ex.Message}");
                return;
            }

            // 更新全局设置
            UpdateGlobalSettings(settingInfo);

            // 关闭对话框
            DialogResult = DialogResult.OK;
        }
        /// <summary>
        /// 点自动启动的复选框远行，加入注册表
        /// </summary>
        /// <param name="startupAutomatically"></param>
        private void ManageRegistrySetting(bool startupAutomatically)
        {
            string execPath = Application.ExecutablePath;
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if (startupAutomatically)
                {
                    rk.SetValue("DmcExec", execPath);
                    LogHelper.WriteLocalLog(this, $"[注册表操作]添加注册表键值：path = {rk.Name}, key = DmcExec, value = {execPath} 成功");
                }
                else
                {
                    rk.DeleteValue("DmcExec", false);
                    LogHelper.WriteLocalLog(this, $"[注册表操作]删除注册表键值：path = {rk.Name}, key = DmcExec 成功");
                }
            }
            
        }
        /// <summary>
        /// 更新服务器地址
        /// </summary>
        /// <param name="settings"></param>
        private void UpdateGlobalSettings(SystemSettingsModel settings)
        {
            AppGlobalModel.ServiceAddress = settings.ServiceAddress;
            AppGlobalModel.ServiceProt = settings.ServiceProt;
            AppGlobalModel.MqttServiceAddress = settings.MqttServiceAddress;
            AppGlobalModel.MqttServiceProt = settings.MqttServiceProt;
            AppGlobalModel.StartupAutomatically = settings.StartupAutomatically;
            
        }
        /// <summary>
        /// 输入IP时的判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键 
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数 
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符 
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void textBox三方ip_TextChanged(object sender, EventArgs e)
        {
            string textBoxIP=textBox三方ip.Text;
            string textBoxPort=textBox三方port.Text;
        }
        //private string oldSize = Size.ToString();
        private void btn向下_Click(object sender, EventArgs e)
        {
            string oldSize=this.Size.ToString();//800,460
            //"{Width=800,Height=450}"
            //panel三方.Visible = true;
            if (this.Size.ToString() == oldSize)
            {
                this.Size = new Size(800, 600);
               //oldSize = this.Size.ToString();
            }
            if(this.Size.ToString() == oldSize)
            {
                this.Size = new Size(800, 460);
            }
        }

       
    }
    


}
