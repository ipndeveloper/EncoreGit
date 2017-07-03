using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for HyperWalletAccountType.
	/// </summary>
	[ContractClass(typeof(Contracts.HyperWalletAccountTypeContracts))]
	public interface IHyperWalletAccountType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HyperWalletAccountTypeID for this HyperWalletAccountType.
		/// </summary>
		int HyperWalletAccountTypeID { get; set; }
	
		/// <summary>
		/// The AccountType for this HyperWalletAccountType.
		/// </summary>
		string AccountType { get; set; }
	
		/// <summary>
		/// The DataVersion for this HyperWalletAccountType.
		/// </summary>
		byte[] DataVersion { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHyperWalletAccountType))]
		internal abstract class HyperWalletAccountTypeContracts : IHyperWalletAccountType
		{
		    #region Primitive properties
		
			int IHyperWalletAccountType.HyperWalletAccountTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletAccountType.AccountType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IHyperWalletAccountType.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
