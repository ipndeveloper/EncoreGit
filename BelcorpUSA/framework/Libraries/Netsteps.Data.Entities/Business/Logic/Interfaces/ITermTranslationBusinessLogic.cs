using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface ITermTranslationBusinessLogic
    {
        List<TermTranslation> LoadTermsByLanguageID(ITermTranslationRepository repository, int languageID);
        SqlUpdatableList<TermTranslation> LoadTermsByLanguageIDWithSqlDependency(ITermTranslationRepository repository, int languageID);
        TermTranslation LoadTermTranslationByTermNameAndLanguageID(ITermTranslationRepository repository, string termName, int languageID);
        List<Language> GetLanguages(ITermTranslationRepository repository, int languageID);
        Dictionary<int, string> GetTranslatedLanguages(bool all);
        Dictionary<int, string> GetTranslatedMarkets(bool all);
        Dictionary<int, string> GetTranslatedCountries(bool all);
        Dictionary<short, string> GetTranslatedAccountStatuses(bool all);
        Dictionary<short, string> GetTranslatedAccountTypes(bool all);
    }
}
