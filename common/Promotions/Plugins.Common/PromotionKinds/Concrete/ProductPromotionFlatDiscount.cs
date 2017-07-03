using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Components;
using NetSteps.Promotions.Plugins.Common.Rewards;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.OrderAdjustments.Common;
using NetSteps.Data.Common.Entities;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace NetSteps.Promotions.Plugins.Common.PromotionKinds.Concrete
{
	[Serializable]
	public class ProductPromotionFlatDiscount : SingleMarketStandardQualificationPromotion, IProductPromotionFlatDiscount
	{
		
		public override string PromotionKind
		{
			get { return PromotionKindNames.ProductFlatDiscount; }
		}
        
        public new static class QualificationNames
        {
            public const string OrderSubtotalRange = "Order Subtotal Range";
            public const string CustomerSubtotalRange = "Customer Subtotal Range";
            public const string CustomerPriceTypeTotalRange = "Customer PriceType Range";
            public const string OrderPriceTypeTotalRange = "Order PriceType Range";
            public const string RequiredItems = "Required Items";
            public const string MarketList = "Required Market";
            public const string PartyHostPromotion = "Party Host Promotion";
        }

        public static class RewardNames
        {
            public const string RewardAddedProducts = "Reward Added Products";
            public const string RewardProductSelections = "Reward Product Selections";
            public const string ReducedSubtotal = "Reduced Subtotal";
            public const string ReducedShipping = "Reduced Shipping";
        }

		public OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind Operation
		{
			get { return OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.FlatAmount; }
		}

		public decimal GetAdjustedValue(int productID, decimal initialAmount, IPriceType priceType)
		{
			var productReward = ProductRewards.OfType<IProductReward>().Where(rew => rew.ProductID == productID).SingleOrDefault();
			if (productReward != null && productReward.GetPriceTypeEffect(priceType) != null)
			{
				if (productReward.GetPriceTypeEffect(priceType).MarketDecimalOperands.ContainsKey(MarketID))
				{
					var adjustmentAmount = productReward.GetPriceTypeEffect(priceType).MarketDecimalOperands[MarketID];
					var calculator = Create.New<IOrderAdjustmentValueCalculator>();
					return calculator.CalculateOrderLineAdjustmentValue(Operation, adjustmentAmount, initialAmount);
				}
			}
			return initialAmount;
		}

		public void AddProductAdjustment(int productID, IPriceType priceType, decimal flatAmount)
		{
			var productReward = ProductRewards.Where(rew => rew.ProductID == productID).SingleOrDefault();
			if (productReward == null)
			{
				productReward = Create.New<IProductReward>();
				productReward.ProductID = productID;
				PromotionRewards.Add(productID.ToString(), productReward);
			}
			if (!productReward.ContainsPriceTypeEffect(priceType))
			{
				productReward.AddPriceTypeEffect(priceType, (int)OrderAdjustmentOrderLineOperationKind.FlatAmount);
				productReward.GetPriceTypeEffect(priceType).MarketDecimalOperands.Add(MarketID, flatAmount);
			}
			else
			{
				productReward.GetPriceTypeEffect(priceType).MarketDecimalOperands[MarketID] = flatAmount;
			}
			productReward.GetPriceTypeEffect(priceType).DefaultMarketID = MarketID;
		}

		public void DeleteProductAdjustment(int productID, IPriceType priceType)
		{
			var effect = GetEffectByPriceTypeID(productID, priceType);
			if (effect != null)
			{
				effect.MarketDecimalOperands.Remove(MarketID);
			}
		}

		public void DeleteProductAdjustments(int productID)
		{
			PromotionRewards.Remove(productID.ToString());
		}


		public decimal GetAdjustmentAmount(int productID, IPriceType priceType)
		{
			var effect = GetEffectByPriceTypeID(productID, priceType);
			if (effect!=null && effect.MarketDecimalOperands.ContainsKey(MarketID))
				return effect.MarketDecimalOperands[MarketID];
			return 0M;
		}

		private IReduceOrderItemPropertyValuePromotionRewardEffect GetEffectByPriceTypeID(int productID, IPriceType priceType)
		{
			var productReward = GetReward(productID);
			if (productReward == null)
				return null;
			return productReward.GetPriceTypeEffect(priceType);
		}

		public IEnumerable<int> PromotedProductIDs
		{
			get
			{
				return ProductRewards.Select(reward => reward.ProductID);
			}
		}

		public IEnumerable<IPriceType> GetPromotedPriceTypesForProductID(int productID)
		{
			var productReward = GetReward(productID);
			if (productReward == null)
				return new IPriceType[0];
			return productReward.PriceTypes;
		}

		protected IProductReward GetReward(int productID)
		{
			return ProductRewards.SingleOrDefault(reward => reward.ProductID == productID);
		}


		protected IEnumerable<IProductReward> ProductRewards
		{
			get
			{
				return PromotionRewards.Values.OfType<IProductReward>();
			}
		}

		public decimal GetPromotionAdjustmentAmount(int productID, int marketID, decimal originalPrice, IPriceType productPriceType)
		{
			if (marketID != MarketID)
				return 0;
			return GetAdjustedValue(productID, originalPrice, productPriceType);
		}

        #region Qualifications

        #region Customer Subtotal Range

        public ICustomerSubtotalRangeQualificationExtension CustomerSubtotalRangeExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.CustomerSubtotalRange))
                {
                    return (ICustomerSubtotalRangeQualificationExtension)PromotionQualifications[QualificationNames.CustomerSubtotalRange];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.CustomerSubtotalRange);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.CustomerSubtotalRange, value);
                }
            }
        }

        public void SetCustomerSubtotalRange(decimal minimumValue, int currencyID)
        {
            SetCustomerSubtotalRange(minimumValue, null, currencyID);
        }

        public void SetCustomerSubtotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID)
        {
            if (minimumValue > 0)
            {
                if (CustomerSubtotalRangeExtension == null)
                {
                    CustomerSubtotalRangeExtension = Create.New<ICustomerSubtotalRangeQualificationExtension>();
                }
                if (CustomerSubtotalRangeExtension.CustomerSubtotalRangesByCurrencyID.ContainsKey(currencyID))
                {
                    var customerSubtotalRangeByCurrencyId = CustomerSubtotalRangeExtension.CustomerSubtotalRangesByCurrencyID[currencyID];
                    customerSubtotalRangeByCurrencyId = customerSubtotalRangeByCurrencyId ?? new CustomerSubtotalRange();
                    customerSubtotalRangeByCurrencyId.Minimum = minimumValue;
                    customerSubtotalRangeByCurrencyId.Maximum = maximumValue;
                }
                else
                {
                    CustomerSubtotalRangeExtension.CustomerSubtotalRangesByCurrencyID.Add(currencyID, new CustomerSubtotalRange(minimumValue, maximumValue));
                }
            }
            else
            {
                RemoveCustomerSubtotalRange(currencyID);
            }
        }

        public void RemoveCustomerSubtotalRanges()
        {
            CustomerSubtotalRangeExtension = null;
        }

        public void RemoveCustomerSubtotalRange(int currencyID)
        {
            if (CustomerSubtotalRangeExtension == null)
            {
                return;
            }
            else
            {
                CustomerSubtotalRangeExtension.CustomerSubtotalRangesByCurrencyID.Remove(currencyID);
                if (CustomerSubtotalRangeExtension.CustomerSubtotalRangesByCurrencyID.Count == 0)
                {
                    CustomerSubtotalRangeExtension = null;
                }
            }
        }

        public ICustomerSubtotalRange GetCustomerSubtotalRange(int currencyID)
        {
            if (CustomerSubtotalRangeExtension == null || !CustomerSubtotalRangeExtension.CustomerSubtotalRangesByCurrencyID.ContainsKey(currencyID))
            {
                return new CustomerSubtotalRange(0, null);
            }
            else
            {
                return CustomerSubtotalRangeExtension.CustomerSubtotalRangesByCurrencyID[currencyID];
            }
        }

        public ICustomerSubtotalRange GetDefaultCustomerSubtotalRange()
        {
            if (CustomerSubtotalRangeExtension == null || CustomerSubtotalRangeExtension.CustomerSubtotalRangesByCurrencyID.Count == 0)
            {
                return new CustomerSubtotalRange(0, null);
            }
            else
            {
                return CustomerSubtotalRangeExtension.CustomerSubtotalRangesByCurrencyID[CustomerSubtotalRangeExtension.CustomerSubtotalRangesByCurrencyID.Keys.First()];
            }
        }

        #endregion

        #region Order Subtotal Range

        public IOrderSubtotalRangeQualificationExtension OrderSubtotalRangeExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.OrderSubtotalRange))
                {
                    return (IOrderSubtotalRangeQualificationExtension)PromotionQualifications[QualificationNames.OrderSubtotalRange];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.OrderSubtotalRange);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.OrderSubtotalRange, value);
                }
            }
        }

        public void SetOrderSubtotalRange(decimal minimumValue, int currencyID)
        {
            SetOrderSubtotalRange(minimumValue, null, currencyID);
        }

        public void SetOrderSubtotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID)
        {
            if (minimumValue > 0)
            {
                if (OrderSubtotalRangeExtension == null)
                {
                    OrderSubtotalRangeExtension = Create.New<IOrderSubtotalRangeQualificationExtension>();
                }
                if (OrderSubtotalRangeExtension.OrderSubtotalRangesByCurrencyID.ContainsKey(currencyID))
                {
                    var orderSubtotalRangesByCurrencyId = OrderSubtotalRangeExtension.OrderSubtotalRangesByCurrencyID[currencyID];
                    orderSubtotalRangesByCurrencyId = orderSubtotalRangesByCurrencyId ?? new OrderSubtotalRange();
                    orderSubtotalRangesByCurrencyId.Minimum = minimumValue;
                    orderSubtotalRangesByCurrencyId.Maximum = maximumValue;
                }
                else
                {
                    OrderSubtotalRangeExtension.OrderSubtotalRangesByCurrencyID.Add(currencyID, new OrderSubtotalRange(minimumValue, maximumValue));
                }
            }
            else
            {
                RemoveOrderSubtotalRange(currencyID);
            }
        }

        public void RemoveOrderSubtotalRanges()
        {
            OrderSubtotalRangeExtension = null;
        }

        public void RemoveOrderSubtotalRange(int currencyID)
        {
            if (OrderSubtotalRangeExtension == null)
            {
                return;
            }
            else
            {
                OrderSubtotalRangeExtension.OrderSubtotalRangesByCurrencyID.Remove(currencyID);
                if (OrderSubtotalRangeExtension.OrderSubtotalRangesByCurrencyID.Count == 0)
                {
                    OrderSubtotalRangeExtension = null;
                }
            }
        }

        public IOrderSubtotalRange GetOrderSubtotalRange(int currencyID)
        {
            if (OrderSubtotalRangeExtension == null || !OrderSubtotalRangeExtension.OrderSubtotalRangesByCurrencyID.ContainsKey(currencyID))
            {
                return new OrderSubtotalRange(0, null);
            }
            else
            {
                return OrderSubtotalRangeExtension.OrderSubtotalRangesByCurrencyID[currencyID];
            }
        }

        public IOrderSubtotalRange GetDefaultOrderSubtotalRange()
        {
            if (OrderSubtotalRangeExtension == null || OrderSubtotalRangeExtension.OrderSubtotalRangesByCurrencyID.Count == 0)
            {
                return new OrderSubtotalRange(0, null);
            }
            else
            {
                return OrderSubtotalRangeExtension.OrderSubtotalRangesByCurrencyID[OrderSubtotalRangeExtension.OrderSubtotalRangesByCurrencyID.Keys.First()];
            }
        }

        #endregion

        #region Customer PriceTypeTotal Range

        public ICustomerPriceTypeTotalRangeQualificationExtension CustomerPriceTypeTotalRangeExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.CustomerPriceTypeTotalRange))
                {
                    return (ICustomerPriceTypeTotalRangeQualificationExtension)PromotionQualifications[QualificationNames.CustomerPriceTypeTotalRange];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.CustomerPriceTypeTotalRange);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.CustomerPriceTypeTotalRange, value);
                }
            }
        }

        public void SetCustomerPriceTypeTotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID, int priceTypeID)
        {
            if (minimumValue > 0)
            {
                if (CustomerPriceTypeTotalRangeExtension == null)
                {
                    CustomerPriceTypeTotalRangeExtension = Create.New<ICustomerPriceTypeTotalRangeQualificationExtension>();
                }

                PriceTypeTotalRangeProductPriceTypeID = priceTypeID;

                if (CustomerPriceTypeTotalRangeExtension.CustomerPriceTypeTotalRangesByCurrencyID.ContainsKey(currencyID))
                {
                    var customerPriceTypeTotalRangeByCurrencyId = CustomerPriceTypeTotalRangeExtension.CustomerPriceTypeTotalRangesByCurrencyID[currencyID];
                    customerPriceTypeTotalRangeByCurrencyId = customerPriceTypeTotalRangeByCurrencyId ?? new CustomerPriceTypeTotalRange();
                    customerPriceTypeTotalRangeByCurrencyId.Minimum = minimumValue;
                    customerPriceTypeTotalRangeByCurrencyId.Maximum = maximumValue;
                }
                else
                {
                    CustomerPriceTypeTotalRangeExtension.CustomerPriceTypeTotalRangesByCurrencyID.Add(currencyID, new CustomerPriceTypeTotalRange(minimumValue, maximumValue));
                }
            }
            else
            {
                RemoveCustomerPriceTypeTotalRange(currencyID);
            }
        }

        public void RemoveCustomerPriceTypeTotalRanges()
        {
            CustomerPriceTypeTotalRangeExtension = null;
        }

        public void RemoveCustomerPriceTypeTotalRange(int currencyID)
        {
            if (CustomerPriceTypeTotalRangeExtension == null)
            {
                return;
            }
            else
            {
                CustomerPriceTypeTotalRangeExtension.CustomerPriceTypeTotalRangesByCurrencyID.Remove(currencyID);
                if (CustomerPriceTypeTotalRangeExtension.CustomerPriceTypeTotalRangesByCurrencyID.Count == 0)
                {
                    CustomerPriceTypeTotalRangeExtension = null;
                }
            }
        }

        public ICustomerPriceTypeTotalRange GetCustomerPriceTypeTotalRange(int currencyID)
        {
            if (CustomerPriceTypeTotalRangeExtension == null || !CustomerPriceTypeTotalRangeExtension.CustomerPriceTypeTotalRangesByCurrencyID.ContainsKey(currencyID))
            {
                return new CustomerPriceTypeTotalRange(0, null);
            }
            else
            {
                return CustomerPriceTypeTotalRangeExtension.CustomerPriceTypeTotalRangesByCurrencyID[currencyID];
            }
        }

        public ICustomerPriceTypeTotalRange GetDefaultCustomerPriceTypeTotalRange()
        {
            if (CustomerPriceTypeTotalRangeExtension == null || CustomerPriceTypeTotalRangeExtension.CustomerPriceTypeTotalRangesByCurrencyID.Count == 0)
            {
                return new CustomerPriceTypeTotalRange(0, null);
            }
            else
            {
                return CustomerPriceTypeTotalRangeExtension.CustomerPriceTypeTotalRangesByCurrencyID[CustomerPriceTypeTotalRangeExtension.CustomerPriceTypeTotalRangesByCurrencyID.Keys.First()];
            }
        }

        #endregion

        #region Order PriceTypeTotal Range

        public IOrderPriceTypeTotalRangeQualificationExtension OrderPriceTypeTotalRangeExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.OrderPriceTypeTotalRange))
                {
                    return (IOrderPriceTypeTotalRangeQualificationExtension)PromotionQualifications[QualificationNames.OrderPriceTypeTotalRange];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.OrderPriceTypeTotalRange);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.OrderPriceTypeTotalRange, value);
                }
            }
        }

        public void SetOrderPriceTypeTotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID, int priceTypeID)
        {
            if (minimumValue > 0)
            {
                if (OrderPriceTypeTotalRangeExtension == null)
                {
                    OrderPriceTypeTotalRangeExtension = Create.New<IOrderPriceTypeTotalRangeQualificationExtension>();
                }

                PriceTypeTotalRangeProductPriceTypeID = priceTypeID;

                if (OrderPriceTypeTotalRangeExtension.OrderPriceTypeTotalRangesByCurrencyID.ContainsKey(currencyID))
                {
                    var orderPriceTypeTotalRangesByCurrencyId = OrderPriceTypeTotalRangeExtension.OrderPriceTypeTotalRangesByCurrencyID[currencyID];
                    orderPriceTypeTotalRangesByCurrencyId = orderPriceTypeTotalRangesByCurrencyId ?? new OrderPriceTypeTotalRange();
                    orderPriceTypeTotalRangesByCurrencyId.Minimum = minimumValue;
                    orderPriceTypeTotalRangesByCurrencyId.Maximum = maximumValue;
                }
                else
                {
                    OrderPriceTypeTotalRangeExtension.OrderPriceTypeTotalRangesByCurrencyID.Add(currencyID, new OrderPriceTypeTotalRange(minimumValue, maximumValue));
                }
            }
            else
            {
                RemoveOrderPriceTypeTotalRange(currencyID);
            }
        }

        public void RemoveOrderPriceTypeTotalRanges()
        {
            OrderPriceTypeTotalRangeExtension = null;
        }

        public void RemoveOrderPriceTypeTotalRange(int currencyID)
        {
            if (OrderPriceTypeTotalRangeExtension == null)
            {
                return;
            }
            else
            {
                OrderPriceTypeTotalRangeExtension.OrderPriceTypeTotalRangesByCurrencyID.Remove(currencyID);
                if (OrderPriceTypeTotalRangeExtension.OrderPriceTypeTotalRangesByCurrencyID.Count == 0)
                {
                    OrderPriceTypeTotalRangeExtension = null;
                }
            }
        }

        public IOrderPriceTypeTotalRange GetOrderPriceTypeTotalRange(int currencyID)
        {
            if (OrderPriceTypeTotalRangeExtension == null || !OrderPriceTypeTotalRangeExtension.OrderPriceTypeTotalRangesByCurrencyID.ContainsKey(currencyID))
            {
                return new OrderPriceTypeTotalRange(0, null);
            }
            else
            {
                return OrderPriceTypeTotalRangeExtension.OrderPriceTypeTotalRangesByCurrencyID[currencyID];
            }
        }

        public IOrderPriceTypeTotalRange GetDefaultOrderPriceTypeTotalRange()
        {
            if (OrderPriceTypeTotalRangeExtension == null || OrderPriceTypeTotalRangeExtension.OrderPriceTypeTotalRangesByCurrencyID.Count == 0)
            {
                return new OrderPriceTypeTotalRange(0, null);
            }
            else
            {
                return OrderPriceTypeTotalRangeExtension.OrderPriceTypeTotalRangesByCurrencyID[OrderPriceTypeTotalRangeExtension.OrderPriceTypeTotalRangesByCurrencyID.Keys.First()];
            }
        }

        #endregion

        #region Required Items

        public IEnumerable<IProductInOrderQualificationExtension> ProductInOrderExtensions
        {
            get
            {
                return PromotionQualifications.Values.OfType<IProductInOrderQualificationExtension>();
            }
        }

        public IEnumerable<IProductOption> RequiredProductOptions
        {
            get
            {
                return ProductInOrderExtensions.Select(extension =>
                {
                    var option = Create.New<IProductOption>();
                    option.ProductID = extension.ProductID;
                    option.Quantity = extension.Quantity;
                    return option;
                });
            }
        }

        public void AddRequiredProductOption(IProductOption option)
        {
            var extension = Create.New<IProductInOrderQualificationExtension>();
            extension.ProductID = option.ProductID;
            extension.Quantity = option.Quantity;
            PromotionQualifications.Add(String.Format("Required ProductID:{0}", option.ProductID), extension);
        }

        public void RemoveRequiredProductOption(int productID)
        {
            PromotionQualifications.Remove(String.Format("Required ProductID:{0}", productID));
        }

        public bool OnlyApplyToPartyHost
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.PartyHostPromotion))
                {
                    return true;
                }
                return false;
            }
            set
            {
                if (value)
                {
                    PromotionQualifications.Add(QualificationNames.PartyHostPromotion, Create.New<ICustomerIsHostQualificationExtension>());
                }
                else
                {
                    PromotionQualifications.Remove(QualificationNames.PartyHostPromotion);
                }
            }
        }

        #endregion

        #endregion
        #region Rewards

        #region Products Added As Reward

        ISimpleProductAdditionReward AddedItemReward
        {
            get
            {
                if (PromotionRewards.ContainsKey(RewardNames.RewardAddedProducts))
                {
                    return (ISimpleProductAdditionReward)PromotionRewards[RewardNames.RewardAddedProducts];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionRewards.Remove(RewardNames.RewardAddedProducts);
                }
                else
                {
                    PromotionRewards[RewardNames.RewardAddedProducts] = value;
                }
            }
        }

        public IEnumerable<IProductOption> ProductsAddedAsReward
        {
            get
            {
                if (AddedItemReward == null)
                {
                    return new IProductOption[0];
                }
                else
                {
                    return AddedItemReward.Products;
                }
            }
        }

        public void AddRewardProduct(IProductOption option)
        {
            if (AddedItemReward == null)
            {
                AddedItemReward = Create.New<ISimpleProductAdditionReward>();
            }
            AddedItemReward.AddProduct(option);
        }

        public void RemoveRewardProduct(int productID)
        {
            if (AddedItemReward == null)
            {
                return;
            }
            AddedItemReward.RemoveProduct(productID);
            if (AddedItemReward.Products.Count() == 0)
                AddedItemReward = null;
        }

        public int ProductSelectionsMaximumCount
        {
            get
            {
                if (SelectFromListReward == null)
                {
                    return 1;
                }
                return SelectFromListReward.AllowedSelectionQuantity;
            }
            set
            {
                if (SelectFromListReward == null)
                {
                    SelectFromListReward = Create.New<ISelectFreeItemsFromListReward>();
                }
                SelectFromListReward.AllowedSelectionQuantity = value;
            }
        }

        #endregion

        #region Product Selections As Reward

        private ISelectFreeItemsFromListReward SelectFromListReward
        {
            get
            {
                if (PromotionRewards.ContainsKey(RewardNames.RewardProductSelections))
                {
                    var reward = ((ISelectFreeItemsFromListReward)PromotionRewards[RewardNames.RewardProductSelections]);
                    return reward;
                }
                else
                    return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionRewards.Remove(RewardNames.RewardProductSelections);
                }
                else
                {
                    if (PromotionRewards.ContainsKey(RewardNames.RewardProductSelections))
                    {
                        PromotionRewards[RewardNames.RewardProductSelections] = value;
                    }
                    else
                    {
                        PromotionRewards.Add(RewardNames.RewardProductSelections, value);
                    }
                }
            }
        }

        public IEnumerable<IProductOption> ProductSelectionsAsReward
        {
            get
            {
                if (SelectFromListReward != null)
                {
                    return SelectFromListReward.SelectionOptions;
                }
                return new IProductOption[0];
            }
        }

        public void AddRewardProductSelection(IProductOption option)
        {
            if (SelectFromListReward == null)
            {
                SelectFromListReward = Create.New<ISelectFreeItemsFromListReward>();
            }
            var existing = SelectFromListReward.SelectionOptions.SingleOrDefault(x => x.ProductID == option.ProductID);
            if (existing != null)
                SelectFromListReward.RemoveSelectionOption(existing.ProductID);
            SelectFromListReward.AddSelectionOption(option);
        }

        public void RemoveRewardProductSelection(int productID)
        {
            if (SelectFromListReward == null)
                return;
            var existing = SelectFromListReward.SelectionOptions.SingleOrDefault(x => x.ProductID == productID);
            if (existing != null)
                SelectFromListReward.RemoveSelectionOption(existing.ProductID);
            if (SelectFromListReward.SelectionOptions.Count() == 0)
                SelectFromListReward = null;
        }

        #endregion

        #region Free Shipping

        public bool FreeShippingAsReward
        {
            get
            {
                return PromotionRewards.Keys.Contains(RewardNames.ReducedShipping);
            }
            set
            {
                if (value)
                {
                    if (!PromotionRewards.ContainsKey(RewardNames.ReducedShipping))
                    {
                        var reward = Create.New<IOrderShippingTotalReductionReward>();
                        reward.DefaultMarketID = MarketID;
                        reward.OrderAdjustmentOrderOperationID = (int)OrderAdjustmentOrderOperationKind.Multiplier;
                        reward.MarketDecimalOperands.Add(MarketID, 1);
                        PromotionRewards.Add(RewardNames.ReducedShipping, reward);
                    }
                }
                else
                {
                    PromotionRewards.Remove(RewardNames.ReducedShipping);
                }
            }
        }

        #endregion

        #region Reduced Subtotal


        public decimal ReduceSubtotalByPercent
        {
            get
            {
                if (PromotionRewards.Keys.Contains(RewardNames.ReducedSubtotal))
                {
                    return ((IOrderSubtotalReductionReward)PromotionRewards[RewardNames.ReducedSubtotal]).GetMarketDecimalOperand(MarketID);
                }
                else
                {
                    return 0M;
                }
            }
            set
            {
                if (value > 0)
                {
                    if (!PromotionRewards.ContainsKey(RewardNames.ReducedSubtotal))
                    {
                        var reward = Create.New<IOrderSubtotalReductionReward>();
                        reward.DefaultMarketID = MarketID;
                        reward.SetMarketDecimalOperand(MarketID, value);
                        PromotionRewards.Add(RewardNames.ReducedSubtotal, reward);
                    }
                    else
                    {
                        var reward = (IOrderSubtotalReductionReward)PromotionRewards[RewardNames.ReducedSubtotal];
                        reward.SetMarketDecimalOperand(MarketID, value);
                    }
                }
                else
                {
                    PromotionRewards.Remove(RewardNames.ReducedSubtotal);
                }
            }
        }

        #endregion

        #endregion

        private int priceTypeTotalRangeProductPriceTypeID;

        public int PriceTypeTotalRangeProductPriceTypeID
        {
            get
            {
                if (CustomerPriceTypeTotalRangeExtension != null)
                {
                    return CustomerPriceTypeTotalRangeExtension.ProductPriceTypeID;
                }
                if (OrderPriceTypeTotalRangeExtension != null)
                {
                    return OrderPriceTypeTotalRangeExtension.ProductPriceTypeID;
                }
                else
                {
                    return priceTypeTotalRangeProductPriceTypeID;
                }
            }
            set
            {
                if (CustomerPriceTypeTotalRangeExtension != null)
                {
                    CustomerPriceTypeTotalRangeExtension.ProductPriceTypeID = value;
                }
                if (OrderPriceTypeTotalRangeExtension != null)
                {
                    OrderPriceTypeTotalRangeExtension.ProductPriceTypeID = value;
                }
                else
                {
                    priceTypeTotalRangeProductPriceTypeID = value;
                }
            }
        }
        
    }
}
