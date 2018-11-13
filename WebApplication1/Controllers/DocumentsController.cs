using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DocumentsController: Controller
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
		// GET api/values
		[HttpGet]
		public JsonResult Get()
		{
			return Json(db.Documents.ToList());
		}
		
		// POST api/values
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] string value)
		{
			db.Documents.Add(new Document(value));
			await db.SaveChangesAsync();
			return RedirectToAction("Get");
		}

		public ActionResult<string> Index()
		{
			return "default";
		}
	}
}