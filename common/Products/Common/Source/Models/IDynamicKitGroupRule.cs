using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for DynamicKitGroupRule.
	/// </summary>
	[ContractClass(typeof(Contracts.DynamicKitGroupRuleContracts))]
	public interface IDynamicKitGroupRule
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DynamicKitGroupRuleID for this DynamicKitGroupRule.
		/// </summary>
		int DynamicKitGroupRuleID { get; set; }
	
		/// <summary>
		/// The DynamicKitGroupID for this DynamicKitGroupRule.
		/// </summary>
		int DynamicKitGroupID { get; set; }
	
		/// <summary>
		/// The ProductTypeID for this DynamicKitGroupRule.
		/// </summary>
		Nullable<int> ProductTypeID { get; set; }
	
		/// <summary>
		/// The ProductID for this DynamicKitGroupRule.
		/// </summary>
		Nullable<int> ProductID { get; set; }
	
		/// <summary>
		/// The Include for this DynamicKitGroupRule.
		/// </summary>
		bool Include { get; set; }
	
		/// <summary>
		/// The Default for this DynamicKitGroupRule.
		/// </summary>
		bool Default { get; set; }
	
		/// <summary>
		/// The Required for this DynamicKitGroupRule.
		/// </summary>
		bool Required { get; set; }
	
		/// <summary>
		/// The SortOrder for this DynamicKitGroupRule.
		/// </summary>
		int SortOrder { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The DynamicKitGroup for this DynamicKitGroupRule.
		/// </summary>
	    IDynamicKitGroup DynamicKitGroup { get; set; }
	
		/// <summary>
		/// The Product for this DynamicKitGroupRule.
		/// </summary>
	    IProduct Product { get; set; }
	
		/// <summary>
		/// The ProductType for this DynamicKitGroupRule.
		/// </summary>
	    IProductType ProductType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDynamicKitGroupRule))]
		internal abstract class DynamicKitGroupRuleContracts : IDynamicKitGroupRule
		{
		    #region Primitive properties
		
			int IDynamicKitGroupRule.DynamicKitGroupRuleID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDynamicKitGroupRule.DynamicKitGroupID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IDynamicKitGroupRule.ProductTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IDynamicKitGroupRule.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDynamicKitGroupRule.Include
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDynamicKitGroupRule.Default
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDynamicKitGroupRule.Required
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDynamicKitGroupRule.SortOrder
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IDynamicKitGroup IDynamicKitGroupRule.DynamicKitGroup
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProduct IDynamicKitGroupRule.Product
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProductType IDynamicKitGroupRule.ProductType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
