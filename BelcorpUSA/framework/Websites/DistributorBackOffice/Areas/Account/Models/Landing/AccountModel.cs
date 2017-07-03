using System.Collections.Generic;

using DistributorBackOffice.Models.Home;

namespace DistributorBackOffice.Areas.Account.Models.Landing
{
	public class AccountModel
	{
		public NetSteps.Data.Entities.Account Account { get; set; }
		public List<AutoshipOrderViewModel> AutoshipOrders { get; set; }
        public NetSteps.Data.Entities.Account Sponsor { get; set; }
	}
}