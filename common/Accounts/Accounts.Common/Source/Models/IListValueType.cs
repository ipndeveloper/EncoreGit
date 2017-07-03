using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for ListValueType.
	/// </summary>
	[ContractClass(typeof(Contracts.ListValueTypeContracts))]
	public interface IListValueType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ListValueTypeID for this ListValueType.
		/// </summary>
		short ListValueTypeID { get; set; }
	
		/// <summary>
		/// The Name for this ListValueType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this ListValueType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this ListValueType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this ListValueType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountListValues for this ListValueType.
		/// </summary>
		IEnumerable<IAccountListValue> AccountListValues { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountListValue"/> to the AccountListValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountListValue"/> to add.</param>
		void AddAccountListValue(IAccountListValue item);
	
		/// <summary>
		/// Removes an <see cref="IAccountListValue"/> from the AccountListValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountListValue"/> to remove.</param>
		void RemoveAccountListValue(IAccountListValue item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IListValueType))]
		internal abstract class ListValueTypeContracts : IListValueType
		{
		    #region Primitive properties
		
			short IListValueType.ListValueTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IListValueType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IListValueType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IListValueType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IListValueType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountListValue> IListValueType.AccountListValues
			{
				get { throw new NotImplementedException(); }
			}
		
			void IListValueType.AddAccountListValue(IAccountListValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IListValueType.RemoveAccountListValue(IAccountListValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
