using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Interfaces.DisbursementProfile;
using System;
using System.Collections.Generic;
using NetSteps.Commissions.Service.Models;

namespace NetSteps.Commissions.Service.DisbursementProfiles
{
    public class DisbursementProfileRepository : BaseListRepository<IDisbursementProfile, int, DisbursementProfile, DisbursementProfileRepository.FieldNames>, IDisbursementProfileRepository
    {
        public enum FieldNames
        {
            DisbursementProfileId,
            Enabled,
            AccountId,
            DisbursementTypeId,
            Percentage,
            CurrencyId,
            AddressId,
            NameOnAccount,
            BankAccountNumber,
            RoutingNumber,
            PayCardAccountNumber,
            BankName,
            BankPhone,
            BankAccountTypeId,
            EnrollmentFormReceived,
            EffectiveDateUtc
        };

        protected override void SetKeyValue(DisbursementProfile obj, int keyValue)
        {
            obj.DisbursementProfileId = keyValue;
        }

        protected override string TableName
        {
            get { return "dbo.DisbursementProfiles"; }
        }

        protected override FieldNames PrimaryKeyProperty
        {
            get { return FieldNames.DisbursementProfileId; }
        }

        IDisbursementProfile ConvertEftProfileFromReader(System.Data.IDataRecord record)
        {
            var obj = new EFTDisbursementProfile
            {
                DisbursementProfileId = record.GetInt32((int)FieldNames.DisbursementProfileId),
                AccountId = record.GetInt32((int)FieldNames.AccountId),
                Percentage = record.GetNullable<decimal>((int)FieldNames.Percentage),
                IsEnabled = record.GetNullable<bool>((int)FieldNames.Enabled),
                NameOnAccount = record.GetNullable<string>((int)FieldNames.NameOnAccount),
                RoutingNumber = record.GetNullable<string>((int)FieldNames.RoutingNumber),
                BankName = record.GetNullable<string>((int)FieldNames.BankName),
                BankPhone = record.GetNullable<string>((int)FieldNames.BankPhone),
                BankAccountTypeId = record.GetNullable<int>((int)FieldNames.BankAccountTypeId),
                EnrollmentFormReceived = record.GetNullable<bool>((int)FieldNames.EnrollmentFormReceived),
                AddressId = record.GetNullable<int>((int)FieldNames.AddressId),
                AccountNumber = record.GetNullable<string>((int)FieldNames.BankAccountNumber),
                CurrencyId = record.GetInt32((int)FieldNames.CurrencyId)
            };
            return obj;
        }

        IDisbursementProfile ConvertPropayProfileFromReader(System.Data.IDataRecord record)
        {
            var obj = new PropayDisbursementProfile
            {
                DisbursementProfileId = record.GetInt32((int)FieldNames.DisbursementProfileId),
                AccountId = record.GetInt32((int)FieldNames.AccountId),
                Percentage = record.GetNullable<int>((int)FieldNames.Percentage),
                IsEnabled = record.GetNullable<bool>((int)FieldNames.Enabled),
                NameOnAccount = record.GetNullable<string>((int)FieldNames.NameOnAccount),
                PropayAccountNumber = record.GetNullable<int>((int)FieldNames.PayCardAccountNumber),
                EnrollmentFormReceived = record.GetNullable<bool>((int)FieldNames.EnrollmentFormReceived),
                CurrencyId = record.GetInt32((int)FieldNames.CurrencyId)
            };
            return obj;
        }

        IDisbursementProfile ConvertCheckProfileFromReader(System.Data.IDataRecord record)
        {
            var obj = new CheckDisbursementProfile
            {
                DisbursementProfileId = record.GetInt32((int)FieldNames.DisbursementProfileId),
                AccountId = record.GetInt32((int)FieldNames.AccountId),
                Percentage = record.GetNullable<decimal>((int)FieldNames.Percentage),
                IsEnabled = record.GetNullable<bool>((int)FieldNames.Enabled),
                NameOnAccount = record.GetNullable<string>((int)FieldNames.NameOnAccount),
                AddressId = record.GetNullable<int>((int)FieldNames.AddressId),
                EnrollmentFormReceived = record.GetNullable<bool>((int)FieldNames.EnrollmentFormReceived),
                CurrencyId = record.GetInt32((int)FieldNames.CurrencyId)
            };
            return obj;
        }

        protected override IDisbursementProfile ConvertFromDataReader(System.Data.IDataRecord record)
        {
            var disbursementMethod = (DisbursementMethodKind)(record.GetInt32((int)FieldNames.DisbursementTypeId));
            switch (disbursementMethod)
            {
                case DisbursementMethodKind.Check:
                    return ConvertCheckProfileFromReader(record);
                case DisbursementMethodKind.EFT:
                    return ConvertEftProfileFromReader(record);
                case DisbursementMethodKind.ProPay:
                    return ConvertPropayProfileFromReader(record);
                default:
                    throw new Exception("unexpected disbursement profile encountered");
            }
        }

