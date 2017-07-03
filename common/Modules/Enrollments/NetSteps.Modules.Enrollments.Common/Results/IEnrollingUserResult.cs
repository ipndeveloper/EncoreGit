using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Enrollments.Common
{
	/// <summary>
	/// Create User Result
	/// </summary>
	[DTO]
	public interface IEnrollingUserResult : IResult
	{
		/// <summary>
		/// User Name
		/// </summary>
		string UserName { get; set; }
	}
}
