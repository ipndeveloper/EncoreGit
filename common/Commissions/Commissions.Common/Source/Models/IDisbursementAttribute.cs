using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for DisbursementAttribute.
	/// </summary>
	[ContractClass(typeof(Contracts.DisbursementAttributeContracts))]
	public interface IDisbursementAttribute
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DisbursementAttributeID for this DisbursementAttribute.
		/// </summary>
		int DisbursementAttributeID { get; set; }
	
		/// <summary>
		/// The Description for this DisbursementAttribute.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The DataType for this DisbursementAttribute.
		/// </summary>
		string DataType { get; set; }
	
		/// <summary>
		/// The DisbursementTypeID for this DisbursementAttribute.
		/// </summary>
		int DisbursementTypeID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The DisbursementType for this DisbursementAttribute.
		/// </summary>
	    IDisbursementType DisbursementType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DisbursementProfileAttributes for this DisbursementAttribute.
		/// </summary>
		IEnumerable<IDisbursementProfileAttribute> DisbursementProfileAttributes { get; }
	
		/// <summary>
		/// Adds an <see cref="IDisbursementProfileAttribute"/> to the DisbursementProfileAttributes collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementProfileAttribute"/> to add.</param>
		void AddDisbursementProfileAttribute(IDisbursementProfileAttribute item);
	
		/// <summary>
		/// Removes an <see cref="IDisbursementProfileAttribute"/> from the DisbursementProfileAttributes collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementProfileAttribute"/> to remove.</param>
		void RemoveDisbursementProfileAttribute(IDisbursementProfileAttribute item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDisbursementAttribute))]
		internal abstract class DisbursementAttributeContracts : IDisbursementAttribute
		{
		    #region Primitive properties
		
			int IDisbursementAttribute.DisbursementAttributeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDisbursementAttribute.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDisbursementAttribute.DataType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursementAttribute.DisbursementTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IDisbursementType IDisbursementAttribute.DisbursementType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDisbursementProfileAttribute> IDisbursementAttribute.DisbursementProfileAttributes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDisbursementAttribute.AddDisbursementProfileAttribute(IDisbursementProfileAttribute item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDisbursementAttribute.RemoveDisbursementProfileAttribute(IDisbursementProfileAttribute item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
