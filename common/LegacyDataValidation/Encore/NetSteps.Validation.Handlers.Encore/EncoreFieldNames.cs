using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Handlers
{
    public static class EncoreFieldNames
    {
        public static class TableSingularNames
        {
            public const string Order = "Order";
            public const string OrderCustomer = "OrderCustomer";
            public const string OrderItem = "OrderItem";
            public const string OrderItemPrice = "OrderItemPrice";
            public const string OrderAdjustmentOrderLineModification = "OrderAdjustmentOrderLineModification";
            public const string OrderAdjustmentOrderModification = "OrderAdjustmentOrderModification";
        }

        public static class Order
        {
            public const string OrderID = "OrderID";
            public const string OrderNumber = "OrderNumber";
            public const string OrderStatusID = "OrderStatusID";
            public const string OrderTypeID = "OrderTypeID";
            public const string AccountID = "AccountID";
            public const string SiteID = "SiteID";
            public const string ParentOrderID = "ParentOrderID";
            public const string CurrencyID = "CurrencyID";
            public const string CompleteDateUTC = "CompleteDateUTC";
            public const string CommissionDateUTC = "CommissionDateUTC";
            public const string HostessRewardsEarned = "HostessRewardsEarned";
            public const string HostessRewardsUsed = "HostessRewardsUsed";
            public const string IsTaxExempt = "IsTaxExempt";
            public const string TaxAmountTotal = "TaxAmountTotal";
            public const string TaxAmountTotalOverride = "TaxAmountTotalOverride";
            public const string TaxableTotal = "TaxableTotal";
            public const string TaxAmountOrderItems = "TaxAmountOrderItems";
            public const string TaxAmountShipping = "TaxAmountShipping";
            public const string TaxAmount = "TaxAmount";
            public const string Subtotal = "Subtotal";
            public const string DiscountTotal = "DiscountTotal";
            public const string ShippingTotal = "ShippingTotal";
            public const string ShippingTotalOverride = "ShippingTotalOverride";
            public const string HandlingTotal = "HandlingTotal";
            public const string GrandTotal = "GrandTotal";
            public const string PaymentTotal = "PaymentTotal";
            public const string Balance = "Balance";
            public const string CommissionableTotal = "CommissionableTotal";
            public const string ReturnTypeID = "ReturnTypeID";
            public const string StepUrl = "StepUrl";
            public const string ModifiedByUserID = "ModifiedByUserID";
            public const string DateCreatedUTC = "DateCreatedUTC";
            public const string CreatedByUserID = "CreatedByUserID";
            public const string DataVersion = "DataVersion";
            public const string DiscountPercent = "DiscountPercent";
            public const string PartyShipmentTotal = "PartyShipmentTotal";
            public const string PartyHandlingTotal = "PartyHandlingTotal";
        }

        public static class OrderCustomer
        {
            public const string OrderCustomerID = "OrderCustomerID";
            public const string OrderCustomerTypeID = "OrderCustomerTypeID";
            public const string OrderID = "OrderID";
            public const string AccountID = "AccountID";
            public const string ShippingTotal = "ShippingTotal";
            public const string HandlingTotal = "HandlingTotal";
            public const string DiscountAmount = "DiscountAmount";
            public const string Subtotal = "Subtotal";
            public const string PaymentTotal = "PaymentTotal";
            public const string CommissionableTotal = "CommissionableTotal";
            public const string Balance = "Balance";
            public const string Total = "Total";
            public const string FutureBookingDateUTC = "FutureBookingDateUTC";
            public const string IsTaxExempt = "IsTaxExempt";
            public const string TaxAmountTotal = "TaxAmountTotal";
            public const string TaxAmountCity = "TaxAmountCity";
            public const string TaxAmountState = "TaxAmountState";
            public const string TaxAmountCounty = "TaxAmountCounty";
            public const string TaxAmountDistrict = "TaxAmountDistrict";
            public const string TaxAmountOrderItems = "TaxAmountOrderItems";
            public const string TaxAmountShipping = "TaxAmountShipping";
            public const string TaxableTotal = "TaxableTotal";
            public const string TaxAmount = "TaxAmount";
            public const string DataVersion = "DataVersion";
            public const string ModifiedByUserID = "ModifiedByUserID";
            public const string TaxAmountCountry = "TaxAmountCountry";
            public const string IsBookingCredit = "IsBookingCredit";
            public const string TaxGeocode = "TaxGeocode";
            public const string SalesTaxTransactionNumber = "SalesTaxTransactionNumber";
            public const string UseTaxTransactionNumber = "UseTaxTransactionNumber";

        }

        public static class OrderItem
        {
            public const string OrderItemID = "OrderItemID";
            public const string OrderCustomerID = "OrderCustomerID";
            public const string OrderItemTypeID = "OrderItemTypeID";
            public const string HostessRewardRuleID = "HostessRewardRuleID";
            public const string ParentOrderItemID = "ParentOrderItemID";
            public const string ProductID = "ProductID";
            public const string ProductPriceTypeID = "ProductPriceTypeID";
            public const string ProductName = "ProductName";
            public const string SKU = "SKU";
            public const string CatalogID = "CatalogID";
            public const string Quantity = "Quantity";
            public const string ItemPrice = "ItemPrice";
            public const string ShippingTotal = "ShippingTotal";
            public const string ShippingTotalOverride = "ShippingTotalOverride";
            public const string HandlingTotal = "HandlingTotal";
            public const string Discount = "Discount";
            public const string DiscountPercent = "DiscountPercent";
            public const string AdjustedPrice = "AdjustedPrice";
            public const string CommissionableTotal = "CommissionableTotal";
            public const string CommissionableTotalOverride = "CommissionableTotalOverride";
            public const string ChargeTax = "ChargeTax";
            public const string ChargeShipping = "ChargeShipping";
            public const string Points = "Points";
            public const string MinCustomerSubtotal = "MinCustomerSubtotal";
            public const string MaxCustomerSubtotal = "MaxCustomerSubtotal";
            public const string TaxPercent = "TaxPercent";
            public const string TaxAmount = "TaxAmount";
            public const string TaxPercentCity = "TaxPercentCity";
            public const string TaxAmountCity = "TaxAmountCity";
            public const string TaxAmountCityLocal = "TaxAmountCityLocal";
            public const string TaxPercentState = "TaxPercentState";
            public const string TaxAmountState = "TaxAmountState";
            public const string TaxPercentCounty = "TaxPercentCounty";
            public const string TaxAmountCounty = "TaxAmountCounty";
            public const string TaxAmountCountyLocal = "TaxAmountCountyLocal";
            public const string TaxPercentDistrict = "TaxPercentDistrict";
            public const string TaxAmountDistrict = "TaxAmountDistrict";
            public const string TaxPercentCountry = "TaxPercentCountry";
            public const string TaxAmountCountry = "TaxAmountCountry";
            public const string TaxableTotal = "TaxableTotal";
            public const string DataVersion = "DataVersion";
            public const string ModifiedByUserID = "ModifiedByUserID";
            public const string DynamicKitGroupID = "DynamicKitGroupID";
            public const string OrderItemParentTypeID = "OrderItemParentTypeID";
            public const string ItemPriceActual = "ItemPriceActual";

        }

        public static class OrderItemPrice
        {
            public const string OrderItemPriceID = "OrderItemPriceID";
            public const string OrderItemID = "OrderItemID";
            public const string OriginalUnitPrice = "OriginalUnitPrice";
            public const string ProductPriceTypeID = "ProductPriceTypeID";
            public const string UnitPrice = "UnitPrice";

        }

        public static class OrderAdjustmentOrderLineModification
        {
            public const string OrderAdjustmentOrderLineModificationID = "OrderAdjustmentOrderLineModificationID";
            public const string OrderItemID = "OrderItemID";
            public const string OrderAdjustmentID = "OrderAdjustmentID";
            public const string PropertyName = "PropertyName";
            public const string ModificationOperationID = "ModificationOperationID";
            public const string ModificationDecimalValue = "ModificationDecimalValue";
            public const string ModificationDescription = "ModificationDescription";

        }

        public static class OrderAdjustmentOrderModification
        {
            public const string OrderAdjustmentOrderModificationID = "OrderAdjustmentOrderModificationID";
            public const string OrderAdjustmentID = "OrderAdjustmentID";
            public const string PropertyName = "PropertyName";
            public const string ModificationOperationID = "ModificationOperationID";
            public const string ModificationDecimalValue = "ModificationDecimalValue";
            public const string ModificationDescription = "ModificationDescription";
            public const string OrderCustomerID = "OrderCustomerID";

        }

    }
}
