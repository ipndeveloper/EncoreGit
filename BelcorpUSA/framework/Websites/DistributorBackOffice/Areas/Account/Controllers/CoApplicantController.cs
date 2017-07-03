using System.Linq;
using NetSteps.Data.Entities.Cache;
using DistributorBackOffice.Areas.Account.Models;
using NetSteps.Data.Entities.Extensions;

namespace DistributorBackOffice.Areas.Account.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Web.Mvc;
	using NetSteps.Common.Extensions;
	using NetSteps.Data.Entities;
	using NetSteps.Data.Entities.Exceptions;
	using NetSteps.Data.Entities.Generated;
	using NetSteps.Encore.Core.IoC;
	using NetSteps.Web.Mvc.Attributes;
    using NetSteps.Commissions.Common.Models;
    using NetSteps.Commissions.Common;
    using NetSteps.Web.Mvc.Controls.Models;
    using NetSteps.Common.Globalization;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Data.Entities.Business.Logic;

	public class CoApplicantController : BaseAccountsController
	{

        [FunctionFilter("Accounts-Disbursement Profiles", "~/Account", ConstantsGenerated.SiteType.BackOffice)]
		public virtual ActionResult Index()
		{
            CoApplicantSearchParameters Model = new AccountBusinessLogic().GetAccountAdditionalTitulars(CurrentAccount.AccountID);
            return View(Model);
		}

        public ActionResult SaveCoApplicant(CoApplicantSearchParameters CoApplicantObj)
        {
            bool result = false;
            string message = string.Empty;

            try
            {
                CoApplicantObj.AccountID = CurrentAccount.AccountID;
                int AccountAdditionalTitularID = new AccountBusinessLogic().InsertAccountAdditionalTitulars(CoApplicantObj);

                AccountBusinessLogic accountBusinessLogic = new AccountBusinessLogic();

                AccountAdditionalTitularSuppliedIDsParameters CPF = new AccountAdditionalTitularSuppliedIDsParameters()
                {
                    AccountAdditionalTitularID = AccountAdditionalTitularID,
                    IDTypeID = 8,
                    AccountSuppliedValue = CoApplicantObj.CPF,
                    IsPrimaryID = true
                };

                accountBusinessLogic.InsertAccountAdditionalTitularSuppliedIDs(CPF);

                AccountAdditionalTitularSuppliedIDsParameters RG = new AccountAdditionalTitularSuppliedIDsParameters()
                {
                    AccountAdditionalTitularID = AccountAdditionalTitularID,
                    IDTypeID = 4,
                    AccountSuppliedValue = CoApplicantObj.RG,
                    IsPrimaryID = false,
                    IDExpeditionDate = CoApplicantObj.IssueDate,
                    ExpeditionEntity = CoApplicantObj.OrgExp
                };

                accountBusinessLogic.InsertAccountAdditionalTitularSuppliedIDs(RG);

                if (string.IsNullOrEmpty(CoApplicantObj.PIS))
                    CoApplicantObj.PIS = string.Empty;

                AccountAdditionalTitularSuppliedIDsParameters PIS = new AccountAdditionalTitularSuppliedIDsParameters()
                {
                    AccountAdditionalTitularID = AccountAdditionalTitularID,
                    IDTypeID = 9,
                    AccountSuppliedValue = CoApplicantObj.PIS,
                    IsPrimaryID = false
                };

                accountBusinessLogic.InsertAccountAdditionalTitularSuppliedIDs(PIS);

                if (CoApplicantObj.Phones != null)
                {
                    foreach (AccountAdditionalPhonesParameters additionalPhone in CoApplicantObj.Phones)
                    {
                        additionalPhone.AccountAdditionalTitularID = AccountAdditionalTitularID;
                        additionalPhone.ModifiedByUserID = CurrentAccount.UserID.ToInt();

                        accountBusinessLogic.InsertAccountAdditionalPhones(additionalPhone);
                    }
                }

                result = true;
                message = "CoApplicant successfully saved.";
            }
            catch (Exception ex)
            {
                message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;
            }

            return Json(new { result = result, message = message });
        }

        #region [Validations]

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
            else
            {

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
