using DMC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DMC.Helper;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
namespace DMC
{
    /// <summary>
    /// 系统临时数据静态类
    /// </summary>
    public static class SystemTempData
    {
        #region 静态变量
        /// <summary>
        /// 获取当前程序的安装路径  
        /// </summary>
        public static string installPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        /// <summary>
        /// 文件列表查询条件:1/fileType,文件来源0 项目区 1归档区;2/type，发起类型 0购物车 1文件夹 2项目 3文件 /3：fileIds,流id列表用，分割/4：parentId, 上级ID/5：tab 是否获得未归档的文件 0未归档 1全部 默认传1，就出版和下载都传 0  剩下的都传1
        /// </summary>
        private static QueryApprovalProjectStructure QueryInfos = new QueryApprovalProjectStructure()
        {
            fileType = 0, //文件来源0 项目区 1归档区
            type = 3,    //发起类型 0购物车 1文件夹 2项目 3文件
            fileIds = "",  //流id列表  用  ，分割
            parentId = "0",  //上级ID
            applyId = "", //审批详情主键Id
            tab = "1"  // 是否获得未归档的文件 0未归档 1全部 默认传1，就出版和下载都传 0  剩下的都传1
        };
        /// <summary>
        /// 存储ApplyListModel对象的列表
        /// </summary>
        private static List<ApplyInfoModel> applyInfoTemp = new List<ApplyInfoModel>();
        /// <summary>
        /// 存储ApplyListModel对象的列表                                                                               
        /// </summary>
        public static List<ApplyListModel> applyListTemp = new List<ApplyListModel>();
        /// <summary>
        /// 存储ApplyListModel对象的列表                                                                               
        /// </summary>
        public static List<ApplyListModel> allApplyListTemp = new List<ApplyListModel>();
        /// <summary>
        /// 存储ApplyListModel对象的列表                                                                               
        /// </summary>
        private static List<ApplyListModel> applyStatisticsListTemp = new List<ApplyListModel>();
        /// <summary>
        /// 存储项目属性对象的临时列表:id主键/name项目名称/userId创建人id/parentId父级id/status项目状态（0正常 1停用 2未发布 3删除 4迭代）/identifier项目编码/unit建筑单位/proType项目类型/realName总负责人/govern项目经理/governName项目经理姓名/customList自定义属性
        /// </summary>
        private static List<GetProjectAttributeModel> projectInfoListTemp = new List<GetProjectAttributeModel>();
        /// <summary>
        /// 存储项目属性对象的临时列表:id主键/name项目名称/userId创建人id/parentId父级id/status项目状态（0正常 1停用 2未发布 3删除 4迭代）/identifier项目编码/unit建筑单位/proType项目类型/realName总负责人/govern项目经理/governName项目经理姓名/customList自定义属性
        /// </summary>
        private static List<GetProjectAttributeModel> statisticsProjectInfoListTemp = new List<GetProjectAttributeModel>();
        /// <summary>
        /// 存储项目属性对象的临时变量:id主键/name项目名称/userId创建人id/parentId父级id/status项目状态（0正常 1停用 2未发布 3删除 4迭代）/identifier项目编码/unit建筑单位/proType项目类型/realName总负责人/govern项目经理/governName项目经理姓名/customList自定义属性
        /// </summary>
        private static GetProjectAttributeModel projectInfoTemp = new GetProjectAttributeModel();
        /// <summary>
        /// 存储项目列表来自本地缓存临时数据； 
        /// </summary>
        private static List<ProjectResultModel> projectListTemp = new List<ProjectResultModel>();
        /// <summary>
        /// 存储项目列表来自本地缓存临时数据； 
        /// </summary>
        private static List<ProjectResultModel> statisticsProjectListTemp = new List<ProjectResultModel>();
        /// <summary>
        /// 存储项目列表来自服务器的临时数据；  
        /// </summary>
        private static List<ProjectResultModel> projectListHttpTemp = new List<ProjectResultModel>();
        /// <summary>
        /// 临时保存每个项目拆解后可以绑定到数据表的最终版list；
        /// </summary>
        private static List<List<ProjectPropertyModel>> projectPropertyListTemp = new List<List<ProjectPropertyModel>>();
        /// <summary>
        /// 存储用户对象的列表
        /// </summary>
        private static List<projectDeptModel> projectUserInfoListTemp = null;
        /// <summary>
        ///  项目Id、Name；阶段id、name；专业id、name；角色id、name；人员id、name；
        /// </summary>
        public static projectDeptModel projectUserInfoModel = new projectDeptModel();
        /// <summary>
        /// 组织架构下的所有用户集合
        /// </summary>
        private static List<QzUserResultModel> deptUserListTemp = new List<QzUserResultModel>();
        /// <summary>
        /// 项目属性模型List的List:0、No序号；1、id； 2、Name名称 3、Value值
        /// </summary>
        private static List<List<ProjectPropertyModel>> ProjectPropertieListS = new List<List<ProjectPropertyModel>>();
        /// <summary>
        /// 项目属性模型List:0、No序号；1、id； 2、Name名称 3、Value值
        /// </summary>
        private static List<ProjectPropertyModel> ProjectPropertieItemS = new List<ProjectPropertyModel>();
        /// <summary>
        /// 项目属性模型:0、No序号；1、id； 2、Name名称 3、Value值
        /// </summary>
        private static ProjectPropertyModel ProjectPropertieItem = new ProjectPropertyModel();
        #endregion

        #region 创建空文件方法
        /// <summary>
        /// 存储JSON文件路径的静态字段  
        /// </summary>
        private static string applyListJsonFile;
        /// <summary>
        /// 存储JSON文件路径的静态字段
        /// </summary>
        private static string applyStatisticsListJsonFile;
      
        /// <summary>
        /// 存储JSON文件路径的静态字段
        /// </summary>
        private static string applyInfoJsonFile;
        /// <summary>
        /// 存储JSON文件路径的静态字段
        /// </summary>
        private static string projectListJsonFile;
       /// <summary>
       /// 统计项目列表JSON文件路径的静态字段
       /// </summary>
        private static string statisticsProjectListJsonFile;
        /// <summary>
        /// 项目信息列表JSON文件路径的静态字段
        /// </summary>
        private static string projectInfoListJsonFile;
        /// <summary>
        /// 项目信息属性列表JSON文件路径的静态字段
        /// </summary>
        private static string projectInfoPropertyListJsonFile;
        /// <summary>
        /// 用户信息列表JSON文件路径的静态字段
        /// </summary>
        private static string statisticsUserInfoListJsonFile;   
        /// <summary>
        /// 用户信息列表JSON文件路径的静态字段
        /// </summary>
        private static string userInfoJsonFile;
        /// <summary>
        /// 组织架构下的所有用户列表JSON文件路径的静态字段
        /// </summary>
        private static string deptUserListJsonFile;
        /// <summary>
        /// 静态构造函数，初始化静态类  
        /// </summary>
        static SystemTempData()
        {
            Thread thread = new Thread(() =>
            {
                // 定义JSON文件的完整路径  
                applyListJsonFile = Path.Combine(installPath, "applyListJsonFile.json");
                applyStatisticsListJsonFile = Path.Combine(installPath, "applyStatisticsListJsonFile.json");
                applyInfoJsonFile = Path.Combine(installPath, "applyInfoJsonFile.json");
                projectListJsonFile = Path.Combine(installPath, "projectListJsonFile.json");
                statisticsProjectListJsonFile = Path.Combine(installPath, "statisticsProjectListJsonFile.json");
                projectInfoListJsonFile = Path.Combine(installPath, "projectInfoListJsonFile.json");
                projectInfoPropertyListJsonFile = Path.Combine(installPath, "projectInfoPropertyListJsonFile.json");
                statisticsUserInfoListJsonFile = Path.Combine(installPath, "statisticsUserInfoListJsonFile.json");
                userInfoJsonFile = Path.Combine(installPath, "userInfoJsonFile.json");
                deptUserListJsonFile = Path.Combine(installPath, "deptUserJsonFile.json");
                // 检查JSON文件是否存在  
                if (!File.Exists(applyListJsonFile))
                {
                    // 如果文件不存在，创建一个新的空文件  
                    CreateEmptyApplyListJsonFile();
                }
                if (!File.Exists(applyStatisticsListJsonFile))
                {
                    // 如果文件不存在，创建一个新的空文件  
                    CreateEmptyApplyStatisticsListJsonFile();
                }
                if (!File.Exists(applyInfoJsonFile))
                {
                    // 如果文件不存在，创建一个新的空文件  
                    CreateEmptyApplyInfoJsonFile();
                }
                if (!File.Exists(projectListJsonFile))
                {
                    // 如果文件不存在，创建一个新的空项目列队文件  
                    CreateEmptyProjectListJsonFile();
                }
                if (!File.Exists(statisticsProjectListJsonFile))
                {
                    // 如果文件不存在，创建一个新的空项目列队文件  
                    CreateEmptyStatisticsProjectListJsonFile();
                }
                if (!File.Exists(projectInfoListJsonFile))
                {
                    // 如果文件不存在，创建一个新的空项目信息文件  
                    CreateEmptyProjectInfoListJsonFile();
                }
                if (!File.Exists(projectInfoPropertyListJsonFile))
                {
                    // 如果文件不存在，创建一个新的空项目信息文件  
                    CreateEmptyProjectInfoPropertyListJsonFile();
                }
                if (!File.Exists(statisticsUserInfoListJsonFile))
                {
                    // 如果文件不存在，创建一个新的空人员列队文件  
                    CreateEmptyStatisticsUserInfoListJsonFile();
                }
                if (!File.Exists(userInfoJsonFile))
                {
                    // 如果文件不存在，创建一个新的空w人员信息文件  
                    CreateEmptyUserInfoJsonFile();
                }
                if (!File.Exists(deptUserListJsonFile))
                {
                    // 如果文件不存在，创建一个新的空w人员信息文件  
                    CreateEmptyDeptUserJsonFile();
                }
            });
            thread.Start();

        }
        public static void CreateEmptyJsonFile()
        {
            Thread thread = new Thread(() =>
            {
                // 定义JSON文件的完整路径  
                applyListJsonFile = Path.Combine(installPath, "applyListJsonFile.json");
                applyStatisticsListJsonFile = Path.Combine(installPath, "applyStatisticsListJsonFile.json");
                applyInfoJsonFile = Path.Combine(installPath, "applyInfoJsonFile.json");
                projectListJsonFile = Path.Combine(installPath, "projectListJsonFile.json");
                statisticsProjectListJsonFile = Path.Combine(installPath, "statisticsProjectListJsonFile.json");
                projectInfoListJsonFile = Path.Combine(installPath, "projectInfoListJsonFile.json");
                projectInfoPropertyListJsonFile = Path.Combine(installPath, "projectInfoPropertyListJsonFile.json");
                statisticsUserInfoListJsonFile = Path.Combine(installPath, "statisticsUserInfoListJsonFile.json");
                userInfoJsonFile = Path.Combine(installPath, "userInfoJsonFile.json");
                deptUserListJsonFile = Path.Combine(installPath, "deptUserJsonFile.json");

                // 如果文件不存在，创建一个新的空文件  
                CreateEmptyApplyListJsonFile();

                // 如果文件不存在，创建一个新的空文件  
                CreateEmptyApplyStatisticsListJsonFile();

                // 如果文件不存在，创建一个新的空文件  
                CreateEmptyApplyInfoJsonFile();

                // 如果文件不存在，创建一个新的空项目列队文件  
                CreateEmptyProjectListJsonFile();

                // 如果文件不存在，创建一个新的空项目列队文件  
                CreateEmptyStatisticsProjectListJsonFile();

                // 如果文件不存在，创建一个新的空项目信息文件  
                CreateEmptyProjectInfoListJsonFile();

                // 如果文件不存在，创建一个新的空项目信息文件  
                CreateEmptyProjectInfoPropertyListJsonFile();

                // 如果文件不存在，创建一个新的空人员列队文件  
                CreateEmptyStatisticsUserInfoListJsonFile();

                // 如果文件不存在，创建一个新的空w人员信息文件  
                CreateEmptyUserInfoJsonFile();

                // 如果文件不存在，创建一个新的空w人员信息文件  
                CreateEmptyDeptUserJsonFile();
            });
            thread.Start();
        }

