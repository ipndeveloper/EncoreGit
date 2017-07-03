using System;

namespace nsCore.Models
{
	[Serializable]
	public class Term
	{
		public int? TermId { get; set; }

		public string TermName { get; set; }

		public string EnglishTerm { get; set; }

		public string LocalTerm { get; set; }

		public int LanguageId { get; set; }

		public DateTime? LastUpdated { get; set; }

		public bool IsOutOfDate { get; set; }
	}
}