using NetSteps.Encore.Core.IoC;
using System;

namespace NetSteps.Enrollment.Common.Models.Config
{
	public interface IEnrollmentOrderConfig
	{
		bool ImportShoppingOrder { get; set; }
		bool SaveAsAutoshipOrder { get; set; }
		decimal? MinimumCommissionableTotal { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IEnrollmentOrderConfig), RegistrationBehaviors.Default)]
	public class EnrollmentOrderConfig : IEnrollmentOrderConfig
	{
		public bool ImportShoppingOrder { get; set; }
		public bool SaveAsAutoshipOrder { get; set; }
		public decimal? MinimumCommissionableTotal { get; set; }
	}
}
