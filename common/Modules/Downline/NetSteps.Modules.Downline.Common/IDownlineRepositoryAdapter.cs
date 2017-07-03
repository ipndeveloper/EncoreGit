using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Modules.Downline.Common
{
	/// <summary>
	/// adapter for downline functions
	/// </summary>
	public interface IDownlineRepositoryAdapter
	{
		/// <summary>
		/// Search downline of a given sponsor for the given info.
		/// </summary>
		/// <param name="model">Search Downline model</param>
		/// <returns></returns>
		IEnumerable<IDownlineAccount> Search(ISearchDownlineModel model);
	}
}
