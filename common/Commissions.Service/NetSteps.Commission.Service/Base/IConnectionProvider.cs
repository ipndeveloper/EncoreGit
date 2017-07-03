using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Base
{
    public interface IConnectionProvider
    {
        SqlConnection GetConnection();

		bool CanConnect();
    }
}
