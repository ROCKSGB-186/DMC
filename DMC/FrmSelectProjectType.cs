using DMC.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 选择项目类型
    /// </summary>
    public partial class FrmSelectProjectType : BaseForm
    {
        private string deptId = null;
        public FrmSelectProjectType(string objId)
        {
            InitializeComponent();
            deptId = objId;
        }

        private void FrmSelectProjectType_Load(object sender, EventArgs e)
        {
            var proTypeList = new List<GetProTypeListModel>();
            if (HttpGet(AppGlobalModel.GetProTypeList, ref proTypeList))
            {
                comboBox1.DataSource = proTypeList;
                comboBox1.DisplayMember = "dictLabel";
                comboBox1.ValueMember = "dictValue";
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\desktop";
            openFileDialog.Filter = "所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string resultData = string.Empty;
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("deptId", deptId);
                paras.Add("proType", comboBox1.SelectedValue.ToString());
                if (HttpUploadFile(AppGlobalModel.ProjectImport, openFileDialog.FileName, ref resultData, paras))
                {
                    ShowSuccessMsg("导入成功！");
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
