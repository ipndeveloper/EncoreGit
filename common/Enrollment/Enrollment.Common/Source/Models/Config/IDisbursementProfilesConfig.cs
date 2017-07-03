using NetSteps.Encore.Core.IoC;
using System;

namespace NetSteps.Enrollment.Common.Models.Config
{
	public interface IDisbursementProfilesConfig
	{
		bool Hidden { get; set; }
		bool EftEnabled { get; set; }
		bool CheckEnabled { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IDisbursementProfilesConfig), RegistrationBehaviors.Default)]
	public class DisbursementProfilesConfig : IDisbursementProfilesConfig
	{
		public bool Hidden { get; set; }

		public bool EftEnabled { get; set; }

		public bool CheckEnabled { get; set; }
	}
}
