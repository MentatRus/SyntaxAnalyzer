using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Analyzer;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : Controller
    {
        private DocumentContext db;

        public DocumentsController(DocumentContext context)
        {
            db = context;
        }

//		private static List<Document> documents = new List<Document>
//		{
//			new Document("First doc", "First doc parsed"),
//			new Document("Second doc", "Second doc parsed")
//		};
        // GET api/documents
        [HttpGet]
        public JsonResult Get()
        {
            var documents = db.Documents.Include(x => x.Words).ThenInclude(x => x.PartOfSpeech).ToList();
            foreach (var document in documents)
            {
                document.Words = document.Words.OrderBy(x => x.Index).ToList();
            }

            return Json(documents);
        }

        // POST api/values
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(DocumentViewModel documentViewModel)
        {
            Document document = MainAnalyzer.ParseText(documentViewModel.Text, db);
            db.Documents.Add(document);


            await db.SaveChangesAsync();
            return RedirectToAction("Get");
        }

        public ActionResult<string> Index()
        {
            return "default";
        }
    }
}