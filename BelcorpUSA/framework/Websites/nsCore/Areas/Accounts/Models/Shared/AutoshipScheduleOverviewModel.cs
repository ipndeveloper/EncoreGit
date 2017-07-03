using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Accounts.Models.Shared
{
    public class AutoshipScheduleOverviewModel
    {
        public int AutoshipScheduleID { get; set; }
        public int AccountID { get; set; }
        public string Title { get; set; }
        public bool Active { get; set; }
        public IEnumerable<AutoshipOverviewModel> AutoshipOverviews { get; set; }
        public bool IsEnrollable { get; set; }
        public virtual AutoshipScheduleOverviewModel LoadResources(
            AutoshipScheduleOverviewData autoshipScheduleOverview, int accountID)
        {
            AccountID = accountID;
            AutoshipScheduleID = autoshipScheduleOverview.AutoshipScheduleID;
            Title = autoshipScheduleOverview.LocalizedName;
            Active = autoshipScheduleOverview.Active;
            AutoshipOverviews = autoshipScheduleOverview.AutoshipOverviews
                .OrderByDescending(x => x.Active)
                .Select(x => new AutoshipOverviewModel().LoadResources(
                    x,
                    autoshipScheduleOverview.IsTemplateEditable)
                );
            IsEnrollable = autoshipScheduleOverview.IsEnrollable;
            return this;
        }
    }
}