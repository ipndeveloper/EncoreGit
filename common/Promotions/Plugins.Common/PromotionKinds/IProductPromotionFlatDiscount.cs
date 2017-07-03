using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Components;
using System.Diagnostics.Contracts;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Base;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Data.Common.Entities;

namespace NetSteps.Promotions.Plugins.Common.PromotionKinds
{
    [ContractClass(typeof(ProductPromotionFlatDiscountContract))]
    public interface IProductPromotionFlatDiscount : IProductPromotion, ISingleMarketStandardQualificationPromotion, IOrderPromotionDefaultCartRewards
    {
        /// <summary>
        /// Gets the adjusted value for a product based upon its current value, market ID and price type.
        /// </summary>
        /// <param name="productID">The product ID.</param>
        /// <param name="initialAmount">The initial pre-adjustment amount.</param>
        /// <param name="priceType">The price type.</param>
        /// <returns></returns>
        decimal GetAdjustedValue(int productID, decimal initialAmount, IPriceType priceType);

        void AddProductAdjustment(int productID, IPriceType priceType, decimal flatAmount);

        void DeleteProductAdjustment(int productID, IPriceType priceType);

        void DeleteProductAdjustments(int productID);

        decimal GetAdjustmentAmount(int productID, IPriceType priceType);
    }

    [ContractClassFor(typeof(IProductPromotionFlatDiscount))]
    public abstract class ProductPromotionFlatDiscountContract : IProductPromotionFlatDiscount
    {

        public decimal GetAdjustedValue(int productID, decimal initialAmount, IPriceType priceType)
        {
            Contract.Requires<ArgumentException>(productID > 0, "Product ID must be greater than 0.");
            return 0;
        }

        public void AddProductAdjustment(int productID, IPriceType priceType, decimal flatAmount)
        {
            Contract.Requires<ArgumentException>(productID > 0, "Product ID must be greater than 0.");
        }

        public void DeleteProductAdjustment(int productID, IPriceType priceType)
        {
            Contract.Requires<ArgumentException>(productID > 0, "Product ID must be greater than 0.");
        }

        public void DeleteProductAdjustments(int productID)
        {
            Contract.Requires<ArgumentException>(productID > 0, "Product ID must be greater than 0.");
        }


        public decimal GetAdjustmentAmount(int productID, IPriceType priceType)
        {
            Contract.Requires<ArgumentException>(productID > 0, "Product ID must be greater than 0.");
            return 0M;
        }

        public decimal GetAdjustedValue(int productID, decimal initialAmount, int marketID, IPriceType priceType)
        {
            Contract.Requires<ArgumentException>(productID > 0, "Product ID must be greater than 0.");
            Contract.Requires<ArgumentException>(marketID > 0, "Market ID must be greater than 0.");
            return 0;
        }

        public abstract int MarketID { get; set; }

        public abstract IEnumerable<int> PromotedProductIDs { get; }

        public abstract IEnumerable<IPriceType> GetPromotedPriceTypesForProductID(int productID);

        public abstract bool RequiresPromotionCode { get; }

        public abstract string PromotionCode { get; set; }

        public abstract bool Continuity { get; set; } //INI-FIN - GR_Encore-07

        public abstract bool OneTimeUse { get; set; }

        public abstract bool FirstOrdersOnly { get; set; }

        public abstract bool RestrictedToAccountTitles { get; }

        public abstract IEnumerable<IAccountTitleOption> AccountTitles { get; }

        public abstract bool RestrictedToOrderTypes { get; }

        public abstract IEnumerable<int> OrderTypes { get; }

        public abstract void AddAccountTitle(int accountTitleID, int titleTypeID);

        public abstract void DeleteAccountTitle(int accountTitleID, int titleTypeID);

        public abstract IEnumerable<string> AssociatedPropertyNames { get; }

        public abstract string Description { get; set; }

        public abstract DateTime? EndDate { get; set; }

        public abstract int PromotionID { get; set; }

        public abstract string PromotionKind { get; }

        public abstract IDictionary<string, IPromotionQualificationExtension> PromotionQualifications { get; set; }

        public abstract IDictionary<string, IPromotionReward> PromotionRewards { get; set; }

        public abstract int PromotionStatusTypeID { get; set; }

