using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Locale.Common.Models
{
	/// <summary>
	/// Common interface for Country.
	/// </summary>
	[ContractClass(typeof(Contracts.CountryContracts))]
	public interface ICountry
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CountryID for this Country.
		/// </summary>
		int CountryID { get; set; }
	
		/// <summary>
		/// The MarketID for this Country.
		/// </summary>
		int MarketID { get; set; }
	
		/// <summary>
		/// The CountryCode for this Country.
		/// </summary>
		string CountryCode { get; set; }
	
		/// <summary>
		/// The Name for this Country.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The CurrencyID for this Country.
		/// </summary>
		int CurrencyID { get; set; }
	
		/// <summary>
		/// The TermName for this Country.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The DefaultLanguageID for this Country.
		/// </summary>
		Nullable<int> DefaultLanguageID { get; set; }
	
		/// <summary>
		/// The Description for this Country.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this Country.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsAvailableForRegistration for this Country.
		/// </summary>
		bool IsAvailableForRegistration { get; set; }
	
		/// <summary>
		/// The CultureInfo for this Country.
		/// </summary>
		string CultureInfo { get; set; }
	
		/// <summary>
		/// The CountryCode3 for this Country.
		/// </summary>
		string CountryCode3 { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICountry))]
		internal abstract class CountryContracts : ICountry
		{
		    #region Primitive properties
		
			int ICountry.CountryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICountry.MarketID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICountry.CountryCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICountry.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICountry.CurrencyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICountry.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICountry.DefaultLanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICountry.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICountry.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICountry.IsAvailableForRegistration
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICountry.CultureInfo
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICountry.CountryCode3
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
