using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public class MailMessageSearchData
	{
		[TermName("ID")]
		[Display(AutoGenerateField = false)]
		public int MailMessageID { get; set; }

		[Display(AutoGenerateField = false)]
		public int? MailAccountID { get; set; }

		[TermName("Subject")]
		public string Subject { get; set; }

		[TermName("Body")]
		public string Body { get; set; }

		[TermName("HTMLBody", "Html Body")]
		public string HTMLBody { get; set; }

		[TermName("Sent")]
		public DateTime DateAdded { get; set; }

		[TermName("BeenRead")]
		[Display(AutoGenerateField = false)]
		public bool BeenRead { get; set; }

		[Display(AutoGenerateField = false)]
		public short MailFolderTypeID { get; set; }

		[TermName("Folder")]
		[Display(Name = "Folder")]
		[PropertyName("MailFolderTypeID")]
		public string MailFolderType { get; set; }

        [TermName("From")]
        [Display(AutoGenerateField = false)]
        public string From { get; set; }

	}
}
