using DMC.Models;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace DMC
{
    /// <summary>
    /// 预览文件
    /// </summary>
    public partial class FrmPreviewArea : BaseForm
    {
        /// <summary>
        /// 文件来源0 项目区 1归档区(规则：归档区文件没有下载本地并打开，项目区文件自动下载到本地并使用windows资源打开，程序推出之后自动清理)
        /// </summary>
        private int fileSource = 0;
        /// <summary>
        /// url路径
        /// </summary>
        private string url = string.Empty;
        /// <summary>
        /// 浏览文件
        /// </summary>
        private List<PreviewAreaViewModel> urlList = null;

        /// <summary>
        /// 预览文件
        /// </summary>
        /// <param name="fileUrl">文件路径</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="fileUrlList">文件路径列表</param>
        public FrmPreviewArea(string fileUrl, int fileType, List<PreviewAreaViewModel> fileUrlList = null)
        {
            InitializeComponent();

            fileSource = fileType;
            if (!string.IsNullOrWhiteSpace(fileUrl))
            {
                if (fileUrl.StartsWith("http"))
                {
                    url = fileUrl;
                }
                else
                {
                    url = $"http://{AppGlobalModel.ServiceAddress}:{AppGlobalModel.ServiceProt}" + fileUrl;
                }
            }

            urlList = fileUrlList;

            if (urlList == null || !urlList.Any())
            {
                panel1.Visible = false;
            }
            else
            {
                var index = urlList.FindIndex(o => url.EndsWith(o.filePath));
                if (index == 0)
                {
                    button1.Enabled = false;
                }

                if (index == urlList.Count - 1)
                {
                    button2.Enabled = false;
                }
                label文件名.Text = urlList[index].name;
                //this.Text = urlList[index].name;
            }
        }
        #region 拉申窗口方法
        const int Guying_HTLEFT = 10;
        const int Guying_HTRIGHT = 11;
        const int Guying_HTTOP = 12;
        const int Guying_HTTOPLEFT = 13;
        const int Guying_HTTOPRIGHT = 14;
        const int Guying_HTBOTTOM = 15;
        const int Guying_HTBOTTOMLEFT = 0x10;
        const int Guying_HTBOTTOMRIGHT = 17;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
                        else m.Result = (IntPtr)Guying_HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)Guying_HTRIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)Guying_HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)Guying_HTBOTTOM;
                    break;
                case 0x0201:                //鼠标左键按下的消息 
                    m.Msg = 0x00A1;         //更改消息为非客户区按下鼠标 
                    m.LParam = IntPtr.Zero; //默认值 
                    m.WParam = new IntPtr(2);//鼠标放在标题栏内 
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion
        #region 简化方法 窗体移动,直接变化Left、Top
        private Point originLocation;
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originLocation = e.Location;
            }
        }
        #endregion
        private void FrmPreviewArea_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    //获得文件扩展名
                    string fileNameEx = Path.GetExtension(url);

                    if (fileNameEx.ToLower().Equals(".pdf"))
                    {
                        Uri u = new Uri(url);
                        HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(u);
                        mRequest.Method = "GET";
                        mRequest.ContentType = "application/pdf";
                        mRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";

                        HttpWebResponse wr = (HttpWebResponse)mRequest.GetResponse();
                        MemoryStream ms = new MemoryStream();
                        using (var stream = wr.GetResponseStream())
                        {
                            byte[] buffer = new byte[wr.ContentLength];
                            int actuallyRead = 0, offset = 0;
                            do
                            {
                                actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
                                offset += actuallyRead;
                            } while (actuallyRead > 0);

                            ms = new MemoryStream(buffer);
                            ms.Seek(0, SeekOrigin.Begin);
                        }

                        pdfViewer2.Document = PdfiumViewer.PdfDocument.Load(ms);
                    }
                    else
                    {
                        //项目区文件，自动下载本地，使用windows资源管理器打开
                        if (fileSource == 0)
                        {
                            //临时文件夹
                            var dir = Environment.CurrentDirectory + $"\\TempFile";
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }

                            var filrName = Path.GetFileName(url);
                            dir = dir + "\\" + filrName;

                            var frmDownload = new FrmDownloadFile(url, dir);
                            frmDownload.isTips = false;
                            var dialogResult = frmDownload.ShowDialog();
                            if (dialogResult == DialogResult.OK)
                            {
                                System.Diagnostics.Process.Start(dir);
                                this.Close();
                            }
                        }
                        else
                        {
                            ShowErrorMsg("文件格式不正确，不是pdf文件！");
                            this.Close();
                        }
                    }
                }
                else
                {
                    ShowErrorMsg("对不起，文件路径为空！");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMsg(ex.Message);
                this.Close();
            }
        }
        /// <summary>
        /// 加载文件
        /// </summary>
        private void loadFile()
        {
            if (!string.IsNullOrEmpty(url))
            {
                //获得文件扩展名
                string fileNameEx = Path.GetExtension(url);

                if (fileNameEx.ToLower().Equals(".pdf"))
                {
                    Uri u = new Uri(url);
                    HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(u);
                    mRequest.Method = "GET";
                    mRequest.ContentType = "application/pdf";
                    mRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";

                    HttpWebResponse wr = (HttpWebResponse)mRequest.GetResponse();
                    MemoryStream ms = new MemoryStream();
                    using (var stream = wr.GetResponseStream())
                    {
                        byte[] buffer = new byte[wr.ContentLength];
                        int actuallyRead = 0, offset = 0;
                        do
                        {
                            actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
                            offset += actuallyRead;
                        } while (actuallyRead > 0);

                        ms = new MemoryStream(buffer);
                        ms.Seek(0, SeekOrigin.Begin);
                    }

                    pdfViewer2.Document = PdfiumViewer.PdfDocument.Load(ms);
                }
                else
                {
                    pdfViewer2.Document = null;
                    ShowErrorMsg($"文件格式不正确，不是pdf文件！【{Path.GetFileName(url)}】");
                }
            }
            else
            {
                pdfViewer2.Document = null;
                ShowErrorMsg("对不起，文件路径为空！");
            }
        }

        /// <summary>
        /// 上一个文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (urlList != null && urlList.Any())
            {
                button2.Enabled = true;

                var index = urlList.FindIndex(o => url.EndsWith(o.filePath));
                if (index == 1)
                {
                    button1.Enabled = false;
                }

                if (urlList[index - 1].filePath.StartsWith("http"))
                {
                    url = urlList[index - 1].filePath;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(urlList[index - 1].filePath))
                    {
                        url = null;
                    }
                    else
                    {
                        url = $"http://{AppGlobalModel.ServiceAddress}:{AppGlobalModel.ServiceProt}" + urlList[index - 1].filePath;
                    }
                }
                label文件名.Text = urlList[index - 1].name;
                //this.Text = urlList[index - 1].name;
                loadFile();
            }
        }

        /// <summary>
        /// 下一个文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (urlList != null && urlList.Any())
            {
                var index = urlList.FindIndex(o => url.EndsWith(o.filePath));
                if (index == 0)
                {
                    button1.Enabled = true;
                }

                if (index == urlList.Count - 2)
                {
                    button2.Enabled = false;
                }

                if (urlList[index + 1].filePath.StartsWith("http"))
                {
                    url = urlList[index + 1].filePath;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(urlList[index + 1].filePath))
                    {
                        url = null;
                    }
                    else
                    {
                        url = $"http://{AppGlobalModel.ServiceAddress}:{AppGlobalModel.ServiceProt}" + urlList[index + 1].filePath;
                    }
                }
                label文件名.Text= urlList[index + 1].name;
                //this.Text = urlList[index + 1].name;
                loadFile();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonMaxSide_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void buttonMinSide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        private void FitPage(PdfViewerZoomMode zoomMode)
        {
            int page = pdfViewer2.Renderer.Page;
            pdfViewer2.ZoomMode = zoomMode;
            pdfViewer2.Renderer.Zoom = 1;
            pdfViewer2.Renderer.Page = page;
        }
        private void button_左_Click(object sender, EventArgs e)
        {
            pdfViewer2.Renderer.RotateLeft();
        }

        private void button_右_Click(object sender, EventArgs e)
        {
            pdfViewer2.Renderer.RotateRight();
        }
        private void button_Width_Click(object sender, EventArgs e)
        {
            FitPage(PdfViewerZoomMode.FitWidth);
        }

        private void button_Height_Click(object sender, EventArgs e)
        {
            FitPage(PdfViewerZoomMode.FitHeight);
        }
    }
}
