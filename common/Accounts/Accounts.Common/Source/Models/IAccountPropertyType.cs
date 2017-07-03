using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountPropertyType.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountPropertyTypeContracts))]
	public interface IAccountPropertyType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountPropertyTypeID for this AccountPropertyType.
		/// </summary>
		int AccountPropertyTypeID { get; set; }
	
		/// <summary>
		/// The Name for this AccountPropertyType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The DataType for this AccountPropertyType.
		/// </summary>
		string DataType { get; set; }
	
		/// <summary>
		/// The Required for this AccountPropertyType.
		/// </summary>
		bool Required { get; set; }
	
		/// <summary>
		/// The SortIndex for this AccountPropertyType.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The TermName for this AccountPropertyType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this AccountPropertyType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this AccountPropertyType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The Editable for this AccountPropertyType.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The IsInternal for this AccountPropertyType.
		/// </summary>
		bool IsInternal { get; set; }
	
		/// <summary>
		/// The MinimumLength for this AccountPropertyType.
		/// </summary>
		int MinimumLength { get; set; }
	
		/// <summary>
		/// The MaximumLength for this AccountPropertyType.
		/// </summary>
		int MaximumLength { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountProperties for this AccountPropertyType.
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
	
		/// <summary>
		/// The AccountPropertyValues for this AccountPropertyType.
		/// </summary>
		IEnumerable<IAccountPropertyValue> AccountPropertyValues { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountPropertyValue"/> to the AccountPropertyValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPropertyValue"/> to add.</param>
		void AddAccountPropertyValue(IAccountPropertyValue item);
	
		/// <summary>
		/// Removes an <see cref="IAccountPropertyValue"/> from the AccountPropertyValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPropertyValue"/> to remove.</param>
		void RemoveAccountPropertyValue(IAccountPropertyValue item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountPropertyType))]
		internal abstract class AccountPropertyTypeContracts : IAccountPropertyType
		{
		    #region Primitive properties
		
			int IAccountPropertyType.AccountPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPropertyType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPropertyType.DataType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountPropertyType.Required
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPropertyType.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPropertyType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPropertyType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountPropertyType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountPropertyType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountPropertyType.IsInternal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPropertyType.MinimumLength
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPropertyType.MaximumLength
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountProperty> IAccountPropertyType.AccountProperties
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountPropertyType.AddAccountProperty(IAccountProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountPropertyType.RemoveAccountProperty(IAccountProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountPropertyValue> IAccountPropertyType.AccountPropertyValues
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountPropertyType.AddAccountPropertyValue(IAccountPropertyValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountPropertyType.RemoveAccountPropertyValue(IAccountPropertyValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
