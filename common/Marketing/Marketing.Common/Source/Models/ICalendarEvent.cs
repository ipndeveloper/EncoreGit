using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Addresses.Common.Models;

namespace NetSteps.Marketing.Common.Models
{
	/// <summary>
	/// Common interface for CalendarEvent.
	/// </summary>
	[ContractClass(typeof(Contracts.CalendarEventContracts))]
	public interface ICalendarEvent
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CalendarEventID for this CalendarEvent.
		/// </summary>
		int CalendarEventID { get; set; }
	
		/// <summary>
		/// The CalendarEventTypeID for this CalendarEvent.
		/// </summary>
		Nullable<int> CalendarEventTypeID { get; set; }
	
		/// <summary>
		/// The CalendarCategoryID for this CalendarEvent.
		/// </summary>
		Nullable<int> CalendarCategoryID { get; set; }
	
		/// <summary>
		/// The CalendarPriorityID for this CalendarEvent.
		/// </summary>
		Nullable<int> CalendarPriorityID { get; set; }
	
		/// <summary>
		/// The CalendarStatusID for this CalendarEvent.
		/// </summary>
		Nullable<int> CalendarStatusID { get; set; }
	
		/// <summary>
		/// The CalendarColorCodingID for this CalendarEvent.
		/// </summary>
		Nullable<int> CalendarColorCodingID { get; set; }
	
		/// <summary>
		/// The ParentID for this CalendarEvent.
		/// </summary>
		Nullable<int> ParentID { get; set; }
	
		/// <summary>
		/// The AccountID for this CalendarEvent.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The StartDateUTC for this CalendarEvent.
		/// </summary>
		System.DateTime StartDateUTC { get; set; }
	
		/// <summary>
		/// The EndDateUTC for this CalendarEvent.
		/// </summary>
		Nullable<System.DateTime> EndDateUTC { get; set; }
	
		/// <summary>
		/// The ReminderDateUTC for this CalendarEvent.
		/// </summary>
		Nullable<System.DateTime> ReminderDateUTC { get; set; }
	
		/// <summary>
		/// The RecurringScheduleID for this CalendarEvent.
		/// </summary>
		Nullable<int> RecurringScheduleID { get; set; }
	
		/// <summary>
		/// The AddressID for this CalendarEvent.
		/// </summary>
		Nullable<int> AddressID { get; set; }
	
		/// <summary>
		/// The IsCorporate for this CalendarEvent.
		/// </summary>
		bool IsCorporate { get; set; }
	
		/// <summary>
		/// The IsPublic for this CalendarEvent.
		/// </summary>
		bool IsPublic { get; set; }
	
		/// <summary>
		/// The IsAllDayEvent for this CalendarEvent.
		/// </summary>
		bool IsAllDayEvent { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this CalendarEvent.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The MarketID for this CalendarEvent.
		/// </summary>
		Nullable<int> MarketID { get; set; }
	
		/// <summary>
		/// The HtmlSectionID for this CalendarEvent.
		/// </summary>
		Nullable<int> HtmlSectionID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Address for this CalendarEvent.
		/// </summary>
	    IAddress Address { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The CalendarEventAttributes for this CalendarEvent.
		/// </summary>
		IEnumerable<ICalendarEventAttribute> CalendarEventAttributes { get; }
	
		/// <summary>
		/// Adds an <see cref="ICalendarEventAttribute"/> to the CalendarEventAttributes collection.
		/// </summary>
		/// <param name="item">The <see cref="ICalendarEventAttribute"/> to add.</param>
		void AddCalendarEventAttribute(ICalendarEventAttribute item);
	
		/// <summary>
		/// Removes an <see cref="ICalendarEventAttribute"/> from the CalendarEventAttributes collection.
		/// </summary>
		/// <param name="item">The <see cref="ICalendarEventAttribute"/> to remove.</param>
		void RemoveCalendarEventAttribute(ICalendarEventAttribute item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICalendarEvent))]
		internal abstract class CalendarEventContracts : ICalendarEvent
		{
		    #region Primitive properties
		
			int ICalendarEvent.CalendarEventID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICalendarEvent.CalendarEventTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICalendarEvent.CalendarCategoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICalendarEvent.CalendarPriorityID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICalendarEvent.CalendarStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICalendarEvent.CalendarColorCodingID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICalendarEvent.ParentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICalendarEvent.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ICalendarEvent.StartDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICalendarEvent.EndDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICalendarEvent.ReminderDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICalendarEvent.RecurringScheduleID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICalendarEvent.AddressID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICalendarEvent.IsCorporate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICalendarEvent.IsPublic
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICalendarEvent.IsAllDayEvent
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICalendarEvent.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICalendarEvent.MarketID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICalendarEvent.HtmlSectionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAddress ICalendarEvent.Address
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICalendarEventAttribute> ICalendarEvent.CalendarEventAttributes
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICalendarEvent.AddCalendarEventAttribute(ICalendarEventAttribute item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICalendarEvent.RemoveCalendarEventAttribute(ICalendarEventAttribute item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
