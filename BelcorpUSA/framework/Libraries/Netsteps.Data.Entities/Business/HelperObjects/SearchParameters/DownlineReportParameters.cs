using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
    public class DownlineReportParameters : DateRangeSearchParameters, IPrimaryKey<int>
	{
		[DataMember]
		public int? AccountReportID { get; set; }

		[DataMember]
        private List<string> _visibleColumns = null;

		[System.Runtime.Serialization.IgnoreDataMember]
        public List<string> VisibleColumns
        {
            get
            {
                return _visibleColumns;
            }
            set
            {
                _visibleColumns = value;
            }
        }

		[DataMember]
        public int PeriodID { get; set; }

		[DataMember]
        public string AccountNumber { get; set; }

		[DataMember]
        public string SearchValue { get; set; }

		[DataMember]
        public decimal? PVFrom { get; set; }

		[DataMember]
        public decimal? PVTo { get; set; }

		[DataMember]
        public decimal? GVFrom { get; set; }

		[DataMember]
        public decimal? GVTo { get; set; }

		[DataMember]
        public decimal? TotalCommissionsPaid { get; set; }

		[DataMember]
        public decimal? PerkPoints { get; set; }

		[DataMember]
        public decimal? ProductRebateCredit { get; set; }

		[DataMember]
        public bool? FastStartQualified { get; set; }

		[DataMember]
        public int? BirthMonth { get; set; }

		[DataMember]
        public List<int> Titles { get; set; }
		
		[DataMember]
        public List<int> AccountTypes { get; set; }

		[DataMember]
        public List<int> States { get; set; }

		[DataMember]
        public int? SponsorID { get; set; }

		[DataMember]
        public bool ShowMyTeam { get; set; }

		[DataMember]
        public int? MonthsInactive { get; set; }

		[DataMember]
        public int? CurrentTopOfTreeAccountID { get; set; }

		[DataMember]
        public bool? GroupBySponsorTree { get; set; }

		[DataMember]
        public List<int> AccountStatuses { get; set; }

        [DataMember]
        public int? LevelFrom { get; set; }

        [DataMember]
        public int? LevelTo { get; set; }

        [DataMember]
        public int? GenFrom { get; set; }

        [DataMember]
        public int? GenTo { get; set; }

        [DataMember]
        public int? DVFrom { get; set; }

        [DataMember]
        public int? DVTo { get; set; }

        #region IPrimaryKey<int> Members

        int IPrimaryKey<int>.PrimaryKey
        {
            get { return AccountReportID.ToInt(); }
        }

        #endregion
    }

}