
namespace NetSteps.Content.Common
{
    /// <summary>
    /// Resolves terms.
    /// </summary>
    public interface ITermResolver
    {
        /// <summary>
        /// Gets the default language ID.
        /// </summary>
        int DefaultLanguageID { get; }
        /// <summary>
        /// Gets the default IETF langauge code.
        /// </summary>
        string DefaultIetfLanguageCode { get; }
        /// <summary>
        /// Translates a language ID to an IETF language code.
        /// </summary>
        /// <param name="langID">the id</param>
        /// <returns>the IETF language code</returns>
        string TranslateLanguageID(int langID);
        /// <summary>
        /// Translates an IETF language code to a language ID.
        /// </summary>
        /// <param name="ietfCode">an IETF language code</param>
        /// <returns>the language ID</returns>
        int TranslateIetfLanguageCode(string ietfCode);
		/// <summary>
		/// Gets a localized term.
		/// </summary>
		/// <param name="termID"></param>
		/// <param name="ietfLang"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
        string Term(string termID, string ietfLang, string defaultValue);
		/// <summary>
		/// Gets a localized term.
		/// </summary>
		/// <param name="termID"></param>
		/// <param name="ietfLang"></param>
		/// <param name="defaultValue"></param>
		/// <param name="args"></param>
		/// <returns></returns>
        string InterpolateTerm(string termID, string ietfLang, string defaultValue, params object[] args);
    }

	/// <summary>
	/// <see cref="ITermResolver"/> extension methods.
	/// </summary>
    public static class TermResolverExtensions
    {
		/// <summary>
		/// Gets a localized term in the default language.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="termID"></param>
		/// <returns></returns>
        public static string Term(this ITermResolver self, string termID) 
        {
            return self.Term(termID, self.DefaultIetfLanguageCode, "");
        }
		/// <summary>
		/// Gets a localized term in the default language.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="termID"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
        public static string Term(this ITermResolver self, string termID, string defaultValue)
        {
            return self.Term(termID, self.DefaultIetfLanguageCode, defaultValue);
        }
		/// <summary>
		/// Gets a localized term in the default language.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="termID"></param>
		/// <param name="args"></param>
		/// <returns></returns>
        public static string InterpolateTerm(this ITermResolver self, string termID, params object[] args) 
        {
            return self.InterpolateTerm(termID, self.DefaultIetfLanguageCode, "", args);
        }
    }
}
