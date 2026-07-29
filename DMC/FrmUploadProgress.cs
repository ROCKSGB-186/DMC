using DMC.Models;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 上传文件处理进度
    /// </summary>
    public partial class FrmUploadProgress : BaseForm
    {

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int RightRect, int nBottonRect, int nWidthEllipse, int nHeightEllipse);

        /// <summary>
        /// 区域类型（0：项目文件，1：归档管理）
        /// </summary>
        private int areaType = 0;
        /// <summary>
        /// 上传类型（1：文件，2：文件夹）
        /// </summary>
        private int uploadType = 0;
        /// <summary>
        /// 文件类型
        /// </summary>
        private string fileType = "";
        /// <summary>
        /// 项目Id
        /// </summary>
        private string parentId = "";
        /// <summary>
        /// 归档管理使用,没有值就是项目
        /// </summary>
        private string dirType = "";
        /// <summary>
        /// Represents the collection of directory structures managed by the current instance.
        /// </summary>
        /// <remarks>This field is initialized to null and should be assigned before use. It is intended
        /// for internal storage of directory structure data and is not exposed directly to consumers of the
        /// class.</remarks>
        private List<DirectoryStructureModel> directoryStructureList = null;
        /// <summary>
        /// Represents the list of calculation frames used in the computation process.
        /// </summary>
        /// <remarks>This field is initialized to null and should be assigned a valid list before use.
        /// Accessing this field without initialization may result in a null reference exception.</remarks>
        private List<JiSuanFrameModel> jiSuanFrameList = null;

        private BackgroundWorker worker = null;

        #region 断点续传相关
        // ===== 断点续传（多文件/单文件）相关字段 =====
        /// <summary>
        /// 仅对 uploadType == 1 生效；用于标识当前上传任务的本地续传文件路径
        /// </summary>
        private string resumeFilePath = string.Empty;

        /// <summary>
        /// 当前上传任务的续传状态
        /// </summary>
        private UploadResumeState resumeState = null;

        /// <summary>
        /// 上传续传状态模型
        /// </summary>
        private class UploadResumeState
        {
            // 任务上下文
            public string BatchId { get; set; }
            public string ParentId { get; set; }
            public int AreaType { get; set; }
            public string DirType { get; set; }
            public int UploadType { get; set; }
            public string FileType { get; set; }

            // 任务快照：用于恢复 UI，无需重新选文件/文件夹
            public List<DirectoryStructureModel> TaskItems { get; set; } = new List<DirectoryStructureModel>();

            // 文件级续传：已成功文件
            public List<string> SuccessFileKeys { get; set; } = new List<string>();

            // 文件夹续传：本地目录(PrimaryKey) -> 服务端目录ID
            public Dictionary<string, string> FolderServerIdMap { get; set; } = new Dictionary<string, string>();

            // 任务完成标记
            public bool Completed { get; set; }
        }

        public class ResumeTaskInfo
        {
            public int UploadType { get; set; }
            public string FileType { get; set; }
            public List<DirectoryStructureModel> DirectoryStructureList { get; set; } = new List<DirectoryStructureModel>();
            public List<string> PendingFileList { get; set; } = new List<string>();
        }

        private static string GetResumeDir()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "upload_resume");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        private static string SafeName(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "none";
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                text = text.Replace(c, '_');
            }
            return text;
        }

        private static string BuildResumePrefix(string parentId, int areaType, string dirType)
        {
            return $"resume_{SafeName(parentId)}_{areaType}_{SafeName(dirType)}_";
        }

        public static bool TryLoadPendingTask(string parentId, int areaType, string dirType, out ResumeTaskInfo info)
        {
            info = null;
            var dir = GetResumeDir();
            var prefix = BuildResumePrefix(parentId, areaType, dirType);
            var files = Directory.GetFiles(dir, prefix + "*.json")
                .OrderByDescending(File.GetLastWriteTimeUtc)
                .ToList();

            foreach (var file in files)
            {
                try
                {
                    var json = File.ReadAllText(file, Encoding.UTF8);
                    var state = JsonConvert.DeserializeObject<UploadResumeState>(json);
                    if (state == null || state.Completed) continue;
                    if (state.TaskItems == null || !state.TaskItems.Any()) continue;

                    var pending = state.TaskItems
                        .Where(o => o.Type == 2 && !string.IsNullOrWhiteSpace(o.Name))
                        .Where(o =>
                        {
                            var key = BuildFileKeyStatic(o.Name);
                            return state.SuccessFileKeys == null || !state.SuccessFileKeys.Contains(key);
                        })
                        .Select(o => o.Name)
                        .ToList();

                    info = new ResumeTaskInfo
                    {
                        UploadType = state.UploadType,
                        FileType = state.FileType,
                        DirectoryStructureList = state.TaskItems,
                        PendingFileList = pending
                    };
                    return true;
                }
                catch
                {
                    // 忽略损坏记录，继续找下一个
                }
            }

            return false;
        }

        private static string BuildFileKeyStatic(string filePath)
        {
            try
            {
                var fi = new FileInfo(filePath);
                var full = fi.FullName.Trim().ToLowerInvariant();
                if (!fi.Exists)
                {
                    return full + "|NOT_EXISTS";
                }
                return $"{full}|{fi.Length}|{fi.LastWriteTimeUtc.Ticks}";
            }
            catch
            {
                return (filePath ?? "").Trim().ToLowerInvariant() + "|ERR";
            }
        }
        #endregion

        /// <summary>
        /// 文件上传处理
        /// </summary>
        /// <param name="objParentId">项目的父Id</param>
        /// <param name="objAreaType">项目类型param>
        /// <param name="objDirType">文件夹类型</param>
        /// <param name="objUploadType">上传类型</param>
        /// <param name="objFileType">文件类型</param>
        /// <param name="objList">文件夹类型</param>
        /// <param name="objJiSuanFrameList">图幅列表</param>
        public FrmUploadProgress(string objParentId, int objAreaType, string objDirType, int objUploadType, string objFileType, List<DirectoryStructureModel> objList, List<JiSuanFrameModel> objJiSuanFrameList)
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));

            parentId = objParentId;
            areaType = objAreaType;
            dirType = objDirType;
            uploadType = objUploadType;
            fileType = objFileType;
            directoryStructureList = objList;
            jiSuanFrameList = objJiSuanFrameList;

            ProgressBar1.Value = 0;
            ProgressBar1.Maximum = directoryStructureList.Count();

            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
        }
        /// <summary>
        /// 文件上传处理自动加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmUploadProgress_Load(object sender, EventArgs e)
        {
            worker.RunWorkerAsync(this);
        }
        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProceessingMethod();
        }

        /// <summary>
        /// 文件上传处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmUploadProgress_FormClosing(object sender, FormClosingEventArgs e)
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

            // 统一初始化续传状态（文件/文件夹都走同一套状态文件）
            InitResumeState();

            FileUploadModel fileUpload;
            Dictionary<string, string> paras;
            string resultData;

            if (uploadType == 1)
            {
                // 文件上传（单文件/多文件）
                foreach (var item in directoryStructureList)
                {
                    // 非文件项跳过
                    if (item == null || item.Type != 2 || string.IsNullOrWhiteSpace(item.Name))
                    {
                        IncreaseProgress();
                        continue;
                    }

                    // 文件不存在则中断
                    if (!File.Exists(item.Name))
                    {
                        this.Hide();
                        ShowErrorMsg($"文件不存在：{item.Name}");
                        DialogResult = DialogResult.Cancel;
                        return;
                    }

                    // 已成功上传过则跳过
                    if (IsFileUploaded(item.Name))
                    {
                        IncreaseProgress();
                        continue;
                    }

                    resultData = string.Empty;
                    paras = new Dictionary<string, string>();
                    fileUpload = new FileUploadModel
                    {
                        parentId = parentId,
                        isPdf = "1",
                        fileTypeId = fileType
                    };

                    // PDF 图幅计算
                    string fileNameEx = Path.GetExtension(item.Name);
                    if (fileNameEx.ToLower().Equals(".pdf"))
                    {
                        fileUpload.isPdf = "0";
                        PdfReader pdfReader = new PdfReader(item.Name);
                        int iPageNum = pdfReader.NumberOfPages;

                        fileUpload.pageAll = iPageNum.ToString();
                        fileUpload.pageInfo = new List<PageInfoItem>();

                        PageInfoItem pageInfoItem;
                        JiSuanFrameModel jiSuanFrame;
                        for (var i = 0; i < iPageNum; i++)
                        {
                            var pdfPage = pdfReader.GetPageSizeWithRotation(i + 1);
                            jiSuanFrame = jiSuanFrameList.FirstOrDefault(o =>
                                (o.minW <= pdfPage.Width && o.maxW >= pdfPage.Width) &&
                                (o.minH <= pdfPage.Height && o.maxH >= pdfPage.Height));

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
                            else
                            {
                                this.Hide();
                                ShowErrorMsg($"此文件{item.Name}，没有对应的图幅，请联系管理员！");
                                DialogResult = DialogResult.Cancel;
                                return;
                            }
                        }

                        fileUpload.frameName = fileUpload.pageInfo.First().frameName;
                        fileUpload.folded = (fileUpload.pageInfo.Sum(o => Convert.ToDecimal(o.folded))).ToString();
                    }

                    paras.Add("fileDetails", JsonConvert.SerializeObject(fileUpload));
                    if (HttpUploadFile(areaType == 0 ? AppGlobalModel.FileUpload : AppGlobalModel.KeepProjectFileUpload, item.Name, ref resultData, paras))
                    {
                        // 成功后写入续传状态
                        MarkFileUploaded(item.Name);
                        IncreaseProgress();
                    }
                    else
                    {
                        // 失败时保存状态，下次继续
                        SaveResumeState();
                        this.Hide();
                        ShowErrorMsg("上传中断，已保存进度。请重新点击上传继续未完成文件。");
                        DialogResult = DialogResult.Cancel;
                        return;
                    }
                }
            }
            else if (uploadType == 2)
            {
                // 文件夹上传：先给所有文件准备 fileUpload 元数据
                foreach (var item in directoryStructureList.Where(o => o.Type == 2))
                {
                    fileUpload = new FileUploadModel();
                    fileUpload.isPdf = "1";
                    fileUpload.fileTypeId = fileType;

                    string fileNameEx = Path.GetExtension(item.Name);
                    if (fileNameEx.ToLower().Equals(".pdf"))
                    {
                        fileUpload.isPdf = "0";
                        PdfReader pdfReader = new PdfReader(item.Name);
                        int iPageNum = pdfReader.NumberOfPages;

                        fileUpload.pageAll = iPageNum.ToString();
                        fileUpload.pageInfo = new List<PageInfoItem>();

                        PageInfoItem pageInfoItem;
                        JiSuanFrameModel jiSuanFrame;
                        for (var i = 0; i < iPageNum; i++)
                        {
                            var pdfPage = pdfReader.GetPageSizeWithRotation(i + 1);
                            jiSuanFrame = jiSuanFrameList.FirstOrDefault(o =>
                                (o.minW <= pdfPage.Width && o.maxW >= pdfPage.Width) &&
                                (o.minH <= pdfPage.Height && o.maxH >= pdfPage.Height));

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
                            else
                            {
                                this.Hide();
                                ShowErrorMsg($"此文件{item.Name}，没有对应的图幅，请联系管理员！");
                                DialogResult = DialogResult.Cancel;
                                return;
                            }
                        }

                        fileUpload.frameName = fileUpload.pageInfo.First().frameName;
                        fileUpload.folded = (fileUpload.pageInfo.Sum(o => Convert.ToDecimal(o.folded))).ToString();
                    }

                    item.fileUpload = fileUpload;
                }

                var parentInfo = directoryStructureList.FirstOrDefault(o => string.IsNullOrWhiteSpace(o.ParentId));
                if (parentInfo == null)
                {
                    this.Hide();
                    ShowErrorMsg("未找到根目录信息，无法继续上传。");
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                if (!UploadFileAndFolder(parentInfo, parentId))
                {
                    SaveResumeState();
                    DialogResult = DialogResult.Cancel;
                    return;
                }
            }
            else
            {
                this.Hide();
                ShowErrorMsg("类型错误！");
                DialogResult = DialogResult.Cancel;
                return;
            }

            // 到这里表示整个任务成功完成
            if (resumeState != null)
            {
                resumeState.Completed = true;
                SaveResumeState();
            }
            ClearResumeState();

            this.Hide();
            ShowSuccessMsg("上传成功！");
            DialogResult = DialogResult.OK;
        }


        //private void ProceessingMethod()
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.BeginInvoke(new Action(ProceessingMethod));
        //        return;
        //    }

        //    FileUploadModel fileUpload;
        //    Dictionary<string, string> paras;
        //    string resultData;
        //    if (uploadType == 1)
        //    {
        //        foreach (var item in directoryStructureList)
        //        {
        //            resultData = string.Empty;
        //            paras = new Dictionary<string, string>();
        //            fileUpload = new FileUploadModel();

        //            fileUpload.parentId = parentId;
        //            fileUpload.isPdf = "1";
        //            fileUpload.fileTypeId = fileType;

        //            //获得文件扩展名
        //            string fileNameEx = Path.GetExtension(item.Name);
        //            if (fileNameEx.ToLower().Equals(".pdf"))
        //            {
        //                fileUpload.isPdf = "0";
        //                PdfReader pdfReader = new PdfReader(item.Name);
        //                //总页数
        //                int iPageNum = pdfReader.NumberOfPages;

        //                fileUpload.pageAll = iPageNum.ToString();
        //                fileUpload.pageInfo = new List<PageInfoItem>();

        //                PageInfoItem pageInfoItem;
        //                JiSuanFrameModel jiSuanFrame;
        //                for (var i = 0; i < iPageNum; i++)
        //                {
        //                    var pdfPage = pdfReader.GetPageSizeWithRotation(i + 1);
        //                    jiSuanFrame = jiSuanFrameList.FirstOrDefault(o => (o.minW <= pdfPage.Width && o.maxW >= pdfPage.Width) && (o.minH <= pdfPage.Height && o.maxH >= pdfPage.Height));

        //                    if (jiSuanFrame != null)
        //                    {
        //                        pageInfoItem = new PageInfoItem();
        //                        pageInfoItem.page = (i + 1).ToString();
        //                        pageInfoItem.width = (pdfPage.Width).ToString();
        //                        pageInfoItem.height = (pdfPage.Height).ToString();
        //                        pageInfoItem.frameName = jiSuanFrame.name;
        //                        pageInfoItem.folded = jiSuanFrame.folded;

        //                        fileUpload.pageInfo.Add(pageInfoItem);
        //                    }
        //                    else
        //                    {
        //                        this.Hide();
        //                        ShowErrorMsg($"此文件{item.Name}，没有对应的图幅，请联系管理员！");
        //                        DialogResult = DialogResult.Cancel;
        //                        return;
        //                    }
        //                }

        //                fileUpload.frameName = fileUpload.pageInfo.First().frameName;
        //                fileUpload.folded = (fileUpload.pageInfo.Sum(o => Convert.ToDecimal(o.folded))).ToString();
        //            }

        //            paras.Add("fileDetails", JsonConvert.SerializeObject(fileUpload));
        //            if (HttpUploadFile(areaType == 0 ? AppGlobalModel.FileUpload : AppGlobalModel.KeepProjectFileUpload, item.Name, ref resultData, paras))
        //            {
        //                ProgressBar1.Value += 1;
        //                ProgressBar1.Text = ((float)ProgressBar1.Value / (float)ProgressBar1.Maximum * 100).ToString("N0") + "%";
        //            }
        //            else
        //            {
        //                DialogResult = DialogResult.Cancel;
        //                return;
        //            }
        //        }
        //    }
        //    else if (uploadType == 2)
        //    {
        //        foreach (var item in directoryStructureList.Where(o => o.Type == 2))
        //        {
        //            fileUpload = new FileUploadModel();
        //            fileUpload.isPdf = "1";
        //            fileUpload.fileTypeId = fileType;

        //            //获得文件扩展名
        //            string fileNameEx = Path.GetExtension(item.Name);
        //            if (fileNameEx.ToLower().Equals(".pdf"))
        //            {
        //                fileUpload.isPdf = "0";
        //                PdfReader pdfReader = new PdfReader(item.Name);
        //                //总页数
        //                int iPageNum = pdfReader.NumberOfPages;

        //                fileUpload.pageAll = iPageNum.ToString();
        //                fileUpload.pageInfo = new List<PageInfoItem>();

        //                PageInfoItem pageInfoItem;
        //                JiSuanFrameModel jiSuanFrame;
        //                for (var i = 0; i < iPageNum; i++)
        //                {
        //                    var pdfPage = pdfReader.GetPageSizeWithRotation(i + 1);
        //                    jiSuanFrame = jiSuanFrameList.FirstOrDefault(o => (o.minW <= pdfPage.Width && o.maxW >= pdfPage.Width) && (o.minH <= pdfPage.Height && o.maxH >= pdfPage.Height));

        //                    if (jiSuanFrame != null)
        //                    {
        //                        pageInfoItem = new PageInfoItem();
        //                        pageInfoItem.page = (i + 1).ToString();
        //                        pageInfoItem.width = (pdfPage.Width).ToString();
        //                        pageInfoItem.height = (pdfPage.Height).ToString();
        //                        pageInfoItem.frameName = jiSuanFrame.name;
        //                        pageInfoItem.folded = jiSuanFrame.folded;

        //                        fileUpload.pageInfo.Add(pageInfoItem);
        //                    }
        //                    else
        //                    {
        //                        this.Hide();
        //                        ShowErrorMsg($"此文件{item.Name}，没有对应的图幅，请联系管理员！");
        //                        DialogResult = DialogResult.Cancel;
        //                        return;
        //                    }
        //                }

        //                fileUpload.frameName = fileUpload.pageInfo.First().frameName;
        //                fileUpload.folded = (fileUpload.pageInfo.Sum(o => Convert.ToDecimal(o.folded))).ToString();
        //            }

        //            item.fileUpload = fileUpload;
        //        }

        //        var parentInfo = directoryStructureList.FirstOrDefault(o => string.IsNullOrWhiteSpace(o.ParentId));

        //        if (!UploadFileAndFolder(parentInfo))
        //        {
        //            DialogResult = DialogResult.Cancel;
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        this.Hide();
        //        ShowErrorMsg("类型错误！");
        //        DialogResult = DialogResult.Cancel;
        //        return;
        //    }

        //    this.Hide();
        //    ShowSuccessMsg("上传成功！");
        //    DialogResult = DialogResult.OK;
        //}
        /// <summary>
        /// 上传文件夹
        /// </summary>
        /// <param name="directoryInfo">文件夹信息</param>
        /// <returns></returns>
        //private bool UploadFileAndFolder(DirectoryStructureModel directoryInfo)
        //{
        //    var paraFolder = new
        //    {
        //        dirName = directoryInfo.Name,
        //        parentId = string.IsNullOrWhiteSpace(directoryInfo.ParentId) ? parentId : directoryInfo.ParentId,
        //        type = dirType
        //    };

        //    var resultDirData = string.Empty;
        //    if (HttpPost(areaType == 0 ? AppGlobalModel.UploadProjectDir : AppGlobalModel.UploadKeepProjectDir, paraFolder, ref resultDirData))
        //    {
        //        //ProgressBar1.Value += 1;
        //        //ProgressBar1.Text = ((float)ProgressBar1.Value / (float)ProgressBar1.Maximum * 100).ToString("N0") + "%";
        //        IncreaseProgress();
        //        Dictionary<string, string> paras;
        //        var resultData = string.Empty;
        //        #region 上传文件
        //        //上传文件
        //        foreach (var item in directoryStructureList.Where(o => o.Type == 2 && o.ParentId == directoryInfo.PrimaryKey))
        //        {
        //            item.fileUpload.parentId = resultDirData;

        //            resultData = string.Empty;
        //            paras = new Dictionary<string, string>();
        //            paras.Add("fileDetails", JsonConvert.SerializeObject(item.fileUpload));
        //            if (HttpUploadFile(areaType == 0 ? AppGlobalModel.FileUpload : AppGlobalModel.KeepProjectFileUpload, item.Name, ref resultData, paras))
        //            {
        //                //ProgressBar1.Value += 1;
        //                //ProgressBar1.Text = ((float)ProgressBar1.Value / (float)ProgressBar1.Maximum * 100).ToString("N0") + "%";
        //                IncreaseProgress();
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        #endregion

        //        #region 创建文件夹
        //        foreach (var item in directoryStructureList.Where(o => o.Type == 1 && o.ParentId == directoryInfo.PrimaryKey))
        //        {
        //            item.ParentId = resultDirData;
        //            //UploadFileAndFolder(item);

        //            if (!UploadFileAndFolder(item))
        //            {
        //                return false;
        //            }
        //        }
        //        #endregion
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        private bool UploadFileAndFolder(DirectoryStructureModel directoryInfo, string parentServerId)
        {
            if (directoryInfo == null) return true;

            string currentFolderServerId;
            if (!resumeState.FolderServerIdMap.TryGetValue(directoryInfo.PrimaryKey, out currentFolderServerId))
            {
                var paraFolder = new
                {
                    dirName = directoryInfo.Name,
                    parentId = parentServerId,
                    type = dirType
                };

                var resultDirData = string.Empty;
                if (!HttpPost(areaType == 0 ? AppGlobalModel.UploadProjectDir : AppGlobalModel.UploadKeepProjectDir, paraFolder, ref resultDirData))
                {
                    return false;
                }

                currentFolderServerId = resultDirData;
                resumeState.FolderServerIdMap[directoryInfo.PrimaryKey] = currentFolderServerId;
                SaveResumeState();
            }

            IncreaseProgress();

            // 上传本目录文件
            foreach (var item in directoryStructureList.Where(o => o.Type == 2 && o.ParentId == directoryInfo.PrimaryKey))
            {
                if (IsFileUploaded(item.Name))
                {
                    IncreaseProgress();
                    continue;
                }

                item.fileUpload.parentId = currentFolderServerId;
                var resultData = string.Empty;
                var paras = new Dictionary<string, string>
        {
            { "fileDetails", JsonConvert.SerializeObject(item.fileUpload) }
        };

                if (!HttpUploadFile(areaType == 0 ? AppGlobalModel.FileUpload : AppGlobalModel.KeepProjectFileUpload, item.Name, ref resultData, paras))
                {
                    SaveResumeState();
                    return false;
                }

                MarkFileUploaded(item.Name);
                IncreaseProgress();
            }

            // 递归子目录（不改 item.ParentId，保持本地树稳定）
            foreach (var child in directoryStructureList.Where(o => o.Type == 1 && o.ParentId == directoryInfo.PrimaryKey))
            {
                if (!UploadFileAndFolder(child, currentFolderServerId))
                {
                    return false;
                }
            }

            return true;
        }

        #region 单点续传相关方法



        /// <summary>
        /// 初始化续传状态（仅 uploadType==1）
        /// </summary>
        private void InitResumeStateForSingleOrMultiFiles()
        {
            if (uploadType != 1 || directoryStructureList == null)
            {
                return;
            }

            var onlyFiles = directoryStructureList
                .Where(o => o != null && o.Type == 2 && !string.IsNullOrWhiteSpace(o.Name))
                .ToList();

            // 计算批次ID：同一批文件（路径+长度+最后写入时间）得到同一个ID
            var batchSeedBuilder = new StringBuilder();
            batchSeedBuilder.Append(parentId).Append("|")
                            .Append(areaType).Append("|")
                            .Append(uploadType).Append("|")
                            .Append(fileType).Append("|")
                            .Append(dirType).Append("|");

            foreach (var item in onlyFiles.OrderBy(o => o.Name, StringComparer.OrdinalIgnoreCase))
            {
                batchSeedBuilder.Append(CreateFileKey(item.Name)).Append(";");
            }

            var batchId = batchSeedBuilder.ToString().GetHashCode().ToString("X");

            var resumeDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "upload_resume");
            if (!Directory.Exists(resumeDir))
            {
                Directory.CreateDirectory(resumeDir);
            }

            resumeFilePath = Path.Combine(resumeDir, $"resume_{batchId}.json");

            // 读取已有续传状态
            if (File.Exists(resumeFilePath))
            {
                try
                {
                    var json = File.ReadAllText(resumeFilePath, Encoding.UTF8);
                    var state = JsonConvert.DeserializeObject<UploadResumeState>(json);
                    if (state != null)
                    {
                        resumeState = state;
                    }
                }
                catch
                {
                    // 读取失败则重置状态，避免影响上传
                    resumeState = null;
                }
            }

            if (resumeState == null)
            {
                resumeState = new UploadResumeState();
                resumeState.BatchId = batchId;
                SaveResumeState();
            }
        }

        /// <summary>
        /// 保存续传状态到本地
        /// </summary>
        private void SaveResumeState()
        {
            if (string.IsNullOrWhiteSpace(resumeFilePath) || resumeState == null)
            {
                return;
            }

            var json = JsonConvert.SerializeObject(resumeState);
            File.WriteAllText(resumeFilePath, json, Encoding.UTF8);
        }

        /// <summary>
        /// 上传全部成功后清理续传状态
        /// </summary>
        private void ClearResumeState()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(resumeFilePath) && File.Exists(resumeFilePath))
                {
                    File.Delete(resumeFilePath);
                }
            }
            catch
            {
                // 清理失败不影响主流程
            }
        }

        /// <summary>
        /// 根据文件路径生成文件键（用于判断是否已成功上传）
        /// </summary>
        private string CreateFileKey(string filePath)
        {
            try
            {
                var fi = new FileInfo(filePath);
                var full = fi.FullName.Trim().ToLowerInvariant();
                if (!fi.Exists)
                {
                    return full + "|NOT_EXISTS";
                }

                return $"{full}|{fi.Length}|{fi.LastWriteTimeUtc.Ticks}";
            }
            catch
            {
                return (filePath ?? "").Trim().ToLowerInvariant() + "|ERR";
            }
        }

        /// <summary>
        /// 判断文件是否已在续传状态中标记为成功
        /// </summary>
        private bool IsFileUploaded(string filePath)
        {
            if (resumeState == null)
            {
                return false;
            }

            var fileKey = CreateFileKey(filePath);
            return resumeState.SuccessFileKeys != null && resumeState.SuccessFileKeys.Contains(fileKey);
        }

        /// <summary>
        /// 标记文件已上传成功，并持久化
        /// </summary>
        private void MarkFileUploaded(string filePath)
        {
            if (resumeState == null)
            {
                return;
            }

            var fileKey = CreateFileKey(filePath);
            if (!resumeState.SuccessFileKeys.Contains(fileKey))
            {
                resumeState.SuccessFileKeys.Add(fileKey);
                SaveResumeState();
            }
        }

        /// <summary>
        /// 统一更新进度条
        /// </summary>
        private void IncreaseProgress()
        {
            if (ProgressBar1.Value < ProgressBar1.Maximum)
            {
                ProgressBar1.Value += 1;
            }
            ProgressBar1.Text = ((float)ProgressBar1.Value / (float)ProgressBar1.Maximum * 100).ToString("N0") + "%";
        }

        private void InitResumeState()
        {
            var resumeDir = GetResumeDir();
            var prefix = BuildResumePrefix(parentId, areaType, dirType);
            resumeFilePath = Path.Combine(resumeDir, $"{prefix}{uploadType}_{SafeName(fileType)}.json");

            if (File.Exists(resumeFilePath))
            {
                try
                {
                    var json = File.ReadAllText(resumeFilePath, Encoding.UTF8);
                    resumeState = JsonConvert.DeserializeObject<UploadResumeState>(json);
                }
                catch
                {
                    resumeState = null;
                }
            }

            if (resumeState == null)
            {
                resumeState = new UploadResumeState
                {
                    BatchId = Guid.NewGuid().ToString("N"),
                    ParentId = parentId,
                    AreaType = areaType,
                    DirType = dirType,
                    UploadType = uploadType,
                    FileType = fileType,
                    TaskItems = directoryStructureList == null
                        ? new List<DirectoryStructureModel>()
                        : JsonConvert.DeserializeObject<List<DirectoryStructureModel>>(JsonConvert.SerializeObject(directoryStructureList)),
                    Completed = false
                };
                SaveResumeState();
            }
            if ((resumeState.TaskItems == null || !resumeState.TaskItems.Any()) && directoryStructureList != null)
            {
                resumeState.TaskItems = JsonConvert.DeserializeObject<List<DirectoryStructureModel>>(
                    JsonConvert.SerializeObject(directoryStructureList));
                SaveResumeState();
            }
        }

        #endregion

    }
}
