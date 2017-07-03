using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for CheckHoldReason.
	/// </summary>
	[ContractClass(typeof(Contracts.CheckHoldReasonContracts))]
	public interface ICheckHoldReason
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ReasonID for this CheckHoldReason.
		/// </summary>
		int ReasonID { get; set; }
	
		/// <summary>
		/// The Enabled for this CheckHoldReason.
		/// </summary>
		bool Enabled { get; set; }
	
		/// <summary>
		/// The Editable for this CheckHoldReason.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The Name for this CheckHoldReason.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this CheckHoldReason.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Code for this CheckHoldReason.
		/// </summary>
		string Code { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The CheckHoldReason1 for this CheckHoldReason.
		/// </summary>
	    ICheckHoldReason CheckHoldReason1 { get; set; }
	
		/// <summary>
		/// The CheckHoldReasons1 for this CheckHoldReason.
		/// </summary>
	    ICheckHoldReason CheckHoldReasons1 { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICheckHoldReason))]
		internal abstract class CheckHoldReasonContracts : ICheckHoldReason
		{
		    #region Primitive properties
		
			int ICheckHoldReason.ReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICheckHoldReason.Enabled
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICheckHoldReason.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICheckHoldReason.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICheckHoldReason.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICheckHoldReason.Code
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICheckHoldReason ICheckHoldReason.CheckHoldReason1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    ICheckHoldReason ICheckHoldReason.CheckHoldReasons1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
