using NetSteps.Encore.Core.IoC;
using System;

namespace NetSteps.Enrollment.Common.Models.Config
{
	public interface IEnrollmentSubscriptionAutoshipConfig
	{
		int? AutoshipScheduleID { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IEnrollmentSubscriptionAutoshipConfig), RegistrationBehaviors.Default)]
	public class EnrollmentSubscriptionAutoshipConfig : IEnrollmentSubscriptionAutoshipConfig
	{
		public int? AutoshipScheduleID { get; set; }
	}
}
