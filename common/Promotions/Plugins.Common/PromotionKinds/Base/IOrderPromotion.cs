using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using System.Diagnostics.Contracts;

namespace NetSteps.Promotions.Plugins.Common.PromotionKinds.Base
{
    [ContractClass(typeof(OrderPromotionContract))]
    public interface IOrderPromotion : IPromotion, IStandardQualificationPromotion
    {

    }

    [ContractClassFor(typeof(IOrderPromotion))]
    public abstract class OrderPromotionContract : IOrderPromotion
    {

        public abstract bool RequiresPromotionCode { get; }

        public abstract string PromotionCode { get; set; }

        public abstract bool Continuity { get; set; } //INI-FIN - GR_Encore-07

        public abstract bool OneTimeUse { get; set; }

        public abstract bool FirstOrdersOnly { get; set; }

        public abstract bool RestrictedToAccountTitlesOrTypes { get; }

        public abstract IEnumerable<IAccountTitleOption> AccountTitles { get; }

        public abstract IEnumerable<short> AccountTypeIDs { get; }

        public abstract bool RestrictedToOrderTypes { get; }

        public abstract IEnumerable<int> OrderTypes { get; }

        public abstract void AddAccountTitle(int accountTitleID, int titleTypeID);

        public abstract void DeleteAccountTitle(int accountTitleID, int titleTypeID);

        public abstract void AddOrderTypeID(int orderTypeID);

        public abstract void DeleteOrderTypeID(int orderTypeID);

        public abstract void AddAccountTypeID(short accountTypeID);

        public abstract void DeleteAccountTypeID(short accountTypeID);

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
