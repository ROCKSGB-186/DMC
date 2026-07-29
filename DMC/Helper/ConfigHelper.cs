using System.Configuration;
using System.Linq;

namespace DMC.Helper
{
    /// <summary>
    /// Config帮助类
    /// </summary>
    public static class ConfigHelper
    {
        public static void SaveConfigInfo(string name, string value)//公共静态方法，接受两个参数 name 和 value，分别表示要保存的配置项的名称和值。
        {
            // 打开当前应用程序的配置文件，ConfigurationUserLevel.None 表示使用不带用户级别参数的配置。
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // 如果配置项已存在，会更新其值；如果不存在，则会添加新的配置项。不存在添加节点
            if (!conf.AppSettings.Settings.AllKeys.Contains(name))// 检查是否已存在具有指定名称的配置项。如果不存在该配置项，执行以下操作：
            {
                //向应用程序配置文件的 appSettings 节点下添加新的配置项（名称为 name，值为 value）。如果已经存在该配置项，则更新其值：
                conf.AppSettings.Settings.Add(name, value);
            }
            else
            {
                // 设置指定名称的配置项的值为新的 value。
                conf.AppSettings.Settings[name].Value = value;
            }
            // 保存对配置文件所做的更改。
            conf.Save();
            // 刷新 appSettings 部分，使应用程序能够立即读取最新的配置信息。
            ConfigurationManager.RefreshSection("appSettings");
            // 这段代码的功能是在应用程序运行时动态地保存配置信息到应用程序配置文件中，以便在下次启动应用程序时可以读取这些配置信息。这种方法通常用于保存应用程序的设置、用户偏好或其他需要持久化的数据。
        }
    }
}
