using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountPropertyValue.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountPropertyValueContracts))]
	public interface IAccountPropertyValue
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountPropertyValueID for this AccountPropertyValue.
		/// </summary>
		int AccountPropertyValueID { get; set; }
	
		/// <summary>
		/// The AccountPropertyTypeID for this AccountPropertyValue.
		/// </summary>
		int AccountPropertyTypeID { get; set; }
	
		/// <summary>
		/// The Name for this AccountPropertyValue.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Value for this AccountPropertyValue.
		/// </summary>
		string Value { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The AccountPropertyType for this AccountPropertyValue.
		/// </summary>
	    IAccountPropertyType AccountPropertyType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountProperties for this AccountPropertyValue.
		/// </summary>
		IEnumerable<IAccountProperty> AccountProperties { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountProperty"/> to the AccountProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountProperty"/> to add.</param>
		void AddAccountProperty(IAccountProperty item);
	
		/// <summary>
		/// Removes an <see cref="IAccountProperty"/> from the AccountProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountProperty"/> to remove.</param>
		void RemoveAccountProperty(IAccountProperty item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountPropertyValue))]
		internal abstract class AccountPropertyValueContracts : IAccountPropertyValue
		{
		    #region Primitive properties
		
			int IAccountPropertyValue.AccountPropertyValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPropertyValue.AccountPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPropertyValue.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPropertyValue.Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccountPropertyType IAccountPropertyValue.AccountPropertyType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountProperty> IAccountPropertyValue.AccountProperties
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountPropertyValue.AddAccountProperty(IAccountProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountPropertyValue.RemoveAccountProperty(IAccountProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
