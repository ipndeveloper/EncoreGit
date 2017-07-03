using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for DisbursementType.
	/// </summary>
	[ContractClass(typeof(Contracts.DisbursementTypeContracts))]
	public interface IDisbursementType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DisbursementTypeID for this DisbursementType.
		/// </summary>
		int DisbursementTypeID { get; set; }
	
		/// <summary>
		/// The Name for this DisbursementType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The NumberAllowed for this DisbursementType.
		/// </summary>
		Nullable<int> NumberAllowed { get; set; }
	
		/// <summary>
		/// The Enabled for this DisbursementType.
		/// </summary>
		bool Enabled { get; set; }
	
		/// <summary>
		/// The Editable for this DisbursementType.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The TermName for this DisbursementType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Code for this DisbursementType.
		/// </summary>
		string Code { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DisbursementAttributes for this DisbursementType.
		/// </summary>
		IEnumerable<IDisbursementAttribute> DisbursementAttributes { get; }
	
		/// <summary>
		/// Adds an <see cref="IDisbursementAttribute"/> to the DisbursementAttributes collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementAttribute"/> to add.</param>
		void AddDisbursementAttribute(IDisbursementAttribute item);
	
		/// <summary>
		/// Removes an <see cref="IDisbursementAttribute"/> from the DisbursementAttributes collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementAttribute"/> to remove.</param>
		void RemoveDisbursementAttribute(IDisbursementAttribute item);
	
		/// <summary>
		/// The DisbursementDetails for this DisbursementType.
		/// </summary>
		IEnumerable<IDisbursementDetail> DisbursementDetails { get; }
	
		/// <summary>
		/// Adds an <see cref="IDisbursementDetail"/> to the DisbursementDetails collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementDetail"/> to add.</param>
		void AddDisbursementDetail(IDisbursementDetail item);
	
		/// <summary>
		/// Removes an <see cref="IDisbursementDetail"/> from the DisbursementDetails collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementDetail"/> to remove.</param>
		void RemoveDisbursementDetail(IDisbursementDetail item);
	
		/// <summary>
		/// The DisbursementMinimums for this DisbursementType.
		/// </summary>
		IEnumerable<IDisbursementMinimum> DisbursementMinimums { get; }
	
		/// <summary>
		/// Adds an <see cref="IDisbursementMinimum"/> to the DisbursementMinimums collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementMinimum"/> to add.</param>
		void AddDisbursementMinimum(IDisbursementMinimum item);
	
		/// <summary>
		/// Removes an <see cref="IDisbursementMinimum"/> from the DisbursementMinimums collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementMinimum"/> to remove.</param>
		void RemoveDisbursementMinimum(IDisbursementMinimum item);
	
		/// <summary>
		/// The DisbursementProfiles for this DisbursementType.
		/// </summary>
		IEnumerable<IDisbursementProfile> DisbursementProfiles { get; }
	
		/// <summary>
		/// Adds an <see cref="IDisbursementProfile"/> to the DisbursementProfiles collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementProfile"/> to add.</param>
		void AddDisbursementProfile(IDisbursementProfile item);
	
		/// <summary>
		/// Removes an <see cref="IDisbursementProfile"/> from the DisbursementProfiles collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementProfile"/> to remove.</param>
		void RemoveDisbursementProfile(IDisbursementProfile item);
	
		/// <summary>
		/// The DisbursementTypeCountries for this DisbursementType.
		/// </summary>
		IEnumerable<IDisbursementTypeCountry> DisbursementTypeCountries { get; }
	
		/// <summary>
		/// Adds an <see cref="IDisbursementTypeCountry"/> to the DisbursementTypeCountries collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementTypeCountry"/> to add.</param>
		void AddDisbursementTypeCountry(IDisbursementTypeCountry item);
	
		/// <summary>
		/// Removes an <see cref="IDisbursementTypeCountry"/> from the DisbursementTypeCountries collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementTypeCountry"/> to remove.</param>
		void RemoveDisbursementTypeCountry(IDisbursementTypeCountry item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDisbursementType))]
		internal abstract class DisbursementTypeContracts : IDisbursementType
		{
		    #region Primitive properties
		
			int IDisbursementType.DisbursementTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDisbursementType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IDisbursementType.NumberAllowed
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDisbursementType.Enabled
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDisbursementType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDisbursementType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDisbursementType.Code
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDisbursementAttribute> IDisbursementType.DisbursementAttributes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDisbursementType.AddDisbursementAttribute(IDisbursementAttribute item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDisbursementType.RemoveDisbursementAttribute(IDisbursementAttribute item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDisbursementDetail> IDisbursementType.DisbursementDetails
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDisbursementType.AddDisbursementDetail(IDisbursementDetail item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDisbursementType.RemoveDisbursementDetail(IDisbursementDetail item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDisbursementMinimum> IDisbursementType.DisbursementMinimums
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDisbursementType.AddDisbursementMinimum(IDisbursementMinimum item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDisbursementType.RemoveDisbursementMinimum(IDisbursementMinimum item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDisbursementProfile> IDisbursementType.DisbursementProfiles
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDisbursementType.AddDisbursementProfile(IDisbursementProfile item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDisbursementType.RemoveDisbursementProfile(IDisbursementProfile item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDisbursementTypeCountry> IDisbursementType.DisbursementTypeCountries
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDisbursementType.AddDisbursementTypeCountry(IDisbursementTypeCountry item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDisbursementType.RemoveDisbursementTypeCountry(IDisbursementTypeCountry item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
