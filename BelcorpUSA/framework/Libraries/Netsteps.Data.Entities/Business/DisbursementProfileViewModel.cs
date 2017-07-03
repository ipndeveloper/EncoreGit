// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisbursementProfileViewModel.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   Defines the DisbursementProfileViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NetSteps.Data.Entities.Business
{
	using System.Collections.Generic;
	using System.Linq;

	using NetSteps.Data.Entities;
	using NetSteps.Data.Entities.Commissions;
	using NetSteps.Data.Entities.Extensions;
	using NetSteps.Data.Entities.Generated;
	using NetSteps.Data.Entities.Services;
	using NetSteps.Encore.Core.IoC;

	/// <summary>
	/// The view model for the Disbursement Profile screen in DistributorBackOffice
	/// </summary>
	public class DisbursementProfileViewModel
	{
		/// <summary>
		/// Gets or sets AccountID.
		/// </summary>
		public Data.Entities.Account Account { get; set; }

		/// <summary>
		/// Gets or sets EFTProfiles.
		/// </summary>
		public List<DisbursementProfile> EFTProfiles { get; set; }

		/// <summary>
		/// Gets or sets CheckProfile.
		/// </summary>
		public DisbursementProfile CheckProfile { get; set; }

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
		public Constants.DisbursementProfileType PaymentPreference
		{
			get
			{
				var retVal = this.EFTProfiles.Any(x => x.Enabled.HasValue && (bool)x.Enabled)
													  ? Constants.DisbursementProfileType.EFT
													  : this.CheckProfile.Enabled.HasValue
														 && (bool)this.CheckProfile.Enabled
															  ? Constants.DisbursementProfileType.Check
															  : Constants.DisbursementProfileType.EFT;
				return retVal;
			}
		}

		/// <summary>
		/// Gets AddressOfRecord.
		/// </summary>
		public Address AddressOfRecord
		{
			get
			{
				if (this.Account.Addresses.Count > 0 && this.Account.Addresses.GetAllByTypeID(ConstantsGenerated.AddressType.Main).Count > 0)
				{
					return this.Account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Main);
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
		public void CreateModel(Entities.Account account)
		{
			var service = Create.New<IDisbursementProfileService>();

			this.Account = account;
			var profiles = service.LoadByAccountID(this.Account.AccountID);

			var checkProfile = profiles.FirstOrDefault(p => p.DisbursementTypeID == (int)Constants.DisbursementProfileType.Check);
			this.CheckProfile = checkProfile
								?? new DisbursementProfile { DisbursementTypeID = (int)Constants.DisbursementProfileType.Check };

			var addressId = this.CheckProfile.CheckProfile.AddressID;
			this.CheckAddress = addressId > 0 && Address.Exists(addressId)
								 ? Address.Load(addressId)
								 : this.Account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Main)
									  ?? new Address();

			this.CheckAddress.Attention = this.CheckProfile.CheckProfile.NameOnCheck;

			this.EFTProfiles = profiles.Where(p => p.DisbursementTypeID == (int)Constants.DisbursementProfileType.EFT).ToList();

			if (this.EFTProfiles.Count >= 2)
			{
				return;
			}

			for (var i = this.EFTProfiles.Count; i < 2; i++)
			{
				this.EFTProfiles.Add(new DisbursementProfile { DisbursementTypeID = (int)Constants.DisbursementProfileType.EFT });
			}
		}
	}
}
