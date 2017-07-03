using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for AutoshipBatch.
	/// </summary>
	[ContractClass(typeof(Contracts.AutoshipBatchContracts))]
	public interface IAutoshipBatch
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AutoshipBatchID for this AutoshipBatch.
		/// </summary>
		int AutoshipBatchID { get; set; }
	
		/// <summary>
		/// The UserID for this AutoshipBatch.
		/// </summary>
		Nullable<int> UserID { get; set; }
	
		/// <summary>
		/// The StartDateUTC for this AutoshipBatch.
		/// </summary>
		Nullable<System.DateTime> StartDateUTC { get; set; }
	
		/// <summary>
		/// The EndDateUTC for this AutoshipBatch.
		/// </summary>
		Nullable<System.DateTime> EndDateUTC { get; set; }
	
		/// <summary>
		/// The Notes for this AutoshipBatch.
		/// </summary>
		string Notes { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AutoshipLogs for this AutoshipBatch.
		/// </summary>
		IEnumerable<IAutoshipLog> AutoshipLogs { get; }
	
		/// <summary>
		/// Adds an <see cref="IAutoshipLog"/> to the AutoshipLogs collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoshipLog"/> to add.</param>
		void AddAutoshipLog(IAutoshipLog item);
	
		/// <summary>
		/// Removes an <see cref="IAutoshipLog"/> from the AutoshipLogs collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoshipLog"/> to remove.</param>
		void RemoveAutoshipLog(IAutoshipLog item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAutoshipBatch))]
		internal abstract class AutoshipBatchContracts : IAutoshipBatch
		{
		    #region Primitive properties
		
			int IAutoshipBatch.AutoshipBatchID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAutoshipBatch.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAutoshipBatch.StartDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAutoshipBatch.EndDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoshipBatch.Notes
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAutoshipLog> IAutoshipBatch.AutoshipLogs
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAutoshipBatch.AddAutoshipLog(IAutoshipLog item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAutoshipBatch.RemoveAutoshipLog(IAutoshipLog item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
