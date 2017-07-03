using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Sites.Common.Models;

namespace NetSteps.Sites.Common.Repositories
{
	/// <summary>
	/// Provides access to site settings.
	/// </summary>
	[ContractClass(typeof(Contracts.SiteSettingRepositoryContracts))]
	public interface ISiteSettingRepository
	{
		/// <summary>
		/// Gets all available site settings (these are kinds, not values).
		/// </summary>
		/// <returns></returns>
		ICollection<ISiteSetting> GetAllKinds();

		/// <summary>
		/// Gets a key-value dictionary of settings for a site (including settings from the base site).
		/// </summary>
		/// <param name="siteId"></param>
		/// <returns></returns>
		IDictionary<string, string> GetSiteSettings(int siteId);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(ISiteSettingRepository))]
		internal abstract class SiteSettingRepositoryContracts : ISiteSettingRepository
		{
			ICollection<ISiteSetting> ISiteSettingRepository.GetAllKinds()
			{
				throw new NotImplementedException();
			}

			IDictionary<string, string> ISiteSettingRepository.GetSiteSettings(int siteId)
			{
				Contract.Requires<ArgumentOutOfRangeException>(siteId > 0);
				throw new NotImplementedException();
			}
		}
	}
}
