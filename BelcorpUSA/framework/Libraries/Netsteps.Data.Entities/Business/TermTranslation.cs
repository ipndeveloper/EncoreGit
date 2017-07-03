using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class TermTranslation
    {
        #region Methods
        public static List<TermTranslation> LoadTermsByLanguageID(int languageID)
        {
            try
            {
                return BusinessLogic.LoadTermsByLanguageID(Repository, languageID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static SqlUpdatableList<TermTranslation> LoadTermsByLanguageIDWithSqlDependency(int languageID)
        {
            try
            {
                return BusinessLogic.LoadTermsByLanguageIDWithSqlDependency(Repository, languageID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// It should not be common practice to call this method to get translated terms due to the Database access overhead. 
        /// Instead for regular lookup of terms we should use terms cached on the client. - JHE
        /// </summary>
        /// <param name="termName"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
        public static string LoadTermByTermNameAndLanguageID(string termName, int languageID)
        {
            try
            {
                return Repository.LoadTermByTermNameAndLanguageID(termName, languageID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public static TermTranslation LoadTermTranslationByTermNameAndLanguageID(string termName, int languageID)
        {
            try
            {
                TermTranslation term = Repository.LoadTermTranslationByTermNameAndLanguageID(termName, languageID);
                if (term != null)
                {
                    term.StartEntityTracking();
                    term.IsLazyLoadingEnabled = true;
                }
                return term;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<Language> GetLanguages(int languageID)
        {
            try
            {
                return BusinessLogic.GetLanguages(Repository, languageID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// By default only the Active Languages will be returned.
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetTranslatedLanguages(bool all = false)
        {
            try
            {
                return BusinessLogic.GetTranslatedLanguages(all);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// By default only the Active Countries will be returned.
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetTranslatedCountries(bool all = false)
        {
            try
            {
                return BusinessLogic.GetTranslatedCountries(all);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// By default only the Active Markets will be returned.
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetTranslatedMarkets(bool all = false)
        {
            try
            {
                return BusinessLogic.GetTranslatedMarkets(all);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<short, string> GetTranslatedAccountStatuses(bool all = false)
        {
            try
            {
                return BusinessLogic.GetTranslatedAccountStatuses(all);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<short, string> GetTranslatedAccountTypes(bool all = false)
        {
            try
            {
                return BusinessLogic.GetTranslatedAccountTypes(all);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<TermTranslation> LoadAll()
        {
            try
            {
                return BusinessLogic.LoadAll(Repository);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual void Save(bool validate)
        {
            try
            {
                if (validate)
                    base.Save();
                else
                    BusinessLogic.Save(GetRepository(), (TermTranslation)this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #endregion
    }
}
