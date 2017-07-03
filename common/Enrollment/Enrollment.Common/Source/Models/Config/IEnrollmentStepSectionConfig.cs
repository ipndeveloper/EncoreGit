using NetSteps.Encore.Core.IoC;
using System;

namespace NetSteps.Enrollment.Common.Models.Config
{
	public interface IEnrollmentStepSectionConfig
	{
		string Name { get; set; }
		string TermName { get; set; }
		string Action { get; set; }
		bool Skippable { get; set; }
		bool Completed { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IEnrollmentStepSectionConfig), RegistrationBehaviors.Default)]
	public class EnrollmentStepSectionConfig : IEnrollmentStepSectionConfig
	{
		public string Name { get; set; }
		public string TermName { get; set; }
		public string Action { get; set; }
		public bool Skippable { get; set; }
		public bool Completed { get; set; }
	}
}
