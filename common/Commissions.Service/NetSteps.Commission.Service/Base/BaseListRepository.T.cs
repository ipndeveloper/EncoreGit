using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Base
{
	public abstract class BaseListRepository<TObject, TPrimaryKeyType, TInternalObjectType, TPropertyEnum> : IRepository<TObject, TPrimaryKeyType>
		where TObject : class
		where TInternalObjectType : TObject, new()
	{
		protected abstract void SetKeyValue(TInternalObjectType obj, TPrimaryKeyType keyValue);
		protected abstract string TableName { get; }
		protected abstract TPropertyEnum PrimaryKeyProperty { get; }
		protected abstract TObject ConvertFromDataReader(IDataRecord record);

		protected virtual string ConnectionProviderName { get { return CommissionsConstants.ConnectionStringNames.Commissions; } }

		private IConnectionProvider _connectionProvider;
		protected IConnectionProvider ConnectionProvider
		{
			get
			{
				if (_connectionProvider == null)
				{
					_connectionProvider = Create.NewNamed<IConnectionProvider>(ConnectionProviderName);
				}
				return _connectionProvider;
			}
		}

		protected virtual TInternalObjectType CopyFrom(TObject original)
		{
			var copier = Create.New<ICopier<TObject, TInternalObjectType>>();
			return copier.Copy(original);
		}

		public virtual TObject AddOrUpdate(TObject obj)
		{
			var dictionary = GetConversionDictionary(obj);
			if (!IncludePrimaryKeyOnInsert && !EqualityComparer<TPrimaryKeyType>.Default.Equals((TPrimaryKeyType)dictionary[PrimaryKeyProperty], default(TPrimaryKeyType)))
			{
				return Update(obj);
			}

			return Add(obj);
		}

		public virtual TObject Add(TObject obj)
		{
			if (ConnectionProvider.CanConnect())
			{
				var dictionary = GetConversionDictionary(obj);
				using (var connection = ConnectionProvider.GetConnection())
				{
					var command = connection.CreateCommand();
					command.CommandType = CommandType.Text;
					var keys = dictionary.Keys.Where(x => IncludePrimaryKeyOnInsert || !x.Equals(PrimaryKeyProperty)).ToArray();

					var keyNames = keys.Select(x => x.ToString());
					command.CommandText = String.Format("INSERT INTO {0} ({1}) VALUES ({2}); SELECT SCOPE_IDENTITY()", TableName, String.Join(",", keyNames), String.Join(",", keyNames.Select(x => String.Format("@{0}", x))));
					foreach (var key in keys)
					{
						command.Parameters.AddWithValue(String.Format("@{0}", key.ToString()), dictionary[key] ?? DBNull.Value);
					}
					connection.Open();
					var result = command.ExecuteScalar();
					var internalObject = CopyFrom(obj);
					SetKeyValue(internalObject, (TPrimaryKeyType)Convert.ChangeType(result, typeof(TPrimaryKeyType)));
					return internalObject;
				}
			}
			return null;
		}

		protected abstract IDictionary<TPropertyEnum, object> GetConversionDictionary(TObject obj);

		protected virtual bool IncludePrimaryKeyOnInsert { get { return false; } }

		public virtual bool Delete(TPrimaryKeyType id)
		{
			if (ConnectionProvider.CanConnect())
			{
				using (var connection = ConnectionProvider.GetConnection())
				{
					var command = connection.CreateCommand();
					command.CommandText = String.Format("DELETE FROM {0} WHERE {1} = {2}", TableName, PrimaryKeyProperty.ToString(), id);
					connection.Open();
					var resultCount = command.ExecuteNonQuery();
					return resultCount == 1;
				}
			}
			return false;
		}

		public virtual TObject Fetch(TPrimaryKeyType id)
		{
			if (ConnectionProvider.CanConnect())
			{
				using (var connection = ConnectionProvider.GetConnection())
				{
					var command = connection.CreateCommand();
					command.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2} = {3}", String.Join(",", Enum.GetNames(typeof(TPropertyEnum))), TableName, PrimaryKeyProperty.ToString(), id);
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						return ConvertFromDataReader(reader);
					}
				}
			}
			return null;
		}

		public virtual IEnumerable<TObject> Fetch(IEnumerable<TPrimaryKeyType> ids)
		{
			if (ConnectionProvider.CanConnect())
			{
				using (var connection = ConnectionProvider.GetConnection())
				{
					var command = connection.CreateCommand();
					command.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2} IN ({3})", String.Join(",", Enum.GetNames(typeof(TPropertyEnum))), TableName, PrimaryKeyProperty.ToString(), String.Join(",", ids));
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						List<TObject> result = new List<TObject>();
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

		public virtual IList<TObject> FetchAll()
		{
			var objects = new List<TObject>();
			if (ConnectionProvider.CanConnect())
			{
				using (var connection = ConnectionProvider.GetConnection())
				{
					var command = connection.CreateCommand();
					command.CommandText = String.Format("SELECT {0} FROM {1}", string.Join(",", Enum.GetNames(typeof(TPropertyEnum))), TableName);
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						objects.Add(ConvertFromDataReader(reader));
					}
				}
			}
			return objects;
		}

		/// <summary>
		/// Gets a primary key list.  Personally I don't like this - I'd have preferred to build an expression evaluator but I just don't have
		/// time as it's my last day.  The problem is that I'm assuming that the user knows to put quotes around where strings, etc... because
		/// this is going directly to sql.  I really don't like this, but this is a temporary ORM anyway so I guess it'll sit where it is.
		/// </summary>
		/// <param name="whereStrings">The where strings.</param>
		/// <returns></returns>
		public virtual IEnumerable<TPrimaryKeyType> GetKeyList(IDictionary<TPropertyEnum, string> whereStrings)
		{
			var ids = new List<TPrimaryKeyType>();
			if (ConnectionProvider.CanConnect())
			{
				using (var connection = ConnectionProvider.GetConnection())
				{
					var command = connection.CreateCommand();
					var whereClause = new StringBuilder();
					if (whereStrings.Any())
					{
						whereClause.Append("WHERE ");
						foreach (var clause in whereStrings)
						{
							if (whereClause.Length > 6)
							{
								whereClause.Append(" AND ");
							}
							whereClause.Append(clause.Key);
							whereClause.Append(" = ");
							whereClause.Append(clause.Value);
						}
					}
					command.CommandText = string.Format("SELECT {0} FROM {1} {2}", PrimaryKeyProperty, TableName, whereClause);
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						ids.Add((TPrimaryKeyType)reader.GetValue(0));
					}
				} 
			}
			return ids;
		}

		public virtual TObject Update(TObject obj)
		{
			if (ConnectionProvider.CanConnect())
			{
				var dictionary = GetConversionDictionary(obj);
				using (var connection = ConnectionProvider.GetConnection())
				{
					var command = connection.CreateCommand();
					command.CommandType = CommandType.Text;
					var keys = dictionary.Keys.Where(key => !key.Equals(PrimaryKeyProperty)).ToArray();
					var keyvalues = keys.Select(key => string.Format("{0} = @{0}", key)).ToArray();

					command.CommandText = String.Format("UPDATE {0} SET {1} WHERE {2} = {3}"
						, TableName
						, string.Join(",", keyvalues)
						, PrimaryKeyProperty
						, dictionary[PrimaryKeyProperty]
						);

					foreach (var key in keys)
					{
						command.Parameters.AddWithValue(String.Format("@{0}", key), dictionary[key] ?? DBNull.Value);
					}

					connection.Open();
					command.ExecuteScalar();
					return Fetch((TPrimaryKeyType)dictionary[PrimaryKeyProperty]);
				} 
			}
			return null;
		}
	}
}
