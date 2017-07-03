using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class ProductCreditLedgerParameters : FilterDateRangePaginatedListParameters<ProductCreditBalanceSearchData>
    {

        public int AccountID { get; set; }

        public int EntryID { get; set; }//   valor autogenerado

        public int EntryReasonID { get; set; }

        public int EntryOriginID { get; set; }

        public int EntryTypeID { get; set; }

        public int UserID { get; set; }

        public decimal EntryAmount { get; set; }       

        public int CurrencyTypeID { get; set; }

        public int OrderID { get; set; }

        public decimal? OrderPaymentID { get; set; }

        //--------
        public string CreditBalanceMin { get; set; }

        public string CreditBalanceMax { get; set; }

        public string EntryDateFrom { get; set; }

        public string EntryDateTo { get; set; }

        public string State { get; set; }
    }
}
