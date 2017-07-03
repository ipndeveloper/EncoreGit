using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.CommissionPlan;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Models;
using System.Collections.Generic;
using System.Data;

namespace NetSteps.Commissions.Service.CommissionPlans
{
	public class CommissionPlanRepository : BaseListRepository<ICommissionPlan, int, CommissionPlan, CommissionPlanRepository.FieldNames>, ICommissionPlanRepository
	{
		public enum FieldNames
		{
			PlanId,
			PlanCode,
			Name,
			Enabled,
			DefaultPlan,
			TermName
		};

		protected override void SetKeyValue(CommissionPlan obj, int keyValue)
		{
			obj.DisbursementFrequency = (DisbursementFrequencyKind)keyValue;
		}

		protected override string TableName
		{
			get { return "dbo.Plans"; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.PlanId; }
		}

		protected override ICommissionPlan ConvertFromDataReader(IDataRecord record)
		{
			var plan = new CommissionPlan();
			plan.DisbursementFrequency = (DisbursementFrequencyKind)record.GetInt32((int)FieldNames.PlanId);
            plan.IsDefault = record.GetNullable<bool>((int)FieldNames.DefaultPlan);
			plan.IsEnabled = record.GetBoolean((int)FieldNames.Enabled);
            plan.Name = record.GetNullable<string>((int)FieldNames.Name);
			plan.PlanCode = record.GetString((int)FieldNames.PlanCode);
            plan.TermName = record.IsDBNull((int)FieldNames.TermName) ? record.GetNullable<string>((int)FieldNames.Name) : record.GetString((int)FieldNames.TermName);
			return plan;
		}



		protected override IDictionary<FieldNames, object> GetConversionDictionary(ICommissionPlan obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.DefaultPlan, obj.IsDefault);
			propDictionary.Add(FieldNames.Enabled, obj.IsEnabled);
			propDictionary.Add(FieldNames.Name, obj.Name);
			propDictionary.Add(FieldNames.PlanCode, obj.PlanCode);
			propDictionary.Add(FieldNames.PlanId, obj.CommissionPlanId);
			propDictionary.Add(FieldNames.TermName, obj.TermName);
			return propDictionary;
		}
	}
}
