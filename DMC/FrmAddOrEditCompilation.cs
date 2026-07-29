using DMC.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 项目档案编研
    /// </summary>
    public partial class FrmAddOrEditCompilation : BaseForm
    {
        private string projectId = null;

        public FrmAddOrEditCompilation(string objStr)
        {
            InitializeComponent();

            projectId = objStr;
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


        private void FrmAddOrEditCompilation_Load(object sender, EventArgs e)
        {
            var resultModel = new GetCompilationModel();
            if (HttpGet(AppGlobalModel.GetCompilation + $"?projectId={projectId}", ref resultModel))
            {
                if (resultModel != null)
                {
                    textBox_archivesType.Text = resultModel.archivesType;
                    textBox_archivesNum.Text = resultModel.archivesNum;
                    textBox_blueprintCabinet.Text = resultModel.blueprintCabinet;
                    textBox_blueprintUser.Text = resultModel.blueprintUser;
                    textBox_materialCabinet.Text = resultModel.materialCabinet;
                    textBox_materialUser.Text = resultModel.materialUser;
                    textBox_fileType.Text = resultModel.fileType;
                    textBox_remarks.Text = resultModel.remarks;
                    textBox_records.Text = resultModel.records.ToString();
                    textBox_content.Text = resultModel.content;
                    dateTimePicker_formationTime.Text = resultModel.formationTime;
                    textBox_address.Text = resultModel.address;
                    textBox_safekeep.Text = resultModel.safekeep;
                    textBox_secrecy.Text = resultModel.secrecy;
                }
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            var para = new
            {
                projectId = projectId,
                archivesType = textBox_archivesType.Text,
                archivesNum = textBox_archivesNum.Text,
                blueprintCabinet = textBox_blueprintCabinet.Text,
                blueprintUser = textBox_blueprintUser.Text,
                materialCabinet = textBox_materialCabinet.Text,
                materialUser = textBox_materialUser.Text,
                fileType = textBox_fileType.Text,
                remarks = textBox_remarks.Text,
                records = textBox_records.Text,
                content = textBox_content.Text,
                formationTime = dateTimePicker_formationTime.Text.ToString(),
                address = textBox_address.Text,
                safekeep = textBox_safekeep.Text,
                secrecy = textBox_secrecy.Text
            };

            var resultData = string.Empty;

            if (HttpPost(AppGlobalModel.AddOrEditCompilation, para, ref resultData))
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void dateTimePicker_formationTime_ValueChanged(object sender, EventArgs e)
        {
            var dateTimePicker = (DateTimePicker)sender;
            //这里可以个更改自己的需要的格式，例：yyyy年MM月dd日
            dateTimePicker.CustomFormat = "yyyy-MM-dd";
        }

        private void textBox_records_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox_secrecy_TextChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void textBox_safekeep_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textBox_address_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox_content_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox_records_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox_remarks_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox_materialUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox_fileType_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox_materialCabinet_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
