using NetSteps.Encore.Core.IoC;
using System;

namespace NetSteps.Enrollment.Common.Models.Config
{
	public interface IEnrollmentBasicInfoConfig
	{
		bool SetShippingAddressFromMain { get; set; }
		bool SetBillingAddressFromMain { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IEnrollmentBasicInfoConfig), RegistrationBehaviors.Default)]
	public class EnrollmentBasicInfoConfig : IEnrollmentBasicInfoConfig
	{
		public bool SetShippingAddressFromMain { get; set; }

		public bool SetBillingAddressFromMain { get; set; }
	}
}
