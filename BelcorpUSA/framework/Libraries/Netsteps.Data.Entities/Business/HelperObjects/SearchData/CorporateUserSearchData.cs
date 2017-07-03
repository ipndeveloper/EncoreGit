using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public class CorporateUserSearchData
	{
		[TermName("ID")]
		[Display(AutoGenerateField = false)]
		public int CorporateUserID { get; set; }

		[Display(AutoGenerateField = false)]
		public int UserID { get; set; }

		[TermName("Name")]
		[Display(Name = "Name")]
		public string FullName { get; set; }

		[TermName("Username")]
		public string Username { get; set; }

		[TermName("Email")]
		public string Email { get; set; }

		[TermName("Role")]
		public string Role { get; set; }

		[TermName("Status")]
		public string Status { get; set; }

		[TermName("LastLogin", "Last Login")]
		public DateTime? LastLogin { get; set; }
	}
}
