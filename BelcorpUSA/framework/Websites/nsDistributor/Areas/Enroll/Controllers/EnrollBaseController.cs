using System;
using System.Web.Mvc;
using NetSteps.Common.Globalization;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common;
using NetSteps.Enrollment.Common.Configuration;
using NetSteps.Enrollment.Common.Models.Config;
using nsDistributor.Controllers;

namespace nsDistributor.Areas.Enroll.Controllers
{
	// All enrollment actions default to "DontCache" but the cache settings
	// can be overridden on derived controllers or on individual actions - Lundy
	[OutputCache(CacheProfile = "DontCache")]
	public abstract class EnrollBaseController : BaseOrderContextController
	{
		private readonly Lazy<IEnrollmentConfigurationProvider> _enrollmentConfigurationProviderFactory = new Lazy<IEnrollmentConfigurationProvider>(Create.New<IEnrollmentConfigurationProvider>);
		protected virtual IEnrollmentConfigurationProvider EnrollmentConfigurationProvider
		{
			get { return _enrollmentConfigurationProviderFactory.Value; }
		}

		private readonly Lazy<IEnrollmentService> _enrollmentServiceFactory = new Lazy<IEnrollmentService>(Create.New<IEnrollmentService>);
		protected virtual IEnrollmentService EnrollmentService
		{
			get { return _enrollmentServiceFactory.Value; }
		}

		protected virtual ActionResult RedirectToStep(IEnrollmentStepConfig stepConfig)
		{
			return RedirectToAction(string.Empty, stepConfig.Controller);
		}

		protected virtual string GetLocalizedStepName(IEnrollmentStepConfig stepConfig)
		{
			return Translation.GetTerm(
				 stepConfig.TermName,
				 stepConfig.Name
			);
		}

		protected virtual string GetStepUrl(IEnrollmentStepConfig stepConfig)
		{
			return Url.Action(string.Empty, stepConfig.Controller);
		}

		protected virtual int GetDefaultSponsorID()
		{
			return CurrentSite.AccountID ?? GetCorporateSponsorID();
		}

		protected virtual int GetCorporateSponsorID()
		{
			return EnrollmentService.GetCorporateSponsorID();
		}

        /*CSTI(CS)-05/03/2016-Inicio*/
        protected virtual ActionResult RedirectToStep(IEnrollmentStepConfig stepConfig, bool MVCAutomation = true)
        {
            if (MVCAutomation)
            {
                return RedirectToAction(string.Empty, stepConfig.Controller);
            }
            else
            {
                return Json(new { TypeRedirect = 1, RouteValueDictionary = new { Action = string.Empty, Controller = stepConfig.Controller } });
            }
        }
        /*CSTI(CS)-05/03/2016-Fin*/
	}
}
