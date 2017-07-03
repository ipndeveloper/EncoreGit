using System.Linq;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Accounts.Models.Shared
{
    public class AutoshipOverviewModel
    {
        public int AccountID { get; set; }
        public int AutoshipOrderID { get; set; }
        public int AutoshipScheduleID { get; set; }
        public string TemplateOrderNumber { get; set; }
        public bool Active { get; set; }
        public string StatusText { get; set; }
        public string ItemsText { get; set; }
        public bool IsTemplateEditable {get;set;}

        public virtual AutoshipOverviewModel LoadResources(
            AutoshipOverviewData autoshipOverviewData,
            bool isTemplateEditable)
        {
            AccountID = autoshipOverviewData.AccountID;
            AutoshipScheduleID = autoshipOverviewData.AutoshipScheduleID;
            AutoshipOrderID = autoshipOverviewData.AutoshipOrderID;
            TemplateOrderNumber = autoshipOverviewData.TemplateOrderNumber;
            Active = autoshipOverviewData.Active;
            StatusText = autoshipOverviewData.StatusText;
            ItemsText = string.Join(", ", autoshipOverviewData.OrderItems
                .Select(x => string.Format("{0} x {1}", x.Quantity, x.SKU))
            );
            IsTemplateEditable = isTemplateEditable;
            return this;
        }
    }
}