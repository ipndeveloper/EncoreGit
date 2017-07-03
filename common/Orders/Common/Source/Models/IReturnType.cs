using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for ReturnType.
	/// </summary>
	[ContractClass(typeof(Contracts.ReturnTypeContracts))]
	public interface IReturnType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ReturnTypeID for this ReturnType.
		/// </summary>
		int ReturnTypeID { get; set; }
	
		/// <summary>
		/// The Name for this ReturnType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this ReturnType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this ReturnType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this ReturnType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this ReturnType.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The Editable for this ReturnType.
		/// </summary>
		bool Editable { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IReturnType))]
		internal abstract class ReturnTypeContracts : IReturnType
		{
		    #region Primitive properties
		
			int IReturnType.ReturnTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IReturnType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IReturnType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IReturnType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IReturnType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IReturnType.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IReturnType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
