using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Comparer;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Business;
using NetSteps.Web.Mvc.Extensions;
using Newtonsoft.Json;
using System.Text;
using NetSteps.Web.Extensions;

namespace nsCore.Areas.Accounts.Controllers
{
    public class EditCoApplicantController: BaseAccountsController
    {

        //[FunctionFilter("Accounts-Edit Coapplicant", "~/Accounts/Overview")]
        [FunctionFilter("Accounts-Create and Edit Account", "~/Accounts/Overview")]
        public virtual ActionResult Index()
		{
            CoApplicantSearchParameters Model = new AccountBusinessLogic().GetAccountAdditionalTitulars(CurrentAccount.AccountID);
            return View(Model);
		}

        public ActionResult SaveCoApplicant(CoApplicantSearchParameters CoApplicantObj)
        {
            bool result = false;
            string message = string.Empty;
            CoApplicantSearchParameters CoApplicant = null;

            #region Validar CPF
            switch (swValidarCPF(CoApplicantObj.CPF))
            {
                case 1: message = Translation.GetTerm("CPFCOIsRegistered", "Provided CPF already in use."); break;
                case 3: message = Translation.GetTerm("CPFCOisInvalid", "Incorrect CPF value."); break;
                default: result = true; break;
            }
            #endregion

            if (result)
            {
                switch (swValidarPIS(CoApplicantObj.PIS))
                {
                    case 1: message = Translation.GetTerm("PisIsRegistered", "Provided PIS already in use."); break;
                    case 3: message = Translation.GetTerm("PISisInvalid", "Incorrect PIS value."); break;
                    default: result = true; break;
                }
            }

            if (!result)
                return Json(new { result = result, message = message, CoApplicant = CoApplicant });

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

                DateTime? IssueDate = null;
                if (CoApplicantObj.IssueDate.Year > 1900)
                    IssueDate = CoApplicantObj.IssueDate;
                AccountAdditionalTitularSuppliedIDsParameters RG = new AccountAdditionalTitularSuppliedIDsParameters()
                {
                    AccountAdditionalTitularID = AccountAdditionalTitularID,
                    IDTypeID = 4,
                    AccountSuppliedValue = CoApplicantObj.RG,
                    IsPrimaryID = false,
                    IDExpeditionDate = IssueDate,
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
                CoApplicant = new AccountBusinessLogic().GetAccountAdditionalTitulars(CurrentAccount.AccountID);
            }
            catch (Exception ex)
            {
                message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;
            }

            return Json(new { result = result, message = message, CoApplicant = CoApplicant });
        }

        public ActionResult UpdateCoApplicant(CoApplicantSearchParameters CoApplicantObj)
        {
            bool result = false;
            string message = string.Empty;
            CoApplicantSearchParameters CoApplicant = null;

            try
            {
                CoApplicantObj.AccountID = CurrentAccount.AccountID;

                int AccountAdditionalTitularID = new AccountBusinessLogic().UpdateAccountAdditionalTitulars(CoApplicantObj);

                AccountBusinessLogic accountBusinessLogic = new AccountBusinessLogic();

                AccountAdditionalTitularSuppliedIDsParameters CPF = new AccountAdditionalTitularSuppliedIDsParameters()
                {
                    AccountAdditionalTitularID = AccountAdditionalTitularID,
                    IDTypeID = 8,
                    AccountSuppliedValue = CoApplicantObj.CPF,
                    IsPrimaryID = true
                };

                accountBusinessLogic.UpdateAccountAdditionalTitularSuppliedIDs(CPF);

                DateTime? IssueDate = null;
                if (CoApplicantObj.IssueDate.Year > 1900)
                    IssueDate = CoApplicantObj.IssueDate;
                AccountAdditionalTitularSuppliedIDsParameters RG = new AccountAdditionalTitularSuppliedIDsParameters()
                {
                    AccountAdditionalTitularID = AccountAdditionalTitularID,
                    IDTypeID = 4,
                    AccountSuppliedValue = CoApplicantObj.RG,
                    IsPrimaryID = false,
                    IDExpeditionDate = IssueDate,
                    ExpeditionEntity = CoApplicantObj.OrgExp
                };

                accountBusinessLogic.UpdateAccountAdditionalTitularSuppliedIDs(RG);

                if (string.IsNullOrEmpty(CoApplicantObj.PIS))
                    CoApplicantObj.PIS = string.Empty;

                AccountAdditionalTitularSuppliedIDsParameters PIS = new AccountAdditionalTitularSuppliedIDsParameters()
                {
                    AccountAdditionalTitularID = AccountAdditionalTitularID,
                    IDTypeID = 9,
                    AccountSuppliedValue = CoApplicantObj.PIS,
                    IsPrimaryID = false
                };

                accountBusinessLogic.UpdateAccountAdditionalTitularSuppliedIDs(PIS);

                if (CoApplicantObj.Phones != null)
                {
                    foreach (AccountAdditionalPhonesParameters additionalPhone in CoApplicantObj.Phones)
                    {
                        if (additionalPhone.PhoneNumber != null)
                        {
                            additionalPhone.AccountAdditionalTitularID = AccountAdditionalTitularID;
                            additionalPhone.ModifiedByUserID = CurrentAccount.UserID.ToInt();
                            accountBusinessLogic.UpdateAccountAdditionalPhones(additionalPhone);
                        }
                    }
                }

                result = true;
                message = "CoApplicant successfully updated.";
                CoApplicant = new AccountBusinessLogic().GetAccountAdditionalTitulars(CurrentAccount.AccountID);
            }
            catch (Exception ex)
            {
                message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;
            }

            return Json(new { result = result, message = message, CoApplicant = CoApplicant });
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
                Account account = CurrentAccount;
                int accountID = 0;
                if (account != null)
                    accountID = account.AccountID;
                dcResultado = AccountExtensions.ValidarExistenciaCPF(NuevePrimerosDigitos + PrimerDigitoValidar + SegundoDigitoValidar, accountID);
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
                Account account = CurrentAccount;
                int accountID = 0;
                if (account != null)
                    accountID = account.AccountID;
                Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaPIS(NuevePrimerosDigitos + PrimerDigito.ToString() + SegundoDigitoValidar.ToString(), accountID);

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
