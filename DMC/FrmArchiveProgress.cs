using DMC.Models;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 档案归档处理进度
    /// </summary>
    public partial class FrmArchiveProgress : BaseForm
    {
        /// <summary>
        /// 创建圆形矩形区域
        /// </summary>
        /// <param name="nLeftRect"></param>
        /// <param name="nTopRect"></param>
        /// <param name="RightRect"></param>
        /// <param name="nBottonRect"></param>
        /// <param name="nWidthEllipse"></param>
        /// <param name="nHeightEllipse"></param>
        /// <returns></returns>
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int RightRect, int nBottonRect, int nWidthEllipse, int nHeightEllipse);
        private List<GetKeepTechnicalNameListModel> dataList = null;//技术资料列表
        private List<DirectoryStructureModel> directoryStructureList = null;//目录结构列表
        private AddKeepProjectAttributeModel paraInfo = null;//归档项目信息
        private BackgroundWorker worker = null;//后台操作对象
        public string resultDataModel = string.Empty;//结果数据

        /// <summary>
        /// 档案归档处理进度构造方法
        /// </summary>
        /// <param name="dataObj"></param>
        /// <param name="dirData"></param>
        /// <param name="objInfo"></param>
        public FrmArchiveProgress(List<GetKeepTechnicalNameListModel> dataObj, List<DirectoryStructureModel> dirData, AddKeepProjectAttributeModel objInfo)
        {
            //初始化窗体，设置圆角
            InitializeComponent();

            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));

            dataList = dataObj;
            directoryStructureList = dirData;
            paraInfo = objInfo;

            ProgressBar1.Value = 0;
            ProgressBar1.Maximum = dataList.Count(o => !string.IsNullOrWhiteSpace(o.localFile)) + (directoryStructureList == null ? 0 : directoryStructureList.Count()) + 1;

            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
        }

        /// <summary>
        /// 加载事件，开启后台操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmArchiveProgress_Load(object sender, EventArgs e)
        {
            worker.RunWorkerAsync(this);
        }

        /// <summary>
        /// 中间处理事件，执行操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProceessingMethod();
        }

        /// <summary>
        /// 窗口关闭事件，取消后台操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmArchiveProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker != null)
            {
                worker.WorkerSupportsCancellation = true;
            }
        }

        /// <summary>
        /// 处理方法
        /// </summary>
        private void ProceessingMethod()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(ProceessingMethod));
                return;
            }

            #region 获取图幅列表
            var resultJiSuanFrameModel = new List<JiSuanFrameModel>();
            if (HttpGet(AppGlobalModel.JiSuanFrame, ref resultJiSuanFrameModel))
            {
                FileUploadModel fileUpload;

                #region 技术资料
                foreach (var item in dataList.Where(o => !string.IsNullOrWhiteSpace(o.localFile)))
                {
                    fileUpload = new FileUploadModel();
                    fileUpload.isPdf = "1";

                    //获得文件扩展名
                    string fileNameEx = Path.GetExtension(item.localFile);
                    if (fileNameEx.ToLower().Equals(".pdf"))
                    {
                        fileUpload.isPdf = "0";
                        PdfReader pdfReader = new PdfReader(item.localFile);
                        //总页数
                        int iPageNum = pdfReader.NumberOfPages;

                        fileUpload.pageAll = iPageNum.ToString();
                        fileUpload.pageInfo = new List<PageInfoItem>();

                        PageInfoItem pageInfoItem;
                        JiSuanFrameModel jiSuanFrame;
                        for (var i = 0; i < iPageNum; i++)
                        {
                            var pdfPage = pdfReader.GetPageSizeWithRotation(i + 1);
                            jiSuanFrame = resultJiSuanFrameModel.FirstOrDefault(o => (o.minW <= pdfPage.Width && o.maxW >= pdfPage.Width) && (o.minH <= pdfPage.Height && o.maxH >= pdfPage.Height));

                            if (jiSuanFrame != null)
                            {
                                pageInfoItem = new PageInfoItem();
                                pageInfoItem.page = (i + 1).ToString();
                                pageInfoItem.width = (pdfPage.Width).ToString();
                                pageInfoItem.height = (pdfPage.Height).ToString();
                                pageInfoItem.frameName = jiSuanFrame.name;
                                pageInfoItem.folded = jiSuanFrame.folded;

                                fileUpload.pageInfo.Add(pageInfoItem);
                            }
                        }

                        fileUpload.frameName = (fileUpload.pageInfo == null || !fileUpload.pageInfo.Any()) ? "" : fileUpload.pageInfo.First(o => !string.IsNullOrWhiteSpace(o.frameName)).frameName;
                        fileUpload.folded = (fileUpload.pageInfo == null || !fileUpload.pageInfo.Any()) ? "" : (fileUpload.pageInfo.Where(o => !string.IsNullOrWhiteSpace(o.folded)).Sum(o => Convert.ToDecimal(o.folded))).ToString();
                    }

                    item.fileUpload = fileUpload;
                }
                #endregion

                #region 审图版图纸
                if (directoryStructureList != null && directoryStructureList.Any())
                {
                    foreach (var item in directoryStructureList.Where(o => o.Type == 2))
                    {
                        fileUpload = new FileUploadModel();
                        fileUpload.isPdf = "1";

                        //获得文件扩展名
                        string fileNameEx = Path.GetExtension(item.Name);
                        if (fileNameEx.ToLower().Equals(".pdf"))
                        {
                            fileUpload.isPdf = "0";
                            PdfReader pdfReader = new PdfReader(item.Name);
                            //总页数
                            int iPageNum = pdfReader.NumberOfPages;

                            fileUpload.pageAll = iPageNum.ToString();
                            fileUpload.pageInfo = new List<PageInfoItem>();

                            PageInfoItem pageInfoItem;
                            JiSuanFrameModel jiSuanFrame;
                            for (var i = 0; i < iPageNum; i++)
                            {
                                var pdfPage = pdfReader.GetPageSizeWithRotation(i + 1);
                                jiSuanFrame = resultJiSuanFrameModel.FirstOrDefault(o => (o.minW <= pdfPage.Width && o.maxW >= pdfPage.Width) && (o.minH <= pdfPage.Height && o.maxH >= pdfPage.Height));

                                if (jiSuanFrame != null)
                                {
                                    pageInfoItem = new PageInfoItem();
                                    pageInfoItem.page = (i + 1).ToString();
                                    pageInfoItem.width = (pdfPage.Width).ToString();
                                    pageInfoItem.height = (pdfPage.Height).ToString();
                                    pageInfoItem.frameName = jiSuanFrame.name;
                                    pageInfoItem.folded = jiSuanFrame.folded;

                                    fileUpload.pageInfo.Add(pageInfoItem);
                                }
                            }

                            fileUpload.frameName = (fileUpload.pageInfo == null || !fileUpload.pageInfo.Any()) ? "" : fileUpload.pageInfo.First(o => !string.IsNullOrWhiteSpace(o.frameName)).frameName;
                            fileUpload.folded = (fileUpload.pageInfo == null || !fileUpload.pageInfo.Any()) ? "" : (fileUpload.pageInfo.Where(o => !string.IsNullOrWhiteSpace(o.folded)).Sum(o => Convert.ToDecimal(o.folded))).ToString();
                        }

                        item.fileUpload = fileUpload;
                    }
                }
                #endregion
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            #endregion

            #region 添加归档项目基本信息
            if (HttpPost(AppGlobalModel.AddKeepProjectAttribute, paraInfo, ref resultDataModel))
            {
                ProgressBar1.Value += 1;
                ProgressBar1.Text = ((float)ProgressBar1.Value / (float)ProgressBar1.Maximum * 100).ToString("N0") + "%";

                #region 技术资料
                Dictionary<string, string> paras;
                string resultData;
                foreach (var item in dataList.Where(o => !string.IsNullOrWhiteSpace(o.localFile)))
                {
                    item.fileUpload.technicalId = item.id;
                    item.fileUpload.tempAttributeId = resultDataModel;
                    item.fileUpload.parentId = "0";

                    resultData = string.Empty;
                    paras = new Dictionary<string, string>();
                    paras.Add("fileDetails", JsonConvert.SerializeObject(item.fileUpload));
                    if (!HttpUploadFile(AppGlobalModel.KeepProjectTempFileUpload, item.localFile, ref resultData, paras))
                    {
                        DialogResult = DialogResult.Cancel;
                        return;
                    }
                    else
                    {
                        ProgressBar1.Value += 1;
                        ProgressBar1.Text = ((float)ProgressBar1.Value / (float)ProgressBar1.Maximum * 100).ToString("N0") + "%";
                    }
                }
                #endregion

                #region 审图版图纸
                if (directoryStructureList != null && directoryStructureList.Any())
                {
                    var parentInfo = directoryStructureList.FirstOrDefault(o => string.IsNullOrWhiteSpace(o.ParentId));

                    if (!UploadFileAndFolder(resultDataModel, parentInfo))
                    {
                        DialogResult = DialogResult.Cancel;
                        return;
                    }
                }
                #endregion
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            #endregion

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 上传文件和文件夹
        /// </summary>
        /// <param name="tempAttributeId"></param>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        private bool UploadFileAndFolder(string tempAttributeId, DirectoryStructureModel directoryInfo)
        {
            var paraFolder = new
            {
                name = directoryInfo.Name,
                parentId = string.IsNullOrWhiteSpace(directoryInfo.ParentId) ? "0" : directoryInfo.ParentId,
                tempAttributeId = tempAttributeId
            };

            var resultDataModel = new KeepProjectTempAddDirModel();
            if (HttpPost(AppGlobalModel.KeepProjectTempAddDir, paraFolder, ref resultDataModel))
            {
                ProgressBar1.Value += 1;
                ProgressBar1.Text = ((float)ProgressBar1.Value / (float)ProgressBar1.Maximum * 100).ToString("N0") + "%";
                
                Dictionary<string, string> paras;
                string resultData;
                #region 上传文件
                //上传文件
                foreach (var item in directoryStructureList.Where(o => o.Type == 2 && o.ParentId == directoryInfo.PrimaryKey))
                {
                    resultData = string.Empty;
                    item.fileUpload.technicalId = "";
                    item.fileUpload.tempAttributeId = tempAttributeId;
                    item.fileUpload.parentId = resultDataModel.id;

                    paras = new Dictionary<string, string>();
                    paras.Add("fileDetails", JsonConvert.SerializeObject(item.fileUpload));
                    if (!HttpUploadFile(AppGlobalModel.KeepProjectTempFileUpload, item.Name, ref resultData, paras))
                    {
                        return false;
                    }
                    else
                    {
                        ProgressBar1.Value += 1;
                        ProgressBar1.Text = ((float)ProgressBar1.Value / (float)ProgressBar1.Maximum * 100).ToString("N0") + "%";
                    }
                }
                #endregion

                #region 创建文件夹
                foreach (var item in directoryStructureList.Where(o => o.Type == 1 && o.ParentId == directoryInfo.PrimaryKey))
                {
                    item.ParentId = resultDataModel.id;
                    UploadFileAndFolder(tempAttributeId, item);
                }
                #endregion
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}