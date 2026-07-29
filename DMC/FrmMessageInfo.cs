using DMC.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 消息详情
    /// </summary>
    public partial class FrmMessageInfo : BaseForm
    {
        private MyMessageModel myMessage = null;
        public FrmMessageInfo(MyMessageModel objInfo)
        {
            InitializeComponent();

            myMessage = objInfo;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            var param = new
            {
                id = myMessage.id//消息id
            };

            var resultData = new MyMessageModel();
            if (HttpPost(AppGlobalModel.ReadedMessage, param, ref resultData))
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void FrmMessageInfo_Load(object sender, EventArgs e)
        {
            if (myMessage.isRead == 1)
            {
                //buttonOk.Visible = false;
                buttonOk.Enabled = false;
                //buttonCancel.Location = new Point(263, 351);
                buttonCancel.Visible = true;
            }
            else
            {
                //buttonCancel.Location = new Point(193, 351);
                buttonCancel.Visible = true;
            }

            textBox1.Text = myMessage.title;
            textBox2.Text = myMessage.proName;
            textBox3.Text = myMessage.createTime;
            textBox4.Text = myMessage.userName;
            textBox5.Text = myMessage.content;
            textBox6.Text = myMessage.readTime;


            if (string.IsNullOrWhiteSpace(myMessage.jumpId))
            {
                label7.Visible = false;
                linkLabel1.Visible = false;
            }
            else
            {
                label7.Visible = true;
                linkLabel1.Text = myMessage.jumpTitle;
                linkLabel1.Visible = true;
            }
        }

        /// <summary>
        /// 消息链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_Click(object sender, EventArgs e)
        {
            if (myMessage.jumpType == "liucheng")
            {
                var frm = new FrmApprovalInfo(myMessage.jumpId);
                this.Hide();
                frm.ShowDialog();
            }
            else if (myMessage.jumpType == "fabuxiangmu")
            {
                var frm = new FrmProjectEdit(null, myMessage.jumpId);
                this.Hide();
                frm.ShowDialog();
            }
            else
            {
                ShowErrorMsg("没有此消息跳转类型！");
            }
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

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
    }
}
