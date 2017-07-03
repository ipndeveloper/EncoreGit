using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public class UserSlimSearchData
	{
		[TermName("ID")]
		[Display(AutoGenerateField = false)]
		public int UserID { get; set; }

		public int UserTypeID { get; set; }

		[TermName("Username")]
		public string Username { get; set; }
	}
}
