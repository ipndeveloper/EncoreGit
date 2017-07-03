using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryKind;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;
using System.Data;
using NetSteps.Commissions.Service.Models;

namespace NetSteps.Commissions.Service.LedgerEntryKinds
{
	public class LedgerEntryKindRepository : BaseListRepository<ILedgerEntryKind, int, LedgerEntryKind, LedgerEntryKindRepository.FieldNames>, ILedgerEntryKindRepository
	{
		public enum FieldNames
		{
			LedgerEntryTypeId,
			Name,
			Enabled,
			Editable,
			Taxable,
			TermName,
			Code
		};

		protected override void SetKeyValue(LedgerEntryKind obj, int keyValue)
		{
			obj.LedgerEntryKindId = keyValue;
		}

		protected override string TableName
		{
			get { return "dbo.LedgerEntryTypes"; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.LedgerEntryTypeId; }
		}

		protected override ILedgerEntryKind ConvertFromDataReader(IDataRecord record)
		{
			var ledgerEntryKind = new LedgerEntryKind();
			ledgerEntryKind.Code = record.GetString((int)FieldNames.Code);
			ledgerEntryKind.IsEditable = record.GetBoolean((int)FieldNames.Editable);
			ledgerEntryKind.IsEnabled = record.GetBoolean((int)FieldNames.Enabled);
			ledgerEntryKind.IsTaxable = record.GetBoolean((int)FieldNames.Taxable);
			ledgerEntryKind.LedgerEntryKindId = record.GetInt32((int)FieldNames.LedgerEntryTypeId);
			ledgerEntryKind.Name = record.GetString((int)FieldNames.Name);
			ledgerEntryKind.TermName = record.GetNullable<string>((int)FieldNames.TermName) ?? ledgerEntryKind.Name;
			return ledgerEntryKind;
		}



		protected override IDictionary<FieldNames, object> GetConversionDictionary(ILedgerEntryKind obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.Code, obj.Code);
			propDictionary.Add(FieldNames.Editable, obj.IsEditable);
			propDictionary.Add(FieldNames.Enabled, obj.IsEnabled);
			propDictionary.Add(FieldNames.LedgerEntryTypeId, obj.LedgerEntryKindId);
			propDictionary.Add(FieldNames.Name, obj.Name);
			propDictionary.Add(FieldNames.Taxable, obj.IsTaxable);
			propDictionary.Add(FieldNames.TermName, obj.TermName);
			return propDictionary;
		}
	}
}
