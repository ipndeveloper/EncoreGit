using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Commissions.Service.Interfaces.DisbursementProfile;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Models;
using NetSteps.Core.Cache;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service.DisbursementProfiles
{
    public class DisbursementProfileResolver : IDisbursementProfileResolver
    {
        private readonly IDisbursementProfileProvider _provider;

        public DisbursementProfileResolver(IDisbursementProfileProvider provider)
        {
            _provider = provider;
        }

        private IEFTDisbursementProfile ResolveEftProfile(int profileId, IAccount account, int addressId, int currencyId, bool agreementOnFile, IEFTAccount eftAccount)
        {
            var disbursementProfile = (_provider.Get(profileId) ?? Create.New<IEFTDisbursementProfile>()) as EFTDisbursementProfile;
            if (disbursementProfile == null)
            {
                return null;
            }

            disbursementProfile.DisbursementProfileId = profileId;
            disbursementProfile.AccountId = account.AccountId;
            disbursementProfile.AccountNumber = eftAccount.AccountNumber;
            disbursementProfile.AddressId = addressId;
            disbursementProfile.BankAccountTypeId = (int)eftAccount.AccountType;
            disbursementProfile.BankName = eftAccount.BankName;
            disbursementProfile.BankPhone = eftAccount.BankPhone;
            disbursementProfile.EnrollmentFormReceived = agreementOnFile;
            disbursementProfile.IsEnabled = eftAccount.IsEnabled;
            disbursementProfile.NameOnAccount = eftAccount.Name;
            disbursementProfile.Percentage = eftAccount.PercentToDeposit;
            disbursementProfile.RoutingNumber = eftAccount.RoutingNumber;
            disbursementProfile.CurrencyId = currencyId;

            return disbursementProfile;
        }

        private ICheckDisbursementProfile ResolveCheckProfile(int profileId, IAccount account, int addressId, int currencyId, bool agreementOnFile, IEFTAccount eftAccount)
        {
            var disbursementProfile = (_provider.Get(profileId) ?? Create.New<ICheckDisbursementProfile>()) as CheckDisbursementProfile;
            if (disbursementProfile == null)
            {
                return null;
            }

            disbursementProfile.DisbursementProfileId = profileId;
            disbursementProfile.AccountId = account.AccountId;
            disbursementProfile.AddressId = addressId;
            disbursementProfile.EnrollmentFormReceived = agreementOnFile;
            disbursementProfile.IsEnabled = eftAccount.IsEnabled;
            disbursementProfile.NameOnAccount = eftAccount.Name;
            disbursementProfile.Percentage = eftAccount.PercentToDeposit;
            disbursementProfile.CurrencyId = currencyId;

            return disbursementProfile;
        }

        private IPropayDisbursementProfile ResolvePropayProfile(int profileId, IAccount account, int currencyId, bool agreementOnFile, IEFTAccount eftAccount)
        {
            var disbursementProfile = (_provider.Get(profileId) ?? Create.New<IPropayDisbursementProfile>()) as PropayDisbursementProfile;
            if (disbursementProfile == null)
            {
                return null;
            }

            disbursementProfile.DisbursementProfileId = profileId;
            disbursementProfile.AccountId = account.AccountId;
            disbursementProfile.EnrollmentFormReceived = agreementOnFile;
            disbursementProfile.IsEnabled = eftAccount.IsEnabled;
            disbursementProfile.NameOnAccount = eftAccount.Name;
            disbursementProfile.Percentage = eftAccount.PercentToDeposit;
            disbursementProfile.PropayAccountNumber = default(int);
            disbursementProfile.CurrencyId = currencyId;

            return disbursementProfile;
        }

        public IDisbursementProfile Resolve(int profileId, IAccount account, int addressId, int currencyId, DisbursementMethodKind disbursementMethod, bool useAddressOfRecord, bool agreementOnFile, IEFTAccount eftAccount)
        {
            switch (disbursementMethod)
            {
                case DisbursementMethodKind.EFT:
                    return ResolveEftProfile(profileId, account, addressId, currencyId, agreementOnFile, eftAccount);
                case DisbursementMethodKind.Check:
                    return ResolveCheckProfile(profileId, account, addressId, currencyId, agreementOnFile, eftAccount);
                case DisbursementMethodKind.ProPay:
                    return ResolvePropayProfile(profileId, account, currencyId, agreementOnFile, eftAccount);
                default:
                    throw new Exception(string.Format("trying to resolve unexpected disbursement kind {0}", disbursementMethod));
            }
        }


        public IEnumerable<IDisbursementProfile> Resolve(int profileId, IAccount account, int addressId, int currencyId, DisbursementMethodKind disbursementMethod, bool useAddressOfRecord, bool agreementOnFile, IEnumerable<IEFTAccount> eftAccounts)
        {
            return eftAccounts.Select(x => Resolve(profileId, account, addressId, currencyId, disbursementMethod, useAddressOfRecord, agreementOnFile, x));
        }
    }
}
