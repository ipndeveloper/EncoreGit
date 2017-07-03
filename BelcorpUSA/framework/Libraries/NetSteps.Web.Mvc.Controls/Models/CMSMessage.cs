using System;

namespace NetSteps.Web.Mvc.Controls.Models
{
	public enum CMSEnvironment
	{
		WorkingDraft,
		Pushed,
		PendingApproval
	}

	public class CMSMessage
	{
		public int? HtmlContentHistoryID { get; set; }
		public int? FromUserID { get; set; }
		public DateTime? Date { get; set; }
		public string Message { get; set; }
		public CMSEnvironment Environment { get; set; }
		public int HtmlSectionID { get; set; }
		public string ReturnUrl { get; set; }
	}
}
