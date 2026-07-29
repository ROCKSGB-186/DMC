using DMC.Models;
using System;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 出版审批结果提交
    /// </summary>
    public partial class FrmPublishedResult : BaseForm
    {
        private string nodeInfoId = null;
        private string applyInfoId = null;
        public FrmPublishedResult(string applyId, string objId)
        {
            InitializeComponent();

            applyInfoId = applyId;
            nodeInfoId = objId;
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

        /// <summary>
        /// 拒绝
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text.Trim()))
            {
                ShowErrorMsg("请填写您的审批意见！");
            }
            else
            {
                var param = new
                {
                    applyNodeId = nodeInfoId,//节点id
                    result = 1, //1通过 -1不通过(出版的时候 -1下载 1完成)
                    title = textBox1.Text.Trim()
                };

                var resultData = string.Empty;
                if (HttpPost(AppGlobalModel.ApprovalResult, param, ref resultData))
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}