namespace NetSteps.Data.Entities.Cache
{
    using System;
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Data.Entities.Business.Logic;
    using System.Linq;

    /// <summary>
    /// Small Collection Cache
    /// </summary>
    public partial class SmallCollectionCache
    {
        #region Customs Added in BelCorp Peru

        /// <summary>
        /// Bonus Types cached
        /// </summary>
        public BonusTypeCache BonusTypes = new BonusTypeCache();
        public sealed class BonusTypeCache : SmallCollectionBase<BonusType, Int32>
        {
            public BonusTypeCache()
                : base("BonusTypeCache")
            { }

            protected override Int32 PerformGetKey(BonusType item)
            {
                return item.BonusTypeId;
            }

            protected override List<BonusType> PerformInitializeList()
            {
                return BonusTypeLogic.Instance.GetAllByCommission();
            }
        }

        /// <summary>
        /// Plans Cached
        /// </summary>
        public PlanCache Plans = new PlanCache();
        public sealed class PlanCache : SmallCollectionBase<Plan, Int32>
        {
            public PlanCache()
                : base("PlanCache")
            { }

            protected override Int32 PerformGetKey(Plan item)
            {
                return item.PlanId;
            }

            protected override List<Plan> PerformInitializeList()
            {
                return PlanLogic.Instance.GetAll();
            }
        }

        //INI - GR_Encore-07
        public AccountConsistencyStatusCache AccountConsistencyStatuses = new AccountConsistencyStatusCache();
        public sealed class AccountConsistencyStatusCache : SmallCollectionBase<AccountConsistencyStatus, Int32>
        {
            public AccountConsistencyStatusCache()
                : base("AccountConsistencyStatusCache")
            { }

            protected override Int32 PerformGetKey(AccountConsistencyStatus item)
            {
                return item.AccountConsistencyStatusID;
            }

            protected override List<AccountConsistencyStatus> PerformInitializeList()
            {
                return AccountConsistencyStatus.GetAll();
            }
        }

        public ActivityStatusCache ActivityStatuses = new ActivityStatusCache();
        public sealed class ActivityStatusCache : SmallCollectionBase<ActivityStatus, Int32>
        {
            public ActivityStatusCache()
                : base("ActivityStatusCache")
            { }

            protected override Int32 PerformGetKey(ActivityStatus item)
            {
                return item.ActivityStatusId;
            }

            protected override List<ActivityStatus> PerformInitializeList()
            {
                var result = ActivityStatus.GetAll();
                return result.ToList();
            }
        }
        //FIN - GR_Encore-07
        #endregion
    }
}
