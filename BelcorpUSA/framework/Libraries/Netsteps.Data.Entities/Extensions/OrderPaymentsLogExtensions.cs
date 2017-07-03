using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;
using System.Data.SqlClient;
using System.Data;

//@1 20150722 BR-CC-019 GYS LIB: se creo el metodo CreateLog al que se hAce llamado en el requerimiento BR-CC-012
namespace NetSteps.Data.Entities.Extensions
{
    public static class OrderPaymentsLogExtensions
    {
        public static bool CreateLog(OrderPaymentsLog preOrderPaymentsLog)
        {
            bool LogID = DataAccess.ExecWithStoreProcedureBool("Core", "upsInsOrderPaymentsLog",
                new SqlParameter("OrderPaymentID", SqlDbType.Int) { Value = preOrderPaymentsLog.OrderPaymentID },
                new SqlParameter("ReasonID", SqlDbType.Int) { Value = preOrderPaymentsLog.ReasonID },
                new SqlParameter("InitialAmount", SqlDbType.Money) { Value = preOrderPaymentsLog.InitialAmount },
                new SqlParameter("InterestAmount", SqlDbType.Money) { Value = preOrderPaymentsLog.InterestAmount },
                new SqlParameter("FineAmount", SqlDbType.Money) { Value = preOrderPaymentsLog.FineAmount },
                new SqlParameter("DisccountedAmount", SqlDbType.Money) { Value = preOrderPaymentsLog.DisccountedAmount },
                new SqlParameter("TotalAmount", SqlDbType.Money) { Value = preOrderPaymentsLog.TotalAmount },
                new SqlParameter("ExpirationDate", SqlDbType.DateTime) { Value = preOrderPaymentsLog.ExpirationDateUTC },
                new SqlParameter("ModifiedByUserID", SqlDbType.Int) { Value = preOrderPaymentsLog.ModifiedByUserID }
                );
            return LogID;
        }

    }
}
