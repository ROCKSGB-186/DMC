using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace DMC.Helper
{
    /// <summary>
    /// 一个Splasher类，用于管理Splash窗体的显示和关闭。这个类将使用多线程来显示Splash窗体，以避免阻塞主线程。
    /// </summary>
    public class Splasher
    {
        private delegate void SplashStatusChangedHandle(string NewStatusInfo);
        /// <summary>
        /// 等待界面窗体
        /// </summary>
        private static Form m_SplashForm = null;
        /// <summary>
        /// 等待页面窗体
        /// </summary>
        private static ISplashForm m_SplashInterface = null;
        /// <summary>
        /// 创建新的线程
        /// </summary>
        private static Thread m_SplashThread = null;
        /// <summary>
        /// 临时状态
        /// </summary>
        private static string m_TempStatus = string.Empty;
        /// <summary>
        /// 显示等待窗口
        /// </summary>
        /// <param name="splashFormType"></param>
        public static void Show(Type splashFormType)
        {
            if (m_SplashThread != null)
                return;
            if (splashFormType == null)
                return;

            m_SplashThread = new Thread(new ThreadStart(delegate ()
            {
                CreateInstance(splashFormType);
                Application.Run(m_SplashForm);
            }));

            m_SplashThread.IsBackground = true;
            m_SplashThread.SetApartmentState(ApartmentState.STA);
            m_SplashThread.Start();
        }
        /// <summary>
        /// 创建窗体实例
        /// </summary>
        /// <param name="FormType">窗体类型</param>
        private static void CreateInstance(Type FormType)
        {
            object obj = FormType.InvokeMember(null,
            BindingFlags.DeclaredOnly |
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
            m_SplashForm = obj as Form;
            m_SplashInterface = obj as ISplashForm;
            if (m_SplashForm == null)
            {
                throw (new Exception("动画窗体必须为Form窗体"));
            }
            if (m_SplashInterface == null)
            {
                throw (new Exception("动画窗体必须继承ISplashForm"));
            }

            if (!string.IsNullOrEmpty(m_TempStatus))
                m_SplashInterface.SetStatusInfo(m_TempStatus);
        }

        /// <summary>
        /// 状态
        /// </summary>
        public static string Status
        {
            set
            {
                if (m_SplashInterface == null || m_SplashForm == null)
                {
                    m_TempStatus = value;
                    return;
                }
                m_SplashForm.Invoke(
                new SplashStatusChangedHandle(delegate (string str) { m_SplashInterface.SetStatusInfo(str); }),
                new object[] { value }
                );
            }
        }
        /// <summary>
        /// 关闭等待窗口
        /// </summary>
        public static void Close()
        {
            if (m_SplashThread == null || m_SplashForm == null) return;

            try
            {
                if (m_SplashForm.IsHandleCreated)
                {
                    m_SplashForm.Invoke(new MethodInvoker(m_SplashForm.Close));
                }
            }
            catch (Exception)
            {
            }
            m_SplashThread = null;
            m_SplashForm = null;
            m_TempStatus = string.Empty;
        }

    }
}
