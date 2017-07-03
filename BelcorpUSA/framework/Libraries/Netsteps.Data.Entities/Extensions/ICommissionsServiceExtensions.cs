using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Common;
using NetSteps.Commissions.Common.Models;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects;
using NetSteps.Data.Entities.Generated;
using NetSteps.Encore.Core.IoC;
using NetSteps.Addresses.Common.Models;

namespace NetSteps.Data.Entities.Extensions
{
    public static class ICommissionsServiceExtensions
    {
        public static bool SaveDisbursementProfile(
            this ICommissionsService service,
            int profileId,
            Account account,
            Address address,
            DisbursementMethodKind profileType,
            bool useAddressOfRecord,
            IEnumerable<IEFTAccount> eftAccounts,
            bool agreementOnFile)
        {
            return Create.New<ICommissionsServiceExtensionsImplementation>().SaveDisbursementProfile(
                service, profileId, account, address, profileType, useAddressOfRecord, eftAccounts, agreementOnFile);
        }
    }

    [ContainerRegister(typeof(ICommissionsServiceExtensionsImplementation), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public class ICommissionsServiceExtensionsImplementation
    {
        public virtual bool SaveDisbursementProfile(
            ICommissionsService service,
            int profileId,
            Account account,
            Address address,
            DisbursementMethodKind profileType,
            bool useAddressOfRecord,
            IEnumerable<IEFTAccount> eftAccounts,
            bool agreementOnFile)
        {
            switch (profileType)
            {
                case DisbursementMethodKind.EFT:
                    HandleEftDisbursementProfile(service, profileId, account, address, profileType, useAddressOfRecord, eftAccounts, agreementOnFile);
                    break;
                case DisbursementMethodKind.Check:
                    HandleCheckDisbursementProfile(service, profileId, account, address, useAddressOfRecord);
                    break;
                case DisbursementMethodKind.ProPay:
                    HandlePropayDisbursementProfile();
                    break;
                default:
                    throw new Exception(string.Format("unexpected disbursement method kind encountered: {0}", profileType));
            }

            return true;
        }

        protected virtual void HandleEftDisbursementProfile(ICommissionsService service, int profileId, Account account, Address address, DisbursementMethodKind profileType, bool useAddressOfRecord, IEnumerable<IEFTAccount> eftAccounts, bool agreementOnFile)
        {
            // 2012-08-11, JWL, inactive existing EFT profiles
            var country = Country.GetCountriesByMarketID(account.MarketID).FirstOrDefault(a => a.Active = true);
            var profiles = service.GetDisbursementProfilesByAccountId(account.AccountID);

            var disbursementProfileCheck = (profiles.FirstOrDefault(p => p.DisbursementMethod == DisbursementMethodKind.Check) ?? Create.New<ICheckDisbursementProfile>()) as ICheckDisbursementProfile;

            if (disbursementProfileCheck != null && disbursementProfileCheck.DisbursementProfileId != 0 && disbursementProfileCheck.IsEnabled)
            {
                disbursementProfileCheck.IsEnabled = false;
                service.SaveDisbursementProfile(disbursementProfileCheck);
            }

            var eftProfileCount = profiles.Count(x => x.DisbursementMethod == DisbursementMethodKind.EFT);

            foreach (var eftAccount in eftAccounts)
            {
                if (eftAccount.IsEnabled == true)
                {
                    var disbursementProfileEFT = profiles.FirstOrDefault(p => p.DisbursementProfileId == eftAccount.DisbursementProfileId && p.DisbursementMethod == DisbursementMethodKind.EFT) as IEFTDisbursementProfile;
                    if (disbursementProfileEFT == null)
                    {
                        if (eftProfileCount < 2)
                        {
                            disbursementProfileEFT = Create.New<IEFTDisbursementProfile>();
                        }
                        else
                        {
                            // for some reason it is trying to add another profile when 2 EFT profiles already exist.
                            // failsafe exception
                            throw new Exception("There is an error with the number of available EFT profiles. There are the maximum number of allowed existing EFT profiles but the application is attempting to add another.");
                        }
                    }

                    disbursementProfileEFT.AccountId = account.AccountID;
                    disbursementProfileEFT.IsEnabled = eftAccount.IsEnabled;
                    disbursementProfileEFT.Percentage = new decimal(eftAccount.PercentToDeposit / (double)100);

                    if (!(country == null))
                    {
                        disbursementProfileEFT.CurrencyId = country.CurrencyID;
                    }

                    var bankAddress = disbursementProfileEFT.AddressId > 0 ? Address.Load(disbursementProfileEFT.AddressId) : new Address();
                    if (bankAddress.AddressID == 0)
                    {
                        bankAddress = account.Addresses.FirstOrDefault(a => a.AddressTypeID == 1);
                    }
                    //if (!string.IsNullOrEmpty(eftAccount.BankAddress1))
                    //{
                    //    bankAddress.AddressTypeID = ConstantsGenerated.AddressType.Disbursement.ToShort();
                    //    bankAddress.Address1 = eftAccount.BankAddress1;
                    //    bankAddress.Address2 = eftAccount.BankAddress2;
                    //    bankAddress.Address3 = eftAccount.BankAddress3;
                    //    bankAddress.City = eftAccount.BankCity;

                    //    // FIRE FIX - Lundy
                    //    int countryID;
                    //    if (int.TryParse(eftAccount.BankCountry, out countryID))
                    //    {
                    //        bankAddress.CountryID = countryID;
                    //    }
                    //    else
                    //    {
                    //        bankAddress.CountryID = (int)ConstantsGenerated.Country.UnitedStates;
                    //    }

                    //    bankAddress.SetState(eftAccount.BankState, bankAddress.CountryID);

                    //    bankAddress.PostalCode = eftAccount.BankZip;

                    //    var accountAddress = account.Addresses.FirstOrDefault(x => x.AddressID == bankAddress.AddressID);
                    //    if (accountAddress == null)
                    //    {
                    //        account.Addresses.Add(bankAddress);
                    //    }
                    //    else
                    //    {
                    //        accountAddress.AddressTypeID = ConstantsGenerated.AddressType.Disbursement.ToShort();
                    //        accountAddress.Address1 = eftAccount.BankAddress1;
                    //        accountAddress.Address2 = eftAccount.BankAddress2;
                    //        accountAddress.Address3 = eftAccount.BankAddress3;
                    //        accountAddress.City = eftAccount.BankCity;

                    //        // FIRE FIX - Lundy
                    //        accountAddress.SetState(eftAccount.BankState, bankAddress.CountryID);

                    //        accountAddress.PostalCode = eftAccount.BankZip;
                    //        accountAddress.CountryID = bankAddress.CountryID;
                    //        accountAddress.Save();
                    //    }

                    //    account.Save();
                    //}                  

                    disbursementProfileEFT.NameOnAccount = account.FullName;
                    disbursementProfileEFT.RoutingNumber = eftAccount.RoutingNumber;
                    disbursementProfileEFT.AccountNumber = BankAccountNumberIsValid(eftAccount.AccountNumber) ? eftAccount.AccountNumber : disbursementProfileEFT.AccountNumber;
                    disbursementProfileEFT.BankName = eftAccount.BankName;
                    disbursementProfileEFT.BankPhone = eftAccount.BankPhone;
                    disbursementProfileEFT.AddressId = bankAddress.AddressID;
                    disbursementProfileEFT.BankAccountTypeId = (int)eftAccount.AccountType;
                    disbursementProfileEFT.EnrollmentFormReceived = agreementOnFile;

                    var savedProfile = service.SaveDisbursementProfile(disbursementProfileEFT);
                    eftAccount.DisbursementProfileId = savedProfile != null ? savedProfile.DisbursementProfileId : 0;

                    int BankId = AccountExtensions.GetBanksComissions().FirstOrDefault(x => x.BankName == disbursementProfileEFT.BankName).BankID;
                    AccountExtensions.UpdateDisbursementProfileBank(eftAccount.DisbursementProfileId, BankId);
                }
            }
        }

        protected virtual void HandleCheckDisbursementProfile(ICommissionsService service, int profileId, Account account, Address address, bool useAddressOfRecord)
        {
            // 2012-08-11, JWL, inactive existing EFT profiles
            var country = Country.GetCountriesByMarketID(account.MarketID).FirstOrDefault(a => a.Active = true);
            var profiles = service.GetDisbursementProfilesByAccountId(account.AccountID);

            var disbursementProfileCheck = (profiles.FirstOrDefault(p => p.DisbursementMethod == DisbursementMethodKind.Check) ?? Create.New<ICheckDisbursementProfile>()) as ICheckDisbursementProfile;

            if (disbursementProfileCheck == null)
            {
                throw new NullReferenceException("failed to cast profile to check disbursement profile");
            }

            var eftProfiles = profiles.Where(x => x.DisbursementMethod == DisbursementMethodKind.EFT);
            foreach (var eftProfile in eftProfiles)
            {
                if (eftProfile.DisbursementProfileId == 0)
                {
                    continue;
                }

                if (!eftProfile.IsEnabled)
                {
                    continue;
                }

                eftProfile.IsEnabled = false;
                service.SaveDisbursementProfile(eftProfile);
            }

            disbursementProfileCheck.AccountId = account.AccountID;
            disbursementProfileCheck.IsEnabled = true;
            disbursementProfileCheck.Percentage = 1;

            if (!(country == null))
            {
                disbursementProfileCheck.CurrencyId = country.CurrencyID;
            }

            Address checkAddress;
            if (!useAddressOfRecord)
            {
                // Get the check address, if it already exists, and update it.
                checkAddress = account.Addresses.FirstOrDefault(a => a.AddressTypeID == 5) ?? new Address();
                UpdateAddressInformation(checkAddress, address);
                checkAddress.ProfileName = "Dir. Pagamento";
                checkAddress.FirstName = account.FirstName;
                checkAddress.LastName = account.LastName;
                              
                var accountAddress = account.Addresses.FirstOrDefault(x => x.AddressID == checkAddress.AddressID);
                account.Addresses.Add(checkAddress);
                account.Save();

                AddressRepository addressRep = new AddressRepository();
                addressRep.UpdateAddressStreet(checkAddress);
            }
            else
            {
                // Use address of record, which I (and those I asked) interpret as the default address on the account. - Bryant Smith
                checkAddress = account.Addresses.FirstOrDefault(a => a.AddressTypeID == 1);
            }

            if (checkAddress != null)
            {
                disbursementProfileCheck.NameOnCheck = string.IsNullOrEmpty(checkAddress.Attention)
                                                        ? string.Format("{0} {1}", account.FirstName, account.LastName)
                                                        : checkAddress.Attention;
                disbursementProfileCheck.NameOnAccount = disbursementProfileCheck.NameOnCheck;
                disbursementProfileCheck.AddressId = checkAddress.AddressID;
            }

            service.SaveDisbursementProfile(disbursementProfileCheck);
        }

        protected virtual void HandlePropayDisbursementProfile()
        {
            throw new NotImplementedException();
        }

        protected bool BankAccountNumberIsValid(string accountNumber)
        {
            return !string.IsNullOrWhiteSpace(accountNumber) && !accountNumber.Contains('*');
        }

        private void UpdateAddressInformation(Address addressToUpdate, Address infoAddress)
        {
            addressToUpdate.ProfileName = infoAddress.ProfileName;
            addressToUpdate.Attention = infoAddress.Attention;
            addressToUpdate.AddressTypeID = ConstantsGenerated.AddressType.Disbursement.ToShort();
            addressToUpdate.Address1 = infoAddress.Address1;
            addressToUpdate.Address2 = infoAddress.Address2;
            addressToUpdate.Address3 = infoAddress.Address3;
            addressToUpdate.City = infoAddress.City;
            addressToUpdate.County = infoAddress.County;/*CS.07JUN2016*/
            addressToUpdate.State = infoAddress.State;
            addressToUpdate.PostalCode = infoAddress.PostalCode;
            addressToUpdate.CountryID = infoAddress.CountryID;
            addressToUpdate.Street = infoAddress.Street;
        }
    }
}
