using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for Redemption.
	/// </summary>
	[ContractClass(typeof(Contracts.RedemptionContracts))]
	public interface IRedemption
	{
	    #region Primitive properties
	
		/// <summary>
		/// The RedemptionID for this Redemption.
		/// </summary>
		int RedemptionID { get; set; }
	
		/// <summary>
		/// The RedemptionNumber for this Redemption.
		/// </summary>
		string RedemptionNumber { get; set; }
	
		/// <summary>
		/// The RedemptionMethodID for this Redemption.
		/// </summary>
		short RedemptionMethodID { get; set; }
	
		/// <summary>
		/// The RedemptionInstruction for this Redemption.
		/// </summary>
		string RedemptionInstruction { get; set; }
	
		/// <summary>
		/// The RedemptionCouponName for this Redemption.
		/// </summary>
		string RedemptionCouponName { get; set; }
	
		/// <summary>
		/// The RedemptionCode for this Redemption.
		/// </summary>
		string RedemptionCode { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The RedemptionMethod for this Redemption.
		/// </summary>
	    IRedemptionMethod RedemptionMethod { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The PublicationChannels for this Redemption.
		/// </summary>
		IEnumerable<IPublicationChannel> PublicationChannels { get; }
	
		/// <summary>
		/// Adds an <see cref="IPublicationChannel"/> to the PublicationChannels collection.
		/// </summary>
		/// <param name="item">The <see cref="IPublicationChannel"/> to add.</param>
		void AddPublicationChannel(IPublicationChannel item);
	
		/// <summary>
		/// Removes an <see cref="IPublicationChannel"/> from the PublicationChannels collection.
		/// </summary>
		/// <param name="item">The <see cref="IPublicationChannel"/> to remove.</param>
		void RemovePublicationChannel(IPublicationChannel item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IRedemption))]
		internal abstract class RedemptionContracts : IRedemption
		{
		    #region Primitive properties
		
			int IRedemption.RedemptionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRedemption.RedemptionNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IRedemption.RedemptionMethodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRedemption.RedemptionInstruction
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRedemption.RedemptionCouponName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRedemption.RedemptionCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IRedemptionMethod IRedemption.RedemptionMethod
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IPublicationChannel> IRedemption.PublicationChannels
			{
				get { throw new NotImplementedException(); }
			}
		
			void IRedemption.AddPublicationChannel(IPublicationChannel item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IRedemption.RemovePublicationChannel(IPublicationChannel item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
