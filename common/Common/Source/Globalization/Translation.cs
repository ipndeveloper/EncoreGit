using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Common.Globalization
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Default implementation of ITermTranslationProvider which just returns the default value.
	/// Created: 08-18-2010
	/// </summary>
	public class Translation
	{
		private static ITermTranslationProvider termTranslation;
		public static ITermTranslationProvider TermTranslation
		{
			get
			{
				if (termTranslation == null)
					termTranslation = Create.New<ITermTranslationProvider>();
				return termTranslation;
			}
			set // created setter because Global.asax.cs was throwing an error
			{
				termTranslation = value;
			}
		}

		public static string GetTerm(string termName, string defaultValue = "")
		{
			return TermTranslation.GetTerm(termName, defaultValue.IsNullOrEmpty() ? termName : defaultValue);
		}

		public static string GetTerm(string termName, string defaultValue, params object[] args)
		{
			return string.Format(TermTranslation.GetTerm(termName, defaultValue), args);
		}

		public static string GetTerm(int languageId, string termName, string defaultValue = "")
		{
			return TermTranslation.GetTerm(languageId, termName, defaultValue);
		}

		public static string GetTerm(int languageId, string termName, object[] args, string defaultValue = "")
		{
			return string.Format(TermTranslation.GetTerm(languageId, termName, defaultValue), args);
		}

		public static string GetTerm(int languageId, string termName, string defaultValue, params object[] args)
		{
			return string.Format(TermTranslation.GetTerm(languageId, termName, defaultValue), args);
		}
	}
}