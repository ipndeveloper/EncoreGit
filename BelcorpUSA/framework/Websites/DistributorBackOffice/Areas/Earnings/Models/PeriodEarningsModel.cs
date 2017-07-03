using System;
using System.Collections.Generic;
using DistributorBackOffice.Models;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.EntityModels;

namespace DistributorBackOffice.Areas.Earnings.Models
{
    public class PeriodEarningsModel
    {
        public AccountQuickFacts AccountQuickFacts { get; set; }
        public Address AccountAddress { get; set; }
        public Address CompanyAddress { get; set; }

        public int PeriodId { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }

        /*Antonio Campos Santos: 30/01/2016 (dd/MM/yyyy)*/
        public IEnumerable<EarningsTotal> EarningsTotals { get; set; }
        public IEnumerable<EarningReportBasic> EarningReportBasics { get; set; }
        /*Fin*/
        public IEnumerable<IBonusPayout> BonusPayouts { get; set; }

        public IEnumerable<IEarningReport> Earnings { get; set; }

        public string ErrorMessage { get; set; }
    }
}