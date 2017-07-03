using NetSteps.Encore.Core.IoC;
using System;

namespace NetSteps.Enrollment.Common.Models.Config
{
	public interface IEnrollmentWebsiteConfig
	{
		int? AutoshipScheduleID { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IEnrollmentWebsiteConfig), RegistrationBehaviors.Default)]
	public class EnrollmentWebsiteConfig : IEnrollmentWebsiteConfig
	{
		public int? AutoshipScheduleID { get; set; }
	}
}
