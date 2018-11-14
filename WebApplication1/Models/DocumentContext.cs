using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using WebApplication1.Analyzer;

namespace WebApplication1.Models
{
	public class DocumentContext : DbContext
	{
		private static readonly string[] partsOfSpeech =
			{"Существительное", "Глагол", "Прилагательное", "Наречие"};
		private static readonly string[] membersOfSentence =
			{"Подлежащее", "Сказуемое", "Обстоятельство", "Дополнение", "Определение"};
		public DocumentContext(DbContextOptions<DocumentContext> options) : base(options)
		{
			Database.EnsureCreated();
			if (PartsOfSpeech.ToList().Count != partsOfSpeech.Length)
			{
				foreach (var pos in partsOfSpeech)
				{
					PartsOfSpeech.Add(new PartOfSpeech()
					{
						Name = pos
					});
				}
			}
			
			if (MembersOfSentence.ToList().Count != membersOfSentence.Length)
			{
				foreach (var mos in membersOfSentence)
				{
					MembersOfSentence.Add(new MemberOfSentence()
					{
						Name = mos
					});
				}
			}
			
			SaveChanges();

			if (Documents.ToList().Count == 0)
			{
				Documents.Add(MainAnalyzer.ParseText(
					"Негодяй, утверждающий, что не видит разницы между силой доллара и силой кнута, должен почувствовать эту разницу на собственной шкуре", this));
			}
			SaveChanges();
		}
		
		public DbSet<Document> Documents { get; set; }
		public DbSet<Word> Words { get; set; }
		public DbSet<PartOfSpeech> PartsOfSpeech { get; set; }
		public DbSet<MemberOfSentence> MembersOfSentence { get; set; }
	}
}