namespace NetSteps.Data.Entities.Repositories
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Common.Base;
    using System;
    using NetSteps.Data.Entities.EntityModels;

    /// <summary>
    /// Interface for AccountSponsorRepository
    /// </summary>
    public partial interface IAccountSuppliedIDsRepository
    {

        void InsertAccountSuppliedIDs(AccountSuppliedIDsParameters AccountSuppliedIDsParameters);
        List<AccountSuppliedIDsTable> GetAccountSuppliedIDByAccountID(AccountSuppliedIDsParameters AccountSuppliedIDsParameters);
        string DeleteAccountSuppliedIDsByAccountID(int accountID);
    }
}
