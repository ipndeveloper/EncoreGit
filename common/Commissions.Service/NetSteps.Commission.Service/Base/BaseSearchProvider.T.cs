using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Common.Base;
using NetSteps.Commissions.Service.Operations;
using System.Data;
using NetSteps.Common.Extensions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service.Base
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">search result return type</typeparam>
	/// <typeparam name="U">search type item</typeparam>
	public class BaseSearchProvider<T, U> where T : IBaseSearchResult<U>, new()
	{
		public T Search(IEnumerable<IOperation> operationList, string tableName, string idField, string orderBy, Func<IDataRecord, U> converter, IConnectionProvider connectionProvider)
		{
			var whereClause = (operationList.Any()) ? string.Format("WHERE {0}", operationList.Select((x, i) => x.GetQueryClause(i)).Join(" AND ")) : string.Empty;

			var countQuery = string.Format("SELECT COUNT(*) AS TotalCount FROM {0} {1}"
				, tableName
				, whereClause
				).TrimEnd();

			var orderByClause = string.Format("ORDER BY {0}.{1}", tableName, string.IsNullOrEmpty(orderBy) ? idField : orderBy);
			var skipQuery = string.Format("INNER JOIN (SELECT ROW_NUMBER() OVER({0}) AS 'ROWID', {2} as Row{2} FROM {1}) RN ON {1}.{2} = RN.Row{2}"
				, orderByClause
				, tableName
				, idField
				);

			var fullQuery = string.Format("SELECT {0}.* FROM {0} {1} {2} {3}"
				, tableName
				, skipQuery
				, whereClause
				, orderByClause
				).TrimEnd();

			if (connectionProvider.CanConnect())
			{
				using (var connection = connectionProvider.GetConnection())
				{
					using (var command = connection.CreateCommand())
					{
						command.CommandType = CommandType.Text;
						command.CommandText = string.Format("{0};{1}", countQuery, fullQuery);
						operationList.Each((x, i) => command.Parameters.Add(x.GetParameter(i)));

						connection.Open();

						var res = new List<U>();
						var total = 0;
						using (var reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								total = reader.IsDBNull(0)
									? default(Int32)
									: reader.GetInt32(0);
							}

							if (reader.NextResult())
							{
								while (reader.Read())
								{
									res.Add(converter(reader));
								}
							}
						}

						return new T
						{
							TotalCount = total,
							Results = res
						};
					}
				}
			}
			return default(T);
		}
	}
}
