using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for DisbursementFeeType.
	/// </summary>
	[ContractClass(typeof(Contracts.DisbursementFeeTypeContracts))]
	public interface IDisbursementFeeType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DisbursementFeeTypeID for this DisbursementFeeType.
		/// </summary>
		int DisbursementFeeTypeID { get; set; }
	
		/// <summary>
		/// The Name for this DisbursementFeeType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Enabled for this DisbursementFeeType.
		/// </summary>
		bool Enabled { get; set; }
	
		/// <summary>
		/// The Editable for this DisbursementFeeType.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The TermName for this DisbursementFeeType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Code for this DisbursementFeeType.
		/// </summary>
		string Code { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DisbursementFees for this DisbursementFeeType.
		/// </summary>
		IEnumerable<IDisbursementFee> DisbursementFees { get; }
	
		/// <summary>
		/// Adds an <see cref="IDisbursementFee"/> to the DisbursementFees collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementFee"/> to add.</param>
		void AddDisbursementFee(IDisbursementFee item);
	
		/// <summary>
		/// Removes an <see cref="IDisbursementFee"/> from the DisbursementFees collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementFee"/> to remove.</param>
		void RemoveDisbursementFee(IDisbursementFee item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDisbursementFeeType))]
		internal abstract class DisbursementFeeTypeContracts : IDisbursementFeeType
		{
		    #region Primitive properties
		
			int IDisbursementFeeType.DisbursementFeeTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDisbursementFeeType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDisbursementFeeType.Enabled
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDisbursementFeeType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDisbursementFeeType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDisbursementFeeType.Code
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDisbursementFee> IDisbursementFeeType.DisbursementFees
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDisbursementFeeType.AddDisbursementFee(IDisbursementFee item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDisbursementFeeType.RemoveDisbursementFee(IDisbursementFee item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
