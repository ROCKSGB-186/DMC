using System;
using System.Drawing;
using System.Windows.Forms;

#pragma warning disable 1591

namespace PdfiumViewer
{
    /// <summary>
    /// 设置游标事件参数
    /// </summary>
    public class SetCursorEventArgs : EventArgs
    {
        public Point Location { get; private set; }

        public HitTest HitTest { get; private set; }

        public Cursor Cursor { get; set; }

        public SetCursorEventArgs(Point location, HitTest hitTest)
        {
            Location = location;
            HitTest = hitTest;
        }
    }

    public delegate void SetCursorEventHandler(object sender, SetCursorEventArgs e);
}
