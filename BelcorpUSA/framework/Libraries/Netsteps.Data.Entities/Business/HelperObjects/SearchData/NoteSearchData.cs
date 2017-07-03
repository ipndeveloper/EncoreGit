using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public struct NoteSearchData
	{
		[TermName("ID")]
		[Display(AutoGenerateField = false)]
		public int NoteID { get; set; }

		[TermName("Date")]
		public DateTime DateCreated { get; set; }

		[Display(AutoGenerateField = false)]
		public int NoteTypeID { get; set; }

		[TermName("Type")]
		[Display(Name = "Type")]
		[PropertyName("NoteType.TermName")]
		public string NoteType { get; set; }

		[Display(AutoGenerateField = false)]
		public int? UserID { get; set; }

		[TermName("Author")]
		public string Username { get; set; }

		[TermName("Subject")]
		public string Subject { get; set; }

		[TermName("Subject")]
		public string NoteText { get; set; }
	}
}
