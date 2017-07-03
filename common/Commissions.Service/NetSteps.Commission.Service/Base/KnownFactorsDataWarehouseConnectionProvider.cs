using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NetSteps.Diagnostics.Utilities;

namespace NetSteps.Commissions.Service.Base
{
	public class KnownFactorsDataWarehouseConnectionProvider : BaseConnectionProvider
	{
		public override SqlConnection GetConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings[CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse].ConnectionString);
		}
	}
}
