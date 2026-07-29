using DMC.Helper;
using DMC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO.Packaging;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 编辑项目
    /// </summary>
    public partial class FrmProjectEdit : BaseForm
    {
        /// <summary>
        /// 项目属性值：id/名称/值
        /// </summary>
        private List<ProjectPropertyModel> ProjectPropertyList = null;
        private string deptId = null;
        /// <summary>
        /// 项目id
        /// </summary>
        private string projectId = null;
        /// <summary>
        /// 项目阶段:1:ID/ 2:StageName 阶段名称/ 3:projectStageId 项目阶段id
        /// </summary>
        private List<ProjectStageViewModel> projectStageViews = new List<ProjectStageViewModel>();
        /// <summary>
        /// 项目专业: 1：stageId 隶属阶段id/ 2: ID/ 3: MajorName 专业名称/ 4: projectMajorId项目专业id/ 5: 慢板列表 List<GetProjectLevelUserModel> template
        /// </summary>
        private List<ProjectMajorViewModel> projectMajorViews = new List<ProjectMajorViewModel>();
        /// <summary>
        /// 项目人员：1：stageId隶属阶段id/2:majorId隶属专业id/3:ID/4:UserName人员名称
        /// </summary>
        private List<ProjectUserViewModel> projectUserViews = new List<ProjectUserViewModel>();
        /// <summary>
        /// 项目人员角色/ 1:stageId 隶属阶段id/ 2:majorId 隶属专业id/ 3:userId 隶属人员id/ 4:ID/ 5:name 名称
        /// </summary>
        private List<ProjectUserRoleViewModel> projectUserRoleViews = new List<ProjectUserRoleViewModel>();
        /// <summary>
        /// 调用删除的项目阶段:1:ID/ 2:StageName 阶段名称/ 3:projectStageId 项目阶段id List
        /// </summary>
        private List<ProjectStageViewModel> delProjectStageViews = new List<ProjectStageViewModel>();
        /// <summary>
        /// 调用删除的项目专业: 1：stageId 隶属阶段id/ 2: ID/ 3: MajorName 专业名称/ 4: projectMajorId项目专业id/ 5: 慢板列表 List<GetProjectLevelUserModel> template
        /// </summary>
        private List<ProjectMajorViewModel> delProjectMajorViews = new List<ProjectMajorViewModel>();
        /// <summary>
        /// windows的组合框架
        /// </summary>
        private ComboBox comboBox = null;
        /// <summary>
        /// 专业勾选事件是否可用
        /// </summary>
        private bool isEnabled = true;
        /// <summary>
        /// 项目经理类型变量：/ 1：userId 用户主键/ 2:userName 账户/ 3:realName 姓名/ 4:userRoleList 角色列表 list
        /// </summary>
        private GetProjectLevelUserModel projectManager = null;
        /// <summary>
        /// 项目状态
        /// </summary>
        private int projectStatus = -1;
        /// <summary>
        /// 项目编辑
        /// </summary>
        /// <param name="selectDept"></param>
        /// <param name="selectProjectId"></param>
        public FrmProjectEdit(string selectDept, string selectProjectId)
        {
            InitializeComponent();

            deptId = selectDept;
            projectId = selectProjectId;
            dataGridView_项目属性表.AutoGenerateColumns = false;
            dataGridView_人员角色表.AutoGenerateColumns = false;

            if (string.IsNullOrWhiteSpace(selectProjectId))
            {
                this.Text = "新建项目";
            }
            else
            {
                this.Text = "修改项目";
            }

            button7.Enabled = false;
        }

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
        private void ComboBox_Leave(object sender, EventArgs e)
        {
            this.comboBox.Visible = false;
        }
        /// <summary>
        /// 项目类型选择框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            if (this.dataGridView_项目属性表.CurrentCell != null)
            {
                this.dataGridView_项目属性表.CurrentCell.Value = ((ComboBox)sender).Text;
            }

            this.comboBox.Visible = false;
        }
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void FrmProjectAdd_Load(object sender, EventArgs e)
        {
            Splasher.Show(typeof(FrmLoading));

            #region 加载项目类型
            var proTypeList = new List<GetProTypeListModel>();
            if (HttpGet(AppGlobalModel.GetProTypeList, ref proTypeList))
            {
                comboBox = new ComboBox();
                comboBox.DataSource = proTypeList;
                comboBox.DisplayMember = "dictLabel";
                comboBox.ValueMember = "dictValue";
                this.comboBox.Leave += new EventHandler(ComboBox_Leave);
                this.comboBox.SelectedIndexChanged += new EventHandler(ComboBox_TextChanged);
                this.comboBox.Visible = false;
                this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

                this.dataGridView_项目属性表.Controls.Add(this.comboBox);
            }
            else
            {
                this.Close();
            }
            #endregion

            #region 加载阶段
            var resultStageData = new List<ProjectStageResultModel>();
            if (HttpGet(AppGlobalModel.GetStageList, ref resultStageData))
            {
                checkedListBox_阶段.DataSource = resultStageData;
                checkedListBox_阶段.ValueMember = "id";
                checkedListBox_阶段.DisplayMember = "name";
                checkedListBox_阶段.ClearSelected();
            }
            else
            {
                this.Close();
            }
            #endregion

            #region 加载专业
            var resultMajorData = new List<MajorResultModel>();
            if (HttpGet(AppGlobalModel.GetMajorList, ref resultMajorData))
            {
                checkedListBox_专业.DataSource = resultMajorData;
                checkedListBox_专业.ValueMember = "majorId";
                checkedListBox_专业.DisplayMember = "majorName";
                checkedListBox_专业.ClearSelected();
            }
            else
            {
                this.Close();
            }
            #endregion

            #region 人员角色
            // 储存角色数据的集合 
            var resultRileData = new List<UserRoleModel>();
            // 获取角色数据是否成功
            if (HttpGet(AppGlobalModel.GetRoleList, ref resultRileData))
            {
                // 循环所有角色列
                foreach (var item in resultRileData)
                {
                    var col = new DataGridViewCheckBoxColumn();
                    //要插入列的类型
                    col.CellTemplate = new DataGridViewCheckBoxCell();
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.Name = item.roleName;
                    col.HeaderText = item.roleName;
                    col.DataPropertyName = item.roleName;
                    col.Tag = item;
                    //初始化人员列表
                    dataGridView_人员角色表.Columns.Add(col);
                }
            }
            else
            {
                this.Close();
            }
            #endregion

            //判断添加还是修改
            if (string.IsNullOrWhiteSpace(projectId))
            {
                #region 加载项目属性
                comboBox.SelectedIndex = 0;
                ProjectPropertyList = new List<ProjectPropertyModel>();
                ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "1", Name = "#工程编号" });
                ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "2", Name = "#工程名称" });
                ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "3", Name = "#建设单位" });
                ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "4", Name = "#项目类型", Value = ((GetProTypeListModel)comboBox.SelectedItem).dictLabel });
                ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "5", Name = "#项目创建人", Value = AppGlobalModel.UseInfo.realName });
                ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "6", Name = "#项目经理(图签)", Value = "" });

                var resultData = new List<ProjectPropertyResultModel>();
                if (HttpGet(AppGlobalModel.GetAttributeList, ref resultData))
                {
                    ProjectPropertyList.AddRange(resultData.Select(o => new ProjectPropertyModel() { Id = o.custom, Name = o.name }));
                }
                #endregion
            }
            else
            {
                #region 获取项目属性信息
                var resultData = new GetProjectAttributeModel();
                if (HttpGet(AppGlobalModel.GetProjectAttribute + $"?projectId={projectId}", ref resultData))
                {
                    //status项目状态（0正常 1停用 2未发布 3删除 4迭代）
                    projectStatus = resultData.status;
                    //项目类型
                    comboBox.SelectedValue = resultData.proType;

                    if (!string.IsNullOrWhiteSpace(resultData.govern))
                    {
                        projectManager = new GetProjectLevelUserModel();
                        //用户id
                        projectManager.userId = resultData.govern;
                        //项目经理
                        projectManager.realName = resultData.governName;
                    }

                    #region 加载项目属性
                    ProjectPropertyList = new List<ProjectPropertyModel>();
                    ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "1", Name = "#工程编号", Value = resultData.identifier });
                    ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "2", Name = "#工程名称", Value = resultData.name });
                    ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "3", Name = "#建设单位", Value = resultData.unit });
                    ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "4", Name = "#项目类型", Value = ((GetProTypeListModel)comboBox.SelectedItem).dictLabel });
                    ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "5", Name = "#项目创建人", Value = resultData.realName });
                    ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "6", Name = "项目经理", Value = resultData.governName });

                    var resultDataList = new List<ProjectPropertyResultModel>();
                    if (HttpGet(AppGlobalModel.GetAttributeList, ref resultDataList))
                    {
                        ProjectPropertyList.AddRange(resultDataList.Select(o => new ProjectPropertyModel()
                        {
                            Id = o.custom,
                            Name = o.name,
                            Value = resultData.customList?.FirstOrDefault(d => d.custom == o.custom)?.content
                        }));
                    }
                    #endregion
                }
                else
                {
                    this.Close();
                }
                #endregion

                #region 阶段
                var resultStage = new List<ProjectResultModel>();
                if (HttpGet(AppGlobalModel.GetProjectLevelDetails + $"?parentId={projectId}", ref resultStage))
                {
                    if (resultStage != null && resultStage.Any())
                    {
                        ProjectStageResultModel stage;
                        ProjectResultModel r_stage;
                        for (var i = 0; i < checkedListBox_阶段.Items.Count; i++)
                        {
                            stage = (ProjectStageResultModel)checkedListBox_阶段.Items[i];
                            r_stage = resultStage.FirstOrDefault(m => m.varargsId == stage.id);

                            if (r_stage != null)
                            {
                                stage.projectStageId = r_stage.id;
                                checkedListBox_阶段.SetItemChecked(i, true);
                            }
                            else
                            {
                                checkedListBox_阶段.SetItemChecked(i, false);
                            }
                        }
                    }
                }
                #endregion
            }

            dataGridView_项目属性表.DataSource = ProjectPropertyList;
            dataGridView_项目属性表.Rows[0].Cells[0].Style.ForeColor = Color.Red;
            dataGridView_项目属性表.Rows[1].Cells[0].Style.ForeColor = Color.Red;
            dataGridView_项目属性表.Rows[2].Cells[0].Style.ForeColor = Color.Red;
            dataGridView_项目属性表.Rows[3].Cells[0].Style.ForeColor = Color.Red;
            dataGridView_项目属性表.Rows[3].Cells[1].ReadOnly = true;
            dataGridView_项目属性表.Rows[4].Cells[0].Style.ForeColor = Color.Red;
            dataGridView_项目属性表.Rows[5].Cells[0].Style.ForeColor = Color.Red;
            dataGridView_项目属性表.Rows[4].Cells[1].ReadOnly = true;
            dataGridView_项目属性表.Rows[5].Cells[1].ReadOnly = true;
        }

        /// <summary>
        /// 列表序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_项目属性表_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                                e.RowBounds.Location.Y,
                                                dataGridView_项目属性表.RowHeadersWidth - 4,
                                                e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView_项目属性表.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView_项目属性表.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <returns></returns>
        private bool SeveProject()
        {
            var selectModel = (GetProTypeListModel)comboBox.SelectedItem;

            if (selectModel.dictValue == "0")
            {
                ShowErrorMsg($"请选择项目类型！");
                return false;
            }

            #region 项目属性
            var propertyIds = new string[] { "1", "2", "3", "4", "5", "6", };
            foreach (var item in ProjectPropertyList.Where(o => propertyIds.Contains(o.Id)))
            {
                if (string.IsNullOrWhiteSpace(item.Value))
                {
                    ShowErrorMsg($"{item.Name}必填！");
                    return false;
                }
            }
;
            var sss = projectMajorViews;
            //添加
            if (string.IsNullOrWhiteSpace(projectId))
            {
                var projectAttribute = new
                {
                    name = ProjectPropertyList.FirstOrDefault(i => i.Id == "2").Value,
                    parentId = deptId,//所属组织机构ID
                    identifier = ProjectPropertyList.FirstOrDefault(i => i.Id == "1").Value,
                    unit = ProjectPropertyList.FirstOrDefault(i => i.Id == "3").Value,
                    proType = selectModel.dictValue,
                    //项目经理
                    govern = projectManager?.userId,
                    customList = ProjectPropertyList.Where(o => !propertyIds.Contains(o.Id)).Select(o => new { custom = o.Id, content = o.Value }).ToList()
                };

                var resultData = string.Empty;
                var postData = $"projectInfo={JsonConvert.SerializeObject(projectAttribute)}";
                if (HttpPost(AppGlobalModel.AddProjectAttribute, postData, ref resultData))
                {
                    projectId = resultData;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var projectAttribute = new
                {
                    id = projectId,
                    name = ProjectPropertyList.FirstOrDefault(i => i.Id == "2").Value,
                    identifier = ProjectPropertyList.FirstOrDefault(i => i.Id == "1").Value,
                    unit = ProjectPropertyList.FirstOrDefault(i => i.Id == "3").Value,
                    proType = selectModel.dictValue,
                    //项目经理
                    govern = projectManager?.userId,
                    customList = ProjectPropertyList.Where(o => !propertyIds.Contains(o.Id)).Select(o => new { custom = o.Id, content = o.Value }).ToList()
                };

                var resultData = string.Empty;
                var postData = $"porjectInfo={JsonConvert.SerializeObject(projectAttribute)}";
                if (!HttpPost(AppGlobalModel.UpdateProjectAttribute, postData, ref resultData))
                {
                    return false;
                }
            }
            #endregion

            #region 阶段、专业、角色人员
            if (projectStageViews != null && projectStageViews.Any())
            {
                #region 保存阶段
                var postData = string.Empty;
                foreach (var item in projectStageViews)
                {
                    if (string.IsNullOrWhiteSpace(item.projectStageId))
                    {
                        var projectInfo = new
                        {
                            parentId = projectId,//项目属性ID
                            varargsId = item.id,
                            name = item.StageName
                        };

                        var resultData = string.Empty;
                        postData = $"projectInfo={JsonConvert.SerializeObject(projectInfo)}";
                        if (HttpPost(AppGlobalModel.AddProjectStage, postData, ref resultData))
                        {
                            item.projectStageId = resultData;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    #region 专业
                    if (projectMajorViews != null && projectMajorViews.Any(o => o.stageId == item.id && string.IsNullOrWhiteSpace(o.projectMajorId)))
                    {
                        var majorPara = new
                        {
                            parentId = item.projectStageId,//所选阶段返回的ID
                            majorList = projectMajorViews.Where(o => o.stageId == item.id && string.IsNullOrWhiteSpace(o.projectMajorId)).Select(o => new { varargsId = o.id, name = o.MajorName })
                        };

                        var resultMajorData = new List<AddProjectMajorResultModel>();
                        postData = $"projectInfo={JsonConvert.SerializeObject(majorPara)}";
                        if (HttpPost(AppGlobalModel.AddProjectMajor, postData, ref resultMajorData))
                        {
                            foreach (var majorItem in projectMajorViews.Where(o => o.stageId == item.id))
                            {
                                if (string.IsNullOrWhiteSpace(majorItem.projectMajorId))
                                {
                                    majorItem.projectMajorId = resultMajorData.FirstOrDefault(o => o.varargsId == majorItem.id).id;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    #endregion

                    #region 角色人员
                    foreach (var majorItem in projectMajorViews.Where(o => o.stageId == item.id))
                    {
                        if (!string.IsNullOrWhiteSpace(majorItem.projectMajorId))
                        {
                            foreach (var userItem in projectUserViews.Where(o => o.stageId == item.id && o.majorId == majorItem.id))
                            {
                                var rolePara = new
                                {
                                    parentId = majorItem.projectMajorId, //上级id
                                    userId = userItem.id,//人员id
                                    roleList = projectUserRoleViews.Where(o => o.stageId == item.id && o.majorId == majorItem.id && o.userId == userItem.id).Select(o => o.id).ToArray()
                                };

                                var resultRoleData = string.Empty;
                                postData = $"projectInfo={JsonConvert.SerializeObject(rolePara)}";
                                if (!HttpPost(AppGlobalModel.EditProjectUserRole, postData, ref resultRoleData))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    #endregion
                }

                #endregion
            }
            #endregion

            #region 删除专业，阶段
            foreach (var item in delProjectMajorViews)
            {
                var resultDelData = string.Empty;
                if (!HttpGet(AppGlobalModel.DelProjectLevel + "?projectLevelId=" + item.projectMajorId, ref resultDelData))
                {
                    return false;
                }
            }

            foreach (var item in delProjectStageViews)
            {
                var resultDelData = string.Empty;
                if (!HttpGet(AppGlobalModel.DelProjectLevel + "?projectLevelId=" + item.projectStageId, ref resultDelData))
                {
                    return false;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 保存按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_保存_Click(object sender, EventArgs e)
        {
            if (SeveProject())
            {
                ShowSuccessMsg("保存成功！");
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 发布按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_发布_Click(object sender, EventArgs e)
        {
            #region 验证阶段
            if (projectStageViews == null || !projectStageViews.Any())
            {
                ShowErrorMsg($"发布项目，请设置阶段！");
                return;
            }
            if (projectMajorViews == null || !projectMajorViews.Any())
            {
                ShowErrorMsg($"发布项目，请设置专业！");
                return;
            }
            if (projectUserViews == null || !projectUserViews.Any())
            {
                ShowErrorMsg($"发布项目，请设置人员！");
                return;
            }
            if (projectUserRoleViews == null || !projectUserRoleViews.Any())
            {
                ShowErrorMsg($"发布项目，请设置人员角色！");
                return;
            }

            var stageInfo = projectStageViews.FirstOrDefault(o => !projectMajorViews.Exists(m => m.stageId == o.id));
            if (stageInfo != null)
            {
                ShowErrorMsg($"请设置{stageInfo.StageName}下的专业信息！");
                return;
            }

            var majorInfo = projectMajorViews.FirstOrDefault(o => !projectUserViews.Exists(m => m.stageId == o.stageId && m.majorId == o.id));
            if (majorInfo != null)
            {
                stageInfo = projectStageViews.FirstOrDefault(o => o.id == majorInfo.stageId);

                ShowErrorMsg($"请设置{stageInfo.StageName}下{majorInfo.MajorName}专业的人员信息！");
                return;
            }

            var userInfo = projectUserViews.FirstOrDefault(o => !projectUserRoleViews.Exists(m => m.stageId == o.stageId && m.majorId == o.majorId && m.userId == o.id));
            if (userInfo != null)
            {
                stageInfo = projectStageViews.FirstOrDefault(o => o.id == userInfo.stageId);
                majorInfo = projectMajorViews.FirstOrDefault(o => o.id == userInfo.majorId);

                ShowErrorMsg($"请设置{stageInfo.StageName}下{majorInfo.MajorName}专业的{userInfo.UserName}角色信息！");
                return;
            }
            #endregion

            if (SeveProject())
            {
                var projectInfo = new
                {
                    projectId = projectId
                };

                var resultData = new List<AddProjectMajorResultModel>();
                if (HttpPost(AppGlobalModel.ReleaseProject, projectInfo, ref resultData))
                {
                    ShowSuccessMsg("发布成功！");
                    DialogResult = DialogResult.OK;
                }
            }
        }


        /// <summary>
        /// 取消按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_取消_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 阶段事件
        /// <summary>
        /// 阶段选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox_阶段_SelectedIndexChanged(object sender, EventArgs e)
        {
            isEnabled = false;
            var index = checkedListBox_阶段.SelectedIndex;
            if (index > -1)
            {
                SelectStage(checkedListBox_阶段.GetItemChecked(index), index);
            }

            isEnabled = true;
        }

        /// <summary>
        /// 阶段勾选事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox_阶段_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            isEnabled = false;
            SelectStage(e.NewValue == CheckState.Checked ? true : false, e.Index);
            isEnabled = true;
        }

        private void SelectStage(bool isChecked, int index)
        {
            var list = (List<ProjectStageResultModel>)checkedListBox_阶段.DataSource;
            var stageInfo = list[index];

            for (var i = 0; i < checkedListBox_专业.Items.Count; i++)
            {
                checkedListBox_专业.SetItemChecked(i, false);
            }
            dataGridView_人员角色表.DataSource = null;

            if (isChecked)
            {
                if (!projectStageViews.Exists(o => o.id == stageInfo.id))
                {
                    projectStageViews.Add(new ProjectStageViewModel() { id = stageInfo.id, StageName = stageInfo.name, projectStageId = stageInfo.projectStageId });
                }

                #region 加载专业

                #region 项目编辑时候的逻辑
                if (!string.IsNullOrWhiteSpace(stageInfo.projectStageId))
                {
                    delProjectStageViews = delProjectStageViews.Where(o => o.id != stageInfo.id).ToList();

                    var resultMajorData = new List<ProjectResultModel>();
                    if (HttpGet(AppGlobalModel.GetProjectLevelDetails + $"?parentId={stageInfo.projectStageId}", ref resultMajorData))
                    {
                        if (resultMajorData != null && resultMajorData.Any())
                        {
                            resultMajorData = resultMajorData.Where(o => !delProjectMajorViews.Exists(d => d.id == o.varargsId && d.stageId == stageInfo.id)).ToList();

                            foreach (var item in resultMajorData)
                            {
                                if (!projectMajorViews.Exists(o => o.id == item.varargsId && o.stageId == stageInfo.id))
                                {
                                    projectMajorViews.Add(new ProjectMajorViewModel() { id = item.varargsId, MajorName = item.name, stageId = stageInfo.id, projectMajorId = item.id });

                                    #region 项目编辑时候的加载逻辑
                                    if (!string.IsNullOrWhiteSpace(item.id))
                                    {
                                        var resultData = new List<GetProjectLevelUserModel>();
                                        if (HttpGet(AppGlobalModel.GetProjectLevelUser + $"?projectLevelId={item.id}", ref resultData))
                                        {
                                            if (resultData != null && resultData.Any())
                                            {
                                                foreach (var itemUser in resultData)
                                                {
                                                    if (!projectUserViews.Exists(o => o.majorId == item.varargsId && o.stageId == stageInfo.id && o.id == itemUser.userId))
                                                    {
                                                        projectUserViews.Add(new ProjectUserViewModel() { id = itemUser.userId, UserName = itemUser.realName, majorId = item.varargsId, stageId = stageInfo.id });
                                                    }

                                                    if (itemUser.userRoleList != null && itemUser.userRoleList.Any())
                                                    {
                                                        foreach (var roleItem in itemUser.userRoleList)
                                                        {
                                                            if (!projectUserRoleViews.Exists(o => o.id == roleItem.roleId && o.stageId == stageInfo.id && o.majorId == item.varargsId && o.userId == itemUser.userId))
                                                            {
                                                                projectUserRoleViews.Add(new ProjectUserRoleViewModel() { stageId = stageInfo.id, majorId = item.varargsId, userId = itemUser.userId, id = roleItem.roleId, name = roleItem.roleName });
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
                #endregion

                var majorList = projectMajorViews.Where(o => o.stageId == stageInfo.id);
                if (majorList != null && majorList.Any())
                {
                    MajorResultModel major;
                    ProjectMajorViewModel r_major;
                    for (var i = 0; i < checkedListBox_专业.Items.Count; i++)
                    {
                        major = (MajorResultModel)checkedListBox_专业.Items[i];
                        r_major = majorList.FirstOrDefault(m => m.id == major.majorId);
                        if (majorList.Any(m => m.id == major.majorId))
                        {
                            major.projectMajorId = r_major.projectMajorId;
                            //后台配置人员，标记特殊专业
                            if (major.template != null && major.template.Any())
                            {
                                r_major.template = major.template;
                                checkedListBox_专业.SetItemCheckState(i, CheckState.Indeterminate);
                            }
                            else
                            {
                                checkedListBox_专业.SetItemChecked(i, true);
                            }
                        }
                        else
                        {
                            major.projectMajorId = "";
                            checkedListBox_专业.SetItemChecked(i, false);
                        }
                    }
                }
                else
                {
                    MajorResultModel major;
                    for (var i = 0; i < checkedListBox_专业.Items.Count; i++)
                    {
                        major = (MajorResultModel)checkedListBox_专业.Items[i];
                        major.projectMajorId = "";

                        //后台配置人员，自动选择
                        if (major.template != null && major.template.Any())
                        {
                            if (string.IsNullOrWhiteSpace(projectId))
                            {
                                //isEnabled = true;
                                checkedListBox_专业.SetItemCheckState(i, CheckState.Indeterminate);
                                SelectMajor(checkedListBox_专业.GetItemChecked(i), i);
                            }
                            else
                            {
                                checkedListBox_专业.SetItemChecked(i, false);
                            }
                        }
                        else
                        {
                            checkedListBox_专业.SetItemChecked(i, false);
                        }
                    }
                }
                #endregion

                checkedListBox_专业.ClearSelected();

                if (checkedListBox_阶段.SelectedIndex > -1)
                {
                    //启用专业
                    groupBox_专业.Enabled = true;
                    // 停用人员
                    groupBox_人员角色.Enabled = false;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(stageInfo.projectStageId))
                {
                    if (!delProjectStageViews.Exists(o => o.id == stageInfo.id))
                    {
                        delProjectStageViews.Add(new ProjectStageViewModel() { id = stageInfo.id, StageName = stageInfo.name, projectStageId = stageInfo.projectStageId });
                    }
                }

                projectStageViews = projectStageViews.Where(o => o.id != stageInfo.id).ToList();
                //停用专业
                groupBox_专业.Enabled = false;
                // 停用人员
                groupBox_人员角色.Enabled = false;
            }
        }
        #endregion

        #region 专业事件
        /// <summary>
        /// 专业选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox_专业_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isEnabled)
            {
                var index = checkedListBox_专业.SelectedIndex;
                if (index > -1)
                {
                    SelectMajor(checkedListBox_专业.GetItemChecked(index), index);
                }
            }
        }

        /// <summary>
        /// 专业勾选事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox_专业_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //勾选状态为Indeterminate时，说明是后台配置的特殊专业，禁止修改
            if (isEnabled)
            {
                //当勾选状态为Indeterminate时，说明是后台配置的特殊专业，禁止修改
                if (e.CurrentValue == CheckState.Indeterminate)
                {
                    // 禁止修改后台配置的特殊专业
                    e.NewValue = e.CurrentValue;
                }
                //正常专业选择
                SelectMajor(e.NewValue != CheckState.Unchecked ? true : false, e.Index);
            }
        }
        /// <summary>
        /// 选择专业
        /// </summary>
        /// <param name="isChecked">是否选择</param>
        /// <param name="index"></param>
        private void SelectMajor(bool isChecked, int index)
        {
            //通过阶段表拿到阶段的表
            var stageInfo = (ProjectStageResultModel)checkedListBox_阶段.SelectedItem;
            //通过专业表拿到专业表的List
            var list = (List<MajorResultModel>)checkedListBox_专业.DataSource;
            //通过调用传进来的index拿到指定的专业
            var majorInfo = list[index];
            //清空人员表
            dataGridView_人员角色表.DataSource = null;
            //当选择专业时，先清空人员角色表，再根据选择的专业加载人员角色信息
            if (isChecked)
            {
                //如果项目专业内没有（指定的专业id与阶段id）
                if (!projectMajorViews.Exists(o => o.id == majorInfo.majorId && o.stageId == stageInfo.id))
                {
                    //加入这个专业与专业内的所有信息；
                    projectMajorViews.Add(new ProjectMajorViewModel() { id = majorInfo.majorId, MajorName = majorInfo.MajorName, stageId = stageInfo.id, projectMajorId = majorInfo.projectMajorId, template = majorInfo.template});
                }

                #region 加载人员

                #region 项目编辑时候的逻辑
                //专业id不是空
                if (!string.IsNullOrWhiteSpace(majorInfo.projectMajorId))
                {
                    //移除已删除的项目专业
                    delProjectMajorViews = delProjectMajorViews.Where(o => !(o.id == majorInfo.majorId && o.stageId == stageInfo.id)).ToList();
                    //根据专业id获取人员信息
                    var resultData = new List<GetProjectLevelUserModel>();
                    //获取专业下人员数据是否成功
                    if (HttpGet(AppGlobalModel.GetProjectLevelUser + $"?projectLevelId={majorInfo.projectMajorId}", ref resultData))
                    {
                        //如果人员数据不为空，则循环添加人员信息和角色信息
                        if (resultData != null && resultData.Any())
                        {
                            //根据阶段id、专业id、人员id判断项目人员表中是否已存在该人员信息，如果不存在则添加到项目人员表中
                            foreach (var item in resultData)
                            {
                                //根据阶段id、专业id、人员id判断项目人员表中是否已存在该人员信息，如果不存在则添加到项目人员表中
                                if (!projectUserViews.Exists(o => o.majorId == majorInfo.majorId && o.stageId == stageInfo.id && o.id == item.userId))
                                {
                                    //如果项目人员表中不存在该人员信息，则添加到项目人员表中
                                    projectUserViews.Add(new ProjectUserViewModel() { id = item.userId, UserName = item.realName, majorId = majorInfo.majorId, stageId = stageInfo.id });
                                }
                                //如果人员信息中的角色列表不为空，则循环添加角色信息到项目人员角色表中
                                if (item.userRoleList != null && item.userRoleList.Any())
                                {
                                    //根据阶段id、专业id、人员id、角色id判断项目人员角色表中是否已存在该角色信息，如果不存在则添加到项目人员角色表中
                                    foreach (var roleItem in item.userRoleList)
                                    {
                                        //根据阶段id、专业id、人员id、角色id判断项目人员角色表中是否已存在该角色信息，如果不存在则添加到项目人员角色表中
                                        if (!projectUserRoleViews.Exists(o => o.id == roleItem.roleId && o.stageId == stageInfo.id && o.majorId == majorInfo.majorId && o.userId == item.userId))
                                        {
                                            //如果项目人员角色表中不存在该角色信息，则添加到项目人员角色表中
                                            projectUserRoleViews.Add(new ProjectUserRoleViewModel() { stageId = stageInfo.id, majorId = majorInfo.majorId, userId = item.userId, id = roleItem.roleId, name = roleItem.roleName });
                                        }
                                    }
                                    //当项目人员角色表中存在该专业下的人员角色信息时，启用发布按钮，否则禁用发布按钮
                                    if (projectUserRoleViews.Any())
                                    {
                                        //当项目人员角色表中存在该专业下的人员角色信息时，启用发布按钮，否则禁用发布按钮
                                        button7.Enabled = projectStatus == 0 ? false : true;
                                    }
                                    else
                                    {
                                        //当项目人员角色表中不存在该专业下的人员角色信息时，禁用发布按钮
                                        button7.Enabled = false;
                                    }
                                }
                            }

                        }
                    }
                }
                #endregion

                //处理特殊专业的人员
                if (majorInfo.template != null && majorInfo.template.Any())
                {
                    label_添加人员.Enabled = false;
                    label_删除人员.Enabled = false;
                    groupBox_人员角色.Enabled = true;
                    dataGridView_人员角色表.ReadOnly = true;
                    SpecialMajorUserHandle(stageInfo.id);
                }
                else
                {
                    //启用人员
                    groupBox_人员角色.Enabled = true;
                    label_删除人员.Enabled = false;
                    label_添加人员.Enabled = true;
                    dataGridView_人员角色表.ReadOnly = false;
                }
                //用户列表
                var userList = projectUserViews.Where(o => o.majorId == majorInfo.majorId && o.stageId == stageInfo.id).ToList();
                if (userList != null && userList.Any())
                {
                    dataGridView_人员角色表.DataSource = userList;
                    dataGridView_人员角色表.ClearSelection();              
                }
                else
                {
                    if (majorInfo.template != null && majorInfo.template.Any())
                    {
                        foreach (var item in majorInfo.template)
                        {
                            if (!projectUserViews.Exists(o => o.majorId == majorInfo.majorId && o.stageId == stageInfo.id && o.id == item.userId))
                            {
                                projectUserViews.Add(new ProjectUserViewModel() { id = item.userId, UserName = item.realName, majorId = majorInfo.majorId, stageId = stageInfo.id });
                            }

                            if (item.userRoleList != null && item.userRoleList.Any())
                            {
                                foreach (var roleItem in item.userRoleList)
                                {
                                    if (!projectUserRoleViews.Exists(o => o.id == roleItem.roleId && o.stageId == stageInfo.id && o.majorId == majorInfo.majorId && o.userId == item.userId))
                                    {
                                        projectUserRoleViews.Add(new ProjectUserRoleViewModel() { stageId = stageInfo.id, majorId = majorInfo.majorId, userId = item.userId, id = roleItem.roleId, name = roleItem.roleName });
                                    }
                                }
                            }
                        }

                        dataGridView_人员角色表.DataSource = projectUserViews.Where(o => o.majorId == majorInfo.majorId && o.stageId == stageInfo.id).ToList(); 
                        dataGridView_人员角色表.ClearSelection();
                    }
                }
                #endregion                
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(majorInfo.projectMajorId))
                {
                    if (!delProjectMajorViews.Exists(o => o.id == majorInfo.majorId && o.stageId == stageInfo.id))
                    {
                        delProjectMajorViews.Add(new ProjectMajorViewModel() { id = majorInfo.majorId, MajorName = majorInfo.MajorName, stageId = stageInfo.id, projectMajorId = majorInfo.projectMajorId });
                    }
                }

                projectMajorViews = projectMajorViews.Where(o => !(o.id == majorInfo.majorId && o.stageId == stageInfo.id)).ToList();
                // 停用人员
                groupBox_人员角色.Enabled = false;
            }
        }
        #endregion

        #region 角色人员
        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_添加人员_Click(object sender, EventArgs e)
        {
            var frm = new FrmSelectUserList();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var majorInfo = (MajorResultModel)checkedListBox_专业.SelectedItem;
                var stageInfo = (ProjectStageResultModel)checkedListBox_阶段.SelectedItem;
                var userList = projectUserViews.Where(o => o.stageId == stageInfo.id && o.majorId == majorInfo.majorId).ToList();
                if (userList != null && userList.Any())
                {
                    frm.SelectUserList = frm.SelectUserList.Where(o => !userList.Exists(m => m.id == o.userId)).ToList();
                }

                projectUserViews.AddRange(frm.SelectUserList.Select(o => new ProjectUserViewModel() { id = o.userId, UserName = o.realName, majorId = majorInfo.majorId, stageId = stageInfo.id }));

                dataGridView_人员角色表.DataSource = projectUserViews.Where(o => o.stageId == stageInfo.id && o.majorId == majorInfo.majorId).ToList();
                dataGridView_人员角色表.ClearSelection();

                //处理特殊专业的人员
                SpecialMajorUserHandle(stageInfo.id);
            }
        }

        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_删除人员_Click(object sender, EventArgs e)
        {
            var index = dataGridView_人员角色表.CurrentRow.Index;
            if (index > -1)
            {
                var stageInfo = (ProjectStageResultModel)checkedListBox_阶段.SelectedItem;
                var majorInfo = (MajorResultModel)checkedListBox_专业.SelectedItem;
                var list = (List<ProjectUserViewModel>)dataGridView_人员角色表.DataSource;
                var selectModel = list[index];
                if (ShowSuccessOKCancelMsg($"是否确定删除【{selectModel.UserName}】！") == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(majorInfo.projectMajorId))
                    {
                        var resultData = string.Empty;
                        if (!HttpGet(AppGlobalModel.DelProjectUser + "?orderId=" + majorInfo.projectMajorId + "&userId=" + selectModel.id, ref resultData))
                        {
                            return;
                        }
                    }                    

                    projectUserViews = projectUserViews.Where(o => !(o.stageId == stageInfo.id && o.majorId == majorInfo.majorId && o.id == selectModel.id)).ToList();
                    dataGridView_人员角色表.DataSource = projectUserViews.Where(o => o.stageId == stageInfo.id && o.majorId == majorInfo.majorId).ToList();
                    dataGridView_人员角色表.ClearSelection(); 
                    label_删除人员.Enabled = false;

                    //删除人员的时候处理此人员的角色
                    projectUserRoleViews = projectUserRoleViews.Where(o => !(o.stageId == stageInfo.id && o.majorId == majorInfo.majorId && o.userId == selectModel.id)).ToList();

                    //处理特殊专业的人员
                    SpecialMajorUserHandle(stageInfo.id);
                }
            }
        }

        /// <summary>
        /// 角色选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_角色人员_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridView dataGridView = (DataGridView)sender;
                if (e.ColumnIndex != 0)
                {
                    var majorInfo = (MajorResultModel)checkedListBox_专业.SelectedItem;
                    var stageInfo = (ProjectStageResultModel)checkedListBox_阶段.SelectedItem;
                    var userlist = (List<ProjectUserViewModel>)dataGridView_人员角色表.DataSource;
                    var userInfo = userlist[e.RowIndex];
                    var cell = dataGridView_人员角色表.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    var cellTag = (UserRoleModel)cell.OwningColumn.Tag;
                    //切换选择状态
                    var curValue = Convert.ToBoolean(cell.Value);
                    //填充切换后的值
                    dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !curValue;

                    //选择
                    if (!curValue)
                    {
                        if (!projectUserRoleViews.Exists(o => o.id == cellTag.roleId && o.stageId == stageInfo.id && o.majorId == majorInfo.majorId && o.userId == userInfo.id))
                        {
                            projectUserRoleViews.Add(new ProjectUserRoleViewModel() { stageId = stageInfo.id, majorId = majorInfo.majorId, userId = userInfo.id, id = cellTag.roleId, name = cellTag.roleName });
                        }
                    }
                    else
                    {
                        projectUserRoleViews = projectUserRoleViews.Where(o => !(o.id == cellTag.roleId && o.stageId == stageInfo.id && o.majorId == majorInfo.majorId && o.userId == userInfo.id)).ToList();
                    }

                    if (projectUserRoleViews.Any())
                    {
                        button7.Enabled = projectStatus == 0 ? false : true;
                    }
                    else
                    {
                        button7.Enabled = false;
                    }

                    //处理特殊专业的人员
                    SpecialMajorUserHandle(stageInfo.id);
                }
            }
        }

        /// <summary>
        /// 角色格式化显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_角色人员_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex > 0 && checkedListBox_专业.SelectedIndex > -1)
                {
                    //在阶段里选择的阶段
                    var stageInfo = (ProjectStageResultModel)checkedListBox_阶段.SelectedItem;
                    //在专业表里选择的某个专业
                    var majorInfo = (MajorResultModel)checkedListBox_专业.SelectedItem;
                    //人员表里的人员List
                    var list = (List<ProjectUserViewModel>)dataGridView_人员角色表.DataSource;
                    //用户的相关信息：阶段id、专业的id、用户id
                    var userInfo = list[e.RowIndex];
                    //项目用户角色名、阶段id、专业id、人员id
                    var userRoleList = projectUserRoleViews.Where(o => o.stageId == stageInfo.id && o.majorId == majorInfo.majorId && o.userId == userInfo.id)?.ToList();
                    //格式化人员表行
                    var cell = dataGridView_人员角色表.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    //格式化行表头名
                    var cellTag = (UserRoleModel)cell.OwningColumn.Tag;

                    if (userRoleList.Exists(o => o.id == cellTag.roleId))
                    {
                        cell.Value = true;
                        e.Value = true;
                    }
                    else
                    {
                        cell.Value = false;
                        e.Value = false;
                    }
                }
            }
        }

        /// <summary>
        /// 专业 行选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_角色人员_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && dataGridView_人员角色表.ReadOnly==false)
            {
                label_删除人员.Enabled = true;
            }
        }
        #endregion

        #region 特殊专业中的人员
        /// <summary>
        /// 特殊专业中的人员
        /// </summary>
        /// <param name="stageId"></param>
        private void SpecialMajorUserHandle(string stageId)
        {
            //获取此阶段的模板专业列表
            var templateMajorList = projectMajorViews.Where(o => o.template != null && o.template.Any() && o.stageId == stageId).ToList();
            //获取此阶段非模板专业的人员
            var itemUserList = projectUserViews.Where(o => o.stageId == stageId && !templateMajorList.Exists(t=>t.id==o.majorId)).ToList();
            //获取此阶段非模板专业的人员角色
            var itemUserRoleList = projectUserRoleViews.Where(o => o.stageId == stageId && !templateMajorList.Exists(t => t.id == o.majorId)).ToList();           

            foreach (var item in templateMajorList)
            {
                #region 删除角色
                //获取此阶段模板专业的人员角色
                var tempItemUserRoleLisr = projectUserRoleViews.Where(o => o.stageId == stageId && o.majorId == item.id).ToList();

                //删除的角色列表
                var delUserRoleList = tempItemUserRoleLisr.Where(o => !itemUserRoleList.Exists(i => i.id == o.id && i.userId == o.userId)).ToList();
                //循环模板排除模板的，就是可以删除的了
                foreach(var tempItemUset in item.template)
                {
                    if (tempItemUset.userRoleList != null && tempItemUset.userRoleList.Any())
                    {
                        foreach (var roleItem in tempItemUset.userRoleList)
                        {
                            if (delUserRoleList.Exists(o => o.id == roleItem.roleId && o.userId == tempItemUset.userId))
                            {
                                delUserRoleList = delUserRoleList.Where(o => !(o.id == roleItem.roleId && o.stageId == stageId && o.majorId == item.id && o.userId == tempItemUset.userId)).ToList();
                            }
                        }
                    }
                }
                //删除角色
                if(delUserRoleList!=null && delUserRoleList.Any())
                {
                    projectUserRoleViews = projectUserRoleViews.Where(o => !delUserRoleList.Exists(d=>d.id == o.id && d.stageId == o.stageId && d.majorId == o.majorId && d.userId == o.userId)).ToList();
                }
                #endregion

                #region 删除人员
                //获取此阶段模板专业的人员
                var tempItemUserList = projectUserViews.Where(o => o.stageId == stageId && o.majorId == item.id).ToList();

                foreach(var tempItemUser in tempItemUserList)
                {
                    //判断角色之后没有，那就是删除了
                    if (!projectUserRoleViews.Exists(o => o.stageId == stageId && o.majorId == item.id && o.userId == tempItemUser.id))
                    {
                        if (!string.IsNullOrWhiteSpace(item.projectMajorId))
                        {
                            var resultData = string.Empty;
                            if (!HttpGet(AppGlobalModel.DelProjectUser + "?orderId=" + item.projectMajorId + "&userId=" + tempItemUser.id, ref resultData))
                            {
                                return;
                            }                            
                        }

                        projectUserViews = projectUserViews.Where(o => !(o.stageId == stageId && o.majorId == item.id && o.id == tempItemUser.id)).ToList();
                    }
                }
                #endregion

                //添加人员
                foreach (var itemUser in itemUserList)
                {
                    if (!projectUserViews.Exists(o => o.majorId == item.id && o.stageId == stageId && o.id == itemUser.id))
                    {
                        projectUserViews.Add(new ProjectUserViewModel() { id = itemUser.id, UserName = itemUser.UserName, majorId = item.id, stageId = stageId });
                    }
                }

                // 添加角色
                foreach (var itemUserRole in itemUserRoleList)
                {
                    if (!projectUserRoleViews.Exists(o => o.id == itemUserRole.id && o.stageId == stageId && o.majorId == item.id && o.userId == itemUserRole.userId))
                    {
                        projectUserRoleViews.Add(new ProjectUserRoleViewModel() { stageId = stageId, majorId = item.id, userId = itemUserRole.userId, id = itemUserRole.id, name = itemUserRole.name });
                    }
                }

                //保障机制再次添加模板中的人员以及角色
                foreach (var itemTemp in item.template)
                {
                    if (!projectUserViews.Exists(o => o.majorId == item.id && o.stageId == stageId && o.id == itemTemp.userId))
                    {
                        projectUserViews.Add(new ProjectUserViewModel() { id = itemTemp.userId, UserName = itemTemp.realName, majorId = item.id, stageId = stageId });
                    }

                    if (itemTemp.userRoleList != null && itemTemp.userRoleList.Any())
                    {
                        foreach (var roleItem in itemTemp.userRoleList)
                        {
                            if (!projectUserRoleViews.Exists(o => o.id == roleItem.roleId && o.stageId == stageId && o.majorId == item.id && o.userId == itemTemp.userId))
                            {
                                projectUserRoleViews.Add(new ProjectUserRoleViewModel() { stageId = stageId, majorId = item.id, userId = itemTemp.userId, id = roleItem.roleId, name = roleItem.roleName });
                            }
                        }
                    }
                }
            }
        }
        #endregion
        /// <summary>
        /// 项目属性表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_项目属性表_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.dataGridView_项目属性表.CurrentCell.RowIndex == 3 && this.dataGridView_项目属性表.CurrentCell.ColumnIndex == 1)
            {
                Rectangle rectangle = dataGridView_项目属性表.GetCellDisplayRectangle(dataGridView_项目属性表.CurrentCell.ColumnIndex, dataGridView_项目属性表.CurrentCell.RowIndex, false);

                if (dataGridView_项目属性表.CurrentCell.Value != null)
                {
                    this.comboBox.SelectedValue = comboBox.SelectedValue;
                }

                this.comboBox.Left = rectangle.Left;
                this.comboBox.Top = rectangle.Top;
                this.comboBox.Width = rectangle.Width;
                this.comboBox.Height = rectangle.Height;
                this.comboBox.Visible = true;
            }
            else
            {
                this.comboBox.Visible = false;
            }
        }
        /// <summary>
        /// 项目属性表点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_项目属性表_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView_项目属性表.CurrentCell.RowIndex == 5 && this.dataGridView_项目属性表.CurrentCell.ColumnIndex == 1)
            {
                var frm = new FrmSelectProjectManager();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    projectManager = frm.SelectUser;
                    if (this.dataGridView_项目属性表.CurrentCell != null)
                    {
                        this.dataGridView_项目属性表.CurrentCell.Value = frm.SelectUser.realName;
                    }
                }
            }
        }
        /// <summary>
        /// 关闭按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}