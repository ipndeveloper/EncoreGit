using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class CampaignActionRepository
    {
        protected override Func<NetStepsEntities, IQueryable<CampaignAction>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<CampaignAction>>(
                 (context) => context.CampaignActions
                     .Include("Campaign")
                     .Include("Campaign.CampaignSubscribers")
                     .Include("EmailCampaignActions")
                     .Include("AlertCampaignActions")
                     .Include("CampaignActionQueueItems")
                     .Include("CampaignActionQueueItems.EventContext")
                     .Include("CampaignActionTokenValues")
                     );
            }
        }

		/// <summary>
		/// Loads all active and not completed campaign actions for the campaign.
		/// </summary>
		/// <param name="campaignID">The campaign ID.</param>
		/// <returns></returns>
		public IEnumerable<CampaignAction> LoadAllActiveAndNotCompletedForCampaign(int campaignID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.CampaignActions.Where(x => x.CampaignID == campaignID && x.Active == true && !x.IsCompleted).ToList();
				}
			});
		}

		/// <summary>
		/// Searches using specified search parameters.
		/// </summary>
		/// <param name="searchParameters">The search parameters.</param>
		/// <returns></returns>
		public PaginatedList<CampaignActionSearchData> Search(CampaignActionSearchParameters searchParameters)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var results = new PaginatedList<CampaignActionSearchData>(searchParameters);
					IQueryable<CampaignAction> campaignActions = context.CampaignActions;

                    if (searchParameters.Active.HasValue)
                    {
                        campaignActions = campaignActions.Where(c => c.Active == searchParameters.Active.Value);
                    }

                    if (searchParameters.CampaignID.HasValue)
                    {
                        campaignActions = campaignActions.Where(c => c.CampaignID == searchParameters.CampaignID.Value);
                    }

                    if (searchParameters.WhereClause != null)
                    {
                        campaignActions = campaignActions.Where(searchParameters.WhereClause);
                    }

                    if (searchParameters.CampaignActionType != Generated.ConstantsGenerated.CampaignActionType.NotSet)
                        campaignActions = campaignActions.Where(ca => ca.CampaignActionTypeID == (short)searchParameters.CampaignActionType);

                    campaignActions = campaignActions.ApplyOrderByFilter(searchParameters, context);

                    results.TotalCount = campaignActions.Count();

                    campaignActions = campaignActions.ApplyPagination(searchParameters);

                    var interimCampaigns = campaignActions.Select(c => new
                    {
                        CampaignActionID = c.CampaignActionID,
                        Name = c.Name,
                        NextRunDateUTC = c.NextRunDateUTC,
                        Active = c.Active,
                        EmailTemplateID = c.EmailCampaignActions.FirstOrDefault()!=null ? c.EmailCampaignActions.FirstOrDefault().EmailCampaignActionID: default(int),
                        AlertTemplateID = c.AlertCampaignActions.FirstOrDefault()!=null ? c.AlertCampaignActions.FirstOrDefault().AlertCampaignActionID : default(int)
                    }).ToList();

                    results.AddRange(interimCampaigns.Select(c => new CampaignActionSearchData()
                    {
                        CampaignActionID = c.CampaignActionID,
                        Name = c.Name,
                        Active = c.Active,
                        NextRunDateUTC = c.NextRunDateUTC.HasValue ? c.NextRunDateUTC : (DateTime?)null,
                        EmailTemplateID = c.EmailTemplateID,
                        AlertTemplateID = c.AlertTemplateID
                    }));

                    return results;
                }
            });
        }

		/// <summary>
		/// Gets the list of pending subscribers for the campaign.
		/// </summary>
		/// <param name="campaignID">The campaign ID.</param>
		/// <returns></returns>
		public IEnumerable<int> GetPendingSubscribers(int campaignID)
		{

            return new List<int>();

            //return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            //{
            //using (NetStepsEntities context = CreateContext())
            //{
            //    var orderItems = (from cs in context.CampaignActions
            //                      join ca in context.CampaignActionQueueItems on cs.CampaignActionID equals ca.CampaignActionID
            //                      where o.OrderID == orderID
            //                      orderby os.OrderID descending
            //                      select os).ToList();

            //    return orderItems.ToList();
            //}

            //using (NetStepsEntities context = CreateContext())
            //{
            //    //var subscribedAccounts = refreshedCampaignAction.CampaignActionQueueItems.ToList(a => a.EventContext.AccountID);
            //    //var validSubscribers = campaign.CampaignSubscribers.Where(c => !subscribedAccounts.Contains(c.AccountID));

            //    return context.CampaignSubscribers.Where(cs => cs.CampaignID == campaignID).Select(c => c.AccountID).ToList();
            //}
            //});

        }
    }
}
