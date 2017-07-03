using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for DisbursementStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.DisbursementStatusContracts))]
	public interface IDisbursementStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DisbursementStatusID for this DisbursementStatus.
		/// </summary>
		int DisbursementStatusID { get; set; }
	
		/// <summary>
		/// The Name for this DisbursementStatus.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Enabled for this DisbursementStatus.
		/// </summary>
		bool Enabled { get; set; }
	
		/// <summary>
		/// The Editable for this DisbursementStatus.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The TermName for this DisbursementStatus.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Code for this DisbursementStatus.
		/// </summary>
		string Code { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DisbursementDetails for this DisbursementStatus.
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
		/// The Disbursements for this DisbursementStatus.
		/// </summary>
		IEnumerable<IDisbursement> Disbursements { get; }
	
		/// <summary>
		/// Adds an <see cref="IDisbursement"/> to the Disbursements collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursement"/> to add.</param>
		void AddDisbursement(IDisbursement item);
	
		/// <summary>
		/// Removes an <see cref="IDisbursement"/> from the Disbursements collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursement"/> to remove.</param>
		void RemoveDisbursement(IDisbursement item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDisbursementStatus))]
		internal abstract class DisbursementStatusContracts : IDisbursementStatus
		{
		    #region Primitive properties
		
			int IDisbursementStatus.DisbursementStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDisbursementStatus.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDisbursementStatus.Enabled
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDisbursementStatus.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDisbursementStatus.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDisbursementStatus.Code
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDisbursementDetail> IDisbursementStatus.DisbursementDetails
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDisbursementStatus.AddDisbursementDetail(IDisbursementDetail item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDisbursementStatus.RemoveDisbursementDetail(IDisbursementDetail item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDisbursement> IDisbursementStatus.Disbursements
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDisbursementStatus.AddDisbursement(IDisbursement item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDisbursementStatus.RemoveDisbursement(IDisbursement item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
