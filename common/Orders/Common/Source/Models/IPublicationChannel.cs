using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for PublicationChannel.
	/// </summary>
	[ContractClass(typeof(Contracts.PublicationChannelContracts))]
	public interface IPublicationChannel
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PublicationChannelID for this PublicationChannel.
		/// </summary>
		short PublicationChannelID { get; set; }
	
		/// <summary>
		/// The PublicationChannelNumber for this PublicationChannel.
		/// </summary>
		string PublicationChannelNumber { get; set; }
	
		/// <summary>
		/// The Name for this PublicationChannel.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this PublicationChannel.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this PublicationChannel.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this PublicationChannel.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Redemptions for this PublicationChannel.
		/// </summary>
		IEnumerable<IRedemption> Redemptions { get; }
	
		/// <summary>
		/// Adds an <see cref="IRedemption"/> to the Redemptions collection.
		/// </summary>
		/// <param name="item">The <see cref="IRedemption"/> to add.</param>
		void AddRedemption(IRedemption item);
	
		/// <summary>
		/// Removes an <see cref="IRedemption"/> from the Redemptions collection.
		/// </summary>
		/// <param name="item">The <see cref="IRedemption"/> to remove.</param>
		void RemoveRedemption(IRedemption item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPublicationChannel))]
		internal abstract class PublicationChannelContracts : IPublicationChannel
		{
		    #region Primitive properties
		
			short IPublicationChannel.PublicationChannelID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPublicationChannel.PublicationChannelNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPublicationChannel.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPublicationChannel.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPublicationChannel.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPublicationChannel.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IRedemption> IPublicationChannel.Redemptions
			{
				get { throw new NotImplementedException(); }
			}
		
			void IPublicationChannel.AddRedemption(IRedemption item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IPublicationChannel.RemoveRedemption(IRedemption item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
