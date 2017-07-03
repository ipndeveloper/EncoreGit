using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for Newsletter.
	/// </summary>
	[ContractClass(typeof(Contracts.NewsletterContracts))]
	public interface INewsletter
	{
	    #region Primitive properties
	
		/// <summary>
		/// The NewsletterID for this Newsletter.
		/// </summary>
		int NewsletterID { get; set; }
	
		/// <summary>
		/// The MarketID for this Newsletter.
		/// </summary>
		int MarketID { get; set; }
	
		/// <summary>
		/// The LanguageID for this Newsletter.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The Name for this Newsletter.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The DateSentUTC for this Newsletter.
		/// </summary>
		Nullable<System.DateTime> DateSentUTC { get; set; }
	
		/// <summary>
		/// The Active for this Newsletter.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The ScheduledSendDateUTC for this Newsletter.
		/// </summary>
		System.DateTime ScheduledSendDateUTC { get; set; }
	
		/// <summary>
		/// The ConsultantEditableUTC for this Newsletter.
		/// </summary>
		Nullable<System.DateTime> ConsultantEditableUTC { get; set; }
	
		/// <summary>
		/// The IsProcessing for this Newsletter.
		/// </summary>
		bool IsProcessing { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The NewsletterTrackingStatistics for this Newsletter.
		/// </summary>
		IEnumerable<INewsletterTrackingStatistic> NewsletterTrackingStatistics { get; }
	
		/// <summary>
		/// Adds an <see cref="INewsletterTrackingStatistic"/> to the NewsletterTrackingStatistics collection.
		/// </summary>
		/// <param name="item">The <see cref="INewsletterTrackingStatistic"/> to add.</param>
		void AddNewsletterTrackingStatistic(INewsletterTrackingStatistic item);
	
		/// <summary>
		/// Removes an <see cref="INewsletterTrackingStatistic"/> from the NewsletterTrackingStatistics collection.
		/// </summary>
		/// <param name="item">The <see cref="INewsletterTrackingStatistic"/> to remove.</param>
		void RemoveNewsletterTrackingStatistic(INewsletterTrackingStatistic item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(INewsletter))]
		internal abstract class NewsletterContracts : INewsletter
		{
		    #region Primitive properties
		
			int INewsletter.NewsletterID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int INewsletter.MarketID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int INewsletter.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INewsletter.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> INewsletter.DateSentUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INewsletter.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime INewsletter.ScheduledSendDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> INewsletter.ConsultantEditableUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INewsletter.IsProcessing
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<INewsletterTrackingStatistic> INewsletter.NewsletterTrackingStatistics
			{
				get { throw new NotImplementedException(); }
			}
		
			void INewsletter.AddNewsletterTrackingStatistic(INewsletterTrackingStatistic item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void INewsletter.RemoveNewsletterTrackingStatistic(INewsletterTrackingStatistic item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
