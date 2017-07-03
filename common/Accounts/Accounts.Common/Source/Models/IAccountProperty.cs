using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountProperty.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountPropertyContracts))]
	public interface IAccountProperty
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountPropertyID for this AccountProperty.
		/// </summary>
		int AccountPropertyID { get; set; }
	
		/// <summary>
		/// The AccountID for this AccountProperty.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The AccountPropertyTypeID for this AccountProperty.
		/// </summary>
		int AccountPropertyTypeID { get; set; }
	
		/// <summary>
		/// The AccountPropertyValueID for this AccountProperty.
		/// </summary>
		Nullable<int> AccountPropertyValueID { get; set; }
	
		/// <summary>
		/// The PropertyValue for this AccountProperty.
		/// </summary>
		string PropertyValue { get; set; }
	
		/// <summary>
		/// The Active for this AccountProperty.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this AccountProperty.
		/// </summary>
	    IAccount Account { get; set; }
	
		/// <summary>
		/// The AccountPropertyType for this AccountProperty.
		/// </summary>
	    IAccountPropertyType AccountPropertyType { get; set; }
	
		/// <summary>
		/// The AccountPropertyValue for this AccountProperty.
		/// </summary>
	    IAccountPropertyValue AccountPropertyValue { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountProperty))]
		internal abstract class AccountPropertyContracts : IAccountProperty
		{
		    #region Primitive properties
		
			int IAccountProperty.AccountPropertyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountProperty.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountProperty.AccountPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountProperty.AccountPropertyValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountProperty.PropertyValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountProperty.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IAccountProperty.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountPropertyType IAccountProperty.AccountPropertyType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountPropertyValue IAccountProperty.AccountPropertyValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
