using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Locale.Common.Models
{
	/// <summary>
	/// Common interface for Currency.
	/// </summary>
	[ContractClass(typeof(Contracts.CurrencyContracts))]
	public interface ICurrency
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CurrencyID for this Currency.
		/// </summary>
		int CurrencyID { get; set; }
	
		/// <summary>
		/// The CurrencyCode for this Currency.
		/// </summary>
		string CurrencyCode { get; set; }
	
		/// <summary>
		/// The Name for this Currency.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The CurrencySymbol for this Currency.
		/// </summary>
		string CurrencySymbol { get; set; }
	
		/// <summary>
		/// The TermName for this Currency.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The CultureInfo for this Currency.
		/// </summary>
		string CultureInfo { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICurrency))]
		internal abstract class CurrencyContracts : ICurrency
		{
		    #region Primitive properties
		
			int ICurrency.CurrencyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICurrency.CurrencyCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICurrency.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICurrency.CurrencySymbol
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICurrency.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICurrency.CultureInfo
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
