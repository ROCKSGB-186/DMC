using System;
using System.Collections.Generic;

namespace DMC.Models
{
    /// <summary>
    /// 全局参数
    /// /00 ServiceAddress 服务地址
    /// /01 ServiceProt 服务端口
    /// /02 MqttServiceAddress Mqtt服务地址
    /// /03 MqttServiceProt Mqtt服务端口
    /// /04 StartupAutomatically 开机自启动
    /// /05 Logging 是否记录日志
    /// /06 TechnicalInfoUrl 技术资料临时文件路径
    /// /07 Login 登录接口
    /// /08 EditPassword 修改密码
    /// /09 GetDeptList 组织机构列表
    /// /10 GetDeptUserList 获得组织架构下人员列表
    /// /11 GetProTypeList 获取项目类型列表
    /// /12 GetAttributeList 获得项目自定义字段列表
    /// /13 AddProjectAttribute 添加项目属性
    /// /14 GetStageList 获得阶段列表
    /// /15 AddProjectStage 添加项目阶段
    /// /16 GetMajorList 获得专业列表
    /// /17 AddProjectMajor 添加项目专业
    /// /18 AddProjectSubitem 添加项目子项
    /// /19 AddProjectDir 添加项目文件夹
    /// /20 GetRoleList 获得角色列表
    /// /21 AddProjectUser 添加项目人员
    /// /22 EditProjectUserRole 编辑项目人员角色
    /// /23 ReleaseProject 发布项目
    /// /24 FileUpload 文件上传
    /// /25 AgainFileUpload 重新上传文件
    /// /26 GetProjectList 获得项目列表
    /// /27 GetProjectNoReleaseList 获得未发布或项目层级有未发布的项目
    /// /28 GetProjectMyCreateList 我创建的项目列表
    /// /29 GetProjectLevelList 获得项目层级列表
    /// /30 GetProjectAttribute 获得项目属性
    /// /31 GetProjectLevelDetails 获得项目层级详情
    /// /32 GetProjectLevelUser 获得项目层级人员
    /// /33 GetProjectFileList 获得项目文件列表
    /// /34 GetProjectFileLogList 获得项目文件历史版本
    /// /35 UpdateProjectAttribute 修改项目属性
    /// /36 UpdateProjectLevelName 修改项目层级名称
    /// /37 DelProjectLevel 删除项目层级
    /// /38 GetFileTypeList 获得文件类型列表(上传文件选择)
    /// /37 DelProjectLevel 删除项目层级
    /// /38 GetFileTypeList 获得文件类型列表(上传文件选择)
    /// /39 JiSuanFrame 获取图幅列表
    /// /40 GetProjectImport 获得导入项目模板
    /// /41 ProjectImport 导入项目
    /// /42 UpdateProjectFileName 修改项目文件名称
    /// /43 DelProjectFile 删除项目文件
    /// /44 DelProjectUser 删除项目人员
    /// /45 ProjectStartOrEnd 项目启动或停用
    /// /46 UploadProjectDir 上传项目文件夹
    /// /47 ProjectFileDownload 下载项目文件
    /// /48 AddFileCart 购物车新增
    /// /49 DelFileCart 购物车移除
    /// /50 ClearFileCart 清除购物车
    /// /51 FileCartList 我的购物车列表
    /// /52 ApprovalList 获取流程列表
    /// /53 ExportApplyInfo  导出出版流程
    /// /54 StartApprovalQita 发起其他(下载)流程
    /// /55 SeallistOfZhuzhi 根据组织架构查询章列表
    /// /56 ApprovalChubanPass 出版完成
    /// /57 StartApprovalChuban 发起出版流程
    /// /58 ApprovalInfo 获取流程信息
    /// /59 DownloadApplyFile 下载流程文件
    /// /60 Seallist 查询名章列表
    /// /61 ApprovalResult 审批流程
    /// /62 StartApprovalQianzhang 发起签章流程(签名、签章、签名签章)
    /// /63 ApplyList 审批列表查询/64 ApplyInfo 审批详情
    /// /64 ApplyInfo 审批详情
    /// /65 StartApprovalGuidang 发起归档流程
    /// /66 DownloadApplyFileTwo 通用下载流程文件
    /// /67 AddApplyFailUser 添加流程失败通知人员
    /// /67-1 GetApplyUse 返回流程中用户
    /// /68 AddKeepDept 创建归档目录层级
    /// /69 UploadKeepProjectDir 归档上传文件夹
    /// /70 SelectKeepDept 查询归档目录层级
    /// /71 EditKeepDept 修改归档目录层级
    /// /72 DelKeepDept 删除归档目录层级
    /// /73 GetProjectUser 获得整个项目的人员列表
    /// /74 AddKeepProjectAttribute 添加归档项目属性
    /// /75 KeepProjectTempAddDir 归档临时文件夹添加
    /// /76 GetKeepTechnicalNameList 获得归档技术资料名称列表
    /// /77 KeepProjectTempFileUpload 归档临时上传文件 （技术资料/外审图纸）
    /// /78 GetKeepProjectTempAttribute 获得归档项目临时字段
    /// /79 GetKeepProjectTempDir 获得归档项目临时文件夹
    /// /80 GetKeepProjectTempTechnical 获得归档项目临时技术资料
    /// /81 GetKeepProjectTempDrawing 获得归档项目临时外审图纸
    /// /82 GetProjectIsKeep 判断项目是否已归档
    /// /83 GetKeepProjectDir 获得归档项目层级
    /// /84 GetKeepProjectFile 获得归档项目文件
    /// /85 DelKeepProject 删除归档项目层级或文件
    /// /86 DelKeepProjectFile 删除归档项目文件
    /// /87 KeepProjectFileUpload 归档区添加文件
    /// /88 EditKeepProjectName 归档修改名称
    /// /89 KeepAgainFileUpload 归档项目重新上传文件
    /// /90 GetKeepFileLog 获得归档文件日志
    /// /91 GetKeepProjectTempNum 获得临时外审图纸数量
    /// /92 SetKeepProjectTempRead 设置归档临时文件已读状态
    /// /93 ExportClassifyProject 按部门分类导出归档项目
    /// /94 ExportProjectMaterial 导出项目技术资料
    /// /95 ExportProjectSubItem 导出项目子项
    /// /96 ExportProjectSpine 导出项目书脊
    /// /97 ExportProjectAll 导出项目总目录
    /// /98 SelectKeepProject 查询归档项目
    /// /99 MyMessage 消息列表分页
    /// /100 MessageInfo 消息详情
    /// /101 ReadedMessage 消息已读设置
    /// /102 GetApprovalProjectStructure 获得审批项目结构
    /// /103 GetApprovalProjectStructureTwo 获得审批项目结构（审批详情专用）
    /// /104 getApprovalProjectFileInfo 获取项目文件信息
    /// /105 GetApprovalProjectStructureAll 获得审批项目结构汇总
    /// /106 GetVersion 客户端获得版本信息
    /// /107 GetDeptMenu 获得部门权限
    /// /108 GetProjectMenu 获得项目权限
    /// /109 GetOverallSituationMenu 获得人员全局权限
    /// /110 VisualTripartiteSignature 可视化签章
    /// /111 AddOrEditCompilation 添加或修改编研
    /// /112 GetCompilation 获得编研
    /// /113 InitialDirectory 选择文件初始位置
    /// /114
    /// /115 UseInfo 用户信息InitialDownloadDirectory 下载初始位置
    /// /116 Token
    /// /117 qzSealList 签章图片，为空就是没有
    /// /118 DeptList 机构列表
    /// /119 OverallSituationMenu 全局权限
    /// </summary>
    public static class AppGlobalModel
    {
        #region ServiceAddress 服务器登录
        /// <summary>
        ///00 ServiceAddress 服务地址
        /// </summary>
        public static string ServiceAddress { get; set; } //= "218.24.35.83";
        /// <summary>
        ///01 ServiceProt 服务端口
        /// </summary>
        public static int ServiceProt { get; set; } //= 8080;
        /// <summary>
        ///02 MqttServiceAddress Mqtt 服务地址
        /// </summary>
        public static string MqttServiceAddress { get; set; } //= "218.24.35.83";
        /// <summary>
        ///03 MqttServiceProt Mqtt服务端口
        /// </summary>
        public static int MqttServiceProt { get; set; } //= 1884;
        /// <summary>
        ///04 StartupAutomatically 开机自启动
        /// </summary>
        public static bool StartupAutomatically { get; set; }
        /// <summary>
        ///05 Logging 是否记录日志
        /// </summary>
        public static bool Logging { get; set; } = false;
        /// <summary>
        ///06 TechnicalInfoUrl 技术资料临时文件路径
        /// </summary>
        public static string TechnicalInfoUrl { get; } = Environment.CurrentDirectory + $"\\TechnicalInfoTemp";
        /// <summary>
        ///07 Login 登录接口
        /// </summary>
        public static string Login { get { return $"http://{ServiceAddress}:{ServiceProt}/app/login"; } }
        /// <summary>
        ///08 EditPassword 修改密码
        /// </summary>
        public static string EditPassword { get { return $"http://{ServiceAddress}:{ServiceProt}/app/editPassword"; } }
        #endregion
        #region 项目相关
        /// <summary>
        ///09 GetDeptList 组织机构列表
        /// </summary>
        public static string GetDeptList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/deptList"; } }
        /// <summary>
        ///10 GetDeptUserList 获得组织架构下人员列表
        /// </summary>
        public static string GetDeptUserList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/deptUserList"; } }
        /// <summary>
        ///11 GetProTypeList 获取项目类型列表
        /// </summary>
        public static string GetProTypeList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProTypeList"; } }
        /// <summary>
        ///12 GetAttributeList 获得项目自定义字段列表
        /// </summary>
        public static string GetAttributeList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getAttributeList"; } }
        /// <summary>
        ///13 AddProjectAttribute 添加项目属性
        /// </summary>
        public static string AddProjectAttribute { get { return $"http://{ServiceAddress}:{ServiceProt}/app/addProjectAttribute"; } }
        /// <summary>
        ///14 GetStageList 获得阶段列表
        /// </summary>
        public static string GetStageList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getStageList"; } }
        /// <summary>
        ///15 AddProjectStage 添加项目阶段
        /// </summary>
        public static string AddProjectStage { get { return $"http://{ServiceAddress}:{ServiceProt}/app/addProjectStage"; } }
        /// <summary>
        ///16 GetMajorList 获得专业列表
        /// </summary>
        public static string GetMajorList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getMajorList"; } }
        /// <summary>
        ///17 AddProjectMajor 添加项目专业
        /// </summary>
        public static string AddProjectMajor { get { return $"http://{ServiceAddress}:{ServiceProt}/app/addProjectMajor"; } }
        /// <summary>
        ///18 AddProjectSubitem 添加项目子项
        /// </summary>
        public static string AddProjectSubitem { get { return $"http://{ServiceAddress}:{ServiceProt}/app/addProjectSubitem"; } }
        /// <summary>
        ///19 AddProjectDir 添加项目文件夹
        /// </summary>
        public static string AddProjectDir { get { return $"http://{ServiceAddress}:{ServiceProt}/app/addProjectDir"; } }
        /// <summary>
        ///20 GetRoleList 获得角色列表
        /// </summary>
        public static string GetRoleList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getRoleList"; } }
        /// <summary>
        ///21 AddProjectUser 添加项目人员
        /// </summary>
        public static string AddProjectUser { get { return $"http://{ServiceAddress}:{ServiceProt}/app/addProjectUser"; } }
        /// <summary>
        ///22 EditProjectUserRole 编辑项目人员角色
        /// </summary>
        public static string EditProjectUserRole { get { return $"http://{ServiceAddress}:{ServiceProt}/app/editProjectUserRole"; } }
        /// <summary>
        ///23 ReleaseProject 发布项目
        /// </summary>
        public static string ReleaseProject { get { return $"http://{ServiceAddress}:{ServiceProt}/app/releaseProject"; } }
        /// <summary>
        ///24 FileUpload 文件上传
        /// </summary>
        public static string FileUpload { get { return $"http://{ServiceAddress}:{ServiceProt}/app/fileUpload"; } }
        /// <summary>
        ///25 AgainFileUpload 重新上传文件
        /// </summary>
        public static string AgainFileUpload { get { return $"http://{ServiceAddress}:{ServiceProt}/app/againFileUpload"; } }
        /// <summary>
        ///26 GetProjectList 获得项目列表
        /// </summary>
        public static string GetProjectList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectList"; } }
        /// <summary>
        ///27 GetProjectNoReleaseList 获得未发布或项目层级有未发布的项目
        /// </summary>
        public static string GetProjectNoReleaseList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectNoReleaseList"; } }
        /// <summary>
        ///28 GetProjectMyCreateList 我创建的项目列表
        /// </summary>
        public static string GetProjectMyCreateList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectMyCreateList"; } }
        /// <summary>
        ///29 GetProjectLevelList 获得项目层级列表
        /// </summary>
        public static string GetProjectLevelList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectLevelList"; } }
        /// <summary>
        ///30 GetProjectAttribute 获得项目属性
        /// </summary>
        public static string GetProjectAttribute { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectAttribute"; } }
        /// <summary>
        ///31 GetProjectLevelDetails 获得项目层级详情
        /// </summary>
        public static string GetProjectLevelDetails { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectLevelDetails"; } }
        /// <summary>
        ///32 GetProjectLevelUser 获得项目层级人员
        /// </summary>
        public static string GetProjectLevelUser { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectLevelUser"; } }
        /// <summary>
        ///33 GetProjectFileList 获得项目文件列表
        /// </summary>
        public static string GetProjectFileList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectFileList"; } }
        /// <summary>
        ///34 GetProjectFileLogList 获得项目文件历史版本
        /// </summary>
        public static string GetProjectFileLogList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectFileLogList"; } }
        /// <summary>
        ///35 UpdateProjectAttribute 修改项目属性
        /// </summary>
        public static string UpdateProjectAttribute { get { return $"http://{ServiceAddress}:{ServiceProt}/app/updateProjectAttribute"; } }
        /// <summary>
        ///36 UpdateProjectLevelName 修改项目层级名称
        /// </summary>
        public static string UpdateProjectLevelName { get { return $"http://{ServiceAddress}:{ServiceProt}/app/updateProjectLevelName"; } }
        /// <summary>
        ///37 DelProjectLevel 删除项目层级
        /// </summary>
        public static string DelProjectLevel { get { return $"http://{ServiceAddress}:{ServiceProt}/app/delProjectLevel"; } }
        /// <summary>
        ///38 GetFileTypeList 获得文件类型列表(上传文件选择)
        /// </summary>
        public static string GetFileTypeList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getFileTypeList"; } }
        /// <summary>
        ///39 JiSuanFrame 获取图幅列表
        /// </summary>
        public static string JiSuanFrame { get { return $"http://{ServiceAddress}:{ServiceProt}/app/jisuanframe"; } }
        /// <summary>
        ///40 GetProjectImport 获得导入项目模板
        /// </summary>
        public static string GetProjectImport { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectImport"; } }
        /// <summary>
        ///41 ProjectImport 导入项目
        /// </summary>
        public static string ProjectImport { get { return $"http://{ServiceAddress}:{ServiceProt}/app/projectImport"; } }
        /// <summary>
        ///42 UpdateProjectFileName 修改项目文件名称
        /// </summary>
        public static string UpdateProjectFileName { get { return $"http://{ServiceAddress}:{ServiceProt}/app/updateProjectFileName"; } }
        /// <summary>
        ///43 DelProjectFile 删除项目文件
        /// </summary>
        public static string DelProjectFile { get { return $"http://{ServiceAddress}:{ServiceProt}/app/delProjectFile"; } }
        /// <summary>
        ///44 DelProjectUser 删除项目人员
        /// </summary>
        public static string DelProjectUser { get { return $"http://{ServiceAddress}:{ServiceProt}/app/delProjectUser"; } }
        /// <summary>
        ///45 ProjectStartOrEnd 项目启动或停用
        /// </summary>
        public static string ProjectStartOrEnd { get { return $"http://{ServiceAddress}:{ServiceProt}/app/projectStartOrEnd"; } }
        /// <summary>
        ///46 UploadProjectDir 上传项目文件夹
        /// </summary>
        public static string UploadProjectDir { get { return $"http://{ServiceAddress}:{ServiceProt}/app/uploadProjectDir"; } }
        /// <summary>
        ///47 ProjectFileDownload 下载项目文件
        /// </summary>
        public static string ProjectFileDownload { get { return $"http://{ServiceAddress}:{ServiceProt}/app/projectFileDownload"; } }
        /// <summary>
        ///48 FileCartDownload 购物车下载文件
        /// </summary>
        public static string FileCartDownload { get { return $"http://{ServiceAddress}:{ServiceProt}/app/fileCartDownload"; } }
        #endregion
        #region 文件购物车
        /// <summary>
        ///48 AddFileCart 购物车新增
        /// </summary>
        public static string AddFileCart { get { return $"http://{ServiceAddress}:{ServiceProt}/app/addFileCart"; } }
        /// <summary>
        ///49 DelFileCart 购物车移除
        /// </summary>
        public static string DelFileCart { get { return $"http://{ServiceAddress}:{ServiceProt}/app/delFileCart"; } }
        /// <summary>
        ///50 ClearFileCart 清除购物车
        /// </summary>
        public static string ClearFileCart { get { return $"http://{ServiceAddress}:{ServiceProt}/app/clearFileCart"; } }
        /// <summary>
        ///51 FileCartList 我的购物车列表
        /// </summary>
        public static string FileCartList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/fileCartList"; } }
        #endregion
        #region 审批流程
        /// <summary>
        ///52  ApprovalList 获取流程列表
        /// </summary>
        public static string ApprovalList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/approvalList"; } }
        /// <summary>
        ///53 ExportApplyInfo  导出出版流程
        /// </summary>
        public static string ExportApplyInfo { get { return $"http://{ServiceAddress}:{ServiceProt}/app/exportApplyInfo"; } }
        /// <summary>
        ///54 StartApprovalQita 发起其他(下载)流程
        /// </summary>
        public static string StartApprovalQita { get { return $"http://{ServiceAddress}:{ServiceProt}/app/startApprovalQita"; } }
        /// <summary>
        ///55 SeallistOfZhuzhi 根据组织架构查询章列表
        /// </summary>
        public static string SeallistOfZhuzhi { get { return $"http://{ServiceAddress}:{ServiceProt}/app/seallistOfZhuzhi"; } }
        /// <summary>
        ///56 ApprovalChubanPass 出版完成
        /// </summary>
        public static string ApprovalChubanPass { get { return $"http://{ServiceAddress}:{ServiceProt}/app/approvalChubanPass"; } }
        /// <summary>
        ///57 StartApprovalChuban 发起出版流程
        /// </summary>
        public static string StartApprovalChuban { get { return $"http://{ServiceAddress}:{ServiceProt}/app/startApprovalChuban"; } }
        /// <summary>
        ///58 ApprovalInfo 获取流程信息
        /// </summary>
        public static string ApprovalInfo { get { return $"http://{ServiceAddress}:{ServiceProt}/app/approvalInfo"; } }
        /// <summary>
        ///59 DownloadApplyFile 下载流程文件
        /// </summary>
        public static string DownloadApplyFile { get { return $"http://{ServiceAddress}:{ServiceProt}/app/downloadApplyFile"; } }
        /// <summary>
        ///60 Seallist 查询名章列表
        /// </summary>
        public static string Seallist { get { return $"http://{ServiceAddress}:{ServiceProt}/app/seallist"; } }
        /// <summary>
        ///61 ApprovalResult 审批流程
        /// </summary>
        public static string ApprovalResult { get { return $"http://{ServiceAddress}:{ServiceProt}/app/approvalResult"; } }
        /// <summary>
        ///62 StartApprovalQianzhang 发起签章流程(签名、签章、签名签章)
        /// </summary>
        public static string StartApprovalQianzhang { get { return $"http://{ServiceAddress}:{ServiceProt}/app/startApprovalQianzhang"; } }
        /// <summary>
        ///63 ApplyList 审批列表查询
        /// </summary>
        public static string ApplyList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/applyList"; } }
        /// <summary>
        ///64 ApplyInfo 审批详情
        /// </summary>
        public static string ApplyInfo { get { return $"http://{ServiceAddress}:{ServiceProt}/app/applyInfo"; } }
        /// <summary>
        ///65 StartApprovalGuidang 发起归档流程
        /// </summary>
        public static string StartApprovalGuidang { get { return $"http://{ServiceAddress}:{ServiceProt}/app/startApprovalGuidang"; } }
        /// <summary>
        ///66 DownloadApplyFileTwo 通用下载流程文件
        /// /app/downloadApplyFileTwo?applyId=c370bd31a0ad409aa514aa85c55f623c
        /// </summary>
        public static string DownloadApplyFileTwo { get { return $"http://{ServiceAddress}:{ServiceProt}/app/downloadApplyFileTwo"; } }
        /// <summary>
        ///67 AddApplyFailUser 添加流程失败通知人员
        /// </summary>
        public static string AddApplyFailUser { get { return $"http://{ServiceAddress}:{ServiceProt}/app/addApplyFailUser"; } }
        /// <summary>
        /// 67-1 GetApplyUse 返回流程中用户
        /// </summary>
        public static string GetApplyUser { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getApplyUser"; } }
        #endregion
        #region 归档
        /// <summary>
        ///68 AddKeepDept 创建归档目录层级
        /// </summary>
        public static string AddKeepDept { get { return $"http://{ServiceAddress}:{ServiceProt}/app/addKeepDept"; } }
        /// <summary>
        ///69 UploadKeepProjectDir 归档上传文件夹
        /// </summary>
        public static string UploadKeepProjectDir { get { return $"http://{ServiceAddress}:{ServiceProt}/app/uploadKeepProjectDir"; } }
        /// <summary>
        ///70 SelectKeepDept 查询归档目录层级
        /// </summary>
        public static string SelectKeepDept { get { return $"http://{ServiceAddress}:{ServiceProt}/app/selectKeepDept"; } }
        /// <summary>
        ///71 EditKeepDept 修改归档目录层级
        /// </summary>
        public static string EditKeepDept { get { return $"http://{ServiceAddress}:{ServiceProt}/app/editKeepDept"; } }
        /// <summary>
        ///72 DelKeepDept 删除归档目录层级
        /// </summary>
        public static string DelKeepDept { get { return $"http://{ServiceAddress}:{ServiceProt}/app/delKeepDept"; } }
        /// <summary>
        ///73 GetProjectUser 获得整个项目的人员列表
        /// </summary>
        public static string GetProjectUser { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectUser"; } }
        /// <summary>
        ///74 AddKeepProjectAttribute 添加归档项目属性
        /// </summary>
        public static string AddKeepProjectAttribute { get { return $"http://{ServiceAddress}:{ServiceProt}/app/addKeepProjectAttribute"; } }
        /// <summary>
        ///75 KeepProjectTempAddDir 归档临时文件夹添加
        /// </summary>
        public static string KeepProjectTempAddDir { get { return $"http://{ServiceAddress}:{ServiceProt}/app/keepProjectTempAddDir"; } }
        /// <summary>
        ///76 GetKeepTechnicalNameList 获得归档技术资料名称列表
        /// </summary>
        public static string GetKeepTechnicalNameList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getKeepTechnicalNameList"; } }
        /// <summary>
        ///77 KeepProjectTempFileUpload 归档临时上传文件 （技术资料/外审图纸）
        /// </summary>
        public static string KeepProjectTempFileUpload { get { return $"http://{ServiceAddress}:{ServiceProt}/app/keepProjectTempFileUpload"; } }
        /// <summary>
        ///78 GetKeepProjectTempAttribute 获得归档项目临时字段
        /// </summary>
        public static string GetKeepProjectTempAttribute { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getKeepProjectTempAttribute"; } }
        /// <summary>
        ///79 GetKeepProjectTempDir 获得归档项目临时文件夹
        /// </summary>
        public static string GetKeepProjectTempDir { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getKeepProjectTempDir"; } }
        /// <summary>
        ///80 GetKeepProjectTempTechnical 获得归档项目临时技术资料
        /// </summary>
        public static string GetKeepProjectTempTechnical { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getKeepProjectTempTechnical"; } }
        /// <summary>
        ///81 GetKeepProjectTempDrawing 获得归档项目临时外审图纸
        /// </summary>
        public static string GetKeepProjectTempDrawing { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getKeepProjectTempDrawing"; } }
        /// <summary>
        ///82 GetProjectIsKeep 判断项目是否已归档
        /// </summary>
        public static string GetProjectIsKeep { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectIsKeep"; } }
        /// <summary>
        ///83 GetKeepProjectDir 获得归档项目层级
        /// </summary>
        public static string GetKeepProjectDir { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getKeepProjectDir"; } }
        /// <summary>
        ///84 GetKeepProjectFile 获得归档项目文件
        /// </summary>
        public static string GetKeepProjectFile { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getKeepProjectFile"; } }
        /// <summary>
        ///85 DelKeepProject 删除归档项目层级或文件
        /// </summary>
        public static string DelKeepProject { get { return $"http://{ServiceAddress}:{ServiceProt}/app/delKeepProject"; } }
        /// <summary>
        ///86 DelKeepProjectFile 删除归档项目文件
        /// </summary>
        public static string DelKeepProjectFile { get { return $"http://{ServiceAddress}:{ServiceProt}/app/delKeepProjectFile"; } }
        /// <summary>
        ///87 KeepProjectFileUpload 归档区添加文件
        /// </summary>
        public static string KeepProjectFileUpload { get { return $"http://{ServiceAddress}:{ServiceProt}/app/keepProjectFileUpload"; } }
        /// <summary>
        ///88 EditKeepProjectName 归档修改名称
        /// </summary>
        public static string EditKeepProjectName { get { return $"http://{ServiceAddress}:{ServiceProt}/app/editKeepProjectName"; } }
        /// <summary>
        ///89 KeepAgainFileUpload 归档项目重新上传文件
        /// </summary>
        public static string KeepAgainFileUpload { get { return $"http://{ServiceAddress}:{ServiceProt}/app/keepAgainFileUpload"; } }
        /// <summary>
        ///90 GetKeepFileLog 获得归档文件日志
        /// </summary>
        public static string GetKeepFileLog { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getKeepFileLog"; } }
        /// <summary>
        ///91 GetKeepProjectTempNum 获得临时外审图纸数量
        /// </summary>
        public static string GetKeepProjectTempNum { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getKeepProjectTempNum"; } }
        /// <summary>
        ///92 SetKeepProjectTempRead 设置归档临时文件已读状态
        /// </summary>
        public static string SetKeepProjectTempRead { get { return $"http://{ServiceAddress}:{ServiceProt}/app/setKeepProjectTempRead"; } }
        /// <summary>
        ///93 ExportClassifyProject 按部门分类导出归档项目
        /// </summary>
        public static string ExportClassifyProject { get { return $"http://{ServiceAddress}:{ServiceProt}/app/exportClassifyProject"; } }
        /// <summary>
        ///94 ExportProjectMaterial 导出项目技术资料
        /// </summary>
        public static string ExportProjectMaterial { get { return $"http://{ServiceAddress}:{ServiceProt}/app/exportProjectMaterial"; } }
        /// <summary>
        ///95 ExportProjectSubItem 导出项目子项
        /// </summary>
        public static string ExportProjectSubItem { get { return $"http://{ServiceAddress}:{ServiceProt}/app/exportProjectSubItem"; } }
        /// <summary>
        ///96 ExportProjectSpine 导出项目书脊
        /// </summary>
        public static string ExportProjectSpine { get { return $"http://{ServiceAddress}:{ServiceProt}/app/exportProjectSpine"; } }
        /// <summary>
        ///97 ExportProjectAll 导出项目总目录
        /// </summary>
        public static string ExportProjectAll { get { return $"http://{ServiceAddress}:{ServiceProt}/app/exportProjectAll"; } }
        /// <summary>
        ///98 SelectKeepProject 查询归档项目
        /// </summary>
        public static string SelectKeepProject { get { return $"http://{ServiceAddress}:{ServiceProt}/app/selectKeepProject"; } }
        #endregion
        #region 消息
        /// <summary>
        ///99 MyMessage 消息列表分页
        /// </summary>
        public static string MyMessage { get { return $"http://{ServiceAddress}:{ServiceProt}/app/myMessage"; } }
        /// <summary>
        ///100 MessageInfo 消息详情
        /// </summary>
        public static string MessageInfo { get { return $"http://{ServiceAddress}:{ServiceProt}/app/messageInfo"; } }
        /// <summary>
        ///101 ReadedMessage 消息已读设置
        /// </summary>
        public static string ReadedMessage { get { return $"http://{ServiceAddress}:{ServiceProt}/app/readedMessage"; } }
        #endregion
        #region 其它杂项
        /// <summary>
        ///102 GetApprovalProjectStructure 获得审批项目结构
        /// </summary>
        public static string GetApprovalProjectStructure { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getApprovalProjectStructure"; } }
        /// <summary>
        ///103 GetApprovalProjectStructureTwo 获得审批项目结构（审批详情专用）
        /// </summary>
        public static string GetApprovalProjectStructureTwo { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getApprovalProjectStructureTwo"; } }
        /// <summary>
        ///104 getApprovalProjectFileInfo 获取项目文件信息
        /// </summary>
        public static string GetApprovalProjectFileInfo { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getApprovalProjectFileInfo"; } }
        /// <summary>
        ///105 GetApprovalProjectStructureAll 获得审批项目结构汇总
        /// </summary>
        public static string GetApprovalProjectStructureAll { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getApprovalProjectStructureAll"; } }
        /// <summary>
        ///106 GetVersion 客户端获得版本信息
        /// </summary>
        public static string GetVersion { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getVersion"; } }
        /// <summary>
        ///107 GetDeptMenu 获得部门权限
        /// </summary>
        public static string GetDeptMenu { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getDeptMenu"; } }
        /// <summary>
        ///108 GetProjectMenu 获得项目权限
        /// </summary>
        public static string GetProjectMenu { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectMenu"; } }
        /// <summary>
        ///109 GetOverallSituationMenu 获得人员全局权限
        /// </summary>
        public static string GetOverallSituationMenu { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getOverallSituationMenu"; } }
        /// <summary>
        ///110 VisualTripartiteSignature 可视化签章
        /// </summary>
        public static string VisualTripartiteSignature { get { return $"http://{ServiceAddress}:{ServiceProt}/app/visualTripartiteSignature"; } }
        #endregion
        #region 编研
        /// <summary>
        ///111 AddOrEditCompilation 添加或修改编研
        /// </summary>
        public static string AddOrEditCompilation { get { return $"http://{ServiceAddress}:{ServiceProt}/app/addOrEditCompilation"; } }
        /// <summary>
        ///112 GetCompilation 获得编研
        /// </summary>
        public static string GetCompilation { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getCompilation"; } }
        #endregion
        #region 统计
        /// <summary>
        /// 113 getQzProjectList 获取项目列表
        /// </summary>
        public static string GetQzProjectList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getQzProjectList"; } }
        //public static string GetProjectList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getProjectList"; } }
        /// <summary>
        /// 114 getUserList 获取用户列表
        /// </summary>
        public static string GetUserList { get { return $"http://{ServiceAddress}:{ServiceProt}/app/getUserList"; } }
        #endregion
        /// <summary>
        ///113 InitialDirectory 选择文件初始位置
        /// </summary>
        public static string InitialDirectory { get; set; }
        /// <summary>
        ///114 InitialDownloadDirectory 下载初始位置
        /// </summary>
        public static string InitialDownloadDirectory { get; set; }
        /// <summary>
        ///115 UseInfo 用户信息
        /// </summary>
        public static UserModel UseInfo { get; set; }
        /// <summary>
        ///116 Token
        /// </summary>
        public static string Token { get; set; }
        /// <summary>
        ///117 qzSealList 签章图片，为空就是没有
        /// </summary>
        public static List<QzSealModel> qzSealList { get; set; }
        /// <summary>
        ///118 DeptList 机构列表
        /// </summary>
        public static List<DeptInfoResultModel> DeptList { get; set; }
        /// <summary>
        ///119 OverallSituationMenu 全局权限
        /// </summary>
        public static List<string> OverallSituationMenu { get; set; }
    }

    /* 权限对应表
     * 
     * 项目管理
	列表(查询)  promanage:list
	新建 promanage:add
	修改 promanage:edit
	删除 promanage:del
	下载模板 promanage:down
	导入 promanage:import
	停用 promanage:disable
项目文件
    项目文件 profile:all:list
	列表(查询) profile:list
	上传文件 profile:upload
	删除文件 profile:del
    档案管理员删除文件 profile:del:all（有流程也可以删除）
	下载 profile:down
	项目归档 profile:archive
	发起审批 profile:apply
	选定文件清单 profile:cart:list

	打开文件 profile:open
	重命名文件 profile:rename
	查看版本(列表) profile:version
	历史版本下载 profile:version:down
	历史版本查看 profile:version:info
	替换文件 profile:replace
	选定文件加入列表 profile:cart:add
 
	新增文件夹 profile:folder:add
	修改文件夹 profile:folder:edit
	删除文件夹 profile:folder:del
    发起可视化签名签章 profile:signature

归档管理
	列表(查询) proarchive:list
	上传文件 proarchive:upload
	删除文件 proarchive:del
	发起审批 proarchive:apply
	打开文件 proarchive:open
	重命名文件 proarchive:rename
	查看版本(列表) proarchive:version
	历史版本下载 proarchive:version:down
	历史版本查看 proarchive:version:info
	替换文件 proarchive:replace
	新增文件夹 proarchive:folder:add
	修改文件夹 proarchive:folder:edit
	删除文件夹 proarchive:folder:del	
    归档管理-编研管理
    proarchive:compilation:i
    归档管理-导出管理
    proarchive:export:i
    归档管理-搜索
    proarchive:search
     * 
     */
}
