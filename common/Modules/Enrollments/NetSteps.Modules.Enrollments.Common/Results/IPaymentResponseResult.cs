using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Enrollments.Common
{
	/// <summary>
	/// Payment Response Result
	/// </summary>
    [DTO]
    public interface IPaymentResponseResult
    {
		/// <summary>
		/// FailureCount
		/// </summary>
        int FailureCount { get; set; }
		/// <summary>
		/// Message
		/// </summary>
        string Message { get; set; }
		/// <summary>
		/// Success
		/// </summary>
        bool Success { get; set; }
    }
}
