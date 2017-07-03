using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for DisbursementTypeCountry.
	/// </summary>
	[ContractClass(typeof(Contracts.DisbursementTypeCountryContracts))]
	public interface IDisbursementTypeCountry
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DisbursementTypeID for this DisbursementTypeCountry.
		/// </summary>
		int DisbursementTypeID { get; set; }
	
		/// <summary>
		/// The CountryID for this DisbursementTypeCountry.
		/// </summary>
		int CountryID { get; set; }
	
		/// <summary>
		/// The Default for this DisbursementTypeCountry.
		/// </summary>
		Nullable<bool> Default { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The DisbursementType for this DisbursementTypeCountry.
		/// </summary>
	    IDisbursementType DisbursementType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDisbursementTypeCountry))]
		internal abstract class DisbursementTypeCountryContracts : IDisbursementTypeCountry
		{
		    #region Primitive properties
		
			int IDisbursementTypeCountry.DisbursementTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursementTypeCountry.CountryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IDisbursementTypeCountry.Default
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IDisbursementType IDisbursementTypeCountry.DisbursementType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
