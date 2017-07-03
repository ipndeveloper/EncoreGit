using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.ComponentModel;

namespace NetSteps.Data.Entities.Repositories
{
    public class AsincronaRepository
    {
        BackgroundWorker ProcesoEjecutar = new BackgroundWorker();

        BankPaymentsSearchParameter _bankPaymentsSearchParameter = new BankPaymentsSearchParameter();

        //Aplicación del recaudo
        public void ImplementationCollection(object intProceso_key, DoWorkEventArgs e)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspImplementationCollection",
                new SqlParameter("TipoCredito", SqlDbType.VarChar) { Value = _bankPaymentsSearchParameter.TipoCredito },
                new SqlParameter("BankId", SqlDbType.Int) { Value = _bankPaymentsSearchParameter.Bankid },
                new SqlParameter("FileSequence", SqlDbType.Int) { Value = _bankPaymentsSearchParameter.FileSequence },
                new SqlParameter("UserID", SqlDbType.Int) { Value = _bankPaymentsSearchParameter.UserID },
                new SqlParameter("NombreArchivo", SqlDbType.VarChar) { Value = _bankPaymentsSearchParameter.FileNameBank },
                new SqlParameter("FechaArchivo", SqlDbType.VarChar) { Value = _bankPaymentsSearchParameter.FileDate },
                new SqlParameter("BankName", SqlDbType.VarChar) { Value = _bankPaymentsSearchParameter.BankName }
              );
        }

        public void ImplementationCollectionAsyn(BankPaymentsSearchParameter param)
        {
            _bankPaymentsSearchParameter = param;
            ProcesoEjecutar.DoWork += ImplementationCollection;
            ProcesoEjecutar.RunWorkerAsync();
        }
    }
}