        public static void CreateEmptyDeptUserJsonFile()
        {
            // 将空的列表写入JSON文件  
            File.WriteAllText(deptUserListJsonFile, JsonConvert.SerializeObject(new List<QzUserResultModel>(), Formatting.Indented));
            //清理临时变量
            //deptUserListTemp.Clear();
        }
        /// <summary>
        /// 创建一个新的空审批列表JSON文件的方法  
        /// </summary>
        public static void CreateEmptyApplyListJsonFile()
        {
            // 将空的列表写入JSON文件  
            File.WriteAllText(applyListJsonFile, JsonConvert.SerializeObject(new List<ApplyListModel>(), Formatting.Indented));
            //清理临时变量
            //applyListTemp.Clear();
        }
        /// <summary>
        /// 创建一个新的空审批列表JSON文件的方法  
        /// </summary>
        public static void CreateEmptyApplyStatisticsListJsonFile()
        {
            // 将空的列表写入JSON文件  
            File.WriteAllText(applyStatisticsListJsonFile, JsonConvert.SerializeObject(new List<ApplyListModel>(), Formatting.Indented));
            //清理临时变量
            //applyListTemp.Clear();
        }
        /// <summary>
        /// 创建一个新的空审批详情JSON文件的方法  
        /// </summary>
        public static void CreateEmptyApplyInfoJsonFile()
        {
            // 将空的列表写入JSON文件  
            File.WriteAllText(applyInfoJsonFile, JsonConvert.SerializeObject(new List<ApplyInfoModel>(), Formatting.Indented));
            //清理临时变量
            //applyInfoTemp.Clear();
        }
        /// <summary>
        /// 创建一个新的空项目列表JSON文件的方法  
        /// </summary>
        public static void CreateEmptyProjectListJsonFile()
        {
            try
            {
                // 将空的列表写入JSON文件  
                File.WriteAllText(projectListJsonFile,
                    JsonConvert.SerializeObject(new List<ProjectResultModel>(), Formatting.Indented));
                //清理临时变量
                //applyListTemp.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }


        }
        /// <summary>
        /// 创建一个新的空项目列表JSON文件的方法  
        /// </summary>
        public static void CreateEmptyStatisticsProjectListJsonFile()
        {
            // 将空的列表写入JSON文件  
            File.WriteAllText(statisticsProjectListJsonFile, JsonConvert.SerializeObject(new List<List<ProjectPropertyModel>>(), Formatting.Indented));
            //清理临时变量
            //applyListTemp.Clear();
        }
        /// <summary>
        /// 创建一个新的空项目详情JSON文件的方法  
        /// </summary>
        public static void CreateEmptyProjectInfoListJsonFile()
        {
            // 将空的列表写入JSON文件  
            File.WriteAllText(projectInfoListJsonFile, JsonConvert.SerializeObject(new List<List<GetProjectAttributeModel>>(), Formatting.Indented));
            //清理临时变量
            //projectInfoListTemp.Clear();
        }
        /// <summary>
        /// 创建一个新的空项目详情拆解JSON文件的方法  
        /// </summary>
        public static void CreateEmptyProjectInfoPropertyListJsonFile()
        {
            // 将空的列表写入JSON文件  
            File.WriteAllText(projectInfoPropertyListJsonFile, JsonConvert.SerializeObject(new List<List<ProjectPropertyModel>>(), Formatting.Indented));
            //清理临时变量
            //projectPropertyListTemp.Clear();
        }
        /// <summary>
        /// 创建一个新的空用户列表JSON文件的方法  
        /// </summary>
        public static void CreateEmptyStatisticsUserInfoListJsonFile()
        {
            // 将空的列表写入JSON文件  
            File.WriteAllText(statisticsUserInfoListJsonFile, JsonConvert.SerializeObject(new List<projectDeptModel>(), Formatting.Indented));
            //清理临时变量
            //projectAllInfoList.Clear();
        }
        /// <summary>
        /// 创建一个新的空用户信息JSON文件的方法  
        /// </summary>
        public static void CreateEmptyUserInfoJsonFile()
        {
            // 将空的列表写入JSON文件  
            File.WriteAllText(userInfoJsonFile, JsonConvert.SerializeObject(new ApplyListModel(), Formatting.Indented));
            //清理临时变量
            //applyInfoTemp.Clear();
        }
        #endregion

        #region 加载本地文件
        /// <summary>
        /// 加载本地的部门用户列表文件并返回文件
        /// </summary>
        /// <param name="applyListFile">审批列表文件</param>
        public static void LoadDetpUserListFromJson(ref List<QzUserResultModel> detpUserListFile)
        {
            // 从JSON文件读取数据并反序列化为ApplyListModel列表  
            string jsonData = File.ReadAllText(deptUserListJsonFile);
            detpUserListFile = JsonConvert.DeserializeObject<List<QzUserResultModel>>(jsonData);
        }
        /// <summary>
        /// 加载本地的流程列表文件并返回文件
        /// </summary>
        /// <param name="applyListFile">审批列表文件</param>
        public static void LoadApplyListFromJson(ref List<ApplyListModel> applyListFile)
        {
            // 从JSON文件读取数据并反序列化为ApplyListModel列表  
            string jsonData = File.ReadAllText(applyListJsonFile);
            applyListFile = JsonConvert.DeserializeObject<List<ApplyListModel>>(jsonData);
        }
        /// <summary>
        /// 加载本地的流程列表文件并返回文件
        /// </summary>
        /// <param name="applyListFile">审批列表文件</param>
        public static void LoadApplyStatisticsListFromJson(ref List<ApplyListModel> applyStatisticsListFile)
        {
            // 从JSON文件读取数据并反序列化为ApplyListModel列表  
            string jsonData = File.ReadAllText(applyStatisticsListJsonFile);
            applyStatisticsListFile = JsonConvert.DeserializeObject<List<ApplyListModel>>(jsonData);
        }
        /// <summary>
        /// 加载本地的流程详情列表文件并返回文件
        /// </summary>
        /// <param name="applyInfoFile">审批详情文件</param>
        public static void LoadApplyInfoDataFromJson(ref List<ApplyInfoModel> applyInfoFile)
        {
            // 从JSON文件读取数据并反序列化为ApplyListModel列表  
            string jsonData = File.ReadAllText(applyInfoJsonFile);
            applyInfoFile = JsonConvert.DeserializeObject<List<ApplyInfoModel>>(jsonData);
        }
        /// <summary>
        /// 加载本地的项目列表文件返回文件
        /// </summary>
        /// <param name="projectListFile"></param>
        public static void LoadProjectListDataFromJson(ref List<ProjectResultModel> projectListFile)
        {
            // 从JSON文件读取数据并反序列化为ApplyListModel列表  
            string jsonData = File.ReadAllText(projectListJsonFile);
            projectListFile = JsonConvert.DeserializeObject<List<ProjectResultModel>>(jsonData);
        }
        /// <summary>
        /// 加载本地的项目列表文件返回文件
        /// </summary>
        /// <param name="projectListFile"></param>
        public static void LoadStatisticsProjectInfoPropertyListDataFromJson(ref List<List<ProjectPropertyModel>> projcetInfoPropertyFileList)
        {
            // 从JSON文件读取数据并反序列化为ApplyListModel列表  
            string jsonData = File.ReadAllText(statisticsProjectListJsonFile);
            projcetInfoPropertyFileList = JsonConvert.DeserializeObject<List<List<ProjectPropertyModel>>>(jsonData);
        }

        /// <summary>
        /// 加载本地的项目属性拆解信息文件并返回文件
        /// </summary>
        /// <param name="projcetInfoPropertyFileList"></param>
        public static void LoadProjectInfoPropertyListDataFromJson(ref List<List<ProjectPropertyModel>> projcetInfoPropertyFileList)
        {
            // 从JSON文件读取数据并反序列化为ApplyListModel列表  
            string jsonData = File.ReadAllText(projectInfoPropertyListJsonFile);
            projcetInfoPropertyFileList = JsonConvert.DeserializeObject<List<List<ProjectPropertyModel>>>(jsonData);
        }
        /// <summary>
        /// 加载本地的项目属性信息文件返回文件
        /// </summary>
        /// <param name="projcetInfoListFile"></param>
        public static void LoadProjectUserInfoListDataFromJson(ref List<projectDeptModel> projectUserInfoListFile)
        {
            // 从JSON文件读取数据并反序列化为ApplyListModel列表  
            string jsonData = File.ReadAllText(statisticsUserInfoListJsonFile);
            projectUserInfoListFile = JsonConvert.DeserializeObject<List<projectDeptModel>>(jsonData);
        }
        #endregion

        #region 写入本地方法
        /// <summary>
        /// 向本地流程列表文件写入新流程方法  
        /// </summary>
        /// <param name="newApply">新流程</param>
        public static void AddApplyList(ApplyListModel newApply)
        {
            //读取本地文件到临时变量里
            LoadApplyListFromJson(ref applyListTemp);
            if (applyListTemp.Count == 0)
            {
                // 将新对象添加到列表  
                applyListTemp.Add(newApply);
                // 将更新后的数据写入JSON文件  
                File.WriteAllText(applyListJsonFile, JsonConvert.SerializeObject(applyListTemp, Formatting.Indented));
            }
            else
            {
                //如果传进来的对像在本地文件内就返回这个对象在本地文件里的索引序号；
                var applyListTempIndex = applyListTemp.FindIndex(o => o.id == newApply.id);
                // 检查要添加的数据是否已存在（根据ID检查）  
                if (applyListTempIndex == -1)
                {
                    for (int i = 0; i < applyListTemp.Count; i++)
                    {
                        if (Convert.ToInt32(applyListTemp[i].applyXh) < Convert.ToInt32(newApply.applyXh))
                        {
                            //把新数据写入到i的位置；
                            applyListTemp.Insert(i, newApply);
                            // 将更新后的数据写入JSON文件 
                            File.WriteAllText(applyListJsonFile, JsonConvert.SerializeObject(applyListTemp, Formatting.Indented));
                            break;
                        }
                        else if (i == applyListTemp.Count - 1)
                        {
                            //把新数据写入到i的位置；
                            applyListTemp.Insert(i + 1, newApply);
                            // 将更新后的数据写入JSON文件 
                            File.WriteAllText(applyListJsonFile, JsonConvert.SerializeObject(applyListTemp, Formatting.Indented));
                            break;
                        }
                    }
                }
                else
                {
                    //移除原有数据；
                    applyListTemp.RemoveAt(applyListTempIndex);
                    //根据序号插入新数据；
                    applyListTemp.Insert(applyListTempIndex, newApply);
                    //写入数据到本地文件
                    File.WriteAllText(applyListJsonFile, JsonConvert.SerializeObject(applyListTemp, Formatting.Indented));
                }
            }
        }

        /// <summary>
        /// 向列表中添加新ApplyListModel对象的方法  
        /// </summary>
        /// <param name="newApply"></param>
        public static void AddApplyStatisticsList(ApplyListModel newApply)
        {
            //读取本地文件到临时变量里
            //LoadApplyListFromJson(ref applyStatisticsListTemp);
            LoadApplyStatisticsListFromJson(ref applyStatisticsListTemp);
            if (applyStatisticsListTemp.Count == 0)
            {
                // 将新对象添加到列表  
                applyStatisticsListTemp.Add(newApply);
                // 将更新后的数据写入JSON文件  
                File.WriteAllText(applyStatisticsListJsonFile, JsonConvert.SerializeObject(applyStatisticsListTemp, Formatting.Indented));
            }
            else
            {
                //如果传进来的对像在本地文件内就返回这个对象在本地文件里的索引序号；
                var applyListTempIndex = applyStatisticsListTemp.FindIndex(o => o.id == newApply.id);
                // 检查要添加的数据是否已存在（根据ID检查）  
                if (applyListTempIndex == -1)
                {
                    for (int i = 0; i < applyStatisticsListTemp.Count; i++)
                    {
                        if (Convert.ToInt32(applyStatisticsListTemp[i].applyXh) < Convert.ToInt32(newApply.applyXh))
                        {
                            //把新数据写入到i的位置；
                            applyStatisticsListTemp.Insert(i, newApply);
                            // 将更新后的数据写入JSON文件 
                            File.WriteAllText(applyStatisticsListJsonFile, JsonConvert.SerializeObject(applyStatisticsListTemp, Formatting.Indented));
                            break;
                        }
                        else if (i == applyStatisticsListTemp.Count - 1)
                        {
                            //把新数据写入到i的位置；
                            applyStatisticsListTemp.Insert(i + 1, newApply);
                            // 将更新后的数据写入JSON文件 
                            File.WriteAllText(applyStatisticsListJsonFile, JsonConvert.SerializeObject(applyStatisticsListTemp, Formatting.Indented));
                            break;
                        }
                    }
                }
                else
                {
                    //移除原有数据；
                    applyStatisticsListTemp.RemoveAt(applyListTempIndex);
                    //根据序号插入新数据；
                    applyStatisticsListTemp.Insert(applyListTempIndex, newApply);
                    //写入数据到本地文件
                    File.WriteAllText(applyStatisticsListJsonFile, JsonConvert.SerializeObject(applyStatisticsListTemp, Formatting.Indented));
                }
            }
        }

        /// <summary>
        /// 向列表中添加新ApplyListModel对象的方法  
        /// </summary>
        /// <param name="newApply"></param>
        public static void AddApplyInfoList(ApplyInfoModel newApply)
        {
            applyInfoTemp.Clear();
            ///加载本地文件
            LoadApplyInfoDataFromJson(ref applyInfoTemp);
            //如果是第一个数据，那么就加入到这个list内
            if (applyInfoTemp.Count == 0)
            {
                applyInfoTemp.Add(newApply);
                File.WriteAllText(applyInfoJsonFile, JsonConvert.SerializeObject(applyInfoTemp, Formatting.Indented));
            }
            else
            {
                //查找本地是不是有新加入的元素，要是有就拿到这个元素所在的序号
                int newApplyIndex = applyInfoTemp.FindIndex(o => o.id == newApply.id);
                if (newApplyIndex == -1)
                {
                    for (int i = 0; i < applyInfoTemp.Count; i++)
                    {
                        if (Convert.ToInt32(applyInfoTemp[i].applyXh) < Convert.ToInt32(newApply.applyXh))
                        {
                            //把新数据写入到i的位置；
                            applyInfoTemp.Insert(i, newApply);
                            // 将更新后的数据写入JSON文件 
                            File.WriteAllText(applyInfoJsonFile, JsonConvert.SerializeObject(applyInfoTemp, Formatting.Indented));
                            break;
                        }
                        else if (i == applyInfoTemp.Count - 1)
                        {
                            //把新数据写入到i的位置；
                            applyInfoTemp.Insert(i + 1, newApply);
                            // 将更新后的数据写入JSON文件 
                            File.WriteAllText(applyInfoJsonFile, JsonConvert.SerializeObject(applyInfoTemp, Formatting.Indented));
                            break;
                        }
                    }
                }
                else if (applyInfoTemp[newApplyIndex].result.ToString() != newApply.result.ToString())
                {
                    //删除原有的这条
                    applyInfoTemp.RemoveAt(newApplyIndex);
                    // 将新对象添加到列表  
                    applyInfoTemp.Insert(newApplyIndex, newApply);
                    // 将更新后的数据写入JSON文件  
                    File.WriteAllText(applyInfoJsonFile, JsonConvert.SerializeObject(applyInfoTemp, Formatting.Indented));
                    //调取本地的审批列表
                    LoadApplyStatisticsListFromJson(ref applyStatisticsListTemp);
                    //找到改变的数据所在的列表位置序号
                    var applyListNumber = applyStatisticsListTemp.FindIndex(o => o.id == newApply.id);
                    if (applyStatisticsListTemp.Count != 0)
                    {
                        //改列表的审批状态
                        applyStatisticsListTemp[applyListNumber].result = newApply.result;
                        //改列表的审批时间
                        applyStatisticsListTemp[applyListNumber].lastTime = newApply.resultTime;

                    }


                    File.WriteAllText(applyListJsonFile, JsonConvert.SerializeObject(applyStatisticsListTemp[applyListNumber], Formatting.Indented));
                }
                ;
            }
        }

        /// <summary>
        /// 向项目列表中添加新的项目对象方法(只读项目列表，不读取项目属性)  
        /// </summary>
        /// <param name="newApply"></param>
        public static void AddProjectList(ProjectResultModel newProjectListItem)
        {
            //清除缓存文件
            projectListTemp.Clear();
            //读取本地文件
            LoadProjectListDataFromJson(ref projectListTemp);
            if (projectListTemp.Count == 0)
            {
                // 将新对象添加到列表  
                projectListTemp.Add(newProjectListItem);
                // 将更新后的数据写入JSON文件  
                File.WriteAllText(projectListJsonFile, JsonConvert.SerializeObject(projectListTemp, Formatting.Indented));
            }
            else
            {
                //如果传进来的对像在本地文件内就返回这个对象在本地文件里的索引序号；
                var projectListTempIndex = projectListTemp.FindIndex(o => o.id == newProjectListItem.id);
                // 检查要添加的数据是否已存在（根据ID检查）  
                if (projectListTempIndex == -1)
                {
                    for (int i = 0; i < projectListTemp.Count; i++)
                    {
                        if (projectListTemp[i].createTime.ToString() == "")
                        {
                            //把新数据写入到i的位置；
                            projectListTemp.Insert(i, newProjectListItem);
                            // 将更新后的数据写入JSON文件 
                            File.WriteAllText(projectListJsonFile, JsonConvert.SerializeObject(projectListTemp, Formatting.Indented));
                            break;
                        }
                        //判断新加入的项目创建时间是不是在指定的时间范围内
                        else if (newProjectListItem.createTime.ToString() == "" || Convert.ToDateTime(projectListTemp[i].createTime) < Convert.ToDateTime(newProjectListItem.createTime))
                        {
                            //把新数据写入到i的位置；
                            projectListTemp.Insert(i, newProjectListItem);
                            // 将更新后的数据写入JSON文件 
                            File.WriteAllText(projectListJsonFile, JsonConvert.SerializeObject(projectListTemp, Formatting.Indented));
                            break;
                        }
                        else if (i == projectListTemp.Count - 1)
                        {
                            //把新数据写入到i的位置；
                            projectListTemp.Insert(i + 1, newProjectListItem);
                            // 将更新后的数据写入JSON文件 
                            File.WriteAllText(projectListJsonFile, JsonConvert.SerializeObject(projectListTemp, Formatting.Indented));
                            break;
                        }
                    }
                }
                else
                {
                    //移除原有数据；
                    projectListTemp.RemoveAt(projectListTempIndex);
                    //根据序号插入新数据；
                    projectListTemp.Insert(projectListTempIndex, newProjectListItem);
                    //写入数据到本地文件
                    File.WriteAllText(projectListJsonFile, JsonConvert.SerializeObject(projectListTemp, Formatting.Indented));
                }
            }
        }

        /// <summary>
        /// 向列表中添加新项目信息对象方法  
        /// </summary>
        /// <param name="newApply"></param>
        public static void AddStatisticsProjectPropertieList(List<List<ProjectPropertyModel>> ProjectPropertieListS)
        {
            File.WriteAllText(statisticsProjectListJsonFile, JsonConvert.SerializeObject(ProjectPropertieListS, Formatting.Indented));
        }

        /// <summary>
        /// 向项目详细分解list内加入对象
        /// </summary>
        public static void AddProjectInfoPropertyList(List<ProjectPropertyModel> newProjectPropertyList)
        {
            projectPropertyListTemp.Clear();
            ///加载本地文件
            LoadProjectInfoPropertyListDataFromJson(ref projectPropertyListTemp);
            //如果是第一个数据，那么就加入到这个list内
            if (projectPropertyListTemp.Count == 0)
            {
                projectPropertyListTemp.Add(newProjectPropertyList);
                File.WriteAllText(projectInfoPropertyListJsonFile, JsonConvert.SerializeObject(projectPropertyListTemp, Formatting.Indented));
            }
            else
            {
                //查找本地是不是有新加入的元素，要是有就拿到这个元素所在的序号
                int newProjectPropertyListIndex = projectPropertyListTemp.FindIndex(o => o[0].Value == newProjectPropertyList[0].Value);
                if (newProjectPropertyListIndex == -1)
                {
                    for (int i = 0; i < projectPropertyListTemp.Count; i++)
                    {
                        if (Convert.ToInt64(new string(projectPropertyListTemp[i][0].Value.Where(c => char.IsDigit(c)).ToArray())) < Convert.ToInt64(new string(newProjectPropertyList[0].Value.Where(c => char.IsDigit(c)).ToArray())))
                        {
                            //把新数据写入到i的位置；
                            projectPropertyListTemp.Insert(i, newProjectPropertyList);
                            // 将更新后的数据写入JSON文件 
                            File.WriteAllText(projectInfoPropertyListJsonFile, JsonConvert.SerializeObject(projectPropertyListTemp, Formatting.Indented));
                            break;
                        }
                        else if (i == projectPropertyListTemp.Count - 1)
                        {
                            //把新数据写入到i的位置；
                            projectPropertyListTemp.Insert(i + 1, newProjectPropertyList);
                            // 将更新后的数据写入JSON文件 
                            File.WriteAllText(projectInfoPropertyListJsonFile, JsonConvert.SerializeObject(projectPropertyListTemp, Formatting.Indented));
                            break;
                        }
                    }
                }
                else if (projectPropertyListTemp[newProjectPropertyListIndex][0].Value.ToString() == newProjectPropertyList[0].Value.ToString())
                {
                    //删除原有的这条
                    projectPropertyListTemp.RemoveAt(newProjectPropertyListIndex);
                    // 将新对象添加到列表  
                    projectPropertyListTemp.Insert(newProjectPropertyListIndex, newProjectPropertyList);
                    // 将更新后的数据写入JSON文件  
                    File.WriteAllText(projectInfoPropertyListJsonFile, JsonConvert.SerializeObject(projectPropertyListTemp, Formatting.Indented));
                }
                ;
            }
        }

        /// <summary>
        /// 向列表中添加新userList对象的方法  ancestors
        /// </summary>
        /// <param name="newApply"></param>
        public static void Read_Mysql_ProjectUserAllInfoList(string userId, string startTime, string endTime, ref int fileNumber, ref double folded)
        {
            #region 加载本地、链接Mysql获得数据
            //创建一个空的本地文件
            CreateEmptyStatisticsUserInfoListJsonFile();
            //加载本地文件
            LoadProjectUserInfoListDataFromJson(ref projectUserInfoListTemp);

            if (projectListTemp.Count == 0)
            {
                projectListTemp = new List<ProjectResultModel>();
                //加载本地的项目列表
                LoadProjectListDataFromJson(ref projectListTemp);
            }
            if (deptUserListTemp.Count == 0)
            {
                //清理部门用户临时文件
                deptUserListTemp = new List<QzUserResultModel>();
                //加载本地的用户列表
                LoadDetpUserListFromJson(ref deptUserListTemp);
            }
            ///部门项目列表变量
            var deptInfoListTemp = new List<projectDeptModel>();
            //与本地数据库通信拿到部门列表
            DataTable sqliteDeptList = SQLiteDataBase.SearchTableFromSQLite("qz_dept");

            //取回Mysql内qz_project表内的数据
            DataTable userProjectFileDataList = SQLiteDataBase.GetDataFromMysql("qz_project", "user_id", userId, "folded", 0, startTime, endTime);
            //拿到用户名
            var userName = deptUserListTemp[deptUserListTemp.FindIndex(x => x.id == userId)].realName;
            #endregion
            DataTable userProjectFileDataListResult = new DataTable();

            if (userProjectFileDataList.Rows.Count > 0)
            {
                // 使用LINQ查询来筛选出不重复的行，并处理name列的值
                ///*
                userProjectFileDataListResult = userProjectFileDataList.AsEnumerable()
                .GroupBy(row => new
                {
                    Name = row.Field<string>("name"),
                    Ancestors = row.Field<string>("ancestors")
                })
                .Select(group => group.First())
                .Select(row =>
                {
                    // 获取当前行的name列的值  
                    string name = row.Field<string>("name");
                    //找到字符串中".pdf"的索引位置  
                    int pdfIndex = name.IndexOf(".pdf");
                    // 如果pdfIndex大于0，说明找到了".pdf" 
                    if (pdfIndex > 0)
                    {
                        // 截取从字符串开始到".pdf"之前的部分  
                        name = name.Substring(0, pdfIndex);
                    }
                    // 找到字符串中最后一个"-"的索引位置  
                    int lastHyphenIndex = name.LastIndexOf('-');
                    // 如果lastHyphenIndex大于0，说明找到了一个"-"符号  
                    if (lastHyphenIndex > 0)
                    {
                        // 截取从字符串开始到最后一个"-"之前的部分  
                        name = name.Substring(0, lastHyphenIndex);
                    }
                    // 更新当前行的name字段为处理后的值  
                    row.SetField("name", name);
                    // 返回更新后的行  
                    return row;
                })
                  // 根据处理后的name和ancestors列进行分组  
                  .CopyToDataTable();

                /* 加入去重处理后的数据 
                //从userProjectFileDataList中获取每一行数据，并将其转换为可枚举集合
                userProjectFileDataListResult = userProjectFileDataList.AsEnumerable()
                  .Select(row =>
                  {
                      // 获取当前行的name列的值  
                      string name = row.Field<string>("name");
                      // 找到字符串中".pdf"的索引位置  
                      int pdfIndex = name.IndexOf(".pdf");
                      // 如果pdfIndex大于0，说明找到了".pdf"  
                      if (pdfIndex > 0)
                      {
                          // 截取从字符串开始到".pdf"之前的部分  
                          name = name.Substring(0, pdfIndex);
                      }
                      // 找到字符串中最后一个"-"的索引位置  
                      int lastHyphenIndex = name.LastIndexOf('-');
                      // 如果lastHyphenIndex大于0，说明找到了一个"-"符号  
                      if (lastHyphenIndex > 0)
                      {
                          // 截取从字符串开始到最后一个"-"之前的部分  
                          name = name.Substring(0, lastHyphenIndex);
                      }
                      // 更新当前行的name字段为处理后的值  
                      row.SetField("name", name);
                      // 返回更新后的行  
                      return row;
                  })
                  // 根据处理后的name和ancestors列进行分组  
                  .GroupBy(row => new
                  {
                      // 为分组指定一个匿名类型，包含处理后的name和ancestors  
                      Name = row.Field<string>("name"),
                      Ancestors = row.Field<string>("ancestors")
                  })
                  // 选择每个组中的第一行（去除重复项）  
                  .Select(group => group.First())
                  // 将结果转换为DataTable形式  
                  .CopyToDataTable();
                */
                #region 初始化变量
                fileNumber = 0;
                folded = 0;
                var userItem = new UserListModel();
                userItem.projectUserId = userId;
                userItem.projectUserName = userName;
                //一个部门变量 
                var deptInfoItemTemp = new projectDeptModel();
                //初始化部门容器存入项目
                deptInfoItemTemp.projectInfoList = new List<ProjectInfoListModel>();
                //一个项目信息表
                var projectInfoItem = new ProjectInfoListModel();
                //初始化项目容器存入阶段；
                projectInfoItem.projectStageList = new List<ProjectStageModel>();
                //初始化一个阶段变量；
                var projectStageTemp = new ProjectStageModel();
                //初始化阶段容器存入专业；
                projectStageTemp.projectMajroList = new List<StageMajroModel>();
                //建立一个专业变量
                var majroItem = new StageMajroModel();
                //初始化专业容器存入子项
                majroItem.subProjectList = new List<subProjectListModel>();
                //初始化一个子项目变量
                var subProjectItem = new subProjectListModel();
                //初始化子项容器存入角色
                subProjectItem.projectRoleList = new List<RoleModel>();
                //人员下的文件数量容器
                userItem.subProjectFileNumberS = new List<SubProjectFileNumber>();
                //人员下的文件数量变量
                var userSubFileNum = new SubProjectFileNumber();
                //角色变量
                var roleItem = new RoleModel();
                #endregion
                //循环每一行
                foreach (DataRow row in userProjectFileDataListResult.Rows)
                {
                    //每行的ancestors列下的内容
                    var ancestorsStr = row["ancestors"].ToString();
                    //分割ancestors内的字符串
                    string[] ancestorsStrS = ancestorsStr.Split(',');

                    #region 部门
                    //判断是不是在部门列表里以存在,找到了，返回所在的位置
                    var deptItemIndex = deptInfoListTemp.FindIndex(o => o.projectDeptId == ancestorsStrS[0]);
                    //判断部门列表是不是为0或没找到所在部门-1
                    if (deptInfoListTemp.Count == 0 || deptItemIndex == -1)
                    {
                        //临时一个部门变量 
                        deptInfoItemTemp = new projectDeptModel();
                        //赋值第1位是部门id
                        deptInfoItemTemp.projectDeptId = ancestorsStrS[0];
                        // 遍历DataTable中的每一行
                        foreach (DataRow sqliteDeptListRow in sqliteDeptList.Rows)
                        {
                            // 检查dept_id列的值与取到的部门id相同
                            if (sqliteDeptListRow["dept_id"].ToString() == ancestorsStrS[0])
                            {
                                // 如果找到匹配的dept_id，获取dept_name列的值，拿到部门名称
                                deptInfoItemTemp.projectDeptName = sqliteDeptListRow["dept_name"].ToString();
                                // 找到后可以跳出循环
                                break;
                            }
                        }
                        //部门列表加入一个部门
                        deptInfoListTemp.Add(deptInfoItemTemp);
                    }

                    #endregion

                    #region 项目
                    if (deptItemIndex == -1)
                        //找到部门index
                        deptItemIndex = deptInfoListTemp.FindIndex(o => o.projectDeptId == ancestorsStrS[0]);
                    //判读这个部门里是不是有这个项目
                    if (deptInfoListTemp[deptItemIndex].projectInfoList == null || !deptInfoListTemp[deptItemIndex].projectInfoList.Any(o => o.projectId == ancestorsStrS[1]))
                    {
                        projectInfoItem = new ProjectInfoListModel();
                        // 赋值项目id
                        projectInfoItem.projectId = ancestorsStrS[1];
                        //赋值项目名称
                        projectInfoItem.projectName = projectListTemp[projectListTemp.FindIndex(o => o.id == ancestorsStrS[1])].name;
                        //赋值项目编号
                        projectInfoItem.projectNo = projectListTemp[projectListTemp.FindIndex(o => o.id == ancestorsStrS[1])].identifier;
                        if (deptInfoItemTemp.projectInfoList == null)//如果部门内没有这个项目
                        {
                            //部门下项目列表内加入一个项目
                            deptInfoListTemp[deptItemIndex].projectInfoList = new List<ProjectInfoListModel>() { projectInfoItem };
                        }
                        else//如果有这个项目
                        {
                            //部门下项目列表内加入一个项目
                            deptInfoListTemp[deptItemIndex].projectInfoList.Add(projectInfoItem);
                        }
                    }
                    #endregion

                    #region 阶段
                    //在部门内查找是不是有这个项目，没有为-1
                    var projectItemIndex = deptInfoListTemp[deptItemIndex].projectInfoList.FindIndex(o => o.projectId == ancestorsStrS[1]);

                    //判断是不是在这个项目内有这个阶段

                    if (deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList == null || !deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList.Any(o => o.projectStageId == ancestorsStrS[2]))
                    {
                        projectStageTemp = new ProjectStageModel();
                        //赋值阶段id
                        projectStageTemp.projectStageId = ancestorsStrS[2];

                        //取回Mysql内qz_project表内的数据
                        DataTable stageDataList = SQLiteDataBase.GetDataFromMysql("qz_project", "id", ancestorsStrS[2]);
                        DataRow stageDataRow = stageDataList.Rows[0];
                        //赋值阶段名称
                        projectStageTemp.projectStageName = stageDataRow["name"].ToString();
                        if (deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList == null)
                        {
                            deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList = new List<ProjectStageModel>() { projectStageTemp };
                        }
                        else
                        {
                            //项目下加入阶段
                            deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList.Add(projectStageTemp);
                        }
                    }
                    #endregion

                    #region 专业
                    //拿到阶段的Index
                    var stageItemIndex = deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList.FindIndex(o => o.projectStageId == ancestorsStrS[2]);

                    string majroId = ancestorsStrS[5];
                    //取回Mysql内qz_project表内的专业名数据
                    DataTable majroDataList = SQLiteDataBase.GetDataFromMysql("qz_project", "id", majroId);
                    DataRow majroDataRow = majroDataList.Rows[0];
                    //去除专业后面的（）与里面的内容
                    string majroName = Regex.Replace(majroDataRow["name"].ToString(), @"\(.*?\)", "");

                    //判断是不是有这个专业
                    if (deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList == null || !deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList.Any(o => o.projectMajroName == majroName))
                    {
                        //初始化这个专业
                        majroItem = new StageMajroModel();
                        majroItem.projectMajroId = majroId;
                        majroItem.projectMajroName = majroName;
                        if (deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList == null)
                        {
                            deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList = new List<StageMajroModel> { majroItem };
                        }
                        else
                        {
                            //阶段下加入专业
                            deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList.Add(majroItem);
                        }
                    }
                    #endregion

                    #region 子项
                    //判断是不是有这个专业
                    var majroItemIndex = deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList.FindIndex(o => o.projectMajroName == majroName);

                    //取回Mysql内qz_project表内子项的数据
                    DataTable subProDataList = SQLiteDataBase.GetDataFromMysql("qz_project", "id", ancestorsStrS[4]);
                    DataRow subProDataRow = subProDataList.Rows[0];
                    string tempSubProjectName = subProDataRow["name"].ToString();
                    //查找是不是有这个子项，没有值为-1

                    if (deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList == null || !deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList.Any(o => o.subProjectName == tempSubProjectName))
                    {
                        //初始化子项目
                        subProjectItem = new subProjectListModel();
                        //赋值子项目id
                        subProjectItem.subProjectId = ancestorsStrS[4];
                        subProjectItem.subProjectName = tempSubProjectName;
                        if (deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList == null)
                        {
                            deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList = new List<subProjectListModel> { subProjectItem };
                            //人员下的文件数量变量
                            userSubFileNum = new SubProjectFileNumber();
                        }
                        else
                        {
                            //专业下加入子项目
                            deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList.Add(subProjectItem);
                        }
                    }

                    #endregion

                    #region 角色、人员

                    var subProItemIndex = deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList.FindIndex(o => o.subProjectName == tempSubProjectName);
                    //取回Mysql内qz_project_user_role表内要找的人的角色数据
                    DataTable roleDataList = SQLiteDataBase.GetDataFromMysql("qz_project_user_role", "order_id", ancestorsStrS[3], "user_id", userId);

                    //循环角色列表
                    foreach (DataRow mysqlRoleRowItem in roleDataList.Rows)
                    {
                        //拿到角色id
                        string roleId = mysqlRoleRowItem["role_id"].ToString();
                        //拿到角色名
                        DataTable roleList = SQLiteDataBase.SearchTableFromSQLite("qz_role", "role_id", $"{roleId}");
                        foreach (DataRow roleRow in roleList.Rows)
                        {
                            //查找是不是有这个角色，没有返回-1
                            if (deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList[subProItemIndex].projectRoleList == null || !deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList[subProItemIndex].projectRoleList.Any(o => o.projectRoleId == roleRow["role_id"].ToString()))
                            {
                                //角色变量初始化
                                roleItem = new RoleModel();
                                roleItem.projectRoleId = roleId;
                                roleItem.projectRoleName = roleRow["role_name"].ToString();
                                if (deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList[subProItemIndex].projectRoleList == null)
                                {
                                    //子项下加入角色
                                    deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList[subProItemIndex].projectRoleList = new List<RoleModel> { roleItem };
                                }
                                else
                                {
                                    deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList[subProItemIndex].projectRoleList.Add(roleItem);
                                }
                            }
                        }
                    }

                    foreach (var itemRole in deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList[subProItemIndex].projectRoleList)
                    {
                        if (itemRole.projectRoleName == "设计人")
                        {
                            if (itemRole.projectUserListModel == null)
                            {
                                userItem.subProjectFileNumberS.Clear();
                                //人员下的文件数量变量
                                userSubFileNum = new SubProjectFileNumber();
                                //文件数量
                                userSubFileNum.fileNumber = userSubFileNum.fileNumber + 1;
                                //A1数量
                                userSubFileNum.A1SizeNumber = userSubFileNum.A1SizeNumber + Convert.ToDouble(row["folded"]);
                                //人员容器下加入子项文件数量；
                                userItem.subProjectFileNumberS.Add(userSubFileNum);
                                //制作一个副本，防止改了原有值后列队会改变
                                var userItemCopy = new UserListModel
                                {
                                    projectUserId = userItem.projectUserId,
                                    projectUserName = userItem.projectUserName,
                                    subProjectFileNumberS = new List<SubProjectFileNumber>(userItem.subProjectFileNumberS)
                                };
                                //角色容器初始与加入人员
                                itemRole.projectUserListModel = new List<UserListModel> { userItemCopy };
                            }
                            else
                            {
                                itemRole.projectUserListModel[0].subProjectFileNumberS[0].fileNumber++;
                                itemRole.projectUserListModel[0].subProjectFileNumberS[0].A1SizeNumber = itemRole.projectUserListModel[0].subProjectFileNumberS[0].A1SizeNumber + Convert.ToDouble(row["folded"]);
                            }
                        }
                        else
                        {
                            if (itemRole.projectUserListModel == null)
                            {
                                //制作一个副本，防止改了原有值后列队会改变
                                var userItemCopy1 = new UserListModel
                                {
                                    projectUserId = userItem.projectUserId,
                                    projectUserName = userItem.projectUserName,
                                    subProjectFileNumberS = new List<SubProjectFileNumber>(userItem.subProjectFileNumberS)
                                };
                                //角色容器初始与加入人员
                                itemRole.projectUserListModel = new List<UserListModel> { userItemCopy1 };
                            }
                        }
                    }
                    #endregion

                    fileNumber = fileNumber + 1;
                    folded = folded + Convert.ToDouble(row["folded"]);
                    // 将更新后的数据写入JSON文件  
                    File.WriteAllText(statisticsUserInfoListJsonFile, JsonConvert.SerializeObject(deptInfoListTemp, Formatting.Indented));
                }

            }

        }

        /// <summary>
        /// 添加用户列表
        /// </summary>
        public static void JsonFileDeptUserList()
        {
            //用户列表初始化
            deptUserListTemp.Clear();
            //从JSON文件中加载部门用户列表
            LoadDetpUserListFromJson(ref deptUserListTemp);
            //判断部门用户列表是否为空
            if (AppGlobalModel.DeptList != null && AppGlobalModel.DeptList.Any())
            {
                //在组织架构内循环
                foreach (var deptItem in AppGlobalModel.DeptList)
                {
                    //从组织架构内获取部门用户列表
                    var resultData = new List<QzUserResultModel>();
                    //获取部门用户列表 请求
                    if (HttpGet(AppGlobalModel.GetDeptUserList + "?deptId=" + deptItem.deptId, ref resultData))
                    {
                        //循环部门用户列表
                        foreach (var item in resultData)
                        {
                            //判断部门用户列表中是否有此用户
                            if (!deptUserListTemp.Exists(x => x.realName == item.realName && item.deptName != null))
                            {
                                deptUserListTemp.Add(item);//添加用户
                            }
                        }
                    }
                }
            }
            // 对内部数据进行字母排序
            deptUserListTemp.Sort((x, y) => x.realName.CompareTo(y.realName));
            File.WriteAllText(deptUserListJsonFile, JsonConvert.SerializeObject(deptUserListTemp, Formatting.Indented));
        }

        /// <summary>
        /// 添加用户列表
        /// </summary>
        /// <param name="newApply"></param>
        public static void JsonFileSQLiteUserList()
        {
            //获取SQLite用户列表 
            var sQLiteUserList = SQLiteDataBase.SearchTableFromSQLite("qz_user");
            //获取内部用户列表
            deptUserListTemp.Clear();
            //加载本地的部门用户列表文件并返回文件
            LoadDetpUserListFromJson(ref deptUserListTemp);
            for (int i = 0; i < sQLiteUserList.Rows.Count; i++)
            {
                var userData = new QzUserResultModel();
                var userName = sQLiteUserList.Rows[i].ItemArray[5].ToString();//在用户表中找到人员名
                if (!deptUserListTemp.Exists(x => x.realName == userName))
                {
                    userData.realName = userName;
                    userData.userName = sQLiteUserList.Rows[i].ItemArray[5].ToString(); ;
                    //userData.deptName = sQLiteUserList.Rows[i].ItemArray[1].ToString(); ;
                    userData.id = sQLiteUserList.Rows[i].ItemArray[0].ToString();
                    var sQLiteUserDeptPostList = SQLiteDataBase.SearchTableFromSQLite("qz_user_dept_post", "user_id", userData.id);
                    foreach (DataRow row in sQLiteUserDeptPostList.Rows)
                    {
                        var userDeptId = row["dept_id"].ToString();

                        foreach (DataRow deptName in SQLiteDataBase.SearchTableFromSQLite("qz_dept", "dept_id", userDeptId).Rows)
                        {
                            userData.deptName = deptName["dept_name"].ToString() + "、" + userData.deptName;
                        }
                        ;
                    }
                    deptUserListTemp.Add(userData);
                }
            }

            // 对内部数据进行字母排序
            deptUserListTemp.Sort((x, y) => x.realName.CompareTo(y.realName));
            File.WriteAllText(deptUserListJsonFile, JsonConvert.SerializeObject(deptUserListTemp, Formatting.Indented));

        }
        #endregion

        #region 读取服务器同步到本地
        /// <summary>
        /// 按传入的参数(设定时间范围内的)读取服务器里审批流程列表数据
        /// </summary>
        /// <param name="queryApply">调取服务器的参数:1：processtypeid：流程类型：（0签名签章 1出版 2下载 3归档 4其他，不传就是查询所有）；2：type审批状态（0我发起的 1待审批 2已审批，不传就是查询所有）；3：pageNum 页数；4：pageSize 要查询的条数；5：proName 项目名称；6：userName 发起人；7：startTime 开始时间；8：endTime 结束时间  </param>
        /// <param name="createTime">查找的流程开始时间</param>
        /// <param name="endTime">查询条件的结束时间</param>
        //public static void ReadApplyListHttpDatas(QueryApply queryApply, DateTime createTime, DateTime endTime)
        //{
        //    int totalRows = 0;
        //    allApplyListTemp.Clear();
        //    //CreateEmptyApplyListJsonFile();
        //    while (true)
        //    {
        //        //清空临时申请列表
        //        applyListTemp.Clear();
        //        //获取总页数
        //        bool countEnd = false;
        //        //向服务器调取审批流程列表
        //        if (HttpPost(AppGlobalModel.ApplyList, queryApply, ref applyListTemp, ref totalRows))
        //        {
        //            //如果没有数据，结束循环
        //            if (applyListTemp.Count == 0) break;
        //            int i = 0;//循环次数用来判断是不是最后一条数据
        //            //循环所有流程
        //            foreach (var applyTempItem in applyListTemp)
        //            {
        //                i++;
        //                //判断调取的数据流程时间的“创建时间”是不是在指定的时间范围内；
        //                if (Convert.ToDateTime(applyTempItem.createTime) >= createTime)
        //                {
        //                    //把查找出来的流程写入本地文件中
        //                    allApplyListTemp.Add(applyTempItem);
        //                    if (i == applyListTemp.Count)
        //                        queryApply.pageNum += 1; //如果是在查询时间内，那么就再加一面，继续查询，直到没在指定时间范围内；
        //                }
        //                else
        //                {
        //                    countEnd = true;
        //                    break;
        //                }
        //            }
        //        }
        //        if (countEnd) break;
        //    }
        //    // 原始写法：
        //    foreach (var applyTempItem in allApplyListTemp)
        //    {
        //        //把查找出来的流程写入本地文件中
        //        AddApplyList(applyTempItem);
        //    }
        //}


        public static void ReadApplyListHttpDatas(QueryApply queryApply, DateTime createTime, DateTime endTime)
        {
            int totalRows = 0;// 服务器返回的总记录数
            allApplyListTemp.Clear(); // 存储所有符合条件的记录
            //CreateEmptyApplyListJsonFile(); // 如果需要，可以在这里创建空文件
            int safetyCounter = 0; // 安全计数器，防止死循环
            const int maxIterations = 100; // 最大迭代次数，根据实际情况调整
            string lastPageSignature = null; // 上一页的签名，用于检测重复页

            while (true)
            {
                if (++safetyCounter > maxIterations)// 超过安全迭代次数，退出循环
                    break;

                applyListTemp.Clear(); // 清空临时列表，准备接收新页数据
                bool success = HttpPost(AppGlobalModel.ApplyList, queryApply, ref applyListTemp, ref totalRows); // 请求服务器获取当前页数据

                if (!success) // 请求失败，退出循环
                    break;

                if (applyListTemp.Count == 0) // 服务器返回空页，结束
                    break;

                // 检测重复页（简单使用第一条id作签名）
                string currentSignature = applyListTemp.FirstOrDefault()?.id ?? Guid.NewGuid().ToString();
                if (lastPageSignature != null && currentSignature == lastPageSignature)
                {
                    // 服务器可能在越界时重复返回最后一页，退出防止死循环
                    break;
                }
                lastPageSignature = currentSignature;

                // 处理本页数据：只收集 >= createTime 的记录，遇到早于 createTime 的记录则停止
                bool encounteredOlder = false;
                foreach (var applyTempItem in applyListTemp)
                {
                    DateTime itemTime;
                    if (!DateTime.TryParse(applyTempItem.createTime, out itemTime))
                    {
                        // 解析失败，跳过该条或根据需求处理
                        continue;
                    }

                    if (itemTime >= createTime) // 在时间范围内，添加到结果列表
                    {
                        allApplyListTemp.Add(applyTempItem); // 把查找出来的流程写入本地文件中
                    }
                    else
                    {
                        encounteredOlder = true; // 遇到早于 createTime 的记录，标记并准备退出
                        break;
                    }
                }

                // 使用 totalRows + pageSize 计算总页数，若当前页已是最后页则退出
                if (totalRows > 0 && queryApply.pageSize > 0)
                {
                    int totalPages = (int)Math.Ceiling((double)totalRows / queryApply.pageSize);
                    if (queryApply.pageNum >= totalPages)
                        break;
                }

                if (encounteredOlder)
                    break;

                // 增加页码，继续下一页
                queryApply.pageNum += 1;
            }

            // 写入本地（原逻辑）
            foreach (var applyTempItem in allApplyListTemp)
            {
                AddApplyList(applyTempItem);
            }
        }



        /// <summary>
        /// 按传入的参数读取服务器里审批流程列表数据
        /// </summary>
        /// <param name="queryApply">调取服务器的参数:1：processtypeid：流程类型：（0签名签章 1出版 2下载 3归档 4其他，不传就是查询所有）；2：type审批状态（0我发起的 1待审批 2已审批，不传就是查询所有）；3：pageNum 页数；4：pageSize 要查询的条数；5：proName 项目名称；6：userName 发起人；7：startTime 开始时间；8：endTime 结束时间  </param>
        /// <param name="createTime">查找的流程开始时间</param>
        /// <param name="endTime">查询条件的结束时间</param>
        public static void ReadApplyListHttpDatas(QueryApply queryApply)
        {
            int totalRows = 0;
            applyListTemp.Clear();
            CreateEmptyApplyListJsonFile();
            while (true)
            {
                ///向服务器调取审批流程列表
                if (HttpPost(AppGlobalModel.ApplyList, queryApply, ref applyListTemp, ref totalRows))
                {
                    if (applyListTemp.Count == 0)
                    {
                        break;
                    }
                    ///判断调取的数据最后一条的“审批时间”是不是在指定的时间范围内；
                    if (applyListTemp[applyListTemp.Count - 1].createTime != null && Convert.ToDateTime(applyListTemp[applyListTemp.Count - 1].createTime) >= Convert.ToDateTime(queryApply.startTime) && Convert.ToDateTime(applyListTemp[applyListTemp.Count - 1].createTime) <= Convert.ToDateTime(queryApply.endTime))
                    {
                        //如果是在查询时间内，那么就再加一面，继续查询，直到没在指定时间范围内；
                        queryApply.pageNum += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                foreach (var applyListTempItem in applyListTemp)
                {
                    allApplyListTemp.Add(applyListTempItem);
                }
            }
            foreach (var applyListTempItem in allApplyListTemp)
            {
                AddApplyList(applyListTempItem);
            }
            //后台写入流程列队
            //Thread thread = new Thread(() =>
            //{
            //    foreach (var applyListTempItem in allApplyListTemp)
            //    {
            //        AddApplyList(applyListTempItem);
            //    }
            //});
            //thread.Start();
        }

        /// <summary>
        /// 调取Mysql内的流程列表数据\统计流程所用
        /// </summary>
        /// <param name="applyTypeId">查找的字符串</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        public static void Read_Mysql_ApplyListHttpDatas(string applyTypeId, string startDateTime, string endDateTime)
        {

            //在Mysql中拿到指定时间内的流程列表0：id；1：user_id；2：approval_id；3：name；4：remark；5：fileids；6：result；7：annex_id；8：create_time；9：pro_id；10：processtype_id；11：pages；12：passtime；13：days；14：xh；15：money；16：fileType；
            DataTable mysql_qz_approval_apply_List = SQLiteDataBase.GetApplyDataFromMysql("qz_approval_apply", "approval_id", $"{applyTypeId}", "create_time", startDateTime, endDateTime);

            DataTable mysql_qz_user_List = SQLiteDataBase.GetDataFromMysql("qz_user");

            //清除缓存文件
            projectListTemp.Clear();
            //读取本地文件
            LoadProjectListDataFromJson(ref projectListTemp);

            var applyListTempItem = new ApplyListModel();
            //循环mysql拿到的数据行数
            for (int i = 0; i < mysql_qz_approval_apply_List.Rows.Count; i++)
            {
                if (mysql_qz_approval_apply_List.Rows[i].ItemArray[10].ToString() != "0")//判断是不是为“0”签名签章流程
                {
                    applyListTempItem.id = mysql_qz_approval_apply_List.Rows[i].ItemArray[0].ToString();
                    var userId = mysql_qz_approval_apply_List.Rows[i].ItemArray[1].ToString();//这个拿到的是userid，要翻译成用户名
                    var sQLiteUserData = SQLiteDataBase.SearchTableFromSQLite("qz_user", "id", userId);
                    applyListTempItem.userName = sQLiteUserData.Rows[0].ItemArray[5].ToString();//在用户表中找到人员名
                    applyListTempItem.NAME = mysql_qz_approval_apply_List.Rows[i].ItemArray[3].ToString();
                    //var applyNodeRow = SQLiteDataBase.GetDataFromMysql("qz_approval_apply_node_result", "apply_node_id", $"{applyListTempItem.id}").Rows;
                    //applyListTempItem.remark = applyNodeRow[0]["remark"].ToString();
                    //var resultStr = Convert.ToInt32(applyNodeRow[0]["result"]);
                    //applyListTempItem.result = resultStr;
                    applyListTempItem.createTime = mysql_qz_approval_apply_List.Rows[i].ItemArray[8].ToString();
                    var projectId = mysql_qz_approval_apply_List.Rows[i].ItemArray[9].ToString();//这个拿到的是pro_id，要翻译成项目名
                    applyListTempItem.proName = projectListTemp.Find(o => o.id == projectId).name;
                    applyListTempItem.processtypeId = mysql_qz_approval_apply_List.Rows[i].ItemArray[10].ToString();
                    applyListTempItem.lastTime = mysql_qz_approval_apply_List.Rows[i].ItemArray[12].ToString();
                    applyListTempItem.applyXh = mysql_qz_approval_apply_List.Rows[i].ItemArray[14].ToString();
                    AddApplyStatisticsList(applyListTempItem);
                    applyListTempItem = new ApplyListModel();
                }
            }
        }

        /// <summary>
        /// 读取服务器中一个流程的审批流程详细内容存入到线下文件LoadStatisticsProjectInfoPropertyListDataFromJson
        /// </summary>
        /// <param name="id">传入进来要查询的流程统计Id</param>
        public static void Read_Mysql_ApplyStatisticsInfoHttpDatas(string id)
        {
            #region 
            ///加载本地缓存流程详情文件   
            //"NAME": "只盖章",
            //"processtype_id": "5",
            //"result": 1,
            //"resultName": "已通过",
            //"createTime": "2024-11-08 16:50:28",
            //"resultTime": "2024-11-08 16:52:32",
            //"fileids": "72ec346b62c24ea8bcb83a383f2df03f",
            //"days": "1",
            //"id": "8e73d0c3012845029f86feb396df0dd8",
            //"proId": "WBS2023100900000001",
            //"proName": "内蒙古瑞达泰丰化工有限责任公司年产15万吨钾碱项目",
            //"annex_id": 0,
            //"fileType": 0,
            //"guiId": null,
            //"downUser": null,
            //"money": null,
            //"FoldedAll": 0.25,
            //"FileAll": 1,
            //"applyUser": "付瑶,许秀玫 ",
            //"resultRemark": "【付瑶】同意 ",
            //"nodeName": null
            #endregion
            /*  Mysql Data
                0 id=8e73d0c3012845029f86feb396df0dd8
                1 user_id=HM2021031000001
                2 approval_id=a6d68ee1ed8c4389ac00bb150d92f333
                3 name=只盖章
                4 remark=
                5 fileids=72ec346b62c24ea8bcb83a383f2df03f
                6 result=1
                7 annex_id=//出版份数
                8 create_time=2024-11-08 16:50:28
                9 pro_id=WBS2023100900000001
                10 processtype_id=5
                11 pages=1
                12 passtime=2024-11-08 16:52:32
                13 days=1
                14 xh=5587
                15 money=
                16 fileType=0//流程文件来源0：项目文件区、1：归档区
                17 guild=
                18 guiType=
             */
            #region applyInfoTempFile详情
            ///审批详情:
            //1: applyXh 序号
            //2：appName 流程类型名
            //3：userDpt 用户部门
            //4：remark 备注
            //5：userId 发起人Id
            //6: userName 用户名
            //7: nodeList 节点List
            //8: NAME 流程标题
            //9：processtype_id 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 
            //10：result 审批状态： 0进行中 1已通过 -1未通过
            //11：createTime 提交时间
            //12：resultTime 最后审批时间
            //13: fileids 要是按文件夹发起的就传文件夹id，按项目就传项目id，文件就传文件id，购物车就不用传
            //14：days 流程天数
            //15：id 主键
            //16：proId 项目id
            //17：proName 项目名称
            //18：annex_id 打印份数
            //19: fileType 文件来源0 项目区 1归档区
            //20: guiId 归档使用
            //21: downUser 下载人主键
            //22: money 出版订单金额
            //23: FoldedAll 折A1
            //24: FileAll 文件总数
            #endregion
            if (projectListTemp.Count == 0)//本地的项目列表是不是有数据
            {
                LoadProjectListDataFromJson(ref projectListTemp);
                //LoadStatisticsProjectInfoPropertyListDataFromJson(ref statisticsProjectListTemp);
            }
            if (deptUserListTemp.Count == 0)//本地的用户列表是不是有数据
            {
                LoadDetpUserListFromJson(ref deptUserListTemp);
            }
            //获取mysql数据库内的流程详情
            var mysql_Qz_approval_apply = SQLiteDataBase.GetDataFromMysql("qz_approval_apply", "id", $"{id}");
            var resultDataApplyInfo = new ApplyInfoModel();
            //1: applyXh 序号
            resultDataApplyInfo.applyXh = mysql_Qz_approval_apply.Rows[0].ItemArray[14].ToString();

            var qz_approval = SQLiteDataBase.SearchTableFromSQLite("qz_approval", "id", $"{mysql_Qz_approval_apply.Rows[0].ItemArray[2]}");
            //2：appName 流程类型名
            resultDataApplyInfo.appName = qz_approval.Rows[0].ItemArray[1].ToString();
            //firstordefault是找第一个满足查找到的元素后并返回它
            var userDept = deptUserListTemp.FirstOrDefault(o => o.id == mysql_Qz_approval_apply.Rows[0].ItemArray[1].ToString());
            if (userDept != null)
            {
                //3：userDept 用户部门
                resultDataApplyInfo.userDept = userDept.deptName;
                //6: userName 用户名
                resultDataApplyInfo.userName = userDept.realName;
            }

            //4：remark 备注
            resultDataApplyInfo.remark = mysql_Qz_approval_apply.Rows[0].ItemArray[4].ToString();
            //5：userId 发起人Id
            resultDataApplyInfo.userId = mysql_Qz_approval_apply.Rows[0].ItemArray[1].ToString();

            //7: nodeList 节点List
            resultDataApplyInfo.nodeList = new List<NodeListItem>();/// 1:nodeName 节点名称// 2:result 审批状态：0进行中 1已通过 -1未通过/ 3:applyUser 负责人/ 4:sum 2就代表我审批了/ 5:id 主键/ 6:resultTime 最后审批时间/ 7:resultRemark 备注
            var mysql_qz_approval_apply_node = SQLiteDataBase.GetDataFromMysql("qz_approval_apply_node", "apply_id", mysql_Qz_approval_apply.Rows[0].ItemArray[0].ToString());
            var nodeItem = new NodeListItem();
            for (int i = 0; mysql_qz_approval_apply_node.Rows.Count > i; i++)
            {
                nodeItem.id = mysql_qz_approval_apply_node.Rows[0].ItemArray[0].ToString();
                nodeItem.nodeName = mysql_qz_approval_apply_node.Rows[i].ItemArray[2].ToString();
                nodeItem.result = Convert.ToInt32(mysql_qz_approval_apply_node.Rows[i].ItemArray[7]);
                nodeItem.resultRemark = mysql_qz_approval_apply_node.Rows[i].ItemArray[11].ToString();
                nodeItem.resultTime = mysql_qz_approval_apply_node.Rows[i].ItemArray[0].ToString();
                nodeItem.sum = Convert.ToInt32(mysql_qz_approval_apply_node.Rows[i].ItemArray[12]);
                nodeItem.applyUser = mysql_qz_approval_apply_node.Rows[i].ItemArray[13].ToString();
                resultDataApplyInfo.nodeList.Add(nodeItem);
            }

            //8: NAME 流程标题
            resultDataApplyInfo.NAME = mysql_Qz_approval_apply.Rows[0].ItemArray[3].ToString();
            //9：processtype_id 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 
            resultDataApplyInfo.processtype_id = mysql_Qz_approval_apply.Rows[0].ItemArray[10].ToString();
            //10：result 审批状态： 0进行中 1已通过 -1未通过
            resultDataApplyInfo.result = Convert.ToInt32(mysql_Qz_approval_apply.Rows[0].ItemArray[6]);
            //11：createTime 提交时间
            resultDataApplyInfo.createTime = mysql_Qz_approval_apply.Rows[0].ItemArray[8].ToString();
            //12：resultTime 最后审批时间
            resultDataApplyInfo.resultTime = mysql_Qz_approval_apply.Rows[0].ItemArray[12].ToString();
            //13: fileids 要是按文件夹发起的就传文件夹id，按项目就传项目id，文件就传文件id，购物车就不用传
            resultDataApplyInfo.fileids = mysql_Qz_approval_apply.Rows[0].ItemArray[5].ToString();
            //14：days 流程天数
            resultDataApplyInfo.days = mysql_Qz_approval_apply.Rows[0].ItemArray[13].ToString();
            //15：id 主键
            resultDataApplyInfo.id = mysql_Qz_approval_apply.Rows[0].ItemArray[0].ToString();
            //16：proId 项目id
            resultDataApplyInfo.proId = mysql_Qz_approval_apply.Rows[0].ItemArray[9].ToString();
            var projectName = projectListTemp.Find(x => x.id == resultDataApplyInfo.proId).name;
            if (projectName != null || projectName != "")
            {
                //17：proName 项目名称
                resultDataApplyInfo.proName = projectName; //需要解释
            }
            else
            {
                resultDataApplyInfo.proName = "";
            }
            if (mysql_Qz_approval_apply.Rows[0].ItemArray[7].ToString() == "")
            {
                //18：annex_id 打印份数
                resultDataApplyInfo.annex_id = 0;
            }
            else
            {
                //18：annex_id 打印份数
                resultDataApplyInfo.annex_id = Convert.ToInt32(mysql_Qz_approval_apply.Rows[0].ItemArray[7].ToString());
            }
            //19: fileType 文件来源0 项目区 1归档区
            resultDataApplyInfo.fileType = Convert.ToInt32(mysql_Qz_approval_apply.Rows[0].ItemArray[16].ToString());
            //20: guiId 归档使用
            resultDataApplyInfo.guiId = mysql_Qz_approval_apply.Rows[0].ItemArray[17].ToString();
            //21: downUser 下载人主键
            resultDataApplyInfo.downUser = mysql_Qz_approval_apply.Rows[0].ItemArray[0].ToString();
            //22: money 出版订单金额
            resultDataApplyInfo.money = mysql_Qz_approval_apply.Rows[0].ItemArray[15].ToString();
            //拿到流程文件数量
            var mysql_qz_approval_apply_file_log = SQLiteDataBase.GetDataFromMysql("qz_approval_apply_file_log", "apply_id", resultDataApplyInfo.id);
            double applyFileFold = 0;
            for (int i = 0; i < mysql_qz_approval_apply_file_log.Rows.Count; i++)
            {
                if (mysql_qz_approval_apply_file_log.Rows[i].ItemArray[13].ToString() == "")
                {
                    applyFileFold = 0 + applyFileFold;
                }
                else
                {
                    applyFileFold = Convert.ToDouble(mysql_qz_approval_apply_file_log.Rows[i].ItemArray[13].ToString()) + applyFileFold;
                }
            }
            //23: FoldedAll 折A1
            resultDataApplyInfo.FoldedAll = applyFileFold;
            //24: FileAll 文件总数
            resultDataApplyInfo.FileAll = mysql_qz_approval_apply_file_log.Rows.Count;

            AddApplyInfoList(resultDataApplyInfo);
        }

        /// <summary>
        /// 读取服务器中的审批流程详细内容存入到线下文件
        /// </summary>
        /// <param name="applyInfoId">要读服务器审批流程内容的流程id
        public static void ReadApplyInfoHttpDatas(string applyInfoId)
        {
            string FileAll = "0";
            string FoldedAll = "0";
            ///加载本地缓存流程详情文件
            LoadApplyInfoDataFromJson(ref applyInfoTemp);

            ///查询流程审批详情所用的参数是流程id
            var param = new
            {
                id = applyInfoId,
            };
            ///审批详情:1:applyXh 序号/2：appName 流程类型名/3：userDpt 用户部门/4：remark 备注/5：userId 发起人Id/6:userName 用户名/ 7:nodeList 节点List/ 8:NAME 流程标题/9：processtype_id 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 /10：result 审批状态： 0进行中 1已通过 -1未通过/11：createTime 提交时间/12：resultTime 最后审批时间/13 :fileids 要是按文件夹发起的就传文件夹id，按项目就传项目id，文件就传文件id，购物车就不用传/14：days 流程天数/15 ：id 主键/16 ：proId 项目id/17：proName 项目名称/ 18：annex_id 打印份数/ 19:fileType 文件来源0 项目区 1归档区/20: guiId 归档使用/21:downUser 下载人主键/22:money 出版订单金额/ 23 :FoldedAll 折A1/ 24: FileAll 文件总数
            var resultDataApplyInfo = new ApplyInfoModel();

            ///服务器调取一个审批详情
            if (HttpPost(AppGlobalModel.ApplyInfo, param, ref resultDataApplyInfo))
            {
                QueryInfos = new QueryApprovalProjectStructure()
                {
                    fileType = resultDataApplyInfo.fileType, //文件来源0 项目区 1归档区
                    type = 3,    //发起类型 0购物车 1文件夹 2项目 3文件
                    fileIds = resultDataApplyInfo.fileids,  //流id列表  用  ，分割
                    parentId = "0",  //上级ID
                    applyId = applyInfoId, //审批详情主键Id
                    tab = "1"  // 是否获得未归档的文件 0未归档 1全部 默认传1，就出版和下载都传 0  剩下的都传1
                };
                ///返回流程文件数的变量；
                var resultFileAllData = new GetApprovalProjectStructureAllModel();
                ///返回流程文件数量
                if (HttpPost(AppGlobalModel.GetApprovalProjectStructureAll, QueryInfos, ref resultFileAllData))
                {
                    if (resultFileAllData != null)
                    {
                        ///文件数量
                        resultDataApplyInfo.FileAll = resultFileAllData.FileAll;
                        FileAll = (Convert.ToInt16(FileAll) + resultFileAllData.FileAll).ToString();
                        ///折A1数
                        resultDataApplyInfo.FoldedAll = resultFileAllData.FoldedAll;
                        FoldedAll = (Convert.ToDouble(FoldedAll) + resultFileAllData.FoldedAll).ToString();
                    }
                }
                foreach (var item in resultDataApplyInfo.nodeList)
                {
                    if (item != null)
                    {
                        ///用户
                        resultDataApplyInfo.applyUser = item.applyUser + " " + resultDataApplyInfo.applyUser;
                        ///审批意见
                        resultDataApplyInfo.resultRemark = item.resultRemark + " " + resultDataApplyInfo.resultRemark;
                        ///审批时间
                        resultDataApplyInfo.resultTime = item.resultTime;
                        ///审批部门
                        resultDataApplyInfo.userDept = resultDataApplyInfo.userDept;
                        ///判断审批状态翻译成中文
                        if (item.result.ToString().Equals("0"))
                        {
                            resultDataApplyInfo.resultName = "进行中";
                        }
                        else if (item.result.ToString().Equals("1"))
                        {
                            resultDataApplyInfo.resultName = "已通过";
                        }
                        else if (item.result.ToString().Equals("-1") || item.result.ToString().Equals("-2"))
                        {
                            resultDataApplyInfo.resultName = "未通过";
                        }
                        else
                        {
                            resultDataApplyInfo.resultName = "未知";
                        }
                        resultDataApplyInfo.result = item.result;
                    }
                }
                AddApplyInfoList(resultDataApplyInfo);
            }
        }

        /// <summary>
        /// 获取服务器中的MYSQL项目列表并写入本地文件中
        /// </summary>
        public static void Read_Mysql_ProjectListHttpDatas()
        {
            //0:id \ 1:name \ 2:user_id \ 3:identifier \ 4:unit \ 5:parent_id \ 6:ancestors \ 7:proType \ 8:type \ 9:file_id \ 10:status \ 11:project_id \ 12:varargs_id \ 13:frame_name \ 14:folded \ 15:page_all\ 16: file_type_id\ 17:create_time \ 18:update_time \ 19:govern \ 20: is_documentation\ 21:is_show \ 22: custom1\ 23:custom2
            //var qz_project_list = SQLiteDataBase.GetDataFromMysql("qz_project", "identifier", 0);
            var qz_project_list = SQLiteDataBase.GetDataFromMysql("qz_project", "type", 0);//只取项目类型的数据
            var projectItem = new ProjectResultModel();//项目临时变量
            for (int i = 0; i < qz_project_list.Rows.Count; i++)//循环循环所有项目
            {
                /// 返回项目信息/1:createTime 创建时间 /2:id 项目id /3:name 项目名称 /4:userId 创建用户id /5:parentId 上级ID /6:type 类型0项目，1阶段，2专业，3子项，4文件夹，5文件 /7:status 0正常，1停用，2未发布，3删除 /8:projectId 项目ID /9:varargsId 原始字典数据id /10:identifier 项目编号 /11:unit 建筑单位 /12:userName 创建人 /13:IsChecked 选择状态 /14:parentList 祖籍列表
                projectItem.createTime = qz_project_list.Rows[i].ItemArray[17].ToString();
                projectItem.name = qz_project_list.Rows[i].ItemArray[1].ToString();
                projectItem.id = qz_project_list.Rows[i].ItemArray[0].ToString();
                projectItem.userId = qz_project_list.Rows[i].ItemArray[2].ToString();
                projectItem.parentId = qz_project_list.Rows[i].ItemArray[5].ToString();
                projectItem.type = Convert.ToInt32(qz_project_list.Rows[i].ItemArray[7]);
                projectItem.status = Convert.ToInt32(qz_project_list.Rows[i].ItemArray[10]);
                projectItem.projectId = qz_project_list.Rows[i].ItemArray[12].ToString();
                projectItem.varargsId = qz_project_list.Rows[i].ItemArray[13].ToString();
                projectItem.identifier = qz_project_list.Rows[i].ItemArray[3].ToString();
                projectItem.unit = qz_project_list.Rows[i].ItemArray[4].ToString();
                //mysql里取用户信息
                var userNameData = SQLiteDataBase.SearchTableFromSQLite("qz_user", "id", $"{projectItem.userId}");
                if (userNameData.Rows.Count == 0)
                {
                    projectItem.userName = "";//项目创建人
                }
                else
                {
                    //添加项目创建人
                    if (userNameData.Rows[0].ItemArray[5].ToString() != null || userNameData.Rows[0].ItemArray[5].ToString() != "")
                    {
                        projectItem.userName = userNameData.Rows[0].ItemArray[5].ToString();//项目创建人
                    }
                    else
                    {
                        projectItem.userName = "";//项目创建人
                    }
                }
                //添加项目父级列表
                projectItem.parentList = qz_project_list.Rows[i].ItemArray[6].ToString();
                //添加项目写入本地文件
                AddProjectList(projectItem);
            }
        }

        /// <summary>
        /// 获取服务器中的项目属性信息并写入本地文件内
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columName">列名</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="fileNumber">文件数量</param>
        /// <param name="folded">折A1数量</param>
        public static void Read_Project_Attribute_Info_Http_Mysql_Datas(string tableName, string columName, string startTime, string endTime, ref int fileNumber, ref double folded)
        {
            fileNumber = 0;
            folded = 0;
            //项目统计List清理
            statisticsProjectListTemp.Clear();
            //加载项目统计List
            LoadProjectListDataFromJson(ref statisticsProjectListTemp);
            //初始化项目总表
            ProjectPropertieListS = new List<List<ProjectPropertyModel>>();
            //一个项目临时文件
            ProjectPropertieItem = new ProjectPropertyModel();
            //初始化部门列表
            //var deptInfoListTemp = new List<projectDeptModel>();
            //与本地数据库通信拿到部门列表
            DataTable sqliteDeptList = SQLiteDataBase.SearchTableFromSQLite("qz_dept");
            //新建一个存储项目分解后信息的list变量；
            var ProjectPropertieItemS = new List<ProjectPropertyModel>();
            //一个项目的出版区list
            var projectCbStrList = new List<projectCbStrList>();
            //项目出版区的string
            var projectCbStr = new projectCbStrList();
            //列名
            string columnName = "folded";
            //读取服务器内的Mysql数据:所有符合时间范围内的文件列表

            //static DataTable GetDataFromMysql(string tableName, string columnName, string projectId, string ISNOTNULL, string columnName2, string startDateTime, string endDateTime)
            DataTable qz_projectAllDataList = SQLiteDataBase.GetDataFromMysql($"{tableName}", $"{columnName}", $"{startTime}", $"{endTime}");

            //所有查询到的文件所在的项目的项目id
            DataTable qz_project_Id_ListResult = new DataTable();
            //赛选出所有项目id
            //出版区列表变量
            //DataTable qz_projectCBList = null;

            #region 获取所有项目文件并去重
            // 检查 qz_projectAllDataList 内有多少个项目
            //if (qz_projectAllDataList.Rows.Count > 0)
            //{
            //    // 创建一个用于存储新的 project_Id 结果变量
            //    qz_project_Id_ListResult = qz_projectAllDataList.Clone(); // 克隆原始数据表的结构,但不包括内容，

            //    // 创建一个用于存储唯一 project_id 的 HashSet
            //    var uniqueProjectIds = new HashSet<string>();

            //    // 遍历每一行数据
            //    foreach (DataRow row in qz_projectAllDataList.Rows)
            //    {
            //        // 获取当前行的 project_id 字段的值
            //        string projectId = row.Field<string>("project_id");

            //        // 检查 project_id 是否已经存在于 HashSet 中
            //        if (uniqueProjectIds.Add(projectId)) // 如果成功添加，说明 project_id 是唯一的
            //        {
            //            // 将当前行的数据用ImportRow方法导入到当前qz_project_Id_ListResult表中
            //            qz_project_Id_ListResult.ImportRow(row);

            //            //拿到出版区的项目id
            //            projectCbStr.projectId = projectId;

            //            //查询出版区列表
            //            qz_projectCBList = SQLiteDataBase.GetDataFromMysql("qz_project", "name", "出版区", "project_id", $"{projectCbStr.projectId}");

            //            //这个项目所有的出版区Id
            //            projectCbStr.ancestorsCbString = new List<string>();

            //            //循环这个项目的所有出版区id
            //            foreach (DataRow CBRowItem in qz_projectCBList.Rows)
            //            {
            //                //加入出版区id
            //                projectCbStr.ancestorsCbString.Add(CBRowItem["id"].ToString());
            //            }
            //            //填加项目id与所有出版区id
            //            projectCbStrList.Add(projectCbStr);
            //            //清理
            //            projectCbStr = new projectCbStrList();

            //        }
            //    }
            //}
            #endregion
            if (qz_projectAllDataList.Rows.Count > 0)
            {
                // 最简单的去重方法解释：
                // 步骤：
                // 1. 遍历 qz_projectAllDataList 的每一行，处理 name 列，去掉最后一个 '-' 及其后面的内容，作为比较用的“简化文件名”
                // 2. 按“简化文件名”+parent_id 分组，只保留每组的第一行
                // 3. 结果赋值给 qz_project_Id_ListResult

                qz_project_Id_ListResult = qz_projectAllDataList.AsEnumerable()
                    .Select(row =>
                    {
                        // 处理 name 列，去掉最后一个 '-' 及其后面的内容
                        string name = row.Field<string>("name");
                        int lastHyphenIndex = name.LastIndexOf('-');
                        if (lastHyphenIndex > 0)
                        {
                            name = name.Substring(0, lastHyphenIndex);
                        }
                        // 新增一列用于分组
                        var newRow = row.Table.NewRow();
                        newRow.ItemArray = row.ItemArray.Clone() as object[];
                        newRow["name"] = name;
                        return new { Row = row, CompareName = name, ParentId = row.Field<string>("parent_id") };
                    })
                    .GroupBy(x => new { x.CompareName, x.ParentId })
                    .Select(g => g.First().Row)
                    .CopyToDataTable();
                //.GroupBy(row => row.Field<string>("name")) //按 name 列分组
                //.Select(group => group.First()) //每组只取第一行
                //.CopyToDataTable(); //将结果转回 DataTable 类型
                //这样 result 就是去除了 name 重复行的新 DataTable
            }

            #region linQ方法，与上面的结果是一样的，只是语句简单
            ////检查 qz_projectAllDataList 是否包含任何行
            //if (qz_projectAllDataList.Rows.Count > 0)
            //{
            //    使用 LINQ 从 qz_projectAllDataList 生成一个新的 DataTable，去重 project_id 字段
            //   qz_project_Id_ListResult = qz_projectAllDataList.AsEnumerable() // 将 DataTable 转换为可枚举的集合
            //       .GroupBy(row => new { Name = row.Field<string>("project_id") }) // 按 project_id 字段进行分组
            //       .Select(group => group.First()) // 从每个分组中选择第一行（去重）
            //       .CopyToDataTable(); // 将结果复制回一个新的 DataTable 中
            //}
            #endregion
            #region 通过项目id拿到这个项目内的所有出版区id

            //foreach (DataRow project_Id_Item in qz_project_Id_ListResult.Rows)
            //{
            //    //所有项目id的列表
            //    projectCbStr.projectId = project_Id_Item["project_id"].ToString();
            //    //查询出版区列表
            //    qz_projectCBList = SQLiteDataBase.GetDataFromMysql("qz_project", "name", "出版区", "project_id", $"{projectCbStr.projectId}");
            //    //这个项目所有的出版区Id
            //    projectCbStr.ancestorsCbString = new List<string>();
            //    //循环这个项目的所有id
            //    foreach (DataRow CBRowItem in qz_projectCBList.Rows)
            //    {
            //        //加入出版区id
            //        projectCbStr.ancestorsCbString.Add(CBRowItem["id"].ToString());
            //    }
            //    //填加项目id与所有出版区id
            //    projectCbStrList.Add(projectCbStr);
            //    //清理
            //    projectCbStr = new projectCbStrList();
            //}

            #endregion

            int proListIndex = 0;
            //循环拿到的Mysql内的所有数据的每行
            foreach (DataRow qz_projectListRowItem in qz_project_Id_ListResult.Rows)
            {
                proListIndex++;
                //每行的ancestors列下的内容
                var ancestorsStr = qz_projectListRowItem["ancestors"].ToString();
                //分割ancestors内的字符串
                string[] ancestorsStrS = ancestorsStr.Split(',');
                //文件ancestors位数
                int ancestorsIdx = ancestorsStrS.Length;

                if (ancestorsStrS.Length <= 6 && ancestorsStrS[0] != "UG141218171001")
                {
                    //查找列队里是不是有这个部门，有就返回所在位置编号，没有返回-1；
                    var deptNum = ProjectPropertieListS.FindAll(o => o[0].Id == ancestorsStrS[0]);

                    if (deptNum.Count == 0)//没找到这个部门
                    {
                        WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                        fileNumber++;
                        folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);
                    }
                    else//已经有这个部门了
                    {
                        int deptItemNum = 0;
                        int proNum = -1;

                        foreach (var deptItem in deptNum)
                        {
                            //在这个部门下查找是不有这个项目，有返回所在位置，没有返回-1；
                            proNum = deptItem.FindIndex(o => o.Id == ancestorsStrS[1]);
                            if (proNum != -1)
                            {
                                break;
                            }
                            deptItemNum++;
                        }

                        if (proNum == -1)
                        {
                            WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                            fileNumber++;
                            folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);
                        }
                        else
                        {
                            //拿到阶段行
                            if (!deptNum[deptItemNum][9].Id.Contains(ancestorsStrS[2]))
                            {
                                var qz_project_StageName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[2]}").Rows[0];
                                deptNum[deptItemNum][9].Value = deptNum[deptItemNum][9].Value + "、" + qz_project_StageName["name"].ToString();
                                deptNum[deptItemNum][9].Id = deptNum[deptItemNum][9].Id + "、" + ancestorsStrS[2];
                            }

                            //拿到专业行
                            if (!deptNum[deptItemNum][10].Id.Contains(ancestorsStrS[3]))
                            {
                                var qz_project_MajroName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[3]}").Rows[0];
                                //去除专业后面的（）与里面的内容
                                string majroName = Regex.Replace(qz_project_MajroName["name"].ToString(), @"\(.*?\)", "");
                                if (!deptNum[deptItemNum][10].Value.Contains(majroName))
                                {
                                    deptNum[deptItemNum][10].Value = deptNum[deptItemNum][10].Value + "、" + majroName;
                                    deptNum[deptItemNum][10].Id = deptNum[deptItemNum][10].Id + "、" + ancestorsStrS[3];
                                }
                            }
                            //拿到子项行
                            if (!deptNum[deptItemNum][11].Id.Contains(ancestorsStrS[4]))
                            {
                                var qz_project_SubProjectName = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", $"{ancestorsStrS[4]}").Rows[0];
                                var subProName = qz_project_SubProjectName["name"].ToString();
                                if (!deptNum[deptItemNum][11].Value.Contains(subProName))
                                {
                                    deptNum[deptItemNum][11].Value = deptNum[deptItemNum][11].Value + "、" + subProName;
                                    deptNum[deptItemNum][11].Id = deptNum[deptItemNum][11].Id + "、" + ancestorsStrS[4];
                                }
                            }

                            deptNum[deptItemNum][12].Value = (Convert.ToInt32(deptNum[deptItemNum][12].Value) + 1).ToString();
                            deptNum[deptItemNum][13].Value = (Convert.ToDouble(deptNum[deptItemNum][13].Value) + Convert.ToDouble(qz_projectListRowItem["folded"])).ToString();

                            fileNumber++;
                            folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);
                        }
                    }
                }
                else if (ancestorsStrS[0] != "UG141218171001")//判断是不是“底图资料区”
                {
                    for (int j = ancestorsIdx - 2; j > 0; j--)
                    {
                        //用这个项目id找到这个项目的出版区id
                        var project_Id_Index = projectCbStrList.FindIndex(o => o.projectId == ancestorsStrS[1]);
                        if (project_Id_Index != -1)//如果能找到
                        {
                            //判断是不是出版区
                            if (projectCbStrList[project_Id_Index].ancestorsCbString.Contains(ancestorsStrS[j]))
                            {
                                //查找列队里是不是有这个部门，有就返回所在位置编号，没有返回-1；
                                var deptNum = ProjectPropertieListS.FindAll(o => o[0].Id == ancestorsStrS[0]);

                                if (deptNum.Count == 0)//没找到这个部门
                                {
                                    WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                    fileNumber++;
                                    folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);

                                    break;
                                }
                                else//已经有这个部门了
                                {
                                    int deptItemNum = 0;
                                    int proNum = -1;

                                    foreach (var deptItem in deptNum)
                                    {
                                        //在这个部门下查找是不有这个项目，有返回所在位置，没有返回-1；
                                        proNum = deptItem.FindIndex(o => o.Id == ancestorsStrS[1]);
                                        if (proNum != -1)
                                        {
                                            break;
                                        }
                                        deptItemNum++;
                                    }

                                    if (proNum == -1)
                                    {
                                        WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                        fileNumber++;
                                        folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);
                                        break;
                                    }
                                    else
                                    {
                                        //拿到阶段行
                                        if (!deptNum[deptItemNum][9].Id.Contains(ancestorsStrS[2]))
                                        {
                                            var qz_project_StageName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[2]}").Rows[0];
                                            deptNum[deptItemNum][9].Value = deptNum[deptItemNum][9].Value + "、" + qz_project_StageName["name"].ToString();
                                            deptNum[deptItemNum][9].Id = deptNum[deptItemNum][9].Id + "、" + ancestorsStrS[2];
                                        }

                                        //拿到专业行
                                        if (!deptNum[deptItemNum][10].Id.Contains(ancestorsStrS[3]))
                                        {
                                            var qz_project_MajroName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[3]}").Rows[0];
                                            //去除专业后面的（）与里面的内容
                                            string majroName = Regex.Replace(qz_project_MajroName["name"].ToString(), @"\(.*?\)", "");

                                            //if (majroName == "协同归档文件" && ancestorsStrS.Length > 6)
                                            //{
                                            //    //拿到专业行
                                            //    qz_project_MajroName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[5]}").Rows[0];
                                            //    //去除专业后面的（）与里面的内容
                                            //    majroName = Regex.Replace(qz_project_MajroName["name"].ToString(), @"\(.*?\)", "");
                                            //    if (!deptNum[deptItemNum][10].Value.Contains(majroName))
                                            //    {
                                            //        WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                            //        deptNum[deptItemNum][10].Value = majroName;
                                            //        deptNum[deptItemNum][10].Id = ancestorsStrS[5];
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    if (!deptNum[deptItemNum][10].Value.Contains(majroName))
                                            //    {
                                            //        WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                            //        deptNum[deptItemNum][10].Value = majroName;
                                            //        deptNum[deptItemNum][10].Id = ancestorsStrS[3];
                                            //    }
                                            //}

                                            if (!deptNum[deptItemNum][10].Value.Contains(majroName))
                                            {
                                                deptNum[deptItemNum][10].Value = deptNum[deptItemNum][10].Value + "、" + majroName;
                                                deptNum[deptItemNum][10].Id = deptNum[deptItemNum][10].Id + "、" + ancestorsStrS[3];
                                            }
                                        }

                                        //拿到子项行
                                        if (!deptNum[deptItemNum][11].Id.Contains(ancestorsStrS[4]))
                                        {
                                            var qz_project_SubProjectName = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", $"{ancestorsStrS[4]}").Rows[0];
                                            var subProName = qz_project_SubProjectName["name"].ToString();
                                            if (!deptNum[deptItemNum][11].Value.Contains(subProName))
                                            {
                                                deptNum[deptItemNum][11].Value = deptNum[deptItemNum][11].Value + "、" + subProName;
                                                deptNum[deptItemNum][11].Id = deptNum[deptItemNum][11].Id + "、" + ancestorsStrS[4];
                                            }
                                        }

                                        deptNum[deptItemNum][12].Value = (Convert.ToInt32(deptNum[deptItemNum][12].Value) + 1).ToString();

                                        deptNum[deptItemNum][13].Value = (Convert.ToDouble(deptNum[deptItemNum][13].Value) + Convert.ToDouble(qz_projectListRowItem["folded"])).ToString();

                                        fileNumber++;
                                        folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                ;
                //添加项目分解信息(ancestors)
                AddStatisticsProjectPropertieList(ProjectPropertieListS);
            }
        }

