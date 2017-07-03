using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Repositories
{
    public class XmlProductMaterialRepository : IXmlProductMaterialRepository
    {
        public int ExistMaterialBySKU(string SKU)
        {
            //using (SqlConnection connection = new SqlConnection("Data Source=BelcorpUSADatabase;Initial Catalog=BelcorpBRACore;Persist Security Info=True;Integrated Security=false;Application Name=GMP;MultipleActiveResultSets=True;uid=usrencorebrasildev;pwd=Belcorp2014"))
             
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspExistMaterialBySKU",
                new SqlParameter("SKU", SqlDbType.VarChar) { Value = SKU }
                );
        }

        public int ValidarConfirmacionPagos(string COP, string VTD1, string VTD2)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCommission, "usp_ValidarConfirmacionPagos",
                new SqlParameter("CodigoOrdemPagamento", SqlDbType.VarChar) { Value = COP },
                new SqlParameter("ValorTotalDescontado1", SqlDbType.VarChar) { Value = VTD1 },
                new SqlParameter("ValorTotalDescontado2", SqlDbType.VarChar) { Value = VTD2 }
                );
        }

        public int ExistWareHouseByExternalCode(string externalCode)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspExistWareHouseByExternalCode",
                new SqlParameter("ExternalCode", SqlDbType.VarChar) { Value = externalCode }
                );
        }

        public string ReturnXmlB090(string fechaMov)
        {
            DataTable dt;
            string ValorRetorno = "";
            string CadenaConexion = ConfigurationManager.ConnectionStrings["Core"].ConnectionString;
            SqlConnection conn = new SqlConnection(CadenaConexion);
            SqlCommand cmd = new SqlCommand("usp_GetValuesB090", conn);
            cmd.Parameters.Add("@FechaInput", SqlDbType.Char);
            cmd.Parameters["@FechaInput"].Value = fechaMov;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            conn.Open();
            dt = DataAccess.GetDataTable(cmd);
            conn.Close();
            ValorRetorno = dt.Rows[0][0].ToString();

            return ValorRetorno;
        }

        public string ReturnXMLE080WSAutenticacion(string Login, string Password)
        {
            DataTable dt;
            string ValorRetorno = "";
            string CadenaConexion = ConfigurationManager.ConnectionStrings["Core"].ConnectionString;
            SqlConnection conn = new SqlConnection(CadenaConexion);
            SqlCommand cmd = new SqlCommand("usp_E080_WSAutenticacion", conn);
            cmd.Parameters.Add("@UserName", SqlDbType.Char);
            cmd.Parameters.Add("@Password", SqlDbType.Char);
            cmd.Parameters["@UserName"].Value = Login;
            cmd.Parameters["@Password"].Value = Password;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            conn.Open();
            dt = DataAccess.GetDataTable(cmd);
            conn.Close();
            ValorRetorno = dt.Rows[0][0].ToString();

            return ValorRetorno;
        }

        public string ReturnXMLE080WSDatosConsultores(string Token, string Login, string CodConsultor)
        {
            DataTable dt;
            string ValorRetorno = "";
            string CadenaConexion = ConfigurationManager.ConnectionStrings["Core"].ConnectionString;
            SqlConnection conn = new SqlConnection(CadenaConexion);
            SqlCommand cmd = new SqlCommand("usp_E080_WSDatosConsultores", conn);
            cmd.Parameters.Add("@CodConsultor", SqlDbType.Char);
            cmd.Parameters.Add("@UserName", SqlDbType.Char);
            cmd.Parameters.Add("@Token", SqlDbType.Char);
            cmd.Parameters["@CodConsultor"].Value = CodConsultor;
            cmd.Parameters["@UserName"].Value = Login;
            cmd.Parameters["@Token"].Value = Token;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            conn.Open();
            dt = DataAccess.GetDataTable(cmd);
            conn.Close();
            ValorRetorno = dt.Rows[0][0].ToString();

            return ValorRetorno;
        }

        public int ExisWareHouseMaterialByWareHouseMaterial(int WarehouseID, int MaterialID)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspExisWareHouseMaterialByWareHouseMaterial",
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = WarehouseID },
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID }
                ); 
        }

        public int ExisWareHouseMaterialBySKU_ExternalCode(string SKU, string ExternalCode)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspExistWareHouseMaterialBySKU_ExternalCode",
                new SqlParameter("SKU", SqlDbType.VarChar) { Value = SKU },
                new SqlParameter("ExternalCode", SqlDbType.VarChar) { Value = ExternalCode }
                );
        }

        public int InsertWareHouseMaterial(int wareHouseID, int materialID, decimal saldo)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspUPDWareHouseMaterialsIntrface",
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID },
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = materialID },
                new SqlParameter("Saldo", SqlDbType.Decimal) { Value = saldo }
                ); 
        }

        public int UpdateInsertWareHouseMaterial(string SKU, string ExternalCode, string Saldo)
        {

            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspUPDWareHouseMaterialsInterface",
                new SqlParameter("SKU", SqlDbType.VarChar) { Value = SKU },
                new SqlParameter("ExternalCode", SqlDbType.VarChar) { Value = ExternalCode },
                new SqlParameter("Saldo", SqlDbType.VarChar) { Value = Saldo }
                );
        } 

        public List<DisbursementsSearchData> ObtenerDisbursementsService(int? periodo)
        {
            return DataAccess.ExecWithStoreProcedure <DisbursementsSearchData>(ConnectionStrings.BelcorpCommission, "uspGetDisbursementsService",
                new SqlParameter("Period", SqlDbType.Int) { Value = (object)periodo ?? DBNull.Value } 
                ).ToList();  
        } 
         
        public List<DisbursementProfilesSearchData> ObtenerDisbursementProfilesService(int? periodo)
        {
            return DataAccess.ExecWithStoreProcedure<DisbursementProfilesSearchData>(ConnectionStrings.BelcorpCommission, "uspDisbursementProfilesService",
               new SqlParameter("Period", SqlDbType.Int) { Value = (object)periodo ?? DBNull.Value }
               ).ToList();  
        }
    }
}
