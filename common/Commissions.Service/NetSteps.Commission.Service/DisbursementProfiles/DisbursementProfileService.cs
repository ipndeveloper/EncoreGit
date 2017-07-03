using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Service.Interfaces.Account;
using NetSteps.Commissions.Service.Interfaces.DisbursementProfile;
using NetSteps.Commissions.Common.Models;

namespace NetSteps.Commissions.Service.DisbursementProfiles
{
	public class DisbursementProfileService : IDisbursementProfileService
	{
		private readonly IDisbursementProfileProvider _provider;

		public DisbursementProfileService(IDisbursementProfileProvider provider)
		{
			_provider = provider;
		}

		public int GetDisbursementProfileCountByAccountAndDisbursementMethod(int accountId, DisbursementMethodKind disbursementMethod)
		{
			return GetDisbursementProfilesByAccountAndDisbursementMethod(accountId, disbursementMethod).Count();
		}
		
		public IDisbursementProfile SaveDisbursementProfile(IDisbursementProfile profile)
		{
			return _provider.SaveDisbursementProfile(profile);
		}
		
		public IEnumerable<IDisbursementProfile> GetDisbursementProfilesByAccountAndDisbursementMethod(int accountId, DisbursementMethodKind disbursementMethod)
		{
			return GetDisbursementProfilesByAccountId(accountId).Where(x => x.DisbursementMethod == disbursementMethod);
		}

		public IEnumerable<IDisbursementProfile> GetDisbursementProfilesByAccountId(int accountId)
		{
			return _provider.GetDisbursementsProfilesForAccount(accountId);
		}

		public int GetMaximumDisbursementProfiles(DisbursementMethodKind method)
		{
			return _provider.GetMaximumDisbursementProfiles(method);
		}


        public string GetDisbursementMethodCode(int disbursementMethodId)
        {
            return _provider.GetDisbursementMethodCode(disbursementMethodId);
        }
    }
}
