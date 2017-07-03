using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.TitleKind;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;
using System.Data;
using NetSteps.Commissions.Service.Models;

namespace NetSteps.Commissions.Service.TitleKinds
{
	public class TitleKindRepository : BaseListRepository<ITitleKind, int, TitleKind, TitleKindRepository.FieldNames>, ITitleKindRepository
	{
		public enum FieldNames
		{
			TitleTypeId,
			TitleTypeCode,
			Name,
			TermName
		};

		protected override void SetKeyValue(TitleKind obj, int keyValue)
		{
			obj.TitleKindId = keyValue;
		}

		protected override string TableName
        {/*CGI(JCT) - MLM - 010*/
            get { return CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "TitleTypes", "Encore"); }
		}

		protected override string ConnectionProviderName
		{
			get { return CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.TitleTypeId; }
		}

		protected override ITitleKind ConvertFromDataReader(IDataRecord record)
		{
			var title = new TitleKind();
			title.Name = record.GetString((int)FieldNames.Name);
            title.TermName = record.GetNullable<string>((int)FieldNames.TermName) ?? record.GetString((int)FieldNames.Name);
			title.TitleKindCode = record.GetString((int)FieldNames.TitleTypeCode);
			title.TitleKindId = record.GetInt32((int)FieldNames.TitleTypeId);
			return title;
		}

		protected override IDictionary<FieldNames, object> GetConversionDictionary(ITitleKind obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.Name, obj.Name);
			propDictionary.Add(FieldNames.TermName, obj.TermName);
			propDictionary.Add(FieldNames.TitleTypeCode, obj.TitleKindCode);
			propDictionary.Add(FieldNames.TitleTypeId, obj.TitleKindId);
			return propDictionary;
		}
	}
}
