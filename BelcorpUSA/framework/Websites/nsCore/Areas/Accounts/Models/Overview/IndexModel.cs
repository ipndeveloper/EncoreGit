using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using nsCore.Areas.Accounts.Models.Shared;

namespace nsCore.Areas.Accounts.Models.Overview
{
    public class IndexModel
    {
        public bool IsUserActive { get; set; }
        public bool IsLocked { get; set; }
        public IEnumerable<ProxyLinkModel> ProxyLinks { get; set; }
        public IEnumerable<AutoshipScheduleOverviewModel> AutoshipScheduleOverviews { get; set; }
        public IEnumerable<AutoshipScheduleOverviewModel> SubscriptionScheduleOverviews { get; set; }

        public virtual IndexModel LoadResources(
            int accountID,
            bool isUserActive,
            IEnumerable<ProxyLinkData> proxyLinks,
            IEnumerable<AutoshipScheduleOverviewData> autoshipScheduleOverviews)
        {
            Contract.Requires<ArgumentNullException>(proxyLinks != null);
            Contract.Requires<ArgumentNullException>(autoshipScheduleOverviews != null);

            IsUserActive = isUserActive;

            ProxyLinks = proxyLinks
                .Select(x => new ProxyLinkModel()
                    .LoadResources(x)
                );

            AutoshipScheduleOverviews = autoshipScheduleOverviews
                .Where(x => x.AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.Normal)
                .Select(x => new AutoshipScheduleOverviewModel().LoadResources(x, accountID))
                .Where(x => x.IsEnrollable || x.AutoshipOverviews.Any());

            SubscriptionScheduleOverviews = autoshipScheduleOverviews
                .Where(x => x.AutoshipScheduleTypeID != (int)Constants.AutoshipScheduleType.Normal)
                .Select(x => new AutoshipScheduleOverviewModel()
                    .LoadResources(x, accountID)
                );

            return this;
        }
    }
}