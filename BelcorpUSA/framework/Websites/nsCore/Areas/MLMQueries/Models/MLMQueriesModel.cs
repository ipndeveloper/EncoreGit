using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using System.Data;

namespace nsCore.Areas.MLMQueries.Models
{
	public static class MLMQueriesModel
	{
        public static Dictionary<int, string> DisplayIndicators
        {
            get
            {
               return DataAccess.GetDataSet(DataAccess.GetCommand("upsGetIndicators",
                                                new Dictionary<string, object>() { },
                                                    "Commissions")).Tables[0].AsEnumerable()
                                                    .ToDictionary(row => row.Field<int>(0), row => row.Field<string>(1));
            }
        }
	}
}