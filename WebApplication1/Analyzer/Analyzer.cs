using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace WebApplication1.Analyzer
{
	public class MainAnalyzer
	{
		public static object ParseText(string input)
		{
			var sentenceMembers = new List<string> {"Подлежащее", "Сказуемое", "Обстоятельство", "Дополнение"};
			var partsOfSpeech = new List<string> {"Существитетельное", "Прилагательное", "Глагол", "Наречие"};
			Thread.Sleep(1000);//Имитирует долгие вычисления
			Random rnd = new Random();
			var sentence = input.Split(" ").Select(x => new
			{
				word = x,
				sentence_member = sentenceMembers[rnd.Next(4)],
				part_of_speech = partsOfSpeech[rnd.Next(4)]
			});
			return JsonConvert.SerializeObject(sentence);

		}
	}
}