using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for IncentiveLedgerEntryType.
	/// </summary>
	[ContractClass(typeof(Contracts.IncentiveLedgerEntryTypeContracts))]
	public interface IIncentiveLedgerEntryType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The IncentiveLedgerEntryTypeID for this IncentiveLedgerEntryType.
		/// </summary>
		int IncentiveLedgerEntryTypeID { get; set; }
	
		/// <summary>
		/// The Name for this IncentiveLedgerEntryType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Enabled for this IncentiveLedgerEntryType.
		/// </summary>
		bool Enabled { get; set; }
	
		/// <summary>
		/// The Editable for this IncentiveLedgerEntryType.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The TermName for this IncentiveLedgerEntryType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Code for this IncentiveLedgerEntryType.
		/// </summary>
		string Code { get; set; }
	
		/// <summary>
		/// The CapRewardPoints for this IncentiveLedgerEntryType.
		/// </summary>
		Nullable<int> CapRewardPoints { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IIncentiveLedgerEntryType))]
		internal abstract class IncentiveLedgerEntryTypeContracts : IIncentiveLedgerEntryType
		{
		    #region Primitive properties
		
			int IIncentiveLedgerEntryType.IncentiveLedgerEntryTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IIncentiveLedgerEntryType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IIncentiveLedgerEntryType.Enabled
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IIncentiveLedgerEntryType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IIncentiveLedgerEntryType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IIncentiveLedgerEntryType.Code
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IIncentiveLedgerEntryType.CapRewardPoints
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
