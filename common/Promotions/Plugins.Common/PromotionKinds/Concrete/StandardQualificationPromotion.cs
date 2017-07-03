using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Base;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Promotions.Plugins.Common.PromotionKinds.Concrete
{
    [Serializable]
    public abstract class StandardQualificationPromotion : BasePromotion, IStandardQualificationPromotion
    {

        #region Qualifications

        public static class QualificationNames
        {
            public const string PromotionCode = "Promotion Code";
            public const string AccountTitleList = "Account Title List";
            public const string AccountTypeList = "Account Type List";
            public const string MarketList = "Market List";
            public const string OrderTypeList = "Order Type List";
            public const string OneTimeUse = "One Time Use";
            public const string AccountIDs = "Account IDs";
            //INI - GR_Encore-07
            public const string Continuity = "Continuity";
            public const string AccountConsistencyStatusList = "Account Consistency Status List";
            public const string ActivityStatusList = "Activity Status List";
            //FIN - GR_Encore-07
        }

        #region Promotion Code

        public bool RequiresPromotionCode
        {
            get { return !String.IsNullOrEmpty(PromotionCode); }
        }

        public string PromotionCode
        {
            get
            {
                if (PromotionCodeExtension == null)
                {
                    return null;
                }
                return PromotionCodeExtension.PromotionCode;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    PromotionCodeExtension = null;
                }
                else
                {
                    if (PromotionCodeExtension == null)
                    {
                        PromotionCodeExtension = Create.New<IPromotionCodeQualificationExtension>();
                    }
                    PromotionCodeExtension.PromotionCode = value;
                }
            }
        }

        protected IPromotionCodeQualificationExtension PromotionCodeExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.PromotionCode))
                {
                    return (IPromotionCodeQualificationExtension)PromotionQualifications[QualificationNames.PromotionCode];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.PromotionCode);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.PromotionCode, value);
                }
            }
        }

        #endregion

        #region Continuity
        //INI - GR_Encore-07
        public bool Continuity
        {
            get
            {
                if (ContinuityExtension == null)
                {
                    return false;
                }
                return ContinuityExtension.HasContinuity;
            }
            set
            {
                if (value == false)
                {
                    ContinuityExtension = null;
                }
                else
                {
                    if (ContinuityExtension == null)
                    {
                        ContinuityExtension = Create.New<IContinuityQualificationExtension>();
                    }
                    ContinuityExtension.HasContinuity = value;
                }
            }
        }

        protected IContinuityQualificationExtension ContinuityExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.Continuity))
                {
                    return (IContinuityQualificationExtension)PromotionQualifications[QualificationNames.Continuity];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.Continuity);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.Continuity, value);
                }
            }
        }
        //FIN - GR_Encore-07
        #endregion

        #region One Time Use

        public bool OneTimeUse
        {
            get
            {
                return (OneTimeUseExtension != null);
            }
            set
            {
                if (value)
                {
                    if (!OneTimeUse)
                    {
                        OneTimeUseExtension = Create.New<IUseCountQualificationExtension>();
                        OneTimeUseExtension.MaximumUseCount = 1;
                    }
                }
                else
                {
                    if (OneTimeUse)
                    {
                        OneTimeUseExtension = null;
                    }
                }
            }
        }

        protected IUseCountQualificationExtension OneTimeUseExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.OneTimeUse))
                {
                    return (IUseCountQualificationExtension)PromotionQualifications[QualificationNames.OneTimeUse];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.OneTimeUse);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.OneTimeUse, value);
                }
            }
        }

        public bool FirstOrdersOnly
        {
            get
            {
                return (OneTimeUseExtension != null && OneTimeUseExtension.FirstOrdersOnly);
            }
            set
            {
                if (value)
                {
                    if (OneTimeUseExtension == null)
                    {
                        // if it doesn't exist, create it
                        OneTimeUseExtension = Create.New<IUseCountQualificationExtension>();
                        OneTimeUseExtension.MaximumUseCount = 1;
                        OneTimeUseExtension.FirstOrdersOnly = true;
                    }
                    else
                    {
                        // it already exists, set first orders only to true
                        OneTimeUseExtension.FirstOrdersOnly = true;
                    }
                }
                else
                {
                    if (OneTimeUseExtension != null)
                    {
                        // set first orders only to false
                        OneTimeUseExtension.FirstOrdersOnly = false;
                    }
                }
            }
        }

        #endregion

        public bool RestrictedToAccountTitlesOrTypes
        {
            get
            {
                return AccountTitleListExtension != null || AccountTypeListExtension != null;
            }
        }

        #region Account Titles List

        protected IAccountHasTitleQualificationExtension AccountTitleListExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.AccountTitleList))
                {
                    return (IAccountHasTitleQualificationExtension)PromotionQualifications[QualificationNames.AccountTitleList];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.AccountTitleList);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.AccountTitleList, value);
                }
            }
        }

        public void DeleteAccountTitle(int accountTitleID, int titleTypeID)
        {
            if (AccountTitleListExtension != null)
            {
                var existing = AccountTitleListExtension.AllowedTitles.SingleOrDefault(title => title.TitleID == accountTitleID && title.TitleTypeID == titleTypeID);
                if (existing != null)
                {
                    AccountTitleListExtension.AllowedTitles.Remove(existing);
                }
                if (AccountTitleListExtension.AllowedTitles.Count == 0)
                {
                    AccountTitleListExtension = null;
                }
            }
        }


        public IEnumerable<IAccountTitleOption> AccountTitles
        {
            get
            {
                if (AccountTitleListExtension == null)
                {
                    return new IAccountTitleOption[] { };
                }
                else
                {
                    return AccountTitleListExtension.AllowedTitles;
                }
            }
        }

        public void AddAccountTitle(int accountTitleID, int titleTypeID)
        {
            if (AccountTitleListExtension == null)
            {
                AccountTitleListExtension = Create.New<IAccountHasTitleQualificationExtension>();
            }
            var newTitleOption = Create.New<IAccountTitleOption>();
            newTitleOption.TitleID = accountTitleID;
            newTitleOption.TitleTypeID = titleTypeID;
            AccountTitleListExtension.AllowedTitles.Add(newTitleOption);
        }

        #endregion

        #region Account Types List

        protected IAccountTypeQualificationExtension AccountTypeListExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.AccountTypeList))
                {
                    return (IAccountTypeQualificationExtension)PromotionQualifications[QualificationNames.AccountTypeList];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.AccountTypeList);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.AccountTypeList, value);
                }
            }
        }

        public void DeleteAccountTypeID(short accountTypeID)
        {
            if (AccountTypeListExtension != null)
            {
                AccountTypeListExtension.AccountTypes.Remove(accountTypeID);
                if (AccountTypeListExtension.AccountTypes.Count == 0)
                {
                    AccountTypeListExtension = null;
                }
            }
        }

        public IEnumerable<short> AccountTypeIDs
        {
            get
            {
                if (AccountTypeListExtension == null)
                {
                    return new short[] { };
                }
                else
                {
                    return AccountTypeListExtension.AccountTypes;
                }
            }
        }

        public void AddAccountTypeID(short accountTypeID)
        {
            if (AccountTypeListExtension == null)
            {
                AccountTypeListExtension = Create.New<IAccountTypeQualificationExtension>();
            }
            AccountTypeListExtension.AccountTypes.Add(accountTypeID);
        }

        #endregion

        #region Order Type List


        protected IOrderTypeQualificationExtension OrderTypeListExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.OrderTypeList))
                {
                    return (IOrderTypeQualificationExtension)PromotionQualifications[QualificationNames.OrderTypeList];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.OrderTypeList);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.OrderTypeList, value);
                }
            }
        }

        public void DeleteOrderTypeID(int orderTypeID)
        {
            if (OrderTypeListExtension != null)
            {
                OrderTypeListExtension.OrderTypes.Remove(orderTypeID);
                if (OrderTypeListExtension.OrderTypes.Count == 0)
                {
                    OrderTypeListExtension = null;
                }
            }
        }

        public IEnumerable<int> OrderTypes
        {
            get
            {
                if (OrderTypeListExtension == null)
                {
                    return new int[] { };
                }
                else
                {
                    return OrderTypeListExtension.OrderTypes;
                }
            }
        }

        public void AddOrderTypeID(int OrderTypeID)
        {
            if (OrderTypeListExtension == null)
            {
                OrderTypeListExtension = Create.New<IOrderTypeQualificationExtension>();
            }
            OrderTypeListExtension.OrderTypes.Add(OrderTypeID);
        }

        public bool RestrictedToOrderTypes
        {
            get
            {
                return OrderTypeListExtension != null;
            }
        }

        #endregion

        #region Account Consistency Status List
        //INI - GR_Encore-07
        protected IAccountConsistencyStatusQualificationExtension AccountConsistencyStatusListExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.AccountConsistencyStatusList))
                {
                    return (IAccountConsistencyStatusQualificationExtension)PromotionQualifications[QualificationNames.AccountConsistencyStatusList];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.AccountConsistencyStatusList);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.AccountConsistencyStatusList, value);
                }
            }
        }

        public void DeleteAccountConsistencyStatusID(short accountConsistencyStatusID)
        {
            if (AccountConsistencyStatusListExtension != null)
            {
                AccountConsistencyStatusListExtension.AccountConsistencyStatuses.Remove(accountConsistencyStatusID);
                if (AccountConsistencyStatusListExtension.AccountConsistencyStatuses.Count == 0)
                {
                    AccountConsistencyStatusListExtension = null;
                }
            }
        }

        public IEnumerable<short> AccountConsistencyStatusIDs
        {
            get
            {
                if (AccountConsistencyStatusListExtension == null)
                {
                    return new short[] { };
                }
                else
                {
                    return AccountConsistencyStatusListExtension.AccountConsistencyStatuses;
                }
            }
        }

        public void AddAccountConsistencyStatusID(short accountConsistencyStatusID)
        {
            if (AccountConsistencyStatusListExtension == null)
            {
                AccountConsistencyStatusListExtension = Create.New<IAccountConsistencyStatusQualificationExtension>();
            }
            AccountConsistencyStatusListExtension.AccountConsistencyStatuses.Add(accountConsistencyStatusID);
        }

        #endregion

        #region Activity Status List

        protected IActivityStatusQualificationExtension ActivityStatusListExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.ActivityStatusList))
                {
                    return (IActivityStatusQualificationExtension)PromotionQualifications[QualificationNames.ActivityStatusList];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.ActivityStatusList);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.ActivityStatusList, value);
                }
            }
        }

        public void DeleteActivityStatusID(short activityStatusID)
        {
            if (ActivityStatusListExtension != null)
            {
                ActivityStatusListExtension.ActivityStatuses.Remove(activityStatusID);
                if (ActivityStatusListExtension.ActivityStatuses.Count == 0)
                {
                    ActivityStatusListExtension = null;
                }
            }
        }

        public IEnumerable<short> ActivityStatusIDs
        {
            get
            {
                if (ActivityStatusListExtension == null)
                {
                    return new short[] { };
                }
                else
                {
                    return ActivityStatusListExtension.ActivityStatuses;
                }
            }
        }

        public void AddActivityStatusID(short activityStatusID)
        {
            if (ActivityStatusListExtension == null)
            {
                ActivityStatusListExtension = Create.New<IActivityStatusQualificationExtension>();
            }
            ActivityStatusListExtension.ActivityStatuses.Add(activityStatusID);
        }
        //FIN - GR_Encore-07
        #endregion

        #region Account IDs
        protected IAccountListQualificationExtension AccountListExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.AccountIDs))
                {
                    return (IAccountListQualificationExtension)PromotionQualifications[QualificationNames.AccountIDs];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.AccountIDs);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.AccountIDs, value);
                }
            }
        }

        public IEnumerable<int> AccountIDs
        {
            get
            {
                if (AccountListExtension == null)
                {
                    return new int[] { };
                }
                else
                {
                    return AccountListExtension.AccountNumbers;
                }
            }
        }

        public void AddAccountID(int accountID)
        {
            if (AccountListExtension == null)
            {
                AccountListExtension = Create.New<IAccountListQualificationExtension>();
            }
            AccountListExtension.AccountNumbers.Add(accountID);
        }

        public void DeleteAccountID(int accountID)
        {
            if (AccountListExtension != null)
            {
                AccountListExtension.AccountNumbers.Remove(accountID);
                if (AccountListExtension.AccountNumbers.Count == 0)
                {
                    AccountListExtension = null;
                }
            }
        }
        #endregion

        #region Market List

        protected IMarketListQualificationExtension MarketListExtension
        {
            get
            {
                if (PromotionQualifications.ContainsKey(QualificationNames.MarketList))
                    return (IMarketListQualificationExtension)PromotionQualifications[QualificationNames.MarketList];
                else
                    return null;
            }
            set
            {
                if (value == null)
                {
                    PromotionQualifications.Remove(QualificationNames.MarketList);
                }
                else
                {
                    PromotionQualifications.Add(QualificationNames.MarketList, value);
                }
            }
        }

        #endregion

        #endregion





    }
}
