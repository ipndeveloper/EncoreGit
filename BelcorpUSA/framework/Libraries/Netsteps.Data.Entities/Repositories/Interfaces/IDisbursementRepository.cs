using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IDisbursementRepository : IBaseRepository<Disbursement, Int32>
    {
        List<Disbursement> LoadDisbursementsByTypeAndPeriod(int disbursementTypeID, int periodID);
        List<Disbursement> LoadDisbursementsByPeriod(int periodID);
    }
}
