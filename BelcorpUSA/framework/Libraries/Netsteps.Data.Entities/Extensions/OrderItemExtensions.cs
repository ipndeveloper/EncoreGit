using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Extensions
{
	public static class OrderItemExtensions
	{
		public static OrderItem GetOrderItem(this IEnumerable<OrderItem> orderItems, int orderItemID)
		{
			OrderItem returnItem = orderItems.FirstOrDefault(orderItem => orderItem.OrderItemID == orderItemID);

			return returnItem;
		}

		public static OrderItem GetOrderItemBySku(this IEnumerable<OrderItem> orderItems, string sku)
		{
			try
			{
				var inventory = Create.New<InventoryBaseRepository>();
				OrderItem returnItem = orderItems.FirstOrDefault(orderItem => inventory.GetProduct(orderItem.ProductID.Value).SKU == sku);

				return returnItem;
			}

			catch
			{
				throw new NetStepsBusinessException(string.Format("Could not find a product with SKU '{0}'", sku))
				{
					PublicMessage = Translation.GetTerm("Could", "Could not find a product with SKU '{0}'", sku)
				};
			}
		}

		public static OrderItem GetOrderItemByProductID(this IEnumerable<OrderItem> orderItems, int productID)
		{
			OrderItem returnItem = orderItems.FirstOrDefault(orderItem => orderItem.ProductID == productID);

			return returnItem;
		}

		public static bool IsRestockable(this OrderItem orderItem)
		{
			var inventory = Create.New<InventoryBaseRepository>();
			bool isShippable = false;
			if (orderItem.ProductID.HasValue)
			{
				var product = inventory.GetProduct(orderItem.ProductID.Value);
				isShippable = product.ProductBase.IsShippable;
			}

			return isShippable;
		}

		public static void CopyTaxValuesFromOrderItem(this OrderItem orderItem)
		{
			try
			{
				orderItem.Taxes.TaxableTotal = orderItem.TaxableTotal.ToDecimal();
				orderItem.Taxes.TaxAmountTotal = orderItem.TaxAmount.ToDecimal();
				orderItem.Taxes.TaxAmountCity = orderItem.TaxAmountCity.ToDecimal();
				orderItem.Taxes.TaxAmountCityLocal = orderItem.TaxAmountCityLocal.ToDecimal();
				orderItem.Taxes.TaxAmountCounty = orderItem.TaxAmountCounty.ToDecimal();
				orderItem.Taxes.TaxAmountCountyLocal = orderItem.TaxAmountCountyLocal.ToDecimal();
				orderItem.Taxes.TaxAmountState = orderItem.TaxAmountState.ToDecimal();
				orderItem.Taxes.TaxAmountDistrict = orderItem.TaxAmountDistrict.ToDecimal();
				orderItem.Taxes.TaxPercent = orderItem.TaxPercent.ToDecimal();
				orderItem.Taxes.TaxPercentCity = orderItem.TaxPercentCity.ToDecimal();
				orderItem.Taxes.TaxPercentCounty = orderItem.TaxPercentCounty.ToDecimal();
				orderItem.Taxes.TaxPercentDistrict = orderItem.TaxPercentDistrict.ToDecimal();
				orderItem.Taxes.TaxPercentState = orderItem.TaxPercentState.ToDecimal();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void AddOrderItemProperty(this OrderItem orderItem, string propertyName, object propertyValue)
		{
			try
			{
				OrderItemPropertyType orderItemPropertyType = OrderItemPropertyType.GetByName(propertyName);
				OrderItemProperty orderItemProperty = orderItem.OrderItemProperties.FirstOrDefault(oip => oip.OrderItemPropertyTypeID == orderItemPropertyType.OrderItemPropertyTypeID);

				if (orderItemProperty == null)
				{
					orderItemProperty = new OrderItemProperty();
					orderItemProperty.StartEntityTracking();
					orderItemProperty.OrderItemPropertyTypeID = orderItemPropertyType.OrderItemPropertyTypeID;
					orderItemProperty.Active = true;
					orderItemProperty.OrderItemID = orderItem.OrderItemID;

					orderItem.OrderItemProperties.Add(orderItemProperty);
				}
				else
				{
					orderItemProperty.StartEntityTracking();
				}

				orderItemProperty.PropertyValue = propertyValue.ToString();
				orderItemProperty.Save();
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
			}
		}

		public static string GetOrderItemProperty(this OrderItem orderItem, string key)
		{
			string propValue = string.Empty;
			OrderItemPropertyType orderItemPropertyType = OrderItemPropertyType.GetByName(key);
			if (orderItemPropertyType != null)
			{
				OrderItemProperty prop = orderItem.OrderItemProperties.FirstOrDefault(oip => oip.OrderItemPropertyTypeID == orderItemPropertyType.OrderItemPropertyTypeID);
				if (prop != null)
					propValue = prop.Value;
			}

			return propValue;
		}

		public static void LoadOrderItemProperties(this OrderItem orderItem)
		{
			var repo = new OrderItemRepository();
			orderItem.StopTracking();
			orderItem.OrderItemProperties = repo.GetOrderItemProperties(orderItem.OrderItemID);
			orderItem.StartTracking();
		}

	}
}
