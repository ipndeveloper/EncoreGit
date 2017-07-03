using NetSteps.Data.Entities;

namespace NetSteps.Web.Mvc.Controls.Models
{
	public class PhotoUploaderModel
	{
		public string Folder { get; set; }
		public string Mode { get; set; }

		public HtmlContent Content { get; set; }
	}
}
