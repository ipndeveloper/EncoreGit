using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Payments.Common.Models
{
	/// <summary>
	/// Common interface for BankAccountType.
	/// </summary>
	[ContractClass(typeof(Contracts.BankAccountTypeContracts))]
	public interface IBankAccountType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The BankAccountTypeID for this BankAccountType.
		/// </summary>
		short BankAccountTypeID { get; set; }
	
		/// <summary>
		/// The Name for this BankAccountType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this BankAccountType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this BankAccountType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this BankAccountType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IBankAccountType))]
		internal abstract class BankAccountTypeContracts : IBankAccountType
		{
		    #region Primitive properties
		
			short IBankAccountType.BankAccountTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IBankAccountType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IBankAccountType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IBankAccountType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IBankAccountType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
