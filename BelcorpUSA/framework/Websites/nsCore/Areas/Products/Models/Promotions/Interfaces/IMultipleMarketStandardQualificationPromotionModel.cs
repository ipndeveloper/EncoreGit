using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace nsCore.Areas.Products.Models.Promotions.Interfaces
{
    [ContractClass(typeof(MultipleMarketStandardQualificationPromotionModelContract))]
    public interface IMultipleMarketStandardQualificationPromotionModel : IStandardQualificationPromotionModel
    {
        bool HasMarketIDs { get; set; }
        IList<int> MarketIDs { get; }
    }

    [ContractClassFor(typeof(IMultipleMarketStandardQualificationPromotionModel))]
    public abstract class MultipleMarketStandardQualificationPromotionModelContract : IMultipleMarketStandardQualificationPromotionModel
    {
        public IList<int> MarketIDs
        {
            get
            {
                Contract.Ensures(Contract.Result<IList<int>>() != null);
                throw new NotImplementedException();
            }
        }

        public bool HasMarketIDs
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

        public abstract int PromotionID { get; set; }

        public abstract string Name { get; set; }

        public abstract bool HasCouponCode { get; set; }

        public abstract bool HasContinuity { get; set; }        //INI-FIN - GR_Encore-07

        public abstract bool HasBAStatusIDs { get; set; }       //INI-FIN - GR_Encore-07

        public abstract bool HasActivityStatusIDs { get; set; } //INI-FIN - GR_Encore-07

        public abstract string CouponCode { get; set; }

        public abstract bool OneTimeUse { get; set; }

        public abstract bool HasDates { get; set; }

        public abstract DateTime? StartDate { get; set; }

        public abstract DateTime? EndDate { get; set; }

        public abstract bool HasAccountTypes { get; set; }

        public abstract IList<int> PaidAsTitleIDs { get; set; }

        public abstract IList<int> RecognizedTitleIDs { get; set; }

        public abstract IList<int> AccountTypeIDs { get; set; }

        public abstract IList<int> NewBAStatusIDs { get; set; }         //INI-FIN - GR_Encore-07

        public abstract IList<short> ActivityStatusIDs { get; set; }    //INI-FIN - GR_Encore-07

        public abstract bool HasOrderTypeIDs { get; set; }

        public abstract IList<int> OrderTypeIDs { get; set; }

        public abstract bool HasAccountIDs { get; set; }

        public abstract IList<int> AccountIDs { get; set; }

    }
}