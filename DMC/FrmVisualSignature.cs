using DMC.Helper;
using DMC.Models;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Asn1.IsisMtt.Ocsp;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace DMC
{
    /// <summary>
    /// 可视化签章
    /// </summary>
    public partial class FrmVisualSignature : BaseForm
    {
        /// <summary>
        /// 文件源信息
        /// </summary>
        private GetProjectFileListModel fileSourceInfo;
        /// <summary>
        /// url路径
        /// </summary>
        private string url = string.Empty;
        /// <summary>
        /// 源文件数据流
        /// </summary>
        private MemoryStream sourceMs;
        /// <summary>
        /// 签章图片
        /// </summary>
        private byte[] signatureByte;

        /// <summary>
        /// 预览文件
        /// </summary>
        /// <param name="obj">文件信息</param>
        public FrmVisualSignature(GetProjectFileListModel obj)
        {
            InitializeComponent();
            //获取文件信息
            fileSourceInfo = obj;
            if (fileSourceInfo.filePath.StartsWith("http"))
            {
                url = fileSourceInfo.filePath;//如果文件路径已经是完整的url，则直接使用
            }
            else
            {
                url = $"http://{AppGlobalModel.ServiceAddress}:{AppGlobalModel.ServiceProt}" + fileSourceInfo.filePath; //否则拼接成完整的url
            }
            // 加载章列表
            if (AppGlobalModel.qzSealList != null && AppGlobalModel.qzSealList.Any())
            {
                //如果章列表中没有“请选择”选项，则添加一个“请选择”选项到列表的开头
                if (!AppGlobalModel.qzSealList.Exists(o => o.id == "请选择"))
                {
                    //如果章列表中没有“请选择”选项，则添加一个“请选择”选项到列表的开头
                    AppGlobalModel.qzSealList.Insert(0, new QzSealModel { id = "请选择", sealname = "请选择" });
                }

                /*
                //将章列表绑定到下拉框
                   comboBox1.DataSource = AppGlobalModel.qzSealList;
                   //设置下拉框的ValueMember为章的id，DisplayMember为章的名称
                   comboBox1.ValueMember = "id";
                   //设置下拉框的SelectedIndex为0，默认选择“请选择”选项
                   comboBox1.DisplayMember = "sealname";
                   //设置下拉框的SelectedIndex为0，默认选择“请选择”选项
                   comboBox1.SelectedIndex = 0;
                 */
               
                // 确保列表不为 null
                if (AppGlobalModel.qzSealList == null) AppGlobalModel.qzSealList = new List<QzSealModel>();

                // 先过滤掉可能已存在的“请选择”占位项，再按 sealname 排序
                var items = AppGlobalModel.qzSealList
                    .Where(x => x != null && !string.Equals(x.sealname, "请选择", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(x => x.sealname ?? string.Empty)
                    .ToList();

                // 插入占位项到首位（不会参与排序）
                items.Insert(0, new QzSealModel { id = "", sealname = "请选择" });

                // 绑定到 ComboBox（DataSource 最后设置）
                comboBox1.DisplayMember = "sealname";
                comboBox1.ValueMember = "id";
                comboBox1.DataSource = items;
                comboBox1.SelectedIndex = 0;

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
                        // 获取文件数据流
                        HttpWebResponse wr = (HttpWebResponse)mRequest.GetResponse();
                        sourceMs = new MemoryStream();//创建内存流对象
                        using (var stream = wr.GetResponseStream()) //使用获取的文件数据流
                        {
                            //将文件数据流复制到内存流中
                            byte[] buffer = new byte[wr.ContentLength];
                            int actuallyRead = 0, offset = 0;
                            do
                            {
                                //从文件数据流中读取数据到缓冲区
                                actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
                                offset += actuallyRead;
                            } while (actuallyRead > 0);
                            //将缓冲区数据写入内存流
                            sourceMs = new MemoryStream(buffer);
                            sourceMs.Seek(0, SeekOrigin.Begin);//将内存流的当前位置设置为流的开始位置
                        }
                        //关闭数据流
                        pdfViewer2.Document = PdfiumViewer.PdfDocument.Load(sourceMs);
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
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 最大化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 最小化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMinSide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        /// <summary>
        /// 宽高输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_wh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// pdf预览鼠标离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PdfViewer1_MouseLeave(object sender, EventArgs e)
        {
            PdfiumViewer.PdfPoint point = this.pdfViewer2.Renderer.PointToPdf(MousePosition);
            if (point.IsValid)
            {
                this.pictureBox_signature.Visible = false;
            }
        }
        /// <summary>
        /// pdf预览鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PdfViewer1_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                PdfiumViewer.PdfPoint point = this.pdfViewer2.Renderer.PointToPdf(MousePosition);
                if (point.IsValid)
                {
                    using (MemoryStream ms = new MemoryStream(signatureByte))
                    {
                        Image signatureImage = Image.FromStream(ms);
                        var imageInfo = signatureImage;
                        this.pictureBox_signature.Image = imageInfo;
                        this.pictureBox_signature.Visible = true;
                    }
                }
            }
            catch (Exception EX_NAME)
            {
                MessageBox.Show(EX_NAME.ToString());
            }


        }
        /// <summary>
        /// pdf预览鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PdfViewer1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.pictureBox_signature.Visible)
            {
                Point mepoint = this.pdfViewer2.Renderer.PointToClient(e.Location);
                this.pictureBox_signature.Location = new Point(e.Location.X, e.Location.Y - this.pictureBox_signature.Height+100);
            }
        }
        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Renderer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectModel = (QzSealModel)comboBox1.SelectedItem;
            if (selectModel.id == "请选择")
            {
                ShowErrorMsg("请选择使用的章！");
                return;
            }

            PdfPoint point = this.pdfViewer2.Renderer.PointToPdf(e.Location);
            if (point.IsValid)
            {
                int page = point.Page;

                try
                {
                    Splasher.Show(typeof(FrmLoading));
                    var para = new { id = fileSourceInfo.id, sealId = selectModel.id, width = selectModel.w, height = selectModel.h, x= point.Location.X, y= point.Location.Y, page = page+1 };
                    var resultData = string.Empty;
                    if (HttpPost(AppGlobalModel.VisualTripartiteSignature, para, ref resultData))
                    {                     
                        if (!resultData.StartsWith("http"))
                        {
                            resultData = $"http://{AppGlobalModel.ServiceAddress}:{AppGlobalModel.ServiceProt}" + resultData;
                        }

                        if (!string.IsNullOrEmpty(resultData))
                        {
                            //获得文件扩展名
                            string fileNameEx = Path.GetExtension(resultData);

                            if (fileNameEx.ToLower().Equals(".pdf"))
                            {
                                Uri u = new Uri(resultData);
                                HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(u);
                                mRequest.Method = "GET";
                                mRequest.ContentType = "application/pdf";
                                mRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";

                                HttpWebResponse wr = (HttpWebResponse)mRequest.GetResponse();
                                sourceMs = new MemoryStream();
                                using (var stream = wr.GetResponseStream())
                                {
                                    byte[] buffer = new byte[wr.ContentLength];
                                    int actuallyRead = 0, offset = 0;
                                    do
                                    {
                                        actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
                                        offset += actuallyRead;
                                    } while (actuallyRead > 0);

                                    sourceMs = new MemoryStream(buffer);
                                    sourceMs.Seek(0, SeekOrigin.Begin);
                                }                            

                                pdfViewer2.Document = PdfiumViewer.PdfDocument.Load(sourceMs);
                                pdfViewer2.Renderer.Page = page;

                                ShowSuccessMsg("签名签章成功");
                            }
                        }
                        else
                        {
                            ShowErrorMsg("对不起，签名签章文件路径为空！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMsg("发生系统异常【" + ex.Message + "】！");
                }
            }
        }
        /// <summary>
        /// 加载图片
        /// </summary>
        private void LoadSignaturePicture()
        {
            if (signatureByte != null && !string.IsNullOrWhiteSpace(url))
            {
                this.pdfViewer2.Renderer.MouseEnter -= PdfViewer1_MouseEnter;
                this.pdfViewer2.Renderer.MouseMove -= PdfViewer1_MouseMove;
                this.pdfViewer2.Renderer.MouseLeave -= PdfViewer1_MouseLeave;
                this.pdfViewer2.Renderer.MouseDoubleClick -= Renderer_MouseDoubleClick;

                this.pdfViewer2.Renderer.MouseEnter += PdfViewer1_MouseEnter;
                this.pdfViewer2.Renderer.MouseMove += PdfViewer1_MouseMove;
                this.pdfViewer2.Renderer.MouseLeave += PdfViewer1_MouseLeave;
                this.pdfViewer2.Renderer.MouseDoubleClick += Renderer_MouseDoubleClick;
            }
        }
        /// <summary>
        /// 下拉选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectModel = (QzSealModel)comboBox1.SelectedItem;//根据选择的章获取章图片
            if (selectModel.sealname != "请选择") //如果选择了章
            {
                //根据选择的章获取章图片
                var requestFile = (HttpWebRequest)WebRequest.Create($"http://{AppGlobalModel.ServiceAddress}:{AppGlobalModel.ServiceProt}" + selectModel.url);
                var responseFile = requestFile.GetResponse();//获取章图片数据流
                using (var streamReader = new MemoryStream())//
                {
                    //将章图片数据流复制到内存流中
                    responseFile.GetResponseStream().CopyTo(streamReader);
                    signatureByte = streamReader.ToArray();//将内存流转换为字节数组
                }
                //关闭数据流
                requestFile.Abort();
                //将字节数组转换为图片并显示在pictureBox中
                using (MemoryStream ms = new MemoryStream(signatureByte))
                {
                    //将内存流转换为图片
                    Image signatureImage = Image.FromStream(ms);
                    //设置pictureBox的图片为章图片
                    this.pictureBox_signature.Image = signatureImage;
                    this.pictureBox_signature.Width = selectModel.w;
                    this.pictureBox_signature.Height = selectModel.h;
                    //显示pictureBox
                    LoadSignaturePicture();
                }
            }
            //else
            //{
            //    //如果没有选择章，则隐藏pictureBox并将章图片字节数组置空
            //    signatureByte = null;
            //    this.pictureBox_signature.Image = null;
            //    //隐藏pictureBox
            //    //pictureBox_signature.Visible = false;
            //}
        }
    }
}
