using System.Web;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities;

namespace DistributorBackOffice.Models
{
    public class OrdersOverviewModel
    {
        public bool IsPartyOrderClient
        {
            get
            {
                if (OrdersSection.Instance != null)
                {
                    return OrdersSection.Instance.IsPartyOrderClient;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool FundraisersEnabled
        {
            get
            {
                if (OrdersSection.Instance != null)
                {
                    return OrdersSection.Instance.FundraisersEnabled;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsOrderHistoryReport
        {
            get
            {
                return (UrlContainsOrderHistory || IsPartyOrderClient == false) && !IsFundraiserOrderHistoryReport;
            }
        }

        public bool IsFundraiserOrderHistoryReport
        {
            get
            {
                string orderTypeQuery = string.Format("type={0}", (int)Constants.OrderType.FundraiserOrder);
                bool containsFundraiserOrderType = HttpContext.Current.Request.Url.Query.Contains(orderTypeQuery);
                return UrlContainsOrderHistory && containsFundraiserOrderType;
            }
        }

        private bool UrlContainsOrderHistory
        {
            get
            {
                return HttpContext.Current.Request.Url.LocalPath.Contains("OrderHistory");
            }
        }
    }
}