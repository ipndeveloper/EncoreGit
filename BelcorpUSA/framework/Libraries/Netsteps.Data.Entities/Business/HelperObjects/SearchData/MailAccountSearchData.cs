using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public class MailAccountSearchData
	{
		[TermName("ID")]
		[Display(AutoGenerateField = false)]
		public int MailAccountID { get; set; }

		[TermName("ID")]
		[Display(AutoGenerateField = false)]
		public int AccountID { get; set; }

		[TermName("Email")]
        [Display(AutoGenerateField = false)]
        public string EmailAddress { get; set; }
	}
}
