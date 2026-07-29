using DMC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 查看版本
    /// </summary>
    public partial class FrmKeepSeeHistoryFile : BaseForm
    {
        private string fileId = null;
        public FrmKeepSeeHistoryFile(string obj)
        {
            InitializeComponent();
            fileId = obj;
            dataGridView1.AutoGenerateColumns = false;

            button2.Visible = AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:version:info");
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

        private void FrmSeeHistoryFile_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(fileId))
            {
                ShowErrorMsg("请选择文件！");
            }
            else
            {
                var resultData = new List<GetKeepProjectDirModel>();
                if (HttpGet(AppGlobalModel.GetKeepFileLog + $"?fileId={fileId}", ref resultData))
                {
                    dataGridView1.DataSource = resultData;
                    dataGridView1.ClearSelection();
                }
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            var index = dataGridView1.CurrentRow.Index;
            if (index > -1)
            {
                var list = (List<GetKeepProjectDirModel>)dataGridView1.DataSource;
                var selectModel = list[index];
                var listUrl = list.Select(o => new PreviewAreaViewModel() { filePath = o.filePath, name = o.name }).ToList();
                var frm = new FrmPreviewArea(selectModel.filePath, 1, listUrl);
                frm.Show();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                button2.Enabled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
