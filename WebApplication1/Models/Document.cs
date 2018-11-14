using System;
using System.Collections.Generic;
using WebApplication1.Analyzer;
namespace WebApplication1.Models
{
	public class Document
	{
		public int Id { get; set; }
		public string Raw { get; set; }
		public ICollection<Word> Words { get; set; }
		public Document()
		{
			Words = new List<Word>();
		}

	}
}