using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Tax;
using NetSteps.Encore.Core.IoC;
using NetSteps.OrderAdjustments.Common.Model;
using System.Collections.ObjectModel;
using NetSteps.Common.Collections;
namespace NetSteps.Data.Entities
{
	public partial class OrderCustomer : ITempGuid, IOrderCustomer, IDateLastModified
	{
		#region Members
		
		private int _productPriceTypeID;
		private bool _calculationsDirty = true;
		private bool _needsAutobundling;

		#endregion

		#region Properties
		internal bool CalculationsDirty
		{
			get { return _calculationsDirty; }
			set { _calculationsDirty = value; }
		}

		internal bool NeedsAutoBundling
		{
			get { return _needsAutobundling; }
			set { _needsAutobundling = value; }
		}

		public string FullName
		{
			get
			{
				return Account.ToFullName(AccountInfo.FirstName, string.Empty, AccountInfo.LastName, SmallCollectionCache.Instance.Languages.GetById(ApplicationContext.Instance.CurrentLanguageID).CultureInfo);
			}
		}

		public string FirstName
		{
			get
			{
				return AccountInfo.FirstName;
			}
		}

		public string LastName
		{
			get
			{
				return AccountInfo.LastName;
			}
		}

	    public int MarketID
	    {
	        get
	        {
	            return AccountInfo.MarketID;
	        }
	    }

	    public int MarketDefaultCurrencyID
	    {
	        get
	        {
	            Market market = null;
	            if (AccountInfo.MarketID != 0)
	            {
	                 market = SmallCollectionCache.Instance.Markets.GetById(AccountInfo.MarketID);
	            }

	            return market != null ? market.GetDefaultCurrencyID() : SmallCollectionCache.Instance.Countries.GetById((int)Constants.Country.UnitedStates).CurrencyID;
	        }
	    }

		[DataMember]
		public List<OrderItem> ParentOrderItems
		{
			get { return this.GetParentOrderItems(); }
			set { } // For WCF Deserialization
		}

		private AccountSlimSearchData _accountInfo;
		public AccountSlimSearchData AccountInfo
		{
			get
			{
				if (_accountInfo != null && _accountInfo.AccountID != this.AccountID)
				{
					_accountInfo = null;
				}
				if (_accountInfo == null && this.AccountID != 0)
				{
					_accountInfo = Account.LoadSlim(this.AccountID);
				}
				return _accountInfo;
			}
		}

		private short _effectiveOrderAccountTypeID = (short)Constants.AccountType.NotSet;
		public short EffectiveOrderAccountTypeID
		{
			get
			{
				return (short)_effectiveOrderAccountTypeID;
			}
			set
			{
				_effectiveOrderAccountTypeID = value;
			}
		}

		private short _defaultAccountTypeID = (short)Constants.AccountType.RetailCustomer;
		public short AccountTypeID
		{
			get
			{
				short acctTypeID = _defaultAccountTypeID;
				if (EffectiveOrderAccountTypeID == (short)Constants.AccountType.NotSet)
				{
					if (this.AccountInfo != null)
						acctTypeID = this.AccountInfo.AccountTypeID;
				}
				else
					acctTypeID = EffectiveOrderAccountTypeID;

				return acctTypeID;
			}
		}

		/// <summary>
		/// Returns the default product price type for the customer account and order type.
		/// </summary>
		public int ProductPriceTypeID
		{
			get
			{
				var priceTypeService = Create.New<IPriceTypeService>();
				var priceType = priceTypeService.GetPriceType(this.AccountTypeID, (int)ConstantsGenerated.PriceRelationshipType.Products, ApplicationContext.Instance.StoreFrontID, this.Order.OrderTypeID);
				_productPriceTypeID = (priceType != null) ? priceType.PriceTypeID : 0;

				return _productPriceTypeID;
			}
		}

