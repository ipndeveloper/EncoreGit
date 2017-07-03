using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Web.Mvc.Extensions
{
	public static class DictionaryExtensions
	{
		public static object ToAJAXSearchResults(this Dictionary<int, string> results)
		{
			return results.Select(kvp => new { id = kvp.Key, text = kvp.Value });
		}

		public static object ToAJAXSearchResults(this Dictionary<string, string> results)
		{
			return results.Select(kvp => new { id = kvp.Key, text = kvp.Value });
		}
	}
}