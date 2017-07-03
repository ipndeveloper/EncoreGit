using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Operations
{
    public interface IOperation
    {
        string GetQueryClause(int position);

        SqlParameter GetParameter(int position);
    }
}
