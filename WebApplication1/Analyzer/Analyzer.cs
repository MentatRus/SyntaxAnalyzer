using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using CsQuery;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
			Regex nonAlphabet = new Regex("[^А-Яа-я ]");
			var words = nonAlphabet
				.Replace(input, " ")
				.Split(" ", StringSplitOptions.RemoveEmptyEntries)
				.Where(word => word.Length > 1);
			
			Random rnd = new Random();
			int index = 0;
			foreach (var rawWord in words)
			{
				string partOfSpeechString = MorphologyRequester.GetPartOfSpeech(rawWord);
				partOfSpeechString = char.ToUpper(partOfSpeechString[0]) + partOfSpeechString.Substring(1).ToLower();
				partOfSpeechString = partOfSpeechString.Replace("<table", "");
				partOfSpeechString = partOfSpeechString.Replace("<br", "");

				if (partOfSpeechString == "Кр.")
				{
					partOfSpeechString = "Краткое прилагательное";
				}
				if (partOfSpeechString == "Инфинитив")
				{
					partOfSpeechString = "Глагол";
				}
				var partOfSpeech = db.PartsOfSpeech.FirstOrDefault(x => x.Name == partOfSpeechString);
				if (partOfSpeech == null)
				{
					partOfSpeech = new PartOfSpeech()
					{
						Name = partOfSpeechString
					};
					db.PartsOfSpeech.Add(partOfSpeech);
					//db.SaveChanges();
				}
				Word word = new Word();

				string memberOfSentenceName = null;
				if (partOfSpeech.Name == "Существительное")
				{
					if (!document.Words.Any(x => x.MemberOfSentence.Name == "Подлежащее"))
					{
						memberOfSentenceName = "Подлежащее";
					}
					else
					{
						memberOfSentenceName = "Дополнение";
					}
				}

				if (partOfSpeech.Name == "Прилагательное")
				{
					memberOfSentenceName = "Определение";
				}

				if (partOfSpeech.Name == "Глагол")
				{
					 var verbRegex = new Regex(@"(т|ла?|ь|я|ти|ли)$");
					var matches = verbRegex.Matches(rawWord);
					if (matches.Count > 0)
					{
						memberOfSentenceName = "Сказуемое";
					}
					else
					{
						memberOfSentenceName = "Определение";
					}
				}
				if (partOfSpeech.Name == "Краткое прилагательное")
				{
					memberOfSentenceName = "Сказуемое";
				}
				if (partOfSpeech.Name == "Наречие")
				{
					memberOfSentenceName = "Обстоятельство";
				}

				if (memberOfSentenceName == null)
				{
					memberOfSentenceName = "Другое";
				}
				
				var memberOfSentence = db.MembersOfSentence.FirstOrDefault(x => x.Name == memberOfSentenceName);
				if (memberOfSentence == null)
				{
					memberOfSentence = new MemberOfSentence()
					{
						Name = memberOfSentenceName
					};
					db.MembersOfSentence.Add(memberOfSentence);
					//db.SaveChanges();
				}
				
				word.Value = rawWord;
				word.MemberOfSentence = memberOfSentence;
				word.PartOfSpeech = partOfSpeech;
				word.Index = index;
				document.Words.Add(word);
				index++;
			}

			return document;

		}
	}

	class MorphologyRequester
	{

		private static string GetUrl(string word)
		{
			return $"http://www.morfologija.ru/словоформа/{word}";
		}
		
		private static string Download(string url)
		{
			using (WebClient client = new WebClient())
			{
				string downloadString = client.DownloadString(url);
				return downloadString;
			}
		}

		private static string ParsePartOfSpeechString(string htmlText)
		{
			int startPostition = htmlText.IndexOf(@"<b>Часть речи:</b>")+ @"<b>Часть речи:</b>".Length + 2;
			int endPostition = htmlText.IndexOf(" ", startPostition + 1);
			string partOfSpeech = htmlText.Substring(startPostition - 1, endPostition - startPostition + 1);
			return partOfSpeech;
		}

		public static string GetPartOfSpeech(string word)
		{
			string htmlText = Download(GetUrl(word));
			return ParsePartOfSpeechString(htmlText);
		}
		
	}
}