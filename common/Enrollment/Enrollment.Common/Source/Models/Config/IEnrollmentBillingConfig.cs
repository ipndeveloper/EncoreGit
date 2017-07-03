using NetSteps.Encore.Core.IoC;
using System;

namespace NetSteps.Enrollment.Common.Models.Config
{
	public interface IEnrollmentBillingConfig
	{
		bool HideBillingAddress { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IEnrollmentBillingConfig), RegistrationBehaviors.Default)]
	public class EnrollmentBillingConfig : IEnrollmentBillingConfig
	{
		public bool HideBillingAddress { get; set; }
	}
}
