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
    public partial interface IManualBonusEntryRepository
    {
        List<ManualBonusEntrySearchData> ManualBonusEntryValidation(DataTable values);
        Tuple<int, string> ManualBonusEntryLoad(DataTable values);
    }
}
