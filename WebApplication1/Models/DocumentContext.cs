using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
	public class DocumentContext : DbContext
	{
		public DocumentContext(DbContextOptions<DocumentContext> options) : base(options)
		{
			//Database.EnsureCreated();
		}
		
		public DbSet<Document> Documents { get; set; }
	}
}