using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for HyperWalletResponseCode.
	/// </summary>
	[ContractClass(typeof(Contracts.HyperWalletResponseCodeContracts))]
	public interface IHyperWalletResponseCode
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HyperWalletResponseCodeID for this HyperWalletResponseCode.
		/// </summary>
		int HyperWalletResponseCodeID { get; set; }
	
		/// <summary>
		/// The ResponseCode for this HyperWalletResponseCode.
		/// </summary>
		string ResponseCode { get; set; }
	
		/// <summary>
		/// The Status for this HyperWalletResponseCode.
		/// </summary>
		string Status { get; set; }
	
		/// <summary>
		/// The Description for this HyperWalletResponseCode.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The DataVersion for this HyperWalletResponseCode.
		/// </summary>
		byte[] DataVersion { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHyperWalletResponseCode))]
		internal abstract class HyperWalletResponseCodeContracts : IHyperWalletResponseCode
		{
		    #region Primitive properties
		
			int IHyperWalletResponseCode.HyperWalletResponseCodeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletResponseCode.ResponseCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletResponseCode.Status
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletResponseCode.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IHyperWalletResponseCode.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