		/// <summary>
		/// Returns the default commissionable price type for the customer account and order type.
		/// </summary>
		public int CommissionablePriceTypeID
		{
			get
			{
				var priceTypeService = Create.New<IPriceTypeService>();
				var priceType = priceTypeService.GetPriceType(this.AccountTypeID, (int)ConstantsGenerated.PriceRelationshipType.Commissions, ApplicationContext.Instance.StoreFrontID, this.Order.OrderTypeID);
				_productPriceTypeID = (priceType != null) ? priceType.PriceTypeID : 0;

				return _productPriceTypeID;
			}
		}

		public bool IsHostess { get { return OrderCustomerTypeID == (int)Constants.OrderCustomerType.Hostess; } }

		public ITaxService TaxService { get { return Create.New<ITaxService>(); } }
		public ITotalsCalculator TotalsCalculator { get { return Create.New<ITotalsCalculator>(); } }
		public IShippingCalculator ShippingCalculator { get { return Create.New<IShippingCalculator>(); } }

		#endregion

		#region Legacy Properties
		[DataMember]
		public decimal SubtotalHostess { get; set; }

		[DataMember]
		public decimal SubtotalHostessRetail { get; set; }

		[DataMember]
		public decimal SubtotalHostessDiscounted { get; set; }

		[DataMember]
		public decimal SubtotalHostessFree { get; set; }

		[DataMember]
		public decimal SubtotalRetail { get; set; }

		[DataMember]
		public decimal DeferredBalance { get; set; }

		[DataMember]
		public decimal QtySubtotal { get; set; }

		[DataMember]
		public decimal QtyRetail { get; set; }

		[DataMember]
		public bool OverrideShippingAmount { get; set; }

		// Are these extra tax properties needed? - JHE
		[DataMember]
		public decimal TaxPercent { get; set; }

		[DataMember]
		public decimal TaxPercentCity { get; set; }

		[DataMember]
		public decimal TaxPercentCounty { get; set; }

		[DataMember]
		public decimal TaxPercentDistrict { get; set; }

		[DataMember]
		public decimal TaxPercentState { get; set; }

        //CGI(CMR)-09/04/2015-Inicio
        [DataMember]
        public decimal SubTotalQV { get; set; }
        
        [DataMember]
        public decimal SumAdjustedItemPrice { get; set; } 

        [DataMember]
        public decimal SumPreadjustedItemPrice { get; set; }

        [DataMember]
        public decimal SubTotalAdjustedItemPrice { get; set; }

        [DataMember]
        public decimal SubTotalPreadjustedItemPrice { get; set; }  
        //CGI(CMR)-09/04/2015-Fin

		#endregion

		#region Legacy OrderItemTotals Logic Replacement
		// Are these truly no longer needed?  They seem harmless as long as they are using the correct order item properties.

		//[Obsolete("This is to ease the transition from OrderItemTotals - don't use it", false)]
		public decimal ParentSubtotalOrderItemTotalByType(Constants.OrderItemType kind)
		{
			var total = 0m;
			foreach (var orderItem in ParentOrderItems.Where(i => i.OrderItemTypeID == (int)kind))
			{
				total += orderItem.GetAdjustedPrice() * orderItem.Quantity;
			}

			return total;
		}

		//[Obsolete("This is to ease the transition from OrderItemTotals - don't use it", false)]
		public virtual decimal ParentSubtotalForShippingOrderItemTotalByType(Constants.OrderItemType kind)
		{
			return BusinessLogic.ParentSubtotalForShippingOrderItemTotalByType(this, kind);
		}

