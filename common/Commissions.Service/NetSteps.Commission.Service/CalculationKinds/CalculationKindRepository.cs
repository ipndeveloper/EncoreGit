using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.CalculationKind;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;
using System.Data;
using NetSteps.Commissions.Service.Models;
namespace NetSteps.Commissions.Service.CalculationKinds
{
	public class CalculationKindRepository : BaseListRepository<ICalculationKind, int, CalculationKind, CalculationKindRepository.FieldNames>, ICalculationKindRepository
	{
		public enum FieldNames
		{
			CalculationTypeId,
			Code,
			Name,
			UserOverridable,
			RealTime,
			TermName,
			DateModified,
			ReportVisibility,
			ClientCode,
			ClientName
		};

		protected override void SetKeyValue(CalculationKind obj, int keyValue)
		{
			obj.CalculationKindId = keyValue;
		}

		protected override string TableName
		{
			get { return "CalculationTypes"; }
		}

		protected override string ConnectionProviderName
		{
			get { return CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.CalculationTypeId; }
		}

		protected override ICalculationKind ConvertFromDataReader(IDataRecord record)
		{
			var calculationKind = new CalculationKind();
			calculationKind.CalculationKindId = record.GetInt32((int)FieldNames.CalculationTypeId);
			calculationKind.ClientCode = record.GetNullable<string>((int)FieldNames.ClientCode);
			calculationKind.ClientName = record.GetNullable<string>((int)FieldNames.ClientName);
            calculationKind.Code = record.GetNullable<string>((int)FieldNames.Code);
			calculationKind.DateModified = record.GetDateTime((int)FieldNames.DateModified);
			calculationKind.IsRealTime = record.GetBoolean((int)FieldNames.RealTime);
			calculationKind.IsUserOverridable = record.GetBoolean((int)FieldNames.UserOverridable);
            calculationKind.Name = record.GetNullable<string>((int)FieldNames.Name);
            calculationKind.ReportVisibility = record.GetNullable<bool>((int)FieldNames.ReportVisibility);
            calculationKind.TermName = record.GetNullable<string>((int)FieldNames.TermName);
			return calculationKind;
		}



		protected override IDictionary<FieldNames, object> GetConversionDictionary(ICalculationKind obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.CalculationTypeId, obj.CalculationKindId);
			propDictionary.Add(FieldNames.ClientCode, obj.ClientCode);
			propDictionary.Add(FieldNames.ClientName, obj.ClientName);
			propDictionary.Add(FieldNames.Code, obj.Code);
			propDictionary.Add(FieldNames.DateModified, obj.DateModified);
			propDictionary.Add(FieldNames.Name, obj.Name);
			propDictionary.Add(FieldNames.RealTime, obj.IsRealTime);
			propDictionary.Add(FieldNames.ReportVisibility, obj.ReportVisibility);
			propDictionary.Add(FieldNames.TermName, obj.TermName);
			propDictionary.Add(FieldNames.UserOverridable, obj.IsUserOverridable);
			return propDictionary;
		}
	}
}
