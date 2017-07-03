using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.Title;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Models;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Core.Cache;

namespace NetSteps.Commissions.Service.Titles
{
	public class TitleRepository : BaseListRepository<ITitle, int, Title, TitleRepository.FieldNames>, ITitleRepository
	{
		protected readonly ICachedList<ITitle> _provider;
		public enum FieldNames
		{
			TitleId,
			TitleCode,
			Name,
			SortOrder,
			Active,
			TermName,
			ClientCode,
			ClientName,
			ReportVisibility
		};

        private IConnectionProvider _connectionProvider;
        private IConnectionProvider ConnectionProvider
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

		protected override void SetKeyValue(Title obj, int keyValue)
		{
			obj.TitleId = keyValue;
		}

		protected override string TableName
        {/*CGI(JCT) MLM - 010*/
            get { return CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "Titles", "Encore"); } 
		}

		protected override string ConnectionProviderName
		{
			get { return CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse; }
		}

		protected override TitleRepository.FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.TitleId; }
		}

		protected override ITitle ConvertFromDataReader(IDataRecord record)
		{
			if (record == null)
			{
				this.TraceError("TitleRepository::ConvertFromDataReader - ConvertFromDataReader called on null record");
				throw new ArgumentNullException();
			}

			var title = new Title();
			title.Active = record.GetBoolean((int)FieldNames.Active);
			title.SortOrder = record.GetInt32((int)FieldNames.SortOrder);
			title.TermName = record.GetNullable<string>((int)FieldNames.TermName);
			title.TitleCode = record.GetNullable<string>((int)FieldNames.TitleCode);
			title.TitleId = record.GetInt32((int)FieldNames.TitleId);
			title.TitleName = record.GetNullable<string>((int)FieldNames.Name);
			title.ClientCode = record.GetNullable<string>((int)FieldNames.ClientCode);
			title.ClientName = record.GetNullable<string>((int)FieldNames.ClientName);
			title.ReportsVisibility = record.GetBoolean((int)FieldNames.ReportVisibility);

			return title;
		}

		protected override IDictionary<TitleRepository.FieldNames, object> GetConversionDictionary(ITitle obj)
		{
			var propDictionary = new Dictionary<FieldNames, object> { };
			propDictionary.Add(FieldNames.Active, obj.Active);
			propDictionary.Add(FieldNames.ClientCode, obj.ClientCode);
			propDictionary.Add(FieldNames.ClientName, obj.ClientName);
			propDictionary.Add(FieldNames.Name, obj.TitleName);
			propDictionary.Add(FieldNames.ReportVisibility, obj.ReportsVisibility);
			propDictionary.Add(FieldNames.SortOrder, obj.SortOrder);
			propDictionary.Add(FieldNames.TermName, obj.TermName);
			propDictionary.Add(FieldNames.TitleCode, obj.TitleCode);
			propDictionary.Add(FieldNames.TitleId, obj.TitleId);
			return propDictionary;
		}

        public IEnumerable<ITitle> GetFromReportByPeriod(int periodId, int accountId)
        {
            var items = new List<ITitle>();
            if (ConnectionProvider.CanConnect())
            {
                using (var connection = ConnectionProvider.GetConnection())
                {
                    using (var command = new SqlCommand(CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "GetReportTitlesMLM", "Encore"), connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PeriodID", periodId));
                        command.Parameters.Add(new SqlParameter("@AccountID", accountId));

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            
                            if (reader.Read())
                            {
                               items.Add(new Title
                                {
                                    Active = reader.GetNullable<bool>((int)FieldNames.Active),
                                    SortOrder = reader.GetNullable<int>((int)FieldNames.SortOrder),
                                    TermName = reader.GetNullable<string>((int)FieldNames.TermName),
                                    TitleCode = reader.GetNullable<string>((int)FieldNames.TitleCode),
                                    TitleId = reader.GetNullable<int>((int)FieldNames.TitleId),
                                    TitleName = reader.GetNullable<string>((int)FieldNames.Name),
                                    ClientCode = reader.GetNullable<string>((int)FieldNames.ClientCode),
                                    ClientName = reader.GetNullable<string>((int)FieldNames.ClientName),
                                    ReportsVisibility = reader.GetNullable<bool>((int)FieldNames.ReportVisibility)
                                });
                            }
                        }
                    }
                }
            }

            return items;
        }
    }
}
