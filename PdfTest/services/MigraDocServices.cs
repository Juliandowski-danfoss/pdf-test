using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.Diagnostics;

namespace PdfTest.services
{
    public class MigraDocServices: IPdf
    {
        public Stream Pdf(string? url = null)
        {
            var pdfResult = new MemoryStream();

            // Create a new MigraDoc document
            Document document = new Document();
            document.Info.Title = "Sample Table";
            document.Info.Subject = "migradoc";
            document.Info.Author = "julian";

            // Create a section in the document
            Section section = document.AddSection();

            // Add a table
            Table table = section.AddTable();
            table.Borders.Width = 0.75;

            // Define columns
            Column column1 = table.AddColumn();
            column1.Width = Unit.FromCentimeter(5);
            Column column2 = table.AddColumn();
            column2.Width = Unit.FromCentimeter(5);

            // Add a header row
            Row headerRow = table.AddRow();
            headerRow.Shading.Color = Colors.LightGray;
            headerRow.Cells[0].AddParagraph("Header 1");
            headerRow.Cells[1].AddParagraph("Header 2");

            // Add some data rows
            for (int i = 1; i <= 60; i++)
            {
                Row row = table.AddRow();
                row.Cells[0].AddParagraph("Row " + i + " - Cell 1");
                row.Cells[1].AddParagraph("Row " + i + " - Cell 2");
            }

            // Render the document to PDF
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };

            pdfRenderer.RenderDocument();

            // Save the PDF document
            const string filename = "TableExample.pdf";
            pdfRenderer.PdfDocument.Save(pdfResult, false);


            pdfResult.Position = 0;
            return pdfResult;
        }
    }
}
