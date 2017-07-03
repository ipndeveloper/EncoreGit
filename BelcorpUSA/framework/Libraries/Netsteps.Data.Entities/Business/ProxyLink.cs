using System.Collections.Generic;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities
{
	public partial class ProxyLink
	{
		#region Methods
        public static IList<ProxyLinkData> GetAccountProxyLinks(
            int accountID,
            short accountTypeID)
        {
            return BusinessLogic.GetAccountProxyLinks(
                accountID,
                accountTypeID
            );
        }
		#endregion
	}
}
