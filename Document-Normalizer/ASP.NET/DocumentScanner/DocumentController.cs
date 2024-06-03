using Dynamsoft.Core;
using Dynamsoft.CVR;
using Dynamsoft.DDN;
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
        [HttpPost("detect")]
        public ActionResult<Document> DetectDocument(Document document)
        {
            Document detectedDocument = new Document();
            if (document.Base64 != null) {

                byte[] bytes = Convert.FromBase64String(document.Base64);
                CapturedResult result = cvr.Capture(bytes, PresetTemplate.PT_DETECT_DOCUMENT_BOUNDARIES);
                DetectedQuadsResult quads = result.GetDetectedQuadsResult();
                Console.WriteLine("length:"+quads.GetItems().Length);
                if (quads.GetItems().Length > 0) {
                    Polygon polygon = ConvertPolygon(quads.GetItems()[0].GetLocation());
                    detectedDocument.Polygon = polygon;
                }
                detectedDocument.Success = true;
            }
            return detectedDocument;
        }

        [HttpGet]
        public ActionResult<string> GetDocument()
        {
            Response.StatusCode = 200;
            Response.ContentType = "text/plain";
            return "not supported";
        }

        private Polygon ConvertPolygon(Quadrilateral quad) {
            Polygon polygon = new Polygon();
            Point[] points = new Point[4];
            points[0] = new Point(quad.points[0][0], quad.points[0][1]);
            points[1] = new Point(quad.points[1][0], quad.points[1][1]);
            points[2] = new Point(quad.points[2][0], quad.points[2][1]);
            points[3] = new Point(quad.points[3][0], quad.points[3][1]);
            polygon.Points = points;
            return polygon;
        }
    }
}
