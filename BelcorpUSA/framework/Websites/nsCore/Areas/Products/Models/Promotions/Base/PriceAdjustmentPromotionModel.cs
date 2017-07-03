using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions.Base
{
	public abstract class PriceAdjustmentPromotionModel : IStandardQualificationPromotionModel
	{
		public int PromotionID { get; set; }
		public string Name { get; set; }
		public string CouponCode { get; set; }
		public bool OneTimeUse { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public IList<int> PaidAsTitleIDs { get; set; }
		public IList<int> RecognizedTitleIDs { get; set; }
		public IList<int> AccountTypeIDs { get; set; }
		public IList<int> OrderTypeIDs { get; set; }
		public bool HasCouponCode { get; set; }
		public bool HasDates { get; set; }  
		public bool HasAccountTypes { get; set; }
		public bool HasOrderTypeIDs { get; set; }
        public bool HasAccountIDs{ get; set; }
        public IList<int> AccountIDs { get; set; }
        public ICartConditionModel CartCondition { get; set; }

        public bool HasContinuity { get; set; }            //INI-FIN - GR_Encore-07
        public bool HasBAStatusIDs { get; set; }           //INI-FIN - GR_Encore-07
        public bool HasActivityStatusIDs { get; set; }     //INI-FIN - GR_Encore-07
        public IList<int> NewBAStatusIDs { get; set; }     //INI-FIN - GR_Encore-07
        public IList<short> ActivityStatusIDs { get; set; }  //INI-FIN - GR_Encore-07

		public PriceAdjustmentPromotionModel()
		{
			PaidAsTitleIDs = new List<int>();
			RecognizedTitleIDs = new List<int>();
			AccountTypeIDs = new List<int>();
			OrderTypeIDs = new List<int>();
            AccountIDs = new List<int>();
            NewBAStatusIDs = new List<int>();           //INI-FIN - GR_Encore-07
            ActivityStatusIDs = new List<short>();      //INI-FIN - GR_Encore-07
		}

		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(PaidAsTitleIDs != null);
			Contract.Invariant(RecognizedTitleIDs != null);
			Contract.Invariant(AccountTypeIDs != null);
			Contract.Invariant(OrderTypeIDs != null);
            //INI - GR_Encore-07
            Contract.Invariant(NewBAStatusIDs != null);
            Contract.Invariant(ActivityStatusIDs != null);
            //FIN - GR_Encore-07
		}
    
    }
}
