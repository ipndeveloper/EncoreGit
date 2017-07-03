using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.OverrideReason;
using NetSteps.Commissions.Service.Interfaces.OverrideReasonSource;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NetSteps.Commissions.Service.OverrideReasons
{
	public class OverrideReasonRepository : BaseListRepository<IOverrideReason, int, OverrideReason, OverrideReasonRepository.FieldNames>, IOverrideReasonRepository
	{
		protected readonly IOverrideReasonSourceProvider OverrideReasonSourceProvider;
		public OverrideReasonRepository(IOverrideReasonSourceProvider overrideReasonSourceProvider)
		{
			OverrideReasonSourceProvider = overrideReasonSourceProvider;
		}

		public enum FieldNames
		{
			OverrideReasonId,
			ReasonCode,
			Name,
			OverrideReasonSourceId,
			TermName
		};


		protected override void SetKeyValue(OverrideReason obj, int keyValue)
		{
			// to do
			//obj.OverrideReasonId = keyValue;
		}

		protected override string TableName
        {/*CGI(JCT) - MLM - 010*/
            get { return CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "OverrideReasons", "Encore"); }
		}

		protected override string ConnectionProviderName
		{
			get { return CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.OverrideReasonId; }
		}

		protected override IOverrideReason ConvertFromDataReader(IDataRecord record)
		{
			if (record == null)
			{
				throw new ArgumentNullException();
			}

			var overrideReason = new OverrideReason();
            overrideReason.Name = record.GetNullable<string>((int)FieldNames.Name);
			overrideReason.OverrideReasonId = record.GetInt32((int)FieldNames.OverrideReasonId);
            overrideReason.OverrideReasonSource = OverrideReasonSourceProvider.SingleOrDefault(x => x.OverrideReasonSourceId == record.GetNullable<int>((int)FieldNames.OverrideReasonSourceId));
			overrideReason.ReasonCode = record.GetString((int)FieldNames.ReasonCode);
            overrideReason.TermName = record.GetNullable<string>((int)FieldNames.TermName) ?? record.GetNullable<string>((int)FieldNames.Name);
			return overrideReason;
		}

		protected override IDictionary<FieldNames, object> GetConversionDictionary(IOverrideReason obj)
		{
		    var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.Name, obj.Name);
			propDictionary.Add(FieldNames.OverrideReasonId, obj.OverrideReasonId);
			propDictionary.Add(FieldNames.OverrideReasonSourceId
				, (obj.OverrideReasonSource != null) ? obj.OverrideReasonSource.OverrideReasonSourceId : 0);
			propDictionary.Add(FieldNames.ReasonCode, obj.ReasonCode);
			propDictionary.Add(FieldNames.TermName, obj.TermName);
			return propDictionary;
		}
	}
}
