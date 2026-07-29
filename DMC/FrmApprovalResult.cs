using DMC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 流程审批结果提交
    /// </summary>
    public partial class FrmApprovalResult : BaseForm
    {
        #region 简化方法 窗体移动,直接变化Left、Top
        private System.Drawing.Point originLocation;

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
        private string nodeInfoId = null;
        private string applyInfoId = null;
        private string userId = null;
        private List<UserModel> userList = null; 
        private int result = -1;

        /// <summary>
        /// 流程审批结果提交
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="objId"></param>
        /// <param name="objUserId"></param>
        /// <param name="onjResult"></param>
        public FrmApprovalResult(string applyId, string objId,string objUserId, int onjResult)
        {
            InitializeComponent();

            applyInfoId = applyId;// 流程id
            nodeInfoId = objId; //节点id
            userId = objUserId; //用户id
            result = onjResult; //审批结果 1通过 -1不通过(出版的时候 -1下载 -2拒绝 1完成)
            // 获取流程相关用户
            var param1 = new
            {
                applyId = applyId
            };
            userList = new List<UserModel>(); //流程相关用户列表
            HttpPost(AppGlobalModel.GetApplyUser, param1, ref userList);// 获取流程相关用户接口
            foreach (var item in userList)// 将流程相关用户添加到选择列表中
            {
                // 将用户昵称添加到选择列表中
                checkedListBoxSelectUser.Items.Add(item.nickName);
            }
            for (int i = 0; i < checkedListBoxSelectUser.Items.Count; i++) //默认全选
            {
                checkedListBoxSelectUser.SetItemChecked(i, true);// 将所有用户默认设置为选中状态
            }
            //foreach (var item in AppGlobalModel.DeptList.Where(o => o.parentId == "0"))
            //{
            //    companyId = item.deptId;
            //    companyName = item.deptName;
            //}
            // 根据公司名称判断是否禁用选择用户的功能
            if (GlobalVariables.companyName == "吉林医药设计院有限公司")
            {
               
                radioButtonSelectUser.Enabled = false;   // 禁用选择用户的功能
                radioButtonInitiatorUser.Visible = false;// 隐藏选择发起人选项
                checkBoxSelectUser.Enabled = false;      // 禁用全选用户的功能
                checkedListBoxSelectUser.Enabled = false;// 禁用选择用户列表
                comCheckBoxList1.Enabled = false;        // 禁用其他相关功能
            }
            
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button取消_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 拒绝
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button确定_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text.Trim()))
            {
                ShowErrorMsg("请填写您的审批意见！");
            }
            else
            {
                var sendType = 0;
                var userIds = "";

                if (radioButtonSelectUser.Checked)
                {
                    if (checkBoxSelectUser.Checked)
                    {//全部用户
                        sendType = 1;
                    }
                    else
                    {
                        // 自选用户
                        sendType = 3;
                        foreach (var item in checkedListBoxSelectUser.Items) //遍历选择用户列表中的每个用户项
                        {
                            if (checkedListBoxSelectUser.GetItemChecked(index: checkedListBoxSelectUser.Items.IndexOf(item))) // 检查当前用户项是否被选中
                            {
                                if (userIds.Length > 0) // 如果已经有用户ID被添加到userIds字符串中，则在添加新的用户ID之前先添加一个逗号分隔符
                                {
                                    userIds = userIds + "," + userList[checkedListBoxSelectUser.Items.IndexOf(item)].id; // 将选中用户的ID添加到userIds字符串中，多个用户ID之间用逗号分隔
                                }
                                else
                                {
                                    userIds = userList[checkedListBoxSelectUser.Items.IndexOf(item)].id; // 如果这是第一个被添加的用户ID，则直接将其赋值给userIds字符串，不需要添加逗号分隔符
                                }
                            }
                        }
                    }
                }
                else if (radioButtonInitiatorUser.Checked) //如果选择了发起人选项
                {//发起人
                    sendType = 2; // 设置sendType为2，表示审批结果将发送给发起人
                }
                else
                {// 发起人
                    sendType = 2; // 设置sendType为2，表示审批结果将发送给发起人
                }
                // 构造一个匿名对象param，包含审批结果相关的信息，包括节点ID、审批结果、审批意见文本、发送类型和用户ID列表
                var param = new
                {
                    applyNodeId = nodeInfoId,//节点id
                    result = result, //1通过 -1不通过(出版的时候 -1下载 -2拒绝 1完成)
                    //审批意见文本
                    title = textBox1.Text.Trim(),
                    sendType = sendType,
                    userIds = userIds
                };
                // 定义一个字符串变量resultData，用于存储接口调用的结果数据
                var resultData = string.Empty;
                if (HttpPost(AppGlobalModel.ApprovalResult, param, ref resultData)) // 调用审批结果提交接口，并将param对象作为请求参数传递，如果接口调用成功，则继续执行以下代码块
                {
                    var param1 = new // 构造一个匿名对象param1，包含流程ID和用户ID列表，用于添加审批失败的用户
                    {
                        applyId = applyInfoId, //流程id
                        userIds = new string[] { userId } // 将当前用户ID添加到userIds数组中，表示将审批失败的用户添加到流程中
                    };
                    HttpPost(AppGlobalModel.AddApplyFailUser, param1, ref resultData);// 调用添加审批失败用户接口，并将param1对象作为请求参数传递，将当前用户添加到审批失败的用户列表中

                    //var frm = new FrmSelectFailUserList();
                    //if (frm.ShowDialog() == DialogResult.OK)
                    //{
                    //    if (frm.SelectUserList != null && frm.SelectUserList.Any())
                    //    {
                    //        var param1 = new
                    //        {
                    //            applyId = applyInfoId,
                    //            userIds = frm.SelectUserList.Select(o => o.userId).ToList()
                    //        };

                    //        HttpPost(AppGlobalModel.AddApplyFailUser, param1, ref resultData);
                    //    }
                    //}

                    DialogResult = DialogResult.OK;
                }
            }
        }
        // 选择用户
        private void SelcetUser(object sender, EventArgs e)
        {
            checkedListBoxSelectUser.Enabled = true;
            checkBoxSelectUser.Enabled = true;
            comCheckBoxList1.Enabled = true;
        }
        /// <summary>
        /// 选择发起人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitiatorUser(object sender, EventArgs e)
        {
            checkedListBoxSelectUser.Enabled = false;
            checkBoxSelectUser.Enabled = false;
            comCheckBoxList1.Enabled = false;
        }
        /// <summary>
        /// 全选用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxSelectUser_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxSelectUser.Items.Count; i++)
            {
                checkedListBoxSelectUser.SetItemChecked(i, checkBoxSelectUser.Checked);
            }
        }
    }
}