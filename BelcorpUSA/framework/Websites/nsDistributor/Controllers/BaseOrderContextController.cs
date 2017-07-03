using System;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web;
using System.Configuration;

namespace nsDistributor.Controllers
{
	public abstract class BaseOrderContextController : BaseController
	{
		private readonly Lazy<IOrderService> _orderServiceFactory = new Lazy<IOrderService>(Create.New<IOrderService>);
		protected IOrderService OrderService { get { return _orderServiceFactory.Value; } }

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

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			// Get OrderContext from session.
			if (filterContext.HttpContext != null
				&& filterContext.HttpContext.Session != null)
			{
				_orderContext = OrderContextSessionProvider.Get(filterContext.HttpContext.Session);
				if (CoreContext.CurrentOrder == null || (ShouldCreateNewOrders && CoreContext.CurrentOrder.OrderStatusID != (int)Constants.OrderStatus.Pending))
				{
					CoreContext.CurrentOrder = CreateNewOrder();
				}
				else
				{
					if (CoreContext.CurrentOrder.ConsultantID == 0)
					{
						CoreContext.CurrentOrder.SetConsultantID(Account, SiteOwner);
					}
				}
				_orderContext.Order = CoreContext.CurrentOrder;
			}
		}

		protected virtual bool ShouldCreateNewOrders
		{
			get
			{
				return true;
			}
		}

        protected Order CreateNewOrder()
        {
            if (OrderContext != null && OrderContext.Order != null)
            {
                OrderContext.Clear();
            }
            var currentSite =
            this.GetCurrentSiteErrorOnNull();
            Order order = new Order(Account) { CurrencyID = SmallCollectionCache.Instance.Markets.GetById(currentSite.MarketID).GetDefaultCurrencyID(), SiteID = currentSite.SiteID };
            order.SetConsultantID(Account, SiteOwner);
            //This is to just default the order shipment tosomething in the same market so we can see the applicable host rewards- DES
            var countryInMarket =
            SmallCollectionCache.Instance.Countries.FirstOrDefault(c => c.MarketID == currentSite.MarketID && c.Active);
            if (countryInMarket != null)
            {
                var state = new StateProvince();
                var estaHabilitado =
                Convert.ToBoolean(ConfigurationManager.AppSettings["ManejaElasticache"]);
                if (estaHabilitado)
                {
                    state =
                    SmallCollectionCache.Instance.StateProvinces.Where(s =>
                    s.ShippingRegionID.HasValue).FirstOrDefault(s =>
                    s.CurrentTimeZoneInfo.Equals(currentSite.CurrentTimeZoneInfo) &&
                    s.CountryID == countryInMarket.CountryID);
                }
                else
                {
                    state =
                    SmallCollectionCache.Instance.StateProvinces.Where(s =>
                    s.ShippingRegionID.HasValue).FirstOrDefault(s => s.CurrentTimeZoneInfo
                    == currentSite.CurrentTimeZoneInfo && s.CountryID ==
                    countryInMarket.CountryID);
                } 
                OrderShipment shipment = order.GetDefaultShipment(); 
            }
            return order;
        }

		public void ResetContext()
		{
			OrderContext.Clear();
		}
	}
}