        /// <summary>
        /// 获取服务器中的项目属性信息并写入本地文件内
        /// </summary>
        /// <param ProjectResultModel="projectItemTemp">要查询的项目</param>
        public static void Read_One_Project_Attribute_Info_Http_Mysql_Datas(string tableName, string columName, string startTime, string endTime, string projectName, ref int fileNumber, ref double folded)
        {
            #region 加载本地、链接Mysql获得数据
            fileNumber = 0;
            folded = 0;
            //项目统计List清理
            statisticsProjectListTemp.Clear();
            //加载项目统计List
            LoadProjectListDataFromJson(ref statisticsProjectListTemp);
            //初始化项目总表
            //ProjectPropertieListS = new List<List<ProjectPropertyModel>>();
            //一个项目临时文件
            ProjectPropertieItem = new ProjectPropertyModel();
            //初始化部门列表
            var deptInfoListTemp = new List<projectDeptModel>();

            //与本地数据库通信拿到部门列表
            DataTable sqliteDeptList = SQLiteDataBase.SearchTableFromSQLite("qz_dept");
            //新建一个存储项目分解后信息的list变量；
            //var ProjectPropertieItemS = new List<ProjectPropertyModel>();
            //一个项目的出版区list
            var projectCbStrList = new List<projectCbStrList>();
            //项目出版区的string
            var projectCbStr = new projectCbStrList();
            //清理项目列表
            projectListTemp.Clear();
            //加载缓存到本地的项目列表
            LoadProjectListDataFromJson(ref projectListTemp);
            //在项目列表中查询出用户指定的项目关键字的项目
            List<ProjectResultModel> findProjectList = new List<ProjectResultModel>();
            //遍历项目列表中所有包含有项目关键字的项目返回给findProjectName变量
            findProjectList = projectListTemp.FindAll(o => o.name.Contains(projectName));
            // 如果findProjectName变量中包含projectName，则将findProjectName变量添加到searchProject列表中
            if (findProjectList.Count == 0)
            {
                MessageBox.Show("没找到包括" + "(" + $"{projectName}" + ")" + "关键字的项目!"); return;
            }
            #endregion
            foreach (var findProjectItem in findProjectList)
            {
                //读取服务内的Mysql数据:所有符合时间范围内的\ 指定的projectId的文件列表
                DataTable qz_projectAllDataList = SQLiteDataBase.GetDataFromMysql($"{tableName}", "folded", findProjectItem.id, "0", "create_time", $"{startTime}", $"{endTime}","0");

                //获取符合时间范围内的\ 指定的projectId的文件列表
                DataTable qz_project_Id_ListResult = new DataTable();
                //赛选出所有重复的项目id
                if (qz_projectAllDataList.Rows.Count > 0)
                {
                    // 最简单的去重方法解释：
                    // 步骤：
                    // 1. 遍历 qz_projectAllDataList 的每一行，处理 name 列，去掉最后一个 '-' 及其后面的内容，作为比较用的“简化文件名”
                    // 2. 按“简化文件名”+parent_id 分组，只保留每组的第一行
                    // 3. 结果赋值给 qz_project_Id_ListResult

                    qz_project_Id_ListResult = qz_projectAllDataList.AsEnumerable()
                        .Select(row =>
                        {
                            // 处理 name 列，去掉最后一个 '-' 及其后面的内容
                            string name = row.Field<string>("name");
                            int lastHyphenIndex = name.LastIndexOf('-');
                            if (lastHyphenIndex > 0)
                            {
                                name = name.Substring(0, lastHyphenIndex);
                            }
                            // 新增一列用于分组
                            var newRow = row.Table.NewRow();
                            newRow.ItemArray = row.ItemArray.Clone() as object[];
                            newRow["name"] = name;
                            return new { Row = row, CompareName = name, ParentId = row.Field<string>("parent_id") };
                        })
                        .GroupBy(x => new { x.CompareName, x.ParentId })
                        .Select(g => g.First().Row)
                        .CopyToDataTable();
                    //.GroupBy(row => row.Field<string>("name")) //按 name 列分组
                    //.Select(group => group.First()) //每组只取第一行
                    //.CopyToDataTable(); //将结果转回 DataTable 类型
                    //这样 result 就是去除了 name 重复行的新 DataTable
                }
                //出版区列表变量
                DataTable qz_projectCBList = new DataTable();
                //循环去重后的项目id列表
                foreach (DataRow project_Id_Item in qz_project_Id_ListResult.Rows)
                {
                    if (projectCbStrList.All(o => o.projectId != (project_Id_Item["project_id"].ToString())))
                    {
                        //拿到一个项目id
                        projectCbStr.projectId = project_Id_Item["project_id"].ToString();
                        //查询出版区列表
                        qz_projectCBList = SQLiteDataBase.GetDataFromMysql("qz_project", "name", "出版区", "project_id", $"{projectCbStr.projectId}");
                        //这个项目所有的出版区Id
                        projectCbStr.ancestorsCbString = new List<string>();
                        //循环这个项目每一行，拿到所有的出版区Id
                        foreach (DataRow CBRowItem in qz_projectCBList.Rows)
                        {
                            //加入出版区id
                            projectCbStr.ancestorsCbString.Add(CBRowItem["id"].ToString());
                        }
                        //填加项目id与所有出版区id
                        projectCbStrList.Add(projectCbStr);
                        //清理
                        projectCbStr = new projectCbStrList();
                    }

                }
                #region 初始化变量
                //一个部门变量 
                var deptInfoItemTemp = new projectDeptModel();
                //初始化部门容器存入项目
                deptInfoItemTemp.projectInfoList = new List<ProjectInfoListModel>();
                //一个项目信息表
                var projectInfoItem = new ProjectInfoListModel();
                //初始化项目容器存入阶段；
                projectInfoItem.projectStageList = new List<ProjectStageModel>();
                //初始化一个阶段变量；
                var projectStageTemp = new ProjectStageModel();
                //初始化阶段容器存入专业；
                projectStageTemp.projectMajroList = new List<StageMajroModel>();
                //建立一个专业变量
                var majroItem = new StageMajroModel();
                //初始化专业容器存入子项
                majroItem.subProjectList = new List<subProjectListModel>();
                //初始化一个子项目变量
                var subProjectItem = new subProjectListModel();
                #endregion

                //循环每一行
                foreach (DataRow row in qz_project_Id_ListResult.Rows)
                {
                    //每行的ancestors列下的内容
                    var ancestorsStr = row["ancestors"].ToString();

                    //分割ancestors内的字符串
                    string[] ancestorsStrS = ancestorsStr.Split(',');

                    //读取本表这一行数据
                    WriteInProjectList(row, ancestorsStrS);

                    #region 部门
                    //判断是不是在部门列表里以存在,找到了，返回所在的位置
                    var deptItemIndex = deptInfoListTemp.FindIndex(o => o.projectDeptId == ancestorsStrS[0]);
                    //判断部门列表是不是为0或没找到所在部门-1
                    if (deptInfoListTemp.Count == 0 || deptItemIndex == -1)
                    {
                        //临时一个部门变量 
                        deptInfoItemTemp = new projectDeptModel();
                        //赋值第1位是部门id
                        deptInfoItemTemp.projectDeptId = ancestorsStrS[0];
                        // 遍历DataTable中的每一行
                        foreach (DataRow sqliteDeptListRow in sqliteDeptList.Rows)
                        {
                            // 检查dept_id列的值与取到的部门id相同
                            if (sqliteDeptListRow["dept_id"].ToString() == ancestorsStrS[0])
                            {
                                // 如果找到匹配的dept_id，获取dept_name列的值，拿到部门名称
                                deptInfoItemTemp.projectDeptName = sqliteDeptListRow["dept_name"].ToString();
                                // 找到后可以跳出循环
                                break;
                            }
                        }
                        //部门列表加入一个部门
                        deptInfoListTemp.Add(deptInfoItemTemp);
                        deptItemIndex = deptInfoListTemp.FindIndex(o => o.projectDeptId == ancestorsStrS[0]);
                    }
                    #endregion

                    #region 项目

                    //判读这个部门集合里是不是有这个项目
                    if (deptInfoListTemp[deptItemIndex].projectDeptName == null || deptInfoListTemp[deptItemIndex].projectInfoList == null || !deptInfoListTemp[deptItemIndex].projectInfoList.Any(o => o.projectId == ancestorsStrS[1]))
                    {
                        //初始化一个项目的信息
                        projectInfoItem = new ProjectInfoListModel();
                        // 赋值项目id
                        projectInfoItem.projectId = ancestorsStrS[1];
                        //赋值项目编号
                        projectInfoItem.projectNo = ProjectPropertieItemS[1].Value;
                        //赋值项目名称
                        projectInfoItem.projectName = ProjectPropertieItemS[2].Value;
                        //赋值建设单位
                        projectInfoItem.constructUnit = ProjectPropertieItemS[3].Value;
                        //赋值项目类型
                        projectInfoItem.projectType = ProjectPropertieItemS[4].Value;
                        //创建人
                        projectInfoItem.founder = ProjectPropertieItemS[5].Value;
                        //项目经理
                        projectInfoItem.projectManager = ProjectPropertieItemS[6].Value;
                        //创建时间
                        projectInfoItem.createTime = ProjectPropertieItemS[7].Value;
                        //项目状态
                        projectInfoItem.projectStatus = ProjectPropertieItemS[8].Value;
                        if (deptInfoItemTemp.projectInfoList == null)//如果部门内没有这个项目
                        {
                            //部门下项目列表初始化后再加入上面项目
                            deptInfoListTemp[deptItemIndex].projectInfoList = new List<ProjectInfoListModel>() { projectInfoItem };
                        }
                        else//如果有这个项目
                        {
                            //部门下项目列表内加入上面项目
                            deptInfoListTemp[deptItemIndex].projectInfoList.Add(projectInfoItem);
                        }
                    }
                    #endregion

                    #region 阶段
                    //在部门内查找是不是有这个项目，没有为-1
                    var projectItemIndex = deptInfoListTemp[deptItemIndex].projectInfoList.FindIndex(o => o.projectId == ancestorsStrS[1]);

                    //判断是不是在这个项目内有这个阶段

                    if (projectItemIndex == -1 || deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList == null || !deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList.Any(o => o.projectStageId == ancestorsStrS[2]))
                    {
                        projectStageTemp = new ProjectStageModel();
                        //赋值阶段id
                        projectStageTemp.projectStageId = ancestorsStrS[2];

                        //取回Mysql内qz_project表内的数据
                        DataTable stageDataList = SQLiteDataBase.GetDataFromMysql("qz_project", "id", ancestorsStrS[2]);
                        DataRow stageDataRow = stageDataList.Rows[0];
                        //赋值阶段名称
                        projectStageTemp.projectStageName = stageDataRow["name"].ToString();
                        if (deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList == null)
                        {
                            deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList = new List<ProjectStageModel>() { projectStageTemp };
                        }
                        else
                        {
                            //项目下加入阶段
                            deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList.Add(projectStageTemp);
                        }
                    }
                    #endregion

                    #region 专业
                    //拿到阶段的Index
                    var stageItemIndex = deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList.FindIndex(o => o.projectStageId == ancestorsStrS[2]);

                    string majroId = ancestorsStrS[5];
                    if (majroId == "") majroId = ancestorsStrS[3];
                    //取回Mysql内qz_project表内的专业名数据
                    DataTable majroDataList = SQLiteDataBase.GetDataFromMysql("qz_project", "id", majroId);
                    DataRow majroDataRow = majroDataList.Rows[0];
                    //去除专业后面的（）与里面的内容
                    string majroName = Regex.Replace(majroDataRow["name"].ToString(), @"\(.*?\)", "");

                    //判断是不是有这个专业
                    if (deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList == null || !deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList.Any(o => o.projectMajroName == majroName))
                    {
                        //初始化这个专业
                        majroItem = new StageMajroModel();
                        majroItem.projectMajroId = majroId;
                        majroItem.projectMajroName = majroName;
                        if (deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList == null)
                        {
                            deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList = new List<StageMajroModel> { majroItem };
                        }
                        else
                        {
                            //阶段下加入专业
                            deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList.Add(majroItem);
                        }
                    }
                    #endregion

                    #region 子项
                    //拿到专业的index
                    var majroItemIndex = deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList.FindIndex(o => o.projectMajroName == majroName);

                    //取回Mysql内qz_project表内子项的数据
                    DataTable subProDataList = SQLiteDataBase.GetDataFromMysql("qz_project", "id", ancestorsStrS[4]);
                    DataRow subProDataRow = subProDataList.Rows[0];
                    string tempSubProjectName = subProDataRow["name"].ToString();
                    //查找是不是有这个子项，没有值为-1


                    if (deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList == null || !deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList.Any(o => o.subProjectName == tempSubProjectName))
                    {
                        //初始化子项目
                        subProjectItem = new subProjectListModel();
                        //赋值子项目id
                        subProjectItem.subProjectId = ancestorsStrS[4];
                        //赋值子项目名称
                        subProjectItem.subProjectName = tempSubProjectName;
                        //赋值文件数量
                        subProjectItem.fileNumber = subProjectItem.fileNumber + 1;
                        //赋值A1文件数量
                        subProjectItem.A1SizeNumber = subProjectItem.A1SizeNumber + Convert.ToDouble(row["folded"]);
                        //专业下创建子项
                        deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList = new List<subProjectListModel> { subProjectItem };
                    }
                    else
                    {
                        //专业下加入子项目
                        deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList[fileNumber].fileNumber++;
                        deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList[fileNumber].A1SizeNumber = deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList[fileNumber].A1SizeNumber + Convert.ToDouble(row["folded"]);

                        //deptInfoListTemp[deptItemIndex].projectInfoList[projectItemIndex].projectStageList[stageItemIndex].projectMajroList[majroItemIndex].subProjectList.Add(subProjectItem);
                    }

                    #endregion

                    //fileNumber = fileNumber + 1;
                    //folded = folded + Convert.ToDouble(row["folded"]);
                    // 将更新后的数据写入JSON文件  
                    File.WriteAllText(statisticsUserInfoListJsonFile, JsonConvert.SerializeObject(deptInfoListTemp, Formatting.Indented));
                    //File.WriteAllText(statisticsProjectListJsonFile, JsonConvert.SerializeObject(ProjectPropertieListS, Formatting.Indented));
                    //AddStatisticsProjectPropertieList(ProjectPropertieListS);
                }
            }
        }

