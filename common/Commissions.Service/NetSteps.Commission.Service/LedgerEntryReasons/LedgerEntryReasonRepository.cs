using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryReason;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;
using System.Data;
using NetSteps.Commissions.Service.Models;

namespace NetSteps.Commissions.Service.LedgerEntryReasons
{
	public class LedgerEntryReasonRepository : BaseListRepository<ILedgerEntryReason, int, LedgerEntryReason, LedgerEntryReasonRepository.FieldNames>, ILedgerEntryReasonRepository
	{
		public enum FieldNames
		{
			EntryReasonId,
			Name,
			Enabled,
			Editable,
			TermName,
			Code
		};

		protected override void SetKeyValue(LedgerEntryReason obj, int keyValue)
		{
			obj.EntryReasonId = keyValue;
		}

		protected override string TableName
		{
			get { return "dbo.LedgerEntryReasons"; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.EntryReasonId; }
		}

		protected override ILedgerEntryReason ConvertFromDataReader(IDataRecord record)
		{
			var ledgerEntryReason = new LedgerEntryReason();
			ledgerEntryReason.EntryReasonId = record.GetInt32((int)FieldNames.EntryReasonId);
			ledgerEntryReason.Code = record.GetString((int)FieldNames.Code);
			ledgerEntryReason.IsEditable = record.GetBoolean((int)FieldNames.Editable);
			ledgerEntryReason.IsEnabled = record.GetBoolean((int)FieldNames.Enabled);
			ledgerEntryReason.Name = record.GetString((int)FieldNames.Name);
            ledgerEntryReason.TermName = record.GetNullable<string>((int)FieldNames.TermName) ?? record.GetString((int)FieldNames.Name);
			return ledgerEntryReason;
		}
		
		protected override IDictionary<FieldNames, object> GetConversionDictionary(ILedgerEntryReason obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.Code, obj.Code);
			propDictionary.Add(FieldNames.Editable, obj.IsEditable);
			propDictionary.Add(FieldNames.Enabled, obj.IsEnabled);
			propDictionary.Add(FieldNames.EntryReasonId, obj.EntryReasonId);
			propDictionary.Add(FieldNames.Name, obj.Name);
			propDictionary.Add(FieldNames.TermName, obj.TermName);
			return propDictionary;
		}
	}
}
