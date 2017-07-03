using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Web.Mvc.Helpers
{
    using NetSteps.Common.Models;
    using NetSteps.Commissions.Common.Models;
    using NetSteps.Commissions.Common;
    using NetSteps.Data.Entities.Business;

	public static class CoreContext
	{
		public static IUser CurrentUser
		{
			get { return (IUser)NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUser; }
			set { NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUser = value; }
		}

		public static List<UserSiteWidget> UserSiteWidgets
		{
			get
			{
				var userSiteWidgets = HttpContext.Current.Session["UserSiteWidgets"] as List<UserSiteWidget>;
				if (userSiteWidgets == null)
				{
					var user = CoreContext.CurrentAccount.User;
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

		public static ILocalizationInfo GetLocalizationInfo()
		{
			var localizationInfo = Create.New<ILocalizationInfo>();
			localizationInfo.CultureName = CurrentCultureInfo.Name;
			localizationInfo.LanguageId = CurrentLanguageID;
			return localizationInfo;
		}

		public static int CurrentMarketId
		{
			get
			{
				if (BaseContext.GetFromSession<int>("CurrentMarketId") == 0 && GetCurrentAccountId() != 0)
				{
					CurrentMarketId = 1;
				}
				return BaseContext.GetFromSession<int>("CurrentMarketId");
			}
			set { BaseContext.SetInSession<int>("CurrentMarketId", value); }
		}

		[Obsolete]
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

        public static IEnumerable<IPeriod> CurrentAccountPeriods
        {
            get
			{
				var commissionsService = Create.New<ICommissionsService>();
                
                bool needsToBeUpdated = true;
                NameValue<int, IEnumerable<IPeriod>> currentAccountPeriods = HttpContext.Current.Session["CurrentAccountPeriods"] as NameValue<int, IEnumerable<IPeriod>>;
				var accountId = GetCurrentAccountId();

				if (currentAccountPeriods != null)
					needsToBeUpdated = false;
				else if (currentAccountPeriods == null && accountId > 0)
					needsToBeUpdated = true;

				if (currentAccountPeriods != null && currentAccountPeriods.Name != accountId)
					needsToBeUpdated = true;

				if (needsToBeUpdated)
				{
					currentAccountPeriods = new NameValue<int, IEnumerable<IPeriod>>()
					{
						Name = accountId,
                        Value = commissionsService.GetPeriodsForAccount(accountId)
					};
					HttpContext.Current.Session["CurrentAccountPeriods"] = currentAccountPeriods;
					return currentAccountPeriods.Value;
				}
				else
					return currentAccountPeriods.Value;
			}
			set
			{
                NameValue<int, IEnumerable<IPeriod>> currentAccountPeriods = new NameValue<int, IEnumerable<IPeriod>>()
				{
					Name = GetCurrentAccountId(),
					Value = value
				};
				HttpContext.Current.Session["CurrentAccountPeriods"] = currentAccountPeriods;
			}
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

		/// <summary>
		/// Sets the current logged on account ID in session.
		/// </summary>
		public static void SetCurrentAccountId(int accountId)
		{
			if (HttpContext.Current != null
				&& HttpContext.Current.Session != null)
			{
				HttpContext.Current.Session["CurrentAccountId"] = accountId;
				ReloadCurrentAccount();
			}
		}

		/// <summary>
		/// Clears the CurrentAccount from session which will trigger a reload
		/// from the database the next time CurrentAccount is accessed.
		/// </summary>
		public static void ReloadCurrentAccount()
		{
			if (HttpContext.Current != null
				&& HttpContext.Current.Session != null)
			{
				HttpContext.Current.Session["CurrentAccount"] = null;
			}
		}

		[Obsolete("Use BaseController.CurrentAccount")]
		public static NetSteps.Data.Entities.Account CurrentAccount
		{
			get
			{
				if (HttpContext.Current != null
					&& HttpContext.Current.Session != null)
				{
					var accountId = GetCurrentAccountId();
					var account = HttpContext.Current.Session["CurrentAccount"] as NetSteps.Data.Entities.Account;
					if (account != null)
					{
						if (account.AccountID == accountId)
						{
							return account;
			}
						else
			{
							// Account IDs don't match.
							HttpContext.Current.Session["CurrentAccount"] = null;
			}
		}
					if (accountId > 0)
					{
						account = NetSteps.Data.Entities.Account.LoadForSession(accountId); // NetSteps.Data.Entities.Account.LoadFull(accountId);
						if (account != null)
						{
							HttpContext.Current.Session["CurrentAccount"] = account;
							return account;
						}
					}
				}
				return null;
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
				var list = AccountListValue.LoadListValuesByTypeAndAccountID(GetCurrentAccountId(), listValueTypeID);

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

		public static IEnumerable<Widget> GetUserWidgets(Site currentSite, bool isOnTop, int? displayColumn = null)
		{
			if (ApplicationContext.Instance.UsesEncoreCommissions)
            {               
                var siteWidgets = currentSite.SiteWidgets.Where(sw => sw.IsOnTop == isOnTop && sw.Widget.Active);

                if (displayColumn.HasValue)
                    siteWidgets = siteWidgets.Where(sw => sw.DisplayColumn == displayColumn);

                return siteWidgets.OrderBy(sw => sw.SortIndex).Select(sw => sw.Widget);
            }
            else
            {
                //Gets all the widgets that do not use commissions data.....
                //CommissionsOverview widget has the WidgetID of 1....
                var siteWidgets = currentSite.SiteWidgets.Where(sw => sw.IsOnTop == isOnTop && sw.Widget.Active && sw.WidgetID != 1);

                if (displayColumn.HasValue)
                    siteWidgets = siteWidgets.Where(sw => sw.DisplayColumn == displayColumn);

                return siteWidgets.OrderBy(sw => sw.SortIndex).Select(sw => sw.Widget);
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

		private static bool IsSessionAvailable
		{
			get
			{
				return HttpContext.Current != null
					&& HttpContext.Current.Session != null;
			}
		}

		public static string CurrentCultureString
		{
			get { return NetSteps.Data.Entities.ApplicationContext.Instance.CurrentCultureInfo; }
			set { NetSteps.Data.Entities.ApplicationContext.Instance.CurrentCultureInfo = value; }
		}


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
    }
}
