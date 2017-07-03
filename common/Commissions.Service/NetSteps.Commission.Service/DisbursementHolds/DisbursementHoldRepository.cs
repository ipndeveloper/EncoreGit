using System;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Interfaces.DisbursementHold;
using NetSteps.Commissions.Service.Interfaces.OverrideReason;
using NetSteps.Commissions.Service.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Commissions.Service.Operations;

namespace NetSteps.Commissions.Service.DisbursementHolds
{
    public class DisbursementHoldRepository : BaseListRepository<IDisbursementHold, int, DisbursementHold, DisbursementHoldRepository.FieldNames>, IDisbursementHoldRepository
	{
		private readonly IOverrideReasonProvider _overrideReasonProvider;

        public DisbursementHoldRepository(IOverrideReasonProvider overrideReasonProvider)
		{
			_overrideReasonProvider = overrideReasonProvider;
		}

		public enum FieldNames
		{
            DisbursementHoldId,
			AccountId,
			UserId,
			OverrideReasonId,
			HoldUntil,
			StartDate,
			Notes,
			ApplicationSourceId,
			CreatedDate,
			DateModified
		};

		protected override void SetKeyValue(DisbursementHold obj, int keyValue)
		{
            obj.DisbursementHoldId = keyValue;
		}

		protected override string TableName
		{
            get { return "dbo.DisbursementHolds"; }
		}

        protected override FieldNames PrimaryKeyProperty
		{
            get { return FieldNames.DisbursementHoldId; }
		}

        protected override IDisbursementHold ConvertFromDataReader(IDataRecord record)
        {
            var obj = new DisbursementHold();
            obj.AccountId = record.GetInt32((int)FieldNames.AccountId);
            obj.ApplicationSourceId = record.GetNullable<int>((int)FieldNames.ApplicationSourceId);
            obj.DisbursementHoldId = record.GetInt32((int)FieldNames.DisbursementHoldId);
            obj.CreatedDate = record.GetDateTime((int)FieldNames.CreatedDate);
            obj.HoldUntil = record.GetNullable<DateTime>((int)FieldNames.HoldUntil);
            obj.Notes = record.GetNullable<string>((int)FieldNames.Notes);
            obj.Reason = _overrideReasonProvider.SingleOrDefault(x => x.OverrideReasonId == record.GetInt32((int)FieldNames.OverrideReasonId));
            obj.StartDate = record.GetDateTime((int)FieldNames.StartDate);
            obj.UpdatedDate = record.GetDateTime((int)FieldNames.DateModified);
            obj.UserId = record.GetInt32((int)FieldNames.UserId);

            return obj;
        }

        protected override IDictionary<FieldNames, object> GetConversionDictionary(IDisbursementHold obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.AccountId, obj.AccountId);
			propDictionary.Add(FieldNames.ApplicationSourceId, obj.ApplicationSourceId);
            propDictionary.Add(FieldNames.DisbursementHoldId, obj.DisbursementHoldId);
			propDictionary.Add(FieldNames.CreatedDate, obj.CreatedDate);
			propDictionary.Add(FieldNames.DateModified, obj.UpdatedDate);
			propDictionary.Add(FieldNames.HoldUntil, obj.HoldUntil);
			propDictionary.Add(FieldNames.Notes, obj.Notes);
			propDictionary.Add(FieldNames.OverrideReasonId, (obj.Reason != null) ? obj.Reason.OverrideReasonId : 0);
			propDictionary.Add(FieldNames.StartDate, obj.StartDate);
			propDictionary.Add(FieldNames.UserId, obj.UserId);
			return propDictionary;
		}

        public IDisbursementHoldSearchResult SearchDisbursementHolds(DisbursementHoldSearchParameters parameters)
		{
            var parameterList = new List<IOperation>();
            if (parameters.AccountId.HasValue)
            {
                parameterList.Add(new EqualsOperation(FieldNames.AccountId.ToString(), (int)parameters.AccountId));
            }

            if (parameters.CheckHoldId.HasValue)
            {
                parameterList.Add(new EqualsOperation(FieldNames.DisbursementHoldId.ToString(), (int)parameters.CheckHoldId));
            }

            if (parameters.ReasonId.HasValue)
            {
                parameterList.Add(new EqualsOperation(FieldNames.OverrideReasonId.ToString(), (int)parameters.ReasonId));
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

            var search = new BaseSearchProvider<DisbursementHoldSearchResult, IDisbursementHold>();
            return search.Search(parameterList, TableName, FieldNames.DisbursementHoldId.ToString(), parameters.OrderByString, ConvertFromDataReader, ConnectionProvider);
        }
	}
}
