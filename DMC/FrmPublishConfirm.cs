using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 出版确认界面
    /// </summary>
    public partial class FrmPublishConfirm : BaseForm
    {
        private ConfirmInfoModel modelInfo = null;

        public FrmPublishConfirm(object obj)
        {
            InitializeComponent();

            modelInfo = obj as ConfirmInfoModel;

            textBox2.Text = modelInfo.projectName;
            textBox3.Text = modelInfo.realName;
            textBox7.Text = modelInfo.annex_id;

            #region 出版

            Panel panel;
            TextBox textBox;
            Label label;
            ListView listView;

            var index = 0;
            foreach (var item in modelInfo.applynodeList)
            {
                panel = new Panel();
                textBox = new TextBox();
                label = new Label();
                listView = new ListView();

                textBox.Location = new Point(24, 30);
                textBox.Multiline = true;
                textBox.Size = new Size(600, 84);
                textBox.Name = $"textBox_{item.node_name}";
                textBox.Text = item.remark;
                textBox.Enabled = false;

                listView.Location = new Point(630, 8);
                listView.Columns.Add("审批人");
                listView.Columns[0].Width = 110;
                listView.Font = new Font("宋体", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                listView.FullRowSelect = true;
                listView.HideSelection = false;
                listView.UseCompatibleStateImageBehavior = false;
                listView.Name = "listView_selectList";
                listView.View = View.Details;
                listView.Size = new Size(140, 106);

                foreach (var user in item.userList)
                {
                    ListViewItem lineLeft = new ListViewItem(user.realName);
                    listView.Items.Add(lineLeft);
                }

                label.AutoSize = true;
                label.Location = new Point(24, 3);
                label.Size = new Size(110, 25);
                label.Text = $"{item.node_name}";
                label.Tag = item;

                panel.Controls.Add(textBox);
                panel.Controls.Add(listView);
                panel.Controls.Add(label);
                panel.Dock = DockStyle.Top;
                panel.Location = new Point(3, (index * 125) + 3);
                panel.Size = new Size(786, 125);

                panel3.Controls.Add(panel);
                panel3.Controls.SetChildIndex(panel, 0);
                index++;
            }
            #endregion
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
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }

    public class ConfirmInfoModel
    {
        public string projectName { get; set; }
        public string realName { get; set; }
        public List<ApplynodeInfo> applynodeList { get; set; }
        public string annex_id { get; set; }
    }
}
