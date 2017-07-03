using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.AvailabilityLookup.Common;

namespace NetSteps.Modules.AvailabilityLookup.Common
{
	/// <summary>
	/// Default implementation of IAvailabilityLookup
	/// </summary>
	[ContainerRegister(typeof(IAvailabilityLookup), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class DefaultAvailabilityLookup : IAvailabilityLookup
	{
		private ISiteRepositoryAdapter _repo;
		/// <summary>
		/// Default Constructor
		/// </summary>
		public DefaultAvailabilityLookup()
			: this(Create.New<ISiteRepositoryAdapter>())
		{
			Contract.Ensures(_repo != null);
		}

		/// <summary>
		/// Constructor with adapter
		/// </summary>
		/// <param name="repo"></param>
		public DefaultAvailabilityLookup(ISiteRepositoryAdapter repo)
		{
			Contract.Ensures(_repo != null);
			if (repo == null)
				_repo = Create.New<ISiteRepositoryAdapter>();
			else
				_repo = repo;
		}

		/// <summary>
		/// Check availability of a host name
		/// </summary>
		/// <param name="hostName">host name to check</param>
		/// <returns>returns ILookupResult:Success = true if hostname found and AccountID</returns>
		public ILookupResult Lookup(string hostName)
		{
			Contract.Assume(this._repo != null);

			var result = Create.New<ILookupResult>();

			result.Success = false;

			var site = _repo.LoadByUrl(hostName);

			if (site != null)
			{
				result.AccountID = site.AccountID;
				result.Success = true;
			}

			return result;
		}

		/// <summary>
		/// Check availability of a host name in a given market
		/// </summary>
		/// <param name="marketID"></param>
		/// <param name="hostName"></param>
		/// <returns>returns ILookupResult:Success = true if hostname found and AccountID</returns>
		public ILookupResult Lookup(int marketID, string hostName)
		{
			Contract.Assume(this._repo != null);

			var result = Create.New<ILookupResult>();

			result.Success = false;

			var site = _repo.LoadByMarketAndUrl(marketID, hostName);
			if (site != null)
			{
				result.AccountID = site.AccountID;
				result.Success = true;
			}

			return result;
		}
	}
}
