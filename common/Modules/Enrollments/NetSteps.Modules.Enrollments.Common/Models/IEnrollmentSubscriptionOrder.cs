using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;
using NetSteps.Modules.Order.Common;

namespace NetSteps.Modules.Enrollments.Common.Models
{
	/// <summary>
	/// Enrollment Subscription Order
	/// </summary>
    [DTO]
    public interface IEnrollmentSubscriptionOrder : IOrderCreate
    {
		/// <summary>
		/// AutoshipScheduleID
		/// </summary>
        int AutoshipScheduleID { get; set; }
		/// <summary>
		/// MarketID
		/// </summary>
        int MarketID { get; set; } 
		/// <summary>
		/// Hostname
		/// </summary>
        string Url { get; set;  }
    
    }
}
