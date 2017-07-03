using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for DistributionSubscriber.
	/// </summary>
	[ContractClass(typeof(Contracts.DistributionSubscriberContracts))]
	public interface IDistributionSubscriber
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DistributionSubscriberID for this DistributionSubscriber.
		/// </summary>
		int DistributionSubscriberID { get; set; }
	
		/// <summary>
		/// The DistributionListID for this DistributionSubscriber.
		/// </summary>
		int DistributionListID { get; set; }
	
		/// <summary>
		/// The AccountID for this DistributionSubscriber.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The DateSubscribedUTC for this DistributionSubscriber.
		/// </summary>
		Nullable<System.DateTime> DateSubscribedUTC { get; set; }
	
		/// <summary>
		/// The DateCancelledUTC for this DistributionSubscriber.
		/// </summary>
		Nullable<System.DateTime> DateCancelledUTC { get; set; }
	
		/// <summary>
		/// The Active for this DistributionSubscriber.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDistributionSubscriber))]
		internal abstract class DistributionSubscriberContracts : IDistributionSubscriber
		{
		    #region Primitive properties
		
			int IDistributionSubscriber.DistributionSubscriberID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDistributionSubscriber.DistributionListID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IDistributionSubscriber.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IDistributionSubscriber.DateSubscribedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IDistributionSubscriber.DateCancelledUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDistributionSubscriber.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
