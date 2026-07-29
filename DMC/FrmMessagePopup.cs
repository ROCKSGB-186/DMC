using DMC.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Forms;
using Control = System.Windows.Forms.Control;

namespace DMC
{

    /// <summary>
    /// 消息弹窗
    /// </summary>
    public partial class FrmMessagePopup : BaseForm
    {
        /// <summary>
        /// 引入Gdi32.dll中的CreateRoundRectRgn函数，用于创建圆角矩形区域
        /// </summary>
        /// <param name="nLeftRect">左</param>
        /// <param name="nTopRect">上</param>
        /// <param name="RightRect">右</param>
        /// <param name="nBottonRect">按键</param>
        /// <param name="nWidthEllipse">宽</param>
        /// <param name="nHeightEllipse">高</param>
        /// <returns></returns>
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int RightRect, int nBottonRect, int nWidthEllipse, int nHeightEllipse);

        #region 新方法二
        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
        #endregion

        /// <summary>
        /// 引入user32.dll中的AnimateWindow函数，用于动画显示窗口
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="dwTime"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        /// <summary>
        /// 存储消息面板的列表
        /// </summary>
        private List<System.Windows.Forms.Panel> messagePanels = new List<System.Windows.Forms.Panel>();
        #region 实现窗口细节参数
        //下面是可用的常量,按照不合的动画结果声明本身须要的
        private const int AW_HOR_POSITIVE = 0x0001;//自左向右显示窗口,该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记
        private const int AW_HOR_NEGATIVE = 0x0002;//自右向左显示窗口,该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记
        private const int AW_VER_POSITIVE = 0x0004;//自顶向下显示窗口,该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记
        private const int AW_VER_NEGATIVE = 0x0008;//自下向上显示窗口,该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记该标记
        private const int AW_CENTER = 0x0010;//若应用了AW_HIDE标记,则使窗口向内重叠;不然向外扩大
        private const int AW_HIDE = 0x10000;//隐蔽窗口
        private const int AW_ACTIVE = 0x20000;//激活窗口,在应用了AW_HIDE标记后不要应用这个标记
        private const int AW_SLIDE = 0x40000;//应用滑动类型动画结果,默认为迁移转变动画类型,当应用AW_CENTER标记时,这个标记就被忽视
        private const int AW_BLEND = 0x80000;//应用淡入淡出结果
        #endregion
        /// <summary>
        /// 最大可视消息数  
        /// </summary>
        private const int MaxVisibleMessages = 3;

