using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for DistributionList.
	/// </summary>
	[ContractClass(typeof(Contracts.DistributionListContracts))]
	public interface IDistributionList
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DistributionListID for this DistributionList.
		/// </summary>
		int DistributionListID { get; set; }
	
		/// <summary>
		/// The DistributionListTypeID for this DistributionList.
		/// </summary>
		short DistributionListTypeID { get; set; }
	
		/// <summary>
		/// The AccountID for this DistributionList.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The Name for this DistributionList.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Active for this DistributionList.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DistributionSubscribers for this DistributionList.
		/// </summary>
		IEnumerable<IDistributionSubscriber> DistributionSubscribers { get; }
	
		/// <summary>
		/// Adds an <see cref="IDistributionSubscriber"/> to the DistributionSubscribers collection.
		/// </summary>
		/// <param name="item">The <see cref="IDistributionSubscriber"/> to add.</param>
		void AddDistributionSubscriber(IDistributionSubscriber item);
	
		/// <summary>
		/// Removes an <see cref="IDistributionSubscriber"/> from the DistributionSubscribers collection.
		/// </summary>
		/// <param name="item">The <see cref="IDistributionSubscriber"/> to remove.</param>
		void RemoveDistributionSubscriber(IDistributionSubscriber item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDistributionList))]
		internal abstract class DistributionListContracts : IDistributionList
		{
		    #region Primitive properties
		
			int IDistributionList.DistributionListID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IDistributionList.DistributionListTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDistributionList.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDistributionList.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDistributionList.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDistributionSubscriber> IDistributionList.DistributionSubscribers
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDistributionList.AddDistributionSubscriber(IDistributionSubscriber item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDistributionList.RemoveDistributionSubscriber(IDistributionSubscriber item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
