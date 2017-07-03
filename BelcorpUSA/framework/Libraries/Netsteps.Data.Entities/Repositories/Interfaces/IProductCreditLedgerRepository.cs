using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IProductCreditLedgerRepository
    {
        List<ProductCreditLedger> LoadByAccountID(int accountID);
        decimal GetBalance(int accountID, DateTime? effectiveDate, int? entryID, int? entryTypeID);
        decimal GetBalanceLessPendingPayments(int accountID, DateTime? effectiveDate, int? entryID, int? orderPaymentID = null);
        void SaveTemporaryAccountToCommission(int accountID, string accountNumber);
    }
}
