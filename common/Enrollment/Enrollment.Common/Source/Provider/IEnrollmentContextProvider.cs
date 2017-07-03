using NetSteps.Enrollment.Common.Models.Context;

namespace NetSteps.Enrollment.Common.Provider
{
	public interface IEnrollmentContextProvider
	{
		IEnrollmentContext GetEnrollmentContext();
	}
}
