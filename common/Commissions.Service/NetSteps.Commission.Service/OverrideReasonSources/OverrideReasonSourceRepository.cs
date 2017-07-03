using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.OverrideReasonSource;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace NetSteps.Commissions.Service.OverrideReasonSources
{
	public class OverrideReasonSourceRepository : BaseListRepository<IOverrideReasonSource, int, OverrideReasonSource, OverrideReasonSourceRepository.FieldNames>, IOverrideReasonSourceRepository
	{
		public enum FieldNames
		{
			OverrideReasonSourceId,
			Code,
			Description,
			TermName,
			Name
		};


		protected override void SetKeyValue(OverrideReasonSource obj, int keyValue)
		{
			// to do
			//obj.OverrideReasonSourceId = keyValue;
		}

		protected override string TableName
		{
			get { return "dbo.OverrideReasonSources"; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.OverrideReasonSourceId; }
		}

		protected override IOverrideReasonSource ConvertFromDataReader(IDataRecord record)
		{
			if (record == null)
			{
				throw new ArgumentNullException();
			}

			var source = new OverrideReasonSource();
            source.Code = record.GetNullable<string>((int)FieldNames.Code);
            source.Description = record.GetNullable<string>((int)FieldNames.Description);
            source.Name = record.GetNullable<string>((int)FieldNames.Name);
			source.OverrideReasonSourceId = record.GetInt32((int)FieldNames.OverrideReasonSourceId);
            source.TermName = record.GetNullable<string>((int)FieldNames.TermName) ?? record.GetNullable<string>((int)FieldNames.Name);
			return source;
		}

		protected override IDictionary<FieldNames, object> GetConversionDictionary(IOverrideReasonSource obj)
		{
		    var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.Code, obj.Code);
			propDictionary.Add(FieldNames.Description, obj.Description);
			propDictionary.Add(FieldNames.Name, obj.Name);
			propDictionary.Add(FieldNames.OverrideReasonSourceId, obj.OverrideReasonSourceId);
			propDictionary.Add(FieldNames.TermName, obj.TermName);
			return propDictionary;
		}
	}
}
