using System.Drawing;

namespace PdfiumViewer
{
    /// <summary>
    /// 表示PDF页面上的标记.
    /// </summary>
    public interface IPdfMarker
    {
        /// <summary>
        /// The page where the marker is drawn on.
        /// </summary>
        int Page { get; }

        /// <summary>
        /// Draw the marker.
        /// </summary>
        /// <param name="renderer">The PdfRenderer to draw the marker with.</param>
        /// <param name="graphics">The Graphics to draw the marker with.</param>
        void Draw(PdfRenderer renderer, Graphics graphics);
    }
}
