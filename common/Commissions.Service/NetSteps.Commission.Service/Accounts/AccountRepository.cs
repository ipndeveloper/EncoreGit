using System;
using NetSteps.Commissions.Service.Interfaces.Account;
using System.Data;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Models;
using System.Collections.Generic;

namespace NetSteps.Commissions.Service.Accounts
{
    public class AccountRepository : BaseListRepository<IAccount, int, Account, AccountRepository.FieldNames>, IAccountRepository
    {
        public enum FieldNames
        {
            AccountId,
            AccountNumber,
            AccountTypeId,
            FirstName,
            MiddleName,
            LastName,
            EmailAddress,
            SponsorId,
            EnrollerId,
            EnrollmentDateUtc,
            IsEntity,
            AccountStatusChangeReasonId,
            AccountStatusId,
            EntityName,
            CountryId,
            BirthdayUtc,
            TerminatedDateUtc,
            ActivationDateUtc,
            ActivationPeriodId
        }

        protected override void SetKeyValue(Account obj, int keyValue)
        {
            obj.AccountId = keyValue;
        }

        protected override string TableName
        {
            get { return "dbo.Accounts"; }
        }

        protected override FieldNames PrimaryKeyProperty
        {
            get { return FieldNames.AccountId; }
        }

        protected override IAccount ConvertFromDataReader(IDataRecord record)
        {
            var account = new Account
            {
                AccountId = record.GetInt32((int)FieldNames.AccountId),
                AccountNumber = record.GetNullable<string>((int)FieldNames.AccountNumber),
                AccountKindId = record.GetInt32((int)FieldNames.AccountTypeId),
                AccountStatusChangeReasonId = record.GetNullable<int>((int)FieldNames.AccountStatusChangeReasonId),
                ActivationDateUtc = record.GetNullable<DateTime>((int)FieldNames.ActivationDateUtc),
                BirthdayUtc = record.GetNullable<DateTime>((int)FieldNames.BirthdayUtc),
                CountryId = record.GetInt32((int)FieldNames.CountryId),
                EmailAddress = record.GetNullable<string>((int)FieldNames.EmailAddress),
                EnrollerId = record.GetNullable<int>((int)FieldNames.EnrollerId),
                EnrollmentDateUtc = record.GetNullable<DateTime>((int)FieldNames.EnrollmentDateUtc),
                EntityName = record.GetNullable<string>((int)FieldNames.EntityName),
                FirstName = record.GetNullable<string>((int)FieldNames.FirstName),
                IsEntity = record.GetBoolean((int)FieldNames.IsEntity),
                LastName = record.GetNullable<string>((int)FieldNames.LastName),
                MiddleName = record.GetNullable<string>((int)FieldNames.MiddleName),
                SponsorId = record.GetNullable<int>((int)FieldNames.SponsorId),
                TerminatedDateUtc = record.GetNullable<DateTime>((int)FieldNames.TerminatedDateUtc),
            };

            return account;
        }

        protected override IDictionary<FieldNames, object> GetConversionDictionary(IAccount obj)
        {
            var propDictionary = new Dictionary<FieldNames, object>
            {
                {FieldNames.AccountId, obj.AccountId},
                {FieldNames.AccountNumber, obj.AccountNumber},
                {FieldNames.AccountTypeId, obj.AccountKindId},
                {FieldNames.FirstName, obj.FirstName},
                {FieldNames.MiddleName, obj.MiddleName},
                {FieldNames.LastName, obj.LastName},
                {FieldNames.EmailAddress, obj.EmailAddress},
                {FieldNames.SponsorId, obj.SponsorId},
                {FieldNames.EnrollerId, obj.EnrollerId},
                {FieldNames.EnrollmentDateUtc, obj.EnrollmentDateUtc},
                {FieldNames.IsEntity, obj.IsEntity},
                {FieldNames.AccountStatusChangeReasonId, obj.AccountStatusChangeReasonId},
                {FieldNames.AccountStatusId, obj.AccountStatusId},
                {FieldNames.EntityName, obj.EntityName},
                {FieldNames.CountryId, obj.CountryId},
                {FieldNames.BirthdayUtc, obj.BirthdayUtc},
                {FieldNames.ActivationDateUtc, obj.ActivationDateUtc},
                {FieldNames.TerminatedDateUtc, obj.TerminatedDateUtc}
            };

            return propDictionary;
        }
    }
}
