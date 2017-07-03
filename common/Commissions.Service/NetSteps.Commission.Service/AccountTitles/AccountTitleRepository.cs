using System;
using System.Collections.Generic;
using NetSteps.Commissions.Service.Interfaces.AccountTitles;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Models;

namespace NetSteps.Commissions.Service.AccountTitles
{
	public class AccountTitleRepository : BaseListRepository<IAccountTitle, int, AccountTitle, AccountTitleRepository.FieldNames>, IAccountTitleRepository
	{
		public enum FieldNames
		{
			AccountId,
			TitleId,
			TitleTypeId,
			PeriodId,
			DateModified
		};

		protected override void SetKeyValue(AccountTitle obj, int keyValue)
		{
			// we currently don't need to be adding items to this set
			throw new NotImplementedException();
		}

		protected override string ConnectionProviderName
		{
			get { return CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse; }
		}

		protected override string TableName
		{
			get { return "AccountTitles"; }
		}

		protected override AccountTitleRepository.FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.AccountId; }
		}

		protected override IAccountTitle ConvertFromDataReader(System.Data.IDataRecord record)
		{
			var obj = new AccountTitle
			{
				AccountId = record.GetInt32((int)FieldNames.AccountId),
				TitleId = record.GetInt32((int)FieldNames.TitleId),
				TitleKindId = record.GetInt32((int)FieldNames.TitleTypeId),
				PeriodId = record.GetInt32((int)FieldNames.PeriodId),
				DateModified = record.GetDateTime((int)FieldNames.DateModified)
			};
			return obj;
		}

		protected override IDictionary<AccountTitleRepository.FieldNames, object> GetConversionDictionary(IAccountTitle obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.AccountId, obj.AccountId);
			propDictionary.Add(FieldNames.TitleId, obj.TitleId);
			propDictionary.Add(FieldNames.TitleTypeId, obj.TitleKindId);
			propDictionary.Add(FieldNames.PeriodId, obj.PeriodId);
			propDictionary.Add(FieldNames.DateModified, obj.DateModified);
			return propDictionary;
		}

		public IEnumerable<IAccountTitle> GetAccountTitlesForPeriod(int periodId)
		{
			if (ConnectionProvider.CanConnect())
			{
				using (var connection = ConnectionProvider.GetConnection())
				{
					var command = connection.CreateCommand();
					command.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2} = {3}", String.Join(",", Enum.GetNames(typeof(FieldNames))), TableName, FieldNames.PeriodId.ToString(), periodId);
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						List<IAccountTitle> result = new List<IAccountTitle>();
						do
						{
							result.Add(ConvertFromDataReader(reader));
						} while (reader.Read());
						return result;
					}
				} 
			}
			return null;
		}

		public override IAccountTitle Add(IAccountTitle obj)
		{
			throw new InvalidOperationException();
		}

		public override IAccountTitle AddOrUpdate(IAccountTitle obj)
		{
			throw new InvalidOperationException();
		}

		public override bool Delete(int id)
		{
			throw new InvalidOperationException();
		}

		public override IAccountTitle Update(IAccountTitle obj)
		{
			throw new InvalidOperationException();
		}
	}
}
