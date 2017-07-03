using System.Collections.Generic;
using System.Linq;
using NetSteps.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class OrderShipmentBusinessLogic
	{
		public override void DefaultValues(IOrderShipmentRepository repository, OrderShipment entity)
		{
			entity.OrderShipmentStatusID = Constants.OrderShipmentStatus.Pending.ToShort();
		}


		public virtual IEnumerable<Order> GetOrderShippingDetails(IEnumerable<string> orderNumbers)
		{
			return Order.LoadOrderWithShipmentAndPaymentDetails(orderNumbers);
		}


		public override void AddValidationRules(OrderShipment entity)
		{
			bool allowPOBoxShipment = ConfigurationManager.GetAppSetting(ConfigurationManager.VariableKey.AllowPOBoxShipment, false);

			if (!allowPOBoxShipment)
			{
				entity.ValidationRules.AddRule(CommonRules.RegexIsNotMatch,
					new CommonRules.RegexRuleArgs("Address1", RegularExpressions.PoBox,
						Translation.GetTerm("InvalidShippingAddressErrorMessage", CustomValidationMessages.ShippingPOBox), true));
			}
			else
			{
				base.AddValidationRules(entity);
			}

			entity.ValidationRules.AddRule(Address.IsCountryIdOnAddressValid, new ValidationRuleArgs("OrderShipmentID", "Invalid CountryID"));
		}

		public virtual IPaginatedList<OrderShippingSearchData> Search(IOrderShipmentRepository repository, OrderShipmentSearchParameters searchParameters)
		{
			return repository.Search(searchParameters);
		}

		public virtual string GetBaseTrackUrl(int shippingMethodID, string trackingNumber)
		{
			var trackingUrl = SmallCollectionCache.Instance.ShippingMethods.GetById(shippingMethodID).TrackingNumberBaseUrl;
			return trackingUrl;
		}

		public IList<ShippingMethodWithRate> FilterOutExcludedShippingMethods(
			Order order,
			IEnumerable<ShippingMethodWithRate> shippingMethods,
			IProductRepository productRepository,
			out IList<int> productIdsWithExclusions)
		{
			var orderProductIds = order.OrderShipments
				.SelectMany(os => os.GetOrderItems())
				.Where(oi => oi.ProductID.HasValue)
				.Select(oi => oi.ProductID.Value)
				.Distinct()
				.ToList();

			var excludedShippingMethodIds = productRepository.GetExcludedShippingMethodIds(orderProductIds, out productIdsWithExclusions);

			return shippingMethods
				.Where(sm => !excludedShippingMethodIds.Contains(sm.ShippingMethodID))
				.ToList();
		}

		/// <summary>
		/// Analyzes the OrderItems and OrderShipmentPackageItems to determine if the OrderShipment is not shipped, partially shipped, or fully shipped.
		/// </summary>
		public virtual short CalculateOrderShipmentStatus(OrderShipment orderShipment)
		{
			// The original items in the shipment.
			var orderItems = orderShipment.GetOrderItems()
				.Select(x => new
				{
					x.OrderItemID,
					x.Quantity
				});

			// The items which have shipped.
			var shippedItems = orderShipment.OrderShipmentPackages
				.SelectMany(x => x.OrderShipmentPackageItems
					.Select(ospi => new
					{
						ospi.OrderItemID,
						ospi.Quantity
					})
				)
				// Sum totals from multiple shipments.
				.GroupBy(x => x.OrderItemID, (OrderItemID, x) => new
				{
					OrderItemID,
					Quantity = x.Sum(y => y.Quantity)
				});

			// Calculate shipped status of each order item.
			var orderItemShipmentStatuses = from oi in orderItems
											// Outer join to include orderItems with or without corresponding shippedItems.
											join _si in shippedItems on oi.OrderItemID equals _si.OrderItemID into _si
											from si in _si.DefaultIfEmpty()
											let orderedQuantity = oi.Quantity
											let shippedQuantity = si != null ? si.Quantity : 0
											select orderedQuantity - shippedQuantity <= 0
												// This will include orderItems with quantity of zero.
												? Constants.OrderShipmentStatus.Shipped
												: shippedQuantity > 0
												// If at least one item has shipped, then it's partially shipped.    
													? Constants.OrderShipmentStatus.PartiallyShipped
												// Else pending.
													: Constants.OrderShipmentStatus.Pending;

			// Calculate shipped status of the entire shipment based on the status of each order item.
			if (orderItemShipmentStatuses.All(x => x == Constants.OrderShipmentStatus.Shipped))
			{
				// All items are shipped.
				return (short)Constants.OrderShipmentStatus.Shipped;
			}
			if (orderItemShipmentStatuses.Any(x =>
				x == Constants.OrderShipmentStatus.Shipped
				|| x == Constants.OrderShipmentStatus.PartiallyShipped))
			{
				// Some items are shipped.
				return (short)Constants.OrderShipmentStatus.PartiallyShipped;
			}
			// No items are shipped.
			return (short)Constants.OrderShipmentStatus.Pending;

		}
	}
}