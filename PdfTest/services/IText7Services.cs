using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;

namespace PdfTest.services
{
    public class IText7Services: IPdf
    {
        public string FilePath => System.IO.Path.Combine(System.IO.Path.GetTempPath());
        public Stream Pdf(string? url = null)
        {
            var data = new MemoryStream();

            var writer = new PdfWriter(data);
            writer.SetCloseStream(false);

            var pdfDoc = new PdfDocument(writer);

            // Create a Document instance
            Document document = new Document(pdfDoc);

            //Set Page Size A4
            pdfDoc.SetDefaultPageSize(PageSize.A4);

            // Add a header to the document
            Paragraph header = new Paragraph("Paragraph")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20);

            document.Add(header);

            //Add Page Break
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            // TABLE
            Table table = new Table(3, false);
            table.SetMarginTop(20);
            table.SetWidth(UnitValue.CreatePercentValue(100));


            var dateHeaderCell = new Cell().Add(new Paragraph("Date")).SetFontColor(ColorConstants.BLACK).SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(dateHeaderCell);
            var tempratureHeaderCell = new Cell().Add(new Paragraph("Temperature (C)")).SetFontColor(ColorConstants.BLACK).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
            table.AddHeaderCell(tempratureHeaderCell);
            var summaryHeaderCell = new Cell().Add(new Paragraph("Summary")).SetFontColor(ColorConstants.BLACK).SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(summaryHeaderCell);
            string[] Summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

            // Add data to the table
            Random rng = new Random();
            for (int i = 0; i < 100; i++)
            {
                var date = DateTime.Now.AddDays(i);
                int temperature = rng.Next(-20, 40);
                string summary = Summaries[rng.Next(Summaries.Length)];
                table.AddCell(date.ToShortDateString());
                table.AddCell(temperature.ToString()).SetTextAlignment(TextAlignment.CENTER);
                table.AddCell(summary);
            }

            // Add the table to the document
            document.Add(table);

            // Close the document
            document.Close();

            data.Position = 0;

            return data;
        }
    }
}
