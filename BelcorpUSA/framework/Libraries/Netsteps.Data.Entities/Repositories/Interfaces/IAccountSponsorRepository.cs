namespace NetSteps.Data.Entities.Repositories
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Common.Base;
    using System;
    using System.Data;

    /// <summary>
    /// Interface for AccountSponsorRepository
    /// </summary>
    public partial interface IAccountSponsorRepository
    {

        void InsertAccountSponsor(AccountSponsorSearchParameters AccountSponsor);
        /// <summary>
        /// Gets Account Information for the EditSponsorView
        /// </summary>
        AccountSponsorSearchData GetAccountInformationEditSponsor(AccountSponsorSearchParameters searchParameters);
        PaginatedList<AccountSponsorLogSearchData> GetUpdateLogEditSponsor(AccountSponsorLogSearchParameters searchParameters);
        string UpdateSponsorInformation(AccountSponsorSearchParameters updateParameters, Boolean PeriodoFuturo);
        bool ValidacionPeriodoActual(AccountSponsorSearchParameters searchParameters);
        Tuple<bool, string> ValidateSponsorShipRules(AccountSponsorSearchParameters searchParameters, string processType);
        bool ValidateManagerOrHigher(AccountSponsorSearchParameters searchParameters);

        AccountLocator SeleccionAutomaticaSponsor(string CEP, int MarketID);

        int[] AplicarreglacValiacionPatrocinio(DataTable dtAccountIds);

        Dictionary<int,string> ListarCuentasPorCodigoPostal(string CEP, int MarketID);

        Account GetSponsorBasicInfo(int AccountID);
        string TerminateConsultant(int AccountID);
    }
}
