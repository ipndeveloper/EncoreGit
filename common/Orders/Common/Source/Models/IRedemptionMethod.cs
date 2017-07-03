using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for RedemptionMethod.
	/// </summary>
	[ContractClass(typeof(Contracts.RedemptionMethodContracts))]
	public interface IRedemptionMethod
	{
	    #region Primitive properties
	
		/// <summary>
		/// The RedemptionMethodID for this RedemptionMethod.
		/// </summary>
		short RedemptionMethodID { get; set; }
	
		/// <summary>
		/// The Name for this RedemptionMethod.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this RedemptionMethod.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this RedemptionMethod.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this RedemptionMethod.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Redemptions for this RedemptionMethod.
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
		[ContractClassFor(typeof(IRedemptionMethod))]
		internal abstract class RedemptionMethodContracts : IRedemptionMethod
		{
		    #region Primitive properties
		
			short IRedemptionMethod.RedemptionMethodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRedemptionMethod.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRedemptionMethod.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRedemptionMethod.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IRedemptionMethod.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IRedemption> IRedemptionMethod.Redemptions
			{
				get { throw new NotImplementedException(); }
			}
		
			void IRedemptionMethod.AddRedemption(IRedemption item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IRedemptionMethod.RemoveRedemption(IRedemption item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
