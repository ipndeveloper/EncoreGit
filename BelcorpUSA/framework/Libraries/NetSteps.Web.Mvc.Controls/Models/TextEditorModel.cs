
namespace NetSteps.Web.Mvc.Controls.Models
{
    public class TextEditorModel
    {
        public string InstanceName { get; set; }
        public string ContentBody { get; set; }
        public string ContentName { get; set; }

        public int HtmlSectionID { get; set; }

        public bool IsRichText { get; set; }
        public bool ShowName { get; set; }
        public bool ShowTabbedHeader { get; set; }

        public bool ShowPreviewLink { get; set; }
        public bool ShowMediaLibraryLink { get; set; }
    }
}
