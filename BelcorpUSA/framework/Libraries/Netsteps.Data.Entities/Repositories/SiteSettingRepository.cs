using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Sites.Common.Models;
using NSCR = NetSteps.Sites.Common.Repositories;

namespace NetSteps.Data.Entities.Repositories
{
	/// <summary>
	/// Provides access to site settings.
	/// </summary>
	public partial class SiteSettingRepository : NSCR.ISiteSettingRepository
	{
		/// <summary>
		/// Gets all available site settings (these are kinds, not values).
		/// </summary>
		public virtual ICollection<ISiteSetting> GetAllKinds()
		{
			return LoadAll()
				.Cast<ISiteSetting>()
				.ToArray();
		}

		/// <summary>
		/// Gets a key-value dictionary of settings for a site (including settings from the base site).
		/// </summary>
		public virtual IDictionary<string, string> GetSiteSettings(int siteId)
		{
			using (var context = CreateContext())
			{
				var siteValues = GetIndividualSiteSettings(siteId, context);

				// Determine if there is a base site.
				var baseSiteId = context.Sites
					.Where(x => x.SiteID == siteId)
					.Select(x => x.BaseSiteID)
					.FirstOrDefault();

				// Add settings from base site.
				// Union() will give preference to the first sequence, so this will only add settings
				// from the base site that haven't already been defined on the child site.
				if (baseSiteId.HasValue)
				{
					siteValues = siteValues.UnionBy(
						GetIndividualSiteSettings(baseSiteId.Value, context),
						x => x.Key
					);
				}

				return siteValues.ToDictionary(x => x.Key, x => x.Value);
			}
		}

		/// <summary>
		/// Returns settings for a single site.
		/// </summary>
		protected virtual IEnumerable<KeyValuePair<string, string>> GetIndividualSiteSettings(int siteId, NetStepsEntities context)
		{
			return context.SiteSettingValues
				.Where(v => v.SiteID == siteId)
				.Join(context.SiteSettings, v => v.SiteSettingID, k => k.SiteSettingID, (v, k) => new
				{
					k.Name,
					v.Value
				})
				.ToArray()
				.Select(x => new KeyValuePair<string, string>(x.Name, x.Value));
		}
	}
}