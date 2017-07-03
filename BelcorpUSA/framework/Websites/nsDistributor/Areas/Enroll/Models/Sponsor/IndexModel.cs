using System.Web.Mvc;

namespace nsDistributor.Areas.Enroll.Models.Sponsor
{
    public class IndexModel
    {
        #region Resources
        public virtual MvcHtmlString SponsorPhotoHtml { get; set; }
        public virtual string SponsorFullName { get; set; }
        public virtual bool ShowBrowseLink { get; set; }
        public bool IsValidSponsor { get; set; }
        #endregion

        #region Infrastructure
        public virtual IndexModel LoadResources(
            MvcHtmlString sponsorPhotoHtml,
            string sponsorFullName,
            bool showBrowseLink)
        {
            this.SponsorPhotoHtml = sponsorPhotoHtml;
            this.SponsorFullName = sponsorFullName;
            this.ShowBrowseLink = showBrowseLink;
            this.IsValidSponsor = true;
            return this;
        }
        #endregion
    }
}