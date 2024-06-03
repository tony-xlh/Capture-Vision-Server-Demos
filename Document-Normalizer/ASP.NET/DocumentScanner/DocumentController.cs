using Dynamsoft.CVR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace DocumentScanner
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private CaptureVisionRouter cvr = new CaptureVisionRouter();
        [HttpPost]
        public async Task<ActionResult<Document>> ProcessDocument(Document document)
        {
            Console.WriteLine(document.Base64);
            Document detectedDocument = new Document();
            return detectedDocument;
        }

        [HttpGet]
        public ActionResult<string> GetDocument()
        {
            Response.StatusCode = 200;
            Response.ContentType = "text/plain";
            return "not supported";
        }
    }
}
