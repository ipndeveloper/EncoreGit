using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public class AccountSearchParameters : FilterDateRangePaginatedListParameters<Account>, IPrimaryKey<int>
	{
		public int? AccountReportID { get; set; }

        public int? AccountID { get; set; }
        public short? AccountStatusID { get; set; }
        public IEnumerable<short> AccountTypes { get; set; }
        public short? AccountTypeID { get; set; }
        public IEnumerable<short> ExcludedAccountStatuses { get; set; }
		////public string State { get; set; }
		public string City { get; set; }
		public int? StateProvinceID { get; set; }
		public string PostalCode { get; set; }
		public int? CountryID { get; set; }

		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string CoApplicant { get; set; }

		/// <summary>
		/// Nullable because it is possible that we want to search everyone.
		/// This field will provide further filtering on top of whatever 
		/// CurrentAccountID is in case this is being called from the GMP.
		/// This will most likely be the same as CurrentAccountID if it is being called from the DWS.
		/// </summary>
		public int? SponsorID { get; set; }

		/// <summary>
		/// Required.
		/// CurrentAccountID will reflect the CorporateAccountID 
		/// whenever this is called from the GMP search.
		/// This will be set to whoever is logged into the DWS when a search is initiated from the DWS.
		/// Used to prevent SponsorID from being null and allowing the full list of accounts being returned.
		/// </summary>
		public int CurrentAccountID { get; set; }

		public string Name { get; set; }

		public int? TitleID { get; set; }
		public string SiteUrl { get; set; }

		public int? ContactCategoryID { get; set; }
		public int? ContactStatusID { get; set; }

		public short? AccountSourceID { get; set; }

        public string CPF { get; set; } //CAMBIO ENCORE-4

        //CAMBIO ENCORE-4
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public string gender { get; set; }
        //CAMBIO ENCORE-4 Fin


		#region IPrimaryKey<int> Members

		int IPrimaryKey<int>.PrimaryKey
		{
            get { return AccountReportID ?? 0; }
		}

		#endregion
	}
}
