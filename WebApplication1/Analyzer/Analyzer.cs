using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Analyzer
{
	public class MainAnalyzer
	{
		public static Document ParseText(string input, DocumentContext db)
		{
			Document document = new Document();
			document.Raw = input;
			var partsOfSpeech = db.PartsOfSpeech.ToList();
			var membersOfSentence = db.MembersOfSentence.ToList();
			Regex nonAlphabet = new Regex("[^А-Яа-я ]");
			var words = nonAlphabet
				.Replace(input, " ")
				.Split(" ", StringSplitOptions.RemoveEmptyEntries)
				.Where(word => word.Length > 1);
			
			Random rnd = new Random();
			foreach (var rawWord in words)
			{
				Word word = new Word();
				word.Value = rawWord;
				word.MemberOfSentence = membersOfSentence[rnd.Next(membersOfSentence.Count)];
				word.PartOfSpeech = partsOfSpeech[rnd.Next(rnd.Next(partsOfSpeech.Count))];
				document.Words.Add(word);
			}

			return document;

		}
	}
}