        /// <summary>
        /// 读取指定的项目名的属性信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columName">列名</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="fileNumber">文件数量</param>
        /// <param name="folded">折合数量</param>
        public static void Read_Project_Attribute_Info_Http_Mysql_Datas(string tableName, string columName, string startTime, string endTime, string projectName, string projectId, ref int fileNumber, ref double folded)
        {
            fileNumber = 0;
            folded = 0;
            //项目统计List清理
            statisticsProjectListTemp.Clear();
            //加载项目统计List
            LoadProjectListDataFromJson(ref statisticsProjectListTemp);
            //初始化项目总表
            ProjectPropertieListS = new List<List<ProjectPropertyModel>>();
            //一个项目临时文件
            ProjectPropertieItem = new ProjectPropertyModel();
            //初始化部门列表
            var deptInfoListTemp = new List<projectDeptModel>();
            //与本地数据库通信拿到部门列表
            DataTable sqliteDeptList = SQLiteDataBase.SearchTableFromSQLite("qz_dept");
            //新建一个存储项目分解后信息的list变量；
            var ProjectPropertieItemS = new List<ProjectPropertyModel>();
            //一个项目的出版区list
            var projectCbStrList = new List<projectCbStrList>();
            //项目出版区的string
            var projectCbStr = new projectCbStrList();
            //清理项目列表
            projectListTemp.Clear();
            //加载缓存到本地的项目列表
            LoadProjectListDataFromJson(ref projectListTemp);
            //在项目列表中查询出用户指定的项目关键字的项目
            List<ProjectResultModel> searchProject = new List<ProjectResultModel>();
        
            var proName = projectListTemp.Find(o => o.name.Contains(projectName));
            if (proName.name.Contains(projectName))
            {
                searchProject.Add(proName);
            }

            if (searchProject.Count == 0)
            {
                MessageBox.Show("没找到包括" + "(" + $"{projectName}" + ")" + "关键字的项目!"); return;
            }
            foreach (var itemSPID in searchProject)
            {
                //读取服务内的Mysql数据:所有符合时间范围内的\ 指定的projectId的文件列表
                DataTable qz_projectAllDataList = SQLiteDataBase.GetDataFromMysql($"{tableName}", "folded", itemSPID.projectId, "0", "create_time", $"{startTime}", $"{endTime}","0");
                               
                //所有查询到的文件所在的项目的项目id
                DataTable qz_project_Id_ListResult = new DataTable();
          
                if (qz_projectAllDataList.Rows.Count > 0)
                {
                    // 最简单的去重方法解释：
                    // 步骤：
                    // 1. 遍历 qz_projectAllDataList 的每一行，处理 name 列，去掉最后一个 '-' 及其后面的内容，作为比较用的“简化文件名”
                    // 2. 按“简化文件名”+parent_id 分组，只保留每组的第一行
                    // 3. 结果赋值给 qz_project_Id_ListResult

                    qz_project_Id_ListResult = qz_projectAllDataList.AsEnumerable()
                        .Select(row =>
                        {
                            // 处理 name 列，去掉最后一个 '-' 及其后面的内容
                            string name = row.Field<string>("name");
                            int lastHyphenIndex = name.LastIndexOf('-');
                            if (lastHyphenIndex > 0)
                            {
                                name = name.Substring(0, lastHyphenIndex);
                            }
                            // 新增一列用于分组
                            var newRow = row.Table.NewRow();
                            newRow.ItemArray = row.ItemArray.Clone() as object[];
                            newRow["name"] = name;
                            return new { Row = row, CompareName = name, ParentId = row.Field<string>("parent_id") };
                        })
                        .GroupBy(x => new { x.CompareName, x.ParentId })
                        .Select(g => g.First().Row)
                        .CopyToDataTable();
               
                }
                //出版区列表变量
                DataTable qz_projectCBList = null;
                foreach (DataRow project_Id_Item in qz_project_Id_ListResult.Rows)
                {
                    //if(searchProject.FindAll(o=>o.projectId == project_Id_Item["project_id"].ToString()))
                    //所有项目id的列表
                    projectCbStr.projectId = project_Id_Item["project_id"].ToString();
                    //查询出版区列表
                    qz_projectCBList = SQLiteDataBase.GetDataFromMysql("qz_project", "name", "出版区", "project_id", $"{projectCbStr.projectId}");
                    //这个项目所有的出版区Id
                    projectCbStr.ancestorsCbString = new List<string>();
                    //循环这个项目的所有id
                    foreach (DataRow CBRowItem in qz_projectCBList.Rows)
                    {
                        //加入出版区id
                        projectCbStr.ancestorsCbString.Add(CBRowItem["id"].ToString());
                    }
                    //填加项目id与所有出版区id
                    projectCbStrList.Add(projectCbStr);
                    //清理
                    projectCbStr = new projectCbStrList();
                }
                //项目列表索引
                int proListIndex = 0;
                //循环项目列表的每行
                foreach (DataRow qz_projectListRowItem in qz_projectAllDataList.Rows)
                {
                    ///项目列表的每行
                    proListIndex++;
                    //每行的ancestors列下的内容
                    var ancestorsStr = qz_projectListRowItem["ancestors"].ToString();
                    //分割ancestors内的字符串
                    string[] ancestorsStrS = ancestorsStr.Split(',');
                    //文件ancestors位数
                    int ancestorsIdx = ancestorsStrS.Length;

                    if (ancestorsStrS.Length <= 6 && ancestorsStrS[0] != "UG141218171001")
                    {
                        //查找列队里是不是有这个部门，有就返回所在位置编号，没有返回-1；
                        var deptNum = ProjectPropertieListS.FindAll(o => o[0].Id == ancestorsStrS[0]);
                        //没找到这个部门
                        if (deptNum.Count == 0)
                        {
                            WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                            fileNumber++;
                            folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);
                        }
                        else//已经有这个部门了
                        {
                            int deptItemNum = 0;
                            int proNum = -1;

                            foreach (var deptItem in deptNum)
                            {
                                //在这个部门下查找是不有这个项目，有返回所在位置，没有返回-1；
                                proNum = deptItem.FindIndex(o => o.Id == ancestorsStrS[1]);
                                if (proNum != -1)
                                {
                                    break;
                                }
                                deptItemNum++;
                            }

                            if (proNum == -1)
                            {
                                WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                fileNumber++;
                                folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);
                            }
                            else
                            {
                                //拿到阶段行
                                if (!deptNum[deptItemNum][9].Id.Contains(ancestorsStrS[2]))
                                {
                                    var qz_project_StageName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[2]}").Rows[0];
                                    deptNum[deptItemNum][9].Value = deptNum[deptItemNum][9].Value + "、" + qz_project_StageName["name"].ToString();
                                    deptNum[deptItemNum][9].Id = deptNum[deptItemNum][9].Id + "、" + ancestorsStrS[2];
                                }

                                //拿到专业行
                                if (!deptNum[deptItemNum][10].Id.Contains(ancestorsStrS[3]))
                                {
                                    var qz_project_MajroName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[3]}").Rows[0];
                                    //去除专业后面的（）与里面的内容
                                    string majroName = Regex.Replace(qz_project_MajroName["name"].ToString(), @"\(.*?\)", "");
                                    if (!deptNum[deptItemNum][10].Value.Contains(majroName))
                                    {
                                        deptNum[deptItemNum][10].Value = deptNum[deptItemNum][10].Value + "、" + majroName;
                                        deptNum[deptItemNum][10].Id = deptNum[deptItemNum][10].Id + "、" + ancestorsStrS[3];
                                    }
                                }
                                //拿到子项行
                                if (!deptNum[deptItemNum][11].Id.Contains(ancestorsStrS[4]))
                                {
                                    var qz_project_SubProjectName = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", $"{ancestorsStrS[4]}").Rows[0];
                                    var subProName = qz_project_SubProjectName["name"].ToString();
                                    if (!deptNum[deptItemNum][11].Value.Contains(subProName))
                                    {
                                        deptNum[deptItemNum][11].Value = deptNum[deptItemNum][11].Value + "、" + subProName;
                                        deptNum[deptItemNum][11].Id = deptNum[deptItemNum][11].Id + "、" + ancestorsStrS[4];
                                    }
                                }

                                deptNum[deptItemNum][12].Value = (Convert.ToInt32(deptNum[deptItemNum][12].Value) + 1).ToString();
                                deptNum[deptItemNum][13].Value = (Convert.ToDouble(deptNum[deptItemNum][13].Value) + Convert.ToDouble(qz_projectListRowItem["folded"])).ToString();

                                fileNumber++;
                                folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);
                            }
                        }
                    }
                    else if (ancestorsStrS[0] != "UG141218171001")//判断是不是“底图资料区”
                    {
                        for (int j = ancestorsIdx - 2; j > 0; j--)
                        {
                            //用这个项目id找到这个项目的出版区id
                            var project_Id_Index = projectCbStrList.FindIndex(o => o.projectId == ancestorsStrS[1]);
                            if (project_Id_Index != -1)//如果能找到
                            {
                                //判断是不是出版区
                                if (projectCbStrList[project_Id_Index].ancestorsCbString.Contains(ancestorsStrS[j]))
                                {
                                    //查找列队里是不是有这个部门，有就返回所在位置编号，没有返回 1；
                                    var deptNum = ProjectPropertieListS.FindAll(o => o[0].Id == ancestorsStrS[0]);

                                    if (deptNum.Count == 0)//没找到这个部门
                                    {
                                        WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                        fileNumber++;
                                        folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);

                                        break;
                                    }
                                    else//已经有这个部门了
                                    {
                                        int deptItemNum = 0;
                                        int proNum = -1;

                                        foreach (var deptItem in deptNum)
                                        {
                                            //在这个部门下查找是不有这个项目，有返回所在位置，没有返回-1；
                                            proNum = deptItem.FindIndex(o => o.Id == ancestorsStrS[1]);
                                            if (proNum != -1)
                                            {
                                                break;
                                            }
                                            deptItemNum++;
                                        }

                                        if (proNum == -1)
                                        {
                                            WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                            fileNumber++;
                                            folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);
                                            break;
                                        }
                                        else
                                        {
                                            //拿到阶段行
                                            if (!deptNum[deptItemNum][9].Id.Contains(ancestorsStrS[2]))
                                            {
                                                var qz_project_StageName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[2]}").Rows[0];
                                                deptNum[deptItemNum][9].Value = deptNum[deptItemNum][9].Value + "、" + qz_project_StageName["name"].ToString();
                                                deptNum[deptItemNum][9].Id = deptNum[deptItemNum][9].Id + "、" + ancestorsStrS[2];
                                            }

                                            //拿到专业行
                                            if (!deptNum[deptItemNum][10].Id.Contains(ancestorsStrS[3]))
                                            {
                                                var qz_project_MajroName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[3]}").Rows[0];
                                                //去除专业后面的（）与里面的内容
                                                string majroName = Regex.Replace(qz_project_MajroName["name"].ToString(), @"\(.*?\)", "");
                                                if (majroName == "协同归档文件" && ancestorsStrS.Length > 6)
                                                {
                                                    //拿到专业行
                                                    qz_project_MajroName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[5]}").Rows[0];
                                                    //去除专业后面的（）与里面的内容
                                                    majroName = Regex.Replace(qz_project_MajroName["name"].ToString(), @"\(.*?\)", "");
                                                    if (!deptNum[deptItemNum][10].Value.Contains(majroName))
                                                    {
                                                        WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                                        deptNum[deptItemNum][10].Value = majroName;
                                                        deptNum[deptItemNum][10].Id = ancestorsStrS[5];
                                                    }
                                                }
                                                else
                                                {
                                                    if (!deptNum[deptItemNum][10].Value.Contains(majroName))
                                                    {
                                                        WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                                        deptNum[deptItemNum][10].Value = majroName;
                                                        deptNum[deptItemNum][10].Id = ancestorsStrS[3];
                                                    }
                                                }
                                            }

                                            //拿到子项行
                                            if (!deptNum[deptItemNum][11].Id.Contains(ancestorsStrS[4]))
                                            {
                                                var qz_project_SubProjectName = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", $"{ancestorsStrS[4]}").Rows[0];
                                                var subProName = qz_project_SubProjectName["name"].ToString();
                                                if (!deptNum[deptItemNum][11].Value.Contains(subProName))
                                                {
                                                    deptNum[deptItemNum][11].Value = deptNum[deptItemNum][11].Value + "、" + subProName;
                                                    deptNum[deptItemNum][11].Id = deptNum[deptItemNum][11].Id + "、" + ancestorsStrS[4];
                                                }
                                            }

                                            deptNum[deptItemNum][12].Value = (Convert.ToInt32(deptNum[deptItemNum][12].Value) + 1).ToString();

                                            deptNum[deptItemNum][13].Value = (Convert.ToDouble(deptNum[deptItemNum][13].Value) + Convert.ToDouble(qz_projectListRowItem["folded"])).ToString();

                                            fileNumber++;
                                            folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);

                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    AddStatisticsProjectPropertieList(ProjectPropertieListS);
                }
            }
        }

        /// <summary>
        /// 读取指定的项目名的属性信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columName">列名</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="fileNumber">文件数量</param>
        /// <param name="folded">折合数量</param>
        public static void Read_Project_Attribute_Info_Http_Mysql_Datas(string tableName, string columName, string startTime, string endTime, string projectName, string projectId,string isFileType , ref int fileNumber, ref double folded)
        {
            fileNumber = 0;
            folded = 0;
            //项目统计List清理
            statisticsProjectListTemp.Clear();
            //加载项目统计List
            LoadProjectListDataFromJson(ref statisticsProjectListTemp);
            //初始化项目总表
            ProjectPropertieListS = new List<List<ProjectPropertyModel>>();
            //一个项目临时文件
            ProjectPropertieItem = new ProjectPropertyModel();
            //初始化部门列表
            var deptInfoListTemp = new List<projectDeptModel>();
            //与本地数据库通信拿到部门列表
            DataTable sqliteDeptList = SQLiteDataBase.SearchTableFromSQLite("qz_dept");
            //新建一个存储项目分解后信息的list变量；
            var ProjectPropertieItemS = new List<ProjectPropertyModel>();
            //一个项目的出版区list
            var projectCbStrList = new List<projectCbStrList>();
            //项目出版区的string
            var projectCbStr = new projectCbStrList();
            //清理项目列表
            projectListTemp.Clear();
            //加载缓存到本地的项目列表
            LoadProjectListDataFromJson(ref projectListTemp);
            //在项目列表中查询出用户指定的项目关键字的项目
            List<ProjectResultModel> searchProject = new List<ProjectResultModel>();

            var proName = projectListTemp.Find(o => o.name.Contains(projectName));
            if (proName.name.Contains(projectName))
            {
                searchProject.Add(proName);
            }

            if (searchProject.Count == 0)
            {
                MessageBox.Show("没找到包括" + "(" + $"{projectName}" + ")" + "关键字的项目!"); return;
            }
            foreach (var itemSPID in searchProject)
            {
                var columnName = "name";
                var columnName2 = "parent_id";
                //static DataTable GetDataFromMysql(string tableName, string columnName, string columnName2, string projectId, string isFileType, string startDateTime, string endDateTime)
                // mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE project_id = '{projectId}' AND '{columnName}' LIKE '%.{isFileType}' AND {columnName2} BETWEEN '{startDateTime}' AND '{endDateTime}' ";
                //读取服务内的Mysql数据:所有符合时间范围内的\ 指定的projectId的文件列表
                DataTable qz_projectAllDataList = SQLiteDataBase.GetDataFromMysql($"{tableName}", $"{columnName}", $"{columnName2}", $"{projectId}", "pdf", $"{startTime}", $"{endTime}");

                //所有查询到的文件所在的项目的项目id
                DataTable qz_project_Id_ListResult = new DataTable();

                if (qz_projectAllDataList.Rows.Count > 0)
                {
                    // 最简单的去重方法解释：
                    // 步骤：
                    // 1. 遍历 qz_projectAllDataList 的每一行，处理 name 列，去掉最后一个 '-' 及其后面的内容，作为比较用的“简化文件名”
                    // 2. 按“简化文件名”+parent_id 分组，只保留每组的第一行
                    // 3. 结果赋值给 qz_project_Id_ListResult

                    qz_project_Id_ListResult = qz_projectAllDataList.AsEnumerable()
                        .Select(row =>
                        {
                            // 处理 name 列，去掉最后一个 '-' 及其后面的内容
                            string name = row.Field<string>("name");
                            int lastHyphenIndex = name.LastIndexOf('-');
                            if (lastHyphenIndex > 0)
                            {
                                name = name.Substring(0, lastHyphenIndex);
                            }
                            // 新增一列用于分组
                            var newRow = row.Table.NewRow();
                            newRow.ItemArray = row.ItemArray.Clone() as object[];
                            newRow["name"] = name;
                            return new { Row = row, CompareName = name, ParentId = row.Field<string>("parent_id") };
                        })
                        .GroupBy(x => new { x.CompareName, x.ParentId })
                        .Select(g => g.First().Row)
                        .CopyToDataTable();

                }
                //出版区列表变量
                DataTable qz_projectCBList = null;
                foreach (DataRow project_Id_Item in qz_project_Id_ListResult.Rows)
                {
                    //if(searchProject.FindAll(o=>o.projectId == project_Id_Item["project_id"].ToString()))
                    //所有项目id的列表
                    projectCbStr.projectId = project_Id_Item["project_id"].ToString();
                    //查询出版区列表
                    qz_projectCBList = SQLiteDataBase.GetDataFromMysql("qz_project", "name", "出版区", "project_id", $"{projectCbStr.projectId}");
                    //这个项目所有的出版区Id
                    projectCbStr.ancestorsCbString = new List<string>();
                    //循环这个项目的所有id
                    foreach (DataRow CBRowItem in qz_projectCBList.Rows)
                    {
                        //加入出版区id
                        projectCbStr.ancestorsCbString.Add(CBRowItem["id"].ToString());
                    }
                    //填加项目id与所有出版区id
                    projectCbStrList.Add(projectCbStr);
                    //清理
                    projectCbStr = new projectCbStrList();
                }
                //项目列表索引
                int proListIndex = 0;
                //循环项目列表的每行
                foreach (DataRow qz_projectListRowItem in qz_projectAllDataList.Rows)
                {
                    ///项目列表的每行
                    proListIndex++;
                    //每行的ancestors列下的内容
                    var ancestorsStr = qz_projectListRowItem["ancestors"].ToString();
                    //分割ancestors内的字符串
                    string[] ancestorsStrS = ancestorsStr.Split(',');
                    //文件ancestors位数
                    int ancestorsIdx = ancestorsStrS.Length;

                    if (ancestorsStrS.Length <= 6 && ancestorsStrS[0] != "UG141218171001")
                    {
                        //查找列队里是不是有这个部门，有就返回所在位置编号，没有返回-1；
                        var deptNum = ProjectPropertieListS.FindAll(o => o[0].Id == ancestorsStrS[0]);
                        //没找到这个部门
                        if (deptNum.Count == 0)
                        {
                            WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                            fileNumber++;
                            folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);
                        }
                        else//已经有这个部门了
                        {
                            int deptItemNum = 0;
                            int proNum = -1;

                            foreach (var deptItem in deptNum)
                            {
                                //在这个部门下查找是不有这个项目，有返回所在位置，没有返回-1；
                                proNum = deptItem.FindIndex(o => o.Id == ancestorsStrS[1]);
                                if (proNum != -1)
                                {
                                    break;
                                }
                                deptItemNum++;
                            }

                            if (proNum == -1)
                            {
                                WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                fileNumber++;
                                folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);
                            }
                            else
                            {
                                //拿到阶段行
                                if (!deptNum[deptItemNum][9].Id.Contains(ancestorsStrS[2]))
                                {
                                    var qz_project_StageName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[2]}").Rows[0];
                                    deptNum[deptItemNum][9].Value = deptNum[deptItemNum][9].Value + "、" + qz_project_StageName["name"].ToString();
                                    deptNum[deptItemNum][9].Id = deptNum[deptItemNum][9].Id + "、" + ancestorsStrS[2];
                                }

                                //拿到专业行
                                if (!deptNum[deptItemNum][10].Id.Contains(ancestorsStrS[3]))
                                {
                                    var qz_project_MajroName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[3]}").Rows[0];
                                    //去除专业后面的（）与里面的内容
                                    string majroName = Regex.Replace(qz_project_MajroName["name"].ToString(), @"\(.*?\)", "");
                                    if (!deptNum[deptItemNum][10].Value.Contains(majroName))
                                    {
                                        deptNum[deptItemNum][10].Value = deptNum[deptItemNum][10].Value + "、" + majroName;
                                        deptNum[deptItemNum][10].Id = deptNum[deptItemNum][10].Id + "、" + ancestorsStrS[3];
                                    }
                                }
                                //拿到子项行
                                if (!deptNum[deptItemNum][11].Id.Contains(ancestorsStrS[4]))
                                {
                                    var qz_project_SubProjectName = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", $"{ancestorsStrS[4]}").Rows[0];
                                    var subProName = qz_project_SubProjectName["name"].ToString();
                                    if (!deptNum[deptItemNum][11].Value.Contains(subProName))
                                    {
                                        deptNum[deptItemNum][11].Value = deptNum[deptItemNum][11].Value + "、" + subProName;
                                        deptNum[deptItemNum][11].Id = deptNum[deptItemNum][11].Id + "、" + ancestorsStrS[4];
                                    }
                                }

                                deptNum[deptItemNum][12].Value = (Convert.ToInt32(deptNum[deptItemNum][12].Value) + 1).ToString();
                                deptNum[deptItemNum][13].Value = (Convert.ToDouble(deptNum[deptItemNum][13].Value) + Convert.ToDouble(qz_projectListRowItem["folded"])).ToString();

                                fileNumber++;
                                folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);
                            }
                        }
                    }
                    else if (ancestorsStrS[0] != "UG141218171001")//判断是不是“底图资料区”
                    {
                        for (int j = ancestorsIdx - 2; j > 0; j--)
                        {
                            //用这个项目id找到这个项目的出版区id
                            var project_Id_Index = projectCbStrList.FindIndex(o => o.projectId == ancestorsStrS[1]);
                            if (project_Id_Index != -1)//如果能找到
                            {
                                //判断是不是出版区
                                if (projectCbStrList[project_Id_Index].ancestorsCbString.Contains(ancestorsStrS[j]))
                                {
                                    //查找列队里是不是有这个部门，有就返回所在位置编号，没有返回 1；
                                    var deptNum = ProjectPropertieListS.FindAll(o => o[0].Id == ancestorsStrS[0]);

                                    if (deptNum.Count == 0)//没找到这个部门
                                    {
                                        WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                        fileNumber++;
                                        folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);

                                        break;
                                    }
                                    else//已经有这个部门了
                                    {
                                        int deptItemNum = 0;
                                        int proNum = -1;

                                        foreach (var deptItem in deptNum)
                                        {
                                            //在这个部门下查找是不有这个项目，有返回所在位置，没有返回-1；
                                            proNum = deptItem.FindIndex(o => o.Id == ancestorsStrS[1]);
                                            if (proNum != -1)
                                            {
                                                break;
                                            }
                                            deptItemNum++;
                                        }

                                        if (proNum == -1)
                                        {
                                            WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                            fileNumber++;
                                            folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);
                                            break;
                                        }
                                        else
                                        {
                                            //拿到阶段行
                                            if (!deptNum[deptItemNum][9].Id.Contains(ancestorsStrS[2]))
                                            {
                                                var qz_project_StageName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[2]}").Rows[0];
                                                deptNum[deptItemNum][9].Value = deptNum[deptItemNum][9].Value + "、" + qz_project_StageName["name"].ToString();
                                                deptNum[deptItemNum][9].Id = deptNum[deptItemNum][9].Id + "、" + ancestorsStrS[2];
                                            }

                                            //拿到专业行
                                            if (!deptNum[deptItemNum][10].Id.Contains(ancestorsStrS[3]))
                                            {
                                                var qz_project_MajroName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[3]}").Rows[0];
                                                //去除专业后面的（）与里面的内容
                                                string majroName = Regex.Replace(qz_project_MajroName["name"].ToString(), @"\(.*?\)", "");
                                                if (majroName == "协同归档文件" && ancestorsStrS.Length > 6)
                                                {
                                                    //拿到专业行
                                                    qz_project_MajroName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[5]}").Rows[0];
                                                    //去除专业后面的（）与里面的内容
                                                    majroName = Regex.Replace(qz_project_MajroName["name"].ToString(), @"\(.*?\)", "");
                                                    if (!deptNum[deptItemNum][10].Value.Contains(majroName))
                                                    {
                                                        WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                                        deptNum[deptItemNum][10].Value = majroName;
                                                        deptNum[deptItemNum][10].Id = ancestorsStrS[5];
                                                    }
                                                }
                                                else
                                                {
                                                    if (!deptNum[deptItemNum][10].Value.Contains(majroName))
                                                    {
                                                        WriteInProjectList(qz_projectListRowItem, ancestorsStrS);
                                                        deptNum[deptItemNum][10].Value = majroName;
                                                        deptNum[deptItemNum][10].Id = ancestorsStrS[3];
                                                    }
                                                }
                                            }

                                            //拿到子项行
                                            if (!deptNum[deptItemNum][11].Id.Contains(ancestorsStrS[4]))
                                            {
                                                var qz_project_SubProjectName = SQLiteDataBase.GetDataFromMysql("qz_project", "parent_id", $"{ancestorsStrS[4]}").Rows[0];
                                                var subProName = qz_project_SubProjectName["name"].ToString();
                                                if (!deptNum[deptItemNum][11].Value.Contains(subProName))
                                                {
                                                    deptNum[deptItemNum][11].Value = deptNum[deptItemNum][11].Value + "、" + subProName;
                                                    deptNum[deptItemNum][11].Id = deptNum[deptItemNum][11].Id + "、" + ancestorsStrS[4];
                                                }
                                            }

                                            deptNum[deptItemNum][12].Value = (Convert.ToInt32(deptNum[deptItemNum][12].Value) + 1).ToString();

                                            deptNum[deptItemNum][13].Value = (Convert.ToDouble(deptNum[deptItemNum][13].Value) + Convert.ToDouble(qz_projectListRowItem["folded"])).ToString();

                                            fileNumber++;
                                            folded = folded + Convert.ToDouble(qz_projectListRowItem["folded"]);

                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    AddStatisticsProjectPropertieList(ProjectPropertieListS);
                }
            }
        }

        /// <summary>
        /// 添加项目分解信息(ancestors)
        /// </summary>
        /// <param name="qz_projectListRowItem">项目列表中的一行</param>
        /// <param name="ancestorsStrS">ancestors字符串</param>
        public static void WriteInProjectList(DataRow qz_projectListRowItem, string[] ancestorsStrS)
        {
            ProjectPropertieItemS = new List<ProjectPropertyModel>();
            //拿到项目行
            #region 加载项目属性1-12
            //拿到部门
            DataRow SQLiteDeptRow = SQLiteDataBase.SearchTableFromSQLite("qz_dept", "dept_id", $"{ancestorsStrS[0]}").Rows[0];
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "0", Id = ancestorsStrS[0], Name = "项目部门", Value = SQLiteDeptRow["dept_name"].ToString() });
            //拿到项目的相关属性
            var projectItem = statisticsProjectListTemp.Find(o => o.id == ancestorsStrS[1]);
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "1", Id = "1", Name = "#工程编号", Value = projectItem.identifier });
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "2", Id = ancestorsStrS[1], Name = "#项目名称", Value = projectItem.name });
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "3", Id = "3", Name = "#建设单位", Value = projectItem.unit });
            //项目类型
            string projectType = projectItem.type.ToString();
            if (projectType == "3")
            {
                projectType = "工业项目";
            }
            else if (projectType == "2")
            {
                projectType = "公共项目";
            }
            else if (projectType == "1")
            {
                projectType = "民用项目";
            }
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "4", Id = "4", Name = "#项目类型", Value = projectType });
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "5", Id = "5", Name = "#项目创建人", Value = projectItem.userName });
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "6", Id = "6", Name = "项目经理", Value = projectItem.userName });
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "7", Id = "7", Name = "创建时间", Value = projectItem.createTime });
            string archivesState = "";
            if (projectItem.is_documentation == 1)
            {
                archivesState = "已归档";
            }
            else if (projectItem.is_documentation == 0)
            {
                archivesState = "未归档";
            }
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "8", Id = "8", Name = "是否归档", Value = archivesState });
            //拿到阶段行
            var qz_project_StageName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[2]}").Rows[0];
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "9", Id = ancestorsStrS[2], Name = "阶段", Value = qz_project_StageName["name"].ToString() });
            //拿到专业行
            var qz_project_MajroName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[3]}").Rows[0];
            //去除专业后面的（）与里面的内容
            string majroName = Regex.Replace(qz_project_MajroName["name"].ToString(), @"\(.*?\)", "");
            if (majroName == "协同归档文件" && ancestorsStrS.Length > 6)
            {
                //拿到专业行
                qz_project_MajroName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[5]}").Rows[0];
                //去除专业后面的（）与里面的内容
                majroName = Regex.Replace(qz_project_MajroName["name"].ToString(), @"\(.*?\)", "");
                ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "10", Id = ancestorsStrS[5], Name = "专业", Value = majroName });
            }
            else
            {
                ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "10", Id = ancestorsStrS[3], Name = "专业", Value = majroName });
            }
            //拿到子项行
            var qz_project_SubProjectName = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{ancestorsStrS[4]}").Rows[0];
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "11", Id = ancestorsStrS[4], Name = "子项", Value = qz_project_SubProjectName["name"].ToString() });
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "12", Id = "12", Name = "文件数量", Value = "1" });
            ProjectPropertieItemS.Add(new ProjectPropertyModel() { No = "13", Id = "13", Name = "折A1数量", Value = qz_projectListRowItem["folded"].ToString() });
            #endregion

            //写入到项目分解表内；
            ProjectPropertieListS.Add(ProjectPropertieItemS);

        }

        #endregion

        #region 与服务器链接相关
        /// <summary>
        /// GET请求(方法一):string getUrl, ref T resultDataModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getUrl"></param>
        /// <param name="resultDataModel"></param>
        /// <returns></returns>
        public static bool HttpGet<T>(string getUrl, ref T resultDataModel)
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
                        //this.Dispose();
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
        public static bool HttpGet<T>(string getUrl, ref T resultDataModel, ref int total)
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
                        //this.Dispose();
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
        public static bool HttpPost<T>(string postUrl, string postData, ref T resultDataModel)
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
                        //Dispose();
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
        public static bool HttpPost<T, T2>(string postUrl, T2 paraData, ref T resultDataModel)
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
        public static bool HttpPost<T, T2>(string postUrl, T2 paraData, ref T resultDataModel, ref int total)
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
                        //this.Dispose();
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
        /// POST请求(方法三):string postUrl, T2 paraData, ref T resultDataModel, ref int total
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="postUrl">服务器地址</param>
        /// <param name="paraData">提供的数据类型</param>
        /// <param name="resultDataModel">返回的数据类型</param>
        /// <param name="total">返回条目数量</param>
        /// <returns></returns>
        public static bool HttpPost<T, T2>(string postUrl, T2 paraData, string Token, ref T resultDataModel, ref int total)
        {
            var headers = new Dictionary<string, string>();
            headers.Add("token", Token);
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
                        //this.Dispose();
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
        public static DialogResult ShowErrorMsg(string msg)
        {
            Splasher.Close();
            var result = DialogResult.No;
            result = MessageBox.Show(msg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return result;
        }

        /// <summary>
        /// 发送成功消息（可能这个地方存在问题）
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DialogResult ShowSuccessMsg(string msg)
        {
            Splasher.Close();
            var result = DialogResult.No;
            result = MessageBox.Show(msg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return result;
        }

        /// <summary>
        /// 给用户提示确认消息框
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DialogResult ShowSuccessOKCancelMsg(string msg)
        {
            var result = DialogResult.No;
            result = MessageBox.Show(msg, "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            return result;
        }

        /// <summary>
        /// 删除临时文件夹
        /// </summary>
        public static void DelTempFile()
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
        #endregion



        /// <summary>
        /// 所有项目Id与出版Id列表
        /// </summary>
        public class projectCbStrList
        {
            public string projectId { get; set; }

            public List<string> ancestorsCbString { get; set; }
        }
        /// <summary>
        /// 1:projectDeptName 部门名称；2：部门Id；3：项目list (1: 项目Id \2:Name\3:projectNo 项目编号（阶段id、name（专业id、name（角色id、name（人员id、name；））））)
        /// </summary>
        public class projectDeptModel
        {
            /// <summary>
            /// 部门
            /// </summary>
            public string projectDeptName { get; set; }
            /// <summary>
            /// 部门Id
            /// </summary>
            public string projectDeptId { get; set; }
            /// <summary>
            /// 项目List
            /// </summary>
            public List<ProjectInfoListModel> projectInfoList { get; set; }
        }
        /// <summary>
        ///1: 项目Id \2:Name\3:projectNo 项目编号（阶段id、name（专业id、name（角色id、name（人员id、name；））））
        /// </summary>
        public class ProjectInfoListModel
        {
            /// <summary>
            /// 1：projectId 项目Id
            /// </summary>
            public string projectId { get; set; }
            /// <summary>
            ///2:projectName 项目阶段
            /// </summary>
            public string projectName { get; set; }
            /// <summary>
            /// 3：projectNo 项目编号
            /// </summary>
            public string projectNo { get; set; }
            /// <summary>
            /// 4：建设单位
            /// </summary>
            public string constructUnit { get; set; }
            /// <summary>
            /// 5: 项目类型
            /// </summary>
            public string projectType { get; set; }
            /// <summary>
            /// 6: 项目创建时间
            /// </summary>
            public string createTime { get; set; }
            /// <summary>
            /// 7: 项目归档状态
            /// </summary>
            public string projectStatus { get; set; }
            /// <summary>
            /// 8: 创建人
            /// </summary>
            public string founder { get; set; }
            /// <summary>
            /// 9: 项目经理
            /// </summary>
            public string projectManager { get; set; }
            /// <summary>
            /// 项目中的阶段列表
            /// </summary>
            public List<ProjectStageModel> projectStageList { get; set; }
        }
        /// <summary>
        /// 项目阶段
        /// </summary>
        public class ProjectStageModel
        {
            /// <summary>
            /// 1：projectId 项目阶段Id
            /// </summary>
            public string projectStageId { get; set; }

            /// <summary>
            ///2:projectStage 项目阶段
            /// </summary>
            public string projectStageName { get; set; }
            /// <summary>
            /// 阶段中的专业列表
            /// </summary>
            public List<StageMajroModel> projectMajroList { get; set; }
        }
        /// <summary>
        /// 项目专业
        /// </summary>
        public class StageMajroModel
        {
            /// <summary>
            /// 1：projectMajroId 项目专业Id
            /// </summary>
            public string projectMajroId { get; set; }

            /// <summary>
            ///2:projectMajro 项目专业
            /// </summary>
            public string projectMajroName { get; set; }
            /// <summary>
            /// 子项列表
            /// </summary>
            public List<subProjectListModel> subProjectList { get; set; }
        }
        /// <summary>
        /// 子项目列表
        /// </summary>
        public class subProjectListModel
        {
            /// <summary>
            /// 子项Id
            /// </summary>
            public string subProjectId { get; set; }
            /// <summary>
            /// 子项名称
            /// </summary>
            public string subProjectName { get; set; }
            /// <summary>
            /// 专业中的角色列表
            /// </summary>
            public List<RoleModel> projectRoleList { get; set; }

            /// <summary>
            /// 文件数量
            /// </summary>
            public int fileNumber { get; set; }
            /// <summary>
            /// 折A1数量
            /// </summary>
            public double A1SizeNumber { get; set; }

        }
        /// <summary>
        /// 项目角色
        /// </summary>
        public class RoleModel
        {
            /// <summary>
            /// 1：projectRoleId 项目角色Id
            /// </summary>
            public string projectRoleId { get; set; }
            /// <summary>
            ///2:projectRole 项目角色
            /// </summary>
            public string projectRoleName { get; set; }
            /// <summary>
            /// 角色中的人员列表
            /// </summary>
            public List<UserListModel> projectUserListModel { get; set; }
        }
        /// <summary>
        /// 项目人员列表
        /// </summary>
        public class UserListModel
        {
            /// <summary>
            /// 2：projectUserId 项目用户Id
            /// </summary>
            public string projectUserId { get; set; }
            /// <summary>
            ///3:projectUser 项目用户名
            /// </summary>
            public string projectUserName { get; set; }
            /// <summary>
            /// 子项文件数与折A1数
            /// </summary>
            public List<SubProjectFileNumber> subProjectFileNumberS { get; set; }
        }
        /// <summary>
        /// 文件数量与折A1数量
        /// </summary>
        public class SubProjectFileNumber
        {
            /// <summary>
            /// 文件数量
            /// </summary>
            public int fileNumber { get; set; }
            /// <summary>
            /// 折A1数量
            /// </summary>
            public double A1SizeNumber { get; set; }
        }
    }
}
#region linQ用法基本操作
/*
 
LINQ 的主要用法和关键字

以下是一些 LINQ 的基本用法和常用关键字：

1. 基本查询

LINQ 主要通过两种方式进行查询：

查询表达式语法（Query Expression Syntax）
方法语法（Method Syntax）
示例：

// 假设我们有一个包含整型数字的列表
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// 查询表达式语法
var evenNumbers1 = from n in numbers
                   where n % 2 == 0
                   select n;

// 方法语法
var evenNumbers2 = numbers.Where(n => n % 2 == 0).ToList();
2. 常用关键字

from: 定义数据源。例如：from n in numbers，表示从 numbers 中获取数据。
where: 添加过滤条件。例如：where n % 2 == 0，表示只选择偶数。
select: 指定返回的数据。例如：select n，表示返回原始的数字。
orderby: 按照指定的条件排序。例如：
var sortedNumbers = from n in numbers
                    orderby n descending
                    select n;
group: 分组操作。例如：
var grouped = from n in numbers
              group n by n % 2 into g
              select new { Key = g.Key, Count = g.Count() };
join: 用于连接两个数据源。例如：
var query = from student in students
            join course in courses on student.CourseId equals course.Id
            select new { student.Name, CourseName = course.Name };
let: 在查询中引入临时变量。例如：
var query = from n in numbers
            let square = n * n
            select new { Number = n, Square = square };
distinct: 返回唯一元素。例如：
var distinctNumbers = numbers.Distinct().ToList();
take: 返回集合中的前 N 个元素。例如：
var top3Numbers = numbers.Take(3).ToList();
skip: 跳过集合中的前 N 个元素。例如：
var skippedNumbers = numbers.Skip(2).ToList();
first / firstOrDefault: 获取集合中的第一个元素。
var firstNumber = numbers.First(); // 返回第一个元素
var firstOrDefault = numbers.FirstOrDefault(); // 如果为空，返回默认值
single / singleOrDefault: 获取集合中的唯一元素。
var singleNumber = numbers.Single(n => n == 3); // 必须存在且唯一
var singleOrDefault = numbers.SingleOrDefault(n => n == 3); // 如果未找到，返回默认值
count: 统计集合中的元素数量。例如：
int count = numbers.Count();
sum: 计算集合中数值的总和。例如：
int total = numbers.Sum();
average: 计算集合中数值的平均值。例如：
double average = numbers.Average();
any: 检查集合中是否有满足条件的元素。例如：
bool hasEven = numbers.Any(n => n % 2 == 0);
all: 检查集合中所有元素是否满足给定条件。例如：
bool allEven = numbers.All(n => n % 2 == 0);
3. LINQ to Objects 和 LINQ to SQL

LINQ to Objects: 直接在内存中的集合（如数组、列表等）上进行查询。
LINQ to SQL: 允许使用 LINQ 查询 SQL 数据库。
示例（LINQ to SQL）：

using (DataContext db = new DataContext())
{
    var query = from student in db.Students
                where student.Age > 18
                select student;
    foreach (var student in query)
    {
        Console.WriteLine(student.Name);
    }
}
 
 */



#endregion