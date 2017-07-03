using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions
{
    [ContainerRegister(typeof(ICartRewardsPromotionModel), RegistrationBehaviors.Default)]
	public class CartRewardsPromotionModel : ICartRewardsPromotionModel
	{
		public CartRewardsPromotionModel()
		{
			OrderTypeIDs = new List<int>();
			AccountTypeIDs = new List<int>();
			RecognizedTitleIDs = new List<int>();
			PaidAsTitleIDs = new List<int>();
            AccountIDs = new List<int>();
            //INI- GR_Encore-07
            NewBAStatusIDs = new List<int>();
            ActivityStatusIDs = new List<short>();
            //FIN - GR_Encore-07
		}

        
		public bool HasCouponCode { get; set; }
		public bool HasDates { get; set; }
		public bool HasAccountTypes { get; set; }
		public bool HasOrderTypeIDs { get; set; }
        public bool HasAccountIDs { get; set; }

        public bool HasContinuity { get; set; }         //INI-FIN - GR_Encore-07
        public bool HasBAStatusIDs { get; set; }        //INI-FIN - GR_Encore-07
        public bool HasActivityStatusIDs { get; set; }  //INI-FIN - GR_Encore-07

		public int PromotionID { get; set; }
		public string Name { get; set; }
		public int AdjustmentTypeID { get; set; }
		public decimal? PercentOff { get; set; }

		public string CouponCode { get; set; }
		public bool RequiresCouponCode
		{
			get
			{
				return string.IsNullOrWhiteSpace(CouponCode);
			}
		}

		public bool OneTimeUse { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int MarketID { get; set; }
		public IList<int> PaidAsTitleIDs { get; set; }
		public IList<int> RecognizedTitleIDs { get; set; }
		public IList<int> AccountTypeIDs { get; set; }
        public IList<int> AccountIDs { get; set; }
        public bool OrCondition { get; set; } //Has a combination of items
        
        public bool AndConditionQVTotal { get; set; } //Has a defined QV Total
        public decimal QvMin { get; set; }
        public decimal QvMax { get; set; }

        public IList<int> NewBAStatusIDs { get; set; }      //INI-FIN - GR_Encore-07
        public IList<short> ActivityStatusIDs { get; set; }   //INI-FIN - GR_Encore-07

        public bool AccountIDsRestricted 
        {
            get {
                return AccountIDs.Count > 0;
            }
       }
        
        public bool RestrictedToAccounts
		{
			get
			{
				return PaidAsTitleIDs.Count > 0 || RecognizedTitleIDs.Count > 0 || AccountTypeIDs.Count > 0;
			}
		}

        //INI - GR_Encore-07
        public bool RestrictedToNewBAs
        {
            get
            {
                return NewBAStatusIDs.Count > 0;
            }
        }

        public bool RestrictedToActivityStatuses
        {
            get
            {
                return ActivityStatusIDs.Count > 0;
            }
        }
        //FIN - GR_Encore-07

		public IList<int> OrderTypeIDs { get; set; }
		public bool OrderTypesRestricted
		{
			get
			{
				return OrderTypeIDs.Count > 0;
			}
		}

		public ICartConditionModel CartCondition { get; set; }
		public ICartRewardModel CartReward { get; set; }

		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(PaidAsTitleIDs != null);
			Contract.Invariant(RecognizedTitleIDs != null);
			Contract.Invariant(AccountTypeIDs != null);
			Contract.Invariant(OrderTypeIDs != null);
            Contract.Invariant(AccountIDs != null);
		}
	}
}