using System.Linq;

using NetSteps.Data.Entities;

namespace NetSteps.Web.Mvc.Controls.Models
{
	public class OrderEntryModel
	{
		public OrderEntryModel(Order order)
		{
			Order = order;
			BulkAddModal = true;
			AddProduct = true;
			ReplacementTables = false;
			MarkAsAutoship = true;
			EnablePayment = true;
            LoadBulkAddModal = false;
            ReLoadBulkAddModal = true;
			HasOverrides =
				order.TaxAmountTotalOverride != null
				|| order.ShippingTotalOverride != null
			   || order.OrderCustomers.SelectMany(oc => oc.OrderItems)
					.Any(oi => oi.ItemPriceActual != null || oi.CommissionableTotalOverride != null);
		}

		public Order Order { get; private set; }
		public bool BulkAddModal { get; set; }
		public bool AddProduct { get; set; }
		public bool ReplacementTables { get; set; }
		public bool MarkAsAutoship { get; set; }
		public bool EnablePayment { get; set; }
		public bool HasOverrides { get; set; }
        public bool LoadBulkAddModal { get; set; }
        public bool ReLoadBulkAddModal { get; set; }

		#region ReplacementUpdateOrderLocation
		private string _replacementUpdateOrderLocation;
		public string ReplacementUpdateOrderLocation
		{
			get
			{
				return _replacementUpdateOrderLocation ?? "Orders/Replacement/UpdateOrder";
			}
			set
			{
				_replacementUpdateOrderLocation = value;
			}
		}
		#endregion

		#region UpdateCartLocation
		private string _updateCartLocation;
		public string UpdateCartLocation
		{
			get
			{
				return _updateCartLocation ?? "Orders/OrderEntry/UpdateCart";
			}
			set
			{
				_updateCartLocation = value;
			}
		}
		#endregion

		#region GetCatalogLocation
		private string _getCatalogLocation;
		public string GetCatalogLocation
		{
			get
			{
				return _getCatalogLocation ?? "Orders/OrderEntry/GetCatalog";
			}
			set
			{
				_getCatalogLocation = value;
			}
		}
		#endregion

		#region BulkAddToCartLocation
		private string _bulkAddToCartLocation;
		public string BulkAddToCartLocation
		{
			get
			{
				return _bulkAddToCartLocation ?? "Orders/OrderEntry/BulkAddToCart";
			}
			set
			{
				_bulkAddToCartLocation = value;
			}
		}
		#endregion

		#region AddBundleLocation
		private string _addBundleLocation;
		public string AddBundleLocation
		{
			get
			{
				return _addBundleLocation ?? "Orders/OrderEntry/AddBundle";
			}
			set
			{
				_addBundleLocation = value;
			}
		}
		#endregion

		#region AddToDynamicKitGroupLocation
		private string _addToDynamicKitGroupLocation;
		public string AddToDynamicKitGroupLocation
		{
			get
			{
				return _addToDynamicKitGroupLocation ?? "Orders/OrderEntry/AddToDynamicKitGroup";
			}
			set
			{
				_addToDynamicKitGroupLocation = value;
			}
		}
		#endregion

		#region RemoveFromBundleLocation
		private string _removeFromBundleLocation;
		public string RemoveFromBundleLocation
		{
			get
			{
				return _removeFromBundleLocation ?? "Orders/OrderEntry/RemoveFromBundle";
			}
			set
			{
				_removeFromBundleLocation = value;
			}
		}
		#endregion

		#region GetDynamicKitContentsLocation
		private string _getDynamicKitContentsLocation;
		public string GetDynamicKitContentsLocation
		{
			get
			{
				return _getDynamicKitContentsLocation ?? "Orders/OrderEntry/GetDynamicKitContents";
			}
			set
			{
				_getDynamicKitContentsLocation = value;
			}
		}
		#endregion

