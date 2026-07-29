using System;

namespace PdfiumViewer
{
    /// <summary>
    /// 解析事件参数
    /// </summary>
    public class PdfiumResolveEventArgs : EventArgs
    {
        public string PdfiumFileName { get; set; }
    }

    public delegate void PdfiumResolveEventHandler(object sender, PdfiumResolveEventArgs e);
}
