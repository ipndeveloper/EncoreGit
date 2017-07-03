using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Enrollments.Common.Results
{
	/// <summary>
	/// Enrollment Value Result
	/// </summary>
	[DTO]
	public interface IEnrollmentValueResults
	{
		/// <summary>
		/// AutoshipOrderTypeID
		/// </summary>
		short AutoshipOrderTypeID { get; set; }
	}
}
