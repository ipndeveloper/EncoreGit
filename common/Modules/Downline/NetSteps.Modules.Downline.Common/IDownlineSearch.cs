using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Modules.Downline.Common
{
	/// <summary>
	/// Module for searching downline
	/// </summary>
	public interface IDownlineSearch
	{
		/// <summary>
		/// Search downline of a given sponsor for the given information.
		/// </summary>
		/// <param name="model">Search Downline model</param>
		/// <returns></returns>
		IDownlineSearchResult Search(ISearchDownlineModel model);
	}
}