		//[Obsolete("This is to ease the transition from OrderItemTotals - don't use it", false)]
		public decimal WeightTotalForShippingOrderItemTotalByType(Constants.OrderItemType kind)
		{
			var inventory = Create.New<InventoryBaseRepository>();
			var total = 0m;
			foreach (var orderItem in ParentOrderItems.Where(i => i.OrderItemTypeID == (int)kind))
			{
				var product = inventory.GetProduct(orderItem.ProductID.Value);
				if (((!product.Weight.HasValue || product.Weight.Value == 0f)) && orderItem.HasChildOrderItems)
				{
					foreach (OrderItem childItem in orderItem.ChildOrderItems)
					{
						var childProduct = inventory.GetProduct((int)childItem.ProductID);
						total += orderItem.ChargeShipping && product.ProductBase.IsShippable ? (decimal)(childProduct.Weight ?? 0d) * childItem.Quantity : 0m;
					}
				}
				else
				{
					if (product.Weight.HasValue)
					{
						total += orderItem.ChargeShipping && product.ProductBase.IsShippable ? (decimal)(product.Weight ?? 0d) * orderItem.Quantity : 0m;
					}
				}
			}

			return total;
		}

		#endregion

		#region Construstors
		public OrderCustomer(Account account)
		{
			this.StartTrackingRecursive();
			if (account != null)
			{
				this.AccountID = account.AccountID;
				this.IsTaxExempt = account.IsTaxExempt == null ? false : account.IsTaxExempt;
				if (account.AccountID == 0)
				{
					this._defaultAccountTypeID = account.AccountTypeID;
				}
			}
		}
		#endregion

		#region Methods

		internal List<OrderItem> GetParentOrderItems()
		{
			return this.OrderItems.Where(oi => oi.ParentOrderItem == null).ToList();
		}

		public OrderPayment AddNewPayment(Constants.PaymentType paymentType)
		{
			OrderPayment payment = new OrderPayment();
			payment.OrderCustomer = this;
			payment.PaymentTypeID = (int)paymentType;
			payment.OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Pending;
			this.OrderPayments.Add(payment);

			return payment;
		}

		public void RemoveAllPayments()
		{
			foreach (OrderPayment payment in this.OrderPayments.ToList())
			{
				RemovePayment(payment);
			}
		}

		public void RemovePayment(string orderPaymentGuid)
		{
			try
			{
                
                var orderPayment = OrderPayments.FirstOrDefault(op => op.Guid == Guid.Parse(orderPaymentGuid));

               
				if (orderPayment == null)
					throw new Exception(Translation.GetTerm("PaymentDoesNotExist", "That payment does not exist."));
				RemovePayment(orderPayment);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (this.Order != null) ? this.Order.OrderID.ToIntNullable() : null);
				throw exception;
			}
		}

