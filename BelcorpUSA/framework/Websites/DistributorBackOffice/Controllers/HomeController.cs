using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Account.Controllers;
using DistributorBackOffice.Models.Home;
using NetSteps.Common.Configuration;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Extensions;
using NetSteps.Communication.UI.Common;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Business;
using NetSteps.Common.Globalization;

namespace DistributorBackOffice.Controllers
{
	using NetSteps.Common.Models;
	using DistributorBackOffice.Models;
    using Belcorp.Policies.Service.DTO;
    using Belcorp.Policies.Service;
    using NetSteps.Data.Entities.Generated;

	public class HomeController : BaseAccountsController
	{
		public enum GlobalSearchGroups
		{
			News,
			Archives,
			Events,
			Prospects,
			RetailCustomers,
			PreferredCustomers,
			TeamMembers
		}

		public virtual List<AccountReport> CurrentAccountReports
		{
			get
			{
				var currentAccountReports = Session["CurrentAccountReports"] as List<AccountReport>;
				if (currentAccountReports == null && CurrentAccount != null && CurrentAccount.AccountID > 0)
				{
					currentAccountReports = Account.LoadAccountReports(CurrentAccount.AccountID);
					Session["CurrentAccountReports"] = currentAccountReports;
				}

				return currentAccountReports;
			}
			set
			{
				Session["CurrentAccountReports"] = value;
			}
		}

		//[FunctionFilter("Workstation-Home", "~/")]
		public virtual ActionResult Index()
		{
            /*CS.16JUN2016.Inicio*/
            var account = CurrentAccount;
            var policiesService = Create.New<IPoliciesService>();
            AccountPolicyDetailsDTO modelPolicy = policiesService.AccountPolicyDetail(account.AccountID, (int)ConstantsGenerated.AccountType.Distributor, account.DefaultLanguageID);/*R2908 - HUNDRED(JAUF)*/
            if (modelPolicy.IsApplicableAccount && !modelPolicy.IsAcceptedPolicy)
            {
                return RedirectToAction("Index", "Welcome");
            }
            /*CS.16JUN2016.Fin*/
			ViewData["CurrentAccountReports"] = CurrentAccountReports;
			var site = CurrentSite;
            ViewData["News"] =  site.News.Where(n => n.Active && DateTime.Now.ApplicationNow().IsBetween(n.StartDate, n.EndDate) && n.HtmlSection.ProductionContent(site) != null).OrderByDescending(n => n.StartDate);
			ViewData["Advertisement1"] = CurrentPage.HtmlSections.GetBySectionName("Advertisement1");
			ViewBag.AccountQuickFacts = new AccountQuickFacts().ForAccount(CurrentAccount);
			ViewBag.SiteDesignContent = site.GetHtmlSectionByName("SiteDesignContent");

            //INI - Encore_13
            ViewBag.FinPeriodo = Periods.GetPeriodbyID(Periods.GetOpenPeriodID()).EndDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo);
            KPIsPerPeriodSearchData kpis =  KPIperPeriod.GetKPISbyPeriodIdandAccountId(Periods.GetOpenPeriodID(), account.AccountID);
            ViewBag.TitPago = kpis == null ? "" : Translation.GetTerm(kpis.PaidAsCurrentMonth, kpis.PaidAsCurrentMonth);
            ViewBag.VenPersonal = kpis == null ? "" : kpis.PQV;
            ViewBag.VenDivision = kpis == null ? "" : kpis.DQV;
            //FIN - Encore_13


			var sites = Site.LoadByAccountID(CurrentAccount.AccountID);
			if (sites.Count > 0)
			{
				var defaultSite = sites.FirstOrDefault(s => s.SiteStatusID == (int)Constants.SiteStatus.Active);
				if (defaultSite != null)
				{
					//Get the localhost url if we are on dev boxes - DES
					var defaultUrl = Request.Url.Authority.Contains("localhost") && defaultSite.SiteUrls.Any(su => su.Url.Contains("localhost")) ? defaultSite.SiteUrls.FirstOrDefault(su => su.Url.Contains("localhost")) : defaultSite.SiteUrls.FirstOrDefault(su => su.IsPrimaryUrl);
					if (defaultUrl == null && defaultSite.SiteUrls.Count > 0)
					{
						defaultUrl = defaultSite.SiteUrls.FirstOrDefault();
					}
					if (defaultUrl != null)
					{
						ViewBag.SiteUrl = string.Format("{0}Login?token={1}&returnUrl=%2FAdmin",
							defaultUrl.Url.AppendForwardSlash().ConvertToSecureUrl(ConfigurationManager.ForceSSL),
							Account.GetSingleSignOnToken(CurrentAccount.AccountID)).EldEncode();
					}
				}
			}

