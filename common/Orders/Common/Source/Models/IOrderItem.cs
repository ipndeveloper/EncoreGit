using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderItem.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderItemContracts))]
	public interface IOrderItem
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderItemID for this OrderItem.
		/// </summary>
		int OrderItemID { get; set; }
	
		/// <summary>
		/// The OrderCustomerID for this OrderItem.
		/// </summary>
		int OrderCustomerID { get; set; }
	
		/// <summary>
		/// The OrderItemTypeID for this OrderItem.
		/// </summary>
		short OrderItemTypeID { get; set; }
	
		/// <summary>
		/// The HostessRewardRuleID for this OrderItem.
		/// </summary>
		Nullable<int> HostessRewardRuleID { get; set; }
	
		/// <summary>
		/// The ParentOrderItemID for this OrderItem.
		/// </summary>
		Nullable<int> ParentOrderItemID { get; set; }
	
		/// <summary>
		/// The ProductID for this OrderItem.
		/// </summary>
		Nullable<int> ProductID { get; set; }
	
		/// <summary>
		/// The ProductPriceTypeID for this OrderItem.
		/// </summary>
		Nullable<int> ProductPriceTypeID { get; set; }
	
		/// <summary>
		/// The ProductName for this OrderItem.
		/// </summary>
		string ProductName { get; set; }
	
		/// <summary>
		/// The SKU for this OrderItem.
		/// </summary>
		string SKU { get; set; }
	
		/// <summary>
		/// The CatalogID for this OrderItem.
		/// </summary>
		Nullable<int> CatalogID { get; set; }
	
		/// <summary>
		/// The Quantity for this OrderItem.
		/// </summary>
		int Quantity { get; set; }
	
		/// <summary>
		/// The ItemPrice for this OrderItem.
		/// </summary>
		decimal ItemPrice { get; set; }
	
		/// <summary>
		/// The Discount for this OrderItem.
		/// </summary>
		Nullable<decimal> Discount { get; set; }
	
		/// <summary>
		/// The DiscountPercent for this OrderItem.
		/// </summary>
		Nullable<decimal> DiscountPercent { get; set; }
	
		/// <summary>
		/// The AdjustedPrice for this OrderItem.
		/// </summary>
		Nullable<decimal> AdjustedPrice { get; set; }
	
		/// <summary>
		/// The CommissionableTotal for this OrderItem.
		/// </summary>
		Nullable<decimal> CommissionableTotal { get; set; }
	
		/// <summary>
		/// The CommissionableTotalOverride for this OrderItem.
		/// </summary>
		Nullable<decimal> CommissionableTotalOverride { get; set; }
	
		/// <summary>
		/// The ChargeTax for this OrderItem.
		/// </summary>
		bool ChargeTax { get; set; }
	
		/// <summary>
		/// The ChargeShipping for this OrderItem.
		/// </summary>
		bool ChargeShipping { get; set; }
	
		/// <summary>
		/// The Points for this OrderItem.
		/// </summary>
		Nullable<int> Points { get; set; }
	
		/// <summary>
		/// The MinCustomerSubtotal for this OrderItem.
		/// </summary>
		Nullable<decimal> MinCustomerSubtotal { get; set; }
	
		/// <summary>
		/// The MaxCustomerSubtotal for this OrderItem.
		/// </summary>
		Nullable<decimal> MaxCustomerSubtotal { get; set; }
	
		/// <summary>
		/// The TaxPercent for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxPercent { get; set; }
	
		/// <summary>
		/// The TaxAmount for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxAmount { get; set; }
	
		/// <summary>
		/// The TaxPercentCity for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxPercentCity { get; set; }
	
		/// <summary>
		/// The TaxAmountCity for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxAmountCity { get; set; }
	
		/// <summary>
		/// The TaxAmountCityLocal for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxAmountCityLocal { get; set; }
	
		/// <summary>
		/// The TaxPercentState for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxPercentState { get; set; }
	
		/// <summary>
		/// The TaxAmountState for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxAmountState { get; set; }
	
		/// <summary>
		/// The TaxPercentCounty for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxPercentCounty { get; set; }
	
		/// <summary>
		/// The TaxAmountCounty for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxAmountCounty { get; set; }
	
		/// <summary>
		/// The TaxAmountCountyLocal for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxAmountCountyLocal { get; set; }
	
		/// <summary>
		/// The TaxPercentDistrict for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxPercentDistrict { get; set; }
	
		/// <summary>
		/// The TaxAmountDistrict for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxAmountDistrict { get; set; }
	
		/// <summary>
		/// The TaxPercentCountry for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxPercentCountry { get; set; }
	
		/// <summary>
		/// The TaxAmountCountry for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxAmountCountry { get; set; }
	
		/// <summary>
		/// The TaxableTotal for this OrderItem.
		/// </summary>
		Nullable<decimal> TaxableTotal { get; set; }
	
		/// <summary>
		/// The DataVersion for this OrderItem.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this OrderItem.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The ShippingTotal for this OrderItem.
		/// </summary>
		Nullable<decimal> ShippingTotal { get; set; }
	
		/// <summary>
		/// The HandlingTotal for this OrderItem.
		/// </summary>
		Nullable<decimal> HandlingTotal { get; set; }
	
		/// <summary>
		/// The ShippingTotalOverride for this OrderItem.
		/// </summary>
		Nullable<decimal> ShippingTotalOverride { get; set; }
	
		/// <summary>
		/// The DynamicKitGroupID for this OrderItem.
		/// </summary>
		Nullable<int> DynamicKitGroupID { get; set; }
	
		/// <summary>
		/// The OrderItemParentTypeID for this OrderItem.
		/// </summary>
		Nullable<short> OrderItemParentTypeID { get; set; }
	
		/// <summary>
		/// The ItemPriceActual for this OrderItem.
		/// </summary>
		Nullable<decimal> ItemPriceActual { get; set; }
	
		/// <summary>
		/// The TaxNumber for this OrderItem.
		/// </summary>
		string TaxNumber { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The OrderCustomer for this OrderItem.
		/// </summary>
	    IOrderCustomer OrderCustomer { get; set; }
	
		/// <summary>
		/// The OrderItemReplacement for this OrderItem.
		/// </summary>
	    IOrderItemReplacement OrderItemReplacement { get; set; }
	
		/// <summary>
		/// The ParentOrderItem for this OrderItem.
		/// </summary>
	    IOrderItem ParentOrderItem { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ChildOrderItems for this OrderItem.
		/// </summary>
		IEnumerable<IOrderItem> ChildOrderItems { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderItem"/> to the ChildOrderItems collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItem"/> to add.</param>
		void AddChildOrderItem(IOrderItem item);
	
		/// <summary>
		/// Removes an <see cref="IOrderItem"/> from the ChildOrderItems collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItem"/> to remove.</param>
		void RemoveChildOrderItem(IOrderItem item);
	
		/// <summary>
		/// The OrderAdjustmentOrderLineModifications for this OrderItem.
		/// </summary>
		IEnumerable<IOrderAdjustmentOrderLineModification> OrderAdjustmentOrderLineModifications { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderAdjustmentOrderLineModification"/> to the OrderAdjustmentOrderLineModifications collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderAdjustmentOrderLineModification"/> to add.</param>
		void AddOrderAdjustmentOrderLineModification(IOrderAdjustmentOrderLineModification item);
	
		/// <summary>
		/// Removes an <see cref="IOrderAdjustmentOrderLineModification"/> from the OrderAdjustmentOrderLineModifications collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderAdjustmentOrderLineModification"/> to remove.</param>
		void RemoveOrderAdjustmentOrderLineModification(IOrderAdjustmentOrderLineModification item);
	
		/// <summary>
		/// The OrderItemMessages for this OrderItem.
		/// </summary>
		IEnumerable<IOrderItemMessage> OrderItemMessages { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderItemMessage"/> to the OrderItemMessages collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemMessage"/> to add.</param>
		void AddOrderItemMessage(IOrderItemMessage item);
	
		/// <summary>
		/// Removes an <see cref="IOrderItemMessage"/> from the OrderItemMessages collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemMessage"/> to remove.</param>
		void RemoveOrderItemMessage(IOrderItemMessage item);
	
		/// <summary>
		/// The OrderItemPrices for this OrderItem.
		/// </summary>
		IEnumerable<IOrderItemPrice> OrderItemPrices { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderItemPrice"/> to the OrderItemPrices collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemPrice"/> to add.</param>
		void AddOrderItemPrice(IOrderItemPrice item);
	
		/// <summary>
		/// Removes an <see cref="IOrderItemPrice"/> from the OrderItemPrices collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemPrice"/> to remove.</param>
		void RemoveOrderItemPrice(IOrderItemPrice item);
	
		/// <summary>
		/// The OrderItemProperties for this OrderItem.
		/// </summary>
		IEnumerable<IOrderItemProperty> OrderItemProperties { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderItemProperty"/> to the OrderItemProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemProperty"/> to add.</param>
		void AddOrderItemProperty(IOrderItemProperty item);
	
		/// <summary>
		/// Removes an <see cref="IOrderItemProperty"/> from the OrderItemProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemProperty"/> to remove.</param>
		void RemoveOrderItemProperty(IOrderItemProperty item);
	
		/// <summary>
		/// The OrderItemReturns for this OrderItem.
		/// </summary>
		IEnumerable<IOrderItemReturn> OrderItemReturns { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderItemReturn"/> to the OrderItemReturns collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemReturn"/> to add.</param>
		void AddOrderItemReturn(IOrderItemReturn item);
	
		/// <summary>
		/// Removes an <see cref="IOrderItemReturn"/> from the OrderItemReturns collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemReturn"/> to remove.</param>
		void RemoveOrderItemReturn(IOrderItemReturn item);
	
		/// <summary>
		/// The OrderShipmentPackageItems for this OrderItem.
		/// </summary>
		IEnumerable<IOrderShipmentPackageItem> OrderShipmentPackageItems { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderShipmentPackageItem"/> to the OrderShipmentPackageItems collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderShipmentPackageItem"/> to add.</param>
		void AddOrderShipmentPackageItem(IOrderShipmentPackageItem item);
	
		/// <summary>
		/// Removes an <see cref="IOrderShipmentPackageItem"/> from the OrderShipmentPackageItems collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderShipmentPackageItem"/> to remove.</param>
		void RemoveOrderShipmentPackageItem(IOrderShipmentPackageItem item);
	
		/// <summary>
		/// The OriginalOrderItemReturns for this OrderItem.
		/// </summary>
		IEnumerable<IOrderItemReturn> OriginalOrderItemReturns { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderItemReturn"/> to the OriginalOrderItemReturns collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemReturn"/> to add.</param>
		void AddOriginalOrderItemReturn(IOrderItemReturn item);
	
		/// <summary>
		/// Removes an <see cref="IOrderItemReturn"/> from the OriginalOrderItemReturns collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemReturn"/> to remove.</param>
		void RemoveOriginalOrderItemReturn(IOrderItemReturn item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderItem))]
		internal abstract class OrderItemContracts : IOrderItem
		{
		    #region Primitive properties
		
			int IOrderItem.OrderItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItem.OrderCustomerID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IOrderItem.OrderItemTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderItem.HostessRewardRuleID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderItem.ParentOrderItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderItem.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderItem.ProductPriceTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItem.ProductName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItem.SKU
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderItem.CatalogID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItem.Quantity
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			decimal IOrderItem.ItemPrice
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.Discount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.DiscountPercent
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.AdjustedPrice
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.CommissionableTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.CommissionableTotalOverride
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderItem.ChargeTax
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderItem.ChargeShipping
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderItem.Points
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.MinCustomerSubtotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.MaxCustomerSubtotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxPercent
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxPercentCity
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxAmountCity
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxAmountCityLocal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxPercentState
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxAmountState
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxPercentCounty
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxAmountCounty
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxAmountCountyLocal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxPercentDistrict
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxAmountDistrict
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxPercentCountry
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxAmountCountry
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.TaxableTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IOrderItem.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderItem.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.ShippingTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.HandlingTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.ShippingTotalOverride
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderItem.DynamicKitGroupID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IOrderItem.OrderItemParentTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItem.ItemPriceActual
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItem.TaxNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrderCustomer IOrderItem.OrderCustomer
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrderItemReplacement IOrderItem.OrderItemReplacement
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrderItem IOrderItem.ParentOrderItem
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderItem> IOrderItem.ChildOrderItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItem.AddChildOrderItem(IOrderItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItem.RemoveChildOrderItem(IOrderItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderAdjustmentOrderLineModification> IOrderItem.OrderAdjustmentOrderLineModifications
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItem.AddOrderAdjustmentOrderLineModification(IOrderAdjustmentOrderLineModification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItem.RemoveOrderAdjustmentOrderLineModification(IOrderAdjustmentOrderLineModification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderItemMessage> IOrderItem.OrderItemMessages
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItem.AddOrderItemMessage(IOrderItemMessage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItem.RemoveOrderItemMessage(IOrderItemMessage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderItemPrice> IOrderItem.OrderItemPrices
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItem.AddOrderItemPrice(IOrderItemPrice item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItem.RemoveOrderItemPrice(IOrderItemPrice item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderItemProperty> IOrderItem.OrderItemProperties
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItem.AddOrderItemProperty(IOrderItemProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItem.RemoveOrderItemProperty(IOrderItemProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderItemReturn> IOrderItem.OrderItemReturns
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItem.AddOrderItemReturn(IOrderItemReturn item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItem.RemoveOrderItemReturn(IOrderItemReturn item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderShipmentPackageItem> IOrderItem.OrderShipmentPackageItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItem.AddOrderShipmentPackageItem(IOrderShipmentPackageItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItem.RemoveOrderShipmentPackageItem(IOrderShipmentPackageItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderItemReturn> IOrderItem.OriginalOrderItemReturns
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItem.AddOriginalOrderItemReturn(IOrderItemReturn item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItem.RemoveOriginalOrderItemReturn(IOrderItemReturn item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
