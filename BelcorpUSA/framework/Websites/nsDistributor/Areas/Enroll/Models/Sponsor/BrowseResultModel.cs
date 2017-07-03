using System.Web;

namespace nsDistributor.Areas.Enroll.Models.Sponsor
{
    public class BrowseResultModel
    {
        #region Resources
        public virtual HtmlString PhotoHtml { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Location { get; set; }
        public virtual string SiteUrl { get; set; }
        public virtual string SelectUrl { get; set; }
        #endregion
    }
}