using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace NetSteps.Promotions.Common.Model
{
    [ContractClass(typeof(Contracts.IPromotionContract))]
    public interface IPromotion
    {
        int PromotionID { get; set; }
        IDictionary<string, IPromotionQualificationExtension> PromotionQualifications { get; set; }
        IDictionary<string, IPromotionReward> PromotionRewards { get; set; }
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        string Description { get; set; }
        int PromotionStatusTypeID { get; set; }
		IEnumerable<string> AssociatedPropertyNames { get; }
        bool ValidFor<TProperty>(string propertyName, TProperty value);
        string PromotionKind { get; }
	}

}

namespace NetSteps.Promotions.Common.Model.Contracts
{
    [ContractClassFor(typeof(IPromotion))]
    public abstract class IPromotionContract : IPromotion
    {

        int IPromotion.PromotionID { get; set; }

        IDictionary<string, IPromotionQualificationExtension> IPromotion.PromotionQualifications
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, IPromotionQualificationExtension>>() != null);
                return new Dictionary<string, IPromotionQualificationExtension>();
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "Promotion qualifications cannot be null.");
            }
        }

        IDictionary<string, IPromotionReward> IPromotion.PromotionRewards
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, IPromotionReward>>() != null);
                return new Dictionary<string, IPromotionReward>();
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "Promotion rewards cannot be null.");
            }
        }

        DateTime? IPromotion.StartDate { get; set; }

        DateTime? IPromotion.EndDate { get; set; }

        string IPromotion.Description
        {
            get
            {
                return String.Empty;
            }
            set
            {
                Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(value));
            }
        }

        int IPromotion.PromotionStatusTypeID { get; set; }

        IEnumerable<string> IPromotion.AssociatedPropertyNames
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

                return default(IEnumerable<string>);
            }
        }

        bool IPromotion.ValidFor<TProperty>(string propertyName, TProperty value)
        {
            Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(propertyName));
            Contract.Requires<ArgumentNullException>(value != null);

            return default(bool);
        }

        string IPromotion.PromotionKind
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return default(string);
            }
        }
	}
}