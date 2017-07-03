namespace NetSteps.Data.Entities.Repositories
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Common.Base;
    using System;
    using NetSteps.Commissions.Common.Models;
    using NetSteps.Data.Entities.Business.HelperObjects;

    public partial interface IDisbusementProfileRepository
    {
        void SaveCheckDisbursementProfile(EFTAccount EFTAccount);
    }
}
