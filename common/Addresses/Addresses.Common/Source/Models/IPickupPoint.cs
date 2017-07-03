using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Addresses.Common.Models
{
	/// <summary>
	/// Common interface for PickupPoint.
	/// </summary>
	[ContractClass(typeof(Contracts.PickupPointContracts))]
	public interface IPickupPoint
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PickupPointID for this PickupPoint.
		/// </summary>
		int PickupPointID { get; set; }
	
		/// <summary>
		/// The PickupPointCode for this PickupPoint.
		/// </summary>
		string PickupPointCode { get; set; }
	
		/// <summary>
		/// The AddressID for this PickupPoint.
		/// </summary>
		int AddressID { get; set; }
	
		/// <summary>
		/// The Name for this PickupPoint.
		/// </summary>
		string Name { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Address for this PickupPoint.
		/// </summary>
	    IAddress Address { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPickupPoint))]
		internal abstract class PickupPointContracts : IPickupPoint
		{
		    #region Primitive properties
		
			int IPickupPoint.PickupPointID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPickupPoint.PickupPointCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IPickupPoint.AddressID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPickupPoint.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAddress IPickupPoint.Address
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
