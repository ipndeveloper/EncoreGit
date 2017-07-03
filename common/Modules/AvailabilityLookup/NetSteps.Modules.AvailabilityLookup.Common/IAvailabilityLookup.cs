using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Modules.AvailabilityLookup.Common
{
	/// <summary>
	/// functions for looking up the availability of a host name
	/// </summary>
	[ContractClass(typeof(AvailabilityLookupContract))]
	public interface IAvailabilityLookup
	{
		/// <summary>
		/// Check availability of a host name
		/// </summary>
		/// <param name="hostName">host name to check</param>
		/// <returns>returns ILookupResult:Success = true if hostname found and AccountID</returns>
		ILookupResult Lookup(string hostName);
		
		/// <summary>
		/// Check availability of a host name in a given market
		/// </summary>
		/// <param name="MarketID"></param>
		/// <param name="hostName"></param>
		/// <returns>returns ILookupResult:Success = true if hostname found and AccountID</returns>
		ILookupResult Lookup(int MarketID, string hostName);
	}

	[ContractClassFor(typeof(IAvailabilityLookup))]
	internal abstract class AvailabilityLookupContract : IAvailabilityLookup
	{
		public ILookupResult Lookup(string hostName)
		{
			Contract.Requires<ArgumentNullException>(hostName != null);
			Contract.Requires<ArgumentException>(hostName.Length > 0);
			Contract.Ensures(Contract.Result<ILookupResult>() != null);
			
			throw new NotImplementedException();
		}

		public ILookupResult Lookup(int MarketID, string hostName)
		{
			Contract.Requires<ArgumentNullException>(hostName != null);
			Contract.Requires<ArgumentException>(hostName.Length > 0);
			Contract.Ensures(Contract.Result<ILookupResult>() != null);

			throw new NotImplementedException();
		}
	}
}
