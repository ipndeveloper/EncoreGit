using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Generated;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities.Cache;
using System.Text;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Logic;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using NetSteps.Data.Entities.Utility;

namespace nsCore.Areas.Orders.Controllers
{
    public class TestOrderEntryController : OrdersBaseController
    {
        //
        // GET: /Orders/TestOrderEntry/
        private readonly IProductCreditLedgerService _productCreditLedgerService = Create.New<IProductCreditLedgerService>();


        public virtual ActionResult TestNewOrder()
        {
            //GetPeriods()
            TempData["GetPeriod"] = from x in Periods.GetPeriods()
                                      orderby x.Value
                                      select new SelectListItem()
                                      {
                                          Text = x.Key,
                                          Value = x.Value
                                      };
            return View();
        }


        public virtual ActionResult Index(int periodId, int? accountId)
        {
            try
            {
                accountId = 66350;
                Session["AccountNumber"] = 2;
                Account account = null;
                ViewBag.DatePeriod = Periods.GetDatePeriod(periodId).ToShortDateString();
                if (accountId.HasValue)
                {
                    account = Account.LoadForSession(accountId.Value); 
                    CoreContext.CurrentAccount = account.Clone();
                    CoreContext.CurrentAccount.StartEntityTracking();
                }
                else if (CoreContext.CurrentAccount != null)
                {
                    account = Account.LoadForSession(accountId.Value); 
                } 
                if (!account.UserID.HasValue)
                {
                    account.User = new User();
                    account.User.Username = Guid.NewGuid().ToString();
                    account.User.Password = NetSteps.Data.Entities.User.GeneratePassword();
                    account.User.UserTypeID = (int)NetSteps.Data.Entities.Constants.UserType.Distributor;
                    account.User.UserStatusID = (int)NetSteps.Data.Entities.Constants.UserStatus.Active;
                    account.User.DefaultLanguageID = CoreContext.CurrentAccount.DefaultLanguageID;
                    account.Save();

                    account = Account.LoadForSession(accountId.Value);
                    CoreContext.CurrentAccount = account.Clone();
                    CoreContext.CurrentAccount.StartEntityTracking();
                }

                if (account == null)
                    return RedirectToAction("NewOrder");

                if (account.AccountTypeID == (short)ConstantsGenerated.AccountType.Distributor)
                {
                    string messageOutput; 
                }

                // Create new order
                if (OrderContext.Order == null)
                {
                    OrderContext.Order = new Order(account);
                    OrderContext.Order.OrderTypeID = NetSteps.Data.Entities.Constants.OrderType.PortalOrder.ToShort();
                    OrderContext.Order.AsOrder().SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
                    OrderContext.Order.AsOrder().DateCreated = DateTime.Now;

                    var accountMarket = SmallCollectionCache.Instance.Markets.FirstOrDefault(m => m.MarketID == CurrentAccount.MarketID);
                    OrderContext.Order.CurrencyID = accountMarket != null ? accountMarket.GetDefaultCurrencyID() : CoreContext.CurrentMarket.GetDefaultCurrencyID();

                    // Due to the single page flow of order entry in Core we must always calculate shipping and taxes.
                    OrderContext.Order.AsOrder().OrderPendingState = NetSteps.Data.Entities.Constants.OrderPendingStates.Quote;
                    //Order.PreOrders.OrderID = OrderContext.Order.OrderID;
                    //Order.PreOrders.WareHouseID = OrderContext.Order.
                }
                 
                Address defaultShippingAddress = account.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Constants.AddressType.Shipping);
                ViewData["ShippingMethods"] = defaultShippingAddress != null ? GetNewShippingMethods(defaultShippingAddress.AddressID) : new List<ShippingMethodWithRate>();

                // TODO: Change back to the normal Inventory cache
                ViewData["Catalogs"] = Inventory.GetActiveCatalogs(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID);

                if (ApplicationContext.Instance.UseDefaultBundling)
                {
                    ViewBag.DynamicKitUpSaleHTML = GetDynamicBundleUpSale();
                    ViewBag.AvailableBundleCount = this._availableBundleCount;
                }

                SetProductCreditViewData(account.AccountID);

                var viewModel = new OrderEntryModel(OrderContext.Order.AsOrder());

                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("NewOrder", "OrderEntry");
            } 
        }
        [NonAction]
        public virtual string GetDynamicBundleUpSale()
        {
            if (OrderContext.Order == null)
            {
                return string.Empty;
            }

            var customer = OrderContext.Order.AsOrder().OrderCustomers[0];
            if (customer.IsTooBigForBundling())
            {
                return string.Empty;
            }

            var possibleDynamicKitProducts = OrderContext.Order.AsOrder().GetPotentialDynamicKitUpSaleProducts(customer, OrderContext.SortedDynamicKitProducts).ToList();
            this._availableBundleCount = possibleDynamicKitProducts.Count();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < possibleDynamicKitProducts.Count(); i++)
            {
                var product = possibleDynamicKitProducts[i];
                sb.Append("<input type=\"hidden\" class=\"dynamicKitProductSuggestion\" value=\"" + product.ProductID + "\" /><a href=\"javascript:void(0);\" class=\"CreateBundle\">" + product.Translations.Name() + "</a>");
                if (i != possibleDynamicKitProducts.Count() - 1)
                {
                    sb.Append(", ");
                }
            }
            return sb.ToString();
        }

        protected virtual void SetProductCreditViewData(int? accountID = null)
        {
            if (!accountID.HasValue)
                accountID = CoreContext.CurrentAccount.AccountID;
            var productCreditBalance = _productCreditLedgerService.GetCurrentBalanceLessPendingPayments(accountID.Value);

            ViewData["ProductCreditBalance"] = productCreditBalance.ToMoneyString();
        }


        public virtual ActionResult XMLPrueba()
        {
            string xmlEnviar = "<saldos>" +
  "<producto>" +
     "<SKU>982</SKU>" +
     "<Saldo>5</Saldo>" +
     "<Centro>BR03</Centro>" +
  "</producto>" +
  "<producto>" +
    "<SKU>234</SKU>" +
    "<Saldo>3</Saldo>" +
    "<Centro>BR13</Centro>" +
   "</producto>" +
"</saldos>";

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlEnviar);
            string xmlString = "";// XmlGeneratorBusinessLogic.Instance.GenerateXmlB020(xml);
            //string XmlString =  XmlProductMaterialBusinessLogic.Instance.GenerateXmlForProductMaterial(Server.MapPath("~") + ConfigurationManager.AppSettings["TemplatesXML_Path"]);
            XDocument XmlDoc = XDocument.Parse(xmlString);
            MemoryStream ms = new MemoryStream();
            XmlDoc.Save(Server.MapPath("~") + ConfigurationManager.AppSettings["TemplatesXML_Path"] + "demo.xml");
            FileHelper.Save(ms, Server.MapPath("~") + ConfigurationManager.AppSettings["TemplatesXML_Path"]);

            return Json(new { data = false, JsonRequestBehavior.AllowGet });
        }
    }
}