        public abstract DateTime? StartDate { get; set; }

        public abstract bool ValidFor<TProperty>(string propertyName, TProperty value);

        public abstract bool ValidateConstruction();

        public abstract void AddOrderTypeID(int orderTypeID);

        public abstract void DeleteOrderTypeID(int orderTypeID);

        public abstract bool RestrictedToAccountTitlesOrTypes { get; }

        public abstract IEnumerable<short> AccountTypeIDs { get; }

        public abstract void AddAccountTypeID(short accountTypeID);

        public abstract void DeleteAccountTypeID(short accountTypeID);

        //INI - GR_Encore-07
        public abstract IEnumerable<short> AccountConsistencyStatusIDs { get; }

        public abstract void AddAccountConsistencyStatusID(short accountConsistencyStatusID);

        public abstract void DeleteAccountConsistencyStatusID(short accountConsistencyStatusID);

        public abstract IEnumerable<short> ActivityStatusIDs { get; }

        public abstract void AddActivityStatusID(short activityStatusID);

        public abstract void DeleteActivityStatusID(short activityStatusID);
        //FIN - GR_Encore-07

        public abstract decimal GetPromotionAdjustmentAmount(int productID, int marketID, decimal originalPrice, IPriceType productPriceType);

        public abstract IEnumerable<string> ConstructionErrors { get; }


        public abstract IEnumerable<int> AccountIDs
        {
            get;
        }

        public abstract void AddAccountID(int accountID);

        public abstract void DeleteAccountID(int accountID);


        public abstract void SetCustomerSubtotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID);

        public abstract void RemoveCustomerSubtotalRanges();

        public abstract void RemoveCustomerSubtotalRange(int currencyID);

        public abstract ICustomerSubtotalRange GetCustomerSubtotalRange(int currencyID);

        public abstract ICustomerSubtotalRange GetDefaultCustomerSubtotalRange();

        public abstract void SetCustomerPriceTypeTotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID, int priceTypeID);

        public abstract void RemoveCustomerPriceTypeTotalRanges();

        public abstract void RemoveCustomerPriceTypeTotalRange(int currencyID);

        public abstract ICustomerPriceTypeTotalRange GetCustomerPriceTypeTotalRange(int currencyID);

        public abstract ICustomerPriceTypeTotalRange GetDefaultCustomerPriceTypeTotalRange();

        public abstract void SetOrderSubtotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID);
        public abstract void RemoveOrderSubtotalRanges();

        public abstract void RemoveOrderSubtotalRange(int currencyID);

        public abstract IOrderSubtotalRange GetOrderSubtotalRange(int currencyID);

        public abstract IOrderSubtotalRange GetDefaultOrderSubtotalRange();

        public abstract void SetOrderPriceTypeTotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID, int priceTypeID);


        public abstract void RemoveOrderPriceTypeTotalRanges();

        public abstract void RemoveOrderPriceTypeTotalRange(int currencyID);

        public abstract IOrderPriceTypeTotalRange GetOrderPriceTypeTotalRange(int currencyID);

        public abstract IOrderPriceTypeTotalRange GetDefaultOrderPriceTypeTotalRange();

        public abstract bool OnlyApplyToPartyHost { get; set; }

        public abstract IEnumerable<Helpers.IProductOption> RequiredProductOptions { get; }

        public abstract IEnumerable<Helpers.IProductOption> ProductsAddedAsReward { get; }

        public abstract IEnumerable<Helpers.IProductOption> ProductSelectionsAsReward { get; }

        public abstract int ProductSelectionsMaximumCount { get; set; }

        public abstract bool FreeShippingAsReward { get; set; }

        public abstract decimal ReduceSubtotalByPercent { get; set; }

        public abstract void AddRequiredProductOption(Helpers.IProductOption option);

        public abstract void RemoveRequiredProductOption(int productID);

        public abstract void AddRewardProduct(Helpers.IProductOption option);

        public abstract void RemoveRewardProduct(int productID);
        public abstract void AddRewardProductSelection(Helpers.IProductOption option);

        public abstract void RemoveRewardProductSelection(int productID);

        public abstract int PriceTypeTotalRangeProductPriceTypeID { get; set; }
    }
}
