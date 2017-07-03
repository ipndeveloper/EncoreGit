using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for NewsletterTrackingStatistic.
	/// </summary>
	[ContractClass(typeof(Contracts.NewsletterTrackingStatisticContracts))]
	public interface INewsletterTrackingStatistic
	{
	    #region Primitive properties
	
		/// <summary>
		/// The NewsletterTrackingStatisticID for this NewsletterTrackingStatistic.
		/// </summary>
		int NewsletterTrackingStatisticID { get; set; }
	
		/// <summary>
		/// The NewsletterID for this NewsletterTrackingStatistic.
		/// </summary>
		int NewsletterID { get; set; }
	
		/// <summary>
		/// The MailMessageID for this NewsletterTrackingStatistic.
		/// </summary>
		int MailMessageID { get; set; }
	
		/// <summary>
		/// The DateUTC for this NewsletterTrackingStatistic.
		/// </summary>
		System.DateTime DateUTC { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Newsletter for this NewsletterTrackingStatistic.
		/// </summary>
	    INewsletter Newsletter { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(INewsletterTrackingStatistic))]
		internal abstract class NewsletterTrackingStatisticContracts : INewsletterTrackingStatistic
		{
		    #region Primitive properties
		
			int INewsletterTrackingStatistic.NewsletterTrackingStatisticID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int INewsletterTrackingStatistic.NewsletterID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int INewsletterTrackingStatistic.MailMessageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime INewsletterTrackingStatistic.DateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    INewsletter INewsletterTrackingStatistic.Newsletter
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
