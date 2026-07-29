using DMC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 编辑项目
    /// </summary>
    public partial class FrmProjectInfo : BaseForm
    {
        private List<ProjectPropertyModel> ProjectPropertyList = null;
        private string projectId = null;
        private List<ProjectStageViewModel> projectStageViews = new List<ProjectStageViewModel>();
        private List<ProjectMajorViewModel> projectMajorViews = new List<ProjectMajorViewModel>();
        private List<ProjectUserViewModel> projectUserViews = new List<ProjectUserViewModel>();
        private List<ProjectUserRoleViewModel> projectUserRoleViews = new List<ProjectUserRoleViewModel>();
        private bool isLoad = false;
        public FrmProjectInfo(string selectProjectId)
        {
            InitializeComponent();

            projectId = selectProjectId;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView4.AutoGenerateColumns = false;
        }

        private void FrmProjectInfo_Load(object sender, EventArgs e)
        {
            #region 加载项目类型
            var proTypeList = new List<GetProTypeListModel>();
            if (!HttpGet(AppGlobalModel.GetProTypeList, ref proTypeList))
            {
                this.Close();
            }
            #endregion

            #region 加载阶段
            var resultStageData = new List<ProjectStageResultModel>();
            if (HttpGet(AppGlobalModel.GetStageList, ref resultStageData))
            {
                checkedListBox1.DataSource = resultStageData;
                checkedListBox1.ValueMember = "id";
                checkedListBox1.DisplayMember = "name";
                checkedListBox1.ClearSelected();
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
                checkedListBox2.DataSource = resultMajorData;
                checkedListBox2.ValueMember = "majorId";
                checkedListBox2.DisplayMember = "majorName";
                checkedListBox2.ClearSelected();
            }
            else
            {
                this.Close();
            }
            #endregion

            #region 人员角色
            var resultRileData = new List<UserRoleModel>();
            if (HttpGet(AppGlobalModel.GetRoleList, ref resultRileData))
            {
                foreach (var item in resultRileData)
                {
                    var col = new DataGridViewCheckBoxColumn();
                    //要插入列的类型
                    col.CellTemplate = new DataGridViewCheckBoxCell();
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    col.Name = item.roleName;
                    col.HeaderText = item.roleName;
                    col.DataPropertyName = item.roleName;
                    col.Tag = item;

                    dataGridView4.Columns.Add(col);
                }
            }
            else
            {
                this.Close();
            }
            #endregion

            #region 获取项目属性信息
            var resultData = new GetProjectAttributeModel();
            if (HttpGet(AppGlobalModel.GetProjectAttribute + $"?projectId={projectId}", ref resultData))
            {
                #region 加载项目属性
                ProjectPropertyList = new List<ProjectPropertyModel>();
                ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "1", Name = "#工程编号", Value = resultData.identifier });
                ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "2", Name = "#工程名称", Value = resultData.name });
                ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "3", Name = "#建设单位", Value = resultData.unit });
                ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "4", Name = "#项目类型", Value = proTypeList.FirstOrDefault(o => o.dictValue == resultData.proType).dictLabel });
                ProjectPropertyList.Add(new ProjectPropertyModel() { Id = "5", Name = "#项目创建人", Value = resultData.realName });

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
                    isLoad = true;
                    ProjectStageResultModel stage;
                    ProjectResultModel r_stage;
                    for (var i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        stage = (ProjectStageResultModel)checkedListBox1.Items[i];
                        r_stage = resultStage.FirstOrDefault(m => m.varargsId == stage.id);

                        if (r_stage != null)
                        {
                            stage.projectStageId = r_stage.id;
                            checkedListBox1.SetItemChecked(i, true);
                        }
                        else
                        {
                            checkedListBox1.SetItemChecked(i, false);
                        }
                    }
                }
            }
            #endregion

            isLoad = false;
            dataGridView1.DataSource = ProjectPropertyList;
            dataGridView1.Rows[0].Cells[0].Style.ForeColor = Color.Red;
            dataGridView1.Rows[1].Cells[0].Style.ForeColor = Color.Red;
            dataGridView1.Rows[2].Cells[0].Style.ForeColor = Color.Red;
            dataGridView1.Rows[3].Cells[0].Style.ForeColor = Color.Red;
            dataGridView1.Rows[3].Cells[1].ReadOnly = true;
            dataGridView1.Rows[4].Cells[0].Style.ForeColor = Color.Red;
            dataGridView1.Rows[4].Cells[1].ReadOnly = true;
        }

        /// <summary>
        /// 列表序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                                e.RowBounds.Location.Y,
                                                dataGridView1.RowHeadersWidth - 4,
                                                e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView1.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        #region 阶段事件
        /// <summary>
        /// 阶段选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isLoad = true;
            var index = checkedListBox1.SelectedIndex;
            if (index > -1)
            {
                SelectStage(checkedListBox1.GetItemChecked(index), index);
            }
            isLoad = false;
        }
        /// <summary>
        /// 选择阶段事件
        /// </summary>
        /// <param name="isChecked">是否选定</param>
        /// <param name="index"></param>
        private void SelectStage(bool isChecked, int index)
        {
            var list = (List<ProjectStageResultModel>)checkedListBox1.DataSource;
            var stageInfo = list[index];

            for (var i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, false);
            }
            dataGridView4.DataSource = null;

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
                    var resultMajorData = new List<ProjectResultModel>();
                    if (HttpGet(AppGlobalModel.GetProjectLevelDetails + $"?parentId={stageInfo.projectStageId}", ref resultMajorData))
                    {
                        if (resultMajorData != null && resultMajorData.Any())
                        {
                            foreach (var item in resultMajorData)
                            {
                                if (!projectMajorViews.Exists(o => o.id == item.varargsId && o.stageId == stageInfo.id))
                                {
                                    projectMajorViews.Add(new ProjectMajorViewModel() { id = item.varargsId, MajorName = item.name, stageId = stageInfo.id, projectMajorId = item.id });
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
                    for (var i = 0; i < checkedListBox2.Items.Count; i++)
                    {
                        major = (MajorResultModel)checkedListBox2.Items[i];
                        r_major = majorList.FirstOrDefault(m => m.id == major.majorId);
                        if (majorList.Any(m => m.id == major.majorId))
                        {
                            major.projectMajorId = r_major.projectMajorId;
                            checkedListBox2.SetItemChecked(i, true);
                        }
                        else
                        {
                            major.projectMajorId = "";
                            checkedListBox2.SetItemChecked(i, false);
                        }
                    }
                }
                #endregion

                checkedListBox2.ClearSelected();

                if (checkedListBox1.SelectedIndex > -1)
                {
                    //启用专业
                    groupBox3.Enabled = true;
                    // 停用人员
                    groupBox4.Enabled = false;
                }
            }
            else
            {
                //停用专业
                groupBox3.Enabled = false;
                // 停用人员
                groupBox4.Enabled = false;
            }
        }
        #endregion

        #region 专业事件
        /// <summary>
        /// 专业选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = checkedListBox2.SelectedIndex;
            if (index > -1)
            {
                SelectMajor(checkedListBox2.GetItemChecked(index), index);
            }
        }

        private void SelectMajor(bool isChecked, int index)
        {
            var list = (List<MajorResultModel>)checkedListBox2.DataSource;
            var majorInfo = list[index];
            var stageInfo = (ProjectStageResultModel)checkedListBox1.SelectedItem;

            dataGridView4.DataSource = null;

            if (isChecked)
            {
                if (!projectMajorViews.Exists(o => o.id == majorInfo.majorId && o.stageId == stageInfo.id))
                {
                    projectMajorViews.Add(new ProjectMajorViewModel() { id = majorInfo.majorId, MajorName = majorInfo.MajorName, stageId = stageInfo.id, projectMajorId = majorInfo.projectMajorId });
                }

                #region 加载人员

                #region 项目编辑时候的逻辑
                if (!string.IsNullOrWhiteSpace(majorInfo.projectMajorId))
                {
                    var resultData = new List<GetProjectLevelUserModel>();
                    if (HttpGet(AppGlobalModel.GetProjectLevelUser + $"?projectLevelId={majorInfo.projectMajorId}", ref resultData))
                    {
                        if (resultData != null && resultData.Any())
                        {
                            foreach (var item in resultData)
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

                        }
                    }
                }
                #endregion

                var userList = projectUserViews.Where(o => o.majorId == majorInfo.majorId && o.stageId == stageInfo.id).ToList();
                if (userList != null && userList.Any())
                {
                    dataGridView4.DataSource = userList;
                    dataGridView4.ClearSelection();
                }
                #endregion

                //启用人员
                groupBox4.Enabled = true;
            }
            else
            {
                // 停用人员
                groupBox4.Enabled = false;
            }
        }
        #endregion

        #region 角色人员   
        /// <summary>
        /// 角色格式化显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView4_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex > 0)
                {
                    var majorInfo = (MajorResultModel)checkedListBox2.SelectedItem;
                    var stageInfo = (ProjectStageResultModel)checkedListBox1.SelectedItem;
                    var list = (List<ProjectUserViewModel>)dataGridView4.DataSource;
                    var userInfo = list[e.RowIndex];
                    var userRoleList = projectUserRoleViews.Where(o => o.stageId == stageInfo.id && o.majorId == majorInfo.majorId && o.userId == userInfo.id).ToList();
                    var cell = dataGridView4.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    var cellTag = (UserRoleModel)cell.OwningColumn.Tag;

                    if (userRoleList.Exists(o => o.id == cellTag.roleId))
                    {
                        e.Value = true;
                    }
                    else
                    {
                        e.Value = false;
                    }
                }
            }
        }
        #endregion

        private void checkedListBox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!isLoad)
            {
                e.NewValue = e.CurrentValue;
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!isLoad)
            {
                e.NewValue = e.CurrentValue;
            }
        }
    }
}
