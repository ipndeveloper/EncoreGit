using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for HyperWalletProfile.
	/// </summary>
	[ContractClass(typeof(Contracts.HyperWalletProfileContracts))]
	public interface IHyperWalletProfile
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HyperWalletProfileID for this HyperWalletProfile.
		/// </summary>
		int HyperWalletProfileID { get; set; }
	
		/// <summary>
		/// The AccountID for this HyperWalletProfile.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The Enabled for this HyperWalletProfile.
		/// </summary>
		Nullable<bool> Enabled { get; set; }
	
		/// <summary>
		/// The DataVersion for this HyperWalletProfile.
		/// </summary>
		byte[] DataVersion { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHyperWalletProfile))]
		internal abstract class HyperWalletProfileContracts : IHyperWalletProfile
		{
		    #region Primitive properties
		
			int IHyperWalletProfile.HyperWalletProfileID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHyperWalletProfile.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IHyperWalletProfile.Enabled
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IHyperWalletProfile.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
