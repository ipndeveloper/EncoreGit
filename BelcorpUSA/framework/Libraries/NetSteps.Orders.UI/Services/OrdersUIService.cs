using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Orders.UI.Common.Models;
using NetSteps.Orders.UI.Common.Services;

namespace NetSteps.Orders.UI.Services
{
	[ContainerRegister(typeof(IOrdersUIService), RegistrationBehaviors.Default)]
	public class OrdersUIService : IOrdersUIService
	{
		public IOrderDetailListModel GetOrderDetailModelsByOrderID(int orderID)
		{
			IList<IOrderDetailModel> models = new List<IOrderDetailModel>();
			IList<Order> ordersOnParty = Order.LoadChildOrdersFull(orderID, (int)Constants.OrderType.OnlineOrder, (int)Constants.OrderType.PortalOrder);

			foreach (Order order in ordersOnParty)
			{
				IOrderDetailModel odm = GenerateOrderDetailModel(order);
				odm.OrderSubTotalSavings = odm.OrderDetailItems.Sum(odi => (odi.Price - (odi.AdjustedPrice / odi.Quantity)) * odi.Quantity);
				odm.OrderSubTotalSavingsText = Translation.GetTerm("OrderSubTotalSavings", "Preferred Customer");
				odm.OrderTotalSavings = -9.99m;
				odm.OrderTotalSavingsText = Translation.GetTerm("OrderTotalSavings", "Saved on this order");
				models.Add(odm);
			}

			IOrderDetailListModel result = Create.New<IOrderDetailListModel>();
			result.Init(models);
			return result;
		}

		private IOrderDetailModel GenerateOrderDetailModel(Order order)
		{
			InventoryBaseRepository inventory = NetSteps.Encore.Core.IoC.Create.New<InventoryBaseRepository>();
			OrderCustomer customer = order.OrderCustomers[0];
			OrderStatus orderStatus = SmallCollectionCache.Instance.OrderStatuses.GetById(order.OrderStatusID);
			List<IOrderDetailItemModel> rows = new List<IOrderDetailItemModel>();
			customer.ParentOrderItems.OrderByDescending(oi => oi.OrderItemTypeID).ToList().ForEach(oi => rows.Add(GenerateOrderDetailItemModel(oi, inventory, order.CurrencyID)));

			CultureInfo culture = order.CurrencyID == 0 ? CultureInfo.CurrentCulture : SmallCollectionCache.Instance.Currencies.GetById(order.CurrencyID) != null ? SmallCollectionCache.Instance.Currencies.GetById(order.CurrencyID).Culture : Currency.Load(order.CurrencyID).Culture;

			IOrderDetailModel model = Create.New<IOrderDetailModel>();

			model.OrderCurrencySymbol = culture.NumberFormat.CurrencySymbol;

			model.CustomersFullName = customer.FullName;
			model.OnlineOrderText = Translation.GetTerm("OutsideOrderIndicator", "(Outside Order)");
			model.OnlineOrderStatusText = orderStatus.GetTerm();
			model.CustomersGuid = customer.Guid;
			model.IsHostess = customer.IsHostess;

			model.SKUHeaderText = Translation.GetTerm("SKU", "SKU");
			model.ProductHeaderText = Translation.GetTerm("Product", "Product");
			model.PriceHeaderText = Translation.GetTerm("Price", "Price");
			model.QuantityHeaderText = Translation.GetTerm("Quantity", "Quantity");
			model.CVHeaderText = Translation.GetTerm("CV", "CV");
			model.TotalHeaderText = Translation.GetTerm("Total", "Total");

			model.OrderDetailItems.AddRange(rows);

			model.OrderCVTotalText = Translation.GetTerm("CV", "CV");
			model.OrderSubtotalText = Translation.GetTerm("Subtotal", "Subtotal");
			model.OrderTaxTotalText = Translation.GetTerm("Tax", "Tax");
			model.OrderShippingAndHandlingTotalText = Translation.GetTerm("ShippingAndHandling", "Shipping &amp; Handling");
			model.OrderTotalText = Translation.GetTerm("Total", "Total");

			model.OrderCVTotal = order.CommissionableTotal.HasValue ? order.CommissionableTotal.Value : 0;
			model.OrderSubtotal = order.Subtotal.HasValue ? order.Subtotal.Value : 0;
			model.OrderTaxTotal = order.TaxAmountTotal.HasValue ? order.TaxAmountTotal.Value : 0;
			if (order.ShippingTotal.HasValue && order.HandlingTotal.HasValue)
				model.OrderShippingAndHandlingTotal = (order.ShippingTotal + order.HandlingTotal).Value;
			model.OrderTotal = order.GrandTotal.HasValue ? order.GrandTotal.Value : 0;

			model.BookAPartyText = Translation.GetTerm("PartyCart_BookAParty", "Book a Party");

			return model;
		}

		private IOrderDetailItemModel GenerateOrderDetailItemModel(OrderItem orderItem, InventoryBaseRepository inventory, int currencyID)
		{
			var product = inventory.GetProduct(orderItem.ProductID.ToInt());
			int requiredItemsInBundleCount = product.DynamicKits != null && product.DynamicKits.Any() ?
				product.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount) :
				0;
			List<IOrderDetailItemModel> childRows = new List<IOrderDetailItemModel>();
			orderItem.ChildOrderItems.ToList().ForEach(oi => childRows.Add(GenerateOrderDetailItemModel(oi, inventory, currencyID)));

			IOrderDetailItemModel model = Create.New<IOrderDetailItemModel>();

			model.IsDynamicKit = product.IsDynamicKit();
			model.Guid = orderItem.Guid;
			model.IsExclusiveProduct = orderItem.OrderItemTypeID == (int)Constants.OrderItemType.ExclusiveProduct;
			model.SKU = orderItem.SKU;
			model.IsBundleFull = orderItem.ChildOrderItems.Sum(oi => oi.Quantity) == requiredItemsInBundleCount;
			model.ChildItems.AddRange(childRows);
			model.Price = orderItem.GetPreAdjustmentPrice();
			model.AdjustedPrice = orderItem.GetAdjustedPrice();
			model.Quantity = orderItem.Quantity;
			model.CV = orderItem.CommissionableTotal.HasValue ? orderItem.CommissionableTotal.Value : 0;
			model.Total = orderItem.GetAdjustedPrice() * orderItem.Quantity;
			model.ExclusiveProductsIndicatorText = Translation.GetTerm("ExclusiveProductsIndicator", "*");
			model.ProductName = product.Translations.Name();
			model.ProductImageUrl = product.MainImage != null ? product.MainImage.FilePath : string.Empty;

			return model;
		}
	}
}
