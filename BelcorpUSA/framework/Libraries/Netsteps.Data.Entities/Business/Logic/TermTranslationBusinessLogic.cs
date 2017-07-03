using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class TermTranslationBusinessLogic
    {
        public virtual List<TermTranslation> LoadTermsByLanguageID(ITermTranslationRepository repository, int languageID)
        {
            try
            {
                var list = repository.LoadTermsByLanguageID(languageID);
                list.Each(item =>
                {
                    item.StartTracking();
                    item.IsLazyLoadingEnabled = true;
                });
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual SqlUpdatableList<TermTranslation> LoadTermsByLanguageIDWithSqlDependency(ITermTranslationRepository repository, int languageID)
        {
            try
            {
                var list = repository.LoadTermsByLanguageIDWithSqlDependency(languageID);
                list.Each(item =>
                {
                    item.StartTracking();
                    item.IsLazyLoadingEnabled = true;
                });
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual TermTranslation LoadTermTranslationByTermNameAndLanguageID(ITermTranslationRepository repository, string termName, int languageID)
        {
            try
            {
                TermTranslation term = repository.LoadTermTranslationByTermNameAndLanguageID(termName, languageID);
                term.StartEntityTracking();
                term.IsLazyLoadingEnabled = true;
                return term;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual List<Language> GetLanguages(ITermTranslationRepository repository, int languageID)
        {
            try
            {
                var list = repository.LoadTermsLanguageIDs(languageID);
                foreach (var item in list)
                {
                    item.StartTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual Dictionary<int, string> GetTranslatedLanguages(bool all)
        {
            try
            {
                Language language = SmallCollectionCache.Instance.Languages.GetById(ApplicationContext.Instance.CurrentLanguageID);

                var currentCulture = language != null ? new CultureInfo(language.CultureInfo) : CultureInfo.CurrentCulture;

                var list = SmallCollectionCache.Instance.Languages
                    .Where(l => l.Active || l.Active == !all)
                    .ToDictionary(sp => sp.LanguageID, sp => sp.GetTerm(language.LanguageID))
                    .OrderBy(x => x.Value, StringComparer.Create(currentCulture ?? CultureInfo.CurrentCulture, false))
                    .ToDictionary(x => x.Key, x => x.Value);

                return list;

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual Dictionary<int, string> GetTranslatedCountries(bool all)
        {
            try
            {
                Language language = SmallCollectionCache.Instance.Languages.GetById(ApplicationContext.Instance.CurrentLanguageID);

                var currentCulture = language != null ? new CultureInfo(language.CultureInfo) : CultureInfo.CurrentCulture;

                var list = SmallCollectionCache.Instance.Countries
                    .Where(l => l.Active || l.Active == !all)
                    .ToDictionary(sp => sp.CountryID, sp => sp.GetTerm(language.LanguageID))
                    .OrderBy(x => x.Value, StringComparer.Create(currentCulture ?? CultureInfo.CurrentCulture, false))
                    .ToDictionary(x => x.Key, x => x.Value);

                return list;

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual Dictionary<int, string> GetTranslatedMarkets(bool all)
        {
            try
            {
                Language language = SmallCollectionCache.Instance.Languages.GetById(ApplicationContext.Instance.CurrentLanguageID);

                var currentCulture = language != null ? new CultureInfo(language.CultureInfo) : CultureInfo.CurrentCulture;

                var list = SmallCollectionCache.Instance.Markets
                    .Where(l => l.Active || l.Active == !all)
                    .ToDictionary(sp => sp.MarketID, sp => sp.GetTerm(language.LanguageID))
                    .OrderBy(x => x.Value, StringComparer.Create(currentCulture ?? CultureInfo.CurrentCulture, false))
                    .ToDictionary(x => x.Key, x => x.Value);

                return list;

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual Dictionary<short, string> GetTranslatedAccountStatuses(bool all)
        {
            try
            {
                Language language = SmallCollectionCache.Instance.Languages.GetById(ApplicationContext.Instance.CurrentLanguageID);

                var currentCulture = language != null ? new CultureInfo(language.CultureInfo) : CultureInfo.CurrentCulture;

                return SmallCollectionCache.Instance.AccountStatuses
                            .Where(l => l.Active || l.Active == !all)
                            .ToDictionary(sp => sp.AccountStatusID, sp => sp.GetTerm(language.LanguageID))
                            .OrderBy(x => x.Value, StringComparer.Create(currentCulture ?? CultureInfo.CurrentCulture, false))
                            .ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual Dictionary<short, string> GetTranslatedAccountTypes(bool all)
        {
            try
            {
                Language language = SmallCollectionCache.Instance.Languages.GetById(ApplicationContext.Instance.CurrentLanguageID);

                var currentCulture = language != null ? new CultureInfo(language.CultureInfo) : CultureInfo.CurrentCulture;

                return SmallCollectionCache.Instance.AccountTypes
                           .Where(l => l.Active || l.Active == !all)
                           .ToDictionary(sp => sp.AccountTypeID, sp => sp.GetTerm(language.LanguageID))
                           .OrderBy(x => x.Value, StringComparer.Create(currentCulture ?? CultureInfo.CurrentCulture, false))
                           .ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public override List<TermTranslation> LoadAll(ITermTranslationRepository repository)
        {
            try
            {
                var list = repository.LoadAll();
                list.Each(item =>
                {
                    item.StartTracking();
                    item.IsLazyLoadingEnabled = true;
                });
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
