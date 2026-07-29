using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DMC.Helper
{
    /// <summary>
    /// 写操作日志记录的类
    /// </summary>
    public static class LogHelper
    {      
        private static readonly object syncRoot = null;
        static LogHelper()
        {
            syncRoot = new object();
        }

        private static void WriteLocalFile(string msg, string fileNamePrefix = null)
        {
            lock (syncRoot)
            {
                try
                {
                    //判断是否加前缀名
                    if (!string.IsNullOrEmpty(fileNamePrefix) && !fileNamePrefix.Substring(fileNamePrefix.Length - 1, 1).Equals("_"))
                    {
                        fileNamePrefix = string.Format("{0}_", fileNamePrefix);
                    }
                    string filePath = string.Format("{0}logs\\", AppDomain.CurrentDomain.BaseDirectory);
                    //如果日志目录不存在就创建
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    int index = 0;
                    DateTime time = DateTime.Now;
                    string fileExtensionName = ".log";
                    string fileNameFormat = Path.Combine(filePath, fileNamePrefix + time.ToString("yyyy-MM-dd") + "_{0}" + fileExtensionName);
                    string fileName = string.Format(fileNameFormat, index);

                    //创建指定目录对象
                    DirectoryInfo folder = new DirectoryInfo(filePath);
                    var folderList = folder.GetFiles("*.log");
                    if (folderList.Any())
                    {
                        while (folderList.Any(o => fileName.EndsWith("\\" + o.Name)))
                        {
                            var selectFile = folderList.FirstOrDefault(o => fileName.EndsWith("\\" + o.Name));
                            if (selectFile != null && selectFile.Length > (1024 * 1024) * 3)
                            {
                                index++;
                                fileName = string.Format(fileNameFormat, index);
                            }
                            else
                            {
                                break;
                            }
                        }
                        var removeTime = DateTime.Today.AddDays(-7);
                        foreach (System.IO.FileInfo itemFile in folderList.Where(o => o.CreationTime < removeTime))
                        {
                            File.Delete(itemFile.FullName);
                        }
                    }

                    //创建文件对象
                    System.IO.FileInfo file = new System.IO.FileInfo(fileName);
                    using (StreamWriter sw = file.AppendText())
                    {
                        string timeStr = time.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        StringBuilder writeContent = new StringBuilder();
                        //向日志文件写入内容
                        writeContent.AppendLine(string.Format("=== {0} Begin ===", timeStr));
                        writeContent.AppendLine(msg);
                        writeContent.AppendLine(string.Format("=== {0} End   ===", timeStr));
                        sw.WriteLine(writeContent.ToString());
                        sw.Close();
                        sw.Dispose();
                    }
                }
                catch
                {

                }
            }
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        /// <param name="fileNamePrefix"></param>
        public static void WriteLocalLog(object sender, string msg, string fileNamePrefix = null)
        {
            string senderStr = "sender is null";
            if (sender != null)
            {
                var oType = sender.GetType();
                senderStr = oType == typeof(string) ? sender.ToString() : oType.ToString();
            }
            //日志文本
            string logTxt = string.Format("{0}:{1}{2}", senderStr, Environment.NewLine, msg);
            var action = new Action<string, string>(WriteLocalFile);
            action.BeginInvoke(logTxt, fileNamePrefix, null, null);
        }
        //public static void WriteLocalLog( string msg, string fileNamePrefix = null)
        //{           
        //    //日志文本
        //    string logTxt = string.Format("{0}:{1}{2}", Environment.NewLine, msg);
        //    var action = new Action<string, string>(WriteLocalFile);
        //    action.BeginInvoke(logTxt, fileNamePrefix, null, null);
        //}
        // 修复后的重载（示例一：使用正确的占位符）
        public static void WriteLocalLog(string msg, string fileNamePrefix = null)
        {
            // 日志文本：换行 + 消息
            string logTxt = string.Format("{0}{1}", Environment.NewLine, msg);
            var action = new Action<string, string>(WriteLocalFile);
            action.BeginInvoke(logTxt, fileNamePrefix, null, null);
        }

        // 或者更直观的写法（示例二：插值）
        public static void WriteLocalLog_Interpolated(string msg, string fileNamePrefix = null)
        {
            string logTxt = $"{Environment.NewLine}{msg}";
            var action = new Action<string, string>(WriteLocalFile);
            action.BeginInvoke(logTxt, fileNamePrefix, null, null);
        }

        public static void WriteLocalErrorLog(object sender, string msg, string fileNamePrefix = null)
        {
            fileNamePrefix = string.Format("Error_{0}", fileNamePrefix);
            WriteLocalLog(sender, msg, fileNamePrefix);
        }

        public static void WriteLocalErrorLog(object sender, Exception ex, string fileNamePrefix = null)
        {
            WriteLocalErrorLog(sender, ex.ToString(), fileNamePrefix);
        }

        public static void WriteLocalDebugLog(object sender, string msg, string fileNamePrefix = null)
        {
#if DEBUG
            fileNamePrefix = string.Format("Debug_{0}", fileNamePrefix);
            WriteLocalLog(sender, msg, fileNamePrefix);
#endif
        }
    }
}
