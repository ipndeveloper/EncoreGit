using NetSteps.Commissions.Common.Models;
using System.Collections.Generic;

namespace NetSteps.Commissions.Service.Interfaces.DisbursementProfile
{
	public interface IDisbursementProfileResolver
	{
        IDisbursementProfile Resolve(int profileId, IAccount account, int addressId, int currencyId,
	        DisbursementMethodKind disbursementMethod, bool useAddressOfRecord, bool agreementOnFile,
	        IEFTAccount eftAccount);

        IEnumerable<IDisbursementProfile> Resolve(int profileId, IAccount account, int addressId, int currencyId,
	        DisbursementMethodKind disbursementMethod, bool useAddressOfRecord, bool agreementOnFile,
	        IEnumerable<IEFTAccount> eftAccounts);
	}
}
