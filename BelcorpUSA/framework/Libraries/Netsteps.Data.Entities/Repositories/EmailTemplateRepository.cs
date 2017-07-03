using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Expressions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class EmailTemplateRepository
	{
		#region Members
		protected override Func<NetStepsEntities, IQueryable<EmailTemplate>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<EmailTemplate>>(
				   (context) => from a in context.EmailTemplates
											   .Include("EmailTemplateTranslations")
								select a);
			}
		}

		#endregion

		public PaginatedList<EmailTemplate> Search(EmailTemplateSearchParameters searchParams)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var results = new PaginatedList<EmailTemplate>(searchParams);
					IQueryable<EmailTemplate> emailTemplates = loadAllFullQuery(context);

					if (searchParams.Active.HasValue)
					{
						emailTemplates = emailTemplates.Where(e => e.Active == searchParams.Active.Value);
					}
					if (searchParams.EmailTemplateTypeIDs != null && searchParams.EmailTemplateTypeIDs.Count > 0)
					{
						var where = ExpressionHelper.MakeWhereInExpression<EmailTemplate, short>(et => et.EmailTemplateTypeID, searchParams.EmailTemplateTypeIDs);
						emailTemplates = emailTemplates.Where(where);
					}

					if (searchParams.WhereClause != null)
					{
						emailTemplates = emailTemplates.Where(searchParams.WhereClause);
					}

					if (!string.IsNullOrEmpty(searchParams.OrderBy))
					{
						switch (searchParams.OrderBy)
						{
							case "Subject":
								var languageId = ApplicationContext.Instance.CurrentLanguageID;
								emailTemplates = emailTemplates.ApplyOrderByFilter(searchParams.OrderByDirection, e => e.EmailTemplateTranslations.Any(t => t.LanguageID == languageId) ? e.EmailTemplateTranslations.FirstOrDefault(t => t.LanguageID == languageId).Subject : e.EmailTemplateTranslations.Any() ? e.EmailTemplateTranslations.FirstOrDefault().Subject : "");
								break;
							default: emailTemplates = emailTemplates.ApplyOrderByFilter(searchParams, context);
								break;
						}
					}
					else
					{
						emailTemplates = emailTemplates.OrderBy(et => et.EmailTemplateID);
					}

					results.TotalCount = emailTemplates.Count();

					emailTemplates = emailTemplates.ApplyPagination(searchParams);

					results.AddRange(emailTemplates);

					return results;
				}
			});
		}

		public Dictionary<short, List<EmailTemplate>> GetEvitesEmailTemplates()
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var emailTemplateTypes = SmallCollectionCache.Instance.EmailTemplateTypes.Where(ett => ett.Name.StartsWith("Evites", StringComparison.InvariantCultureIgnoreCase)).Select(ett => ett.EmailTemplateTypeID);
					var where = ExpressionHelper.MakeWhereInExpression<EmailTemplate, short>(et => et.EmailTemplateTypeID, emailTemplateTypes);
					return context.EmailTemplates.Where(where).GroupBy(et => et.EmailTemplateTypeID).ToDictionary(etg => etg.Key, etg => etg.ToList());
				}
			});
		}

		public List<EmailTemplate> GetEmailTemplatesByEmailTemplateTypeID(short emailTemplateTypeID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.EmailTemplates.Where(x => x.EmailTemplateTypeID == emailTemplateTypeID).ToList();
				}
			});
		}
	}
}
