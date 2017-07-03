namespace NetSteps.Data.Entities.Repositories
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Common.Base;
    using System;

    public partial interface IAccountAdditionalTitularsRepository
    {
        int InsertAccountAdditionalTitulars(CoApplicantSearchParameters CoApplicantSearchParameters);
        void InsertAccountAdditionalTitularSuppliedIDs(AccountAdditionalTitularSuppliedIDsParameters AccountAdditionalTitularSuppliedIDsParameters);
    }
}
