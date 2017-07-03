using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for AutoresponderMessageToken.
	/// </summary>
	[ContractClass(typeof(Contracts.AutoresponderMessageTokenContracts))]
	public interface IAutoresponderMessageToken
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AutoresponderMessageTokenID for this AutoresponderMessageToken.
		/// </summary>
		int AutoresponderMessageTokenID { get; set; }
	
		/// <summary>
		/// The AutoresponderMessageID for this AutoresponderMessageToken.
		/// </summary>
		int AutoresponderMessageID { get; set; }
	
		/// <summary>
		/// The Token for this AutoresponderMessageToken.
		/// </summary>
		string Token { get; set; }
	
		/// <summary>
		/// The Value for this AutoresponderMessageToken.
		/// </summary>
		string Value { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The AutoresponderMessage for this AutoresponderMessageToken.
		/// </summary>
	    IAutoresponderMessage AutoresponderMessage { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAutoresponderMessageToken))]
		internal abstract class AutoresponderMessageTokenContracts : IAutoresponderMessageToken
		{
		    #region Primitive properties
		
			int IAutoresponderMessageToken.AutoresponderMessageTokenID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoresponderMessageToken.AutoresponderMessageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoresponderMessageToken.Token
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoresponderMessageToken.Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAutoresponderMessage IAutoresponderMessageToken.AutoresponderMessage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
