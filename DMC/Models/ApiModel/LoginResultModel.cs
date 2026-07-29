using System.Collections.Generic;

namespace DMC.Models
{
    /// <summary>
    /// 登录返回类/1:deptName 用户部门/2：userMajors用户专业列表/3：user 用户信息/4：接口token/5：qzSealList 签章图片，为空就是没有
    /// </summary>
    public class LoginResultModel
    {
        /// <summary>
        /// 用户部门
        /// </summary>
        public string deptName { get; set; }
        /// <summary>
        /// 用户专业列表
        /// </summary>
        public List<UserMajorsModel> userMajors { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserModel user { get; set; }
        /// <summary>
        /// 接口token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 签章图片，为空就是没有
        /// </summary>
        public List<QzSealModel> qzSealList { get;set; }
    }
    /// <summary>
    /// 签名证书的1：id/2：sealname 证收名称/3：url路径/4：w 宽/5：h 高
    /// </summary>
    public class QzSealModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 证书名称
        /// </summary>
        public string sealname { get; set; }
        /// <summary>
        /// 证书路径
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 签名图片宽
        /// </summary>
        public int w { get; set; }
        /// <summary>
        /// 签名图片高
        /// </summary>
        public int h { get; set; }
    }
}
