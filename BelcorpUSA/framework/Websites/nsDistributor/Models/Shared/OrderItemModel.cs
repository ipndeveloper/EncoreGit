using NetSteps.Encore.Core.IoC;

namespace nsDistributor.Models.Shared
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities;

    public interface IOrderItemModel
    {
        string Guid { get; set; }
        int ProductID { get; set; }
        string SKU { get; set; }
        string ProductName { get; set; }
        string AdjustedUnitPrice { get; set; }
        string OriginalUnitPrice { get; set; }
        bool SamePrice { get; }
        int Quantity { get; set; }
        string OriginalCommissionableTotal { get; set; }
        string AdjustedCommissionableTotal { get; set; }
        bool SameCommissionableTotal { get; }
        string AdjustedTotal { get; set; }
        string OriginalTotal { get; set; }
        bool SameTotal { get; }
        bool IsStaticKit { get; set; }
        bool IsDynamicKit { get; set; }
        bool IsDynamicKitFull { get; set; }
        bool IsHostReward { get; set; }
        string BundlePackItemsUrl { get; set; }
        IKitItemsModel KitItemsModel { get; set; }
        ICollection<string> Messages { get; }
        string retailPricePerItem { get; set; } //recently added 
        string ImageUrl { get; set; }
        string ModificationReason { get; set; }
        //decimal? OriginalUnitPrice { get; set; }
        //decimal? AdjustedUnitPrice { get; set; }
        //decimal? OriginalCommissionableTotal { get; set; }
        //decimal? AdjustedCommissionableTotal { get; set; }
        //decimal? OriginalTotal { get; set; }
        //decimal? AdjustedTotal { get; set; }
        //bool IsStaticKit { get; set; }
        //bool IsDynamicKit { get; set; }
        //bool IsDynamicKitFull { get; set; }
        //bool IsHostReward { get; set; }
        //string BundlePackItemsUrl { get; set; }
        //IKitItemsModel KitItemsModel { get; set; }
        string CurrencySymbol { get; set; }
        List<string> GiftCardCodes { get; set; }

        // Calculated Properties
        bool IsRemovable { get; }
        bool IsKit { get; }
        bool IsQuantityEditable { get; }
	    bool IsGiftCard { get;  }

        object CustomValue { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this OrderItem has client customization.
        /// </summary>
        bool HasCustomization { get; set; }
        /// <summary>
        /// Gets or sets a value indicating customization type of this Order Item.
        /// </summary>
        string CustomizationType { get; set; }

        #region

        decimal TotalQV { get; set; } //CGI(CMR)-29/10/2014
        string TotalQV_Currency { get; set; } //CGI(CMR)-29/10/2014

        #endregion
    }

    [ContainerRegister(typeof(IOrderItemModel), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
    public class OrderItemModel : IOrderItemModel
    {
        public virtual string Guid { get; set; }
        public virtual int ProductID { get; set; }
        public virtual string SKU { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string ModificationReason { get; set; }
        public virtual string OriginalUnitPrice { get; set; }
        public virtual string AdjustedUnitPrice { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string OriginalCommissionableTotal { get; set; }
        public virtual string AdjustedCommissionableTotal { get; set; }
        public virtual string OriginalTotal { get; set; }
        public virtual string AdjustedTotal { get; set; }

        //public decimal? OriginalUnitPrice { get; set; }
        //public decimal? AdjustedUnitPrice { get; set; }
        //public decimal? OriginalCommissionableTotal { get; set; }
        //public decimal? AdjustedCommissionableTotal { get; set; }
        //public decimal? OriginalTotal { get; set; }
        //public decimal? AdjustedTotal { get; set; }
        public virtual bool IsStaticKit { get; set; }
        public virtual bool IsDynamicKit { get; set; }
        public virtual bool IsDynamicKitFull { get; set; }
        public virtual bool IsHostReward { get; set; }
        public virtual string BundlePackItemsUrl { get; set; }
        public virtual IKitItemsModel KitItemsModel { get; set; }
        public string CurrencySymbol { get; set; }
        public List<string> GiftCardCodes { get; set; }

        public virtual string retailPricePerItem { get; set; } //recently added

        // Calculated Properties
        public virtual bool IsRemovable
        {
            get
            {
                return !IsHostReward;
            }
        }

        public virtual bool IsKit
        {
            get
            {
                return IsStaticKit || IsDynamicKit;
            }
        }

        public virtual bool IsQuantityEditable
        {
            get
            {
                return !IsHostReward && !IsDynamicKit;
            }
        }

		public virtual bool IsGiftCard
		{
			get
			{
				return GiftCardCodes != null && GiftCardCodes.Any();
			}
		}

        public object CustomValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this OrderItem has client customization.
        /// </summary>
        public virtual bool HasCustomization { get; set; }

        /// <summary>
        /// Gets or sets a value indicating customization type of this Order Item.
        /// </summary>
        public virtual string CustomizationType { get; set; }

        #region
        public virtual decimal TotalQV { get; set; } //CGI(CMR)-29/10/2014
        public virtual string TotalQV_Currency { get; set; } //CGI(CMR)-29/10/2014
        #endregion


        public virtual bool SamePrice
        {
            get
            {
                return AdjustedUnitPrice == OriginalUnitPrice;
            }
        }

        public virtual bool SameCommissionableTotal
        {
            get
            {
                return AdjustedCommissionableTotal == OriginalCommissionableTotal;
            }
        }

        public virtual bool SameTotal
        {
            get
            {
                return AdjustedTotal == OriginalTotal;
            }
        }

        ICollection<string> _messages;
        public virtual ICollection<string> Messages
        {
            get
            {
                return _messages ?? (_messages = new List<string>());
            }
        }

       
    }
}