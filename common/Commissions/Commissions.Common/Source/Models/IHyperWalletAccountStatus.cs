using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for HyperWalletAccountStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.HyperWalletAccountStatusContracts))]
	public interface IHyperWalletAccountStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HyperWalletAccountStatusID for this HyperWalletAccountStatus.
		/// </summary>
		int HyperWalletAccountStatusID { get; set; }
	
		/// <summary>
		/// The Status for this HyperWalletAccountStatus.
		/// </summary>
		string Status { get; set; }
	
		/// <summary>
		/// The DataVersion for this HyperWalletAccountStatus.
		/// </summary>
		byte[] DataVersion { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHyperWalletAccountStatus))]
		internal abstract class HyperWalletAccountStatusContracts : IHyperWalletAccountStatus
		{
		    #region Primitive properties
		
			int IHyperWalletAccountStatus.HyperWalletAccountStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletAccountStatus.Status
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IHyperWalletAccountStatus.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
