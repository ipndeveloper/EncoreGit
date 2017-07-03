using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Extensions
{
    public class PersonalIndicardorAsynExtensions
    {
        BackgroundWorker ProcesoEjecutar = new BackgroundWorker();
        int _OrderID = 0;
        short _OrderStatusID = 0;

        private void UpdatePersonalIndicator(object intProceso_key, DoWorkEventArgs e)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Commissions", "SPOnLineMLM",
                new SqlParameter("OrderID", SqlDbType.Int) { Value = _OrderID },
                new SqlParameter("StatusOrderID", SqlDbType.Int) { Value = _OrderStatusID }
                );
        }
        
        public void UpdatePersonalIndicatorAsyn(int OrderID, short OrderStatusID)
        {
            _OrderID = OrderID;
            _OrderStatusID = OrderStatusID;
            ProcesoEjecutar.DoWork += UpdatePersonalIndicator;
            ProcesoEjecutar.RunWorkerAsync();
        }

    }
}
