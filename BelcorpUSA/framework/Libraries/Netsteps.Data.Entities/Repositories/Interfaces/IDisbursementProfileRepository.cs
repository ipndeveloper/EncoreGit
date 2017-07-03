// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDisbursementProfileRepository.cs" company="NetSteps">
//   Copyright, 2012 NetSteps, LLC
// </copyright>
// <summary>
//   Defines the IDisbursementProfileRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IDisbursementProfileRepository
    {
        List<DisbursementProfile> LoadByAccountID(int accountID);
        //void DeleteByAccountID(int accountID);
        void DisableByAccountID(int accountID);
        string GetDisbursementTypeCode(int disbursementTypeId);
        List<DisbursementType> LoadEnabledDisbursementTypes();
    }
}
