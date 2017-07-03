using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for UserFunctionOverride.
	/// </summary>
	[ContractClass(typeof(Contracts.UserFunctionOverrideContracts))]
	public interface IUserFunctionOverride
	{
	    #region Primitive properties
	
		/// <summary>
		/// The UserFunctionOverrideID for this UserFunctionOverride.
		/// </summary>
		int UserFunctionOverrideID { get; set; }
	
		/// <summary>
		/// The UserID for this UserFunctionOverride.
		/// </summary>
		int UserID { get; set; }
	
		/// <summary>
		/// The FunctionID for this UserFunctionOverride.
		/// </summary>
		int FunctionID { get; set; }
	
		/// <summary>
		/// The Allow for this UserFunctionOverride.
		/// </summary>
		bool Allow { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Function for this UserFunctionOverride.
		/// </summary>
	    IFunction Function { get; set; }
	
		/// <summary>
		/// The User for this UserFunctionOverride.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IUserFunctionOverride))]
		internal abstract class UserFunctionOverrideContracts : IUserFunctionOverride
		{
		    #region Primitive properties
		
			int IUserFunctionOverride.UserFunctionOverrideID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IUserFunctionOverride.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IUserFunctionOverride.FunctionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IUserFunctionOverride.Allow
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IFunction IUserFunctionOverride.Function
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IUserFunctionOverride.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
