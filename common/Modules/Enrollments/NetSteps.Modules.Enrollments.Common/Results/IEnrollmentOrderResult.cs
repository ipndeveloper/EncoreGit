using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Enrollments.Common
{
	/// <summary>
	/// Enrollment Order Result
	/// </summary>
	[DTO]
	public interface IEnrollmentOrderResult : IResult
	{
		/// <summary>
		/// OrderID
		/// </summary>
		int OrderID { get; set; }
	}
}
