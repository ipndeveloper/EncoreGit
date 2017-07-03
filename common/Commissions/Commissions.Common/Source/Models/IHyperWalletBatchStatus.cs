using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for HyperWalletBatchStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.HyperWalletBatchStatusContracts))]
	public interface IHyperWalletBatchStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HyperWalletBatchStatusID for this HyperWalletBatchStatus.
		/// </summary>
		int HyperWalletBatchStatusID { get; set; }
	
		/// <summary>
		/// The Name for this HyperWalletBatchStatus.
		/// </summary>
		string Name { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The HyperWalletBatches for this HyperWalletBatchStatus.
		/// </summary>
		IEnumerable<IHyperWalletBatch> HyperWalletBatches { get; }
	
		/// <summary>
		/// Adds an <see cref="IHyperWalletBatch"/> to the HyperWalletBatches collection.
		/// </summary>
		/// <param name="item">The <see cref="IHyperWalletBatch"/> to add.</param>
		void AddHyperWalletBatch(IHyperWalletBatch item);
	
		/// <summary>
		/// Removes an <see cref="IHyperWalletBatch"/> from the HyperWalletBatches collection.
		/// </summary>
		/// <param name="item">The <see cref="IHyperWalletBatch"/> to remove.</param>
		void RemoveHyperWalletBatch(IHyperWalletBatch item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHyperWalletBatchStatus))]
		internal abstract class HyperWalletBatchStatusContracts : IHyperWalletBatchStatus
		{
		    #region Primitive properties
		
			int IHyperWalletBatchStatus.HyperWalletBatchStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchStatus.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IHyperWalletBatch> IHyperWalletBatchStatus.HyperWalletBatches
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHyperWalletBatchStatus.AddHyperWalletBatch(IHyperWalletBatch item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHyperWalletBatchStatus.RemoveHyperWalletBatch(IHyperWalletBatch item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