        protected IDictionary<FieldNames, object> GetConversionDictionary(IEFTDisbursementProfile obj)
        {
            var propDictionary = new Dictionary<FieldNames, object>();
            propDictionary.Add(FieldNames.DisbursementProfileId, obj.DisbursementProfileId);
            propDictionary.Add(FieldNames.Enabled, obj.IsEnabled);
            propDictionary.Add(FieldNames.AccountId, obj.AccountId);
            propDictionary.Add(FieldNames.DisbursementTypeId, (int)obj.DisbursementMethod);
            propDictionary.Add(FieldNames.Percentage, obj.Percentage);
            propDictionary.Add(FieldNames.CurrencyId, obj.CurrencyId);
            propDictionary.Add(FieldNames.AddressId, obj.AddressId);
            propDictionary.Add(FieldNames.NameOnAccount, obj.NameOnAccount);
            propDictionary.Add(FieldNames.BankAccountNumber, obj.AccountNumber);
            propDictionary.Add(FieldNames.RoutingNumber, obj.RoutingNumber);
            //propDictionary.Add(FieldNames.PayCardAccountNumber, obj.PropayAccountNumber);
            propDictionary.Add(FieldNames.BankName, obj.BankName);
            propDictionary.Add(FieldNames.BankPhone, obj.BankPhone);
            propDictionary.Add(FieldNames.BankAccountTypeId, obj.BankAccountTypeId);
            propDictionary.Add(FieldNames.EnrollmentFormReceived, obj.EnrollmentFormReceived);
            propDictionary.Add(FieldNames.EffectiveDateUtc, DateTime.Now);

            return propDictionary;
        }

        protected IDictionary<FieldNames, object> GetConversionDictionary(ICheckDisbursementProfile obj)
        {
            var propDictionary = new Dictionary<FieldNames, object>();
            propDictionary.Add(FieldNames.DisbursementProfileId, obj.DisbursementProfileId);
            propDictionary.Add(FieldNames.Enabled, obj.IsEnabled);
            propDictionary.Add(FieldNames.AccountId, obj.AccountId);
            propDictionary.Add(FieldNames.DisbursementTypeId, (int)obj.DisbursementMethod);
            propDictionary.Add(FieldNames.Percentage, obj.Percentage);
            propDictionary.Add(FieldNames.CurrencyId, obj.CurrencyId);
            propDictionary.Add(FieldNames.AddressId, obj.AddressId);
            propDictionary.Add(FieldNames.NameOnAccount, obj.NameOnAccount);
            //propDictionary.Add(FieldNames.BankAccountNumber, obj.AccountNumber);
            //propDictionary.Add(FieldNames.RoutingNumber, obj.RoutingNumber);
            //propDictionary.Add(FieldNames.PayCardAccountNumber, obj.PropayAccountNumber);
            //propDictionary.Add(FieldNames.BankName, obj.BankName);
            //propDictionary.Add(FieldNames.BankPhone, obj.BankPhone);
            //propDictionary.Add(FieldNames.BankAccountTypeId, obj.BankAccountTypeId);
            propDictionary.Add(FieldNames.EnrollmentFormReceived, obj.EnrollmentFormReceived);
            propDictionary.Add(FieldNames.EffectiveDateUtc, DateTime.Now);

            return propDictionary;
        }

        protected IDictionary<FieldNames, object> GetConversionDictionary(IPropayDisbursementProfile obj)
        {
            var propDictionary = new Dictionary<FieldNames, object>();
            propDictionary.Add(FieldNames.DisbursementProfileId, obj.DisbursementProfileId);
            propDictionary.Add(FieldNames.Enabled, obj.IsEnabled);
            propDictionary.Add(FieldNames.AccountId, obj.AccountId);
            propDictionary.Add(FieldNames.DisbursementTypeId, (int)obj.DisbursementMethod);
            propDictionary.Add(FieldNames.Percentage, obj.Percentage);
            propDictionary.Add(FieldNames.CurrencyId, obj.CurrencyId);
            //propDictionary.Add(FieldNames.AddressId, obj.AddressId);
            propDictionary.Add(FieldNames.NameOnAccount, obj.NameOnAccount);
            //propDictionary.Add(FieldNames.BankAccountNumber, obj.AccountNumber);
            //propDictionary.Add(FieldNames.RoutingNumber, obj.RoutingNumber);
            propDictionary.Add(FieldNames.PayCardAccountNumber, obj.PropayAccountNumber);
            //propDictionary.Add(FieldNames.BankName, obj.BankName);
            //propDictionary.Add(FieldNames.BankPhone, obj.BankPhone);
            //propDictionary.Add(FieldNames.BankAccountTypeId, obj.BankAccountTypeId);
            propDictionary.Add(FieldNames.EnrollmentFormReceived, obj.EnrollmentFormReceived);
            propDictionary.Add(FieldNames.EffectiveDateUtc, DateTime.Now);

            return propDictionary;
        }

        protected override IDictionary<FieldNames, object> GetConversionDictionary(IDisbursementProfile obj)
        {
            if (obj is ICheckDisbursementProfile)
                return GetConversionDictionary(obj as ICheckDisbursementProfile);

            if (obj is IEFTDisbursementProfile)
                return GetConversionDictionary(obj as IEFTDisbursementProfile);

            if (obj is IPropayDisbursementProfile)
                return GetConversionDictionary(obj as IPropayDisbursementProfile);

            throw new Exception("unexpected Disbursement profile type");
        }

        public IEnumerable<int> GetDisbursementProfileIds(int accountId)
        {
            var dictionary = new Dictionary<FieldNames, string>();
            dictionary.Add(FieldNames.AccountId, accountId.ToString());
            return base.GetKeyList(dictionary);
        }
    }
}