		/// <summary>
		/// Will remove the payment if it has no payment results.  If it has payment results but the status is not completed it will cancel the payment.  Completed payments are ignored.
		/// </summary>
		/// <param name="orderPayment"></param>
		public void RemovePayment(OrderPayment orderPayment)
		{
			Contract.Requires(orderPayment != null);

			try
			{
				bool hasExistingOrderPaymentResults = OrderPayment.HasOrderPaymentResults(orderPayment.OrderPaymentID);

				if (hasExistingOrderPaymentResults)
				{
					if (orderPayment.OrderPaymentStatusID != (short)Constants.OrderPaymentStatus.Completed)
					{
						orderPayment.OrderPaymentStatusID = (short)Constants.OrderPaymentStatus.Cancelled;
					}
				}
				else
				{
					if (orderPayment.ChangeTracker.State != ObjectState.Added)
					{
						orderPayment.MarkAsDeleted();
					}
					this.OrderPayments.Remove(orderPayment);
					this.Order.OrderPayments.Remove(orderPayment);
				}
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (this.Order != null) ? this.Order.OrderID.ToIntNullable() : null);
				throw exception;
			}
		}

		public IEnumerable<ShippingMethodWithRate> GetShippingMethods()
		{
			try
			{
				return BusinessLogic.GetShippingMethods(this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public void SetShippingMethod(int? shippingMethodId)
		{
			if (this.OrderShipments.Count > 0)
			{
				OrderShipment shipment = this.OrderShipments[0];
				shipment.ShippingMethodID = shippingMethodId;

				this.CalculationsDirty = true;
			}
		}

		public decimal GetDefaultShippingAmount()
		{
			var rates = ShippingCalculator.GetShippingMethodsWithRates(this);
			if (rates == null || rates.Count == 0)
			{
				return 0M;
			}
			var defaultRate = rates.SingleOrDefault(x => x.IsDefaultShippingMethod);
			if (defaultRate != null)
			{
				return defaultRate.ShippingAmount;
			}
			else
			{
				// Since we've already got the rates, let's use them,
				// instead of calling ShippingCalculator.GetLeastExpensiveShippingMethod and recalculating the rates.
				var leastExpensive = rates.MinBy(r => r.ShippingAmount);
				return leastExpensive.ShippingAmount;
			}
		}

		/// <summary>
		/// Sets the account type that will be returned if the actual customer account type can not be determined.
		/// </summary>
		/// <param name="accountTypeID">The new default account type</param>
		public void SetDefaultAccountTypeID(short accountTypeID)
		{
			_defaultAccountTypeID = accountTypeID;
		}

		public decimal GetShippableSubtotal()
		{
			var shippableOrderItems = OrderItems.Where(x => x.ChargeShipping);
			return shippableOrderItems.Sum(x => x.GetAdjustedPrice() * x.Quantity);
		}

		#endregion

		#region Host Reward Validation

		public int GetNumberOfRedeemedItems(OrderItem orderItem)
		{
			return GetNumberOfRedeemedItems(orderItem.HostessRewardRuleID);
		}

		public int GetNumberOfRedeemedItems(int? hostessRewardRuleID)
		{
			int result = 0;

			if (hostessRewardRuleID.HasValue)
			{
				var orderItems = this.OrderItems.Where(oi => oi.HostessRewardRuleID == hostessRewardRuleID).ToList();

				result = orderItems.Sum(oi => oi.Quantity);
			}

			return result;
		}

		public int GetNumberOfRedeemedItemsByType(ConstantsGenerated.HostessRewardType hostessRewardType)
		{
			int result = 0;

			var rewardRuleIDs = SmallCollectionCache.Instance.HostessRewardRules.Where(r => r.HostessRewardTypeID == (int)hostessRewardType)
				.Select(r => r.HostessRewardRuleID).ToList();
			rewardRuleIDs.Each(id => result += this.OrderItems.Where(oi => oi.HostessRewardRuleID == id).Sum(oi => oi.Quantity));

			return result;
		}

		public int GetTotalRedeemableItems(OrderItem orderItem)
		{
			return GetTotalRedeemableItems(orderItem.HostessRewardRuleID);
		}

		public int GetTotalRedeemableItems(int? hostRewardRuleID)
		{
			int result = 0;

			if (hostRewardRuleID.HasValue)
			{
				var hostRewardRule = SmallCollectionCache.Instance.HostessRewardRules.GetById(hostRewardRuleID.ToInt());

				result = hostRewardRule.Products.ToInt();
			}

			return result;
		}

		public void ClearHostessRewards()
		{
			BusinessLogic.ClearHostessRewards(this);
		}

		public BasicResponse ValidateHostessRewards()
		{
			return BusinessLogic.ValidateHostessRewards(this);
		}

		public BasicResponse ValidateHostessRewardItem(OrderItem orderItem, Order order, bool includeQuantityInCheck)
		{
			return BusinessLogic.ValidateHostessRewardItem(this, includeQuantityInCheck ? orderItem.Quantity : 0, orderItem.HostessRewardRuleID, order);
		}

		public BasicResponse ValidateHostessRewardItem(int quantity, int? hostRewardRuleId, Order order)
		{
			return BusinessLogic.ValidateHostessRewardItem(this, quantity, hostRewardRuleId, order);
		}

		public BasicResponse ValidateHostessRewardTotal(HostessRewardRule rule, Order order)
		{
			return BusinessLogic.ValidateHostessRewardTotal(rule, order);
		}

		#endregion

		#region Calculation Methods

		internal void CalculateTax()
		{
			try
			{
				if (!Order.TaxAmountTotalOverride.HasValue)
					TaxService.CalculateTax(this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		internal void FinalizeTax()
		{

		}

		internal void FinalizePartialReturnTax()
		{
			try
			{
				TaxService.FinalizePartialReturnTax(this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		internal void CalculateReturnOrderTax()
		{
			try
			{
				TaxService.CalculateReturnOrderTax(this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		internal void CalculateShipping()
		{
			try
			{
				if (!Order.ShippingTotalOverride.HasValue)
					ShippingCalculator.CalculateShipping(this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		internal void ResetOrderCustomerTaxes()
		{
			TaxAmountCity = 0;
			TaxAmountCounty = 0;
			TaxAmountDistrict = 0;
			TaxAmountState = 0;
			TaxAmountTotal = 0;
			TaxAmountShipping = 0;

			TaxPercentCity = 0;
			TaxPercentCounty = 0;
			TaxPercentDistrict = 0;
			TaxPercentState = 0;
			TaxPercent = 0;

			foreach (OrderItem orderItem in OrderItems)
			{
				orderItem.Taxes.TaxAmountCity = 0;
				orderItem.Taxes.TaxAmountCounty = 0;
				orderItem.Taxes.TaxAmountCountry = 0;
				orderItem.Taxes.TaxAmountCityLocal = 0;
				orderItem.Taxes.TaxAmountState = 0;
				orderItem.Taxes.TaxAmountDistrict = 0;
				orderItem.Taxes.TaxAmountShipping = 0;
				orderItem.Taxes.TaxAmountTotal = 0;

				orderItem.Taxes.TaxPercentCity = 0;
				orderItem.Taxes.TaxPercentCounty = 0;
				orderItem.Taxes.TaxPercentCountry = 0;
				orderItem.Taxes.TaxPercentCityLocal = 0;
				orderItem.Taxes.TaxPercentState = 0;
				orderItem.Taxes.TaxPercentDistrict = 0;
				orderItem.Taxes.TaxPercent = 0;
			}
		}
		#endregion

		#region ITempGuid Members

		private Guid? _guid = null;
		public Guid Guid
		{
			get
			{
				if (_guid == null)
					_guid = Guid.NewGuid();
				return _guid.Value;
			}
			internal set
			{
				_guid = value;
			}
		}

		#endregion

		public IList<OrderItem> GetPromotionallyAddedOrderItems()
		{
			//If list is rendered via a clause it should be read only or considered enumerable
			//because changing the list has no impact on the order
			return OrderItems.Where(x => x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == (int)OrderAdjustmentOrderLineOperationKind.AddedItem)).ToList().AsReadOnly();
		}

		IList<IOrderItem> IOrderCustomer.OrderItems
		{
			get 
			{
				//Wrap the order items in an abstract list the add/remove/clear methods
				//map back to the original collection so that implementing collection is also modifed
				return OrderItems.AsAbstract<IOrderItem, OrderItem>();

		}

		}

		public IList<IOrderAdjustmentOrderModification> OrderModifications
		{
			// If the add order modification method is exposed it would appear that the intention of this
			// list should be read only if that is not the case it should be wrapped in the abstract list
			// like the order items list above.
			get { return OrderAdjustmentOrderModifications.Cast<IOrderAdjustmentOrderModification>().ToList().AsReadOnly(); }

		}

		public void AddOrderModification(IOrderAdjustmentOrderModification modification)
		{
			OrderAdjustmentOrderModifications.Add((OrderAdjustmentOrderModification)modification);
		}

		public decimal AdjustedHandlingTotal
		{
			get
			{
				var total = HandlingTotal.HasValue ? HandlingTotal.Value : 0;
				if (OrderAdjustmentOrderModifications != null && OrderAdjustmentOrderModifications.Count > 0)
				{
					foreach (var mod in OrderAdjustmentOrderModifications)
					{
						if (mod.PropertyName == OrderAdjustablePropertyNames.HandlingTotal)
						{
							total -= mod.CalculatedValue(total);
						}
					}
				}

				return Math.Max(total.GetRoundedNumber(), 0);
			}
            
		}
        

		public decimal AdjustedShippingTotal
		{
			get
			{
				var total = ShippingTotal.HasValue ? ShippingTotal.Value : 0;
				if (total > 0)
				{
					total -= ShippingAdjustmentAmount;
				}

				return Math.Max(total.GetRoundedNumber(), 0);
			}
		}

		public decimal AdjustedTaxTotal
		{
			get
			{
				var total = TaxAmountTotal.HasValue ? TaxAmountTotal.Value : 0;
				if (OrderAdjustmentOrderModifications != null && OrderAdjustmentOrderModifications.Count > 0)
				{
					foreach (var mod in OrderAdjustmentOrderModifications.Where(om => om.PropertyName == OrderAdjustablePropertyNames.TaxAmountTotal))
					{
						total -= mod.CalculatedValue(total);
					}
				}

				return Math.Max(total.GetRoundedNumber(), 0);
			}
		}

		public IList<OrderItem> LoadOrderItems()
		{
			return this.GetRepository().LoadOrderItems(this.OrderCustomerID);
		}

		public IList<OrderPayment> LoadOrderPayments()
		{
			return this.GetRepository().LoadOrderPayments(this.OrderCustomerID);
		}

		public decimal ShippingAdjustmentAmount
		{
			get
			{
				decimal adjustmentTotal = 0;
				if (OrderAdjustmentOrderModifications != null && OrderAdjustmentOrderModifications.Count > 0)
				{
					foreach (var mod in OrderAdjustmentOrderModifications)
					{
						if (mod.PropertyName == OrderAdjustablePropertyNames.ShippingTotal)
						{
							adjustmentTotal += mod.CalculatedValue(GetDefaultShippingAmount());
						}
					}
				}

				return Math.Min(adjustmentTotal, GetDefaultShippingAmount());
			}
		}

		public decimal GetShippingAmountAfterAdjustment(decimal shippingAmount)
		{
			return Math.Max(shippingAmount - ShippingAdjustmentAmount, 0);
		}

		public decimal AdjustedSubTotal
		{
			get
			{
				decimal adjustmentTotal = Subtotal.HasValue ? Subtotal.Value : 0;
				if (OrderAdjustmentOrderModifications != null && OrderAdjustmentOrderModifications.Count > 0)
				{
					foreach (var mod in OrderAdjustmentOrderModifications)
					{
						if (mod.PropertyName == OrderAdjustablePropertyNames.OrderSubtotal)
						{
							adjustmentTotal -= mod.CalculatedValue(Subtotal.Value);
						}
					}
				}

				return adjustmentTotal.GetRoundedNumber();
			}
            set { decimal myvar = value; } //CGI(CMR)-06/04/2015
		}

		public decimal ProductSubTotal
		{
			get { return Subtotal.ToDecimal(); }
		}

		public IEnumerable<IOrderItem> AdjustableOrderItems
		{
			get
			{
				return OrderItems.Where(item => item.ParentOrderItem == null &&
											item.ParentOrderItemID == null &&
											!item.IsHostReward &&
											item.ItemPriceActual == null &&
											item.CommissionableTotalOverride == null);
			}
		}

		public bool IsTooBigForBundling()
		{
			var bundlingCartSizeLimitWithQuantity = Convert.ToInt32(NetSteps.Common.Configuration.ConfigurationManager.AppSettings["BundlingCartSizeLimitWithQuantity"] ?? "201");

			return this.OrderItems.Sum(oi => oi.Quantity) > bundlingCartSizeLimitWithQuantity;
		}
	}
}
