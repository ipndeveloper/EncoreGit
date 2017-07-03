using System.Web.Mvc;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;

namespace NetSteps.Web.Mvc.Extensions
{
	public static class TermExtensions
	{
		public static string Term(this HtmlHelper helper, string termName)
		{
			return Term(helper, termName, termName);
		}

		public static string Term(this HtmlHelper helper, string termName, string defaultValue)
		{
			return Translation.GetTerm(termName, defaultValue);
		}

		/// <summary>
		/// Method created to handle term translations used in javascript methods ( ie. watermark, showError, etc.)
		/// This will allow special charaters from different languages to be displayed properly, as well as escaping 
		/// invalid ones.  As errors arise, we just need to add the replace statement below to escape.
		/// </summary>
		/// <param name="helper"></param>
		/// <param name="termName"></param>
		/// <param name="defaultValue"></param>
		/// <returns>Working string that can be used in Javascript functions</returns>
		public static MvcHtmlString JavascriptTerm(this HtmlHelper helper, string termName, string defaultValue, params object[] args)
		{
			var termVal = Translation.GetTerm(termName, defaultValue, args);

			return termVal.ToJavascriptSafeMvcHtmlString();
		}

		public static MvcHtmlString JavascriptTerm(this HtmlHelper helper, string termName)
		{
			return JavascriptTerm(helper, termName, termName);
		}

		public static string Term(this HtmlHelper helper, string termName, string defaultValue, params object[] args)
		{
			return Translation.GetTerm(ApplicationContext.Instance.CurrentLanguageID, termName, defaultValue, args);
		}
	}
}
