using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Modules.Enrollments.Common
{   
 	/// <summary>
 	/// Payment Repository Adapter
 	/// </summary>
    public interface IPaymentRepositoryAdapter
    {
		/// <summary>
		/// Submit Order Payments
		/// </summary>
		/// <param name="orderID"></param>
		/// <returns></returns>
        IPaymentResponse SubmitPayments(int orderID);
    }
}
