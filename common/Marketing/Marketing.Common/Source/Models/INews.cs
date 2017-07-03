using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Marketing.Common.Models
{
	/// <summary>
	/// Common interface for News.
	/// </summary>
	[ContractClass(typeof(Contracts.NewsContracts))]
	public interface INews
	{
	    #region Primitive properties
	
		/// <summary>
		/// The NewsID for this News.
		/// </summary>
		int NewsID { get; set; }
	
		/// <summary>
		/// The NewsTypeID for this News.
		/// </summary>
		short NewsTypeID { get; set; }
	
		/// <summary>
		/// The StartDateUTC for this News.
		/// </summary>
		System.DateTime StartDateUTC { get; set; }
	
		/// <summary>
		/// The EndDateUTC for this News.
		/// </summary>
		Nullable<System.DateTime> EndDateUTC { get; set; }
	
		/// <summary>
		/// The Active for this News.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsPublic for this News.
		/// </summary>
		bool IsPublic { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this News.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The MarketID for this News.
		/// </summary>
		Nullable<int> MarketID { get; set; }
	
		/// <summary>
		/// The HtmlSectionID for this News.
		/// </summary>
		Nullable<int> HtmlSectionID { get; set; }
	
		/// <summary>
		/// The IsFeatured for this News.
		/// </summary>
		bool IsFeatured { get; set; }
	
		/// <summary>
		/// The IsMobile for this News.
		/// </summary>
		bool IsMobile { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(INews))]
		internal abstract class NewsContracts : INews
		{
		    #region Primitive properties
		
			int INews.NewsID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short INews.NewsTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime INews.StartDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> INews.EndDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INews.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INews.IsPublic
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INews.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INews.MarketID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INews.HtmlSectionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INews.IsFeatured
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INews.IsMobile
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
