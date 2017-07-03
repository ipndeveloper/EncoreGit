using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using Account = NetSteps.Data.Entities.Account;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;

namespace NetSteps.Web.Mvc.Controls.Models
{
	public class CheckDisbursementViewModel
	{
		public Data.Entities.Account Account { get; set; }

		public ICheckDisbursementProfile CheckProfile { get; set; }

		public Address CheckAddress { get; set; }

		public string PostalCodeLookupURL { get; set; }

		public string ChangeCountryURL { get; set; }

		public Address AddressOfRecord
		{
			get
			{
				if (Account.Addresses.Count > 0 && Account.Addresses.GetAllByTypeID(ConstantsGenerated.AddressType.Main).Count > 0)
				{
					return Account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Main);
				}

				return new Address();
			}
		}


		public CheckDisbursementViewModel SetupCheckDisbursementModel(List<IDisbursementProfile> profiles, Data.Entities.Account account)
		{
            var found = profiles.FirstOrDefault(p => p.DisbursementMethod == DisbursementMethodKind.Check);
            if (found != null)
            {
                CheckProfile = (ICheckDisbursementProfile)found;
            
                this.CheckAddress = CheckProfile.AddressId > 0
                                     ? Address.Load(CheckProfile.AddressId)
                                     : account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Main)
                                          ?? new Address();
                this.CheckAddress.Attention = CheckProfile.NameOnCheck;
            }
			return this;
		}
	}
}