		#region InPageSearchLocation
		private string _inPageSearchLocation;
		public string InPageSearchLocation
		{
			get
			{
				return _inPageSearchLocation ?? "Orders/OrderEntry/InPageSearch";
			}
			set
			{
				_inPageSearchLocation = value;
			}
		}
		#endregion

		#region ApplyCouponCodeLocation
		private string _applyCouponCodeLocation;
		public string ApplyCouponCodeLocation
		{
			get
			{
				return _applyCouponCodeLocation ?? "Orders/OrderEntry/ApplyCouponCode";
			}
			set
			{
				_applyCouponCodeLocation = value;
			}
		}
		#endregion

		#region AddToCartLocation
		private string _addToCartLocation;
		public string AddToCartLocation
		{
			get
			{
				return _addToCartLocation ?? "Orders/OrderEntry/AddToCart";
			}
			set
			{
				_addToCartLocation = value;
			}
		}
		#endregion

        #region ExistsProductInOrder
        private string _existsProductInOrder;
        public string ExistsProductInOrder
        {
            get
            {
                return _existsProductInOrder ?? "Orders/OrderEntry/ExistsProductInOrder";
            }
            set
            {
                _existsProductInOrder = value;
            }
        }
        #endregion

		#region RemoveFromCartLocation
		private string _removeFromCartLocation;
		public string RemoveFromCartLocation
		{
			get
			{
				return _removeFromCartLocation ?? "Orders/OrderEntry/RemoveFromCart";
			}
			set
			{
				_removeFromCartLocation = value;
			}
		}
		#endregion

		#region SearchProductsLocation
		private string _searchProductsLocation;
		public string SearchProductsLocation
		{
			get
			{
				return _searchProductsLocation ?? "Orders/OrderEntry/SearchProducts";
			}
			set
			{
				_searchProductsLocation = value;
			}
		}
		#endregion

		#region ApplyPaymentLocation
		private string _applyPaymentLocation;
		public string ApplyPaymentLocation
		{
			get
			{
				return _applyPaymentLocation ?? "Orders/OrderEntry/ApplyPayment";
			}
			set
			{
				_applyPaymentLocation = value;
			}
		}
		#endregion

		#region RemovePaymentLocation
		private string _removePaymentLocation;
		public string RemovePaymentLocation
		{
			get
			{
				return _removePaymentLocation ?? "Orders/OrderEntry/RemovePayment";
			}
			set
			{
				_removePaymentLocation = value;
			}
		}
		#endregion

		#region GetPaymentLocation
		private string _getPaymentLocation;
		public string GetPaymentLocation
		{
			get
			{
				return _getPaymentLocation ?? "Orders/OrderEntry/GetPayment";
			}
			set
			{
				_getPaymentLocation = value;
			}
		}
		#endregion

		#region ChangeShippingAddressLocation
		private string _changeShippingAddressLocation;
		public string ChangeShippingAddressLocation
		{
			get
			{
				return _changeShippingAddressLocation ?? "Orders/OrderEntry/ChangeShippingAddress";
			}
			set
			{
				_changeShippingAddressLocation = value;
			}
		}
		#endregion

		#region SetShippingMethodLocation
		private string _setShippingMethodLocation;
		public string SetShippingMethodLocation
		{
			get
			{
				return _setShippingMethodLocation ?? "Orders/OrderEntry/SetShippingMethod";
			}
			set
			{
				_setShippingMethodLocation = value;
			}
		}
		#endregion

		#region GetOverridesLocation
		private string _getOverridesLocation;
		public string GetOverridesLocation
		{
			get
			{
				return _getOverridesLocation ?? "Orders/OrderEntry/GetOverrides";
			}
			set
			{
				_getOverridesLocation = value;
			}
		}
		#endregion

		#region ApplyPromotionCodeLocation

		private string _applyPromotionCodeLocation;
		public string ApplyPromotionCodeLocation
		{
			get { return _applyPromotionCodeLocation ?? "Orders/OrderEntry/ApplyPromotionCode"; }
			set { _applyPromotionCodeLocation = value; }
		}

		#endregion
	}
}