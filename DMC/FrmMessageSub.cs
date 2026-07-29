using DMC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace DMC
{
    public partial class FrmMessageSub : BaseForm
    {
        /// <summary>  
        /// 消息内容  
        /// </summary>  
        private MyMessageModel myMessage;

        /// <summary>  
        /// 关闭窗口事件  
        /// </summary>  
        public event Action<FrmMessageSub> OnClose;

        /// <summary>  
        /// 窗口初始化  
        /// </summary>  
        /// <param name="obj">消息对象</param>  
        public FrmMessageSub(MyMessageModel message)
        {
            InitializeComponent();
            myMessage = message; // 存储消息内容  
        }

        /// <summary>  
        /// 窗口加载时展示消息内容  
        /// </summary>  
        private void FrmMessageSub_Load(object sender, EventArgs e)
        {
            richTextBox_Message.Text = $"消息时间：{myMessage.createTime} \r\n消息内容：{myMessage.content.Replace("\r\n", "")}"; // 设置消息内容  
            label_流程标题.Text = myMessage.title; // 设置流程标题  
            label_发起人.Text = myMessage.userName; // 设置发起人  
            label_项目名称.Text = myMessage.proName; // 设置项目名称  
            if (myMessage.jumpId == null)
            {
                button_link.Visible = false;
            }
        }
        /// <summary>  
        /// 已读按钮点击事件  
        /// </summary>  
        private void button_已读_Click(object sender, EventArgs e)
        {
            var param = new { id = myMessage.id }; // 获取消息ID  

            var resultData = new MyMessageModel();
            // 发送已读请求  
            if (HttpPost(AppGlobalModel.ReadedMessage, param, ref resultData))
            {
                OnClose?.Invoke(this); // 触发关闭事件  
                this.Close(); // 关闭子窗口  
            }
        }
       /// <summary>
       /// 流程链接
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void button_link_Click(object sender, EventArgs e)
        {
            Form frm = null;
            if (myMessage.jumpType == "liucheng")
            {
                frm = new FrmApprovalInfo(myMessage.jumpId);
            }
            else if (myMessage.jumpType == "fabuxiangmu")
            {
                frm = new FrmProjectEdit(null, myMessage.jumpId);
            }
            else
            {
                ShowErrorMsg("没有此消息跳转类型！");
                return;
            }
            // 获取拥有者：优先使用父窗体的拥有者，如果没有则使用当前窗体的拥有者（如果当前窗体是顶级窗体则为 null）
            var owner = this.Parent?.FindForm() ?? this.TopLevelControl as Form;
            //this.Hide();

            DialogResult result;
            if (owner != null)
            {
                frm.Owner = owner;
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.TopMost = true;
                result = frm.ShowDialog(owner);
            }
            else
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.TopMost = true;
                result = frm.ShowDialog();
            }

            // 只有审批窗口明确返回“已完成”（由 FrmApprovalInfo 设置 DialogResult=OK）时才关闭并标记已读
            if (result == DialogResult.OK)
            {
                OnClose?.Invoke(this);
                try { this.Close(); } catch { }

                _ = Task.Run(() =>
                {
                    var param = new { id = myMessage.id };
                    var resultData = new MyMessageModel();
                    try { HttpPost(AppGlobalModel.ReadedMessage, param, ref resultData); }
                    catch { /* 网络失败不影响 UI */ }
                });
            }
            // 否则保持子窗体显示，不做已读操作
        }
    }
}
