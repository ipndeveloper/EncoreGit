using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class CampaignRepository
    {
        protected override Func<NetStepsEntities, IQueryable<Campaign>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Campaign>>(
                 (context) => context.Campaigns
                     .Include("CampaignEmails")
                     .Include("CampaignSubscribers")
                     .Include("CampaignActions")
                     .Include("CampaignActions.EmailCampaignActions")
                     .Include("CampaignActions.AlertCampaignActions")
                     .Include("Market"));
            }
        }

        public PaginatedList<CampaignSearchData> Search(CampaignSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var results = new PaginatedList<CampaignSearchData>(searchParameters);
                    IQueryable<Campaign> campaigns = context.Campaigns;

                    if (searchParameters.Active.HasValue)
                    {
                        campaigns = campaigns.Where(c => c.Active == searchParameters.Active.Value);
                    }

                    if (searchParameters.MarketID.HasValue)
                    {
                        campaigns = campaigns.Where(c => c.MarketID == searchParameters.MarketID.Value);
                    }

                    if (searchParameters.WhereClause != null)
                    {
                        campaigns = campaigns.Where(searchParameters.WhereClause);
                    }

                    if (!searchParameters.OrderBy.IsNullOrEmpty())
                    {
                        switch (searchParameters.OrderBy)
                        {
                            case "NextRunDateUTC":
                                campaigns = campaigns.ApplyOrderByFilter(searchParameters.OrderByDirection, c => c.CampaignActions
                                       .AsQueryable()
                                       .OrderBy(ca => ca.NextRunDateUTC)
                                       .FirstOrDefault()
                                       .NextRunDateUTC);
                                break;
                            default:
                                campaigns = campaigns.ApplyOrderByFilter(searchParameters, context);
                                break;
                        }
                    }

                    results.TotalCount = campaigns.Count();

                    campaigns = campaigns.ApplyPagination(searchParameters);

                    var interimCampaigns = campaigns.Select(c => new
                    {
                        CampaignID = c.CampaignID,
                        Name = c.Name,
                        NumberOfEmails = c.CampaignEmails.Count,
                        StartDate = c.StartDateUTC,
                        EndDate = c.EndDateUTC,
                        Active = c.Active,
                        MarketID = c.MarketID,
                        NextRunDate = c.CampaignActions
                                       .AsQueryable()
                                       .OrderBy(ca => ca.NextRunDateUTC)
                                       .FirstOrDefault()
                                       .NextRunDateUTC,
                    }).ToList();

                    results.AddRange(interimCampaigns.Select(c => new CampaignSearchData()
                    {
                        CampaignID = c.CampaignID,
                        Name = c.Name,
                        NumberOfEmails = c.NumberOfEmails,
                        StartDate = c.StartDate.HasValue ? c.StartDate.UTCToLocal() : (DateTime?)null,
                        EndDate = c.EndDate.HasValue ? c.EndDate.UTCToLocal() : (DateTime?)null,
                        Active = c.Active,
                        MarketID = c.MarketID,
                        NextCampaignActionScheduledDate = c.NextRunDate.HasValue ? c.NextRunDate.UTCToLocal() : (DateTime?)null
                    }));

                    return results;
                }
            });

        }


        public List<Campaign> LoadFullAllByDomainEventTypeID(int domainEventTypeID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var campaigns = (from c in loadAllFullQuery(context)
                                     where c.DomainEventTypeID == domainEventTypeID
                                     select c).ToList();
                    return campaigns;
                }
            });
        }

        public List<Campaign> LoadFullAllByCampaignTypeID(short campaignTypeID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var campaigns = (from c in loadAllFullQuery(context)
                                     where c.CampaignTypeID == campaignTypeID
                                     select c).ToList();
                    return campaigns;
                }
            });
        }
    }
}
