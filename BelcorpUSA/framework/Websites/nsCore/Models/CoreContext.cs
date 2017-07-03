using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using nsCore.Models;
using nsCore.Areas.Commissions.CommissionRunners;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Web.Mvc.Helpers
{
    public static class CoreContext
    {
        public static IUser CurrentUser
        {
            get { return (IUser)NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUser; }
            set { NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUser = value; }
        }

        public static int CurrentLanguageID
        {
            get
            {
                return ApplicationContext.Instance.CurrentLanguageID;
            }
            set
            {
                ApplicationContext.Instance.CurrentLanguageID = value;
            }
        }
        public static Language CurrentLanguage
        {
            get
            {
                return SmallCollectionCache.Instance.Languages.GetById(ApplicationContext.Instance.CurrentLanguageID);
            }
        }

        public static CultureInfo CurrentCultureInfo
        {
            get
            {
                return CurrentLanguage.Culture;
            }
        }

        public static string GetMonthName(int month)
        {
            return month.GetMonthName(CoreContext.CurrentCultureInfo);
        }

        public static List<Market> CurrentUserMarkets
        {
            get
            {
                if (HttpContext.Current.Session["CurrentUserMarkets"] == null || (HttpContext.Current.Session["CurrentUserMarkets"] as CurrentUserMarketsModel).CorporateUserID != (CoreContext.CurrentUser as CorporateUser).CorporateUserID)
                {
                    HttpContext.Current.Session["CurrentUserMarkets"] = new CurrentUserMarketsModel()
                    {
                        CorporateUserID = (CoreContext.CurrentUser as CorporateUser).CorporateUserID,
                        Markets = Market.LoadAllBySiteIDAndUserID(0, (CoreContext.CurrentUser as CorporateUser).CorporateUserID).OrderBy(m => m.MarketID).ToList()
                    };
                }

                return (HttpContext.Current.Session["CurrentUserMarkets"] as CurrentUserMarketsModel).Markets;
            }
        }

        public static int CurrentMarketId
        {
            get { return ApplicationContext.Instance.CurrentMarketID; }
            set { ApplicationContext.Instance.CurrentMarketID = value; }
        }

        public static Market CurrentMarket
        {
            get
            {
                if (BaseContext.GetFromSession<Market>("CurrentMarket") == null ||
                    BaseContext.GetFromSession<Market>("CurrentMarket").MarketID != CurrentMarketId)
                {
                    CurrentMarket = CurrentUserMarkets.Find(m => m.MarketID == CurrentMarketId);
                }

                return BaseContext.GetFromSession<Market>("CurrentMarket");
            }

            set
            {
                BaseContext.SetInSession<Market>("CurrentMarket", value);
            }
        }

        public static Order CurrentOrder
        {
            get
            {
                return IsSessionAvailable
                    ? OrderContextSessionProvider.Get(HttpContext.Current.Session).Order as Order
                    : null;
            }
            set
            {
                if (IsSessionAvailable)
                {
                    (OrderContextSessionProvider.Get(HttpContext.Current.Session) as NetSteps.Data.Entities.Context.OrderContext).Order = value;
                }
            }
        }

        public static NetSteps.Data.Entities.Account CurrentAccount
        {
            get
            {
                return HttpContext.Current.Session["CurrentAccount"] as NetSteps.Data.Entities.Account;
            }
            set
            {
                HttpContext.Current.Session["CurrentAccount"] = value;
            }
        }

        public static AutoshipOrder CurrentAutoship
        {
            get
            {
                return HttpContext.Current.Session["CurrentAutoship"] as AutoshipOrder;
            }

            set
            {
                HttpContext.Current.Session["CurrentAutoship"] = value;
            }
        }

        public static NetSteps.Data.Entities.SupportTicket CurrentSupportTicket
        {
            get
            {
                return HttpContext.Current.Session["SupportTicket"] as NetSteps.Data.Entities.SupportTicket;
            }
            set
            {
                HttpContext.Current.Session["SupportTicket"] = value;
            }
        }
        //protected virtual List<SupportTicketsFilesBE> CurrentFilesSupportTickets
        //{
        //    get { return Session["lstSupportTicketsFilesBE"] as List<SupportTicketsFilesBE>; }
        //    set { Session["lstSupportTicketsFilesBE"] = value; }
        //}
        public static List<SupportTicketsFilesBE> CurrentFilesSupportTickets
        {
            get
            {
                return HttpContext.Current.Session["lstSupportTicketsFilesBE"] as List<SupportTicketsFilesBE>;
            }
            set
            {
                HttpContext.Current.Session["lstSupportTicketsFilesBE"] = value;
            }
        }


        public static int StoreFrontID
        {
            get
            {
                return ApplicationContext.Instance.StoreFrontID;
            }
        }

        public static List<Order> LoadChildReturnOrdersFull(int orderId)
        {
            List<Order> returnOrders = null;
            string cacheKey = string.Format("ChildReturnOrdersOfOrder{0}", orderId);
            if (HttpContext.Current.Cache[cacheKey] == null)
            {
                returnOrders = Order.LoadChildOrdersFull(orderId, NetSteps.Data.Entities.Constants.OrderType.ReturnOrder.ToInt()).Select(o => (Order)o).ToList();
                HttpContext.Current.Cache.Insert(cacheKey, returnOrders, null, DateTime.Now.Add(TimeSpan.FromMinutes(2)), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
                returnOrders = (List<Order>)HttpContext.Current.Cache[cacheKey];

            return returnOrders;
        }

        public static List<Order> LoadChildReplacementOrdersFull(int orderId)
        {
            List<Order> replacementOrders = null;
            string cacheKey = string.Format("ChildReplacementOrdersOfOrder{0}", orderId);
            if (HttpContext.Current.Cache[cacheKey] == null)
            {
				replacementOrders = Order.LoadChildOrdersFull(orderId, NetSteps.Data.Entities.Constants.OrderType.ReplacementOrder.ToInt()).Select(o => (Order)o).ToList();
                HttpContext.Current.Cache.Insert(cacheKey, replacementOrders, null, DateTime.Now.Add(TimeSpan.FromMinutes(2)), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
                replacementOrders = (List<Order>)HttpContext.Current.Cache[cacheKey];

            return replacementOrders;
        }

        public static ICommissionRun CurrentCommissionRun
        {
            get
            {
                return HttpContext.Current.Session["CurrentCommissionRun"] as ICommissionRun;
            }

            set
            {
                HttpContext.Current.Session["CurrentCommissionRun"] = value;
            }
        }

        private static bool IsSessionAvailable
        {
            get
            {
                return HttpContext.Current != null
                    && HttpContext.Current.Session != null;
            }
        }


    }
}
