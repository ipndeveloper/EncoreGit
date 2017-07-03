using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class EmailCampaignActionRepository : IEmailCampaignActionRepository
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<EmailCampaignAction>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<EmailCampaignAction>>(
                   (context) => from x in context.EmailCampaignActions
                                               .Include("CampaignAction")
                                               .Include("CampaignAction.Campaign")
                                select x);
            }
        }
        #endregion

        #region Public Methods
        public List<EmailCampaignAction> LoadAllDistributorVisible(Constants.CampaignType? campaignType = null, int? campaignID = null)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var query = context.EmailCampaignActions
                        .Include("CampaignAction")
                        .AsQueryable();

                    query = ApplyDistributorVisibleQuery(query, campaignType, campaignID);

                    return query.ToList();
                }
            });
        }

        public List<EmailCampaignAction> LoadFullAllDistributorVisible(Constants.CampaignType? campaignType = null, int? campaignID = null)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var query = loadAllFullQuery(context);
                    
                    query = ApplyDistributorVisibleQuery(query, campaignType, campaignID);
                    
                    return query.ToList();
                }
            });                   
        }
        #endregion

        #region Private Methods
        private IQueryable<EmailCampaignAction> ApplyDistributorVisibleQuery(IQueryable<EmailCampaignAction> query, Constants.CampaignType? campaignType = null, int? campaignID = null)
        {
            // Load both future(editable) and past actions.
            query = from x in query
                    where x.CampaignAction.Campaign.Active == true
                        && x.CampaignAction.Active == true
                        && (
                            x.DistributorEditableDateUTC <= DateTime.UtcNow
                            ||
                            x.CampaignAction.IsCompleted == true
                        )
                    select x;

            if (campaignType.HasValue)
            {
                // EF doesn't like enums
                short campaignTypeID = (short)campaignType.Value;
                query = query.Where(x => x.CampaignAction.Campaign.CampaignTypeID == campaignTypeID);
            }

            if (campaignID.HasValue)
                query = query.Where(x => x.CampaignAction.CampaignID == campaignID.Value);

            return query;
        }
        #endregion
    }
}
