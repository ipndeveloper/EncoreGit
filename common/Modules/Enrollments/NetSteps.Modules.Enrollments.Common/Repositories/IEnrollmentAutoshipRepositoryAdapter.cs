using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Modules.Enrollments.Common.Results;
using NetSteps.Modules.Enrollments.Common.Models;

namespace NetSteps.Modules.Enrollments.Common
{    
	/// <summary>
	/// Enrollment Autoship Adapter
	/// </summary>
    public interface IEnrollmentAutoshipRepositoryAdapter
    {
		/// <summary>
		/// Create a new Autoship Order
		/// </summary>
        /// <param name="subscriptionOrder"></param>
		/// <returns></returns>
        IEnrollmentAutoshipOrderResult CreateAutoshipScheduleAndOrder(IEnrollmentSubscriptionOrder subscriptionOrder);
    }
}
