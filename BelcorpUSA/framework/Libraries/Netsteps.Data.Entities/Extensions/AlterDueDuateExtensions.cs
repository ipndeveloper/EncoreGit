using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Extensions
{
    public class AlterDueDuateExtensions
    {
        public static void AlterDueDate(int TicketNumber,string NewDate)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "spUpdateAlterDueDate",
                new SqlParameter("TicketNumber", SqlDbType.VarChar) { Value = TicketNumber },
                new SqlParameter("NewDate", SqlDbType.VarChar) { Value =NewDate}
                );
            
        }
    }
}
