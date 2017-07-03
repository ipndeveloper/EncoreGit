
namespace NetSteps.Web.Mvc.Controls.Models
{
	public class MediaLibraryModel
	{
		public string WebBaseUrl { get; set; }
		public string SystemBaseUrl { get; set; }
		private string _uploadUrl = "~/Edit/UploadFile";
		public string UploadUrl { get { return _uploadUrl; } set { _uploadUrl = value; } }
		public bool AllowImageInsert { get; set; }
		public bool GenerateSelectButtons { get; set; }
	}
}