        /// <summary>
        /// 窗口初始化
        /// </summary>
        public FrmMessagePopup()
        {
            InitializeComponent(); // 初始化组件
            SetupScrollableContainer(); // 设置滚动容器
            // 设置窗口圆角
            //Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            // 正确处理GDI资源  
            IntPtr regionHandle = CreateRoundRectRgn(0, 0, Width, Height, 25, 25);
            Region = Region.FromHrgn(regionHandle);
            DeleteObject(regionHandle); // 释放原始区域句柄  
            TopMost = true; // 窗口置顶
        }
        /// <summary>
        /// 主窗体自动加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMessagePopup_Load(object sender, EventArgs e)
        {
            // 计算窗口位置，使其显示在屏幕右下角
            int x = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
            this.Location = new Point(x, y);//设置窗体在屏幕右下角显示
            // 使用动画显示窗口
            AnimateWindow(this.Handle, 500, AW_SLIDE | AW_ACTIVE | AW_VER_NEGATIVE);
        }
        /// <summary>
        /// 设置垂直滚动条
        /// </summary>
        private void SetupScrollableContainer()
        {
            panel_AllMessage.AutoScroll = true;// 启用自动滚动
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="newMessage">消息内容</param>
        public void AddMessage(MyMessageModel newMessage)
        {
            // 如果需要在UI线程上执行，则使用BeginInvoke
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)(() => AddMessage(newMessage)));
                return;
            }
            // 创建一个新的面板用于显示消息
            var messagePanel = new System.Windows.Forms.Panel
            {
                Dock = DockStyle.Top, // 停靠在顶部
                Height = 100 // 设置面板高度
            };
         
            // 创建消息子窗体
            var messageSubForm = new FrmMessageSub(newMessage)
            {
                TopLevel = false,// 设置为非顶级窗体
                Dock = DockStyle.Fill, // 填充面板
                FormBorderStyle = FormBorderStyle.None, // 无边框
                Width = panel_AllMessage.Width - 20, // 设置宽度
            };
            
            messagePanel.Controls.Add(messageSubForm); // 添加子窗体到面板
            messageSubForm.Show(); // 显示子窗体
            panel_AllMessage.Controls.Add(messagePanel); // 添加面板到消息容器
            panel_AllMessage.Controls.SetChildIndex(messagePanel, 0); // 设置面板索引
            messagePanels.Insert(0, messagePanel); // 插入面板到列表
            AdjustLayout(); // 调整布局
            if (messagePanels.Count == 1)
            { Show(); } // 如果是第一条消息，显示窗口

            //messageSubForm.OnClose += (sender) => CloseSubForm(messagePanel); // 关闭子窗体时的事件
            // 使用lambda表达式处理子窗体的关闭事件，确保在UI线程上执行CloseSubForm方法
            messageSubForm.OnClose += (sender) =>
            {
                BeginInvoke(new Action(() => CloseSubForm(messagePanel)));
            };
        }
        /// <summary>
        /// 调整窗口
        /// </summary>
        private void AdjustLayout()
        {
            if (messagePanels.Count == 0) return; // 如果没有子窗口，直接返回 
            //取消息容器的数量与最大消息显示数的最小值
            int visibleCount = Math.Min(messagePanels.Count, MaxVisibleMessages);
            //取消息高度值
            int height = panel_AllMessage.ClientSize.Height / visibleCount;
            // 如果消息数量超过MaxVisibleMessages，启用滚动条  
            panel_AllMessage.AutoScroll = messagePanels.Count > MaxVisibleMessages;
            // 设置每个面板的高度
            foreach (var panel in messagePanels)
            {
                panel.Height = height;
            }
            // 滚动到顶部
            panel_AllMessage.AutoScrollPosition = new Point(0, 0);
            // 滚动到顶部以显示最新消息  
            panel_AllMessage.VerticalScroll.Value = 0;
            panel_AllMessage.AutoScroll = true;
            panel_AllMessage.PerformLayout();// 重新布局
        }
       

        /// <summary>
        /// 关闭子窗体  
        /// </summary>
        /// <param name="messagePanel">子窗体</param>
        private void CloseSubForm(System.Windows.Forms.Panel messagePanel)
        {
            // 确保在UI线程上执行
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => CloseSubForm(messagePanel)));
                return;
            }

            // 从容器中移除面板并释放其子控件（包括嵌入的子窗体）以避免资源泄漏
            if (messagePanel != null)
            {
                // 先关闭并释放面板内的窗体或控件
                foreach (Control c in messagePanel.Controls)
                {
                    if (c is Form f)
                    {
                        try { f.Close(); } catch { }
                        try { f.Dispose(); } catch { }
                    }
                    else
                    {
                        try { c.Dispose(); } catch { }
                    }
                }

                panel_AllMessage.Controls.Remove(messagePanel);
                try { messagePanel.Dispose(); } catch { }
            }

            messagePanels.Remove(messagePanel);
            AdjustLayout(); // 调整布局

            // 额外判断容器中是否真没子面板，延迟一次关闭以避免时序问题
            if ((messagePanels == null || messagePanels.Count == 0) && panel_AllMessage.Controls.Count == 0)
            {
                try { Close(); } catch { }
                // 延迟到消息循环空闲时再关闭
                //BeginInvoke(new Action(() =>
                //{
                //    try { Close(); } catch { }
                //}));
            }
        }
        /// <summary>
        /// 关闭窗口时动画显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMessagePopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 使用动画隐藏窗口
            AnimateWindow(this.Handle, 0, AW_BLEND | AW_HIDE);
        }
    }
}