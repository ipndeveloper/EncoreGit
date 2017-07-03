using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class CampaignActionTokenValueRepository
    {
        protected override Func<NetStepsEntities, IQueryable<CampaignActionTokenValue>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<CampaignActionTokenValue>>(
                 (context) => context.CampaignActionTokenValues
                     .Include("Token")
                     );
            }
        }

        public List<CampaignActionTokenValue> LoadAllByCampaignActionID(int campaignActionID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var campaignsActionTokenValues = (from tv in loadAllFullQuery(context)
                                                      where tv.CampaignActionID == campaignActionID
                                                      select tv).ToList();
                    return campaignsActionTokenValues;
                }
            });
        }

		public CampaignActionTokenValue LoadByUniqueKey(Constants.Token token, int languageID, int campaignActionID, int? accountID = null)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					int tokenID = (int)token;

					var query = from tv in loadAllFullQuery(context)
								where tv.TokenID == tokenID
									&& tv.LanguageID == languageID
									&& tv.CampaignActionID == campaignActionID
									&& (accountID == null ? tv.AccountID == null : tv.AccountID == accountID)//uses IS NULL comparison when accountID is null
								select tv;

					var campaignsActionTokenValue = query.FirstOrDefault();

#if DEBUG
                    string q = query.ToTraceString();
#endif

					return campaignsActionTokenValue;
				}
			});
		}

        public List<CampaignActionTokenValue> LoadAll(CampaignActionTokenValueSearchParameters searchParams)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var query = ApplySearchParameters(context.CampaignActionTokenValues, searchParams);

#if DEBUG
                    var q = query.ToTraceString();
#endif

                    return query.ToList();
                }
            });
        }

        public List<CampaignActionTokenValue> LoadAllFull(CampaignActionTokenValueSearchParameters searchParams)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var query = ApplySearchParameters(loadAllFullQuery(context), searchParams);

#if DEBUG
                    var q = query.ToTraceString();
#endif

                    return query.ToList();
                }
            });
        }

        private IQueryable<CampaignActionTokenValue> ApplySearchParameters(IQueryable<CampaignActionTokenValue> query, CampaignActionTokenValueSearchParameters searchParams)
        {
            if (!searchParams.AccountID.HasValue)
            {
                if (searchParams.IncludeDefaults.HasValue)
                {
                    if (searchParams.IncludeDefaults.Value)
                    {
                        // Defaults only
                        query = query.Where(x => x.AccountID == null);
                    }
                    else
                    {
                        // All accounts, no defaults
                        query = query.Where(x => x.AccountID != null);
                    }
                }
                else
                {
                    // Both values are null. Do not filter by AccountID.
                }
            }
            else
            {
                if (searchParams.IncludeDefaults.HasValue && searchParams.IncludeDefaults.Value)
                {
                    // One account, and defaults
                    query = query.Where(x => x.AccountID == null || x.AccountID == searchParams.AccountID.Value);
                }
                else
                {
                    // One account, no defaults (when IncludeDefaults is null, we assume it to be false)
                    query = query.Where(x => x.AccountID == searchParams.AccountID.Value);
                }
            }

            if (searchParams.CampaignActionID.HasValue)
                query = query.Where(x => x.CampaignActionID == searchParams.CampaignActionID.Value);

            if (searchParams.LanguageID.HasValue)
                query = query.Where(x => x.LanguageID == searchParams.LanguageID.Value);

            if (searchParams.TokenID.HasValue)
                query = query.Where(x => x.TokenID == searchParams.TokenID.Value);

            return query;
        }
    }
}