			var model = new IndexViewModel();

			Index_LoadResources(model);

			return View(model);
		}

		public void Index_LoadResources(IndexViewModel model)
		{
			var allActiveWidgets = GetAllActiveAndEditableSiteWidgets().ToList();
			var userWidgets = GetActiveUserWidgets(allActiveWidgets);
			var alertMessages = GetAlertMessages(CurrentAccount.AccountID);
			var alertModals = GetAlertModals(CurrentAccount.AccountID);

			model.LoadResources(
				CurrentSite,
				ControllerContext,
				allActiveWidgets,
				userWidgets,
				alertMessages,
				alertModals
			);
		}

		public virtual IEnumerable<SiteWidget> GetAllActiveAndEditableSiteWidgets()
		{
			if (ApplicationContext.Instance.UsesEncoreCommissions)
			{
				return CurrentSite.SiteWidgets.Where(sw => sw.Widget.Active && sw.Editable).ToList();
			}
			//Gets all the widgets that do not use commissions data.....
			//CommissionsOverview widget has the WidgetID of 1....
			return CurrentSite.SiteWidgets.Where(sw => sw.Widget.Active && sw.Editable && sw.WidgetID != 1).ToList();
		}

		public virtual IEnumerable<Widget> GetActiveUserWidgets(List<SiteWidget> allActiveWidgets)
		{
			var userWidgets = allActiveWidgets.Where(sw => CoreContext.UserSiteWidgets.All(sw2 => sw2.WidgetID != sw.WidgetID)).Select(sw => sw.Widget).ToList();
			return userWidgets;
		}

		protected virtual IList<IAccountAlertMessageModel> GetAlertMessages(int accountID)
		{
			var accountAlertUIService = Create.New<IAccountAlertUIService>();
			var accountAlertUISearchParameters = Create.New<IAccountAlertUISearchParameters>();
			accountAlertUISearchParameters.AccountId = accountID;
			accountAlertUISearchParameters.PageSize = 5;

			var localizationInfo = Create.New<ILocalizationInfo>();
			localizationInfo.CultureName = CoreContext.CurrentCultureInfo.Name;
			localizationInfo.LanguageId = CoreContext.CurrentLanguageID;

			return accountAlertUIService.GetMessages(accountAlertUISearchParameters, localizationInfo);
		}

		protected virtual IList<IAccountAlertModalModel> GetAlertModals(int accountID)
		{
			var accountAlertUIService = Create.New<IAccountAlertUIService>();
			var accountAlertUISearchParameters = Create.New<IAccountAlertUISearchParameters>();
			accountAlertUISearchParameters.AccountId = accountID;
			accountAlertUISearchParameters.PageSize = 5;

			var localizationInfo = Create.New<ILocalizationInfo>();
			localizationInfo.CultureName = CoreContext.CurrentCultureInfo.Name;
			localizationInfo.LanguageId = CoreContext.CurrentLanguageID;

			return accountAlertUIService.GetModals(accountAlertUISearchParameters, localizationInfo);
		}


