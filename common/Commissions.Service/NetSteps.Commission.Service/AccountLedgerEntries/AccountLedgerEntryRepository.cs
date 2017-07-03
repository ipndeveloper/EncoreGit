using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.AccountLedger;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryKind;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryOrigin;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryReason;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NetSteps.Commissions.Service.AccountLedgerEntries
{
    public class AccountLedgerEntryRepository : BaseListRepository<IAccountLedgerEntry, int, AccountLedgerEntry, AccountLedgerEntryRepository.FieldNames>, IAccountLedgerEntryRepository
    {
        private readonly ILedgerEntryKindProvider _ledgerEntryKindProvider;
        private readonly ILedgerEntryOriginProvider _ledgerEntryOriginProvider;
        private readonly ILedgerEntryReasonProvider _ledgerEntryReasonProvider;

        public AccountLedgerEntryRepository(ILedgerEntryKindProvider ledgerEntryKindProvider, ILedgerEntryOriginProvider ledgerEntryOriginProvider, ILedgerEntryReasonProvider ledgerEntryReasonProvider)
        {
            _ledgerEntryKindProvider = ledgerEntryKindProvider;
            _ledgerEntryOriginProvider = ledgerEntryOriginProvider;
            _ledgerEntryReasonProvider = ledgerEntryReasonProvider;
        }

        public enum FieldNames
        {
            AccountId,
            EntryId,
            EntryDescription,
            EntryReasonId,
            EntryOriginId,
            EntryTypeId,
            UserId,
            EntryNotes,
            EntryAmount,
            EntryDate,
            EffectiveDate,
            BonusTypeId,
            DisbursementId,
            BonusValueId,
            CurrencyTypeId,
            EndingBalance,
        };

        protected override void SetKeyValue(AccountLedgerEntry obj, int keyValue)
        {
            obj.EntryId = keyValue;
        }

        protected override string TableName
        {
            get { return "dbo.AccountLedger"; }
        }

        protected override FieldNames PrimaryKeyProperty
        {
            get { return FieldNames.EntryId; }
        }

        protected override IAccountLedgerEntry ConvertFromDataReader(IDataRecord record)
        {
            var accountLedgerEntry = new AccountLedgerEntry
            {
                AccountId = record.GetInt32((int)FieldNames.AccountId),
                EntryId = record.GetInt32((int)FieldNames.EntryId),
                BonusTypeId = record.GetNullable<int>((int)FieldNames.BonusTypeId),
                BonusValueId = record.GetNullable<int>((int)FieldNames.BonusValueId),
                CurrencyTypeId = record.GetInt32((int)FieldNames.CurrencyTypeId),
                EffectiveDate = record.GetDateTime((int)FieldNames.EffectiveDate),
                EndingBalance = record.GetNullable<decimal>((int)FieldNames.EndingBalance),
                EntryAmount = record.GetDecimal((int)FieldNames.EntryAmount),
                EntryDate = record.GetDateTime((int)FieldNames.EntryDate),
                EntryDescription = record.GetString((int)FieldNames.EntryDescription),
                EntryKind = _ledgerEntryKindProvider.Single(x => x.LedgerEntryKindId == record.GetInt32((int)FieldNames.EntryTypeId)),
                EntryNotes = record.GetNullable<string>((int)FieldNames.EntryNotes),
                EntryOrigin = _ledgerEntryOriginProvider.Single(x => x.EntryOriginId == record.GetInt32((int)FieldNames.EntryOriginId)),
                EntryReason = _ledgerEntryReasonProvider.Single(x => x.EntryReasonId == record.GetInt32((int)FieldNames.EntryReasonId)),
                UserId = record.GetInt32((int)FieldNames.UserId)
            };
            return accountLedgerEntry;
        }



        protected override IDictionary<FieldNames, object> GetConversionDictionary(IAccountLedgerEntry obj)
        {
            var propDictionary = new Dictionary<FieldNames, object>
		    {
                {FieldNames.AccountId, obj.AccountId},
                {FieldNames.BonusTypeId,obj.BonusTypeId},
                {FieldNames.BonusValueId,obj.BonusValueId},
                {FieldNames.CurrencyTypeId,obj.CurrencyTypeId},
                {FieldNames.EffectiveDate,obj.EffectiveDate},
                {FieldNames.EntryAmount,obj.EntryAmount},
                {FieldNames.EntryDate,obj.EntryDate},
                {FieldNames.EntryDescription,obj.EntryDescription},
                {FieldNames.EntryNotes,obj.EntryNotes},
                {FieldNames.EntryOriginId,obj.EntryOrigin.EntryOriginId},
                {FieldNames.EntryReasonId,obj.EntryReason.EntryReasonId},
                {FieldNames.EntryTypeId,obj.EntryKind.LedgerEntryKindId},
                {FieldNames.UserId,obj.UserId}
		    };
            return propDictionary;
        }

        public IEnumerable<int> GetAccountLedgerEntryIds(int accountId)
        {
            var dictionary = new Dictionary<FieldNames, string>
            {
                {FieldNames.AccountId, accountId.ToString()}
            };
            return base.GetKeyList(dictionary);
        }

    }
}
