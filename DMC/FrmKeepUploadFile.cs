using DMC.Helper;
using DMC.Models;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public partial class FrmKeepUploadFile : BaseForm
    {
        //1：文件，2：文件夹
        public int upload_type = 0;
        private string parentId = null;
        private List<string> fileList = new List<string>();
        private string type = "0";
        private List<JiSuanFrameModel> jiSuanFrameList = null;
        private List<DirectoryStructureModel> directoryStructureList = new List<DirectoryStructureModel>();
        public FrmKeepUploadFile(string objStr, string objType)
        {
            InitializeComponent();

            parentId = objStr;
            type = objType;
        }
        #region 简化方法 窗体移动,直接变化Left、Top
        private Point originLocation;

        private void WindowMove_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void WindowMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originLocation = e.Location;
            }
        }
        #endregion

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            upload_type = 1;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = AppGlobalModel.InitialDirectory;
            openFileDialog.Filter = "所有文件(*.*)|*.*";
            openFileDialog.Multiselect = true; //是否可以多选true=ok/false=no
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                #region 保存打开的文件目录
                AppGlobalModel.InitialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                ConfigHelper.SaveConfigInfo("InitialDirectory", AppGlobalModel.InitialDirectory);
                #endregion

                directoryStructureList = new List<DirectoryStructureModel>();
                string[] strNames = openFileDialog.FileNames;
                for (int i = 0; i < strNames.Length; i++)
                {
                    if (Path.GetExtension(strNames[i]).ToLower().Equals(".db") || Path.GetExtension(strNames[i]).ToLower().Equals(".log"))
                    {
                        continue;
                    }

                    if (!checkFileInList(strNames[i]))
                    {
                        fileListView.Items.Add(strNames[i]);
                        fileList.Add(strNames[i]);
                    }

                    btnUpload.Enabled = true;
                }
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private Boolean checkFileInList(String fileName)
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                if (fileName == fileList[i])
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            upload_type = 2;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileListView.Items.Clear();
                string foldPath = dialog.SelectedPath;

                directoryStructureList = new List<DirectoryStructureModel>();
                var thisOne = new DirectoryInfo(foldPath);
                ListTreeShow(thisOne, "");

                //修改回全部显示
                //var parentKey = directoryStructureList.FirstOrDefault(o => o.Name == thisOne.Name && o.Type == 1).PrimaryKey;
                //foreach (var item in directoryStructureList.Where(o => o.ParentId == parentKey && o.Type == 2))
                foreach (var item in directoryStructureList.Where(o => o.Type == 2))
                {
                    fileListView.Items.Add(item.Name);
                }

                if (fileListView.Items.Count > 0)
                {
                    btnUpload.Enabled = true;
                }
            }
            else
            {
                btnAdd.Enabled = true;
            }
        }

        /// <summary> 
        /// 获取指定文件夹下所有子目录及文件函数 
        /// </summary> 
        /// <param name="theDir">指定目录</param> 
        /// <param name="nLevel">默认起始值,调用时,一般为0</param> 
        /// <returns></returns> 
        public void ListTreeShow(DirectoryInfo theDir, string nLevel)//递归目录 文件 
        {
            var dirModel = new DirectoryStructureModel();
            dirModel.ParentId = nLevel;
            dirModel.PrimaryKey = Guid.NewGuid().ToString();
            dirModel.Name = theDir.Name.ToString();
            dirModel.Type = 1;
            directoryStructureList.Add(dirModel);

            FileInfo[] fileInfo = theDir.GetFiles(); //目录下的文件 
            foreach (FileInfo fInfo in fileInfo)
            {
                if (Path.GetExtension(fInfo.FullName).ToLower().Equals(".db") || Path.GetExtension(fInfo.FullName).ToLower().Equals(".log"))
                {
                    continue;
                }

                var fileModel = new DirectoryStructureModel();
                fileModel.ParentId = dirModel.PrimaryKey;
                fileModel.Name = fInfo.FullName;
                fileModel.Type = 2;
                directoryStructureList.Add(fileModel);
            }

            DirectoryInfo[] subDirectories = theDir.GetDirectories();//获得目录 
            foreach (DirectoryInfo dirinfo in subDirectories)
            {
                ListTreeShow(dirinfo, dirModel.PrimaryKey);
            }
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (ShowSuccessOKCancelMsg("是否确认上传！") == DialogResult.OK)
            {
                Splasher.Show(typeof(FrmLoading));
                FileUploadModel fileUpload;
                Dictionary<string, string> paras;
                string resultData;
                if (upload_type == 1)
                {
                    foreach (var item in directoryStructureList)
                    {
                        resultData = string.Empty;
                        paras = new Dictionary<string, string>();
                        fileUpload = new FileUploadModel();

                        fileUpload.parentId = parentId;
                        fileUpload.isPdf = "1";
                        fileUpload.fileTypeId = comFiletype.SelectedValue.ToString();

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
                                jiSuanFrame = jiSuanFrameList.FirstOrDefault(o => (o.minW <= pdfPage.Width && o.maxW >= pdfPage.Width) && (o.minH <= pdfPage.Height && o.maxH >= pdfPage.Height));

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
                                    ShowErrorMsg($"此文件{item.Name}，没有对应的图幅，请联系管理员！");
                                    return;
                                }
                            }

                            fileUpload.frameName = fileUpload.pageInfo.First().frameName;
                            fileUpload.folded = (fileUpload.pageInfo.Sum(o => Convert.ToDecimal(o.folded))).ToString();
                        }

                        paras.Add("fileDetails", JsonConvert.SerializeObject(fileUpload));
                        if (!HttpUploadFile(AppGlobalModel.KeepProjectFileUpload, item.Name, ref resultData, paras))
                        {
                            return;
                        }
                    }
                }
                else if (upload_type == 2)
                {
                    foreach (var item in directoryStructureList.Where(o => o.Type == 2))
                    {
                        fileUpload = new FileUploadModel();
                        fileUpload.isPdf = "1";
                        fileUpload.fileTypeId = comFiletype.SelectedValue.ToString();

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
                                jiSuanFrame = jiSuanFrameList.FirstOrDefault(o => (o.minW <= pdfPage.Width && o.maxW >= pdfPage.Width) && (o.minH <= pdfPage.Height && o.maxH >= pdfPage.Height));

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
                                    ShowErrorMsg($"此文件{item.Name}，没有对应的图幅，请联系管理员！");
                                    return;
                                }
                            }

                            fileUpload.frameName = fileUpload.pageInfo.First().frameName;
                            fileUpload.folded = (fileUpload.pageInfo.Sum(o => Convert.ToDecimal(o.folded))).ToString();
                        }

                        item.fileUpload = fileUpload;
                    }

                    var parentInfo = directoryStructureList.FirstOrDefault(o => string.IsNullOrWhiteSpace(o.ParentId));

                    if (!UploadFileAndFolder(parentInfo))
                    {
                        return;
                    }
                }
                else
                {
                    ShowErrorMsg("类型错误！");
                    return;
                }

                ShowSuccessMsg("上传成功！");
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 上传文件夹
        /// </summary>
        /// <param name="tempAttributeId"></param>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        private bool UploadFileAndFolder(DirectoryStructureModel directoryInfo)
        {
            var paraFolder = new
            {
                dirName = directoryInfo.Name,
                parentId = string.IsNullOrWhiteSpace(directoryInfo.ParentId) ? parentId : directoryInfo.ParentId,
                type = type
            };

            var resultDirData = string.Empty;
            if (HttpPost(AppGlobalModel.UploadKeepProjectDir, paraFolder, ref resultDirData))
            {
                Dictionary<string, string> paras;
                var resultData = string.Empty;
                #region 上传文件
                //上传文件
                foreach (var item in directoryStructureList.Where(o => o.Type == 2 && o.ParentId == directoryInfo.PrimaryKey))
                {
                    item.fileUpload.parentId = resultDirData;

                    resultData = string.Empty;
                    paras = new Dictionary<string, string>();
                    paras.Add("fileDetails", JsonConvert.SerializeObject(item.fileUpload));
                    if (!HttpUploadFile(AppGlobalModel.KeepProjectFileUpload, item.Name, ref resultData, paras))
                    {
                        return false;
                    }
                }
                #endregion

                #region 创建文件夹
                foreach (var item in directoryStructureList.Where(o => o.Type == 1 && o.ParentId == directoryInfo.PrimaryKey))
                {
                    item.ParentId = resultDirData;
                    UploadFileAndFolder(item);
                }
                #endregion
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileListView_Click(object sender, EventArgs e)
        {
            fileList.RemoveAt(fileListView.Items.IndexOf(fileListView.SelectedItems[0]));
            fileListView.Items.Remove(fileListView.SelectedItems[0]);

            if (fileListView.Items.Count == 0)
            {
                button1.Enabled = true;
                btnAdd.Enabled = true;
                upload_type = 0;
                btnUpload.Enabled = false;
            }
        }

        private void FrmUploadFile_Load(object sender, EventArgs e)
        {
            var resultData = new List<GetFileTypeListModel>();
            if (HttpGet(AppGlobalModel.GetFileTypeList, ref resultData))
            {
                comFiletype.DataSource = resultData;
                comFiletype.DisplayMember = "name";
                comFiletype.ValueMember = "id";
            }
            else
            {
                this.Close();
            }

            if (!HttpGet(AppGlobalModel.JiSuanFrame, ref jiSuanFrameList))
            {
                this.Close();
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
