using System.Collections.Generic;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface ITermTranslationRepository
	{
		List<TermTranslation> LoadTermsByLanguageID(int languageID);
		SqlUpdatableList<TermTranslation> LoadTermsByLanguageIDWithSqlDependency(int languageID);
		string LoadTermByTermNameAndLanguageID(string termName, int languageID);
		TermTranslation LoadTermTranslationByTermNameAndLanguageID(string termName, int languageID);
		List<Language> LoadTermsLanguageIDs(int languageID);
	}
}
