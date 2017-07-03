using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Marketing.Common.Models
{
	/// <summary>
	/// Common interface for CalendarEventAttribute.
	/// </summary>
	[ContractClass(typeof(Contracts.CalendarEventAttributeContracts))]
	public interface ICalendarEventAttribute
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CalendarEventAttributeID for this CalendarEventAttribute.
		/// </summary>
		int CalendarEventAttributeID { get; set; }
	
		/// <summary>
		/// The CalendarEventID for this CalendarEventAttribute.
		/// </summary>
		int CalendarEventID { get; set; }
	
		/// <summary>
		/// The Name for this CalendarEventAttribute.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Value for this CalendarEventAttribute.
		/// </summary>
		string Value { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The CalendarEvent for this CalendarEventAttribute.
		/// </summary>
	    ICalendarEvent CalendarEvent { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICalendarEventAttribute))]
		internal abstract class CalendarEventAttributeContracts : ICalendarEventAttribute
		{
		    #region Primitive properties
		
			int ICalendarEventAttribute.CalendarEventAttributeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICalendarEventAttribute.CalendarEventID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICalendarEventAttribute.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICalendarEventAttribute.Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICalendarEvent ICalendarEventAttribute.CalendarEvent
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
