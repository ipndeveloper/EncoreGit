using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.BonusKind;
using NetSteps.Commissions.Service.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Service.Models;

namespace NetSteps.Commissions.Service.BonusKinds
{
	public class BonusKindRepository : BaseListRepository<IBonusKind, int, BonusKind, BonusKindRepository.FieldNames>, IBonusKindRepository
	{
		public enum FieldNames
		{
			BonusTypeId,
			BonusCode,
			Enabled,
			Editable,
			PlanId,
			EarningsTypeId,
			TermName,
			Name,
			ClientName,
			ClientCode
		};

		protected override void SetKeyValue(BonusKind obj, int keyValue)
		{
			obj.BonusKindId = keyValue;
		}

		protected override string TableName
		{
			get { return "BonusTypes"; }
		}

		protected override string ConnectionProviderName
		{
			get { return CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse; }
		}

		protected override BonusKindRepository.FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.BonusTypeId; }
		}

		protected override IBonusKind ConvertFromDataReader(IDataRecord record)
		{
			var bonusKind = new BonusKind();
			bonusKind.BonusCode = record.GetNullable<string>((int)FieldNames.BonusCode);
			bonusKind.BonusKindId = record.GetInt32((int)FieldNames.BonusTypeId);
			bonusKind.ClientCode = record.GetNullable<string>((int)FieldNames.ClientCode);
			bonusKind.ClientName = record.GetNullable<string>((int)FieldNames.ClientName);
            bonusKind.EarningsKindId = record.GetNullable<int>((int)FieldNames.EarningsTypeId);
			bonusKind.IsEditable = record.GetBoolean((int)FieldNames.Editable);
			bonusKind.IsEnabled = record.GetBoolean((int)FieldNames.Enabled);
            bonusKind.Name = record.GetNullable<string>((int)FieldNames.Name);
            bonusKind.PlanId = record.GetNullable<int>((int)FieldNames.PlanId);
			bonusKind.TermName = record.GetNullable<string>((int)FieldNames.TermName) ?? bonusKind.Name;
			return bonusKind;
		}



		protected override IDictionary<BonusKindRepository.FieldNames, object> GetConversionDictionary(IBonusKind obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.BonusCode, obj.BonusCode);
			propDictionary.Add(FieldNames.BonusTypeId, obj.BonusKindId);
			propDictionary.Add(FieldNames.ClientCode, obj.ClientCode);
			propDictionary.Add(FieldNames.ClientName, obj.ClientName);
			propDictionary.Add(FieldNames.EarningsTypeId, obj.EarningsKindId);
			propDictionary.Add(FieldNames.Editable, obj.IsEditable);
			propDictionary.Add(FieldNames.Enabled, obj.IsEnabled);
			propDictionary.Add(FieldNames.Name, obj.Name);
			propDictionary.Add(FieldNames.PlanId, obj.PlanId);
			propDictionary.Add(FieldNames.TermName, obj.TermName);
			return propDictionary;
		}
	}
}
