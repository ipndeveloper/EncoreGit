using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Modules.Downline.Common;
using NetSteps.Encore.Core.IoC;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;

namespace NetSteps.Modules.Downline.Common
{
	/// <summary>
	/// Default Implementation of IDownlineSearch
	/// </summary>
	[ContainerRegister(typeof(IDownlineSearch), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class DefaultDownlineSearch : IDownlineSearch
	{
		private IDownlineRepositoryAdapter _repo;
		/// <summary>
		/// Default Constructor
		/// </summary>
		public DefaultDownlineSearch()
			: this(Create.New<IDownlineRepositoryAdapter>())
		{
		}

		/// <summary>
		/// Constructor with adapter
		/// </summary>
		/// <param name="repo"></param>
		public DefaultDownlineSearch(IDownlineRepositoryAdapter repo)
		{
			_repo = repo ?? Create.New<IDownlineRepositoryAdapter>();
		}

		/// <summary>
		/// Search downline of a given sponsor for the given info.
		/// </summary>
		/// <param name="model">Search Downline model</param>
		/// <returns></returns>
		public IDownlineSearchResult Search(ISearchDownlineModel model)
		{
			var result = Create.New<IDownlineSearchResult>();
			result.DownlineAccounts = new List<IDownlineAccount>();

			var matchingItems = _repo.Search(model);

			if (matchingItems.Count() > 0)
			{
				result.DownlineAccounts = matchingItems.ToList();
				result.Success = true;
			}

			return result;
		}
	}
}
