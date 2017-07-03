
namespace nsCore.Areas.Sites.Models
{
    public class PageModel
    {
        public int PageId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ExternalUrl { get; set; }
        public bool Active { get; set; }
        public string Keywords { get; set; }
        public int LayoutId { get; set; }

    }
}