using DMC.Helper;
using DMC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public partial class FrmUploadFile : BaseForm
    {
        //1：文件，2：文件夹
        public int upload_type = 0;
        //归档管理使用
        private string dirType = "";
        //项目ID
        private string projectId = null;
        //判断是不是医药设计院，是关闭文件类型选项

        /// <summary>
        /// 类型0项目，1阶段，2专业，3子项，4文件夹，5文件
        /// </summary>
        private int proType = -1;
        /// <summary>
        /// 计算文件图幅
        /// </summary>
        private List<JiSuanFrameModel> jiSuanFrameList = null;
        /// <summary>
        /// 文件夹结构
        /// </summary>
        private List<DirectoryStructureModel> directoryStructureList = new List<DirectoryStructureModel>();
        /// <summary>
        /// 上传文件加载窗体
        /// </summary>
        /// <param name="objStr"></param>
        /// <param name="objType"></param>
        public FrmUploadFile(string objStr, string objType = "")
        {
            InitializeComponent();

            projectId = objStr;
            dirType = objType;

        }

        /// <summary>
        /// 上传文件加载窗体
        /// </summary>
        /// <param name="objStr"></param>
        /// <param name="objProType"></param>
        public FrmUploadFile(string objStr, int objProType)
        {
            InitializeComponent();

            projectId = objStr;
            proType = objProType;

            btnAdd.Visible = proType == 2 ? false : true;
           
            if (GlobalVariables.companyName == "吉林医药设计院有限公司")
            {
                //comFiletype.Enabled = false;
                //comFiletype.SelectedIndex = 1;
            }
            if (GlobalVariables.companyName == "辽宁方大工程设计有限公司")
            {
                //comFiletype.Enabled = false;
                //comFiletype.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (GlobalVariables.companyName == "辽宁方大工程设计有限公司")
            {
                //comFiletype.Enabled = false;
                comFiletype.SelectedIndex = 0;
            }
            if (GlobalVariables.companyName == "吉林医药设计有限公司")
            {
                comFiletype.Enabled = false;
                comFiletype.SelectedIndex = 1;
            }
            TryRestorePendingTask();
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
        /// <summary>
        /// 窗口拉申方法
        /// </summary>
        /// <param name="m"></param>
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

        /// <summary>
        /// 原始位置
        /// </summary>
        private Point originLocation;

        /// <summary>
        /// 窗口移动
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
        /// 对窗口点下鼠标键
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
        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            butFolder.Enabled = false;
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

                        var fileModel = new DirectoryStructureModel();
                        fileModel.ParentId = "";
                        fileModel.Name = strNames[i];
                        fileModel.Type = 2;
                        directoryStructureList.Add(fileModel);
                    }

                    btnUpload.Enabled = true;
                }
            }
            else
            {
                butFolder.Enabled = true;
            }
        }

        /// <summary>
        /// 检查文件是否在列表中
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private Boolean checkFileInList(String fileName)
        {
            if (directoryStructureList != null)
            {
                for (int i = 0; i < directoryStructureList.Count; i++)
                {
                    if (fileName == directoryStructureList[i].Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_选择文件夹_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            upload_type = 2;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            dialog.SelectedPath = AppGlobalModel.InitialDirectory;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                #region 保存打开的文件目录
                AppGlobalModel.InitialDirectory = dialog.SelectedPath;
                ConfigHelper.SaveConfigInfo("InitialDirectory", AppGlobalModel.InitialDirectory);
                #endregion

                directoryStructureList = new List<DirectoryStructureModel>();
                fileListView.Items.Clear();
                string foldPath = dialog.SelectedPath;

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
                var frm = new FrmUploadProgress(projectId,
                    string.IsNullOrWhiteSpace(dirType) ? 0 : 1,
                    dirType,
                    upload_type,
                    comFiletype.SelectedValue.ToString(),
                    directoryStructureList,
                    jiSuanFrameList);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileListView_Click(object sender, EventArgs e)
        {
            directoryStructureList.Remove(directoryStructureList.FirstOrDefault(o => o.Name == fileListView.SelectedItems[0].Text));
            fileListView.Items.Remove(fileListView.SelectedItems[0]);

            if (fileListView.Items.Count == 0)
            {
                directoryStructureList = new List<DirectoryStructureModel>();
                butFolder.Enabled = true;
                btnAdd.Enabled = true;
                upload_type = 0;
                btnUpload.Enabled = false;
            }
        }

       

        /// <summary>
        /// 按键点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_OffWindow_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 添加补充文件上传功能的相关方法
        

        /// <summary>
        /// 根据项目ID通过后端 API 查询目标 folderId 并上传补充文件（不再直接访问 MySQL）
        /// </summary>
        /// <param name="projectId">项目 ID</param>
        /// <returns>上传后的文件 ID，失败返回空字符串</returns>
        //public static string UploadSupplementaryFileForProject(string projectId)
        //{
        //    try
        //    {
        //        // 1）通过后端 API 获取目标 folder（后端负责所有 MySQL 查询）
        //        var target = new SupplementaryTargetModel();
        //        string getUrl = AppGlobalModel.GetSupplementaryTargetFolder + $"?projectId={Uri.EscapeDataString(projectId)}";

        //        if (!HttpGet(getUrl, ref target) || target == null || string.IsNullOrWhiteSpace(target.folderId))
        //        {
        //            // 记录并提示
        //            LogHelper.WriteLocalLog($"UploadSupplementaryFileForProject: 无法从后端获取目标文件夹，projectId={projectId}");
        //            MessageBox.Show("无法获取上传目标文件夹（后端），请联系管理员或检查网络。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return string.Empty;
        //        }

        //        // 2）复用现有本地上传流程将资源文件上传到后端（内部会调用 FrmUploadProgress -> HttpUploadFile）
        //        return UploadSupplementaryFileToFolder(target.folderId, target.folderName);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLocalErrorLog("FrmUploadFile", ex, $"UploadSupplementaryFileForProject projectId={projectId}");
        //        MessageBox.Show($"上传补充文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return string.Empty;
        //    }
        //}

        #region 华商不好用的上传补充文件方法
        /// <summary>
        /// 根据项目ID上传补充文件到指定位置
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns>上传的文件ID，如果上传失败则返回空字符串</returns>
        public static string UploadSupplementaryFileForProject(string projectId)
        {
            #region 没加写入日志的方法
            try
            {
                // 1. 根据项目ID查询阶段
                var stages = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", projectId, "type", "2");

                if (stages.Rows.Count == 0)
                {
                    MessageBox.Show("该项目下没有找到任何阶段！");
                    return string.Empty;
                }

                // 2. 遍历阶段，查找专业
                foreach (DataRow stageRow in stages.Rows)
                {
                    string stageId = stageRow["id"].ToString();

                    // 查询该阶段下的所有专业
                    var majors = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", stageId, "type", "3");

                    // 检查是否存在"00-技术资料"专业
                    DataRow technicalDataMajor = null;
                    foreach (DataRow majorRow in majors.Rows)
                    {
                        if (majorRow["name"].ToString().Equals("00-技术资料"))
                        {
                            technicalDataMajor = majorRow;
                            break;
                        }
                    }
                    // 3. 如果找到"00-技术资料"专业，继续查找该专业下的"00-技术资料"文件夹
                    if (technicalDataMajor != null)
                    {
                        // 1. 如果找到"00-技术资料"专业
                        string majorId = technicalDataMajor["id"].ToString();

                        // 查询该专业下的所有文件夹
                        var folders = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", majorId, "type", "4");

                        // 查找"00-技术资料"文件夹
                        DataRow technicalDataFolder = null;
                        foreach (DataRow folderRow in folders.Rows)
                        {
                            // 2. 在"00-技术资料"专业下找到"00-技术资料"文件夹
                            if (folderRow["name"].ToString().Equals("00-技术资料"))
                            {
                                technicalDataFolder = folderRow;
                                break;
                            }
                        }
                        // 3. 如果在"00-技术资料"专业下找到"00-技术资料"文件夹，则上传补充文件到该文件夹；如果没有找到，则使用该专业下的第一个文件夹上传补充文件
                        if (technicalDataFolder != null)
                        {
                            string folderId = technicalDataFolder["id"].ToString();
                            string folderName = technicalDataFolder["name"].ToString();

                            // 上传Resources文件夹中的"补充.txt"文件到该文件夹
                            return UploadSupplementaryFileToFolder(folderId, folderName);
                        }
                        else
                        {
                            // 如果在"00-技术资料"专业下没有找到"00-技术资料"文件夹，则使用该专业下的第一个文件夹
                            if (folders.Rows.Count > 0)
                            {
                                string firstFolderId = folders.Rows[0]["id"].ToString();
                                string firstFolderName = folders.Rows[0]["name"].ToString();

                                // 上传Resources文件夹中的"补充.txt"文件到第一个文件夹
                                return UploadSupplementaryFileToFolder(firstFolderId, firstFolderName);
                            }
                        }
                    }
                    else
                    {
                        // 2. 如果没有"00-技术资料"专业，使用第一个专业
                        if (majors.Rows.Count > 0)
                        {
                            DataRow firstMajor = majors.Rows[0];
                            string firstMajorId = firstMajor["id"].ToString();
                            string firstMajorName = firstMajor["name"].ToString();

                            // 查询该专业下的所有文件夹
                            var folders = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", firstMajorId, "type", "4");

                            if (folders.Rows.Count > 0)
                            {
                                // 使用第一个文件夹
                                string firstFolderId = folders.Rows[0]["id"].ToString();
                                string firstFolderName = folders.Rows[0]["name"].ToString();

                                // 上传Resources文件夹中的"补充.txt"文件到第一个文件夹
                                return UploadSupplementaryFileToFolder(firstFolderId, firstFolderName);
                            }
                            else
                            {
                                var subProject = SQLiteDataBase.GetDataFromMysql("qz_project", "id", firstMajorId);
                                // 使用第一个文件夹
                                string firstFolderId = subProject.Rows[0]["id"].ToString();
                                string firstFolderName = subProject.Rows[0]["name"].ToString();

                                // 上传Resources文件夹中的"补充.txt"文件到第一个文件夹
                                return UploadSupplementaryFileToFolder(firstFolderId, firstFolderName);
                            }
                        }
                    }
                }
                //LogHelper.WriteLocalLog($"未能找到合适的目标文件夹来上传补充文件: {stages.TableName},{stages.Rows},{stages.Columns},{stages.Namespace}");
                MessageBox.Show("未能找到合适的目标文件夹来上传补充文件");
                return string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"上传补充文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
            #endregion           
        }

        //public static string UploadSupplementaryFileForProject(string projectId)
        //{
        //    try
        //    {
        //        LogHelper.WriteLocalLog($"UploadSupplementaryFileForProject START - projectId={projectId}");

        //        // 1. 根据项目ID查询阶段
        //        LogHelper.WriteLocalLog("Step: Query stages from qz_project where parent_id = projectId and type = 2");
        //        var stages = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", projectId, "type", "2");
        //        LogHelper.WriteLocalLog($"Query stages completed - resultRows={(stages == null ? "null" : stages.Rows.Count.ToString())}");

        //        if (stages == null || stages.Rows.Count == 0)
        //        {
        //            LogHelper.WriteLocalLog("No stages found for projectId=" + projectId);
        //            MessageBox.Show("该项目下没有找到任何阶段！");
        //            return string.Empty;
        //        }

        //        // 2. 遍历阶段，查找专业
        //        foreach (DataRow stageRow in stages.Rows)
        //        {
        //            string stageId = stageRow["id"].ToString();
        //            LogHelper.WriteLocalLog($"Processing stageId={stageId}");

        //            // 查询该阶段下的所有专业
        //            LogHelper.WriteLocalLog($"Step: Query majors from qz_project where parent_id = {stageId} and type = 3");
        //            var majors = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", stageId, "type", "3");
        //            LogHelper.WriteLocalLog($"Query majors completed - resultRows={(majors == null ? "null" : majors.Rows.Count.ToString())}");

        //            // 检查是否存在"00-技术资料"专业
        //            DataRow technicalDataMajor = null;
        //            if (majors != null)
        //            {
        //                foreach (DataRow majorRow in majors.Rows)
        //                {
        //                    if (majorRow["name"].ToString().Equals("00-技术资料"))
        //                    {
        //                        technicalDataMajor = majorRow;
        //                        LogHelper.WriteLocalLog($"Found technicalDataMajor id={majorRow["id"]}, name={majorRow["name"]}");
        //                        break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                LogHelper.WriteLocalLog($"Majors query returned null for stageId={stageId}");
        //            }

        //            // 3. 如果找到"00-技术资料"专业，继续查找该专业下的"00-技术资料"文件夹
        //            if (technicalDataMajor != null)
        //            {
        //                string majorId = technicalDataMajor["id"].ToString();
        //                LogHelper.WriteLocalLog($"Step: Query folders under majorId={majorId} where type = 4");
        //                var folders = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", majorId, "type", "4");
        //                LogHelper.WriteLocalLog($"Query folders completed - resultRows={(folders == null ? "null" : folders.Rows.Count.ToString())}");

        //                DataRow technicalDataFolder = null;
        //                if (folders != null)
        //                {
        //                    foreach (DataRow folderRow in folders.Rows)
        //                    {
        //                        if (folderRow["name"].ToString().Equals("00-技术资料"))
        //                        {
        //                            technicalDataFolder = folderRow;
        //                            LogHelper.WriteLocalLog($"Found technicalDataFolder id={folderRow["id"]}, name={folderRow["name"]}");
        //                            break;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    LogHelper.WriteLocalLog($"Folders query returned null for majorId={majorId}");
        //                }

        //                if (technicalDataFolder != null)
        //                {
        //                    string folderId = technicalDataFolder["id"].ToString();
        //                    string folderName = technicalDataFolder["name"].ToString();
        //                    LogHelper.WriteLocalLog($"Uploading to folder (technicalDataFolder) id={folderId}, name={folderName}");
        //                    return UploadSupplementaryFileToFolder(folderId, folderName);
        //                }
        //                else
        //                {
        //                    LogHelper.WriteLocalLog("technicalDataFolder not found under majorId=" + majorId);
        //                    if (folders != null && folders.Rows.Count > 0)
        //                    {
        //                        string firstFolderId = folders.Rows[0]["id"].ToString();
        //                        string firstFolderName = folders.Rows[0]["name"].ToString();
        //                        LogHelper.WriteLocalLog($"Uploading to first folder under majorId: id={firstFolderId}, name={firstFolderName}");
        //                        return UploadSupplementaryFileToFolder(firstFolderId, firstFolderName);
        //                    }
        //                    else
        //                    {
        //                        LogHelper.WriteLocalLog($"No folders found under majorId={majorId}");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                LogHelper.WriteLocalLog($"technicalDataMajor not found for stageId={stageId}");
        //                // 2. 如果没有"00-技术资料"专业，使用第一个专业
        //                if (majors != null && majors.Rows.Count > 0)
        //                {
        //                    DataRow firstMajor = majors.Rows[0];
        //                    string firstMajorId = firstMajor["id"].ToString();
        //                    string firstMajorName = firstMajor["name"].ToString();
        //                    LogHelper.WriteLocalLog($"Using first major id={firstMajorId}, name={firstMajorName}");

        //                    LogHelper.WriteLocalLog($"Step: Query folders under firstMajorId={firstMajorId} where type = 4");
        //                    var folders = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", firstMajorId, "type", "4");
        //                    LogHelper.WriteLocalLog($"Query folders completed - resultRows={(folders == null ? "null" : folders.Rows.Count.ToString())}");

        //                    if (folders != null && folders.Rows.Count > 0)
        //                    {
        //                        string firstFolderId = folders.Rows[0]["id"].ToString();
        //                        string firstFolderName = folders.Rows[0]["name"].ToString();
        //                        LogHelper.WriteLocalLog($"Uploading to first folder under firstMajor: id={firstFolderId}, name={firstFolderName}");
        //                        return UploadSupplementaryFileToFolder(firstFolderId, firstFolderName);
        //                    }
        //                    else
        //                    {
        //                        LogHelper.WriteLocalLog($"No folders under firstMajorId={firstMajorId}, trying subProject query");
        //                        var subProject = SQLiteDataBase.GetDataFromMysql("qz_project", "id", firstMajorId);
        //                        LogHelper.WriteLocalLog($"Query subProject completed - resultRows={(subProject == null ? "null" : subProject.Rows.Count.ToString())}");
        //                        if (subProject != null && subProject.Rows.Count > 0)
        //                        {
        //                            string firstFolderId = subProject.Rows[0]["id"].ToString();
        //                            string firstFolderName = subProject.Rows[0]["name"].ToString();
        //                            LogHelper.WriteLocalLog($"Uploading to subProject folder id={firstFolderId}, name={firstFolderName}");
        //                            return UploadSupplementaryFileToFolder(firstFolderId, firstFolderName);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    LogHelper.WriteLocalLog($"No majors found for stageId={stageId}");
        //                }
        //            }
        //        }

        //        LogHelper.WriteLocalLog("未能找到合适的目标文件夹来上传补充文件 - projectId=" + projectId);
        //        MessageBox.Show("未能找到合适的目标文件夹来上传补充文件");
        //        return string.Empty;
        //    }
        //    catch (Exception ex)
        //    {
        //        // 记录完整异常堆栈，便于定位是哪个数据库调用抛出的错误
        //        try
        //        {
        //            LogHelper.WriteLocalErrorLog("FrmUploadFile", ex, $"UploadSupplementaryFileForProject projectId={projectId}");
        //        }
        //        catch
        //        {
        //            // 如果 LogHelper 也出错，仍然捕获防止二次抛出
        //            try
        //            {
        //                File.AppendAllText(Path.Combine(Application.StartupPath, "UploadSupplementaryFileForProject_error.log"),
        //                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | Exception: " + ex.ToString() + Environment.NewLine);
        //            }
        //            catch { }
        //        }

        //        MessageBox.Show($"上传补充文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return string.Empty;
        //    }
        //}

        #endregion


        /// <summary>
        /// 上传补充文件到指定文件夹
        /// </summary>
        /// <param name="folderId">目标文件夹ID</param>
        /// <param name="folderName">目标文件夹名称</param>
        /// <returns>上传的文件ID，如果上传失败则返回空字符串</returns>
        private static string UploadSupplementaryFileToFolder(string folderId, string folderName)
        {
            #region 没有历史记录的方法
            try
            {
                string fileContent = null;

                // 从 Properties.Resources 获取文件内容
                try
                {
                    fileContent = Properties.Resources.补充;
                    if (string.IsNullOrEmpty(fileContent))
                    {
                        //MessageBox.Show("无法从资源中获取补充.txt 内容");
                        return string.Empty;
                    }
                    Console.WriteLine("从 Properties.Resources 成功获取补充.txt 内容");
                }
                catch (Exception resEx)
                {
                    MessageBox.Show($"从 Properties.Resources 获取内容失败: {resEx.Message}");
                    return string.Empty;
                }

                // 将内容写入临时文件
                string tempFilePath = Path.Combine(Path.GetTempPath(), "补充.txt");
                File.WriteAllText(tempFilePath, fileContent, Encoding.UTF8);

                // 读取临时文件内容
                byte[] fileBytes = File.ReadAllBytes(tempFilePath);
                string fileName = "补充.txt";

                // 准备上传参数
                var uploadParams = new
                {
                    folderId = folderId,
                    fileName = fileName,
                    fileContent = Convert.ToBase64String(fileBytes),
                    fileSize = fileBytes.Length,
                    createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    type = 5, // 文件类型，表示文件
                };

                // 执行文件上传
                string uploadedFileId = PerformFileUpload(uploadParams);

                // 删除临时文件
                if (File.Exists(tempFilePath))
                {
                    try
                    {
                        File.Delete(tempFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"删除临时文件失败: {ex.Message}");
                    }
                }

                if (!string.IsNullOrEmpty(uploadedFileId))
                {
                    MessageBox.Show($"成功上传补充文件 '{fileName}' 到文件夹 '{folderName}'");
                    return uploadedFileId; // 返回上传的文件ID
                }
                else
                {
                    MessageBox.Show($"上传补充文件失败");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"上传补充文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
            #endregion
        
        }

        /// <summary>
        /// 执行文件上传的具体实现
        /// </summary>
        private static string PerformFileUpload(dynamic uploadParams)
        {
            #region 没有记录log的方法
            try
            {
                // 创建临时文件在应用程序目录中
                string tempFileName = Convert.ToString(uploadParams.fileName);
                string tempFilePath = Path.Combine(Application.StartupPath, tempFileName);

                // 解码base64内容并写入临时文件
                byte[] fileBytes = Convert.FromBase64String(uploadParams.fileContent);
                File.WriteAllBytes(tempFilePath, fileBytes);

                // 创建DirectoryStructureModel列表来模拟文件上传
                var directoryStructureList = new List<DirectoryStructureModel>();

                var fileModel = new DirectoryStructureModel();
                fileModel.ParentId = Convert.ToString(uploadParams.folderId);
                fileModel.Name = tempFilePath;
                fileModel.Type = 2; // 文件类型

                fileModel.fileUpload = new FileUploadModel
                {
                    parentId = Convert.ToString(uploadParams.folderId),
                    fileTypeId = Convert.ToString(uploadParams.type),
                    isPdf = "0",
                    pageAll = "1",
                    frameName = "补充文件",
                    folded = "0",
                    pageInfo = new List<PageInfoItem>()
                };

                directoryStructureList.Add(fileModel);

                // 使用现有的上传进度窗体
                var frm = new FrmUploadProgress(
                    Convert.ToString(uploadParams.folderId),
                    0, // fileType
                    "", // dirType
                    1,  // uploadType (1=file, 2=folder)
                    Convert.ToString(uploadParams.fileName), // filetype
                    directoryStructureList,
                    null // jiSuanFrameList
                );

                // 执行上传
                var result = frm.ShowDialog();

               
                // 返回上传结果
                if (result == DialogResult.OK)
                {
                    // 上传成功后，查询数据库获取真实的文件ID
                    // 这里需要根据您的具体情况调整查询逻辑
                    //string realFileId = SQLiteDataBase.GetLatestUploadedFileId(Convert.ToString(uploadParams.folderId), Convert.ToString(uploadParams.fileName));
                    string realFileId = "true";
                    // 上传完成后删除临时文件
                    if (File.Exists(tempFilePath))
                    {
                        try
                        {
                            File.Delete(tempFilePath);
                        }
                        catch (Exception deleteEx)
                        {
                            Console.WriteLine($"删除临时文件失败: {deleteEx.Message}");
                        }
                    }
                    return realFileId; // 返回真实的文件ID
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"文件上传错误: {ex.Message}");
                return string.Empty;
            }
            #endregion
           
        }

        #endregion

        #region 断点续传相关方法
        /// <summary>
        /// 尝试恢复未完成的上传任务（如果存在），并询问用户是否继续上传剩余文件
        /// </summary>
        private void TryRestorePendingTask()
        {
            var areaType = string.IsNullOrWhiteSpace(dirType) ? 0 : 1;
            FrmUploadProgress.ResumeTaskInfo info;

            if (!FrmUploadProgress.TryLoadPendingTask(projectId, areaType, dirType, out info))
            {
                return;
            }

            var msg = $"检测到未完成上传任务。\r\n剩余文件：{info.PendingFileList.Count}\r\n是否恢复并继续上传？";
            if (MessageBox.Show(msg, "续传提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                return;
            }

            upload_type = info.UploadType;
            directoryStructureList = info.DirectoryStructureList ?? new List<DirectoryStructureModel>();

            if (!string.IsNullOrWhiteSpace(info.FileType))
            {
                comFiletype.SelectedValue = info.FileType;
            }

            fileListView.Items.Clear();
            foreach (var file in info.PendingFileList)
            {
                fileListView.Items.Add(file);
            }

            btnUpload.Enabled = info.PendingFileList.Any();
            btnAdd.Enabled = upload_type != 2;
            butFolder.Enabled = upload_type != 1;
        }

        #endregion
    }
}