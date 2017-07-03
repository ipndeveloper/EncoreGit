using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for HostessRewardRule.
	/// </summary>
	[ContractClass(typeof(Contracts.HostessRewardRuleContracts))]
	public interface IHostessRewardRule
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HostessRewardRuleID for this HostessRewardRule.
		/// </summary>
		int HostessRewardRuleID { get; set; }
	
		/// <summary>
		/// The HostessRewardTypeID for this HostessRewardRule.
		/// </summary>
		int HostessRewardTypeID { get; set; }
	
		/// <summary>
		/// The HostessRewardRuleTypeID for this HostessRewardRule.
		/// </summary>
		int HostessRewardRuleTypeID { get; set; }
	
		/// <summary>
		/// The Min for this HostessRewardRule.
		/// </summary>
		Nullable<decimal> Min { get; set; }
	
		/// <summary>
		/// The Max for this HostessRewardRule.
		/// </summary>
		Nullable<decimal> Max { get; set; }
	
		/// <summary>
		/// The Reward for this HostessRewardRule.
		/// </summary>
		Nullable<decimal> Reward { get; set; }
	
		/// <summary>
		/// The CreditPercent for this HostessRewardRule.
		/// </summary>
		Nullable<decimal> CreditPercent { get; set; }
	
		/// <summary>
		/// The Products for this HostessRewardRule.
		/// </summary>
		Nullable<int> Products { get; set; }
	
		/// <summary>
		/// The ProductDiscount for this HostessRewardRule.
		/// </summary>
		Nullable<decimal> ProductDiscount { get; set; }
	
		/// <summary>
		/// The OrderItemTypeID for this HostessRewardRule.
		/// </summary>
		Nullable<short> OrderItemTypeID { get; set; }
	
		/// <summary>
		/// The DiscountCap for this HostessRewardRule.
		/// </summary>
		Nullable<decimal> DiscountCap { get; set; }
	
		/// <summary>
		/// The StartDateUTC for this HostessRewardRule.
		/// </summary>
		Nullable<System.DateTime> StartDateUTC { get; set; }
	
		/// <summary>
		/// The EndDateUTC for this HostessRewardRule.
		/// </summary>
		Nullable<System.DateTime> EndDateUTC { get; set; }
	
		/// <summary>
		/// The DaysOffset for this HostessRewardRule.
		/// </summary>
		Nullable<int> DaysOffset { get; set; }
	
		/// <summary>
		/// The ProductID for this HostessRewardRule.
		/// </summary>
		Nullable<int> ProductID { get; set; }
	
		/// <summary>
		/// The MarketID for this HostessRewardRule.
		/// </summary>
		Nullable<int> MarketID { get; set; }
	
		/// <summary>
		/// The MinCustomers for this HostessRewardRule.
		/// </summary>
		Nullable<int> MinCustomers { get; set; }
	
		/// <summary>
		/// The IsRedeemedAtChildParty for this HostessRewardRule.
		/// </summary>
		bool IsRedeemedAtChildParty { get; set; }
	
		/// <summary>
		/// The MinOrderSubTotal for this HostessRewardRule.
		/// </summary>
		Nullable<decimal> MinOrderSubTotal { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHostessRewardRule))]
		internal abstract class HostessRewardRuleContracts : IHostessRewardRule
		{
		    #region Primitive properties
		
			int IHostessRewardRule.HostessRewardRuleID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHostessRewardRule.HostessRewardTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHostessRewardRule.HostessRewardRuleTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IHostessRewardRule.Min
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IHostessRewardRule.Max
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IHostessRewardRule.Reward
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IHostessRewardRule.CreditPercent
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IHostessRewardRule.Products
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IHostessRewardRule.ProductDiscount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IHostessRewardRule.OrderItemTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IHostessRewardRule.DiscountCap
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IHostessRewardRule.StartDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IHostessRewardRule.EndDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IHostessRewardRule.DaysOffset
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IHostessRewardRule.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IHostessRewardRule.MarketID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IHostessRewardRule.MinCustomers
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHostessRewardRule.IsRedeemedAtChildParty
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IHostessRewardRule.MinOrderSubTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
