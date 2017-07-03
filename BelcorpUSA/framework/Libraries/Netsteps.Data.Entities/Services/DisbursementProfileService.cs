// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisbursementProfileService.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   Disbursement Profile service
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NetSteps.Data.Entities.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Addresses.Common.Models;
    using NetSteps.Common.Extensions;
    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Business.HelperObjects;
    using NetSteps.Data.Entities.Commissions;
    using NetSteps.Data.Entities.Extensions;
    using NetSteps.Data.Entities.Generated;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Encore.Core.IoC;
    using Account = NetSteps.Data.Entities.Account;
	/// <summary>
	/// Disbursement Profile service
	/// </summary>
	[ContainerRegister(typeof(IDisbursementProfileService), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
	public class DisbursementProfileService : IDisbursementProfileService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DisbursementProfileService"/> class.
		/// </summary>
		public DisbursementProfileService()
		{
			Repository = new DisbursementProfileRepository();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DisbursementProfileService"/> class.
		/// </summary>
		/// <param name="repository">
		/// The repository.
		/// </param>
		public DisbursementProfileService(DisbursementProfileRepository repository)
		{
			Repository = repository;
		}

		/// <summary>
		/// Gets or sets Repository.
		/// </summary>
		private DisbursementProfileRepository Repository { get; set; }

		/// <summary>
		/// Loads profiles by account
		/// </summary>
		/// <param name="accountID">
		/// The account id.
		/// </param>
		/// <returns>
		/// returns a list of disbursement profiles
		/// </returns>
		public List<DisbursementProfile> LoadByAccountID(int accountID)
		{
			var list = Repository.LoadByAccountID(accountID);
			foreach (var item in list)
			{
				item.StartEntityTracking();
				item.IsLazyLoadingEnabled = true;
			}

			return list;
		}

		/// <summary>
		/// Gets the count of existing profiles for account by type
		/// </summary>
		/// <param name="accountID">
		/// The account id.
		/// </param>
		/// <param name="profileType">
		/// The profile type.
		/// </param>
		/// <returns>
		/// returns an int representing the count of existing profiles
		/// </returns>
		public int GetProfileCountByAccountAndType(int accountID, Constants.DisbursementProfileType profileType)
		{
			return Repository.GetProfileCountByAccountAndType(accountID, profileType);
		}

		public List<DisbursementProfile> LoadByAccountAndType(int account, Constants.DisbursementProfileType profileType)
		{
			return Repository.LoadByAccountAndType(account, profileType);
		}

		/// <summary>
		/// Saves the disbursement profile
		/// </summary>
		/// <param name="profileID">
		/// The profile ID.
		/// </param>
		/// <param name="account">
		/// The account.
		/// </param>
		/// <param name="address">
		/// The address.
		/// </param>
		/// <param name="profileType">
		/// The profile Type.
		/// </param>
		/// <param name="useAddressOfRecord">
		/// The use Address Of Record.
		/// </param>
		/// <param name="eftAccounts">
		/// The eft Accounts.
		/// </param>
		/// <param name="agreementOnFile">
		/// The agreement On File.
		/// </param>
		/// <returns>
		/// returns a disbursement profile
		/// </returns>
		public bool Save(
			 int profileID,
			 Account account,
			 Address address,
			 Constants.DisbursementProfileType profileType,
			 bool useAddressOfRecord,
			 List<EFTAccount> eftAccounts,
			 bool agreementOnFile)
		{
			var service = Create.New<IDisbursementProfileService>();
			var profiles = service.LoadByAccountID(account.AccountID);
			var disbursementProfileCheck =
				 profiles.FirstOrDefault(
					  p =>
					  p.DisbursementProfileID == profileID
					  && p.DisbursementTypeID == (int)Constants.DisbursementProfileType.Check)
					  ?? new DisbursementProfile();

			if (address.CountryID == 0)
			{
				address.CountryID = (int)ConstantsGenerated.Country.UnitedStates;
			}

			switch (profileType)
			{
				case Constants.DisbursementProfileType.Check:
					// 2012-08-11, JWL, inactive existing EFT profiles
					var eftProfiles = profiles.Where(x => x.DisbursementTypeID == (int)Constants.DisbursementProfileType.EFT);

					foreach (var eftProfile in eftProfiles)
					{
						if (eftProfile.DisbursementProfileID == 0)
						{
							continue;
						}

						if (!eftProfile.Enabled.HasValue || eftProfile.Enabled == false)
						{
							continue;
						}

						eftProfile.Enabled = false;
						eftProfile.Save();
					}

					disbursementProfileCheck.AccountID = account.AccountID;
					disbursementProfileCheck.DisbursementTypeID = (int)Constants.DisbursementProfileType.Check;
					disbursementProfileCheck.Enabled = true;
					disbursementProfileCheck.Percentage = 1;

					var checkProfile = disbursementProfileCheck.CheckProfile ?? new CheckProfile(disbursementProfileCheck);

					Address checkAddress;

					if (!useAddressOfRecord)
					{
						// Get the check address, if it already exists, and update it.
						var addressId = checkProfile.AddressID;
						checkAddress = addressId > 0 && Address.Exists(addressId) ? Address.Load(addressId) : new Address();

						UpdateAddressInformation(checkAddress, address);
						checkAddress.Save();

						// If the check address is not yet associated with the account, add it to the account.
						var accountAddress = account.Addresses.FirstOrDefault(x => x.AddressID == checkAddress.AddressID);

						if (accountAddress == null)
						{
							account.Addresses.Add(checkAddress);
							account.Save();
						}
					}
					else
					{
						// Use address of record, which I (and those I asked) interpret as the default address on the account. - Bryant Smith
						checkAddress = account.Addresses.FirstOrDefault(a => a.IsDefault);
					}


					if (checkAddress != null)
					{
						checkProfile.NameOnCheck = string.IsNullOrEmpty(checkAddress.Attention)
																? string.Format("{0} {1}", account.FirstName, account.LastName)
																: checkAddress.Attention;
						checkProfile.AddressID = checkAddress.AddressID;
					}

					disbursementProfileCheck.Save();

					break;

				case Constants.DisbursementProfileType.EFT:
					// 2012-08-11, JWL, inactivate existing check profile
					if (disbursementProfileCheck.DisbursementProfileID != 0 && disbursementProfileCheck.Enabled == true)
					{
						disbursementProfileCheck.Enabled = false;
						disbursementProfileCheck.Save();
					}

					foreach (var eftAccount in eftAccounts)
					{
						var disbursementProfileEFT =
							 profiles.FirstOrDefault(p => p.DisbursementProfileID == eftAccount.DisbursementProfileID
																	&&
																	p.DisbursementTypeID
																	== (int)Constants.DisbursementProfileType.EFT);
						if (disbursementProfileEFT == null)
						{
							if (this.GetProfileCountByAccountAndType(
								 account.AccountID, Constants.DisbursementProfileType.EFT) < 2)
							{
								disbursementProfileEFT = new DisbursementProfile();
							}
							else
							{
								// for some reason it is trying to add another profile when 2 EFT profiles already exist.
								// failsafe exception
								throw new Exception("There is an error with the number of available EFT profiles. There are the maximum number of allowed existing EFT profiles but the application is attempting to add another.");
							}
						}

						disbursementProfileEFT.AccountID = account.AccountID;
						disbursementProfileEFT.DisbursementTypeID = (int)Constants.DisbursementProfileType.EFT;
						disbursementProfileEFT.Enabled = eftAccount.Enabled;
						disbursementProfileEFT.Percentage = eftAccount.PercentToDeposit / (double)100;

						var eftProfile = disbursementProfileEFT.EFTProfile;

						var bankAddress = eftProfile.BankAddressID > 0 ? Address.Load(eftProfile.BankAddressID) : new Address();
						if (!string.IsNullOrEmpty(eftAccount.BankAddress1))
						{
							bankAddress.AddressTypeID = ConstantsGenerated.AddressType.Disbursement.ToShort();
							bankAddress.Address1 = eftAccount.BankAddress1;
							bankAddress.Address2 = eftAccount.BankAddress2;
							bankAddress.Address3 = eftAccount.BankAddress3;
							bankAddress.City = eftAccount.BankCity;

                            // FIRE FIX - Lundy
							int countryID;
							if (int.TryParse(eftAccount.BankCountry, out countryID))
							{
								bankAddress.CountryID = countryID;
							}
							else
							{
								bankAddress.CountryID = (int)ConstantsGenerated.Country.UnitedStates;
							}

							bankAddress.SetState(eftAccount.BankState, bankAddress.CountryID);
							
							bankAddress.PostalCode = eftAccount.BankZip;
					
							var accountAddress =
							account.Addresses.FirstOrDefault(
								 x => x.AddressID == bankAddress.AddressID);
							if (accountAddress == null)
							{
								account.Addresses.Add(bankAddress);
							}
							else
							{
								accountAddress.AddressTypeID = ConstantsGenerated.AddressType.Disbursement.ToShort();
								accountAddress.Address1 = eftAccount.BankAddress1;
								accountAddress.Address2 = eftAccount.BankAddress2;
								accountAddress.Address3 = eftAccount.BankAddress3;
								accountAddress.City = eftAccount.BankCity;
								
                                // FIRE FIX - Lundy
								accountAddress.SetState(eftAccount.BankState, bankAddress.CountryID);

								accountAddress.PostalCode = eftAccount.BankZip;
								accountAddress.CountryID = bankAddress.CountryID;
								accountAddress.Save();
							}

							account.Save();
						}

						eftProfile.NameOnAccount = eftAccount.Name;
						eftProfile.RoutingNumber = eftAccount.RoutingNumber;
                        eftProfile.BankAccountNumber = BankAccountNumberIsValid(eftAccount.AccountNumber) ? eftAccount.AccountNumber : eftProfile.BankAccountNumber;
						eftProfile.BankName = eftAccount.BankName;
						eftProfile.BankPhone = eftAccount.BankPhone;
						eftProfile.BankAddressID = bankAddress.AddressID;
						eftProfile.BankAccountType = eftAccount.AccountType;
						eftProfile.EnrollmentFormReceived = agreementOnFile;
						disbursementProfileEFT.Save();
						eftAccount.DisbursementProfileID = disbursementProfileEFT.DisbursementProfileID;
					}

					break;
			}

			return true;
		}

        protected bool BankAccountNumberIsValid(string accountNumber)
        {
            return !String.IsNullOrWhiteSpace(accountNumber) && !accountNumber.Contains('*');
        }

		/// <summary>
		/// deletes profiles by account
		/// </summary>
		/// <param name="accountID">
		/// The account id.
		/// </param>
		//public void DeleteByAccountID(int accountID)
		//{
		//    Repository.DeleteByAccountID(accountID);
		//}

		/// <summary>
		/// gets the disbursement types by the type code
		/// </summary>
		/// <param name="disbursementTypeId">
		/// The disbursement type id.
		/// </param>
		/// <returns>
		/// returns a disbursement type code
		/// </returns>
		public string GetDisbursementTypeCode(int disbursementTypeId)
		{
			return Repository.GetDisbursementTypeCode(disbursementTypeId);
		}

		//public static void DeleteByAccountID(int accountID)
		//{
		//    Repository.DeleteByAccountID(accountID);
		//}

		///// <summary>
		///// disables disbursement profiles by account
		///// </summary>
		///// <param name="accountID">
		///// The account id.
		///// </param>
		//public void DisableByAccountID(int accountID)
		//{
		//    Repository.DisableByAccountID(accountID);
		//}

		/// <summary>
		/// Populates the information from one address object to anotther
		/// </summary>
		/// <param name="addressToUpdate">
		/// The address to update.
		/// </param>
		/// <param name="infoAddress">
		/// The address containing the information to update
		/// </param>
		private void UpdateAddressInformation(IAddress addressToUpdate, IAddress infoAddress)
		{
			addressToUpdate.ProfileName = infoAddress.ProfileName;
			addressToUpdate.Attention = infoAddress.Attention;
			addressToUpdate.AddressTypeID = ConstantsGenerated.AddressType.Disbursement.ToShort();
			addressToUpdate.Address1 = infoAddress.Address1;
			addressToUpdate.Address2 = infoAddress.Address2;
			addressToUpdate.Address3 = infoAddress.Address3;
			addressToUpdate.City = infoAddress.City;
			addressToUpdate.State = infoAddress.State;
			addressToUpdate.PostalCode = infoAddress.PostalCode;
			addressToUpdate.CountryID = infoAddress.CountryID;
		}
	}
}