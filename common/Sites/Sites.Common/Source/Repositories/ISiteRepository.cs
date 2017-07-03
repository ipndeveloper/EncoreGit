using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Sites.Common.Models;

namespace NetSteps.Sites.Common.Repositories
{
	/// <summary>
	/// Provides access to sites.
	/// </summary>
	[ContractClass(typeof(Contracts.SiteRepositoryContracts))]
	public interface ISiteRepository
	{
		/// <summary>
		/// Gets a site by ID or returns null if the site is not found.
		/// </summary>
		/// <param name="siteId">The site id.</param>
		/// <returns>The <see cref="ISite"/> or null.</returns>
		ISite GetSite(int siteId);

		/// <summary>
		/// Gets a site ID by URL or returns null if the site is not found.
		/// </summary>
		/// <param name="url">A properly formatted site URL.</param>
		/// <returns>The site ID or null.</returns>
		int? GetSiteId(string url);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(ISiteRepository))]
		internal abstract class SiteRepositoryContracts : ISiteRepository
		{
			ISite ISiteRepository.GetSite(int siteId)
			{
				Contract.Requires<ArgumentOutOfRangeException>(siteId > 0);
				throw new System.NotImplementedException();
			}

			int? ISiteRepository.GetSiteId(string url)
			{
				Contract.Requires<ArgumentNullException>(url != null);
				Contract.Requires<ArgumentException>(url.Length > 0);
				throw new System.NotImplementedException();
			}
		}
	}
}
