using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DistributorBackOffice.Controllers;

using NetSteps.Data.Common.Context;
using NetSteps.Data.Entities;
using NetSteps.Web;

namespace DistributorBackOffice.Areas.Account.Controllers
{
	public abstract class BaseAccountsController : BaseController
	{
		/// <summary>
		/// The current order context retrieved from session on each request.
		/// </summary>
		protected virtual IOrderContext OrderContext
		{
			get
			{
				return _orderContext;
			}
		}
		private IOrderContext _orderContext;

		public virtual List<AutoshipSchedule> Schedules
		{
			get
			{
				if (Session["AutoshipSchedules"] == null || (Session["AutoshipSchedules"] as Tuple<int, List<AutoshipSchedule>>).Item1 != CurrentAccount.AccountTypeID)
				{
					Session["AutoshipSchedules"] = new Tuple<int, List<AutoshipSchedule>>(CurrentAccount.AccountTypeID, AutoshipSchedule.LoadByAccountTypeID(CurrentAccount.AccountTypeID));
				}
				return (Session["AutoshipSchedules"] as Tuple<int, List<AutoshipSchedule>>).Item2;
			}
		}

		public virtual List<AutoshipOrder> Autoships
		{
			get
			{
				if (Session["AutoshipOrders"] == null || (Session["AutoshipOrders"] as Tuple<int, List<AutoshipOrder>>).Item1 != CurrentAccount.AccountID)
				{
					Session["AutoshipOrders"] = new Tuple<int, List<AutoshipOrder>>(CurrentAccount.AccountID, AutoshipOrder.LoadAllFullByAccountID(CurrentAccount.AccountID));
				}
				return (Session["AutoshipOrders"] as Tuple<int, List<AutoshipOrder>>).Item2;
			}
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.HttpContext != null
				&& filterContext.HttpContext.Session != null)
			{
				_orderContext = OrderContextSessionProvider.Get(filterContext.HttpContext.Session);
			}

			if (CurrentAccount != null)
			{
				ViewData["Schedules"] = Schedules;
				ViewData["Autoships"] = Autoships;
			}
			base.OnActionExecuting(filterContext);
		}

		protected void ReloadAutoships()
		{
			Session["AutoshipOrders"] = null;
		}
	}
}
