using Dynamsoft.Core;
using Dynamsoft.CVR;
using Dynamsoft.DDN;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
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
                long ID = SaveImage(bytes);
                detectedDocument.ID = ID;
                detectedDocument.Success = true;
            }
            return detectedDocument;
        }

        [HttpPost("crop")]
        public ActionResult<Document> CropDocument(Document document)
        {
            Document detectedDocument = new Document();
            if (document.Base64 != null)
            {

                byte[] bytes = Convert.FromBase64String(document.Base64);
                CapturedResult result = cvr.Capture(bytes, PresetTemplate.PT_DETECT_DOCUMENT_BOUNDARIES);
                DetectedQuadsResult quads = result.GetDetectedQuadsResult();
                Console.WriteLine("length:" + quads.GetItems().Length);
                if (quads.GetItems().Length > 0)
                {
                    Polygon polygon = ConvertPolygon(quads.GetItems()[0].GetLocation());
                    detectedDocument.Polygon = polygon;
                }
                detectedDocument.Success = true;
            }
            return detectedDocument;
        }

        [HttpGet("{ID}")]
        public ActionResult<string> GetDocument(long ID)
        {
            string filePath = "./images/"+ID+".jpg";
            Response.ContentType = "text/plain";
            if (System.IO.File.Exists(filePath))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                string base64String = Convert.ToBase64String(bytes);
                Response.StatusCode = 200;
                return base64String;
            }
            else {
                Response.StatusCode = 404;
                return "not found";
            }
        }

        [HttpGet("cropped/{ID}")]
        public ActionResult<string> GetCroppedDocument(long ID)
        {
            string filePath = "./images/" + ID + "-cropped.jpg";
            Response.ContentType = "text/plain";
            if (System.IO.File.Exists(filePath))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                string base64String = Convert.ToBase64String(bytes);
                Response.StatusCode = 200;
                return base64String;
            }
            else
            {
                Response.StatusCode = 404;
                return "not found";
            }
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

        private long SaveImage(byte[] bytes) {
            if (Directory.Exists("images") == false) {
                Directory.CreateDirectory("images");
            }
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            long ID = dateTimeOffset.ToUnixTimeMilliseconds();
            string filePath = "./images/"+ID+".jpg";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return ID;
        }
    }
}
