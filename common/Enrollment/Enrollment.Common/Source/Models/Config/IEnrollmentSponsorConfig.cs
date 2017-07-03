using NetSteps.Encore.Core.IoC;
using System;

namespace NetSteps.Enrollment.Common.Models.Config
{
	public interface IEnrollmentSponsorConfig
	{
		bool DenySponsorChange { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IEnrollmentSponsorConfig), RegistrationBehaviors.Default)]
	public class EnrollmentSponsorConfig : IEnrollmentSponsorConfig
	{
		public bool DenySponsorChange { get; set; }
	}
}
