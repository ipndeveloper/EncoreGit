using System.Configuration;
using System.Data.SqlClient;

namespace NetSteps.Commissions.Service.Base
{
	internal class CommissionsPrepConnectionProvider : BaseConnectionProvider
    {
        public override SqlConnection GetConnection()
        {
			return new SqlConnection(ConfigurationManager.ConnectionStrings[CommissionsConstants.ConnectionStringNames.CommissionsPrep].ConnectionString);
        }
	}
}
