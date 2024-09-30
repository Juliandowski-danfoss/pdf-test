using Microsoft.AspNetCore.Mvc;
using PdfTest.services;

namespace PdfTest.Controllers
{
    [Route("pdf")]
    [ApiController]
    public class PdfController : ControllerBase
    {

        [HttpPost("PuppeteerSharp")]
        public async Task<IActionResult> GeneratePdfPuppeteer([FromBody] string url)
        {
            var service = new PuppeteerSharpServices();

            var pdf = await service.Pdf(url);

            return File(pdf, "application/pdf", $"{Guid.NewGuid()}.pdf");
        }


        [HttpPost("itext7")]
        public IActionResult GeneratePdfItext([FromBody] string url)
        {
            var service = new IText7Services();

            var pdf = service.Pdf();

            return File(pdf, "application/pdf", $"{Guid.NewGuid()}.pdf");
        }


        [HttpPost("adobePdf")]
        public IActionResult GeneratePdfAdobeServices()
        {
            var service = new AdobePdfApiServices();

            var pdf = service.Pdf();

            return File(pdf, "application/pdf", $"{Guid.NewGuid()}.pdf");
        }

        [HttpPost("migradoc")]
        public IActionResult GeneratePdfMigradoc()
        {
            var service = new MigraDocServices();

            var pdf = service.Pdf();

            return File(pdf, "application/pdf", $"{Guid.NewGuid()}.pdf");
        }  
        
        [HttpPost("pdfsharp")]
        public IActionResult GeneratePdfPdfSharp()
        {
            var service = new PdfSharpServices();

            var pdf = service.Pdf();

            return File(pdf, "application/pdf", $"{Guid.NewGuid()}.pdf");
        }
    }
}
