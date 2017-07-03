using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Sites.Common.Models
{
	/// <summary>
	/// Common interface for Site.
	/// </summary>
	[ContractClass(typeof(Contracts.SiteContracts))]
	public interface ISite
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SiteID for this Site.
		/// </summary>
		int SiteID { get; set; }
	
		/// <summary>
		/// The BaseSiteID for this Site.
		/// </summary>
		Nullable<int> BaseSiteID { get; set; }
	
		/// <summary>
		/// The SiteTypeID for this Site.
		/// </summary>
		short SiteTypeID { get; set; }
	
		/// <summary>
		/// The SiteStatusID for this Site.
		/// </summary>
		short SiteStatusID { get; set; }
	
		/// <summary>
		/// The Name for this Site.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The DisplayName for this Site.
		/// </summary>
		string DisplayName { get; set; }
	
		/// <summary>
		/// The Description for this Site.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The AccountID for this Site.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The AccountNumber for this Site.
		/// </summary>
		string AccountNumber { get; set; }
	
		/// <summary>
		/// The DateSignedUpUTC for this Site.
		/// </summary>
		System.DateTime DateSignedUpUTC { get; set; }
	
		/// <summary>
		/// The MarketID for this Site.
		/// </summary>
		int MarketID { get; set; }
	
		/// <summary>
		/// The IsBase for this Site.
		/// </summary>
		bool IsBase { get; set; }
	
		/// <summary>
		/// The DefaultLanguageID for this Site.
		/// </summary>
		int DefaultLanguageID { get; set; }
	
		/// <summary>
		/// The SiteStatusChangeReasonID for this Site.
		/// </summary>
		Nullable<short> SiteStatusChangeReasonID { get; set; }
	
		/// <summary>
		/// The AutoshipOrderID for this Site.
		/// </summary>
		Nullable<int> AutoshipOrderID { get; set; }
	
		/// <summary>
		/// The DateCreatedUTC for this Site.
		/// </summary>
		System.DateTime DateCreatedUTC { get; set; }
	
		/// <summary>
		/// The CreatedByUserID for this Site.
		/// </summary>
		Nullable<int> CreatedByUserID { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this Site.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The DateLastModifiedUTC for this Site.
		/// </summary>
		Nullable<System.DateTime> DateLastModifiedUTC { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ISite))]
		internal abstract class SiteContracts : ISite
		{
		    #region Primitive properties
		
			int ISite.SiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISite.BaseSiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ISite.SiteTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ISite.SiteStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISite.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISite.DisplayName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISite.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISite.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISite.AccountNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ISite.DateSignedUpUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISite.MarketID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISite.IsBase
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISite.DefaultLanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> ISite.SiteStatusChangeReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISite.AutoshipOrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ISite.DateCreatedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISite.CreatedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISite.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ISite.DateLastModifiedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
