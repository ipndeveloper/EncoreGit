using System.Collections.Generic;
using NetSteps.Encore.Core.IoC;

namespace DistributorBackOffice.Areas.Orders.Models.Shared
{
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
        
        // Calculated Properties
        bool IsRemovable { get; }
        bool IsKit { get; }
        bool IsQuantityEditable { get; }

        string CustomItemText { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this OrderItem has custom text.
        /// </summary>
        bool HasCustomText { get; set; }

        decimal TotalQV { get; set; } //CGI(CMR)-29/10/2014
        string TotalQV_Currency { get; set; } //CGI(CMR)-29/10/2014
        int? OrderItemID { get; set; } // WV 20160617
    }

    [ContainerRegister(typeof(IOrderItemModel), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
    public class OrderItemModel : IOrderItemModel
    {
        public virtual string Guid { get; set; }
        public virtual int ProductID { get; set; }
        public virtual string SKU { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string OriginalUnitPrice { get; set; }
        public virtual string AdjustedUnitPrice { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string OriginalCommissionableTotal { get; set; }
        public virtual string AdjustedCommissionableTotal { get; set; }
        public virtual string OriginalTotal { get; set; }
        public virtual string AdjustedTotal { get; set; }
        public virtual bool IsStaticKit { get; set; }
        public virtual bool IsDynamicKit { get; set; }
        public virtual bool IsDynamicKitFull { get; set; }
        public virtual bool IsHostReward { get; set; }
        public virtual string BundlePackItemsUrl { get; set; }
        public virtual IKitItemsModel KitItemsModel { get; set; }
        public virtual string retailPricePerItem { get; set; } //recently added
        public virtual int? OrderItemID { get; set; }

    	public virtual bool SameTotal
    	{
    		get
    		{
				return AdjustedTotal == OriginalTotal;
    		}
    	}

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

        ICollection<string> _messages;
        public virtual ICollection<string> Messages
        {
            get
            {
                return _messages ?? (_messages = new List<string>());
            }
        }

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

        public string CustomItemText
        {
            get;
            set;
    }

        /// <summary>
        /// Gets or sets a value indicating whether this OrderItem has custom text.
        /// </summary>
        public virtual bool HasCustomText { get; set; }

        public virtual decimal TotalQV { get; set; } //CGI(CMR)-29/10/2014
        public virtual string TotalQV_Currency { get; set; } //CGI(CMR)-29/10/2014
    }
}