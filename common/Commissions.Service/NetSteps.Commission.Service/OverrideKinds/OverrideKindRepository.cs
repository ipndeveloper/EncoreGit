using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using System;
using System.Collections.Generic;
using System.Data;
using NetSteps.Commissions.Service.Models;
using NetSteps.Commissions.Service.Interfaces.OverrideKind;

namespace NetSteps.Commissions.Service.OverrideKinds
{
	public class OverrideKindRepository : BaseListRepository<IOverrideKind, int, OverrideKind, OverrideKindRepository.FieldNames>, IOverrideKindRepository
	{
		public enum FieldNames
		{
			OverrideTypeId,
			OverrideCode,
			Description,
			Operator
        };


		protected override void SetKeyValue(OverrideKind obj, int keyValue)
		{
			// commented out initially - we currently don't need to be adding items to this set
			//obj.OverrideKindId = keyValue;
		}

		protected override string TableName
        {/*CGI(JCT) - MLM - 010*/
            get { return CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "OverrideTypes", "Encore"); }
		}

		protected override string ConnectionProviderName
		{
			get { return CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.OverrideTypeId; }
		}

		protected override IOverrideKind ConvertFromDataReader(IDataRecord record)
		{
			if (record == null)
			{
				throw new ArgumentNullException();
			}

			var overrideKind = new OverrideKind();
			overrideKind.Description = record.GetNullable<string>((int)FieldNames.Description);
			overrideKind.Operator = record.GetNullable<string>((int)FieldNames.Operator);
			overrideKind.OverrideCode = record.GetString((int)FieldNames.OverrideCode);
			overrideKind.OverrideKindId = record.GetInt32((int)FieldNames.OverrideTypeId);
			return overrideKind;
		}



		protected override IDictionary<FieldNames, object> GetConversionDictionary(IOverrideKind obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.Description, obj.Description);
			propDictionary.Add(FieldNames.Operator, obj.Operator);
			propDictionary.Add(FieldNames.OverrideCode, obj.OverrideCode);
			propDictionary.Add(FieldNames.OverrideTypeId, obj.OverrideKindId);
			return propDictionary;
		}
	}
}
