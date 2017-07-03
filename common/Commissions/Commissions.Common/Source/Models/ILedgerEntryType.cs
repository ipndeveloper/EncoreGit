using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for LedgerEntryType.
	/// </summary>
	[ContractClass(typeof(Contracts.LedgerEntryTypeContracts))]
	public interface ILedgerEntryType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The LedgerEntryTypeID for this LedgerEntryType.
		/// </summary>
		int LedgerEntryTypeID { get; set; }
	
		/// <summary>
		/// The Enabled for this LedgerEntryType.
		/// </summary>
		bool Enabled { get; set; }
	
		/// <summary>
		/// The Editable for this LedgerEntryType.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The Name for this LedgerEntryType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Taxable for this LedgerEntryType.
		/// </summary>
		bool Taxable { get; set; }
	
		/// <summary>
		/// The TermName for this LedgerEntryType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Code for this LedgerEntryType.
		/// </summary>
		string Code { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountLedgers for this LedgerEntryType.
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
		/// The ProductCreditLedgers for this LedgerEntryType.
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
		[ContractClassFor(typeof(ILedgerEntryType))]
		internal abstract class LedgerEntryTypeContracts : ILedgerEntryType
		{
		    #region Primitive properties
		
			int ILedgerEntryType.LedgerEntryTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ILedgerEntryType.Enabled
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ILedgerEntryType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILedgerEntryType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ILedgerEntryType.Taxable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILedgerEntryType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILedgerEntryType.Code
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountLedger> ILedgerEntryType.AccountLedgers
			{
				get { throw new NotImplementedException(); }
			}
		
			void ILedgerEntryType.AddAccountLedger(IAccountLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ILedgerEntryType.RemoveAccountLedger(IAccountLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductCreditLedger> ILedgerEntryType.ProductCreditLedgers
			{
				get { throw new NotImplementedException(); }
			}
		
			void ILedgerEntryType.AddProductCreditLedger(IProductCreditLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ILedgerEntryType.RemoveProductCreditLedger(IProductCreditLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
