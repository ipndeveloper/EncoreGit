using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace nsCore.Areas.Products.Models.Promotions.Interfaces
{
    [ContractClass(typeof(StandardQualificationPromotionModelContract))]
    public interface IStandardQualificationPromotionModel
    {
        int PromotionID { get; set; }
        string Name { get; set; }
        bool HasCouponCode { get; set; }
        bool HasContinuity { get; set; }            //INI-FIN - GR_Encore-07
        string CouponCode { get; set; }
        bool OneTimeUse { get; set; }
        bool HasDates { get; set; }
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        bool HasAccountTypes { get; set; }
        bool HasAccountIDs { get; set; }
        bool HasBAStatusIDs { get; set; }           //INI-FIN - GR_Encore-07
        bool HasActivityStatusIDs { get; set; }     //INI-FIN - GR_Encore-07
        IList<int> NewBAStatusIDs { get; set; }     //INI-FIN - GR_Encore-07
        IList<short> ActivityStatusIDs { get; set; }  //INI-FIN - GR_Encore-07
        IList<int> PaidAsTitleIDs { get; set; }
        IList<int> RecognizedTitleIDs { get; set; }
        IList<int> AccountTypeIDs { get; set; }
        bool HasOrderTypeIDs { get; set; }
        IList<int> OrderTypeIDs { get; set; }
        IList<int> AccountIDs { get; set; }
    }

    [ContractClassFor(typeof(IStandardQualificationPromotionModel))]
    public abstract class StandardQualificationPromotionModelContract : IStandardQualificationPromotionModel
    {

        public int PromotionID
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

        public string Name
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

        public bool HasCouponCode
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

        //INI - GR_Encore-07
        public bool HasContinuity
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
        //FIN - GR_Encore-07

        public bool HasAccountIDs
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

        public string CouponCode
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

        public bool OneTimeUse
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

        public bool HasDates
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

        public DateTime? StartDate
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

        public DateTime? EndDate
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

        public bool HasAccountTypes
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

        //INI - GR_Encore-07
        public bool HasBAStatusIDs
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

        public bool HasActivityStatusIDs
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

        public IList<int> NewBAStatusIDs
        {
            get
            {
                Contract.Ensures(Contract.Result<IList<int>>() != null);
                throw new NotImplementedException();
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                throw new NotImplementedException();
            }
        }

        public IList<short> ActivityStatusIDs
        {
            get
            {
                Contract.Ensures(Contract.Result<IList<short>>() != null);
                throw new NotImplementedException();
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                throw new NotImplementedException();
            }
        }
        //FIN - GR_Encore-07

        public IList<int> PaidAsTitleIDs
        {
            get
            {
                Contract.Ensures(Contract.Result<IList<int>>() != null);
                throw new NotImplementedException();
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                throw new NotImplementedException();
            }
        }

        public IList<int> RecognizedTitleIDs
        {
            get
            {
                Contract.Ensures(Contract.Result<IList<int>>() != null);
                throw new NotImplementedException();
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                throw new NotImplementedException();
            }
        }

        public IList<int> AccountTypeIDs
        {
            get
            {
                Contract.Ensures(Contract.Result<IList<int>>() != null);
                throw new NotImplementedException();
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                throw new NotImplementedException();
            }
        }

        public bool HasOrderTypeIDs
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

        public IList<int> OrderTypeIDs
        {
            get
            {
                Contract.Ensures(Contract.Result<IList<int>>() != null);
                throw new NotImplementedException();
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                throw new NotImplementedException();
            }
        }

        public IList<int> AccountIDs
        {
            get
            {
                Contract.Ensures(Contract.Result<IList<int>>() != null);
                throw new NotImplementedException();
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                throw new NotImplementedException();
            }
        }
    }
}
