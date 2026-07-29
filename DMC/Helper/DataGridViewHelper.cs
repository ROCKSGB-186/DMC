using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace DMC.Helper
{
    public static class DataGridViewHelper
    {
        /// <summary>
        /// 注册滚动条滚功到末尾时的处理事件
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="onScrollToEnd"></param>
        public static void RegistScrollToEndEvent(this DataGridView grid, EventHandler onScrollToEnd)
        {
            grid.Scroll += new ScrollEventHandler((sender, e) =>
            {
                if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                {
                    if (e.NewValue + grid.DisplayedRowCount(false) == grid.Rows.Count)
                    {
                        if (onScrollToEnd != null)
                        {
                            onScrollToEnd(grid, null);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 双缓冲，解决闪烁问题DataGridView
        /// </summary>
        /// <param name="dataGridView "></param>
        /// <param name="flag"></param>
        public static void DoubleBufferedDataGirdView(this DataGridView dataGridView, bool flag)
        {
            Type type = dataGridView.GetType();
            PropertyInfo pi = type.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dataGridView, flag, null);
        }

        /// <summary>

        /// 附加数据到DataGridView（支持IList<T>类型的数据源）

        /// </summary>

        /// <param name="grid"></param>

        /// <param name="source"></param>

        public static void AppendDataToGrid<T>(DataGridView grid, List<T> source)
        {

            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            Type t = source[0].GetType();

            int rowIndex = grid.Rows.Add();

            var girdCells = grid.Rows[rowIndex].Cells;

            foreach (object item in source)
            {
                var row = new DataGridViewRow();

                foreach (DataGridViewCell cell in girdCells)
                {
                    var p = t.GetProperty(cell.OwningColumn.DataPropertyName);

                    object pValue = null;
                    if (p != null)
                    {
                        pValue = p.GetValue(item, null);
                    }

                    var newCell = (DataGridViewCell)cell.Clone();

                    newCell.Value = pValue;

                    row.Cells.Add(newCell);
                }

                rows.Add(row);
            }

            grid.Rows.RemoveAt(rowIndex);

            grid.Rows.AddRange(rows.ToArray());

        }
    }
}
