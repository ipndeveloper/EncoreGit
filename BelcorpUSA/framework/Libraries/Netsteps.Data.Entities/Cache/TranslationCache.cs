using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Threading;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Core.Cache;
using System.Collections.Generic;
using System.Web;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Cache
{
	/// <summary>
	/// Description: This is the TermTranslation cache for use managing Terms (editing, adding, translating) from nsCore).
	/// </summary>
	[ContainerRegister(typeof(ITermTranslationProvider), RegistrationBehaviors.DefaultOrOverrideDefault, ScopeBehavior = ScopeBehavior.Singleton)]
	public class TranslationCache : ITermTranslationProvider, IExpireCache
	{
		#region Members
		private MruCacheOptions _termsByLanguageIdOptions = new MruCacheOptions() { CacheDepth = 10000, CacheItemLifespan = TimeSpan.FromMinutes(30), FullActive = true };
		private ICache<Tuple<int, string>, string> _termsByLanguageID;
		private ICache<Tuple<int, string>, string> TermsByLanguageID
		{
			get
			{
				if (_termsByLanguageID == null)
				{
					_termsByLanguageID = new ActiveMruLocalMemoryCache<Tuple<int, string>, string>("TermsByLanguageId", _termsByLanguageIdOptions, new DelegatedDemuxCacheItemResolver<Tuple<int, string>, string>(ResolveTermsByLanguageID));
				}
				return _termsByLanguageID;
			}
		}

		#endregion

		#region Methods

		private bool ResolveTermsByLanguageID(Tuple<int, string> langIdAndTerm, out string term)
		{
			using (var nse = new NetStepsEntities())
			{
				term = (from t in nse.TermTranslations
						where t.LanguageID == langIdAndTerm.Item1
						&& t.TermName == langIdAndTerm.Item2
						select t)
						.OrderByDescending(t => t.Active)
						.OrderByDescending(t => t.LastUpdatedUTC)
						.Select(t => t.Term).FirstOrDefault();
			}
			return (term != null);
		}

		public string GetTerm(string termName, string defaultValue = "")
		{
			return GetTerm((ApplicationContext.Instance.CurrentLanguageID == 0 ? 5 : ApplicationContext.Instance.CurrentLanguageID), termName, defaultValue);
		}

        public string GetTerm(int languageId, string termName, string defaultValue = "")
        {
            string term = "";
            if (!string.IsNullOrEmpty(termName))
            {
                if (!TermsByLanguageID.TryGet(Tuple.Create(languageId, termName.Trim()), out term))
                {
                    term = defaultValue != null ? defaultValue.Trim() : String.Empty;
                    if (!String.IsNullOrWhiteSpace(defaultValue) && languageId == (int)Constants.Language.English)
                    {
                        try
                        {
                            //Don't create duplicates...
                            if (!TermTranslation.Repository.Any(t => t.LanguageID == languageId && t.TermName.Trim() == termName.Trim()))
                            {
                                TermTranslation newTerm = new TermTranslation();
                                newTerm.Active = true;
                                newTerm.LastUpdatedUTC = DateTime.UtcNow;
                                newTerm.TermName = termName.Trim();
                                newTerm.LanguageID = languageId;
                                newTerm.Term = term;
                                newTerm.Save(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            // We don't want to throw an error for this; just log it. - JHE
                            EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                        }
                    }

                    return term.ReplaceCmsTokens();
                }
            }
            return term;
        }
		/// <summary>
		/// Provide the 'args' parameters to do a string.format on the translated term. - JHE
		/// </summary>
		/// <param name="languageId"></param>
		/// <param name="termName"></param>
		/// <param name="args"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public string GetTerm(int languageId, string termName, object[] args, string defaultValue = "")
		{
			return string.Format(GetTerm(languageId, termName, defaultValue), args);
		}

		public void ExpireCache()
		{
			TermsByLanguageID.FlushAll();
		}

		#endregion
	}
}
