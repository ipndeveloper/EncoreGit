using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.ProductCreditLedgerEntries
{
	public class AccountProductCreditLedgerEntryResolver : ICacheItemResolver<int, IEnumerable<int>>
	{
		public int AttemptCount
		{
			get { throw new NotImplementedException(); }
		}

		public int ResolveCount
		{
			get { throw new NotImplementedException(); }
		}

		public ResolutionKind TryResolve(int key, out IEnumerable<int> value)
		{
			throw new NotImplementedException();
		}
	}
}
