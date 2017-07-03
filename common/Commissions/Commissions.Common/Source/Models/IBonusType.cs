using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for BonusType.
	/// </summary>
	[ContractClass(typeof(Contracts.BonusTypeContracts))]
	public interface IBonusType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The BonusTypeID for this BonusType.
		/// </summary>
		int BonusTypeID { get; set; }
	
		/// <summary>
		/// The BonusCode for this BonusType.
		/// </summary>
		string BonusCode { get; set; }
	
		/// <summary>
		/// The BonusDesc for this BonusType.
		/// </summary>
		string BonusDesc { get; set; }
	
		/// <summary>
		/// The Enabled for this BonusType.
		/// </summary>
		bool Enabled { get; set; }
	
		/// <summary>
		/// The Editable for this BonusType.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The PlanID for this BonusType.
		/// </summary>
		Nullable<int> PlanID { get; set; }
	
		/// <summary>
		/// The EarningsTypeID for this BonusType.
		/// </summary>
		Nullable<int> EarningsTypeID { get; set; }
	
		/// <summary>
		/// The TermName for this BonusType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Name for this BonusType.
		/// </summary>
		string Name { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Plan for this BonusType.
		/// </summary>
	    IPlan Plan { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountLedgers for this BonusType.
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
		/// The ProductCreditLedgers for this BonusType.
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
		[ContractClassFor(typeof(IBonusType))]
		internal abstract class BonusTypeContracts : IBonusType
		{
		    #region Primitive properties
		
			int IBonusType.BonusTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IBonusType.BonusCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IBonusType.BonusDesc
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IBonusType.Enabled
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IBonusType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IBonusType.PlanID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IBonusType.EarningsTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IBonusType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IBonusType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IPlan IBonusType.Plan
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountLedger> IBonusType.AccountLedgers
			{
				get { throw new NotImplementedException(); }
			}
		
			void IBonusType.AddAccountLedger(IAccountLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IBonusType.RemoveAccountLedger(IAccountLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductCreditLedger> IBonusType.ProductCreditLedgers
			{
				get { throw new NotImplementedException(); }
			}
		
			void IBonusType.AddProductCreditLedger(IProductCreditLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IBonusType.RemoveProductCreditLedger(IProductCreditLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
