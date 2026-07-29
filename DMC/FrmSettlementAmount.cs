using DMC.Models;
using System;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 出版完成结算金额
    /// </summary>
    public partial class FrmSettlementAmount : BaseForm
    {
        private string applyId = null;
        public FrmSettlementAmount(string objStr)
        {
            InitializeComponent();
            applyId = objStr;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            var money = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(money))
            {
                ShowErrorMsg("请输入结算金额！");
                return;
            }

            var para = new
            {
                applyId = applyId,
                money = money
            };

            var resultData = string.Empty;
            if (HttpPost(AppGlobalModel.ApprovalChubanPass, para, ref resultData))
            {
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
