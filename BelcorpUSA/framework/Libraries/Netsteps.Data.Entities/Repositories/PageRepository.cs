using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class PageRepository
    {
        protected override Func<NetStepsEntities, IQueryable<Page>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Page>>(
                 (context) => context.Pages
                                        .Include("Translations")
                                        .Include("HtmlSections")
                                        .Include("HtmlSections.HtmlSectionContents.HtmlContent")
                                        .Include("HtmlSections.HtmlSectionContents.HtmlContent.HtmlElements")
                                        .Include("PageType")
                                        .Include("PageType.Layouts"));
            }
        }


        public override void Delete(Page obj)
        {
            Delete(obj.PageID);
        }

        // TODO: Test this methods - JHE
        public override void Delete(int primaryKey)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var obj = context.Pages
                                        .Include("Translations")
                                        .Include("HtmlSections")
                                        .Include("HtmlSections.HtmlSectionContents.HtmlContent")
                                        .Include("HtmlSections.HtmlSectionContents.HtmlContent.HtmlElements")
                                        .FirstOrDefault(pb => pb.PageID == primaryKey);

                    if (obj == null)
                        return;

                    obj.StartEntityTracking();

                    if (obj.HtmlSections != null)
                    {
                        foreach (var htmlSection in obj.HtmlSections)
                        {
                            if (htmlSection.HtmlSectionContents != null)
                            {
                                foreach (var htmlSectionContent in htmlSection.HtmlSectionContents)
                                {
                                    if (htmlSectionContent.HtmlContent != null)
                                    {
                                        if (htmlSectionContent.HtmlContent.HtmlElements != null)
                                        {
                                            foreach (var htmlElement in htmlSectionContent.HtmlContent.HtmlElements)
                                            {
                                                context.DeleteObject(htmlElement);
                                            }
                                        }
                                        context.DeleteObject(htmlSectionContent.HtmlContent);
                                    }
                                    context.DeleteObject(htmlSectionContent);
                                }
                            }
                            context.DeleteObject(htmlSection);
                        }
                    }

                    context.DeleteObjects(obj.Translations);

                    context.DeleteObject(obj);

                    Save(obj, context);
                }
            });
        }

        public Page PageWithTranslations(int pageID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var page = context.Pages.Include("Translations").FirstOrDefault(p => p.PageID == pageID);

                    return page;
                }
            });
        }

        public List<Page> PagesForSite(int siteID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var page = context.Pages.Include("Translations").Where(p => p.SiteID == siteID).ToList();

                    return page;
                }
            });
        }
    }
}
