namespace WebApplication1.Models
{
	public class Word
	{
		public int Id { get; set; }
		public string Value { get; set; }
		public PartOfSpeech PartOfSpeech { get; set; }
		public MemberOfSentence MemberOfSentence { get; set; }
	}
}