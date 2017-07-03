using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Web;

namespace nsDistributor
{
    public static class CoreContext
    {
        public static IUser CurrentUser
        {
            get { return (IUser)NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUser; }
            set { NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUser = value; }
        }

        /// <summary>
        /// Gets the current logged on account ID from session.
        /// </summary>
        public static int GetCurrentAccountId()
        {
            int result = 0;
            if (HttpContext.Current != null
                && HttpContext.Current.Session != null)
            {
                result = (HttpContext.Current.Session["CurrentAccountId"] as int?) ?? 0;
            }
            return result;
        }

        public static List<UserSiteWidget> UserSiteWidgets
        {
            get
            {
                var userSiteWidgets = HttpContext.Current.Session["UserSiteWidgets"] as List<UserSiteWidget>;
                if (userSiteWidgets == null)
                {
                    var user = (CoreContext.CurrentUser as NetSteps.Data.Entities.Account).User;
                    userSiteWidgets = User.LoadUserSiteWigets(user.UserID);
                    HttpContext.Current.Session["UserSiteWidgets"] = userSiteWidgets;
                }
                return userSiteWidgets;
            }
            set { HttpContext.Current.Session["UserSiteWidgets"] = value; }
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

        public static int CurrentMarketId
        {
            get { return ApplicationContext.Instance.CurrentMarketID; }
            set { ApplicationContext.Instance.CurrentMarketID = value; }
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

        public static IOrderContext CurrentOrderContext
        {
            get
            {
                return IsSessionAvailable ? OrderContextSessionProvider.Get(HttpContext.Current.Session) : null;
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

        public static NetSteps.Data.Entities.Account SiteOwner
        {
            get
            {
                return HttpContext.Current.Session["SiteOwner"] as NetSteps.Data.Entities.Account;
            }
            set
            {
                HttpContext.Current.Session["SiteOwner"] = value;
            }
        }

        public static Site CurrentSite
        {
            get
            {
                return HttpContext.Current.Session["CurrentSite"] as Site;
            }
            set
            {
                HttpContext.Current.Session["CurrentSite"] = value;
            }
        }

        public static bool HasModalBeenShown
        {
            get
            {
                if (HttpContext.Current.Session["HasModalBeenShown"] == null)
                {
                    return false;
                }
                return (bool)HttpContext.Current.Session["HasModalBeenShown"];
            }
            set
            {
                HttpContext.Current.Session["HasModalBeenShown"] = value;
            }
        }

        private static Dictionary<short, List<AccountListValue>> _currentAccountListValues
        {
            get
            {
                return HttpContext.Current.Session["CurrentAccountListValues"] as Dictionary<short, List<AccountListValue>>;
            }
            set
            {
                HttpContext.Current.Session["CurrentAccountListValues"] = value;
            }
        }
        public static List<AccountListValue> GetCurrentAccountListValuesByType(short listValueTypeID)
        {
            if (_currentAccountListValues == null)
                _currentAccountListValues = new Dictionary<short, List<AccountListValue>>();

            if (!_currentAccountListValues.ContainsKey(listValueTypeID))
            {
                var list = AccountListValue.LoadListValuesByTypeAndAccountID(CurrentAccount.AccountID, listValueTypeID);

                _currentAccountListValues.Add(listValueTypeID, list);
                _currentAccountListValues = _currentAccountListValues; // Save back to session. - JHE
            }

            if (_currentAccountListValues[listValueTypeID].Count > 0 && _currentAccountListValues[listValueTypeID][0].IsCorporate)
                return SmallCollectionCache.Instance.CorporateAccountListValues.Where(alv => alv.ListValueTypeID == listValueTypeID).ToList();
            else
                return _currentAccountListValues[listValueTypeID];
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

        public static int StoreFrontID
        {
            get
            {
                return ApplicationContext.Instance.StoreFrontID;
            }
        }

        //public static Commissions CurrentCommissions
        //{
        //    get
        //    {
        //        Commissions result = BaseContext.GetFromApplication<Commissions>("CurrentCommissions");
        //        if (result == null)
        //        {
        //            CurrentCommissions = new Commissions();
        //            return CurrentCommissions;
        //        }

        //        return result;
        //    }
        //    set { BaseContext.SetInApplication<Commissions>("CurrentCommissions", value); }
        //}

        public static List<Widget> GetUserWigets(Site currentSite, bool isOnTop, int? displayColumn = null)
        {
            var siteWigets = currentSite.SiteWidgets.Where(sw => sw.IsOnTop == isOnTop && sw.Widget.Active);
            if (displayColumn != null)
                siteWigets = siteWigets.Where(sw => sw.DisplayColumn == displayColumn);

            var user = (CoreContext.CurrentUser as NetSteps.Data.Entities.Account).User;
            var userWidgets = UserSiteWidgets;
            if (userWidgets.Count != 0)
                siteWigets = siteWigets.Where(sw => userWidgets.Select(sw2 => sw2.WidgetID).Contains(sw.WidgetID));

            return siteWigets.OrderBy(sw => sw.SortIndex).Select(sw => sw.Widget).ToList();
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
