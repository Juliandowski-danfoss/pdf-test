using PuppeteerSharp;
using PuppeteerSharp.BrowserData;
using System.Text.Json;

namespace PdfTest.services
{
    public class PuppeteerSharpServices
    {
        private string ChromiumPath => Path.Combine(Path.GetTempPath());

        public async Task<MemoryStream> Pdf(string? url)
        {
            var browser = await CreateBrowserAsync();

            byte[] pdfBytes = null;

            using var chrome = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true, ExecutablePath = browser.GetExecutablePath(Chrome.DefaultBuildId) });

            using var page = await chrome.NewPageAsync();
            await page.EmulateMediaTypeAsync(PuppeteerSharp.Media.MediaType.Print);
            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = 1200,
                Height = 800
            });

            await page.GoToAsync(url);
            await page.EvaluateExpressionHandleAsync("document.fonts.ready");
            var contentSize = await page.EvaluateFunctionAsync<dynamic>("() => { return { width: document.documentElement.scrollWidth, height: document.documentElement.scrollHeight }; }");

            var width = GetIntegerProperty(contentSize, "width");
            var height = GetIntegerProperty(contentSize, "height");


            // Set the viewport size based on the actual content size
            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = width,
                Height = height
            });

            pdfBytes = await page.PdfDataAsync(new PdfOptions()
            {
                PrintBackground = true,
                Width = width,
                Height = height
            });

            return new MemoryStream(pdfBytes);
        }
        private int GetIntegerProperty(dynamic prop, string propName)
        {
            int value = 0;

            if (prop.TryGetProperty(propName, out JsonElement widthElement))
            {
                if (widthElement.TryGetInt32(out int propValue))
                {
                    value = propValue;
                }
            }
            return value;
        }

        private async Task<BrowserFetcher> CreateBrowserAsync()
        {
            var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions { Path = ChromiumPath });

            await browserFetcher.DownloadAsync(Chrome.DefaultBuildId);

            return browserFetcher;
        }
    }
}
