using System.Web.Mvc;
using NetSteps.Web.Mvc.Controls.Controllers.Enrollment;

namespace nsCore.Areas.Enrollment
{
	public class EnrollmentAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Enrollment";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Enrollment_Index",
				"Enrollment",
				new { controller = "Enrollment", action = "Index" }
			);

            context.MapRoute("PlacementValidation", "Enrollment/PlacementValidation", new { controller = "Enrollment", action = "PlacementValidation" });
            context.MapRoute("DocumentValidation", "Enrollment/DocumentValidation", new { controller = "Enrollment", action = "DocumentValidation" });
            context.MapRoute("UserNameValidation", "Enrollment/UserNameValidation", new { controller = "Enrollment", action = "UserNameValidation" });
            context.MapRoute("EmailValidation", "Enrollment/EmailValidation", new { controller = "Enrollment", action = "EmailValidation" });
            context.MapRoute(
                EnrollmentStep.StepActionRouteName,
                "Enrollment/{step}/{stepAction}",
                new { controller = "Enrollment", action = "StepAction", stepAction = EnrollmentStep.DefaultStepAction }
            );

		}
	}
}
