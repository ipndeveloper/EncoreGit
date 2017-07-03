using System;

namespace NetSteps.Web.Mvc.Controls.Models.Enrollment
{
	[Serializable]
	public class EnrollmentKitConfig
	{
		public virtual string HeaderText { get; set; }
		public virtual string HeaderTermName { get; set; }
		public virtual string HeaderCssClass { get; set; }
		public virtual string SKU { get; set; }
		public virtual string AccountTypeID { get; set; }
	}
}