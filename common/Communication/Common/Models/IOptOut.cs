using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for OptOut.
	/// </summary>
	[ContractClass(typeof(Contracts.OptOutContracts))]
	public interface IOptOut
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OptOutID for this OptOut.
		/// </summary>
		int OptOutID { get; set; }
	
		/// <summary>
		/// The OptOutTypeID for this OptOut.
		/// </summary>
		short OptOutTypeID { get; set; }
	
		/// <summary>
		/// The EmailAddress for this OptOut.
		/// </summary>
		string EmailAddress { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The OptOutType for this OptOut.
		/// </summary>
	    IOptOutType OptOutType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOptOut))]
		internal abstract class OptOutContracts : IOptOut
		{
		    #region Primitive properties
		
			int IOptOut.OptOutID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IOptOut.OptOutTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOptOut.EmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOptOutType IOptOut.OptOutType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
