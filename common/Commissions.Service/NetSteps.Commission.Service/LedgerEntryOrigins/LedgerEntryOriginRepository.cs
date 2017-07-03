using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryOrigin;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;
using System.Data;
using NetSteps.Commissions.Service.Models;

namespace NetSteps.Commissions.Service.LedgerEntryOrigins
{
	public class LedgerEntryOriginRepository : BaseListRepository<ILedgerEntryOrigin, int, LedgerEntryOrigin, LedgerEntryOriginRepository.FieldNames>, ILedgerEntryOriginRepository
	{
		public enum FieldNames
		{
			EntryOriginId,
			Name,
			Enabled,
			Editable,
			TermName,
			Code
		};

		protected override void SetKeyValue(LedgerEntryOrigin obj, int keyValue)
		{
			obj.EntryOriginId = keyValue;
		}

		protected override string TableName
		{
			get { return "dbo.LedgerEntryOrigins"; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.EntryOriginId; }
		}

		protected override ILedgerEntryOrigin ConvertFromDataReader(IDataRecord record)
		{
			var ledgerEntryOrigin = new LedgerEntryOrigin();
			ledgerEntryOrigin.Code = record.GetString((int)FieldNames.Code);
			ledgerEntryOrigin.EntryOriginId = record.GetInt32((int)FieldNames.EntryOriginId);
			ledgerEntryOrigin.IsEditable = record.GetBoolean((int)FieldNames.Editable);
			ledgerEntryOrigin.IsEnabled = record.GetBoolean((int)FieldNames.Enabled);
			ledgerEntryOrigin.Name = record.GetString((int)FieldNames.Name);
            ledgerEntryOrigin.TermName = record.GetNullable<string>((int)FieldNames.TermName) ?? record.GetString((int)FieldNames.Name);
			return ledgerEntryOrigin;
		}



		protected override IDictionary<FieldNames, object> GetConversionDictionary(ILedgerEntryOrigin obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.Code, obj.Code);
			propDictionary.Add(FieldNames.Editable, obj.IsEditable);
			propDictionary.Add(FieldNames.Enabled, obj.IsEnabled);
			propDictionary.Add(FieldNames.EntryOriginId, obj.EntryOriginId);
			propDictionary.Add(FieldNames.Name, obj.Name);
			propDictionary.Add(FieldNames.TermName, obj.TermName);
			return propDictionary;
		}
	}
}
