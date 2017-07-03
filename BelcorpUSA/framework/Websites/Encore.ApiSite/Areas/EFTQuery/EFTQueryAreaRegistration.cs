using System.Web.Mvc;
using System.Web.Routing;

namespace Encore.ApiSite.Areas.EFTQuery
{
    public class EFTQueryAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "EFTQuery";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "eftquery_by_class",
                "efts/nacha/classes/{classType}",
                new { action = "GetEftsByClassType", controller = "Efts", id = UrlParameter.Optional },
                new { httpMethod = new HttpMethodConstraint("GET") }
            );

            context.MapRoute(
                "eftquery_by_orderId",
                "efts/nacha/{OrderId}",
                new { action = "GetEftsByOrderId", controller = "Efts", id = UrlParameter.Optional },
                new { httpMethod = new HttpMethodConstraint("GET") }
            );

            context.MapRoute(
                "eftquery_updatepayments",
                "efts/nacha/updatepayments",
                new { action = "UpdateEftPaymentsByOrderPaymentIds", controller = "Efts", id = UrlParameter.Optional },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );
        }
    }
}
