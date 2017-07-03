using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Data.Entities.Business.Logic;

namespace nsCore.Areas.Communication.Controllers
{
    public class OrderPaymentTemplatesController : BaseController
    {
       
        [FunctionFilter("Communication", "~/Accounts")]
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var orderPaymentTemplates = OrderPaymentTemplatesExtensions.GetOrderPaymentTemplates(new OrderPaymentTemplatesSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });


                foreach (var item in orderPaymentTemplates)
                {
                    builder.Append("<tr>");
                    builder.AppendCheckBoxCell(value: item.OrderPaymentTemplateId.ToString(), name: "");
                    builder                        
                        .AppendLinkCell("~/Communication/OrderPaymentTemplates/Edit/" + item.OrderPaymentTemplateId, item.OrderPaymentTemplateId.ToString())
                        .AppendCell(item.Type)
                        .AppendCell(item.Description)
                        .AppendCell(item.Days.ToString())
                        .AppendCell(item.MinimalAmount.ToString())
                        .AppendLinkCell("~/Communication/EmailTemplates/Edit/" + item.EmailTemplateId.ToString(), item.EmailTemplateName)                                         
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = orderPaymentTemplates.TotalPages, page = orderPaymentTemplates.TotalCount == 0 ? "<tr><td colspan=\"5\">There are no plans</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Edit(string id = "0")
        {
            try
            {
                var data = OrderPaymentTemplatesBusinessLogic.Edit(id);

                return View(data);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Save(                                       
                                        string description,
                                        int days,
                                        int minimalAmount,
                                        string id="0"
                                        )
        {
            try
            {               
                var orderPaymentTem = OrderPaymentTemplatesBusinessLogic.Edit(id);
                orderPaymentTem.OrderPaymentTemplateId = id != "0" ? orderPaymentTem.OrderPaymentTemplateId : 0;
                orderPaymentTem.Description = description;
                orderPaymentTem.Days = days;
                orderPaymentTem.MinimalAmount = minimalAmount;

                if (id == "0")
                    OrderPaymentTemplatesExtensions.Insert(orderPaymentTem);
                else
                    OrderPaymentTemplatesExtensions.Update(orderPaymentTem);

               
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Delete(List<int> items)
        {
            try
            {
                if (items != null)
                {
                    foreach (var itemId in items)
                    {
                        OrderPaymentTemplatesExtensions.Delete(itemId);
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }      

    }
}
