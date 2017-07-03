using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Exceptions;
using System.Globalization;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;


namespace NetSteps.Data.Entities
{
    public class KPIsPerPeriodDataAccess
    {
        public static KPIsPerPeriodSearchData GetKPISbyPeriodIdandAccountId(int periodId, int accountId)
        {

            DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("upsGetKPISbyPeriodIdandAccountId",
                                               new Dictionary<string, object>() { 
                                                                                    {"@periodId", periodId},
                                                                                    {"@accountId", accountId}
                                                                                }, 
                                                "Commissions"));

            DataTable table = ds.Tables[0];
            KPIsPerPeriodSearchData itemRow = null;
            foreach (System.Data.DataRow row in table.Rows)
            {
                itemRow = new KPIsPerPeriodSearchData();
                object[] values = row.ItemArray;
                itemRow.PeriodID = Convert.ToString(values[0]);
                itemRow.AccountID = Convert.ToString(values[1]);
                itemRow.PaidAsCurrentMonth = Convert.ToString(values[2]);
                itemRow.PQV = Convert.ToString(values[3]);
                itemRow.DQV = Convert.ToString(values[4]);
            }

            return itemRow;
        }
    }
}
