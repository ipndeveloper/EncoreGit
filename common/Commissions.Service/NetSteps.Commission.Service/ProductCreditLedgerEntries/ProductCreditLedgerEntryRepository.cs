using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryKind;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryOrigin;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryReason;
using NetSteps.Commissions.Service.Interfaces.ProductCreditLedger;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetSteps.Commissions.Service.Models;

namespace NetSteps.Commissions.Service.ProductCreditLedgerEntries
{
	public class ProductCreditLedgerEntryRepository : BaseListRepository<IProductCreditLedgerEntry, int, ProductCreditLedgerEntry, ProductCreditLedgerEntryRepository.FieldNames>, IProductCreditLedgerEntryRepository
	{
		private readonly ILedgerEntryKindProvider _ledgerEntryKindProvider;
		private readonly ILedgerEntryOriginProvider _ledgerEntryOriginProvider;
		private readonly ILedgerEntryReasonProvider _ledgerEntryReasonProvider;

		public ProductCreditLedgerEntryRepository(ILedgerEntryKindProvider ledgerEntryKindProvider, ILedgerEntryOriginProvider ledgerEntryOriginProvider, ILedgerEntryReasonProvider ledgerEntryReasonProvider)
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
			BonusValueId,
			CurrencyTypeId,
			EndingBalance,
			OrderId,
			OrderPaymentId
		};

		protected override void SetKeyValue(ProductCreditLedgerEntry obj, int keyValue)
		{
			obj.EntryId = keyValue;
		}

		protected override string TableName
		{
			get { return "dbo.ProductCreditLedger"; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.EntryId; }
		}

		protected override IProductCreditLedgerEntry ConvertFromDataReader(IDataRecord record)
		{
			var productCreditLedgerEntry = new ProductCreditLedgerEntry();
			productCreditLedgerEntry.EntryId = record.GetInt32((int)FieldNames.EntryId);
			productCreditLedgerEntry.AccountId = record.GetInt32((int)FieldNames.AccountId);
			productCreditLedgerEntry.BonusTypeId = record.GetNullable<int>((int)FieldNames.BonusTypeId);
			productCreditLedgerEntry.BonusValueId = record.GetNullable<int>((int)FieldNames.BonusValueId);
			productCreditLedgerEntry.CurrencyTypeId = record.GetInt32((int)FieldNames.CurrencyTypeId);
			productCreditLedgerEntry.EffectiveDate = record.GetDateTime((int)FieldNames.EffectiveDate);
			productCreditLedgerEntry.EndingBalance = record.GetDecimal((int)FieldNames.EndingBalance);
			productCreditLedgerEntry.EntryAmount = record.GetDecimal((int)FieldNames.EntryAmount);
			productCreditLedgerEntry.EntryDate = record.GetDateTime((int)FieldNames.EntryDate);
			productCreditLedgerEntry.EntryDescription = record.GetString((int)FieldNames.EntryDescription);
			productCreditLedgerEntry.EntryKind = _ledgerEntryKindProvider.Single(x => x.LedgerEntryKindId == record.GetInt32((int)FieldNames.EntryTypeId));
            productCreditLedgerEntry.EntryNotes = record.GetNullable<string>((int)FieldNames.EntryNotes);
			productCreditLedgerEntry.EntryOrigin = _ledgerEntryOriginProvider.Single(x => x.EntryOriginId == record.GetInt32((int)FieldNames.EntryOriginId));
			productCreditLedgerEntry.EntryReason = _ledgerEntryReasonProvider.Single(x => x.EntryReasonId == record.GetInt32((int)FieldNames.EntryReasonId));
			productCreditLedgerEntry.OrderId = record.GetNullable<int>((int)FieldNames.OrderId);
			productCreditLedgerEntry.OrderPaymentId = record.GetNullable<int>((int)FieldNames.OrderPaymentId);
			productCreditLedgerEntry.UserId = record.GetInt32((int)FieldNames.UserId);
			return productCreditLedgerEntry;
		}



		protected override IDictionary<FieldNames, object> GetConversionDictionary(IProductCreditLedgerEntry obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.AccountId, obj.AccountId);
			propDictionary.Add(FieldNames.BonusTypeId, obj.BonusTypeId);
			propDictionary.Add(FieldNames.BonusValueId, obj.BonusValueId);
			propDictionary.Add(FieldNames.CurrencyTypeId, obj.CurrencyTypeId);
			propDictionary.Add(FieldNames.EffectiveDate, obj.EffectiveDate);
			propDictionary.Add(FieldNames.EntryAmount, obj.EntryAmount);
			propDictionary.Add(FieldNames.EntryDate, obj.EntryDate);
			propDictionary.Add(FieldNames.EntryDescription, obj.EntryDescription);
			propDictionary.Add(FieldNames.EntryNotes, obj.EntryNotes);
			propDictionary.Add(FieldNames.EntryOriginId, obj.EntryOrigin.EntryOriginId);
			propDictionary.Add(FieldNames.EntryReasonId, obj.EntryReason.EntryReasonId);
			propDictionary.Add(FieldNames.EntryTypeId, obj.EntryKind.LedgerEntryKindId);
			propDictionary.Add(FieldNames.OrderId, obj.OrderId);
			propDictionary.Add(FieldNames.OrderPaymentId, obj.OrderPaymentId);
			propDictionary.Add(FieldNames.UserId, obj.UserId);
			return propDictionary;
		}

		public IEnumerable<int> GetProductCreditLedgerEntryIds(int accountId)
		{
			var dictionary = new Dictionary<FieldNames, string>();
			dictionary.Add(FieldNames.AccountId, accountId.ToString());
			return base.GetKeyList(dictionary);
		}
	}
}
