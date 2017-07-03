using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;
using NetSteps.Modules.Enrollments.Common.Models;

namespace NetSteps.Modules.Enrollments.Common.Results
{
	/// <summary>
	/// Enrollment Autoship Order Result
	/// </summary>
	[DTO]
	public interface IEnrollmentAutoshipOrderResult : IResult
	{
		/// <summary>
		/// AutoshipOrderID
		/// </summary>
		int AutoshipOrderID { get; set; }

		/// <summary>
		/// TemplateOrderID
		/// </summary>
		int TemplateOrderID { get; set; }
		
	}
}
