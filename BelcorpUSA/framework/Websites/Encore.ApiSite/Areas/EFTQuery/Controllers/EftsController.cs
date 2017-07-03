using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EFTQuery.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc;
using NetSteps.Web.Mvc.Restful;

namespace Encore.ApiSite.Areas.EFTQuery.Controllers
{
    public class EftsController : Controller
    {
		public IEFTQueryProcessor EftQueryProcessorQuery { get { return Create.New<IEFTQueryProcessor>(); } }

        public ActionResult GetEftsByClassType(string classType)
        {
            if (String.IsNullOrEmpty(classType))
            {
                classType = "WEB";
            }
            var results = EftQueryProcessorQuery.GetTransfersByClassType(classType);
            if (results != null && results.Count() > 0)
            {
                var formatType = Request.Headers["response-format-type"];
                if (!string.IsNullOrEmpty(formatType))
                {
                    if (formatType.Equals("CSV", StringComparison.InvariantCultureIgnoreCase))
                        return Content(results.ToCsv(), "text/csv");
                    if (formatType.Equals("XML", StringComparison.InvariantCultureIgnoreCase))
                        return Content(results.ToXml(), "text/xml");
                }
                else
                {
                    return Content(results.ToXml(), "text/xml");
                }
            }
            return Content("No results found for that class type");
        }

        public ActionResult GetEftsByOrderId(int orderId)
        {
            if (orderId <= 0) throw new HttpRequestException(HttpConstants.HttpStatusCodes.UnproccessableEntity, "OrderId must be greater than 0");
            var result = EftQueryProcessorQuery.GetTransferByOrderId(orderId);
            if (result != null)
            {
                var resultList = new List<IEFTQueryProcessorResult>();
                resultList.Add(result);
                var formatType = Request.Headers["response-format-type"];
                if (!string.IsNullOrEmpty(formatType))
                {
                    if (formatType.Equals("CSV", StringComparison.InvariantCultureIgnoreCase))
                        return Content(resultList.ToCsv(), "text/csv");
                    if (formatType.Equals("XML", StringComparison.InvariantCultureIgnoreCase))
                        return Content(resultList.ToXml(), "text/xml");
                }
                else
                {
                    return Content(resultList.ToXml(), "text/xml");
                }
            }
            return Content("No results found for that order id");
        }

        public ActionResult UpdateEftPaymentsByOrderPaymentIds(List<int> orderPaymentIds)
        {
            if (orderPaymentIds.Count() <= 0) throw new HttpRequestException(HttpConstants.HttpStatusCodes.UnproccessableEntity, "You must pass in order payment id(s)");
			var results = EftQueryProcessorQuery.UpdateNachaQueryProcessorResults(orderPaymentIds).ToDictionary(r => r.OrderPaymentId, r => r.Success);
            var formatType = Request.Headers["response-format-type"];
            if (!string.IsNullOrEmpty(formatType))
            {
                if (formatType.Equals("CSV", StringComparison.InvariantCultureIgnoreCase))
                    return Content(results.DictionaryToCSV(), "text/csv");
                if (formatType.Equals("XML", StringComparison.InvariantCultureIgnoreCase))
                    return Content(results.DictionaryToXML(), "text/xml");
            }

            return Content(results.DictionaryToXML(), "text/xml");
        }
    }
}
