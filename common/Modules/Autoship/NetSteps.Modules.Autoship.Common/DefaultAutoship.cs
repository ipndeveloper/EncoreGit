using System;
using System.Linq;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Autoship.Common;

namespace NetSteps.Modules.Autoship.Common
{
	/// <summary>
	/// Default implementation of IAutoship
	/// </summary>
	[ContainerRegister(typeof(IAutoship), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class DefaultAutoship : IAutoship
	{
		private IAutoshipRepositoryAdapter _repo;

		/// <summary>
		/// Default constuctor
		/// </summary>
		public DefaultAutoship()
			: this(Create.New<IAutoshipRepositoryAdapter>())
		{ }

		/// <summary>
		/// Create a default autoship with a provided adapter.
		/// </summary>
		/// <param name="repo"></param>
		public DefaultAutoship(IAutoshipRepositoryAdapter repo)
		{
			_repo = repo ?? Create.New<IAutoshipRepositoryAdapter>();
		}

		/// <summary>
		/// Search for autoships of a specific schedule for a given account.
		/// </summary>
		/// <param name="accountID"></param>
		/// <param name="autoshipScheduleID"></param>
		/// <param name="ActiveAutoshipsOnly">ActiveAutoshipsOnly</param>
		/// <returns></returns>
		public IAutoshipSearchResult Search(int accountID, int? autoshipScheduleID, bool ActiveAutoshipsOnly = true)
		{
			Contract.Requires<ArgumentException>(accountID != 0 || autoshipScheduleID != 0);

			var result = Create.New<IAutoshipSearchResult>();
			result.Success = false;
			var site = _repo.SearchByAccount(accountID, autoshipScheduleID, ActiveAutoshipsOnly);

			if (site != null && site.Count > 0)
			{
				result.Autoships = site;
				result.Success = true;
			}

			return result;
		}

		/// <summary>
		/// Cancel an autoship for the given account.
		/// </summary>
		/// <param name="accountID"></param>
		/// <param name="autoshipID"></param>
		/// <returns></returns>
		public IAutoshipCancelResult Cancel(int accountID, int autoshipID)
		{
			Contract.Requires<ArgumentNullException>(accountID != 0 || autoshipID != 0);
			var result = Create.New<IAutoshipCancelResult>();
			result.Success = false;
			var site = _repo.CancelByAccountIDAndAutoshipID(accountID, autoshipID);

			if (site != null)
			{
				result.AccountID = site.AccountID;
				result.AutoshipID = site.OrderID;
				result.Success = true;
			}

			return result;
		}
	}
}
