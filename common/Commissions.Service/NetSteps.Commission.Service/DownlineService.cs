using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fasterflect;
using NetSteps.Commissions.Common;
using System.Data;
using NetSteps.Common.Dynamic;
using NetSteps.Common.Extensions;
using System.Data.SqlClient;
using System.Configuration;
using NetSteps.Foundation.Common;
using System.Data.Common;
using NetSteps.Commissions.Service.Base;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service
{
    public class DownlineService : IDownlineService
    {
        private static Type _generatedType;
        private static readonly Dictionary<string, MemberSetter> Setters = new Dictionary<string, MemberSetter>();
        private static readonly Dictionary<string, int> ColumnOrdinals = new Dictionary<string, int>();

        private static Type _generatedTypeSingle;
        private static readonly Dictionary<string, MemberSetter> SettersSingle = new Dictionary<string, MemberSetter>();
        private static readonly Dictionary<string, int> ColumnOrdinalsSingle = new Dictionary<string, int>();

        private static Type _generatedTypeMLM;
        private static readonly Dictionary<string, MemberSetter> SettersMLM = new Dictionary<string, MemberSetter>();
        private static readonly Dictionary<string, int> ColumnOrdinalsMLM = new Dictionary<string, int>();

        private IConnectionProvider _commissionsConnectionProvider;
        public IConnectionProvider CommissionsConnectionProvider
        {
            get
            {
                if (_commissionsConnectionProvider == null)
                {
                    _commissionsConnectionProvider = Create.NewNamed<IConnectionProvider>(CommissionsConstants.ConnectionStringNames.Commissions);
                }
                return _commissionsConnectionProvider;
            }
        }

        private IConnectionProvider _knownFactorsConnectionProvider;
        public IConnectionProvider KnownFactorsConnectionProvider
        {
            get
            {
                if (_knownFactorsConnectionProvider == null)
                {
                    _knownFactorsConnectionProvider = Create.NewNamed<IConnectionProvider>(CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse);
                }
                return _knownFactorsConnectionProvider;
            }
        }

        public IEnumerable<dynamic> GetDownline(int periodId)
        {
            var results = new List<dynamic>();

            if (KnownFactorsConnectionProvider.CanConnect())
            {
                using (var connection = KnownFactorsConnectionProvider.GetConnection())
                {/*CGI(JCT) - MLM - 010*/
                    using (var command = new SqlCommand(CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "usp_get_downline", "Encore"), connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PeriodID", periodId));

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            if (_generatedType == null || Setters.Count == 0)
                            {
                                ColumnOrdinals.Clear();
                                var schema = reader.GetSchemaTable();
                                var properties = new Dictionary<string, Type>();
                                var dateTimeType = typeof(DateTime);
                                if (schema != null)
                                {
                                    foreach (DataRow row in schema.Rows)
                                    {
                                        var type = row["DataType"] as Type;
                                        if (type != null && (type.IsPrimitive || type.IsNumeric() || type.IsEnum || type == dateTimeType))
                                        {
                                            type = typeof(Nullable<>).MakeGenericType(type);
                                        }
                                        properties.Add(row["ColumnName"].ToString(), type);
                                        ColumnOrdinals.Add(row["ColumnName"].ToString(), (int)row["ColumnOrdinal"]);
                                    }

                                    _generatedType = TypeBuilder.GenerateType("DownlineData", properties, addDisplayAttribute: true,
                                        addTermNameAttribute: true);
                                    foreach (DataRow row in schema.Rows)
                                    {
                                        Setters.Add(row["ColumnName"].ToString(),
                                            _generatedType.DelegateForSetPropertyValue(row["ColumnName"].ToString()));
                                    }
                                }
                            }

                            while (reader.Read())
                            {
                                var downlineData = _generatedType.NewFast();
                                foreach (var setter in Setters)
                                {
                                    try
                                    {
                                        var columnOrdinal = ColumnOrdinals[setter.Key];
                                        var isNull = reader.IsDBNull(columnOrdinal);
                                        var value = isNull ? null : reader[setter.Key];
                                        setter.Value(downlineData, value);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                                results.Add(downlineData);
                            }

                            reader.Close();
                        }

                        connection.Close();
                    }
                }
            }

            return results;
        }

        public IEnumerable<dynamic> GetDownline(int periodId, int sponsorID)
        {
            var results = new List<dynamic>();

            if (KnownFactorsConnectionProvider.CanConnect())
            {
                using (var connection = KnownFactorsConnectionProvider.GetConnection())
                {/*CGI(JCT) - MLM - 010*/
                    using (var command = new SqlCommand(CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "usp_get_downlineMLM", "Commissions"), connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PeriodID", periodId));
                        command.Parameters.Add(new SqlParameter("@SponsorID", sponsorID));

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            if (_generatedTypeMLM == null || SettersMLM.Count == 0)
                            {
                                ColumnOrdinalsMLM.Clear();
                                var schema = reader.GetSchemaTable();
                                var properties = new Dictionary<string, Type>();
                                var dateTimeType = typeof(DateTime);
                                if (schema != null)
                                {
                                    foreach (DataRow row in schema.Rows)
                                    {
                                        var type = row["DataType"] as Type;
                                        if (type != null && (type.IsPrimitive || type.IsNumeric() || type.IsEnum || type == dateTimeType))
                                        {
                                            type = typeof(Nullable<>).MakeGenericType(type);
                                        }
                                        properties.Add(row["ColumnName"].ToString(), type);
                                        ColumnOrdinalsMLM.Add(row["ColumnName"].ToString(), (int)row["ColumnOrdinal"]);
                                    }

                                    _generatedTypeMLM = TypeBuilder.GenerateType("DownlineDataMLM", properties, addDisplayAttribute: true,
                                        addTermNameAttribute: true);
                                    foreach (DataRow row in schema.Rows)
                                    {
                                        SettersMLM.Add(row["ColumnName"].ToString(),
                                            _generatedTypeMLM.DelegateForSetPropertyValue(row["ColumnName"].ToString()));
                                    }
                                }
                            }

                            while (reader.Read())
                            {
                                var downlineData = _generatedTypeMLM.NewFast();
                                foreach (var setter in SettersMLM)
                                {
                                    try
                                    {
                                        var columnOrdinal = ColumnOrdinalsMLM[setter.Key];
                                        var isNull = reader.IsDBNull(columnOrdinal);
                                        var value = isNull ? null : reader[setter.Key];
                                        setter.Value(downlineData, value);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                                results.Add(downlineData);
                            }

                            reader.Close();
                        }

                        connection.Close();
                    }
                }
            }

            return results;
        }

        public IEnumerable<dynamic> GetSingleLayerDownline(int downlineId, int periodId)
        {
            var results = new List<dynamic>();

            if (CommissionsConnectionProvider.CanConnect())
            {
                using (var connection = CommissionsConnectionProvider.GetConnection())
                {
                    using (var command = new SqlCommand("usp_get_downline_single", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PeriodID", periodId));
                        command.Parameters.Add(new SqlParameter("@PlanID", 1));
                        command.Parameters.Add(new SqlParameter("@AccountID", downlineId));

                        command.CommandTimeout = 300;

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            if (_generatedTypeSingle == null)
                            {
                                ColumnOrdinalsSingle.Clear();
                                var schema = reader.GetSchemaTable();
                                var properties = new Dictionary<string, Type>();
                                var dateTimeType = typeof(DateTime);

                                if (schema != null)
                                {
                                    foreach (DataRow row in schema.Rows)
                                    {
                                        var type = row["DataType"] as Type;
                                        if (type != null && (type.IsPrimitive || type.IsNumeric() || type.IsEnum || type == dateTimeType))
                                        {
                                            type = typeof(Nullable<>).MakeGenericType(type);
                                        }
                                        properties.Add(row["ColumnName"].ToString(), type);
                                        ColumnOrdinalsSingle.Add(row["ColumnName"].ToString(), (int)row["ColumnOrdinal"]);
                                    }
                                    _generatedTypeSingle = TypeBuilder.GenerateType("DownlineData", properties, addDisplayAttribute: true,
                                        addTermNameAttribute: true);
                                    foreach (DataRow row in schema.Rows)
                                    {
                                        SettersSingle.Add(row["ColumnName"].ToString(),
                                            _generatedTypeSingle.DelegateForSetPropertyValue(row["ColumnName"].ToString()));
                                    }
                                }
                            }

                            while (reader.Read())
                            {
                                var downlineData = _generatedTypeSingle.NewFast();
                                foreach (var setter in SettersSingle)
                                {
                                    try
                                    {
                                        var columnOrdinal = ColumnOrdinalsSingle[setter.Key];
                                        var isNull = reader.IsDBNull(columnOrdinal);
                                        var value = isNull ? null : reader[setter.Key];
                                        setter.Value(downlineData, value);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                                results.Add(downlineData);
                            }
                            reader.Close();
                        }
                    }
                }
            }

            return results;
        }

        public class SimpleResult
        {
            public SimpleResult(int sponsorId, int accountId)
            {
                this.SponsorId = sponsorId;
                this.AccountId = accountId;
            }

            public int AccountId { get; private set; }
            public int SponsorId { get; private set; }
        }

        public IEnumerable<dynamic> GetSimpleDownline(int periodId)
        {
            var results = new List<dynamic>();

            if (CommissionsConnectionProvider.CanConnect())
            {
                using (var connection = CommissionsConnectionProvider.GetConnection())
                {
                    using (var command = new SqlCommand("usp_get_downline_simple", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PeriodID", periodId));
                        command.Parameters.Add(new SqlParameter("@PlanID", 1));

                        command.CommandTimeout = 300;

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            var accountId = Convert.ToInt32(reader["AccountID"]);
                            var sponsorId = Convert.ToInt32(reader["SponsorID"]);

                            results.Add(new SimpleResult(sponsorId, accountId));
                        }
                    }
                }
            }

            return results;
        }
    }
}
