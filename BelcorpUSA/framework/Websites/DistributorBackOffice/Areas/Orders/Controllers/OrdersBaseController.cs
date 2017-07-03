using System;
using System.Diagnostics.Contracts;
using System.Web;
using System.Web.Mvc;
using DistributorBackOffice.Controllers;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web;

namespace DistributorBackOffice.Areas.Orders.Controllers
{
	public abstract class OrdersBaseController : BaseController
	{
		private readonly Lazy<IOrderService> _orderServiceFactory = new Lazy<IOrderService>(Create.New<IOrderService>);
		protected IOrderService OrderService { get { return _orderServiceFactory.Value; } }

		private IOrderContext _orderContext;
		/// <summary>
		/// The current order context retrieved from session on each request.
		/// </summary>
		protected virtual IOrderContext OrderContext
		{
			get
			{
				if (_orderContext == null && HttpContext != null && HttpContext.Session != null)
				{
					_orderContext = OrderContextSessionProvider.Get(HttpContext.Session);
				}

				return _orderContext;
			}
			set
			{
				_orderContext = value;
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public OrdersBaseController()
		{ }

		/// <summary>
		/// Constructor with dependency injection.
		/// </summary>
		/// <param name="orderContext">The order context.</param>
		public OrdersBaseController(IOrderContext orderContext)
		{
			Contract.Requires<ArgumentNullException>(orderContext != null);

			OrderContext = orderContext;
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);

			if (_orderContext != null && HttpContext != null && HttpContext.Session != null)
			{
				OrderContextSessionProvider.Set(HttpContext.Session, _orderContext);
			}
		}

		protected virtual bool CurrentAccountCanAccessParty(Party party)
		{
			// Newly-created parties have no order until an order is created separately and added.
			return party != null && (party.Order == null || CurrentAccountCanAccessOrder(party.Order));
		}

		protected virtual bool CurrentAccountCanAccessOrder(Order order)
		{
			return order != null && order.ConsultantID == CurrentAccountId;
		}

		protected virtual ActionResult RedirectToSafePage()
		{
			// Bounce them out to the Order History screen
			OrderContext.Order = null;
			TempData["Error"] = _errorOrderNotFound;
			return RedirectToAction("Index", "OrderHistory");
		}

		public virtual ActionResult NewOrder()
		{
			OrderContext.Clear();

			return RedirectToAction("Index");
		}

		protected virtual string _errorOrderNotFound { get { return Translation.GetTerm("ErrorOrderNotFound", "The order could not be found."); } }
	}
}
