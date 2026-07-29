using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PdfiumViewer
{
    /// <summary>
    /// 树状示图
    /// </summary>
    internal class NativeTreeView : TreeView
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName,
                                                string pszSubIdList);

        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(this.Handle, "explorer", null);
        }
    }
}
