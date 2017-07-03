using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class LayoutRepository
    {
        protected override Func<NetStepsEntities, IQueryable<Layout>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Layout>>(
                 (context) => context.Layouts.Include("HtmlSections"));
            }
        }

        public List<int> GetLayoutsForSite(int siteID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.Layouts.Where(l => l.Sites.Any(s => s.SiteID == siteID)).Select(l => l.LayoutID).ToList();
                }
            });
        }
    }
}
