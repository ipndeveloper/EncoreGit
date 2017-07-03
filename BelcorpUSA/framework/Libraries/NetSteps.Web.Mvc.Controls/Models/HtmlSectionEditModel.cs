namespace NetSteps.Web.Mvc.Controls.Models
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;


    using NetSteps.Common.Base;
    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Cache;
    using NetSteps.Data.Entities.Extensions;
    using NetSteps.Data.Entities.Generated;

    using Site = NetSteps.Data.Entities.Site;

    public class HtmlSectionEditModel
    {
        public HtmlSection Section;

        public Site Site;

        public Site BaseSite;

        public HtmlContent ProductionContent;

        public HtmlContent DraftContent;

        public HtmlContent WorkingDraft;

        public Common.Base.OrderedList<HtmlContent> ArchiveContentList;

        public List<HtmlContent> PushedContentList;

        public List<HtmlContent> SubmittedContentList;

        public Page Page;

        public CultureInfo Culture;

        public Common.Interfaces.IUser User;

        public bool IsEditor;

        public bool IsApprover;

        public bool IsPusher;


    }

    static public class HtmlSectionEditModelExtensions
    {
        static public void LoadModel(this HtmlSectionEditModel model, HtmlSection section, NetSteps.Common.Interfaces.IUser user, Site currentSite, int? pageId = null)
        {
            var baseSite = currentSite.IsBase ? currentSite : SiteCache.GetSiteByID((int)currentSite.BaseSiteID);

            model.Section = section;
            model.Site = currentSite;
            model.BaseSite = baseSite;
            model.ArchiveContentList = new OrderedList<HtmlContent>(section.AllContentByStatus(baseSite, ConstantsGenerated.HtmlContentStatus.Archive).OrderByDescending(hc => hc.PublishDate));
            model.ProductionContent = section.ProductionContentForDisplay(baseSite);
            model.DraftContent = section.ContentByStatus(baseSite, ConstantsGenerated.HtmlContentStatus.Draft);
            model.WorkingDraft = section.WorkingDraft(baseSite);
            model.PushedContentList = section.AllContentByStatus(baseSite, ConstantsGenerated.HtmlContentStatus.Pushed);
            model.SubmittedContentList = section.AllContentByStatus(baseSite, ConstantsGenerated.HtmlContentStatus.Submitted);
            model.Page = baseSite.Pages.FirstOrDefault(p => pageId != null && p.PageID == pageId.Value);
            model.Culture = SmallCollectionCache.Instance.Languages.GetById(ApplicationContext.Instance.CurrentLanguageID).Culture;
            model.User = user;
            model.IsEditor = user.HasFunction("CMS-Content Editing");
            model.IsApprover = user.HasFunction("CMS-Content Approving");
            model.IsPusher = user.HasFunction("CMS-Content Pushing");
        }
    }   
}