using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Enrollments.Common
{
	/// <summary>
	/// Enrollment Result
	/// </summary>
	[DTO]
	public interface IEnrollmentResult : IResult
	{
		/// <summary>
		/// AccountID
		/// </summary>
		int AccountID { get; set; }
		/// <summary>
		/// UserName
		/// </summary>
		string UserName { get; set; }
		/// <summary>
		/// OrderIDs
		/// </summary>
		List<int> OrderIDs { get; set; }
	}
}
