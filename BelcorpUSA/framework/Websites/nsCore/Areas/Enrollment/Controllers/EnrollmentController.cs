using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Configuration;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Controllers.Enrollment;
using NetSteps.Web.Mvc.Controls.Infrastructure;
using nsCore.Controllers;
using NetSteps.Data.Entities.Business.Logic;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace nsCore.Areas.Enrollment.Controllers
{
    public class EnrollmentController : BaseController
    {
		private Lazy<IEnrollmentConfigurationProvider> _configurationProviderFactory = new Lazy<IEnrollmentConfigurationProvider>(Create.New<IEnrollmentConfigurationProvider>);
		protected virtual IEnrollmentConfigurationProvider ConfigurationProvider
		{
			get { return _configurationProviderFactory.Value; }
		}

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts", "~/Sites")]
        public virtual ActionResult Index(
            int? sponsorId,
            short? acctTypeId,
            bool? isEntity,
            bool? continueEnrollment,
            IEnrollmentContext enrollmentContext)
        {
            EnrollmentConfigHandler.WebSiteAssembly = Assembly.GetExecutingAssembly();


            //Borrar la session HttpContext.Session["EsBusquedaCep"]
            HttpContext.Session["EsBusquedaCep"] = null;//Variable de session donde se asigna el tipo de busqueda de sponsor (es usado en BrowseController)

            if (!(continueEnrollment ?? false))
            {
                enrollmentContext.Clear();
            }

            if (sponsorId.HasValue)
            {
                enrollmentContext.SponsorID = sponsorId;
            }

            if (acctTypeId.HasValue)
            {
				var enrollmentConfig = ConfigurationProvider.GetEnrollmentConfig(acctTypeId.Value, ApplicationContext.Instance.SiteTypeID);
				enrollmentContext.Initialize(enrollmentConfig, (int)Constants.Country.UnitedStates, (int)Constants.Language.English, ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID));
            }

            // IsEntity only applies to distributors (added >= 1000 to allow custom types to be entities - trotter)
            if (enrollmentContext.AccountTypeID == (short)Constants.AccountType.Distributor || enrollmentContext.AccountTypeID >= 1000)
            {
                if (isEntity.HasValue)
                {
                    enrollmentContext.IsEntity = isEntity.Value;
                }
            }
            else
            {
                enrollmentContext.IsEntity = false;
            }

            if (enrollmentContext.AccountTypeID == (short)Constants.AccountType.NotSet)
            {
                return View("EnrollmentType");
            }

            string stepName = enrollmentContext.EnrollmentConfig.Steps.CurrentItem.Controller;
            var stepResult = StepAction(stepName, EnrollmentStep.DefaultStepAction, enrollmentContext);
            ViewData["DisplayUsernameField"] = GetAuthUIService().GetConfiguration().ShowUsernameFormFields;
            TempData["getEBanks"] = NetSteps.Data.Entities.Business.GeneralLedger.GetEntity();

            //Shows or Hides Second Disbursement Profile Account 
          
          
            var mark =  Market.Repository.FirstOrDefault(x => x.Active == true);
            //ViewBag.DPA = OrderExtensions.GeneralParameterVal(56, "DPA");
            ViewBag.DPA = OrderExtensions.GeneralParameterVal(mark == null ? 0 : mark.MarketID, "DPA");

            if (stepResult is PartialViewResult)
            {
                var partialViewResult = stepResult as PartialViewResult;
                ViewBag.AccountTypeName = SmallCollectionCache.Instance.AccountTypes.GetById((short)enrollmentContext.AccountTypeID).GetTerm();
                ViewBag.PartialViewName = partialViewResult.ViewName;
                ViewBag.PartialViewModel = partialViewResult.Model;
                return View();
            }

            return stepResult;
        }

        /// <summary>
        /// This is the main entry point for all enrollment requests.
        /// </summary>
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult StepAction(string step, string stepAction, IEnrollmentContext enrollmentContext)
        {
            // Check if steps are in session
            if (enrollmentContext.EnrollmentConfig == null
                || enrollmentContext.EnrollmentConfig.Steps == null)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new { result = false, message = _errorSessionExpiredString });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            try
            {
                var stepConfig = enrollmentContext.EnrollmentConfig.Steps.FirstOrDefault(x => x.Controller.Equals(step, StringComparison.OrdinalIgnoreCase));

                if (stepConfig == null)
                {
                    return HttpNotFound();
                }
                
                EnrollmentStep stepObj = EnrollmentStep.GetStep(stepConfig, this, enrollmentContext);
                return stepObj.ExecuteAction(stepAction);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

                if (Request.IsAjaxRequest())
                {
                    return Json(new { result = false, message = exception.PublicMessage });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }

        #region Strings
        protected string _errorSessionExpiredString { get { return Translation.GetTerm("YourSessionExpired"); } }
        #endregion

        #region [Validations]

        public ActionResult PlacementValidation(int PlacementID)
        {
            bool result = false;
            string message = string.Empty;
            Tuple<bool,string >tplResultado= null;
            try
            {
                tplResultado = AccountSponsorBusinessLogic.Instance.ValidateSponsorShipRules(PlacementID, Periods.GetOpenPeriodID());

                if (!tplResultado .Item1)
                    message =string.Format("{0}:{1}", Translation.GetTerm("NotValidatedPlacement", "Selected Placement did not pass the Sponsorship Rules validation"),tplResultado.Item2);

            }
            catch (Exception ex)
            {
                message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;
            }

            return Json(new { result = tplResultado.Item1, message = message });
        }

        public ActionResult UserNameValidation(string UserName)
        {
            bool result = false;
            string message = string.Empty;

            try
            {
                result = new User().IsUsernameAvailable(UserName);

                if (!result)
                    message = Translation.GetTerm("UserNameisNotAvailablePleaseEnteraDifferentUsername", "User name is not available. Please enter a different Username.");

            }
            catch (Exception ex)
            {
                message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;
            }

            return Json(new { result = result, message = message });
        }

        public ActionResult DocumentValidation(int DocumentType, string DocumentValue)
        {
            bool result = false;
            string message = string.Empty;

            try
            {
                switch (DocumentType)
                {
                    case 8: //CPF
                        switch (swValidarCPF(DocumentValue))
                        {
                            case 1: message = Translation.GetTerm("CPFCOIsRegistered", "Provided CPF already in use."); break;
                            case 3: message = Translation.GetTerm("CPFCOisInvalid", "Incorrect CPF value."); break;
                            default: result = true; break;
                        }
                        break;
                    case 9: //PIS
                        switch (swValidarPIS(DocumentValue))
                        {
                            case 1: message = Translation.GetTerm("PisIsRegistered", "Provided PIS already in use."); break;
                            case 3: message = Translation.GetTerm("PISisInvalid", "Incorrect PIS value."); break;
                            default: result = true; break;
                        }
                        break;
                    case 4: //RG
                        switch (swValidarRG(DocumentValue))
                        {
                            case 1: message = Translation.GetTerm("RGReq", "RG is required."); break;
                            case 2: message = Translation.GetTerm("RGIsRegistered", "Provided RG already in use."); break;
                            default: result = true; break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;
            }

            return Json(new { result = result, message = message });
        }

        public ActionResult EmailValidation(string email)
        {
            bool result = false;
            string message = string.Empty;

            try
            {
                Regex RxEmail = new Regex(@"^$|^([\w\-\.]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$");

                if (!RxEmail.IsMatch(email))
                {
                    message = Translation.GetTerm("EmailAccountInvalid", "Provided e-mail is invalid.");
                }
                else
                {
                    result = MailAccount.IsAvailable(email);

                    if (!result)
                        message = Translation.GetTerm("EmailAccountAlreadyExists", "An account with this e-mail already exists.");
                }
            }
            catch (Exception ex)
            {
                message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;
            }

            return Json(new { result = result, message = message });
        }

        #endregion

        #region [Document Validations]

        public int swValidarCPF(string CPFTextoInput)
        {
            int returnInt = 0;
            CPFTextoInput = (CPFTextoInput == null ? "" : CPFTextoInput);

            Boolean Resulado = true;
            if (CPFTextoInput.Length < 11 || CPFTextoInput.Length < 9)
                Resulado = false;


            Dictionary<string, string> dcResultado = new Dictionary<string, string>();
            string NuevePrimerosDigitos = CPFTextoInput.Substring(0, 9);

            string PrimerDigito = string.Empty;

            string SegundoDigito = string.Empty;
            if (CPFTextoInput.Length > 9)
            {
                SegundoDigito = CPFTextoInput.Substring(10, 1);
                PrimerDigito = CPFTextoInput.Substring(9, 1);
            }

            int PrimerDigitoValidar = ValidarPrimerDigito(NuevePrimerosDigitos);
            int SegundoDigitoValidar = ValidarSegundoDigito(NuevePrimerosDigitos + PrimerDigitoValidar.ToString());

            if (CPFTextoInput.Length > 9)
            {
                Resulado = (Convert.ToByte(PrimerDigito) == PrimerDigitoValidar & Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);


            }
            if (Resulado)
            {
                dcResultado = AccountExtensions.ValidarExistenciaCPF(NuevePrimerosDigitos + PrimerDigitoValidar + SegundoDigitoValidar);
                if (dcResultado.Count > 0)
                {
                    returnInt = 1;
                }
                else
                {
                    returnInt = 2;
                }

            }
            else
            {
                returnInt = 3;
            }

            return returnInt;

        }
        public int swValidarPIS(string TextoInputPIS)
        {
            int returnInt = 0;
            Boolean Resultado = false;

            TextoInputPIS = TextoInputPIS == null ? "" : TextoInputPIS.Trim();

            if (string.IsNullOrEmpty(TextoInputPIS))
                return 2;

            if (TextoInputPIS.Length < 11 || TextoInputPIS.Length < 9)
                Resultado = false;


            string NuevePrimerosDigitos = TextoInputPIS.Substring(0, 9);
            string PrimerDigito = TextoInputPIS.Substring(9, 1);
            string SegundoDigito = TextoInputPIS.Substring(10, 1);
            int SegundoDigitoValidar = ValidarSegundoDigitoPIS(NuevePrimerosDigitos + PrimerDigito.ToString());

            Resultado = (Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);
            if (Resultado)
            {
                Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaPIS(NuevePrimerosDigitos + PrimerDigito.ToString() + SegundoDigitoValidar.ToString());

                if (dcResultado.Count > 0)
                {
                    returnInt = 1;
                }
                else
                {
                    returnInt = 2;
                }
            }
            else
            {
                returnInt = 3;
            }

            return returnInt;
        }
        public int swValidarRG(string TextoRG)
        {
            int returnInt = 0;
            
            if (string.IsNullOrEmpty(TextoRG))
            {
                returnInt = 1;
            }
            else{
            
                Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaRG(TextoRG);

                if (dcResultado.Count > 1)
                {
                    returnInt = 2;
                }
                else
                {
                    returnInt = 3;
                }
            }
            
            return returnInt;
        }

        #region validaciones PIS
        static bool ValidarPIS(string TextoInput)
        {
            if (TextoInput.Length < 11 || TextoInput == "")
                return false;

            string NuevePrimerosDigitos = TextoInput.Substring(0, 9);
            string PrimerDigito = TextoInput.Substring(9, 1);
            string SegundoDigito = TextoInput.Substring(10, 1);
            int SegundoDigitoValidar = ValidarSegundoDigitoPIS(NuevePrimerosDigitos + PrimerDigito.ToString());

            return (Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);
        }

        static int ValidarSegundoDigitoPIS(string TextoValidar)
        {
            int[] Multiplicadores = { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[10];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }
            total = Resultados.Sum();
            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }
        #endregion

        #region validaciones CPF
        static bool ValidarCPF(string TextoInput)
        {
            if (TextoInput.Length < 11 || TextoInput == "")
                return false;

            string NuevePrimerosDigitos = TextoInput.Substring(0, 9);
            string PrimerDigito = TextoInput.Substring(9, 1);
            string SegundoDigito = TextoInput.Substring(10, 1);
            int PrimerDigitoValidar = ValidarPrimerDigito(NuevePrimerosDigitos);
            int SegundoDigitoValidar = ValidarSegundoDigito(NuevePrimerosDigitos + PrimerDigitoValidar.ToString());
            return (Convert.ToByte(PrimerDigito) == PrimerDigitoValidar & Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);
        }
        static int ValidarPrimerDigito(string TextoValidar)
        {
            int[] Multiplicadores = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[9];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }
            total = Resultados.Sum();
            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }
        static int ValidarSegundoDigito(string TextoValidar)
        {
            int[] Multiplicadores = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[10];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }
            total = Resultados.Sum();
            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }
        #endregion

        #endregion
    }
}
