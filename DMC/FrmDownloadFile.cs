using DMC.Helper;
using DMC.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 下载文件
    /// </summary>
    public partial class FrmDownloadFile : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int RightRect, int nBottonRect, int nWidthEllipse, int nHeightEllipse);

        private string url = null;
        private string path = null;
        private int times = 3;
        private BackgroundWorker worker = null;
        //委托
        delegate void SetProgressBarCallback();

        #region 公共属性
        public bool isTips = true;
        #endregion

        public FrmDownloadFile(string objUrl, string defaultPath = null)
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            ProgressBar1.Value = 0;

            if (objUrl.StartsWith("http"))
            {
                url = objUrl;
            }
            else
            {
                url = $"http://{AppGlobalModel.ServiceAddress}:{AppGlobalModel.ServiceProt}" + (objUrl.StartsWith("/") ? "" : "/") + objUrl;
            }
            path = defaultPath;

            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
        }

        private void FrmDownloadFile_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择路径";
                dialog.SelectedPath = AppGlobalModel.InitialDownloadDirectory;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    #region 保存打开的文件目录
                    AppGlobalModel.InitialDownloadDirectory = dialog.SelectedPath;
                    ConfigHelper.SaveConfigInfo("InitialDownloadDirectory", AppGlobalModel.InitialDownloadDirectory);
                    #endregion

                    var filrName = Path.GetFileName(url);
                    path = dialog.SelectedPath + "\\" + filrName;

                    worker.RunWorkerAsync(this);
                }
                else
                {
                    DialogResult = DialogResult.No;
                }
            }
            else
            {
                worker.RunWorkerAsync(this);
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Download();
        }

        private void Download()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(Download));
                return;
            }

            try
            {
                Uri u = new Uri(url);
                HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(u);
                mRequest.Method = "GET";
                mRequest.ContentType = "application/x-www-form-urlencoded";
                //mRequest.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; QQDownload 534; TencentTraveler 4.0; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; CIBA; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; InfoPath.2)";
                mRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";

                HttpWebResponse wr = (HttpWebResponse)mRequest.GetResponse();

                Stream sIn = wr.GetResponseStream();
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);

                long length = wr.ContentLength;

                long totalDownloadedByte = 0;
                float percent = 0;

                byte[] buffer = new byte[1024 * 40];
                do
                {
                    if (worker.WorkerSupportsCancellation)
                    {
                        return;
                    }

                    int len = sIn.Read(buffer, 0, buffer.Length);
                    if (len <= 0)
                    {
                        // 下载完成
                        break;
                    }

                    totalDownloadedByte = len + totalDownloadedByte;
                    Application.DoEvents();

                    fs.Write(buffer, 0, len);

                    percent = (float)totalDownloadedByte / (float)length * 100;
                    ProgressBar1.Value = Convert.ToInt32(percent.ToString("N0"));
                    ProgressBar1.Text = ProgressBar1.Value.ToString() + "%";
                    Application.DoEvents();

                } while (true);

                sIn.Close();
                wr.Close();
                fs.Close();

                /*
                if (isTips)
                {
                    MessageBox.Show(this, "下载成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                DialogResult = DialogResult.OK;
                */

                if (isTips)
                {
                    timer1.Enabled = true;
                    label2.Text = $"下载成功！{times}秒后关闭";
                }
                else
                {
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"下载失败【{ex.Message}】", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }
        }

        private void FrmDownloadFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker != null)
            {
                worker.WorkerSupportsCancellation = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            times--;

            label2.Text = $"下载成功！{times}秒后关闭";

            if (times == 0)
            {
                timer1.Enabled = false;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
