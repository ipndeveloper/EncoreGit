using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Base;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Components;
using System.Diagnostics.Contracts;
using NetSteps.Promotions.Plugins.Common.Helpers;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.PromotionKinds
{
    [ContractClass(typeof(OrderPromotionDefaultCartRewardContract))]
    public interface IOrderPromotionDefaultCartRewards : ISingleMarketStandardQualificationPromotion
    {
        void SetCustomerSubtotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID);

        void RemoveCustomerSubtotalRanges();

        void RemoveCustomerSubtotalRange(int currencyID);

        ICustomerSubtotalRange GetCustomerSubtotalRange(int currencyID);

        ICustomerSubtotalRange GetDefaultCustomerSubtotalRange();

        void SetCustomerPriceTypeTotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID, int priceTypeID);

        void RemoveCustomerPriceTypeTotalRanges();

        void RemoveCustomerPriceTypeTotalRange(int currencyID);

        ICustomerPriceTypeTotalRange GetCustomerPriceTypeTotalRange(int currencyID);

        ICustomerPriceTypeTotalRange GetDefaultCustomerPriceTypeTotalRange();

        void SetOrderSubtotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID);

        void RemoveOrderSubtotalRanges();

        void RemoveOrderSubtotalRange(int currencyID);

        IOrderSubtotalRange GetOrderSubtotalRange(int currencyID);

        IOrderSubtotalRange GetDefaultOrderSubtotalRange();

        void SetOrderPriceTypeTotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID, int priceTypeID);

        void RemoveOrderPriceTypeTotalRanges();

        void RemoveOrderPriceTypeTotalRange(int currencyID);

        IOrderPriceTypeTotalRange GetOrderPriceTypeTotalRange(int currencyID);

        IOrderPriceTypeTotalRange GetDefaultOrderPriceTypeTotalRange();

        bool OnlyApplyToPartyHost { get; set; }

        IEnumerable<IProductOption> RequiredProductOptions { get; }

        IEnumerable<IProductOption> ProductsAddedAsReward { get; }

        IEnumerable<IProductOption> ProductSelectionsAsReward { get; }

        int ProductSelectionsMaximumCount { get; set; }

        bool FreeShippingAsReward { get; set; }

        decimal ReduceSubtotalByPercent { get; set; }

        void AddRequiredProductOption(IProductOption option);

        void RemoveRequiredProductOption(int productID);

        void AddRewardProduct(IProductOption option);

        void RemoveRewardProduct(int productID);

        void AddRewardProductSelection(IProductOption option);

        void RemoveRewardProductSelection(int productID);

        int PriceTypeTotalRangeProductPriceTypeID { get; set; }

    }

    [ContractClassFor(typeof(IOrderPromotionDefaultCartRewards))]
    public abstract class OrderPromotionDefaultCartRewardContract : IOrderPromotionDefaultCartRewards
    {
        public IEnumerable<IProductOption> RequiredProductOptions
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<IProductOption>>() != null);
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IProductOption> ProductsAddedAsReward
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<IProductOption>>() != null);
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IProductOption> ProductSelectionsAsReward
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<IProductOption>>() != null);
                throw new NotImplementedException();
            }
        }

        public bool FreeShippingAsReward
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public decimal ReduceSubtotalByPercent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void AddRequiredProductOption(IProductOption option)
        {
            Contract.Requires<ArgumentNullException>(option != null);
            throw new NotImplementedException();
        }

        public void RemoveRequiredProductOption(int productID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(productID <= 0);
            throw new NotImplementedException();
        }

        public void AddRewardProduct(IProductOption option)
        {
            Contract.Requires<ArgumentNullException>(option != null);
            throw new NotImplementedException();
        }

        public void RemoveRewardProduct(int productID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(productID >= 0);
            throw new NotImplementedException();
        }

        public void AddRewardProductSelection(IProductOption option)
        {
            Contract.Requires<ArgumentNullException>(option != null);
            throw new NotImplementedException();
        }

        public void RemoveRewardProductSelection(int productID)
        {
            throw new NotImplementedException();
        }

        public abstract bool RequiresPromotionCode { get; set; }

        public abstract string PromotionCode { get; set; }

        public abstract bool Continuity { get; set; } //INI - GR_Encore-07

        public abstract bool OneTimeUse { get; set; }

        public abstract bool FirstOrdersOnly { get; set; }

        public abstract bool RestrictedToAccountTitlesOrTypes { get; }

        public abstract IEnumerable<Qualifications.IAccountTitleOption> AccountTitles { get; }

        public abstract IEnumerable<short> AccountTypeIDs { get; }

        public abstract bool RestrictedToOrderTypes { get; }

        public abstract IEnumerable<int> OrderTypes { get; }

        public abstract void AddAccountTitle(int accountTitleID, int titleTypeID);

        public abstract void DeleteAccountTitle(int accountTitleID, int titleTypeID);

        public abstract void AddOrderTypeID(int orderTypeID);

        public abstract void DeleteOrderTypeID(int orderTypeID);

        public abstract void AddAccountTypeID(short accountTypeID);

        public abstract void DeleteAccountTypeID(short accountTypeID);

        //INI - GR_Encore-07
        public abstract IEnumerable<short> AccountConsistencyStatusIDs { get; }

        public abstract void AddAccountConsistencyStatusID(short accountConsistencyStatusID);

        public abstract void DeleteAccountConsistencyStatusID(short accountConsistencyStatusID);

        public abstract IEnumerable<short> ActivityStatusIDs { get; }

        public abstract void AddActivityStatusID(short activityStatusID);

        public abstract void DeleteActivityStatusID(short activityStatusID);
        //INI - GR_Encore-07

        public abstract IEnumerable<string> AssociatedPropertyNames { get; }

        public abstract string Description { get; set; }

        public abstract DateTime? EndDate { get; set; }

        public abstract int PromotionID { get; set; }

        public abstract string PromotionKind { get; }

        public abstract IDictionary<string, Promotions.Common.Model.IPromotionQualificationExtension> PromotionQualifications { get; set; }

        public abstract IDictionary<string, Promotions.Common.Model.IPromotionReward> PromotionRewards { get; set; }

        public abstract int PromotionStatusTypeID { get; set; }

        public abstract DateTime? StartDate { get; set; }

        public abstract bool ValidFor<TProperty>(string propertyName, TProperty value);

        public abstract IEnumerable<string> ConstructionErrors { get; }

        public void RemoveCustomerSubtotalRanges()
        {
            throw new NotImplementedException();
        }

        public void RemoveCustomerSubtotalRange(int currencyID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(currencyID > 0);
            throw new NotImplementedException();
        }


        public decimal GetSubtotalRange(int currencyID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(currencyID > 0);
            throw new NotImplementedException();
        }

        public decimal GetDefaultSubtotalRange()
        {
            throw new NotImplementedException();
        }

        public abstract int MarketID { get; set; }


        public int ProductSelectionsMaximumCount
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(value > 0, "Maximum product selections count cannot be 0.");
                throw new NotImplementedException();
            }
        }


        ICustomerSubtotalRange IOrderPromotionDefaultCartRewards.GetCustomerSubtotalRange(int currencyID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(currencyID > 0);
            throw new NotImplementedException();
        }

        ICustomerSubtotalRange IOrderPromotionDefaultCartRewards.GetDefaultCustomerSubtotalRange()
        {
            throw new NotImplementedException();
        }

        public void SetOrderSubtotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(!minimumValue.HasValue || minimumValue.Value > 0);
            Contract.Requires<ArgumentOutOfRangeException>(!maximumValue.HasValue || maximumValue.Value > 0);
            Contract.Requires<ArgumentOutOfRangeException>(currencyID > 0);
            throw new NotImplementedException();
        }

        public void RemoveOrderSubtotalRanges()
        {
            throw new NotImplementedException();
        }

        public void RemoveOrderSubtotalRange(int currencyID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(currencyID > 0);
            throw new NotImplementedException();
        }

        public IOrderSubtotalRange GetOrderSubtotalRange(int currencyID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(currencyID > 0);
            throw new NotImplementedException();
        }

        public IOrderSubtotalRange GetDefaultOrderSubtotalRange()
        {
            throw new NotImplementedException();
        }

        public void SetCustomerSubtotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(!minimumValue.HasValue || minimumValue.Value > 0);
            Contract.Requires<ArgumentOutOfRangeException>(!maximumValue.HasValue || maximumValue.Value > 0);
            throw new NotImplementedException();
        }


        public bool OnlyApplyToPartyHost
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void SetCustomerPriceTypeTotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID, int priceTypeID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(!minimumValue.HasValue || minimumValue.Value > 0);
            Contract.Requires<ArgumentOutOfRangeException>(!maximumValue.HasValue || maximumValue.Value > 0);
            Contract.Requires<ArgumentOutOfRangeException>(currencyID > 0);
            Contract.Requires<ArgumentOutOfRangeException>(priceTypeID >= 0);
            throw new NotImplementedException();
        }

        public void RemoveCustomerPriceTypeTotalRanges()
        {
            throw new NotImplementedException();
        }

        public void RemoveCustomerPriceTypeTotalRange(int currencyID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(currencyID > 0);
            throw new NotImplementedException();
        }

        public ICustomerPriceTypeTotalRange GetCustomerPriceTypeTotalRange(int currencyID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(currencyID > 0);
            throw new NotImplementedException();
        }

        public ICustomerPriceTypeTotalRange GetDefaultCustomerPriceTypeTotalRange()
        {
            throw new NotImplementedException();
        }

        public void SetOrderPriceTypeTotalRange(decimal? minimumValue, decimal? maximumValue, int currencyID, int priceTypeID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(!minimumValue.HasValue || minimumValue.Value > 0);
            Contract.Requires<ArgumentOutOfRangeException>(!maximumValue.HasValue || maximumValue.Value > 0);
            Contract.Requires<ArgumentOutOfRangeException>(currencyID > 0);
            Contract.Requires<ArgumentOutOfRangeException>(priceTypeID >= 0);
            throw new NotImplementedException();
        }

        public void RemoveOrderPriceTypeTotalRanges()
        {
            throw new NotImplementedException();
        }

        public void RemoveOrderPriceTypeTotalRange(int currencyID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(currencyID > 0);
            throw new NotImplementedException();
        }

        public IOrderPriceTypeTotalRange GetOrderPriceTypeTotalRange(int currencyID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(currencyID > 0);
            throw new NotImplementedException();
        }

        public IOrderPriceTypeTotalRange GetDefaultOrderPriceTypeTotalRange()
        {
            throw new NotImplementedException();
        }


        public int PriceTypeTotalRangeProductPriceTypeID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public IEnumerable<int> AccountIDs
        {
            get { throw new NotImplementedException(); }
        }

        public void AddAccountID(int accountID)
        {
            throw new NotImplementedException();
        }

        public void DeleteAccountID(int accountID)
        {
            throw new NotImplementedException();
        }
    }

}
