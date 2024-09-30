using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfTest.services
{
    public class PdfSharpServices: IPdf
    {
        public Stream Pdf(string? url = null)
        {
            var document = new PdfDocument();
            document.Info.Title = "PDFsharp V6 title";
            document.Info.Subject = "Subject testing asdasd123";

            // Create an empty page in this document.
            var page = document.AddPage();
            page.Size = PageSize.Letter;
            XFont font = new XFont("Verdana", 12);

            // XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Set up table dimensions
            double tableX = 50;
            double tableY = 50;
            double rowHeight = 20;
            double columnWidth = 200;

            // Draw table header
            gfx.DrawString("Header 1", font, XBrushes.Black, tableX, tableY);
            gfx.DrawString("Header 2", font, XBrushes.Black, tableX + columnWidth, tableY);

            // Draw the separator line
            gfx.DrawLine(XPens.Black, tableX, tableY + rowHeight, tableX + columnWidth * 2, tableY + rowHeight);


            // Add data rows
            for (int i = 1; i <= 5; i++)
            {
                double currentRowY = tableY + rowHeight * (i + 1);
                gfx.DrawString($"Row {i} - Cell 1", font, XBrushes.Black, tableX, currentRowY);
                gfx.DrawString($"Row {i} - Cell 2", font, XBrushes.Black, tableX + columnWidth, currentRowY);
            }

            var pdfResult = new MemoryStream();

            document.Save(pdfResult, false);
            pdfResult.Position = 0;

            return pdfResult;
        }


    }

  
}
