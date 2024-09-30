using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.auth;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.documentmerge;
using Adobe.PDFServicesSDK.pdfjobs.results;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities.Zlib;

namespace PdfTest.services
{
    public class AdobePdfApiServices : IPdf
    {
        public PDFServices Services { get; set; }

        public AdobePdfApiServices()
        {
            // HOW TO GET CREDENTIALS  https://acrobatservices.adobe.com/dc-integration-creation-app-cdn/main.html?api=document-generation-api
            // DOWNLOAD ZIP FILE WITH CREDENTIALS

            var credentials = (clientId: "<YOUR-CLIENTID>", clientSecret: "<YOUR-CLIENTSECRET>");

            // Initial setup, create credentials instance
            Services = new PDFServices(new ServicePrincipalCredentials(credentials.clientId, credentials.clientSecret));
        }

        public Stream Pdf(string? url = null)
        {
            using Stream inputStream = File.OpenRead(@"templates/receiptTemplate.docx");
            IAsset asset = Services.Upload(inputStream, PDFServicesMediaType.DOCX.GetMIMETypeValue());

            String json = this.GetRequest();
            JObject jsonDataForMerge = JObject.Parse(json);

            // Create parameters for the job
            DocumentMergeParams documentMergeParams = DocumentMergeParams.DocumentMergeParamsBuilder()
                .WithJsonDataForMerge(jsonDataForMerge)
                .WithOutputFormat(OutputFormat.PDF)
                .Build();

            // Creates a new job instance
            DocumentMergeJob documentMergeJob = new DocumentMergeJob(asset, documentMergeParams);

            // Submits the job and gets the job result
            String location = Services.Submit(documentMergeJob);
            PDFServicesResponse<DocumentMergeResult> pdfServicesResponse =
                Services.GetJobResult<DocumentMergeResult>(location, typeof(DocumentMergeResult));

            // Get content from the resulting asset(s)
            IAsset resultAsset = pdfServicesResponse.Result.Asset;
            StreamAsset streamAsset = Services.GetContent(resultAsset);

            return streamAsset.Stream;
        }


        private string GetRequest()
        {
            return "{\r\n    \"author\": \"Gary Lee\",\r\n    \"Company\": {\r\n        \"Name\": \"Projected\",\r\n        \"Address\": \"19718 Mandrake Way\",\r\n        \"PhoneNumber\": \"+1-100000098\"\r\n    },\r\n    \"Invoice\": {\r\n        \"Date\": \"January 15, 2021\",\r\n        \"Number\": 123,\r\n        \"Items\": [\r\n            {\r\n                \"item\": \"Gloves\",\r\n                \"description\": \"Microwave gloves\",\r\n                \"UnitPrice\": 5,\r\n                \"Quantity\": 2,\r\n                \"Total\": 10\r\n            },\r\n            {\r\n                \"item\": \"Bowls\",\r\n                \"description\": \"Microwave bowls\",\r\n                \"UnitPrice\": 10,\r\n                \"Quantity\": 2,\r\n                \"Total\": 20\r\n            }\r\n        ]\r\n    },\r\n    \"Customer\": {\r\n        \"Name\": \"Collins Candy\",\r\n        \"Address\": \"315 Dunning Way\",\r\n        \"PhoneNumber\": \"+1-200000046\",\r\n        \"Email\": \"cc@abcdef.co.dw\"\r\n    },\r\n    \"Tax\": 5,\r\n    \"Shipping\": 5,\r\n    \"clause\": {\r\n        \"overseas\": \"The shipment might take 5-10 more than informed.\"\r\n    },\r\n    \"paymentMethod\": \"Cash\"\r\n}";
        }
    }
}
