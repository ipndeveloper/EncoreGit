using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Dynamic;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Context;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components;
using NetSteps.Promotions.Service;
using NetSteps.Promotions.UI.Common.Interfaces;
using NetSteps.Web;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Orders.Models;
using nsCore.Controllers;
using NetSteps.Web.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.Logic;

namespace nsCore.Areas.Orders.Controllers
{
	public abstract class OrdersBaseController : BaseController
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public OrdersBaseController()
		{

		}

		/// <summary>
		/// Constructor with dependency injection.
		/// </summary>
		/// <param name="orderContext">The order context.</param>
		public OrdersBaseController(IOrderContext orderContext)
		{
			Contract.Requires<ArgumentNullException>(orderContext != null);

			_orderContext = orderContext;
		}

		#endregion

		#region Properties

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
			}
		}

		public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }
		public IShippingCalculator ShippingCalculator { get { return Create.New<IShippingCalculator>(); } }
		public IOrderService OrderService { get { return Create.New<IOrderService>(); } }
        void calculateDispatch()
        {
            var dispatchs = OrderExtensions.GetDispatchByOrder(OrderContext.Order.OrderID);
            int countDispatch = 0;
            decimal totalDispatch = 0;
            foreach (var dispa in dispatchs)
            {
                foreach (var oi in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Where(x => x.OrderItemID == dispa.OrderItemID))
                {
                    totalDispatch = totalDispatch + (oi.ItemPrice * oi.Quantity);
                    countDispatch++;
                    oi.ShippingTotal = 0;
                    oi.TaxAmount = 0;
                    oi.ShippingTotalOverride = 0;
                    oi.ItemPrice = 0;
                }
            }
            OrderContext.Order.Subtotal = OrderContext.Order.AsOrder().Subtotal - totalDispatch;
            OrderContext.Order.GrandTotal = OrderContext.Order.AsOrder().GrandTotal - totalDispatch;
            OrderContext.Order.AsOrder().OrderCustomers[0].AdjustedSubTotal = OrderContext.Order.AsOrder().OrderCustomers[0].AdjustedSubTotal - totalDispatch;
            //OrderContext.Order.AsOrder().OrderCustomers[0].ProductSubTotal =  OrderContext.Order.AsOrder().OrderCustomers[0].ProductSubTotal - totalDispatch;
        }

        protected virtual object Totals
        {
            get
            {
                Order order = OrderContext.Order.AsOrder();
                CalculateQV_CV_Total();
                if (order == null)
                    return null;
                decimal paymentTotal = order.OrderCustomers[0].OrderPayments.Where(p => p.OrderPaymentStatusID != (int)Constants.OrderPaymentStatus.Cancelled && p.OrderPaymentStatusID != (int)Constants.OrderPaymentStatus.Declined).Sum(p => p.Amount);

                decimal balance = order.GrandTotal.GetRoundedNumber() - paymentTotal;
                if (paymentTotal > order.GrandTotal.GetRoundedNumber())
                {
                    if (balance < 0)
                        balance = balance * (-1);
                }
                else
                {
                    if (balance > 0)
                        balance = balance * (-1);
                }

                int numberofPayment = OrderContext.Order.AsOrder().OrderPayments.Where(x => x.BankName != "PREVIOS BALANCE").ToList().Count();
                //var dispatchs = OrderExtensions.GetDispatchByOrder(OrderContext.Order.OrderID); 
                //if (dispatchs.Count > 0)
                //{
                //    calculateDispatch();
                //    decimal balances = order.GrandTotal.GetRoundedNumber() - paymentTotal;
                //    if (paymentTotal > order.GrandTotal.GetRoundedNumber())
                //    {
                //        if (balances < 0)
                //            balances = balances * (-1);
                //    }
                //    else
                //    {
                //        if (balances > 0)
                //            balances = balances * (-1);
                //    }
                //    return new
                //    {
                //        adjustedSubtotal = order.Subtotal.ToString(order.CurrencyID),
                //        subtotal = order.Subtotal.ToDecimal().GetRoundedNumber().ToString(order.CurrencyID),
                //        commissionableTotal = order.CommissionableTotal.ToDecimal().GetRoundedNumber().ToString(order.CurrencyID),
                //        qualificationTotal = order.QualificationTotal.ToDecimal().GetRoundedNumber().ToString(order.CurrencyID),
                //        taxTotal = (order.TaxAmountTotalOverride != null) ? order.TaxAmountTotalOverride.ToString(order.CurrencyID) : order.TaxAmountTotal.GetRoundedNumber().ToString(order.CurrencyID),
                //        shippingTotal = (order.ShippingTotalOverride != null) ? order.ShippingTotalOverride.ToString(order.CurrencyID) : order.ShippingTotal.ToString(order.CurrencyID),
                //        handlingTotal = order.HandlingTotal.ToString(order.CurrencyID),
                //        grandTotal = order.GrandTotal.GetRoundedNumber().ToString(order.CurrencyID),
                //        paymentTotal = order.OrderCustomers[0].OrderPayments.Where(p => p.OrderPaymentStatusID != (int)Constants.OrderPaymentStatus.Cancelled && p.OrderPaymentStatusID != (int)Constants.OrderPaymentStatus.Declined && p.Amount > 0).Sum(p => p.Amount).ToString(order.CurrencyID),
                //        balanceDue = (balances).GetRoundedNumber().ToString(order.CurrencyID),
                //        balanceAmount = (balances).GetRoundedNumber(),
                //        numberOfItems = (order.OrderCustomers.Sum(oc => oc.ParentOrderItems.Sum(oi => oi.Quantity))),
                //        //CGI(CMR)-09/04/2015-Inicio
                //        sumAdjustedItemPrice = order.OrderCustomers[0].SumAdjustedItemPrice.GetRoundedNumber().ToString(order.CurrencyID),
                //        sumPreadjustedItemPrice = order.OrderCustomers[0].SumPreadjustedItemPrice.GetRoundedNumber().ToString(order.CurrencyID), 
                //        subTotalAdjustedItemPrice = order.OrderCustomers[0].SubTotalAdjustedItemPrice.GetRoundedNumber().ToString(order.CurrencyID),
                //        subTotalPreadjustedItemPrice = order.OrderCustomers[0].SubTotalPreadjustedItemPrice.GetRoundedNumber().ToString(order.CurrencyID), 
                //        subTotalAdjustedItemPrice_Text = "(" + Translation.GetTerm("SubTotalDTO", "Sub total dto") + ")",
                //        subTotalPreadjustedItemPrice_Text = "(" + Translation.GetTerm("SubTotal", "Sub total") + ")"

                //        //CGI(CMR)-09/04/2015-Fin
                //    };
                //}
                //else
                //{
                return new
                {
                    adjustedSubtotal = order.OrderCustomers[0].AdjustedSubTotal.ToString(order.CurrencyID),
                    subtotal = order.OrderCustomers[0].Subtotal.ToDecimal().GetRoundedNumber().ToString(order.CurrencyID),
                    commissionableTotal = order.CommissionableTotal.ToDecimal().GetRoundedNumber().ToString(order.CurrencyID),
                    qualificationTotal = order.QualificationTotal.ToDecimal().GetRoundedNumber().ToString(order.CurrencyID),
                    taxTotal = (order.TaxAmountTotalOverride != null) ? order.TaxAmountTotalOverride.ToString(order.CurrencyID) : order.TaxAmountTotal.GetRoundedNumber().ToString(order.CurrencyID),
                    shippingTotal = (order.ShippingTotalOverride != null) ? order.ShippingTotalOverride.ToString(order.CurrencyID) : order.ShippingTotal.ToString(order.CurrencyID),
                    handlingTotal = order.HandlingTotal.ToString(order.CurrencyID),
                    grandTotal = order.GrandTotal.GetRoundedNumber().ToString(order.CurrencyID),
                    paymentTotal = order.OrderCustomers[0].OrderPayments.Where(p => p.OrderPaymentStatusID != (int)Constants.OrderPaymentStatus.Cancelled && p.OrderPaymentStatusID != (int)Constants.OrderPaymentStatus.Declined && p.Amount > 0).Sum(p => p.Amount).ToString(order.CurrencyID),
                    balanceDue = (balance).GetRoundedNumber().ToString(order.CurrencyID),
                    balanceAmount = (balance).GetRoundedNumber(),
                    numberOfItems = (order.OrderCustomers.Sum(oc => oc.ParentOrderItems.Sum(oi => oi.Quantity))),
                    //CGI(CMR)-09/04/2015-Inicio
                    sumAdjustedItemPrice = order.OrderCustomers[0].SumAdjustedItemPrice.GetRoundedNumber().ToString(order.CurrencyID),
                    sumPreadjustedItemPrice = order.OrderCustomers[0].SumPreadjustedItemPrice.GetRoundedNumber().ToString(order.CurrencyID),

                    subTotalAdjustedItemPrice = order.OrderCustomers[0].SubTotalAdjustedItemPrice.GetRoundedNumber().ToString(order.CurrencyID),
                    subTotalPreadjustedItemPrice = order.OrderCustomers[0].SubTotalPreadjustedItemPrice.GetRoundedNumber().ToString(order.CurrencyID),

                    subTotalAdjustedItemPrice_Text = "(" + Translation.GetTerm("SubTotalDTO", "Sub total dto") + ")",
                    subTotalPreadjustedItemPrice_Text = "(" + Translation.GetTerm("SubTotal", "Sub total") + ")",
                    numberofPayment = numberofPayment
                    //CGI(CMR)-09/04/2015-Fin
                };
                //} 
            }
        }

        private void CalculateQV_CV_Total()
        {
            decimal sumQV = 0, sumCV = 0;
            foreach (var item in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
            {
                foreach (var price in item.OrderItemPrices)
                {
                    if (price.ProductPriceTypeID == 21) sumQV += price.UnitPrice * item.Quantity;
                    if (price.ProductPriceTypeID == 18) sumCV += price.UnitPrice * item.Quantity;
                }
            }
            OrderContext.Order.AsOrder().QualificationTotal = sumQV;
            OrderContext.Order.AsOrder().CommissionableTotal = sumCV;
            //Model.Order.OrderCustomers[0].SubTotalQV
            OrderContext.Order.AsOrder().OrderCustomers[0].SubTotalQV = sumQV.FormatGlobalizationDecimal();
        }

        protected virtual object Totals2
        {
            get
            {
                Order order = OrderContext.Order.AsOrder();
                if (order == null)
                    return null;
                decimal paymentTotal = order.OrderCustomers[0].OrderPayments.Where(p => p.OrderPaymentStatusID != (int)Constants.OrderPaymentStatus.Cancelled && p.OrderPaymentStatusID != (int)Constants.OrderPaymentStatus.Declined).Sum(p => p.Amount);
                return new
                {
                    adjustedSubtotal = order.OrderCustomers[0].AdjustedSubTotal.ToString(order.CurrencyID),
                    subtotal = order.OrderCustomers[0].Subtotal.ToDecimal().GetRoundedNumber().ToString(order.CurrencyID),
                    commissionableTotal = order.CommissionableTotal.ToDecimal().GetRoundedNumber().ToString(order.CurrencyID),
                    taxTotal = (order.TaxAmountTotalOverride != null) ? order.TaxAmountTotalOverride.ToString(order.CurrencyID) : order.TaxAmountTotal.GetRoundedNumber().ToString(order.CurrencyID),
                    shippingTotal = (order.ShippingTotalOverride != null) ? order.ShippingTotalOverride.ToString(order.CurrencyID) : order.ShippingTotal.ToString(order.CurrencyID),
                    handlingTotal = order.HandlingTotal.ToString(order.CurrencyID),
                    grandTotal = order.GrandTotal.GetRoundedNumber().ToString(order.CurrencyID),
                    paymentTotal = paymentTotal.ToString(order.CurrencyID),
                    balanceDue = Convert.ToDecimal(0).ToString(order.CurrencyID),//(order.GrandTotal.GetRoundedNumber() - paymentTotal).GetRoundedNumber().ToString(order.CurrencyID),
                    balanceAmount = 0, //(order.GrandTotal.GetRoundedNumber() - paymentTotal).GetRoundedNumber(),
                    numberOfItems = (order.OrderCustomers.Sum(oc => oc.ParentOrderItems.Sum(oi => oi.Quantity))),
                    //CGI(CMR)-09/04/2015-Inicio
                    sumAdjustedItemPrice = order.OrderCustomers[0].SumAdjustedItemPrice.GetRoundedNumber().ToString(order.CurrencyID),
                    sumPreadjustedItemPrice = order.OrderCustomers[0].SumPreadjustedItemPrice.GetRoundedNumber().ToString(order.CurrencyID),

                    subTotalAdjustedItemPrice = order.OrderCustomers[0].SubTotalAdjustedItemPrice.GetRoundedNumber().ToString(order.CurrencyID),
                    subTotalPreadjustedItemPrice = order.OrderCustomers[0].SubTotalPreadjustedItemPrice.GetRoundedNumber().ToString(order.CurrencyID),

                    subTotalAdjustedItemPrice_Text = "(" + Translation.GetTerm("SubTotalDTO", "Sub total dto") + ")",
                    subTotalPreadjustedItemPrice_Text = "(" + Translation.GetTerm("SubTotal", "Sub total") + ")"

                    //CGI(CMR)-09/04/2015-Fin
                };
            }
        }

        protected virtual string ShippingMethods
        {
            get
            {
                OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipment();

                OrderCustomer customer = OrderContext.Order.AsOrder().OrderCustomers[0];

                // These should never be null
                if (shipment == null || shipment.IsEmpty(true) || customer == null || !OrderContext.Order.AsOrder().OrderCustomers.SelectMany(x => x.OrderItems).Any(x => Inventory.GetProduct(x.ProductID ?? 0).ProductBase.IsShippable))
                {
                    return string.Empty;
                }

                StringBuilder builder = new StringBuilder();
                Boolean vGenShipping = Convert.ToBoolean(Session["vControlGenShipping"]);
                if (vGenShipping == false)
                {
                    string valShiping = Session["ValorShiping"].ToString();
                    if (valShiping == "N")
                        Session["vControlGenShipping"] = true;

                    var shippingMethods = ShippingCalculator.GetShippingMethodsWithRates(customer, shipment).OrderBy(x => x.ShippingAmount);

                    // If shipping methods have changed, this will make sure the selected shipping method is still valid.
                    OrderContext.Order.AsOrder().ValidateOrderShipmentShippingMethod(shipment, shippingMethods);
                    OrderService.UpdateOrder(OrderContext);
                    foreach (var item in OrderContext.Order.AsOrder().OrderShipments)
                    {
                        foreach (var sp in shippingMethods.Where(x => x.ShippingMethodID == item.ShippingMethodID))
                        {
                            item.DateShipped = Convert.ToDateTime(sp.DateEstimated);
                        }
                    }
                    var shipmentAdjustmentAmount = OrderContext.Order.AsOrder().OrderCustomers[0].ShippingAdjustmentAmount;

                    string dateEstimated = "";
                    foreach (var shippingMethod in shippingMethods)
                    {

                        builder
                            .Append("<div class=\"FL AddressProfile\"><input value=\"")
                            .Append(shippingMethod.ShippingMethodID)
                            .Append(shipment.ShippingMethodID == shippingMethod.ShippingMethodID ? "\" checked=\"checked" : "")
                            .Append("\" type=\"radio\" name=\"shippingMethod\" class=\"Radio\" /><b>")
                            .Append(shippingMethod.Name)
                            .Append("</b><br />")
                            .Append(shipmentAdjustmentAmount == 0
                                ? shippingMethod.ShippingAmount.ToString(OrderContext.Order.CurrencyID)
                                : String.Format("<span class=\"shipMethodPrice originalPrice strikethrough\">{0}</span>&nbsp;<span class=\"shipMethodPrice discountPrice\">{1}</span></label>", shippingMethod.ShippingAmount.ToString(OrderContext.Order.CurrencyID), (shippingMethod.ShippingAmount - shipmentAdjustmentAmount).ToString(OrderContext.Order.CurrencyID)))
                            .Append("</div>");
                        Session["vbuilder"] = builder.ToString();
                    }
                }
                else
                    builder.Append(Session["vbuilder"]);
                return builder.ToString();

            }
        }

		protected virtual string _errorInvalidPromotionCode(string promotionCode) { return Translation.GetTerm("ErrorInvalidPromotionCode", "The promotion could not be applied. Invalid promotion code: '{0}'.", promotionCode); }
		protected virtual string _errorNoItemsInOrder { get { return Translation.GetTerm("PleaseAddItemsToOrderBeforeUpdating", "Please add items to Order before updating."); } }

		/// <summary>
		/// Gets or sets _availableBundleCount.
		/// </summary>
		protected int _availableBundleCount { get; set; }

		#endregion

		#region Helper Methods

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetAddresses()
        {
            try
            {
                StringBuilder addresses = new StringBuilder();
                StringBuilder options = new StringBuilder();

                foreach (Address address in CoreContext.CurrentAccount.Addresses.Where(a => a.AddressTypeID == Constants.AddressType.Shipping.ToInt()).OrderByDescending(a => a.IsDefault).ThenBy(a => a.AddressID))
                {
                    addresses.Append("<div id=\"shippingAddress").Append(address.AddressID).Append("\" class=\"shippingAddressDisplay\">")
                        .Append("<b>").Append(address.ProfileName).Append("</b> - ")
                        .Append("<a title=\"Edit\" style=\"cursor: pointer;\" onclick=\"editAddress(").Append(address.AddressID).Append(");\">Edit</a>")
                        .Append("<br />")
                        .Append(address.ToString().ToHtmlBreaks())
                        .Append("</div>");

                    options.Append("<option value=\"").Append(address.AddressID).Append("\">").Append(address.ProfileName);
                    if (address.IsDefault)
                        options.Append(" (default)");
                    options.Append("</option>");
                }

                return Json(new { options = options.ToString(), addresses = addresses.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpPost]
        [FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
        public virtual ActionResult SaveAddress(int? addressId, string profileName, string attention, string address1, string address2,
            string address3, string postalCode, string city, string county, string state, string street, int countryId, string phone)
        {
            try
            {
                Address address = null;
                //var account = CoreContext.CurrentAccount;
                Account account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);

                account.StartEntityTracking();
                if (addressId.HasValue && addressId > 0)
                {
                    address = account.Addresses.GetByAddressID(addressId.Value);
                }
                else
                {
                    address = new Address();
                    account.Addresses.Add(address);
                }
                address.StartEntityTracking();

                address.AttachAddressChangedCheck();
                address.ProfileName = profileName.ToCleanString();
                address.Attention = attention.ToCleanString();
                address.Address1 = address1.ToCleanString();
                address.Address2 = address2.ToCleanString();
                address.Address3 = address3.ToCleanString();
                address.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Shipping.ToShort();
                address.City = city.ToCleanString();
                address.SetState(state, countryId);
                address.County = county.ToCleanString();
                address.PostalCode = postalCode.ToCleanString();
                address.CountryID = countryId;
                address.PhoneNumber = phone.ToCleanString();
                address.LookUpAndSetGeoCode();
                if (!address.IsValid)
                {
                    return Json(new
                    {
                        result = false,
                        message = address.GetValidationErrorMessage()
                    });
                }
                var result = address.ValidateAddressAccuracy();
                if (!result.Success)
                    return Json(new { result = false, message = result.Message });

                account.Save();

                UpdateAddressStreet(account, street, address.AddressID);

                CoreContext.CurrentAccount = account;

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void UpdateAddressStreet(Account account, string street, int addressID)
        {
            Address direccion = new Address();
            direccion = account.Addresses.Where(donde => donde.AddressID == addressID).FirstOrDefault();
            if (direccion != null)
            {
                direccion.Street = street;
                AddressBusinessLogic business = new AddressBusinessLogic();
                business.UpdateAddressStreet(direccion);
            }
        }

        protected virtual void AddOrUpdateOrderItems(IEnumerable<IOrderItemQuantityModification> changes)
        {
            try
            {
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                OrderService.UpdateOrderItemQuantities(OrderContext, changes);
            }
            catch (Exception ex)
            {
                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                else
                    exception = ex as NetStepsException;
                throw exception;
            }
        }

		protected virtual string GetProductShortDescriptionDisplay(Product product)
		{
			Contract.Requires<ArgumentNullException>(product != null);
			string shortDesc = product.Translations.ShortDescription();
			if (shortDesc.IsNullOrWhiteSpace())
			{
				shortDesc = product.ProductBase.Translations.ShortDescription();
			}

			return shortDesc ?? string.Empty;
		}

		protected virtual IEnumerable<string> GetOutOfStockItemsString(Order o)
		{
			return o.OrderCustomers.SelectMany(oi => oi.ParentOrderItems)
                    .Where(oi => Inventory.IsOutOfStock(oi.ProductID.Value)/* && !Inventory.IsAvailable(oi.ProductID ?? 0)*/&& !oi.HasChildOrderItems)
					.Select(oi => oi.SKU + " - " + oi.ProductName);
		}

		protected virtual void UpdateOrderType(bool isAutoship)
		{
			var shipment = OrderContext.Order.AsOrder().GetDefaultShipment();
			var customer = OrderContext.Order.AsOrder().OrderCustomers[0];

			var newOrderTypeID = isAutoship
					? AutoshipOrder.GetDefaultGeneratedAutoshipOrderTypeID(customer.AccountTypeID)
					: Order.GetStandardOrderTypeID(customer);

			if (OrderContext.Order.AsOrder().OrderTypeID != newOrderTypeID)
			{
				OrderContext.Order.AsOrder().OrderTypeID = newOrderTypeID;
				// Ensure that shipping method is valid
				OrderContext.Order.AsOrder().ValidateOrderShipmentShippingMethod(shipment, customer);
				// Reset payments because shipping amounts may have changed.
				customer.RemoveAllPayments();
				OrderService.UpdateOrder(OrderContext);
			}
		}

		protected virtual void UpdateOrderShipmentAddress(int addressId)
		{
			OrderContext.Order.AsOrder().StartEntityTracking();
			var shipment = OrderContext.Order.AsOrder().GetDefaultShipment();
			shipment.StartEntityTracking();
			var customer = OrderContext.Order.AsOrder().OrderCustomers[0];
			customer.StartEntityTracking();

			OrderContext.Order.AsOrder().UpdateOrderShipmentAddress(shipment, addressId);
			// Ensure that shipping method is valid
			OrderContext.Order.AsOrder().ValidateOrderShipmentShippingMethod(shipment, customer);
			// Reset payments because shipping amounts may have changed.
			customer.RemoveAllPayments();
			OrderService.UpdateOrder(OrderContext);
		}

		protected virtual IEnumerable<ShippingMethodWithRate> GetNewShippingMethods(int addressId)
		{
			var shippingMethods = OrderContext.Order.AsOrder().UpdateOrderShipmentAddressAndDefaultShipping(addressId);
			OrderService.UpdateOrder(OrderContext);

			return shippingMethods;
		}

		protected virtual object GetApplicablePromotions(Order order)
		{
			var promotionAdjustments = order.OrderAdjustments.Where(adjustment => adjustment.ExtensionProviderKey == PromotionProvider.ProviderKey);
			var adjustments = promotionAdjustments.Where(adjustment => adjustment.OrderAdjustmentOrderLineModifications.Any() || adjustment.OrderAdjustmentOrderModifications.Any() || adjustment.InjectedOrderSteps.Any());
			return adjustments.Select(adj =>
			{
				bool isAvailable = false;
				if (adj.InjectedOrderSteps.Count() > 0)
				{
					foreach (var step in adj.InjectedOrderSteps)
					{
						isAvailable = StepHasAnItemInStockToBeChosen(step.OrderStepReferenceID);
						if (isAvailable)
							break;
					}
				}
				else
				{
					isAvailable = !adj.OrderModifications.Any(om => om.ModificationDescription.Contains("Unable"));
				}
				var giftStep = adj.InjectedOrderSteps.FirstOrDefault(os => os is IUserProductSelectionOrderStep &&
					(os.Response == null || (os.Response is IUserProductSelectionOrderStepResponse && (os.Response as IUserProductSelectionOrderStepResponse).SelectedOptions.Count == 0)));
				return new { Description = adj.Description, StepID = giftStep == null ? null : giftStep.OrderStepReferenceID.ToString(), isAvailable };
			});
		}

		protected virtual string GetPromotionsHtml(Order order)
		{
			List<IDisplayInfo> promoDisplayInfos = GetPromotionsDisplayInfo();
			string promotionsHtml = RenderRazorPartialViewToString("~/Views/Promotions/_AppliedPromotions.cshtml", promoDisplayInfos);
			return promotionsHtml;
		}

		protected virtual List<IDisplayInfo> GetPromotionsDisplayInfo()
		{
			return new List<IDisplayInfo>();
		}

        protected virtual List<object> GetOrderItemsHtml(Order order)
        {
            ViewDataDictionary vdd = new ViewDataDictionary();
            List<object> orderItems = new List<object>();
            OrderCustomer orderCustomer = OrderContext.Order.AsOrder().OrderCustomers[0];
            var addedItemOperationID = (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem;
            var nonPromotionalItems = orderCustomer.ParentOrderItems.Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == addedItemOperationID));
            var promotionalItems = orderCustomer.ParentOrderItems.Except(nonPromotionalItems).ToList();
            var adjustments = promotionalItems.GroupBy(x => x.OrderAdjustmentOrderLineModifications.Single(y => y.ModificationOperationID == addedItemOperationID).OrderAdjustment);

            // ------------------------------------------------------------------------------------------------
            List<NetSteps.Data.Entities.Business.HelperObjects.SearchData.DispatchProducts> itemsProductsDispatch = new List<NetSteps.Data.Entities.Business.HelperObjects.SearchData.DispatchProducts>();
            /*
             * wv: 20160606 Controla la inserción y visualizacion de los Dispatch para los accounts
             * Valida que no exista previamente una dispatch para la cuenta activa
             */
            bool existListDispatch = false;
            var lstProductsVal = (List<NetSteps.Data.Entities.Business.HelperObjects.SearchData.DispatchProducts>)Session["itemsProductsDispatch"];
            if (!(lstProductsVal != null))
            {
                existListDispatch = true;
                itemsProductsDispatch = NetSteps.Data.Entities.Order.getDispatchProducts(orderCustomer.AccountID, orderCustomer.OrderID, orderCustomer.OrderCustomerTypeID, Convert.ToInt32(Session["WareHouseId"]), Convert.ToInt32(Session["PreOrder"]), 1, 22, false, true, existListDispatch);
                Session["itemsProductsDispatch"] = itemsProductsDispatch;
            }
            // ------------------------------------------------------------------------------------------------           


            if ((Convert.ToBoolean(Session["IsClains"]) == true))
            {
                int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;
                int accountType = CoreContext.CurrentAccount.AccountTypeID;
                var ItemClains = Order.GetClains(OrderContext.Order.AsOrder().OrderCustomers[0].AccountID, OrderContext.Order.OrderID, OrderContext.Order.OrderTypeID, Convert.ToInt32(Session["WareHouseId"]), Convert.ToInt32(Session["PreOrder"]), ppt, accountType, true, false);
                string orderItemHtml = "";
                foreach (var item in ItemClains.Where(x => x.ParentOrderItemID == null))
                {
                    orderItemHtml = orderItemHtml + "<tr style=\"background: #E6E6E6;\"><td></td><td>" + item.SKU + "</td><td>" + item.Name + "<label style=\"color: #FE6200;\">(Claim)</label>";
                    if (ItemClains.Where(x => x.ParentOrderItemID == item.OrderItemID).Count() > 0)
                    {
                        orderItemHtml = orderItemHtml + "<div>'<table><tbody><tr><th>" + "SKU" + "</th><th>" + "Product" + "</th><th>" + "Quantity" + "</th></tr>";
                        foreach (var itemChild in ItemClains.Where(x => x.ParentOrderItemID == item.OrderItemID))
                        {
                            orderItemHtml = orderItemHtml + "<tr><td>" + itemChild.SKU + "</td><td>" + itemChild.Name + "</td><td>" + itemChild.Quantity + "</td></tr>";
                        }
                        orderItemHtml = orderItemHtml + "</tbody></table></div>";
                    }
                    orderItemHtml = orderItemHtml + "</td><td>$0.00</td><td>$0.00</td><td>$0.00</td><td>@item.Quantity</td><td>$0.00</td> <td>$0.00</td> </tr>";
                    orderItems.Add(new
                    {
                        orderItemId = 0,
                        orderItem = orderItemHtml
                    });
                    orderItemHtml = "";
                }
            }

            foreach (OrderItem orderItem in nonPromotionalItems)
            {
                orderItems.Add(new
                {
                    orderItemId = orderItem.Guid.ToString("N"),
                    orderItem = GetOrderItemHtml(order, orderItem)
                });
            }

            
            if (adjustments.Count() > 0)
            {
                orderItems.Add(new
                {
                    orderItemId = Guid.NewGuid(),
                    orderItem = RenderPartialToString("~/Areas/Orders/Views/Shared/PartialOrderEntryPromotionItems.ascx", vdd, model: adjustments)
                });
            }

            if (lstProductsVal != null) // wv:20160606 Visualizacion de los dispatch para la cuenta Activa
            {
                calculateDispatch();
                orderItems.Add(new
                {
                    orderItemId = Guid.NewGuid(),
                    orderItem = RenderPartialToString("~/Areas/Orders/Views/Shared/PartialOrderEntryDispatchItems.ascx", vdd, model: itemsProductsDispatch)
                });
            }


            return orderItems;
        } 
         
		protected virtual object GetOrderItemHtml(Order order, OrderItem orderItem)
		{
			ViewDataDictionary vdd = new ViewDataDictionary();

			var autoshipSchedule = Session["AutoshipSchedule" + "_" + order.OrderID.ToString()] as AutoshipSchedule;
			bool fixedAutoship = autoshipSchedule != null && autoshipSchedule.AutoshipScheduleProducts.Count > 0;

			vdd.Add("FixedAutoship", fixedAutoship);
			vdd.Add("CurrencyID", order.CurrencyID);
            string result = "";

            //if ((Convert.ToBoolean(Session["IsClains"]) == true))
            //{
            //    int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;
            //    int accountType = CoreContext.CurrentAccount.AccountTypeID;
            //    foreach (var item in Order.GetClains(OrderContext.Order.AsOrder().OrderCustomers[0].AccountID, OrderContext.Order.OrderID, OrderContext.Order.OrderTypeID, Convert.ToInt32(Session["WareHouseId"]), Convert.ToInt32(Session["PreOrder"]), ppt, accountType))
            //    {
            //        result = result + "<tr><td></td><td>" + item.SKU + "</td><td>" + item.Name + "</td><td>$0.00</td><td>$0.00</td><td>$0.00</td><td>" + item.Quantity + "</td><td>$0.00</td><td>$0.00</td></tr>";
            //    }
            //}

			result  =result+ RenderPartialToString("~/Areas/Orders/Views/Shared/PartialOrderEntryLineItem.ascx", vdd, model: orderItem);

			return result;
		}

		protected virtual IDictionary<string, object> GetOrderEntryModelData(Order order)
		{
			Contract.Requires(order != null);

			return LoadOrderEntryModelData(new DynamicDictionary(), order).AsDictionary();
		}

		/// <summary>
		/// Loads the data bag for the client-side viewmodel.
		/// For consistency, this method should be used for
		/// both the initial page load and for AJAX calls.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="order"></param>
		protected virtual dynamic LoadOrderEntryModelData(dynamic data, Order order)
		{
			Contract.Requires<ArgumentNullException>(order != null);
			// Code contracts rewriter doesn't work with dynamics
			if (data == null)
			{
				throw new ArgumentNullException("options");
			}

			data.Subtotal = GetSubtotal(order);
			data.OrderItemModels = GetOrderItemModels(order);

			return data;
		}

		/// <summary>
		/// Returns subtotal, formatted for the client-side viewmodel.
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
		protected virtual string GetSubtotal(Order order)
		{
			Contract.Requires<ArgumentNullException>(order != null);

			return order.Subtotal.ToString(order.CurrencyID);
		}

		/// <summary>
		/// Returns order items, formatted for the client-side viewmodel.
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
		protected virtual IList<IOrderItemModel> GetOrderItemModels(Order order)
		{
			Contract.Requires<ArgumentNullException>(order != null);
			Contract.Requires<ArgumentException>(order.OrderCustomers != null);
			Contract.Requires<ArgumentException>(order.OrderCustomers.Count > 0);

			return order.OrderCustomers[0].ParentOrderItems
				.Select(orderItem =>
				{
					var orderItemModel = Create.New<IOrderItemModel>();
					var orderItemProduct = Inventory.GetProduct(orderItem.ProductID.Value);

					orderItemModel.Guid = orderItem.Guid.ToString("N");
					orderItemModel.ProductID = orderItem.ProductID ?? 0;
					orderItemModel.SKU = orderItemProduct.SKU;
					orderItemModel.ProductName = orderItemProduct.Translations.Name();

					orderItemModel.UnitPrice = orderItem.GetAdjustedPrice().ToString(order.CurrencyID);
					orderItemModel.Quantity = orderItem.Quantity;
					orderItemModel.CommissionableTotal = (orderItem.CommissionableTotalOverride ?? orderItem.CommissionableTotal ?? 0).ToString(order.CurrencyID);
					orderItemModel.Total = (orderItem.GetAdjustedPrice() * orderItem.Quantity).ToString(order.CurrencyID);

					// Hostess rewards show the discount amount next to the total.
					if (orderItem.IsHostReward)
					{
						// when hostess rewards are an order adjustment type we can refactor this.
						orderItemModel.Total = string.Format("{0} {1}",
							orderItemModel.Total,
							Translation.GetTerm("HostRewardDiscount", "(discounted {0})",
								((orderItem.ItemPrice - orderItem.GetAdjustedPrice()) * orderItem.Quantity).ToString(order.CurrencyID)
							)
						);
					}
					orderItemModel.IsStaticKit = orderItemProduct.IsStaticKit();
					orderItemModel.IsDynamicKit = orderItemProduct.IsDynamicKit();
					if (orderItemModel.IsDynamicKit)
					{
						orderItemModel.IsDynamicKitFull = orderItem.ChildOrderItems.Sum(oi => oi.Quantity) >= orderItemProduct.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount);
					}
					orderItemModel.IsHostReward = orderItem.IsHostReward;
					orderItemModel.BundlePackItemsUrl = Url.Action("BundlePackItems", new { productId = orderItem.ProductID, bundleGuid = orderItem.Guid.ToString("N"), orderCustomerId = orderItem.OrderCustomer.Guid.ToString("N") });

					orderItemModel.KitItemsModel = Create.New<IKitItemsModel>();
					if (orderItemProduct.IsStaticKit() || orderItemProduct.IsDynamicKit())
					{
						orderItemModel.KitItemsModel.KitItemModels = orderItem.ChildOrderItems
							.Select(k =>
							{
								var kitItemModel = Create.New<IKitItemModel>();
								var kitItemProduct = Inventory.GetProduct(k.ProductID.Value);
								kitItemModel.ProductName = kitItemProduct.Translations.Name();
								kitItemModel.Quantity = k.Quantity;
								kitItemModel.SKU = kitItemProduct.SKU;
								return kitItemModel;
							})
							.ToList();
					}
					else
					{
						// Non-kits still need an empty list.
						orderItemModel.KitItemsModel.KitItemModels = new List<IKitItemModel>();
					}

					return orderItemModel;
				})
				.ToList();
		}

		protected virtual List<PaymentType> GetAdditionalPaymentTypes()
		{
			return CoreContext.CurrentAccount.GetNonProfilePaymentTypes(CoreContext.CurrentUser, OrderContext.Order.OrderTypeID);
		}

		protected static void AppendPaymentMethodDiv(StringBuilder paymentMethods, int methodID, string profileName, string uiTemplate)
		{
			paymentMethods.AppendFormat("<div id=\"paymentMethod{0}\" class=\"paymentMethodDisplay\">", methodID);
			paymentMethods.AppendFormat("<b>{0}</b> - ", profileName);
			paymentMethods.AppendFormat("<a title=\"Edit\" style=\"cursor: pointer;\" onclick=\"editPaymentMethod({0});\">Edit</a>", methodID);
			paymentMethods.AppendFormat("<br/>{0}</div>", uiTemplate);
		}

		protected static void AppendPaymentMethodOption(StringBuilder options, string displayName, int paymentMethodID, bool isDefault)
		{
			options.AppendFormat("<option value=\"{0}\">{1}{2}</option>", paymentMethodID, displayName, isDefault ? String.Format(" ({0})", Translation.GetTerm("default", "default")) : String.Empty);
		}

		[NonAction]
		public virtual HtmlString GetGroupItemsHtml(string parentGuid, int groupId)
		{
			if (OrderContext.Order.AsOrder() == null)
				return new HtmlString("");

			var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
			StringBuilder builder = new StringBuilder();

			var orderItem = customer.OrderItems.GetByGuid(parentGuid);
			var product = Inventory.GetProduct(orderItem.ProductID.ToInt());
			var dynamicKit = product.DynamicKits[0];
			var groupItems = orderItem.ChildOrderItems.Where(index => index.DynamicKitGroupID == groupId);

			foreach (var item in groupItems)
			{
				Product childProduct = Inventory.GetProduct(item.ProductID.Value);
				for (int q = 0; q < item.Quantity; q++)
				{
					TagBuilder span = new TagBuilder("span");
					span.AddCssClass("block");

					span.InnerHtml = new StringBuilder()
							.Append("<input type=\"hidden\" value=\"" + item.Guid.ToString("N") + "\" class=\"orderItemGuid\" />")
							.Append("<a href=\"javascript:void(0)\" class=\"UI-icon icon-x RemoveItem\"></a>&nbsp;" + childProduct.SKU + " " + childProduct.Translations.Name()).ToString();

					builder.Append(span.ToString());
				}
			}

			var results = builder.ToString();
			return new HtmlString(string.IsNullOrEmpty(results) ? "" : results);
		}

		public bool StepHasAnItemInStockToBeChosen(string stepId)
		{
			try
			{
				var allSteps = OrderContext.InjectedOrderSteps.Union(OrderContext.Order.OrderAdjustments.SelectMany(oa => oa.InjectedOrderSteps));
				var step = (IUserProductSelectionOrderStep)allSteps.Single(os => os is IUserProductSelectionOrderStep && os.OrderStepReferenceID.ToString() == stepId);
				var inventoryService = Create.New<IInventoryService>();
				var options = step.AvailableOptions.Select(o =>
				{
					var product = Inventory.GetProduct(o.ProductID);
					return inventoryService.GetProductAvailabilityForOrder(OrderContext, o.ProductID, 1);
				});
				foreach (var option in options)
				{
					if (option.CanAddBackorder > 0 || option.CanAddNormally > 0)
					{
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return false;
			}
		}

		#endregion

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult GetPaymentMethods()
		{
			try
			{
				StringBuilder paymentMethods = new StringBuilder();
				StringBuilder options = new StringBuilder();

				foreach (AccountPaymentMethod paymentMethod in CoreContext.CurrentAccount.AccountPaymentMethods.OrderByDescending(pm => pm.IsDefault).ThenBy(pm => pm.AccountPaymentMethodID))
				{
					AppendPaymentMethodDiv(paymentMethods, paymentMethod.AccountPaymentMethodID, paymentMethod.ProfileName, paymentMethod.ToDisplay(CoreContext.CurrentCultureInfo));
					AppendPaymentMethodOption(options, String.IsNullOrWhiteSpace(paymentMethod.ProfileName) ? paymentMethod.MaskedAccountNumber : paymentMethod.ProfileName, paymentMethod.AccountPaymentMethodID, paymentMethod.IsDefault);
				}
				if (!OrderContext.Order.AsOrder().IsTemplate)
				{
					var additionalPaymentTypes = GetAdditionalPaymentTypes();

					foreach (var paymentMethod in additionalPaymentTypes)
					{
						AppendPaymentMethodDiv(paymentMethods, paymentMethod.PaymentTypeID, paymentMethod.GetTerm(), string.Empty);
						AppendPaymentMethodOption(options, paymentMethod.GetTerm(), paymentMethod.PaymentTypeID, false);
					}
				}
				return Json(new { options = options.ToString(), paymentMethods = paymentMethods.ToString() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Orders-Order Entry", "~/Orders")]
		public virtual ActionResult SetShippingMethod(int shippingMethodId)
		{
			try
			{ 
                Session["DateEstimated"] = "";
                OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipment();
                OrderCustomer customer = OrderContext.Order.AsOrder().OrderCustomers[0]; 
                var shippingMethods = ShippingCalculator.GetShippingMethodsWithRates(customer, shipment).OrderBy(x => x.ShippingAmount);
                foreach (var sp in shippingMethods.Where(x => x.ShippingMethodID == shippingMethodId))
                {
                    Session["DateEstimated"] = (sp.DateEstimated);
                }
				OrderContext.Order.AsOrder().SetShippingMethod(shippingMethodId); 

				OrderService.UpdateOrder(OrderContext);
                Session["GetAddress"] = true;
                return Json(new { result = true, totals = Totals, dateEstimated = Session["DateEstimated"] });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpPost]
		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
		public virtual ActionResult SavePaymentMethodEFT(int? paymentMethodId, string nameOnAccount, string bankName, string routingInput, string accountInput,
			short bankAccountTypeID, string profileName, string attention, string address1, string address2, string address3, string zip,
            string city, string county, string state, string street, int countryId, string phone, int? addressId, bool? savePaymentInfo = true)
		{
			try
			{
				if (savePaymentInfo.Value)
				{
                    //Account account = CoreContext.CurrentAccount;
                    Account account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
                    //var originalAccount = CoreContext.CurrentAccount.Clone();
                    var originalAccount = account.Clone();

					account.StartEntityTracking();
					AccountPaymentMethod paymentMethod = null;
					if (paymentMethodId.HasValue && paymentMethodId.Value > 0)
					{
						paymentMethod = account.AccountPaymentMethods.First(a => a.AccountPaymentMethodID == paymentMethodId.Value);
					}
					else
					{
						paymentMethod = new AccountPaymentMethod();
						account.AccountPaymentMethods.Add(paymentMethod);
					}
					paymentMethod.BankName = bankName;
					paymentMethod.BankAccountTypeID = bankAccountTypeID;
					paymentMethod.ProfileName = profileName;
					paymentMethod.StartEntityTracking();
					paymentMethod.PaymentTypeID = (int)NetSteps.Data.Entities.Constants.PaymentType.EFT;
					if (!accountInput.Contains("*"))
						paymentMethod.DecryptedAccountNumber = accountInput.RemoveNonNumericCharacters();
					paymentMethod.NameOnCard = nameOnAccount.ToCleanString();
					paymentMethod.RoutingNumber = routingInput.ToCleanString();

					Address billingAddress = null;
					if (addressId.HasValue && addressId.Value > 0)
					{
						billingAddress = account.Addresses.GetByAddressID(addressId.Value);
					}
					else
					{
						billingAddress = new Address();
						account.Addresses.Add(billingAddress);

						if (paymentMethod.BillingAddress != null && paymentMethod.BillingAddress.AddressID == 0)
							paymentMethod.BillingAddress = null;
					}

					billingAddress.AttachAddressChangedCheck();
					billingAddress.ProfileName = profileName.ToCleanString();
					billingAddress.Attention = attention.ToCleanString();
					billingAddress.Address1 = address1.ToCleanString();
					billingAddress.Address2 = address2.ToCleanString();
					billingAddress.Address3 = address3.ToCleanString();
					billingAddress.City = city.ToCleanString();
                    billingAddress.County = county.ToCleanString();
                    billingAddress.SetState(state.ToCleanString(), countryId);
                    //if (state.IsValidInt())
                    //{
                    //    int spid = state.ToInt();
                    //    billingAddress.StateProvinceID = (spid > 0) ? spid : (int?)null;
                    //    billingAddress.State = (spid > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(spid).StateAbbreviation : state.ToCleanString();
                    //}
                    //else if (!String.IsNullOrWhiteSpace(state))
                    //{
                    //    string st = state.ToCleanString();
                    //    var stprov = SmallCollectionCache.Instance.StateProvinces.Where(sp => sp.StateAbbreviation.Equals(st, StringComparison.InvariantCultureIgnoreCase) && sp.CountryID == countryId).FirstOrDefault();
                    //    if (stprov != null)
                    //    {
                    //        billingAddress.StateProvinceID = stprov.StateProvinceID;
                    //        billingAddress.State = stprov.StateAbbreviation;
                    //    }
                    //}
					billingAddress.PostalCode = zip.ToCleanString();
					billingAddress.CountryID = countryId;
					billingAddress.PhoneNumber = phone.ToCleanString();
					billingAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort();
					billingAddress.LookUpAndSetGeoCode();
					billingAddress.Validate();
					if (!billingAddress.IsValid)
					{
						CoreContext.CurrentAccount = originalAccount;
						return Json(new
						{
							result = false,
							message = billingAddress.GetValidationErrorMessage()
						});
					}
					var result = billingAddress.ValidateAddressAccuracy();
					if (!result.Success)
						return Json(new { result = false, message = result.Message });

					account.Save();
					paymentMethod.BillingAddress = billingAddress;
					paymentMethod.BillingAddressID = billingAddress.AddressID;
					account.Save();

                    UpdateAddressStreet(account, street, billingAddress.AddressID); 

					CoreContext.CurrentAccount = account;

					return Json(new { result = true });
				}
				else
				{
					// TODO: Don't save AccountPaymentMethod; just add to order and update UI drop down list to include a 'One time only' payment - JHE
					// TODO: Finish this - JHE

					return Json(new { result = false, message = "This functionality has not yet been implemented." });
				}
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpPost]
		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
		public virtual ActionResult SavePaymentMethod(int? paymentMethodId, /*string accountName, */string nameOnCard, string accountNumber,
				DateTime expDate, string profileName, string attention, string address1, string address2, string address3, string zip,
                string city, string county, string state, string street, int countryId, string phone, int? addressId, bool? savePaymentInfo = true)
		{
			try
			{
				if (savePaymentInfo.Value)
				{
                    //Account account = CoreContext.CurrentAccount;
                    //var originalAccount = CoreContext.CurrentAccount.Clone();
                    Account account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
                    var originalAccount = account.Clone();

					account.StartEntityTracking();

					AccountPaymentMethod paymentMethod = null;
					if (paymentMethodId.HasValue && paymentMethodId.Value > 0)
					{
						paymentMethod = account.AccountPaymentMethods.First(a => a.AccountPaymentMethodID == paymentMethodId.Value);
					}
					else
					{
						paymentMethod = new AccountPaymentMethod();
						account.AccountPaymentMethods.Add(paymentMethod);
					}

					paymentMethod.StartEntityTracking();
					paymentMethod.ProfileName = profileName.ToCleanString();
					paymentMethod.PaymentTypeID = (int)NetSteps.Data.Entities.Constants.PaymentType.CreditCard;
					if (!accountNumber.Contains("*"))
						paymentMethod.DecryptedAccountNumber = accountNumber.RemoveNonNumericCharacters();
					paymentMethod.NameOnCard = nameOnCard.ToCleanString();
					paymentMethod.ExpirationDateUTC = expDate.LastDayOfMonth();

					Address billingAddress = null;
					if (addressId.HasValue && addressId.Value > 0)
					{
						billingAddress = account.Addresses.GetByAddressID(addressId.Value);
					}
					else
					{
						billingAddress = new Address();
						account.Addresses.Add(billingAddress);

						if (paymentMethod.BillingAddress != null && paymentMethod.BillingAddress.AddressID == 0)
							paymentMethod.BillingAddress = null;
					}

					billingAddress.AttachAddressChangedCheck();
					billingAddress.ProfileName = profileName.ToCleanString();
					billingAddress.Attention = attention.ToCleanString();
					billingAddress.Address1 = address1.ToCleanString();
					billingAddress.Address2 = address2.ToCleanString();
					billingAddress.Address3 = address3.ToCleanString();
					billingAddress.City = city.ToCleanString();
                    billingAddress.SetState(state, countryId);
                    //billingAddress.StateProvinceID = (state.IsValidInt() && state.ToInt() > 0) ? state.ToInt() : (int?)null;
                    //billingAddress.State = (state.IsValidInt() && state.ToInt() > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(state.ToInt()).StateAbbreviation : state.ToCleanString();
					billingAddress.PostalCode = zip.ToCleanString();
					billingAddress.CountryID = countryId;
					billingAddress.PhoneNumber = phone.ToCleanString();
					billingAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort();
					billingAddress.LookUpAndSetGeoCode();

					billingAddress.Validate();
					if (!billingAddress.IsValid)
					{
						CoreContext.CurrentAccount = originalAccount;
						return Json(new
						{
							result = false,
							message = billingAddress.GetValidationErrorMessage()
						});
					}

					var result = billingAddress.ValidateAddressAccuracy();
					if (!result.Success)
						return Json(new { result = false, message = result.Message });

					account.Save();
					paymentMethod.BillingAddressID = billingAddress.AddressID;
					account.Save();

                    UpdateAddressStreet(account, street, billingAddress.AddressID);

					CoreContext.CurrentAccount = account;

					return Json(new { result = true });
				}
				else
				{
					// TODO: Don't save AccountPaymentMethod; just add to order and update UI drop down list to include a 'One time only' payment - JHE

					// TODO: Finish this - JHE

					return Json(new { result = false, message = "This functionality has not yet been implemented." });
				}
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
	}
}
