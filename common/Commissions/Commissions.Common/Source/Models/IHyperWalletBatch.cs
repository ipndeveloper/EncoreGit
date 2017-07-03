using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for HyperWalletBatch.
	/// </summary>
	[ContractClass(typeof(Contracts.HyperWalletBatchContracts))]
	public interface IHyperWalletBatch
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HyperWalletBatchID for this HyperWalletBatch.
		/// </summary>
		int HyperWalletBatchID { get; set; }
	
		/// <summary>
		/// The FileDate for this HyperWalletBatch.
		/// </summary>
		System.DateTime FileDate { get; set; }
	
		/// <summary>
		/// The EntryCount for this HyperWalletBatch.
		/// </summary>
		Nullable<int> EntryCount { get; set; }
	
		/// <summary>
		/// The TotalAmount for this HyperWalletBatch.
		/// </summary>
		Nullable<decimal> TotalAmount { get; set; }
	
		/// <summary>
		/// The FileName for this HyperWalletBatch.
		/// </summary>
		string FileName { get; set; }
	
		/// <summary>
		/// The BatchStatusID for this HyperWalletBatch.
		/// </summary>
		int BatchStatusID { get; set; }
	
		/// <summary>
		/// The DataVersion for this HyperWalletBatch.
		/// </summary>
		byte[] DataVersion { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The HyperWalletBatchStatus for this HyperWalletBatch.
		/// </summary>
	    IHyperWalletBatchStatus HyperWalletBatchStatus { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHyperWalletBatch))]
		internal abstract class HyperWalletBatchContracts : IHyperWalletBatch
		{
		    #region Primitive properties
		
			int IHyperWalletBatch.HyperWalletBatchID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IHyperWalletBatch.FileDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IHyperWalletBatch.EntryCount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IHyperWalletBatch.TotalAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatch.FileName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHyperWalletBatch.BatchStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IHyperWalletBatch.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IHyperWalletBatchStatus IHyperWalletBatch.HyperWalletBatchStatus
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
