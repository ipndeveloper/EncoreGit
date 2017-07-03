using System.Collections.Generic;
using NetSteps.Commissions.Service.Interfaces.DisbursementKinds;
using NetSteps.Commissions.Service.Base;

namespace NetSteps.Commissions.Service.DisbursementKinds
{
	public class DisbursementKindRepository : BaseListRepository<IDisbursementKind, int, DisbursementKind, DisbursementKindRepository.FieldNames>, IDisbursementKindRepository
	{
		public enum FieldNames
		{
			DisbursementTypeId,
			Name,
			NumberAllowed,
			Enabled,
			Editable,
			TermName,
			Code,
			DateModified
		};

		protected override void SetKeyValue(DisbursementKind obj, int keyValue)
		{
			obj.DisbursementKindId = keyValue;
		}

		protected override string TableName
		{
			get { return "dbo.DisbursementTypes"; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.DisbursementTypeId; }
		}

		protected override IDisbursementKind ConvertFromDataReader(System.Data.IDataRecord record)
		{
			var obj = new DisbursementKind
			{
				DisbursementKindId = record.GetInt32((int)FieldNames.DisbursementTypeId),
				Code = record.GetString((int)FieldNames.Code),
				DateModified = record.GetDateTime((int)FieldNames.DateModified),
				IsEditable = record.GetBoolean((int)FieldNames.Editable),
				IsEnabled = record.GetBoolean((int)FieldNames.Enabled),
				Name = record.GetString((int)FieldNames.Name),
				NumberAllowed = record.GetInt32((int)FieldNames.NumberAllowed),
				TermName = record.GetString((int)FieldNames.TermName)
			};
			return obj;
		}

		protected override IDictionary<FieldNames, object> GetConversionDictionary(IDisbursementKind obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.DisbursementTypeId, obj.DisbursementKindId);
			propDictionary.Add(FieldNames.Code, obj.Code);
			propDictionary.Add(FieldNames.DateModified, obj.DateModified);
			propDictionary.Add(FieldNames.Editable, obj.IsEditable);
			propDictionary.Add(FieldNames.Enabled, obj.IsEnabled);
			propDictionary.Add(FieldNames.Name, obj.Name);
			propDictionary.Add(FieldNames.NumberAllowed, obj.NumberAllowed);
			propDictionary.Add(FieldNames.TermName, obj.TermName);

			return propDictionary;
		}
	}
}
