using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.PromotionKinds.Base
{
    [ContractClass(typeof(SingleMarketStandardQualificationPromotionContract))]
    public interface ISingleMarketStandardQualificationPromotion : IStandardQualificationPromotion
    {
        /// <summary>
        /// Gets or sets the market ID.
        /// </summary>
        /// <value>The market ID.</value>
        int MarketID { get; set; }
    }

    [ContractClassFor(typeof(ISingleMarketStandardQualificationPromotion))]
    public abstract class SingleMarketStandardQualificationPromotionContract : ISingleMarketStandardQualificationPromotion
    {
        public int MarketID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(value > 0);
                throw new NotImplementedException();
            }
        }

        public abstract IEnumerable<int> PromotedProductIDs { get; }

        public abstract IEnumerable<int> GetPromotedPriceTypesForProductID(int productID);

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

        public abstract IEnumerable<int> AccountIDs { get; }

        public abstract void AddAccountID(int accountID);

        public abstract void DeleteAccountID(int accountID);

    }
}
