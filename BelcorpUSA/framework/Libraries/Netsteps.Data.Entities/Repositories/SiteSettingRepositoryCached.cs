using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Core.Cache;
using NetSteps.Sites.Common.Models;

namespace NetSteps.Data.Entities.Repositories
{
	/// <summary>
	/// Cached implementation of <see cref="SiteSettingRepository"/>.
	/// </summary>
	public class SiteSettingRepositoryCached : SiteSettingRepository
	{
		protected readonly ICache<int, ICollection<ISiteSetting>> _siteSettingKindsCache;
		protected readonly ICache<int, IDictionary<string, string>> _siteSettingsCache;

		public SiteSettingRepositoryCached(
			ICache<int, ICollection<ISiteSetting>> siteSettingKindsCache,
			ICache<int, IDictionary<string, string>> siteSettingsCache)
		{
			Contract.Requires<ArgumentNullException>(siteSettingKindsCache != null);
			Contract.Requires<ArgumentNullException>(siteSettingsCache != null);

			_siteSettingKindsCache = siteSettingKindsCache;
			_siteSettingsCache = siteSettingsCache;
		}

		/// <summary>
		/// Cached!
		/// </summary>
		public override ICollection<ISiteSetting> GetAllKinds()
		{
			return _siteSettingKindsCache.GetOrAdd(0, key => base.GetAllKinds());
		}

		/// <summary>
		/// Cached!
		/// </summary>
		public override IDictionary<string, string> GetSiteSettings(int siteId)
		{
			return _siteSettingsCache.GetOrAdd(siteId, base.GetSiteSettings);
		}
	}
}
