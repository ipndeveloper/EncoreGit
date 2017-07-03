using System.Collections.Generic;
using System.Linq;
using NetSteps.Commissions.Common;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Web.Mvc.Controls.Models
{
	public class DisbursementProfileViewModel
	{
		/// <summary>
		/// Gets or sets AccountID.
		/// </summary>
		public Account Account { get; set; }

		/// <summary>
		/// Gets or sets EFTProfiles.
		/// </summary>
		public List<IEFTDisbursementProfile> EFTProfiles { get; set; }

		/// <summary>
		/// Gets or sets CheckProfile.
		/// </summary>
		public ICheckDisbursementProfile CheckProfile { get; set; }

		/// <summary>
		/// Gets or sets CheckAddress.
		/// </summary>
		public Address CheckAddress { get; set; }

		/// <summary>
		/// Gets or set ChangeCountryURL
		/// </summary>
		public string ChangeCountryURL { get; set; }

		/// <summary>
		/// Gets or sets PostalCodeLookupURL.
		/// </summary>
		public string PostalCodeLookupURL { get; set; }

		/// <summary>
		/// Gets PaymentPreference.
		/// </summary>
		public DisbursementMethodKind PaymentPreference
		{
			get
			{
				var retVal = EFTProfiles.Any(x => x.IsEnabled && x.IsEnabled)
													  ? DisbursementMethodKind.EFT
													  : CheckProfile.IsEnabled
														 && CheckProfile.IsEnabled
															  ? DisbursementMethodKind.Check
															  : DisbursementMethodKind.EFT;
				return retVal;
			}
		}

		public bool IsCheckDisbursement
		{
			get { return PaymentPreference == DisbursementMethodKind.Check; }
		}

		public bool IsEftDisbursement
		{
			get { return PaymentPreference == DisbursementMethodKind.EFT; }
		}

		/// <summary>
		/// Gets AddressOfRecord.
		/// </summary>
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

		/// <summary>
		/// Loads the View Model 
		/// </summary>
		/// <param name="account">
		/// The account.
		/// </param>
		public void CreateModel(Account account)
		{
			var service = Create.New<ICommissionsService>();

			Account = account;
			var profiles = service.GetDisbursementProfilesByAccountId(Account.AccountID);
			CheckProfile = profiles.Where(p => p.DisbursementMethod == DisbursementMethodKind.Check).Cast<ICheckDisbursementProfile>().FirstOrDefault() ?? Create.New<ICheckDisbursementProfile>();

			var addressId = CheckProfile.AddressId;
			CheckAddress = addressId > 0 && Address.Exists(addressId)
								 ? Address.Load(addressId)
								 : Account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Main)
									  ?? new Address();

			CheckAddress.Attention = CheckProfile.NameOnCheck ?? (CheckProfile.NameOnAccount ?? CheckAddress.Attention);

			EFTProfiles = profiles.Where(p => p.DisbursementMethod == DisbursementMethodKind.EFT).Cast<IEFTDisbursementProfile>().ToList();

            /*CS.04JUL2016.Inicio.Reemplazo*/
            if (EFTProfiles.Count == 1)
                return;
            /*CS.04JUL2016.Fin.Reemplazo*/

            /*CS.04JUL2016.Inicio.Comentado*/
            //if (EFTProfiles.Count >= 2)
            //{
            //    return;
            //}
            /*CS.04JUL2016.Fin.Comentado*/
			for (var i = EFTProfiles.Count; i < 2; i++)
			{
				EFTProfiles.Add(Create.New<IEFTDisbursementProfile>());
			}
		}
	}
}
