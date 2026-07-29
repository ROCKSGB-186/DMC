using DMC.Helper;
using DMC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DMC
{
    internal static class Program
    {
        private static Mutex run = null;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //捕获未处理的异常:全局异常捕获
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //捕获未处理的任务异常:捕获未观察的异常
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //捕获未处理的异常:捕获UI线程异常
            Application.ThreadException += Application_ThreadException;
            //捕获未处理的异常:捕获非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            //启动程序
            Application.EnableVisualStyles();
            //禁止双缓冲
            Application.SetCompatibleTextRenderingDefault(false);
            //创建Mutex对象单实例运行
            bool runone;
            //创建Mutex对象
            run = new Mutex(true, @"Global\DMC", out runone);
            if (runone)
            {
                run.ReleaseMutex();

                //获取当前登录的Windows用户标示
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                //创建WindowsPrincipal对象
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                //判断当前用户是否为管理员判断当前登录用户是否为管理员
                if (principal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    var systemSettings = ConfigurationManager.AppSettings["SystemSettings"];
                    AppGlobalModel.Logging = Convert.ToBoolean(Convert.ToInt32(ConfigurationManager.AppSettings["Logging"]));

#if DEBUG
                //systemSettings = "{\"ServiceAddress\":\"218.24.35.83\",\"ServiceProt\":8888,\"MqttServiceAddress\":\"218.24.35.83\",\"MqttServiceProt\":1884}";
                //systemSettings = "{\"ServiceAddress\":\"127.0.0.1\",\"ServiceProt\":8888,\"MqttServiceAddress\":\"127.0.0.1\",\"MqttServiceProt\":1884}";
                AppGlobalModel.Logging = true;
#endif
                    
                    if (string.IsNullOrWhiteSpace(systemSettings))
                    {
                        var frmSet = new FrmSystemSettings();
                        if (frmSet.ShowDialog() != DialogResult.OK)
                        {
                            Application.Exit();
                            return;
                        }
                    }
                    else
                    {
                        var settingInfo = JsonConvert.DeserializeObject<SystemSettingsModel>(systemSettings);
                        AppGlobalModel.ServiceAddress = settingInfo.ServiceAddress;
                        AppGlobalModel.ServiceProt = settingInfo.ServiceProt;
                        AppGlobalModel.MqttServiceAddress = settingInfo.MqttServiceAddress;
                        AppGlobalModel.MqttServiceProt = settingInfo.MqttServiceProt;
                        AppGlobalModel.StartupAutomatically = settingInfo.StartupAutomatically;
                    }

                    Splasher.Show(typeof(FrmLoading));
                    Splasher.Status = "正在检测软件版本...";
                    #region 检查版本信息
                    /******************版本信息******************/
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    //产品版本
                    string versionStr = assembly.GetName().Version.ToString();

                    //删除当前版本的升级文件
                    var dir = Environment.CurrentDirectory + $"\\{versionStr}";
                    if (Directory.Exists(dir))
                    {
                        Directory.Delete(dir, true);
                    }

                    //检查版本信息
                    var resultData = string.Empty;
                    if (HttpHelper.PostData(AppGlobalModel.GetVersion, $"version={versionStr}", ref resultData))
                    {
                        var resultModel = JsonConvert.DeserializeObject<ResultModel<GetVersionModel>>(resultData);

                        if (resultModel.code == 200)
                        {
                            if (resultModel.data != null && !string.IsNullOrWhiteSpace(resultModel.data.downloadUrl) && !string.IsNullOrWhiteSpace(resultModel.data.code))
                            {
                                var result = DialogResult.OK;
                                if (resultModel.data.updateType == 0)
                                {
                                    result = MessageBox.Show($"系统有新的版本，版本号：v{resultModel.data.code},是否确定下载更新", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                }

                                if (result == DialogResult.OK)
                                {
                                    dir = Environment.CurrentDirectory + $"\\{resultModel.data.code}";
                                    if (!Directory.Exists(dir))
                                    {
                                        Directory.CreateDirectory(dir);
                                    }

                                    var filrName = Path.GetFileName(resultModel.data.downloadUrl);
                                    dir = dir + "\\" + filrName;

                                    var frmDownload = new FrmDownloadFile(resultModel.data.downloadUrl, dir);
                                    frmDownload.isTips = false;
                                    var dialogResult = frmDownload.ShowDialog();
                                    if (dialogResult == DialogResult.OK)
                                    {      
                                        //创建进程
                                        var pi = new ProcessStartInfo(dir);
                                        pi.UseShellExecute = true;
                                        pi.FileName = dir;

                                        var proc = new Process();
                                        proc.StartInfo = pi;

                                        proc.Start();

                                        Application.Exit();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Splasher.Close();
                        MessageBox.Show(resultData, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    #endregion

                    //登录
                    var frm = new FrmLogin();
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new FrmMian());
                    }
                }
                else
                {
                    //创建启动对象
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.WorkingDirectory = Environment.CurrentDirectory;
                    startInfo.FileName = Application.ExecutablePath;

                    //设置启动动作，确保以管理员身份运行
                    startInfo.Verb = "runas";
                    try
                    {
                        Process.Start(startInfo);
                    }
                    catch
                    {
                        return;
                    }

                    Application.Exit();
                }               
            }
            else
            {
                MessageBox.Show("程序已经运行，不能重复运行！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //记录日志
            LogHelper.WriteLocalErrorLog(sender, e.Exception, "Application");
        }
        /// <summary>
        /// 未处理的异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {   //记录日志
            LogHelper.WriteLocalLog(sender, e.ExceptionObject.ToString(), "UnhandledException");
        }
        /// <summary>
        /// 任务调度器未处理的异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogHelper.WriteLocalErrorLog(sender, e.Exception, "TaskScheduler");
            e.SetObserved();
        }
    }
}
