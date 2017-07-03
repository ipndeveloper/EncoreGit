namespace NetSteps.Data.Entities.Business.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Common.Base;
    using System;
    using NetSteps.Data.Entities.Dto;
    using System.Data;
    using System.Web;

    /// <summary>
    /// Methos for AccountSponsorBusinessLogic business Object
    /// </summary>
    public partial class AccountSponsorBusinessLogic
    {

        #region constructor - singleton
        /// <summary>
        /// Prevents a default instance of the AccountSponsorBusinessLogic class.
        /// </summary>
        private AccountSponsorBusinessLogic()
        {
        }

        /// <summary>
        /// Gets instance of the AccountSponsorBusinessLogic class using singleton pattern
        /// </summary>
        public static AccountSponsorBusinessLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountSponsorBusinessLogic();
                    //Injection TODO: use IOC
                    repositoryAccountSponsor = new AccountSponsorRepository();
                }

                return instance;
            }
        }
        #endregion

        #region privates

        /// <summary>
        /// Gets or sets AccountSponsorBusinessLogic class
        /// </summary>
        private static AccountSponsorBusinessLogic instance;

        /// <summary>
        /// gets or sets IAccountSponsorRepository implementation
        /// </summary>
        private static IAccountSponsorRepository repositoryAccountSponsor;

        #endregion

        #region Methods

        public void InsertAccountSponsor(AccountSponsorSearchParameters AccountSponsor)
        {
            try
            {
                repositoryAccountSponsor.InsertAccountSponsor(AccountSponsor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets Account Information for the EditSponsorView
        /// </summary>
        public AccountSponsorSearchData GetAccountInformationEditSponsor(AccountSponsorSearchParameters seachParameters)
        {
            return repositoryAccountSponsor.GetAccountInformationEditSponsor(seachParameters);
        }

        /// <summary>
        /// Gets Update Log for the EditSponsorView
        /// </summary>
        public PaginatedList<AccountSponsorLogSearchData> GetUpdateLogEditSponsor(AccountSponsorLogSearchParameters seachParameters)
        {
            return repositoryAccountSponsor.GetUpdateLogEditSponsor(seachParameters);
        }

        /// <summary>
        /// Gets Update Log for the EditSponsorView
        /// </summary>
        public string UpdateSponsorInformation(AccountSponsorSearchParameters updateParameters)
        {
            string result = string.Empty;
            Tuple<bool, string> validateSponsorShipRules;
            if (updateParameters.PeriodID == updateParameters.NewPeriodID)
            {
                bool aptconsul = false;
                aptconsul = repositoryAccountSponsor.ValidacionPeriodoActual(updateParameters);
                if (aptconsul)
                {
                    validateSponsorShipRules = repositoryAccountSponsor.ValidateSponsorShipRules(updateParameters, "Manual");
                    if (validateSponsorShipRules.Item1) result = repositoryAccountSponsor.UpdateSponsorInformation(updateParameters, false);
                    else result = validateSponsorShipRules.Item2;
                }
                else result = "El consultor no es apto para cambiar de patrocinador en la campaña actual.";
            }
            else
            {
                bool aptosponsor = false;
                validateSponsorShipRules = repositoryAccountSponsor.ValidateSponsorShipRules(updateParameters, "Manual");
                aptosponsor = repositoryAccountSponsor.ValidateManagerOrHigher(updateParameters);
                if (validateSponsorShipRules.Item1 && aptosponsor) result = repositoryAccountSponsor.UpdateSponsorInformation(updateParameters, true);
                else result = "El sponsor  no es apto para recibir consultores en su red.";
            }
            return result;
        }

        public Tuple<bool, string> ValidateSponsorShipRules(int AccountID, int PeriodID)
        {
            try
            {
                AccountSponsorSearchParameters parameters = new AccountSponsorSearchParameters();
                parameters.AccountID = AccountID;
                parameters.NewPeriodID = PeriodID;

                Tuple<bool, string> result = repositoryAccountSponsor.ValidateSponsorShipRules(parameters, "Manual");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public AccountLocator SeleccionAutomaticaSponsor(string CEP, int MarketID)
        {
            try
            {
                AccountLocator objAccountLocator = new AccountLocator();
                AccountLocatorContentData objAccountLocatorContentData = new AccountLocatorContentData();
                objAccountLocator = repositoryAccountSponsor.SeleccionAutomaticaSponsor(CEP, MarketID);
                AccountRepository objAccountRepository = new AccountRepository();

                objAccountLocatorContentData = (AccountLocatorContentData)objAccountRepository.GetAccountLocatorContent(new List<int>() { objAccountLocator.AccountID }).FirstOrDefault();
                objAccountLocator.html = objAccountLocatorContentData == null ? "" : objAccountLocatorContentData.PhotoContent.ToHtmlString();

                return objAccountLocator;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int[] AplicarreglacValiacionPatrocinio(DataTable dtAccountIds)
        {
            return repositoryAccountSponsor.AplicarreglacValiacionPatrocinio(dtAccountIds);
        }

        public Dictionary<int, string> ListarCuentasPorCodigoPostal(string CEP, int MarketID)
        {
            return repositoryAccountSponsor.ListarCuentasPorCodigoPostal(CEP, MarketID);

        }

        public Account GetSponsorBasicInfo(int AccountID)
        {
            return repositoryAccountSponsor.GetSponsorBasicInfo(AccountID);
        }

        public string TerminateConsultant(int AccountID)
        {
            return repositoryAccountSponsor.TerminateConsultant(AccountID);
        }

        #endregion
    }
}
