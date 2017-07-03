using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace nsCore.Areas.Accounts.Controllers
{
    public class LedgerSupportController : Controller
    {
        //
        // GET: /Accounts/LedgerSupport/

        public ActionResult Index()
        {

            TempData["GetEntryReason"] = from x in LedgerSupport.GetEntryReason()
                                         orderby x.Value
                                         select new SelectListItem()
                                         {
                                             Value = x.Key,
                                             Text = x.Value
                                         };


            TempData["GetEntryType"] =   from x in LedgerSupport.GetLedgerEntryType()                                         
                                         orderby x.Value
                                         select new SelectListItem()
                                         {
                                             Value = x.Key,
                                             Text = x.Value
                                         };

            TempData["GetBonusTypes"] =  from x in LedgerSupport.GetBonusTypes()                                         
                                         orderby x.Value
                                         select new SelectListItem()
                                         {
                                             Value = x.Key,
                                             Text = x.Value
                                         };


            return View();
        }

        public ActionResult ValidResult(int accountID, int orderID)
        {
            List<LedgerSupportSearchData> result = LedgerSupport.GetLedgerSupportTicketByID(accountID, orderID);

            foreach (var item in result)
            {
                if (accountID == 0)
                {
                    return Json(new { success = false, JsonRequestBehavior.AllowGet });
                }
                else
                {
                    int AccountId = item.AccountID;
                    int OrderId = item.OrderID;
                    int SupportTicketId = item.SupportTicketID;
                    int EntryReasonId = item.EntryReasonID;
                    int EntryTypeId = item.EntryTypeID;
                    int BonusTypeId = item.BonusTypeID;
                    return Json(new
                    {
                        success = true,
                        accountId = AccountId,
                        orderId = OrderId,
                        supportTicketId = SupportTicketId,
                        entryReasonId = EntryReasonId,
                        entryTypeId = EntryTypeId,
                        
                        JsonRequestBehavior.AllowGet
                    });
                }
            }
            return Json(new { success = false, JsonRequestBehavior.AllowGet });
        
        }

    }
}
