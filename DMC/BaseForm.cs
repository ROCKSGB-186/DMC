using DMC.Helper;
using DMC.Models;
using Mysqlx.Crud;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DMC
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();

        }
        /// <summary>
        /// GET请求(方法一):string getUrl, ref T resultDataModel
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="getUrl"></param>
        /// <param name="resultDataModel"></param>
        /// <returns></returns>
        public bool HttpGet<T>(string getUrl, ref T resultDataModel)
        {            
            var headers = new Dictionary<string, string>();
            headers.Add("token", AppGlobalModel.Token);
            var resultData = string.Empty;
            if (HttpHelper.GetData(getUrl, headers, ref resultData))
            {
                var resultModel = JsonConvert.DeserializeObject<ResultModel<T>>(resultData);

                if (resultModel.code == 200)
                {
                    resultDataModel = resultModel.data;
                    return true;
                }
                else
                {
                    if (resultModel.code == -13)
                    {
                        ShowErrorMsg(resultModel.msg);
                        DelTempFile();
                        this.Dispose();
                        Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        //关闭所有的线程
                        Process.GetCurrentProcess().Kill();
                        return false;
                    }
                    else
                    {
                        ShowErrorMsg(resultModel.msg);
                        return false;
                    }
                }
            }
            else
            {
                ShowErrorMsg(resultData);
                return false;
            }
        }

        /// <summary>
        /// GET请求(方法二):string getUrl, ref T resultDataModel, ref int total
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getUrl"></param>
        /// <param name="resultDataModel"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public bool HttpGet<T>(string getUrl, ref T resultDataModel, ref int total)
        {
            var headers = new Dictionary<string, string>();
            headers.Add("token", AppGlobalModel.Token);
            var resultData = string.Empty;
            if (HttpHelper.GetData(getUrl, headers, ref resultData))
            {
                var resultModel = JsonConvert.DeserializeObject<ResultModel<T>>(resultData);

                if (resultModel.code == 200)
                {
                    total = resultModel.total;
                    resultDataModel = resultModel.data;
                    return true;
                }
                else
                {
                    if (resultModel.code == -13)
                    {
                        ShowErrorMsg(resultModel.msg);
                        DelTempFile();
                        this.Dispose();
                        Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        //关闭所有的线程
                        Process.GetCurrentProcess().Kill();
                        return false;
                    }
                    else
                    {
                        ShowErrorMsg(resultModel.msg);
                        return false;
                    }
                }
            }
            else
            {
                ShowErrorMsg(resultData);
                return false;
            }
        }

        /// <summary>
        /// POST请求(方法一):string postUrl, string postData, ref T resultDataModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="postUrl"></param>
        /// <param name="postData"></param>
        /// <param name="resultDataModel"></param>
        /// <returns></returns>
        public bool HttpPost<T>(string postUrl, string postData, ref T resultDataModel)
        {
            var headers = new Dictionary<string, string>();
            headers.Add("token", AppGlobalModel.Token);
            var resultData = string.Empty;
            if (HttpHelper.PostData(postUrl, postData, headers, ref resultData))
            {
                var resultModel = JsonConvert.DeserializeObject<ResultModel<T>>(resultData);

                if (resultModel.code == 200)
                {
                    resultDataModel = resultModel.data;
                    return true;
                }
                else
                {
                    if (resultModel.code == -13)
                    {
                        ShowErrorMsg(resultModel.msg);
                        DelTempFile();
                        this.Dispose();
                        Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        //关闭所有的线程
                        Process.GetCurrentProcess().Kill();
                        return false;
                    }
                    else
                    {
                        ShowErrorMsg(resultModel.msg);
                        return false;
                    }
                }
            }
            else
            {
                ShowErrorMsg(resultData);
                return false;
            }
        }

        /// <summary>
        /// POST请求(方法二):string postUrl, T2 paraData, ref T resultDataModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="postUrl"></param>
        /// <param name="paraData"></param>
        /// <param name="resultDataModel"></param>
        /// <returns></returns>
        public bool HttpPost<T, T2>(string postUrl, T2 paraData, ref T resultDataModel)
        {
            var postData = HttpHelper.GetPostData(paraData);
            return HttpPost(postUrl, postData, ref resultDataModel);
        }

        /// <summary>
        /// POST请求(方法三):string postUrl, T2 paraData, ref T resultDataModel, ref int total
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="postUrl">服务器地址</param>
        /// <param name="paraData">提供的数据类型</param>
        /// <param name="resultDataModel">返回的数据类型</param>
        /// <param name="total">返回条目数量</param>
        /// <returns></returns>
        public bool HttpPost<T, T2>(string postUrl, T2 paraData, ref T resultDataModel, ref int total)
        {
            var headers = new Dictionary<string, string>();
            headers.Add("token", AppGlobalModel.Token);
            var resultData = string.Empty;
            var postData = HttpHelper.GetPostData(paraData);
            if (HttpHelper.PostData(postUrl, postData, headers, ref resultData))
            {
                var resultModel = JsonConvert.DeserializeObject<ResultModel<T>>(resultData);

                if (resultModel.code == 200)
                {
                    total = resultModel.total;
                    resultDataModel = resultModel.data;
                    return true;
                }
                else
                {
                    if (resultModel.code == -13)
                    {
                        ShowErrorMsg(resultModel.msg);
                        DelTempFile();
                        this.Dispose();
                        Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        //关闭所有的线程
                        Process.GetCurrentProcess().Kill();
                        return false;
                    }
                    else
                    {
                        ShowErrorMsg(resultModel.msg);
                        return false;
                    }
                }
            }
            else
            {
                ShowErrorMsg(resultData);
                return false;
            }
        }

        /// <summary>
        /// 后台推送更新 HttpUploadFile
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">url路径</param>
        /// <param name="path">路径</param>
        /// <param name="resultDataModel"></param>
        /// <param name="paras"></param>
        /// <param name="paraName"></param>
        /// <returns></returns>
        public bool HttpUploadFile<T>(string url, string path, ref T resultDataModel, Dictionary<string, string> paras = null, string paraName = "file")
        {
            var resultData = string.Empty;
            if (HttpHelper.HttpUploadFile(url, AppGlobalModel.Token, path, ref resultData, paras, paraName))
            {
                var resultModel = JsonConvert.DeserializeObject<ResultModel<T>>(resultData);

                if (resultModel.code == 200)
                {
                    resultDataModel = resultModel.data;
                    return true;
                }
                else
                {
                    if (resultModel.code == -13)
                    {
                        ShowErrorMsg(resultModel.msg);
                        DelTempFile();
                        this.Dispose();
                        Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        //关闭所有的线程
                        Process.GetCurrentProcess().Kill();
                        return false;
                    }
                    else
                    {
                        ShowErrorMsg(resultModel.msg);
                        return false;
                    }
                }
            }
            else
            {
                ShowErrorMsg(resultData);
                return false;
            }
        }

        /// <summary>
        /// 发送错误提示或者问题消息
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <returns></returns>
        public DialogResult ShowErrorMsg(string msg)
        {
            Splasher.Close();
            var result = DialogResult.No;
            result = MessageBox.Show(this, msg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return result;
        }

        /// <summary>
        /// 发送成功消息（可能这个地方存在问题）
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DialogResult ShowSuccessMsg(string msg)
        {
            Splasher.Close();
            var result = DialogResult.No;
            result = MessageBox.Show(this, msg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return result;
        }
        /// <summary>
        /// 给用户提示确认消息框
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DialogResult ShowSuccessOKCancelMsg(string msg)
        {
            var result = DialogResult.No;
            result = MessageBox.Show(this, msg, "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            return result;
        }
        /// <summary>
        /// 窗体显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseForm_Shown(object sender, EventArgs e)
        {
            Splasher.Close();
            this.Activate();
        }

        /// <summary>
        /// 删除临时文件夹
        /// </summary>
        public void DelTempFile()
        {
            var dir = Environment.CurrentDirectory + $"\\TempFile";
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }

            var logsDir = Environment.CurrentDirectory + $"\\logs";
            if (Directory.Exists(logsDir))
            {
                DirectoryInfo folder = new DirectoryInfo(logsDir);
                var folderList = folder.GetFiles("*.log");
                if (folderList.Any())
                {
                    foreach (FileInfo itemFile in folderList.Where(o => o.CreationTime < DateTime.Now.Date))
                    {
                        File.Delete(itemFile.FullName);
                    }
                }
            }
        }


        #region 日志文件方法


        // 在类的字段部分添加
        /// <summary>
        /// 日志文件存储路径
        /// </summary>
        public static string 日志文件目录;

        /// <summary>
        /// 日志文件路径
        /// </summary>
        public static string 操作日志文件路径;
        public static string 错误日志文件路径;

        /// <summary>
        /// 日志级别枚举
        /// </summary>
        public enum LogLevel
        {
            Debug = 0,
            Info = 1,
            Warning = 2,
            Error = 3,
            Fatal = 4
        }

        /// <summary>
        /// 日志条目模型
        /// </summary>
        public class LogEntry
        {
            public DateTime DateTime { get; set; }
            public LogLevel Level { get; set; }
            /// <summary>
            /// 模块名称
            /// </summary>
            public string Module { get; set; }
            public string Message { get; set; }
            /// <summary>
            /// 详细信息
            /// </summary>
            public string Details { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
            public string UserDeptName { get; set; }
            public string ProjectID { get; set; }
            public string Version { get; set; }


        }

        /// <summary>
        /// 初始化日志系统
        /// </summary>
        public static void 初始化日志系统(string versionStr, string realName, string userDeptName)
        {
            try
            {
                // 设置日志目录为程序安装目录下的Logs文件夹
                string 程序目录 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                日志文件目录 = System.IO.Path.Combine(程序目录, "Logs");

                // 确保日志目录存在
                if (!Directory.Exists(日志文件目录))
                {
                    Directory.CreateDirectory(日志文件目录);
                }

                // 设置日志文件路径
                string 日期前缀 = DateTime.Now.ToString("yyyy-MM");
                操作日志文件路径 = System.IO.Path.Combine(日志文件目录, $"操作日志_{日期前缀}.log");
                错误日志文件路径 = System.IO.Path.Combine(日志文件目录, $"错误日志_{日期前缀}.log");

                var 日志条目 = new LogEntry
                {
                    DateTime = DateTime.Now,
                    ID = Environment.UserName
                };

                // 记录系统启动日志
                SaveLogs(LogLevel.Info.ToString(), 日志条目);

                Console.WriteLine($"日志系统初始化完成，日志目录: {日志文件目录}");
            }
            catch (Exception ex)
            {
                // 如果日志初始化失败，输出到控制台
                Console.WriteLine($"日志系统初始化失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="级别">日志级别</param>
        /// <param name="模块">操作模块</param>
        /// <param name="消息">日志消息</param>
        /// <param name="详细信息">详细信息</param>
        public static void SaveOperationLogs(LogLevel logLevel, string module, string message, string details = "", string projectId = null)
        {
            try
            {
                var 日志条目 = new LogEntry
                {
                    DateTime = DateTime.Now,
                    Level = logLevel,
                    Module = module,
                    Message = message,
                    Details = details,
                    ID = Environment.UserName,
                    ProjectID = projectId ?? "未知项目",
                };

                // 写入操作日志文件
                SaveLogs(操作日志文件路径, 日志条目);

                // 如果是警告或错误级别，同时写入错误日志文件
                if (logLevel >= LogLevel.Warning)
                {
                    SaveLogs(错误日志文件路径, 日志条目);
                }

                // 在调试模式下输出到控制台
#if DEBUG
                Console.WriteLine($"[{logLevel}] {module}: {message} {details}");
#endif
            }
            catch (Exception ex)
            {
                // 避免日志记录本身的错误影响主程序
                Console.WriteLine($"记录日志时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="模块">出错模块</param>
        /// <param name="异常">异常对象</param>
        /// <param name="额外信息">额外信息</param>
        public static void SaveErrorLogs(string 模块, Exception 异常, string 额外信息 = "", string projectId = null)
        {
            try
            {
                var 日志条目 = new LogEntry
                {
                    DateTime = DateTime.Now,
                    Level = LogLevel.Error,
                    Module = 模块,
                    Message = 异常.Message,
                    Details = $"额外信息: {额外信息}\n堆栈跟踪: {异常.StackTrace}\n内部异常: {异常.InnerException?.Message ?? "无"}",
                    ID = Environment.UserName,
                    ProjectID = projectId ?? "未知项目"
                };

                // 写入错误日志文件
                SaveLogs(错误日志文件路径, 日志条目);

                // 同时写入操作日志文件
                SaveLogs(操作日志文件路径, 日志条目);

                // 在调试模式下输出到控制台
#if DEBUG
                Console.WriteLine($"[错误] {模块}: {异常.Message}\n{异常.StackTrace}");
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine($"记录错误日志时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 写入日志文件
        /// </summary>
        public static void SaveLogs(string 文件路径, LogEntry 日志条目)
        {
            try
            {
                // 格式化日志条目
                string 日志行 = $"[{日志条目.DateTime:yyyy-MM-dd HH:mm:ss.fff}] " +
                               $"[{日志条目.Level}] " +
                               $"[用户:{日志条目.ID}] " +
                               $"[项目:{日志条目.ProjectID}] " +
                               $"[{日志条目.Module}] " +
                               $"{日志条目.Message}" +
                               (string.IsNullOrEmpty(日志条目.Module) ? "" : $" | 详细: {日志条目.Module}");

                // 追加写入文件
                File.AppendAllText(文件路径, 日志行 + Environment.NewLine, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                // 如果写入日志文件失败，输出到控制台
                Console.WriteLine($"写入日志文件失败: {ex.Message}");
            }
        }

        #endregion



#if !DEBUG
                ///<summary>
                /// 设置控件窗口创建参数的扩展风格
                ///</summary>
                protected override CreateParams CreateParams
                {
                    get
                    {
                        CreateParams cp = base.CreateParams;
                        cp.ExStyle |= 0x02000000;
                        return cp;
                    }
                }
#endif
    }
}