using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.AccountTitleOverride;
using NetSteps.Commissions.Service.Interfaces.OverrideKind;
using NetSteps.Commissions.Service.Interfaces.OverrideReason;
using NetSteps.Commissions.Service.Interfaces.Period;
using NetSteps.Commissions.Service.Interfaces.Title;
using NetSteps.Commissions.Service.Interfaces.TitleKind;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Commissions.Service.Models;
using NetSteps.Common.Extensions;
using NetSteps.Commissions.Service.Operations;

namespace NetSteps.Commissions.Service.AccountTitleOverrides
{
	public class AccountTitleOverrideRepository : BaseListRepository<IAccountTitleOverride, int, AccountTitleOverride, AccountTitleOverrideRepository.FieldNames>, IAccountTitleOverrideRepository
	{
		protected readonly IOverrideReasonProvider OverrideReasonProvider;
		protected readonly ITitleProvider TitleProvider;
		protected readonly ITitleKindProvider TitleKindProvider;
		protected readonly IPeriodProvider PeriodProvider;
		protected readonly IOverrideKindProvider OverrideKindProvider;
		public AccountTitleOverrideRepository(
			IOverrideReasonProvider overrideReasonProvider,
			ITitleProvider titleProvider,
			ITitleKindProvider titleKindProvider,
			IPeriodProvider periodProvider,
			IOverrideKindProvider overrideKindProvider)
		{

			OverrideReasonProvider = overrideReasonProvider;
			TitleProvider = titleProvider;
			TitleKindProvider = titleKindProvider;
			PeriodProvider = periodProvider;
			OverrideKindProvider = overrideKindProvider;
		}

		public enum FieldNames
		{
			AccountTitleOverrideId,
			AccountId,
			TitleId,
			TitleTypeId,
			PeriodId,
			OverrideReasonId,
			UserId,
			Notes,
			ApplicationSourceId,
			CreatedDate,
			UpdatedDate
		};

		protected override void SetKeyValue(AccountTitleOverride obj, int keyValue)
		{
			obj.AccountTitleOverrideId = keyValue;
		}

		protected override string TableName
		{
			get { return "AccountTitleOverrides"; }
		}

		protected override string ConnectionProviderName
		{
			get { return CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.AccountTitleOverrideId; }
		}

		protected override IAccountTitleOverride ConvertFromDataReader(System.Data.IDataRecord record)
		{
			var obj = new AccountTitleOverride();
			obj.AccountId = record.GetInt32((int)FieldNames.AccountId);
			obj.AccountTitleOverrideId = record.GetInt32((int)FieldNames.AccountTitleOverrideId);
			obj.ApplicationSourceId = record.GetNullable<int>((int)FieldNames.ApplicationSourceId);
			obj.CreatedDateUTC = record.GetDateTime((int)FieldNames.CreatedDate);
			obj.Notes = record.GetNullable<string>((int)FieldNames.Notes);
			obj.UpdatedDateUTC = record.GetDateTime((int)FieldNames.UpdatedDate);
			obj.UserId = record.GetInt32((int)FieldNames.UserId);
			obj.OverrideReason = OverrideReasonProvider.SingleOrDefault(x => x.OverrideReasonId == record.GetInt32((int)FieldNames.OverrideReasonId));
			obj.OverrideTitle = TitleProvider.SingleOrDefault(x => x.TitleId == record.GetInt32((int)FieldNames.TitleId));
			obj.OverrideTitleKind = TitleKindProvider.SingleOrDefault(x => x.TitleKindId == record.GetInt32((int)FieldNames.TitleTypeId));
			obj.Period = PeriodProvider.SingleOrDefault(x => x.PeriodId == record.GetInt32((int)FieldNames.PeriodId));
			obj.Title = TitleProvider.SingleOrDefault(x => x.TitleId == record.GetInt32((int)FieldNames.TitleId));
			obj.TitleKind = TitleKindProvider.SingleOrDefault(x => x.TitleKindId == record.GetInt32((int)FieldNames.TitleTypeId));
			return obj;
		}

		protected override IDictionary<FieldNames, object> GetConversionDictionary(IAccountTitleOverride obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.AccountId, obj.AccountId);
			propDictionary.Add(FieldNames.AccountTitleOverrideId, obj.AccountTitleOverrideId);
			propDictionary.Add(FieldNames.ApplicationSourceId, (obj.ApplicationSourceId != 0) ? obj.ApplicationSourceId : (int?)null);
			propDictionary.Add(FieldNames.CreatedDate, obj.CreatedDateUTC);
			propDictionary.Add(FieldNames.Notes, obj.Notes);
			if (obj.OverrideReason != null) { propDictionary.Add(FieldNames.OverrideReasonId, obj.OverrideReason.OverrideReasonId); }
			if (obj.Period != null) { propDictionary.Add(FieldNames.PeriodId, obj.Period.PeriodId); }
			if (obj.OverrideTitle != null) { propDictionary.Add(FieldNames.TitleId, obj.OverrideTitle.TitleId); }
			if (obj.OverrideTitleKind != null) { propDictionary.Add(FieldNames.TitleTypeId, obj.OverrideTitleKind.TitleKindId); }
			propDictionary.Add(FieldNames.UpdatedDate, obj.UpdatedDateUTC);
			propDictionary.Add(FieldNames.UserId, obj.UserId);
			return propDictionary;
		}

		public IAccountTitleOverrideSearchResult SearchAccountTitleOverrides(AccountTitleOverrideSearchParameters parameters)
		{
			var parameterList = new List<IOperation>();
			if (parameters.AccountId.HasValue)
			{
				parameterList.Add(new EqualsOperation(FieldNames.AccountId.ToString(), (int)parameters.AccountId));
			}

			if (parameters.AccountTitleOverrideId.HasValue)
			{
				parameterList.Add(new EqualsOperation(FieldNames.AccountTitleOverrideId.ToString(), (int)parameters.AccountTitleOverrideId));
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

			var search = new BaseSearchProvider<AccountTitleOverrideSearchResult, IAccountTitleOverride>();
			return search.Search(parameterList, TableName, FieldNames.AccountTitleOverrideId.ToString(), parameters.OrderByString, ConvertFromDataReader, ConnectionProvider);
		}
	}
}
