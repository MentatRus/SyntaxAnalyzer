using System;
using WebApplication1.Analyzer;
namespace WebApplication1.Models
{
	public class Document
	{
		public int Id { get; set; }
		public string Raw { get; set; }
		public string Parsed { get; set; }

		public Document()
		{
			
		}

		public Document(string value)
		{
			Raw = value;
			Parsed = MainAnalyzer.ParseText(value);
		}
	}
}