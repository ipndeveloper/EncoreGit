using System;
using System.Data.Objects;
using System.Linq;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class PageTypeRepository
    {
        protected override Func<NetStepsEntities, IQueryable<PageType>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<PageType>>(
                 (context) => context.PageTypes
                                        .Include("Layouts"));
            }
        }
    }
}
