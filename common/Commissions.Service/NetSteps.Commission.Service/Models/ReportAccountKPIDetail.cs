using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Common.Models;

namespace NetSteps.Commissions.Service.Models
{
    public class ReportAccountKPIDetail: IReportAccountKPIDetail
    {
        public int? AccountKPIDetailID
        {
            get; set;
        }

        //public int? CareerTitle Antonio Campos Santos (11/feb/2016)
        //{
        //    get; set;
        //}
        public string CareerTitle
        {
            get;
            set;
        }

        public decimal? DQV
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

        public string DownlinePaidAsTitle
        {
            get; set;
        }

        public int? Generation
        {
            get; set;
        }

        public int? Level
        {
            get; set;
        }

        public decimal? PQV
        {
            get; set;
        }

        public decimal? QV
        {
            get; set;
        }
    }
}
