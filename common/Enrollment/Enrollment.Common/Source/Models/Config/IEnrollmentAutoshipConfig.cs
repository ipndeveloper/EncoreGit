using NetSteps.Encore.Core.IoC;
using System;

namespace NetSteps.Enrollment.Common.Models.Config
{
	public interface IEnrollmentAutoshipConfig
	{
		int? AutoshipScheduleID { get; set; }
		bool ImportShoppingOrder { get; set; }
		bool Hidden { get; set; }
		decimal? MinimumCommissionableTotal { get; set; }
		bool Skippable { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IEnrollmentAutoshipConfig), RegistrationBehaviors.Default)]
	public class EnrollmentAutoshipConfig : IEnrollmentAutoshipConfig
	{
		public int? AutoshipScheduleID { get; set; }

		public bool ImportShoppingOrder { get; set; }

		public bool Hidden { get; set; }

		public decimal? MinimumCommissionableTotal { get; set; }

		public bool Skippable { get; set; }
	}
}
