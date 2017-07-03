using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Payments.Common.Models
{
	/// <summary>
	/// Common interface for CreditCardType.
	/// </summary>
	[ContractClass(typeof(Contracts.CreditCardTypeContracts))]
	public interface ICreditCardType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CreditCardTypeID for this CreditCardType.
		/// </summary>
		short CreditCardTypeID { get; set; }
	
		/// <summary>
		/// The Name for this CreditCardType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this CreditCardType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Editable for this CreditCardType.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The Active for this CreditCardType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The TermName for this CreditCardType.
		/// </summary>
		string TermName { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICreditCardType))]
		internal abstract class CreditCardTypeContracts : ICreditCardType
		{
		    #region Primitive properties
		
			short ICreditCardType.CreditCardTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICreditCardType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICreditCardType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICreditCardType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICreditCardType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICreditCardType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
