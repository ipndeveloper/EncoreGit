using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for CurrencyType.
	/// </summary>
	[ContractClass(typeof(Contracts.CurrencyTypeContracts))]
	public interface ICurrencyType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CurrencyTypeID for this CurrencyType.
		/// </summary>
		int CurrencyTypeID { get; set; }
	
		/// <summary>
		/// The CurrencyCode for this CurrencyType.
		/// </summary>
		string CurrencyCode { get; set; }
	
		/// <summary>
		/// The Name for this CurrencyType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The DecimalPlaces for this CurrencyType.
		/// </summary>
		int DecimalPlaces { get; set; }
	
		/// <summary>
		/// The TermName for this CurrencyType.
		/// </summary>
		string TermName { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountLedgers for this CurrencyType.
		/// </summary>
		IEnumerable<IAccountLedger> AccountLedgers { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountLedger"/> to the AccountLedgers collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountLedger"/> to add.</param>
		void AddAccountLedger(IAccountLedger item);
	
		/// <summary>
		/// Removes an <see cref="IAccountLedger"/> from the AccountLedgers collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountLedger"/> to remove.</param>
		void RemoveAccountLedger(IAccountLedger item);
	
		/// <summary>
		/// The DisbursementFees for this CurrencyType.
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
	
		/// <summary>
		/// The DisbursementMinimums for this CurrencyType.
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
		/// The Disbursements for this CurrencyType.
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
	
		/// <summary>
		/// The ProductCreditLedgers for this CurrencyType.
		/// </summary>
		IEnumerable<IProductCreditLedger> ProductCreditLedgers { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductCreditLedger"/> to the ProductCreditLedgers collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductCreditLedger"/> to add.</param>
		void AddProductCreditLedger(IProductCreditLedger item);
	
		/// <summary>
		/// Removes an <see cref="IProductCreditLedger"/> from the ProductCreditLedgers collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductCreditLedger"/> to remove.</param>
		void RemoveProductCreditLedger(IProductCreditLedger item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICurrencyType))]
		internal abstract class CurrencyTypeContracts : ICurrencyType
		{
		    #region Primitive properties
		
			int ICurrencyType.CurrencyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICurrencyType.CurrencyCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICurrencyType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICurrencyType.DecimalPlaces
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICurrencyType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountLedger> ICurrencyType.AccountLedgers
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICurrencyType.AddAccountLedger(IAccountLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICurrencyType.RemoveAccountLedger(IAccountLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDisbursementFee> ICurrencyType.DisbursementFees
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICurrencyType.AddDisbursementFee(IDisbursementFee item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICurrencyType.RemoveDisbursementFee(IDisbursementFee item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDisbursementMinimum> ICurrencyType.DisbursementMinimums
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICurrencyType.AddDisbursementMinimum(IDisbursementMinimum item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICurrencyType.RemoveDisbursementMinimum(IDisbursementMinimum item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDisbursement> ICurrencyType.Disbursements
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICurrencyType.AddDisbursement(IDisbursement item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICurrencyType.RemoveDisbursement(IDisbursement item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductCreditLedger> ICurrencyType.ProductCreditLedgers
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICurrencyType.AddProductCreditLedger(IProductCreditLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICurrencyType.RemoveProductCreditLedger(IProductCreditLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
