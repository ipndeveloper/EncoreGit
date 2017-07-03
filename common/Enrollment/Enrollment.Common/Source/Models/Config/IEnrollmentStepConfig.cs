using NetSteps.Common.Base;
using NetSteps.Encore.Core.IoC;
using System;

namespace NetSteps.Enrollment.Common.Models.Config
{
	public interface IEnrollmentStepConfig
	{
		string Name { get; set; }
		string TermName { get; set; }
		string Controller { get; set; }
		bool Skippable { get; set; }
		OrderedList<IEnrollmentStepSectionConfig> Sections { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IEnrollmentStepConfig), RegistrationBehaviors.Default)]
	public class EnrollmentStepConfig : IEnrollmentStepConfig
	{
		public string Name { get; set; }
		public string TermName { get; set; }
		public string Controller { get; set; }
		public bool Skippable { get; set; }
		public OrderedList<IEnrollmentStepSectionConfig> Sections { get; set; }
	}
}
