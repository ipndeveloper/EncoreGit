using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Addresses.Common.Models;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Web.Mvc.Extensions
{
	public static class StringExtensions
	{
		public static MvcHtmlString ToMvcHtmlString(this string value)
		{
			return MvcHtmlString.Create(value);
		}

		public static string DropdownForStateProvince(this IEnumerable<StateProvince> stateProvinces, int countryId, IAddress address, bool isoUseName)
		{
			var stateBuilder = new StringBuilder();

			foreach (StateProvince state in SmallCollectionCache.Instance.StateProvinces.Where(s => s.CountryID == countryId).OrderBy(s => s.StateAbbreviation))
			{
				string text = isoUseName ? state.Name : state.StateAbbreviation;

				stateBuilder.Append("<option value=\"").Append(state.StateProvinceID).Append("\"").Append(address.StateProvinceID == state.StateProvinceID ? " selected=\"selected\"" : "").Append(">").Append(text).Append("</option>");
			}

			return stateBuilder.ToString();
		}

		public static MvcHtmlString ToJavascriptSafeMvcHtmlString(this string value)
		{
			// We need to escape the \ characters
			value = value.Replace("'", "\\'");

			// Whitespace causes issues in javascript
			value = value.Trim();

			return MvcHtmlString.Create(value);
		}

		public static string ToJavascriptSafeString(this string value)
		{
			return value.ToJavascriptSafeMvcHtmlString().ToString();
		}
	}
}
