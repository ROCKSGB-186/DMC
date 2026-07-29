#pragma warning disable 1591

namespace PdfiumViewer
{
    /// <summary>
    /// Pdf匹配
    /// </summary>
    public class PdfMatch
    {
        public string Text { get; }
        public PdfTextSpan TextSpan { get; }
        public int Page { get; }

        public PdfMatch(string text, PdfTextSpan textSpan, int page)
        {
            Text = text;
            TextSpan = textSpan;
            Page = page;
        }
    }
}