		[OutputCache(CacheProfile = "AutoCompleteData")]
		public virtual ActionResult GlobalSearch(string query)
		{
			return Json(NetSteps.Data.Entities.Business.GlobalSearch.Search(CurrentAccount.AccountID, CurrentSite.SiteID, query).GroupBy(o => o.Type).Select(g => new
			{
				id = g.Key.RemoveSpaces(),
				text = g.Key,
				items = g.Select(o => new
				{
					id = o.ID,
					text = o.DisplayText,
					extra = o.ExtraText
				})
			}));
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Autoship Template", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult GetAutoshipOverview(int autoshipScheduleId)
		{
			AutoshipSchedule schedule = GetAutoshipSchedule(autoshipScheduleId);

			AutoshipOrder autoshipOrder = GetAutoshipOrder(autoshipScheduleId);

			var viewModel = new AutoshipOrderViewModel(schedule);

			if (autoshipOrder != null)
			{
				viewModel.AutoshipOrder = autoshipOrder;
				viewModel.IsValid = true;
				viewModel.IsCanceled = IsCanceled(autoshipOrder);
				viewModel.OrderItems = GetOrderItems(autoshipOrder);
				viewModel.Site = GetSite(autoshipOrder, schedule);
			}

			if (schedule != null)
			{
				viewModel.IsTemplateEditable = schedule.IsTemplateEditable;
				viewModel.IsEnrollable = schedule.IsEnrollable;
			}

			return PartialView("AutoshipModule", viewModel);
		}

		[HttpPost]
		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Workstation-Edit Widget Settings", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult SaveWidgetSettings(int siteId, List<int> widgets)
		{
			try
			{
				Account account = CurrentAccount;
				account.StartEntityTracking();

				var userSiteWidgets = account.User.UserSiteWidgets;
				if (widgets != null && widgets.Count > 0)
				{
					userSiteWidgets.SyncTo(widgets, r => r.WidgetID, id => new UserSiteWidget
					{
						UserID = account.User.UserID,
						SiteID = siteId,
						WidgetID = id
					}, sw => sw.MarkAsDeleted());
				}
				else
				{
					userSiteWidgets.RemoveAll();
				}

				account.Save();

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpPost]
		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Workstation-Edit Widget Settings", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult HideWidget(int widgetId)
		{
			try
			{
				var siteWidget = CoreContext.UserSiteWidgets.FirstOrDefault(w => w.WidgetID == widgetId);
				if (siteWidget.ChangeTracker.State != ObjectState.Added)
				{
					siteWidget.MarkAsDeleted();
				}
				siteWidget.Save();
				CoreContext.UserSiteWidgets.Remove(siteWidget);

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpPost]
		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Workstation-Edit Widget Settings", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult ToggleWidget(int widgetID, bool showWidget)
		{
			try
			{
				#region ORIGINAL IMPLEMENTATION
				//Account account = CurrentAccount;

				//var siteWidget = CoreContext.UserSiteWidgets.FirstOrDefault(w => w.WidgetID == widgetId);
				//if (!showWidget)
				//{
				//    if (siteWidget != null)
				//    {
				//        siteWidget.MarkAsDeleted();
				//        siteWidget.Save();
				//        CoreContext.UserSiteWidgets.Remove(siteWidget);
				//    }
				//    return Json(new { result = true });
				//}
				//else
				//{
				//    if (siteWidget == null)
				//    {
				//        siteWidget = new UserSiteWidget()
				//       {
				//           UserID = account.User.UserID,
				//           SiteID = CurrentSite.SiteID,
				//           WidgetID = widgetId
				//       };

				//        siteWidget.Save();
				//        CoreContext.UserSiteWidgets.Add(siteWidget);
				//    }
				//}
				//return Json(new { result = true });
				#endregion

				Account account = CurrentAccount;

				var siteWidget = CoreContext.UserSiteWidgets.FirstOrDefault(w => w.WidgetID == widgetID);

				if (!showWidget)
				{
					// Add to the UserSiteWidget table
					if (siteWidget == null)
					{
						siteWidget = new UserSiteWidget
										 {
											 UserID = account.User.UserID,
											 SiteID = CurrentSite.SiteID,
											 WidgetID = widgetID
										 };
						siteWidget.Save();
						CoreContext.UserSiteWidgets.Add(siteWidget);
					}
					return Json(new { result = true });
				}
				// Remove from the UserSiteWidget table
				if (siteWidget != null)
				{
					if (siteWidget.ChangeTracker.State != ObjectState.Added)
						siteWidget.MarkAsDeleted();
					siteWidget.Save();
					CoreContext.UserSiteWidgets.Remove(siteWidget);
				}
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		#region Helper Methods

		[NonAction]
		public virtual Site GetSite(AutoshipOrder autoshipOrder, AutoshipSchedule schedule)
		{
			return schedule.AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.SiteSubscription
					   ? Site.LoadByAutoshipOrderID(autoshipOrder.AutoshipOrderID)
					   : null;
		}

		[NonAction]
		public virtual AutoshipSchedule GetAutoshipSchedule(int autoshipScheduleId)
		{
			return AutoshipSchedule.LoadFull(autoshipScheduleId);
		}

		[NonAction]
		public virtual bool IsValidAutoship(AutoshipOrder autoshipOrder)
		{
			return autoshipOrder != null && autoshipOrder.AutoshipOrderID > 0;
		}

		[NonAction]
		public bool IsCanceled(AutoshipOrder autoshipOrder)
		{
			return autoshipOrder.Order.OrderStatusID == (short)Constants.OrderStatus.Cancelled;
		}

		[NonAction]
		public IEnumerable<OrderItem> GetOrderItems(AutoshipOrder autoshipOrder)
		{
			return autoshipOrder.Order.OrderCustomers[0].OrderItems;
		}

		[NonAction]
		public virtual AutoshipOrder GetAutoshipOrder(int autoshipScheduleId)
		{
			AutoshipOrder autoshipOrder = AutoshipOrder.LoadFullByAccountIDAndAutoshipScheduleID(CurrentAccount.AccountID, autoshipScheduleId);

			return IsValidAutoship(autoshipOrder) ? autoshipOrder : null;
		}
		#endregion
	}
}
