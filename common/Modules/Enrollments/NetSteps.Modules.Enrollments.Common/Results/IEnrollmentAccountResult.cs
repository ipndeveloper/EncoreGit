using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Enrollments.Common
{
	/// <summary>
	/// Create Account Result
	/// </summary>
	[DTO]
	public interface IEnrollmentAccountResult : IResult
	{
		/// <summary>
		/// AccountID
		/// </summary>
		int AccountID { get; set; }
	}
}
