using System.Collections.Generic;
using System.Linq;
using NetSteps.Common;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class UrlRedirectBusinessLogic : IUrlRedirectBusinessLogic
    {
        public IList<IUrlRedirect> GetUrlRedirects(IUrlRedirectRepository repository, IEnumerable<short> siteTypeIDs)
        {
            if (!siteTypeIDs.Any())
            {
                return new List<IUrlRedirect>();
            }

            return repository
                .Where(x => siteTypeIDs.Contains(x.SiteTypeID))
                .Cast<IUrlRedirect>()
                .ToList();
        }
    }
}