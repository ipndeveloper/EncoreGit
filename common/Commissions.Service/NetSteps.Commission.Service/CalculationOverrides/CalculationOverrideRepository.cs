using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Interfaces.CalculationKind;
using NetSteps.Commissions.Service.Interfaces.CalculationOverride;
using NetSteps.Commissions.Service.Interfaces.OverrideKind;
using NetSteps.Commissions.Service.Interfaces.OverrideReason;
using NetSteps.Commissions.Service.Interfaces.Period;
using NetSteps.Commissions.Service.Models;
using NetSteps.Commissions.Service.Operations;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;

namespace NetSteps.Commissions.Service.CalculationOverrides
{
	public class CalculationOverrideRepository : BaseListRepository<ICalculationOverride, int, CalculationOverride, CalculationOverrideRepository.FieldNames>, ICalculationOverrideRepository
	{
		private readonly ICalculationKindProvider _calculationKindProvider;
		private readonly IOverrideKindProvider _overrideKindProvider;
		private readonly IOverrideReasonProvider _overrideReasonProvider;
		private readonly IPeriodProvider _periodProvider;

		public CalculationOverrideRepository(
			ICalculationKindProvider calculationKindProvider
			, IOverrideKindProvider overrideKindProvider
			, IOverrideReasonProvider overrideReasonProvider
			, IPeriodProvider periodProvider)
		{
			_calculationKindProvider = calculationKindProvider;
			_overrideKindProvider = overrideKindProvider;
			_overrideReasonProvider = overrideReasonProvider;
			_periodProvider = periodProvider;
		}

		public enum FieldNames
		{
			CalculationOverrideId,
			CalculationTypeId,
			AccountId,
			PeriodId,
			NewValue,
			OverrideTypeId,
			OverrideReasonId,
			OverrideIfNull,
			Note,
			UserId,
			ApplicationSourceId,
			CreatedDate,
			UpdatedDate
		};

		protected override void SetKeyValue(CalculationOverride obj, int keyValue)
		{
			obj.CalculationOverrideId = keyValue;
		}

		protected override string TableName
		{
			get { return "CalculationOverrides"; }
		}

		protected override string ConnectionProviderName
		{
			get { return CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.CalculationOverrideId; }
		}

		protected override ICalculationOverride ConvertFromDataReader(System.Data.IDataRecord record)
		{
			var obj = new CalculationOverride();
			obj.AccountId = record.GetInt32((int)FieldNames.AccountId);
            obj.ApplicationSourceId = record.GetNullable<int>((int)FieldNames.ApplicationSourceId);
			obj.CalculationOverrideId = record.GetInt32((int)FieldNames.CalculationOverrideId);
			obj.CalculationKind = _calculationKindProvider.FirstOrDefault(x => x.CalculationKindId == record.GetInt32((int)FieldNames.CalculationTypeId));
			obj.CreatedDateUTC = record.GetDateTime((int)FieldNames.CreatedDate);
            obj.NewValue = record.GetNullable<decimal>((int)FieldNames.NewValue);
            obj.Notes = record.GetNullable<string>((int)FieldNames.Note);
			obj.OverrideIfNull = record.GetBoolean((int) FieldNames.OverrideIfNull);
			obj.OverrideKind = _overrideKindProvider.FirstOrDefault(x => x.OverrideKindId == record.GetInt32((int)FieldNames.OverrideTypeId));
			obj.OverrideReason = _overrideReasonProvider.FirstOrDefault(x => x.OverrideReasonId == record.GetInt32((int)FieldNames.OverrideReasonId));
			obj.Period = _periodProvider.FirstOrDefault(x => x.PeriodId == record.GetInt32((int)FieldNames.PeriodId));
			obj.UpdatedDateUTC = record.GetDateTime((int)FieldNames.UpdatedDate);
            obj.UserId = record.GetNullable<int>((int)FieldNames.UserId);
			return obj;
		}
        
        protected override IDictionary<FieldNames, object> GetConversionDictionary(ICalculationOverride obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.AccountId, obj.AccountId);
			propDictionary.Add(FieldNames.ApplicationSourceId, obj.ApplicationSourceId);
			propDictionary.Add(FieldNames.CalculationOverrideId, obj.CalculationOverrideId);
			if (obj.CalculationKind != null) propDictionary.Add(FieldNames.CalculationTypeId, obj.CalculationKind.CalculationKindId);
			propDictionary.Add(FieldNames.CreatedDate, obj.CreatedDateUTC);
			propDictionary.Add(FieldNames.NewValue, obj.NewValue);
			propDictionary.Add(FieldNames.Note, obj.Notes);
			propDictionary.Add(FieldNames.OverrideIfNull, obj.OverrideIfNull);
			if (obj.OverrideReason != null) propDictionary.Add(FieldNames.OverrideReasonId, obj.OverrideReason.OverrideReasonId);
			if (obj.OverrideKind != null) propDictionary.Add(FieldNames.OverrideTypeId, obj.OverrideKind.OverrideKindId);
			if (obj.Period != null) propDictionary.Add(FieldNames.PeriodId, obj.Period.PeriodId);
			propDictionary.Add(FieldNames.UpdatedDate, obj.UpdatedDateUTC);
			propDictionary.Add(FieldNames.UserId, obj.UserId);
			return propDictionary;
		}

		public ICalculationOverrideSearchResult SearchCalculationOverrides(CalculationOverrideSearchParameters parameters)
		{
            var parameterList = new List<IOperation>();
            if (parameters.AccountId.HasValue)
            {
                parameterList.Add(new EqualsOperation(FieldNames.AccountId.ToString(), (int)parameters.AccountId));
            }

            if (parameters.CalculationOverrideId.HasValue)
            {
                parameterList.Add(new EqualsOperation(FieldNames.CalculationOverrideId.ToString(), (int)parameters.CalculationOverrideId));
            }

            if (parameters.OverrideReasonId.HasValue)
            {
                parameterList.Add(new EqualsOperation(FieldNames.OverrideReasonId.ToString(), (int)parameters.OverrideReasonId));
            }

            if (parameters.StartDate.HasValue)
            {
                var startDateUtc = parameters.StartDate.Value.StartOfDay().LocalToUTC();
                parameterList.Add(new GreaterThanOperation(FieldNames.CreatedDate.ToString(), startDateUtc, true));
            }

            if (parameters.EndDate.HasValue)
            {
                var endDateUtc = parameters.EndDate.Value.EndOfDay().LocalToUTC();
                parameterList.Add(new LessThanOperation(FieldNames.CreatedDate.ToString(), endDateUtc, true));
            }

            var search = new BaseSearchProvider<CalculationOverrideSearchResult, ICalculationOverride>();
            return search.Search(parameterList, TableName, FieldNames.CalculationOverrideId.ToString(), parameters.OrderByString, ConvertFromDataReader, ConnectionProvider);
        }
	}
}
