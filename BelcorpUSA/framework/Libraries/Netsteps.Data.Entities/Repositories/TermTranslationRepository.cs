using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class TermTranslationRepository
	{
		#region Members
		#endregion

		public List<TermTranslation> LoadTermsByLanguageID(int languageID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.TermTranslations.Where(t => t.LanguageID == languageID).ToList();
				}
			});
		}

		public SqlUpdatableList<TermTranslation> LoadTermsByLanguageIDWithSqlDependency(int languageID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var termTranslations = from t in context.TermTranslations
										   where t.LanguageID == languageID
										   select t;

					var entityQuery = (ObjectQuery<TermTranslation>)termTranslations;
					return LoadListWithSqlDependency(entityQuery);
				}
			});
		}

		public string LoadTermByTermNameAndLanguageID(string termName, int languageID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var term = (from t in context.TermTranslations
								where t.TermName == termName
									&& t.LanguageID == languageID
								select t.Term).FirstOrDefault();
					return term;
				}
			});
		}

		public TermTranslation LoadTermTranslationByTermNameAndLanguageID(string termName, int languageID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var term = (from t in context.TermTranslations
								where t.TermName == termName
									&& t.LanguageID == languageID
								select t).FirstOrDefault();
					return term;
				}
			});
		}

		public List<Language> LoadTermsLanguageIDs(int languageID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var languages = from l in context.Languages
									join t in context.TermTranslations on l.TermName equals t.TermName
									where t.LanguageID == languageID
									select l;

					return languages.OrderBy(l => l.LanguageID).ToList();
				}
			});
		}
	}
}
