using DMC.Helper;
using DMC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DMC
{

    /// <summary>
    /// 登录
    /// </summary>
    public partial class FrmLogin : BaseForm
    {
        #region 简化方法 窗体移动,直接变化Left、Top
        private Point originLocation;

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

        private void WindowMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originLocation = e.Location;
            }
        }
        #endregion
        //初始化一个登录过的用户列表
        private List<UserConfigModel> UserList = null;
        /// <summary>
        /// 权限列表
        /// </summary>
        private List<string> authList = new List<string>();
        /// <summary>
        /// 窗体初始化
        /// </summary>
        public FrmLogin()
        {
            var systemSettings = ConfigurationManager.AppSettings["SystemSettings"];
            
            //判断是不是加载数据为空；
            if (systemSettings != "")
            {
                //读取保存的系统设置,序列化字段后赋值给systemSettingsModel
                var systemSettingsModel = JsonConvert.DeserializeObject<SystemSettingsModel>(systemSettings);
                //赋值给系统IP
                AppGlobalModel.ServiceAddress = systemSettingsModel.ServiceAddress;
                //系统端口
                AppGlobalModel.ServiceProt = systemSettingsModel.ServiceProt;
                //消息IP
                AppGlobalModel.MqttServiceAddress = systemSettingsModel.MqttServiceAddress;
                //消息端口
                AppGlobalModel.MqttServiceProt = systemSettingsModel.MqttServiceProt;
            }
            InitializeComponent();
            //加载调用之前保存过的用户登录信息；
            var userValue = ConfigurationManager.AppSettings["userList"];
            //判断是不是加载数据为空；
            if (userValue != "")
            {
                //把读取的登录用户列表保存到UserList里；
                UserList = JsonConvert.DeserializeObject<List<UserConfigModel>>(userValue);
                txtUsername.ValueMember = "Name";//value隐藏值
                txtUsername.DisplayMember = "Name";//Display显示
                //把登录窗口的数据原与UserList数据绑定；
                txtUsername.DataSource = UserList;
                txtUsername.AutoCompleteSource = AutoCompleteSource.ListItems;
                GlobalVariables.userName = UserList[0].Name;
            }
            //初始化本平台的安装路径；
            AppGlobalModel.InitialDirectory = ConfigurationManager.AppSettings["InitialDirectory"];
            //初始化平台下载路径；
            AppGlobalModel.InitialDownloadDirectory = ConfigurationManager.AppSettings["InitialDownloadDirectory"];
            GlobalVariables.companyName = "";
            GlobalVariables.companyId = "";
#if DEBUG
            //txtUsername.Text = "songguang";
            //txtPassword.Text = "123456";
#endif
        }
        
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowErrorMsg("请输入用户名！");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowErrorMsg("请输入密码！");
                return;
            }
            //拿到用户名
            var username = txtUsername.Text.Trim();
            //拿到密码
            var password = txtPassword.Text.Trim();
            if (username != GlobalVariables.userName) 
            {
                GlobalVariables.userName = username;
                //SystemTempData.CreateEmptyJsonFile();
            }

            //登录类，存的用户名与密码
            var loginModel = new LoginModel()
            {
                username = username,
                password = password
            };
            


            var resultData = string.Empty;
            //与服务器通讯，获取登录用户信息；
            var postData = HttpHelper.GetPostData(loginModel);
            //postData提交数据；
            if (HttpHelper.PostData(AppGlobalModel.Login, postData, ref resultData))//与平台的Login接口通信，如果平台内有这个用户并且密码正确，返回数据到resultData里；
            {
                //结果
                var loginResultModel = JsonConvert.DeserializeObject<ResultModel<LoginResultModel>>(resultData);
                //如果loginResultModel.code=200,表示与平台通信正确；
                if (loginResultModel.code == 200)
                {
                    AppGlobalModel.UseInfo = loginResultModel.data.user;
                    AppGlobalModel.UseInfo.deptName = loginResultModel.data.deptName;
                    AppGlobalModel.Token = loginResultModel.data.token;
                    AppGlobalModel.qzSealList = loginResultModel.data.qzSealList;
                    GlobalVariables.userName = AppGlobalModel.UseInfo.realName;
                    GlobalVariables.userId = AppGlobalModel.UseInfo.id;
                    GlobalVariables.userDeptName = loginResultModel.data.deptName;

                    #region 保存登录用户

                    var userModel = new UserConfigModel()
                    {
                        Name = username,
                        PassWord = password
                    };

                    if (UserList == null)
                    {
                        UserList = new List<UserConfigModel>();
                    }
                    /*    解读lambda表达式
                    //这段代码是在 C# 中使用 LINQ（Language Integrated Query）查询语法查找列表 UserList 中第一个满足条件的元素。具体来说，FirstOrDefault 方法接受一个条件判断函数作为参数，该函数用于确定列表中元素是否满足特定的条件。

                    //在这里，代码中的条件判断函数 o => o.Name == username 使用了 Lambda 表达式，表示对于列表中的每个对象 o，如果对象的 Name 属性等于给定的 username 变量的值，则返回 true，否则返回 false。

                    //FirstOrDefault 方法将遍历 UserList 列表，并返回第一个满足条件的元素，如果列表中没有元素满足条件，则返回默认值（null 或默认类型值，具体取决于元素类型）。

                    //所以，变量 isModel 会存储列表中第一个满足条件的元素，或者如果没有满足条件的元素则为 null。
                    */
                    //判断是否存在
                    var isModel = UserList.FirstOrDefault(o => o.Name == username);
                    if (isModel != null)
                    {
                        UserList.Remove(isModel);
                    }
                    //用户列表0位置插入用户名与密码userModel；
                    UserList.Insert(0, userModel);
                    /*     这种方法常用于保存配置信息、设置或其他需要持久化的信息，使得这些数据可以在应用程序重新启动时被读取和使用。
                       在 C# 中，这句话的作用可以分解为两个部分来解释：

                        JsonConvert.SerializeObject(UserList):

                        这一部分使用了 Newtonsoft.Json 库中的 JsonConvert 类，将 UserList 对象序列化为 JSON 格式的字符串。SerializeObject 方法会遍历 UserList 对象中的数据，并将其转换为 JSON 字符串，这是一个与 JavaScript 对象表示法（JSON）相同的格式，用于数据的存储和传输。
                        ConfigHelper.SaveConfigInfo("userList", ...):

                        这一部分是调用了一个名为 ConfigHelper 的类中的 SaveConfigInfo 方法，向其传递两个参数。第一个参数 "userList" 是一个字符串，可能作为配置项的标识符；第二个参数是序列化后的 JSON 字符串，表示 UserList 对象的内容。
                        完整地解释这句话：

                        ConfigHelper.SaveConfigInfo("userList", JsonConvert.SerializeObject(UserList));
                        意思是：首先将 UserList 对象序列化为 JSON 格式的字符串，然后调用 ConfigHelper 类的 SaveConfigInfo 方法，将这个 JSON 字符串与标识符 "userList" 一起保存到某个配置存储中（可能是文件、数据库或应用程序的配置系统）。

                        示例代码：

                        // 1. 使用 Newtonsoft.Json 库将 UserList 对象序列化为 JSON 字符串
                        string jsonString = JsonConvert.SerializeObject(UserList);

                        // 2. 使用 ConfigHelper 类将 JSON 字符串保存到配置项中
                        ConfigHelper.SaveConfigInfo("userList", jsonString);
                        这种方法常用于保存配置信息、设置或其他需要持久化的信息，使得这些数据可以在应用程序重新启动时被读取和使用。
                     */

                    //保存入文件
                    ConfigHelper.SaveConfigInfo("userList", JsonConvert.SerializeObject(UserList));
                    #endregion

                    Splasher.Status = "加载组织架构......";
                    AppGlobalModel.DeptList = new List<DeptInfoResultModel>();
                    LoadDept("0");

                    Splasher.Status = "加载权限......";
                    #region 加载全局权限
                    var resultAuthData = new List<string>();
                    if (HttpPost(AppGlobalModel.GetOverallSituationMenu, "", ref resultAuthData))
                    {
                        AppGlobalModel.OverallSituationMenu = resultAuthData;
                    }
                    #endregion

                    DialogResult = DialogResult.OK;

                    Splasher.Show(typeof(FrmLoading));
                    Splasher.Status = "正在登录，请稍后...";
                    foreach (var item in AppGlobalModel.DeptList.Where(o => o.parentId == "0"))
                    {
                        //赋值公司id全局变量
                        GlobalVariables.companyId = item.deptId;
                        //赋值公司名称全局变量
                        GlobalVariables.companyName = item.deptName;
                    }
                }
                else
                {
                    ShowErrorMsg(loginResultModel.msg);
                }
            }
            else
            {
                ShowErrorMsg(resultData);
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUsername_SelectedIndexChanged(object sender, EventArgs e)
        {
            var userModel = UserList?.FirstOrDefault(o => o.Name == txtUsername.SelectedValue.ToString());
            if (userModel != null)
            {
                txtPassword.Text = userModel.PassWord;
            }
        }
        /// <summary>
        /// 密码回车登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUsername_TextUpdate(object sender, EventArgs e)
        {
            var userModel = UserList?.FirstOrDefault(o => o.Name == txtUsername.Text);
            if (userModel != null)
            {
                txtPassword.Text = userModel.PassWord;
            }
            else
            {
                txtPassword.Text = "";
            }
        }
        /// <summary>
        /// 加载部门
        /// </summary>
        /// <param name="parentId"></param>
        private void LoadDept(string parentId)
        {
            var resultData = new List<DeptInfoResultModel>();
            if (HttpGet(AppGlobalModel.GetDeptList + "?parentId=" + parentId, ref resultData))
            {
                foreach (var item in resultData)
                {
                    item.parentId = parentId;
                    AppGlobalModel.DeptList.Add(item);

                    if (item.deptType == null)
                    {
                        continue;
                    }

                    LoadDept(item.deptId);
                }
            }
        }
        /// <summary>
        /// 系统设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_Click(object sender, EventArgs e)
        {
            var frmSet = new FrmSystemSettings();
            frmSet.ShowDialog();
        }
       

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }       

        //private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    char ch = e.KeyChar; 
        //    if (ch == '\r')
        //    {
                
        //        //btnLogin_Click(sender, e);
        //        this.Close();
        //    }
    
        //}

        /// <summary>
        /// 回车登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnBtn(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar=='\r') { btnLogin_Click(sender, e); } ;  
        }
    }
}
