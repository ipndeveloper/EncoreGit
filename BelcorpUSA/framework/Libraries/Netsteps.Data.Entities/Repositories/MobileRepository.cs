using System;
using System.Collections.Generic;
using System.Data;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
    public class MobileRepository
    {
        /// <summary>
        /// Using a modified Scentsy SP
        /// </summary>
        /// <param name="accountTypeID2">Nullable additional account type</param>
        public static DataTable MobileAccountSearch(int sponsorID, int accountTypeID, int? accountTypeID2 = null, int? accountID = null)
        {
            IDbCommand dbCommand = null;

            try
            {
                dbCommand = DataAccess.SetCommand("usp_contactsgrid_select_by_sponsorid", connectionString: EntitiesHelpers.GetAdoConnectionString<NetStepsEntities>());
                DataAccess.AddInputParameter("SponsorID", sponsorID, dbCommand);
                DataAccess.AddInputParameter("AccountTypeID", accountTypeID, dbCommand);
                if (accountTypeID2.HasValue)
                    DataAccess.AddInputParameter("AccountTypeID2", accountTypeID2, dbCommand);
                if (accountID.HasValue)
                    DataAccess.AddInputParameter("AccountID", accountID, dbCommand);

                return DataAccess.GetDataTable(dbCommand);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
            }
            finally
            {
                DataAccess.Close(dbCommand);
            }
        }

        public static List<MobilePerformanceWidgetDataSet> MobilePerformanceWidgets(int accountID, int periodID)
        {
            List<MobilePerformanceWidgetDataSet> results = new List<MobilePerformanceWidgetDataSet>();
            //IDbCommand dbCommand = null;

            //TODO: Commissions Refactor - Mobile Performance Widget Data - Commissions removed goals, so I'm not sure on this
            //try
            //{
            //    dbCommand = DataAccess.SetCommand("Usp_get_mobileperformancewidgets", connectionString: EntitiesHelpers.GetAdoConnectionString<CommissionsEntities>());
            //    DataAccess.AddInputParameter("AccountID", accountID, dbCommand);
            //    DataAccess.AddInputParameter("PeriodID", periodID, dbCommand);

            //    var ds = DataAccess.GetDataSet(dbCommand);

            //    for (int i = 0; i < ds.Tables.Count; i++)
            //    {
            //        DataTable resultsDT, metaDataDT;
            //        resultsDT = ds.Tables[i];
            //        if (ds.Tables.Count > ++i)
            //        {
            //            metaDataDT = ds.Tables[i];
            //            results.Add(new MobilePerformanceWidgetDataSet(resultsDT, metaDataDT));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
            //}
            //finally
            //{
            //    DataAccess.Close(dbCommand);
            //}

            return results;
        }
    }

    public class MobilePerformanceWidgetDataSet : DataSet
    {
        public string KPITitle { get; private set; }

        public string KPIValue { get; private set; }

        public List<string> SummaryViewColumns { get; private set; }

        public MobilePerformanceWidgetDataSet(DataTable resultsDT, DataTable metaDataDT)
        {
            this.SummaryViewColumns = new List<string>();

            this.KPITitle = metaDataDT.Rows[0]["KPITitle"].ToString();
            this.KPIValue = metaDataDT.Rows[0]["KPIValue"].ToString();

            foreach (DataColumn item in metaDataDT.Columns)
            {
                if (item.ColumnName.StartsWith("SummaryViewColumn", StringComparison.OrdinalIgnoreCase))
                {
                    SummaryViewColumns.Add(metaDataDT.Rows[0][item].ToString());
                }
            }

            this.Tables.Add(resultsDT.Copy());
        }
    }
}
