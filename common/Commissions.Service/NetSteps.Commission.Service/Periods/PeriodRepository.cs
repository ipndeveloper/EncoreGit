using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.Period;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NetSteps.Commissions.Service.Periods
{
	public class PeriodRepository : BaseListRepository<IPeriod, int, Period, PeriodRepository.FieldNames>, IPeriodRepository
	{
		public enum FieldNames
		{
			PeriodId,
			StartDate,
			EndDate,
			ClosedDate,
			PlanId,
			EarningsViewable,
			BackOfficeDisplayStartDate,
			DisbursementsProcessed,
			Description,
			StartDateUtc,
			EndDateUtc
		};

		protected override void SetKeyValue(Period obj, int keyValue)
		{
			obj.PeriodId = keyValue;
		}

		protected override string TableName
        {/*CGI(JCT) - MLM - 010*/
            get { return CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "Periods", "Encore"); }
		}

		protected override string ConnectionProviderName
		{
			get { return CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse; }
		}

		protected override FieldNames PrimaryKeyProperty
		{
			get { return FieldNames.PeriodId; }
		}

		protected override IPeriod ConvertFromDataReader(IDataRecord record)
		{
			var period = new Period();
			period.ClosedDateUTC = record.IsDBNull((int)FieldNames.ClosedDate) ? (DateTime?)null : record.GetDateTime((int)FieldNames.ClosedDate);
            period.Description = record.GetNullable<string>((int)FieldNames.Description);
            period.DisbursementFrequency = (DisbursementFrequencyKind)record.GetNullable<int>((int)FieldNames.PlanId);
            period.EndDateUTC = record.GetNullable<DateTime>((int)FieldNames.EndDateUtc);
			period.IsOpen = record.IsDBNull((int)FieldNames.ClosedDate);
			period.PeriodId = record.GetInt32((int)FieldNames.PeriodId);
            period.StartDateUTC = record.GetNullable<DateTime>((int)FieldNames.StartDateUtc);
			return period;
		}

		public IEnumerable<int> PeriodIdsForAccount(int accountId)
		{
			if (ConnectionProvider.CanConnect())
			{
                string CommandPeriodIds = ObtainCommandPeriodIds((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry);
				using (var connection = ConnectionProvider.GetConnection())
				{
					var command = connection.CreateCommand();
                    command.CommandText = CommandPeriodIds;
					command.Parameters.Add(new SqlParameter("@AccountId", accountId));
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					int enrolldatekey = 0;
					if (reader.Read())
					{
						enrolldatekey = reader.GetInt32(0);
					}
					var periodIds = new List<int>();
					if (enrolldatekey > 0)
					{
						int startYear = enrolldatekey / 10000;
						int startMonth = (enrolldatekey / 100) % 100;

						DateTime now = DateTime.UtcNow;
						int endYear = now.Year;
						int endPeriod = (endYear * 100) + now.Month;

						int year = startYear;
						int month = startMonth;
						do
						{
							do
							{
								int period = (year * 100) + month;
								if (period > endPeriod)
								{
									break;
								}
								periodIds.Add(period);
							} while (++month <= 12);
							month = 1;
						} while (++year <= endYear);

					}
					return periodIds;
				} 
			}
			return Enumerable.Empty<int>();
		}
         
        /*CGI(JCT) - Inicio MLM - 010*/


        
        private string ObtainCommandPeriodIds(CommissionsConstants.EnvironmentList pEnvironment)
        {

            //  => HUNDRED => Inicio => 09052017 => para poder generalizar la funcionalidad en distintos idiomas
            //string ReturnCommand;
            //switch (pEnvironment)
            //{
            //    case CommissionsConstants.EnvironmentList.USA:
            //        ReturnCommand = "SELECT EnrollmentDateClientKey FROM [Belcorp_DW].Accounts.Distributor WHERE DistributorId = @AccountId AND CurrentFlag = 1 ORDER BY EnrollmentDateClientKey ASC";
            //        break;
            //    case CommissionsConstants.EnvironmentList.Brazil:
            //        ReturnCommand = "exec [GetPeriodIdsForAccount] @AccountId";
            //        break;
            //    default:
            //        ReturnCommand = "SELECT EnrollmentDateClientKey FROM [Belcorp_DW].Accounts.Distributor WHERE DistributorId = @AccountId AND CurrentFlag = 1 ORDER BY EnrollmentDateClientKey ASC";
            //        break;
            //}

            
            //return ReturnCommand;
            //=> HUNDRED => fin => 09052017

            return "exec [GetPeriodIdsForAccount] @AccountId";
        }
        /*CGI(JCT) - Fin MLM - 010*/

		protected override IDictionary<FieldNames, object> GetConversionDictionary(IPeriod obj)
		{
			var propDictionary = new Dictionary<FieldNames, object>();
			propDictionary.Add(FieldNames.BackOfficeDisplayStartDate, obj);
			propDictionary.Add(FieldNames.ClosedDate, obj);
			propDictionary.Add(FieldNames.Description, obj);
			propDictionary.Add(FieldNames.DisbursementsProcessed, obj);
			propDictionary.Add(FieldNames.EarningsViewable, obj);
			propDictionary.Add(FieldNames.EndDate, obj);
			propDictionary.Add(FieldNames.EndDateUtc, obj);
			propDictionary.Add(FieldNames.PeriodId, obj);
			propDictionary.Add(FieldNames.PlanId, obj);
			propDictionary.Add(FieldNames.StartDate, obj);
			propDictionary.Add(FieldNames.StartDateUtc, obj);
			return propDictionary;
		}
	}
}
