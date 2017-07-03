using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for ReturnReason.
	/// </summary>
	[ContractClass(typeof(Contracts.ReturnReasonContracts))]
	public interface IReturnReason
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ReturnReasonID for this ReturnReason.
		/// </summary>
		int ReturnReasonID { get; set; }
	
		/// <summary>
		/// The Name for this ReturnReason.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this ReturnReason.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this ReturnReason.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this ReturnReason.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this ReturnReason.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IReturnReason))]
		internal abstract class ReturnReasonContracts : IReturnReason
		{
		    #region Primitive properties
		
			int IReturnReason.ReturnReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IReturnReason.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IReturnReason.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IReturnReason.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IReturnReason.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IReturnReason.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
