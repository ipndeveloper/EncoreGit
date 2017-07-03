using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Common;

namespace NetSteps.Data.Entities
{
    public partial class UrlRedirect : IUrlRedirect
    {
        public static IList<IUrlRedirect> GetUrlRedirects(IEnumerable<short> siteTypeIDs)
        {
            Contract.Requires<ArgumentNullException>(siteTypeIDs != null);
            return BusinessLogic.GetUrlRedirects(Repository, siteTypeIDs);
        }
    }
}
