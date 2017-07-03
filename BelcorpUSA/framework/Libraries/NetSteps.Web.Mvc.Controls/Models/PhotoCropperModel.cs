using NetSteps.Data.Entities;

namespace NetSteps.Web.Mvc.Controls.Models
{
	public class PhotoCropperModel
	{
		public int TargetWidth { get; set; }
		public int TargetHeight { get; set; }

		public int OriginalWidth { get; set; }
		public int OriginalHeight { get; set; }

		public int? BoxWidth { get; set; }
		public int? BoxHeight { get; set; }

		public string Folder { get; set; }
		public string Mode { get; set; }

		public string UploadButtonTermName { get; set; }
		public string UploadButtonTerm { get; set; }

		public HtmlContent Content { get; set; }

        private string _loadingImage = "~/Resource/Content/Images/loading.gif";
		public string LoadingImage { get { return _loadingImage; } set { _loadingImage = value; } }
	}
}
