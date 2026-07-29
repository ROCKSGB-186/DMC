using DMC.Helper;
using DMC.Models;
using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections.Concurrent;
namespace DMC
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class FrmMian : BaseForm
    {
        #region 拉申窗口方法

        const int Guying_HTLEFT = 10;
        const int Guying_HTRIGHT = 11;
        const int Guying_HTTOP = 12;
        const int Guying_HTTOPLEFT = 13;
        const int Guying_HTTOPRIGHT = 14;
        const int Guying_HTBOTTOM = 15;
        const int Guying_HTBOTTOMLEFT = 0x10;
        const int Guying_HTBOTTOMRIGHT = 17;
        /// <summary>
        /// 窗口拉申方法
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
                        else m.Result = (IntPtr)Guying_HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)Guying_HTRIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)Guying_HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)Guying_HTBOTTOM;
                    break;
                case 0x0201:                //鼠标左键按下的消息 
                    m.Msg = 0x00A1;         //更改消息为非客户区按下鼠标 
                    m.LParam = IntPtr.Zero; //默认值 
                    m.WParam = new IntPtr(2);//鼠标放在标题栏内 
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

        #region 简化方法 窗体移动,直接变化Left、Top
        private Point originLocation;
        /// <summary>
        /// 窗口移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 对窗口点下鼠标键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originLocation = e.Location;
            }
        }
        #endregion

        /// <summary>
        /// Mqtt客户端帮助类
        /// </summary>
        private MqttClientHelper mqttClientHelper = null;

        /// <summary>
        /// 单独后台执行
        /// </summary>
        private BackgroundWorker backgroundWorker;

        /// <summary>
        /// 存储项目列表来自本地缓存临时数据； 
        /// </summary>
        private static List<ProjectResultModel> projectListTemp = new List<ProjectResultModel>();

        /// <summary>
        /// 主程序
        /// </summary>
        public FrmMian()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ButtonName = MainBtn;
            customizedesing();//折叠菜单
            openChildFrom(new FrmProTran(), "");
            btnSignatureTools.Visible = false;
            #region ADMIN按键开关
            string userName = Convert.ToString(AppGlobalModel.UseInfo.realName);
            if (userName == "管理员")
            { butADMIN.Visible = true; }
            else
            {
                panel系统设置.Size = new System.Drawing.Size(183, 260);
                //butADMIN.Visible = false;
            }
            #endregion
            #region 判断客户打开纬衡档案平台
       
            string urlVhsoft = AppGlobalModel.ServiceAddress.ToString();//取登录的Ip地址

            if (GlobalVariables.companyName == "辽宁方大工程设计有限公司")
            {
                panel档案库.Size = new Size(183, 130);
                button纬衡档案.Visible = true;
            }
            else if (GlobalVariables.companyName == "辽宁省建筑设计研究院有限责任公司")
            {
                panel档案库.Size = new Size(183, 130);
                button纬衡档案.Visible = false;
            }
            else if (GlobalVariables.companyName == "华商国际工程有限公司")
            {
                panel档案库.Size = new Size(183, 130);
                button纬衡档案.Visible = true;
            }
            else if (GlobalVariables.companyName == "吉林医药设计院有限公司")
            {
                panel档案库.Size = new Size(183, 130);
                button纬衡档案.Text = "";
                button纬衡档案.Enabled = false;
                this.pictureBox1.Image = global::DMC.Properties.Resources.Jlpdi_Logo;
                this.LogoText.Text = GlobalVariables.companyName;
                this.logoText1.Text = "Jilin Pharmaceutical Industry ";
                this.logoText1.Location = new Point(0, 103);
                this.logoText2.Text = "Design Institue Co., Ltd.";
            }
            else
            {
                //button纬衡档案.Visible = false;
                panel档案库.Size = new Size(183, 65);
            }
            if (GlobalVariables.userDeptName == null || !GlobalVariables.userDeptName.Contains("统计"))
            {
                button统计.Visible = false;
            }
            //||GlobalVariables.userDeptName.Contains("底图资料室")||GlobalVariables.userDeptName.Contains("技术质量管理部")
            #endregion

            /******************版本信息******************/
            Assembly assembly = Assembly.GetExecutingAssembly();
            //产品版本
            string versionStr = assembly.GetName().Version.ToString();
            label_time.Text = "启动时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            label_user.Text = $"系统版本：V{versionStr}       登录人：" + AppGlobalModel.UseInfo.realName + "    " + GlobalVariables.userDeptName;
            //定义后台执行方法
            backgroundWorker = new BackgroundWorker();
            //赋值后台执行的是什么任务
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            //SystemTempData.CreateEmptyJsonFile();
            初始化日志系统(versionStr, AppGlobalModel.UseInfo.realName, GlobalVariables.userDeptName);
        }

        /// <summary>
        /// 窗口启动加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMian_Load(object sender, EventArgs e)
        {
            if (mqttClientHelper == null)
            {
                mqttClientHelper = new MqttClientHelper();
                //存Mqtt的服务器信息和用户信息
                var mqttConnectModel = new MqttConnectModel();
                mqttConnectModel.ServerUrl = AppGlobalModel.MqttServiceAddress;
                mqttConnectModel.Port = AppGlobalModel.MqttServiceProt;
                mqttConnectModel.UserName = "";
                mqttConnectModel.Password = "";
                mqttConnectModel.ClientId = Guid.NewGuid().ToString();
                mqttClientHelper.Connect(mqttConnectModel);
                mqttClientHelper.CallbackMessageReceived += Mqtt_CallbackMessageReceived;
                SystemTempData.CreateEmptyJsonFile();//创建空的json文件
            }
            //启动后台加载未读消息
            backgroundWorker.RunWorkerAsync();
            //加载本地缓存的项目列表数据
            SystemTempData.LoadProjectListDataFromJson(ref projectListTemp);
          
            Thread thread = new Thread(() =>
            {
                Thread.Sleep(5000);
                if (GlobalVariables.userDeptName != null && GlobalVariables.userDeptName.Contains("统计"))
                {
                    //初始化本地数据库
                    SQLiteDataBase sQLiteDataBase = new SQLiteDataBase();
                }
                Thread.Sleep(5000);//
                if (GlobalVariables.userDeptName != null && GlobalVariables.userDeptName.Contains("统计"))
                {
                    //读MYsql内的项目列表
                    SystemTempData.Read_Mysql_ProjectListHttpDatas();
                    if (GlobalVariables.companyName == "辽宁方大工程设计有限公司")
                    {
                        //读SQLite内的用户列表
                        SystemTempData.JsonFileSQLiteUserList();
                    }
                    else
                    {
                        SystemTempData.JsonFileDeptUserList();
                    }
                    //if (GlobalVariables.companyName == "辽宁方大工程设计有限公司")
                    //{
                    //    //读SQLite内的用户列表
                    //    SystemTempData.JsonFileSQLiteUserList();
                    //}
                    //else
                    //{
                    //    SystemTempData.JsonFileDeptUserList();
                    //}
                }
                SystemTempData.JsonFileDeptUserList();
            });
            thread.Start();
        }

        /// <summary>
        /// 后台-加载未读消息:1：开机加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            #region 加载未读消息
            var queryMessage = new QueryMessage()
            {
                isRead = "0",    //是否已读
                pageNum = 1,  //页数
                pageSize = int.MaxValue,  //条数
            };
            var resultData = new List<MyMessageModel>();
            if (HttpPost(AppGlobalModel.MyMessage, queryMessage, ref resultData))
            {
                //后台运行
                foreach (var newMessageInfoItem in resultData.OrderBy(o => o.createTime))
                {
                    Thread thread = new Thread(new ParameterizedThreadStart(LoadMessage));  //第一步  创建线程对象  并把要交给线程执行的函数  通过参数传递给线程  
                    Thread.Sleep(500);//延迟0.5秒再执行
                    thread.Start(newMessageInfoItem);//开始执行
                    Thread.Sleep(500);
                }
            }
            #endregion
        }
      
        #region 新方法二
        private void Mqtt_CallbackMessageReceived(object obj, MqttApplicationMessage e)
        {
            // 在后台线程处理消息解析  
            Task.Run(() => {
                string payload;
                if (EncodingHelper.IsValidUTF8ByteArray(e.Payload))
                {
                    payload = Encoding.UTF8.GetString(e.Payload);
                }
                else
                {
                    payload = Encoding.GetEncoding("GB2312").GetString(e.Payload);
                }
                LogHelper.WriteLocalLog(this, payload, "Message");
                try
                {
                    var messageInfo = JsonConvert.DeserializeObject<MyMessageModel>(payload);
                    // 仅UI更新需要在UI线程  
                    this.BeginInvoke(new Action(() => { LoadMessage(messageInfo); }));
                }
                catch (JsonException ex)
                {
                    // 处理任何JSON反序列化错误  
                    LogHelper.WriteLocalLog(this, $"JSON 反序列化错误: {ex.Message}", "Error");
                    LogHelper.WriteLocalLog(this, payload, "Message");//写入日志
                }
                catch (Exception ex)
                {
                    // 处理可能发生的其他任何异常  
                    LogHelper.WriteLocalLog(this, $"在 Mqtt_CallbackMessageReceived 中出错: {ex.Message}", "Error");
                }
            });
        }
        #endregion

        /// <summary>
        /// 使用 LINQ 来简化检查特定窗体是否打开
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static bool IsFormOpen<T>() where T : Form
        {
            return Application.OpenForms.OfType<T>().Any();
        }

        /// <summary>
        /// 消息容器
        /// </summary>
        private FrmMessagePopup frmMessagePopup;


        #region 右下角弹出消息框并显示内容新方法
       
        private void LoadMessage(object messageInfo)
        {
            // 强制转换传入的消息  
            var newMessageInfoList = (MyMessageModel)messageInfo;
            // 模拟处理延迟  
            Thread.Sleep(3000);
            if (!IsFormOpen<FrmMessageSub>()) // 检查子窗体是否已经打开  
            {
                frmMessagePopup = new FrmMessagePopup();
                frmMessagePopup.FormClosed += (s, e) => frmMessagePopup = null; // 关闭时重置引用  
            }
            // 确保在UI线程上执行UI代码  
            this.Invoke((MethodInvoker)delegate
            {
                try
                {
                    frmMessagePopup.AddMessage(newMessageInfoList); // 将新消息添加到弹出窗口  
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLocalLog(this, $"在 LoadMessage 中出错: {ex.Message}", "Error");
                }
            });
        }

        #endregion


       

        #region 窗口相关
        /// <summary>
        /// 退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            DelTempFile();
            //关闭所有的线程
            this.notifyIconMain.Dispose();
            this.Dispose();
            Application.ExitThread();
            //关闭所有的线程
            Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// 双击鼠标按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIconMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                MinimizeForm();
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Maximized;
                this.Activate();
            }
        }

        /// <summary>
        /// 关闭键是最小化到时间栏里；
        /// </summary>
        private void MinimizeForm()
        {
            this.notifyIconMain.ShowBalloonTip(5, "提示", "系统仍在运行！\n如要显示窗体，请用鼠标双击图标\n或用鼠标右键点击图标选择菜单操作", ToolTipIcon.Info);
            this.WindowState = FormWindowState.Minimized;
            this.notifyIconMain.Visible = true;
            this.Hide();
        }

        /// <summary>
        /// 主窗口最小化按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMian_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            //最小化窗体
            MinimizeForm();
        }

        /// <summary>
        /// 最大化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMaxSide_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
                this.WindowState = FormWindowState.Maximized;
                Application.DoEvents();
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            MinimizeForm();
        }

        /// <summary>
        /// 最小化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMinSide_Click(object sender, EventArgs e)
        {
            MinimizeForm();
            //this.WindowState = FormWindowState.Minimized;
        }
        #endregion

        #region 左侧主菜单相关
        /// <summary>
        /// 定制窗口
        /// </summary>
        public void customizedesing()
        {
            panel项目.Visible = false;
            panel档案库.Visible = false;
            panel系统设置.Visible = false;

        }

        /// <summary>
        /// 自动隐藏二级菜单
        /// </summary>
        public void hideSubMenu()
        {
            if (panel项目.Visible == true) panel项目.Visible = false;
            if (panel档案库.Visible == true) panel档案库.Visible = false;
            if (panel系统设置.Visible == true) panel系统设置.Visible = false;
        }

        /// <summary>
        /// 显示子菜单方法
        /// </summary>
        /// <param name="submenu"></param>
        public void showSubMenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                hideSubMenu();
                submenu.Visible = true;
            }
            else
            {
                submenu.Visible = false;
            }
        }

        /// <summary>
        /// 点击按键后的按键颜色
        /// </summary>
        /// <param name="color">原按键颜色</param>
        public void customizebuttoncolor(Button color, Button Oldcolor)
        {
            Oldcolor.BackColor = Color.FromArgb(0, 71, 160);
            color.BackColor = Color.FromArgb(0, 120, 200);
        }

        /// <summary>
        /// 提示白条容器
        /// </summary>
        /// <param name="btn">输入点击按键Button</param>
        private void panelMoveSize(Control btn)
        {
            panelShowSelect.Top = btn.Top;
            panelShowSelect.Height = btn.Height;

        }

        private int tabCounter = 0;

        /// <summary>
        /// 检查 TabControl 中是否存在指定名 的 TabPage
        /// </summary>
        /// <param name="tabName">指定的页面名</param>
        /// <returns></returns>
        public bool TabPageExists(string tabName)
        {
            tabCounter = 0;
            foreach (TabPage tab in tabControl_mainWindows.TabPages)
            {
                if (tab.Text == tabName)
                {
                    return true; // 找到匹配的 TabPage，返回 true
                }
                tabCounter++;
            }
            return false; // 未找到匹配的 TabPage，返回 false
        }

        /// <summary>
        /// 在主容器内打开子窗口
        /// </summary>
        /// <param name="childForm">输入要打开的子窗口名称</param>
        private void openChildFrom(Form childForm, string buttonName)
        {
            if (buttonName == "") buttonName = "流程事务";
            if (!TabPageExists(buttonName))
            {
                // 创建新的 TabPage
                TabPage newTab = new TabPage(buttonName); // 每个新的 Tab 都有一个唯一的名称
                tabControl_mainWindows.TabPages.Add(newTab); // 将新的 Tab 添加到 TabControl
                // 使用 Panel 将子窗体内容嵌入到 TabPage 中
                System.Windows.Forms.Panel panel = new System.Windows.Forms.Panel();
                panel.Dock = DockStyle.Fill; // 填充整个 TabPage
                newTab.Controls.Add(panel); // 将 Panel 添加到 TabPage
                childForm.TopLevel = false; // 设置为非顶级窗体
                childForm.FormBorderStyle = FormBorderStyle.None; // 不使用边框
                childForm.Dock = DockStyle.Fill; // 填充 Panel
                panel.Controls.Add(childForm); // 将子窗体添加到 Panel
                childForm.Show(); // 显示子窗体       
                tabControl_mainWindows.SelectedTab = newTab;
            }
            else
            {
                tabControl_mainWindows.SelectedIndex = tabCounter;
            }
        }

        Button ButtonName = null;
        /// <summary>
        /// 主页点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainBtn_Click(object sender, EventArgs e)
        {
            panelMoveSize(MainBtn);

            customizebuttoncolor(MainBtn, ButtonName);
            ButtonName = MainBtn;
            openChildFrom(new FrmProTran(), "流程事务");
        }

        /// <summary>
        /// 流程事务点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button流程事务_Click(object sender, EventArgs e)
        {
            panelMoveSize(button流程事务);
            customizebuttoncolor(button流程事务, ButtonName);
            ButtonName = button流程事务;
            openChildFrom(new FrmProTran(), "流程事务");
        }

        /// <summary>
        /// 手动签名签章
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSignatureTools_Click(object sender, EventArgs e)
        {
            panelMoveSize(btnSignatureTools);

            customizebuttoncolor(btnSignatureTools, ButtonName);
            ButtonName = btnSignatureTools;
            openChildFrom(new FrmProTran(), "");

        }

        /// <summary>
        /// 点项目按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button项目_Click(object sender, EventArgs e)
        {
            customizebuttoncolor(button项目, ButtonName);
            ButtonName = button项目;
            showSubMenu(panel项目);
            panelMoveSize(button项目);
        }

        /// <summary>
        /// 点项目文件按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button项目文件_Click(object sender, EventArgs e)
        {
            customizebuttoncolor(button项目文件, ButtonName);
            ButtonName = button项目文件;
            openChildFrom(new FrmProjectFile(), "项目文件");

        }

        /// <summary>
        /// 点项目管理按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button项目管理_Click(object sender, EventArgs e)
        {
            customizebuttoncolor(button项目管理, ButtonName);
            ButtonName = button项目管理;
            openChildFrom(new FrmProject(), "项目管理");
        }

        /// <summary>
        /// 点消息管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button消息管理_Click(object sender, EventArgs e)
        {
            panelMoveSize(button消息管理);
            customizebuttoncolor(button消息管理, ButtonName);
            ButtonName = button消息管理;
            openChildFrom(new FrmMessageManage(), "消息管理");
        }

        /// <summary>
        /// 点档案管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button档案管理_Click(object sender, EventArgs e)
        {
            customizebuttoncolor(button档案管理, ButtonName);
            ButtonName = button档案管理;
            showSubMenu(panel档案库);
            panelMoveSize(button档案管理);
        }

        /// <summary>
        /// 点签章平台档案按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button签章平台档案_Click(object sender, EventArgs e)
        {
            customizebuttoncolor(button签章平台档案, ButtonName);
            ButtonName = button签章平台档案;
            openChildFrom(new FrmArchiveManage(), "档案管理");
        }

        private void button纬衡档案_Click(object sender, EventArgs e)
        {
            customizebuttoncolor(button纬衡档案, ButtonName);
            ButtonName = button纬衡档案;
            openChildFrom(new FrmWebVhsoft(), "纬衡档案");
        }

        private void button系统设置_Click(object sender, EventArgs e)
        {
            customizebuttoncolor(button系统设置, ButtonName);
            ButtonName = button系统设置;
            showSubMenu(panel系统设置);
            panelMoveSize(button系统设置);
        }

        private void button服务器与端口_Click(object sender, EventArgs e)
        {
            customizebuttoncolor(button服务器与端口, ButtonName);
            ButtonName = button服务器与端口;
            openChildFrom(new FrmSystemSettings(), "服务器与端口");
        }

        private void button修改密码_Click(object sender, EventArgs e)
        {
            customizebuttoncolor(button修改密码, ButtonName);
            ButtonName = button修改密码;
            //新独立窗口打开
            var frm = new FrmChangePassword();
            frm.ShowDialog();
        }

        private void button切换用户_Click(object sender, EventArgs e)
        {
            customizebuttoncolor(button切换用户, ButtonName);
            ButtonName = button切换用户;
            DelTempFile();
            //关闭所有的线程
            this.notifyIconMain.Dispose();
            this.Dispose();
            Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //关闭所有的线程
            Process.GetCurrentProcess().Kill();
        }

        private void button退出_Click(object sender, EventArgs e)
        {
            customizebuttoncolor(button退出, ButtonName);
            ButtonName = button退出;
            var result = MessageBox.Show("确定退出吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                DelTempFile();
                //关闭所有的线程
                this.notifyIconMain.Dispose();
                this.Dispose();
                Application.ExitThread();
                //关闭所有的线程
                Process.GetCurrentProcess().Kill();
            }
        }

        private void button统计_Click(object sender, EventArgs e)
        {
            panelMoveSize(button统计);
            customizebuttoncolor(button统计, ButtonName);
            ButtonName = button统计;
            openChildFrom(new FrmStatistics(), "统计");
        }

        private void ButADMIN_Click(object sender, EventArgs e)
        {
            customizebuttoncolor(butADMIN, ButtonName);
            ButtonName = butADMIN;
            openChildFrom(new FrmADMIN(), "ADMIN");
        }
        #endregion
       
    }

    /// <summary>
    /// 查询流程相关内容类型：1：processtypeid：流程类型：（0签名签章 1出版 2下载 3归档 4其他，不传就是查询所有）；2：type审批状态（0我发起的 1待审批 2已审批，不传就是查询所有）；3：pageNum 页数；4：pageSize 要查询的条数；5：proName 项目名称；6：userName 发起人；7：startTime 开始时间；8：endTime 结束时间 
    /// </summary>
    public class QueryApply
    {
        /// <summary>
        /// 1：processtypeId 0签名签章 1出版 2下载 3归档 4其他，不传就是查询所有
        /// </summary>
        public string processtypeId { get; set; }
        /// <summary>
        /// 2：type 0我发起的 1待审批 2已审批，不传就是查询所有
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 3：pageNum 页数
        /// </summary>
        public int pageNum { get; set; }
        /// <summary>
        /// 4：pageSize 要查询的条数
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 5：proName 项目名称
        /// </summary>
        public string proName { get; set; }
        /// <summary>
        /// 6：userName 发起人
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 7：startTime 开始时间
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 8：endTime 结束时间
        /// </summary>
        public string endTime { get; set; }
    }
    /// <summary>
    /// 全局变量GlobalVariables / 1:companyName 公司名称/ 2:companyName 公司Id/ 3:userName 用户名/ 4:userId 用户Id/ 5：userDeptName 用户部门名称/ 6:EndTime 结束时间/ 7：newProject 新项目
    /// </summary>
    public static class GlobalVariables
    {
        /// <summary>
        /// 1:companyName 公司名称
        /// </summary>
        public static string companyName { get; set; }
        /// <summary>
        /// 2:companyId 公司Id
        /// </summary>
        public static string companyId { get; set; }
        /// <summary>
        /// 3:userName 用户名
        /// </summary>
        public static string userName { get; set; }
        /// <summary>
        /// 4:userId 用户Id
        /// </summary>
        public static string userId { get; set; }
        /// <summary>
        /// 5：userDeptName 用户部门名称
        /// </summary>
        public static string userDeptName { get; set; }
        /// <summary>
        /// 6：EndTime 结束时间
        /// </summary>
        public static string endTime { get; set; }
        /// <summary>
        /// 7：newProject 新项目
        /// </summary>
        public static int newProject { get; set; }
    }

}