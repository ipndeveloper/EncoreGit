using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for AutoshipLog.
	/// </summary>
	[ContractClass(typeof(Contracts.AutoshipLogContracts))]
	public interface IAutoshipLog
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AutoshipLogID for this AutoshipLog.
		/// </summary>
		int AutoshipLogID { get; set; }
	
		/// <summary>
		/// The AutoshipBatchID for this AutoshipLog.
		/// </summary>
		Nullable<int> AutoshipBatchID { get; set; }
	
		/// <summary>
		/// The TemplateOrderID for this AutoshipLog.
		/// </summary>
		int TemplateOrderID { get; set; }
	
		/// <summary>
		/// The Succeeded for this AutoshipLog.
		/// </summary>
		bool Succeeded { get; set; }
	
		/// <summary>
		/// The Results for this AutoshipLog.
		/// </summary>
		string Results { get; set; }
	
		/// <summary>
		/// The NewOrderID for this AutoshipLog.
		/// </summary>
		Nullable<int> NewOrderID { get; set; }
	
		/// <summary>
		/// The DateCreatedUTC for this AutoshipLog.
		/// </summary>
		System.DateTime DateCreatedUTC { get; set; }
	
		/// <summary>
		/// The RunDate for this AutoshipLog.
		/// </summary>
		System.DateTime RunDate { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The AutoshipBatch for this AutoshipLog.
		/// </summary>
	    IAutoshipBatch AutoshipBatch { get; set; }
	
		/// <summary>
		/// The NewOrder for this AutoshipLog.
		/// </summary>
	    IOrder NewOrder { get; set; }
	
		/// <summary>
		/// The TemplateOrder for this AutoshipLog.
		/// </summary>
	    IOrder TemplateOrder { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAutoshipLog))]
		internal abstract class AutoshipLogContracts : IAutoshipLog
		{
		    #region Primitive properties
		
			int IAutoshipLog.AutoshipLogID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAutoshipLog.AutoshipBatchID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoshipLog.TemplateOrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAutoshipLog.Succeeded
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoshipLog.Results
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAutoshipLog.NewOrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IAutoshipLog.DateCreatedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IAutoshipLog.RunDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAutoshipBatch IAutoshipLog.AutoshipBatch
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrder IAutoshipLog.NewOrder
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrder IAutoshipLog.TemplateOrder
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
