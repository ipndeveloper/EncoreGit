using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Common.Models;

namespace NetSteps.Commissions.Service.Models
{
    [Serializable]
    public class ReportBonusDetail : IReportBonusDetail
    {
        public decimal? AmountPaid
        {
            get; set;
        }

        public int? BonusTypeID
        {
            get; set;
        }

        public string BonusTypeName
        {
            get; set;
        }

        public decimal? BonusValue
        {
            get; set;
        }

        public decimal? CB
        {
            get; set;
        }

        public int? DownlineID
        {
            get; set;
        }

        public string DownlineName
        {
            get; set;
        }

        public decimal? PCV
        {
            get; set;
        }

        public decimal? PQV
        {
            get; set;
        }
    }
}
