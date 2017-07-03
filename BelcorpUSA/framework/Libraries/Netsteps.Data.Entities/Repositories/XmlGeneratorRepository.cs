using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Dto;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities;
using System.Configuration;
using System.Xml.Linq;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business;
using System.Globalization;

/*
 * @01 20150820 BR-E020 CSTI JMO: Added ValidarNotaFiscal and UpdateOrderStatusToShipped Methods
 * 
 */

namespace NetSteps.Data.Entities.Repositories
{
    public partial class XmlGeneratorRepository : IXmlGeneratorRepository
    {
        #region Propierties

        private char DecimalSeparator
        {
            get
            {
                if (ConfigurationManager.AppSettings["DecimalSeparator"] != null)
                {
                    return Convert.ToChar(ConfigurationManager.AppSettings["DecimalSeparator"].ToString());
                }
                return ',';
            }
        }

        #endregion

        public IEnumerable<OrderHeaderXmlDto> GetOrderHead(int OrderID)
        {

            List<OrderHeaderXmlDto> data = DataAccess.ExecWithStoreProcedureListParam<OrderHeaderXmlDto>(ConnectionStrings.BelcorpCore, 
                                                                                                       "DevolverCabeceraOrdenCanceladaXML",
                                                                                                        new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }).ToList();

            if (data == null)
                throw new Exception("Selected accounts not found");

            return data;
        }

        public IEnumerable<OrderDetailXmlDto> GetOrderDetail(int OrderID)
        {

            List<OrderDetailXmlDto> data = DataAccess.ExecWithStoreProcedureListParam<OrderDetailXmlDto>(ConnectionStrings.BelcorpCore,
                                                                                                       "DevolverDetalleOrdenCanceladaXML",
                                                                                                        new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }).ToList();
            //XDocument doc = XDocument.Parse(str);
            

            if (data == null)
                throw new Exception("Selected accounts not found");

            return data;
        }

        public IEnumerable<AdvancePaymentXmlDto> GetAdvancePayment(int OrderID)
        {

            List<AdvancePaymentXmlDto> data = DataAccess.ExecWithStoreProcedureListParam<AdvancePaymentXmlDto>(ConnectionStrings.BelcorpCore,
                                                                                                       "DevolverAdiantamentoOrdenCanceladaXML",
                                                                                                        new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }).ToList();

            if (data == null)
                throw new Exception("Selected accounts not found");

            return data;
        }

        /// <summary>
        /// Developed By KTC - CSTI
        /// BR-B200
        /// </summary>
        /// <param name="Material">MaterialXmlDto</param>
        /// <returns>List<MaterialLogXmlDto></returns>
        /// <summary>
        /// Developed By KTC - CSTI
        /// BR-B200
        /// </summary>
        /// <param name="Material">MaterialXmlDto</param>
        /// <returns>List<MaterialLogXmlDto></returns>
        public List<MaterialLogXmlDto> InsertMaterialDto(MaterialXmlDto Material)
        {


            List<MaterialLogXmlDto> data = DataAccess.ExecWithStoreProcedureListParam<MaterialLogXmlDto>(ConnectionStrings.BelcorpCore, "upsInsertMaterialsServ",
                      new SqlParameter("SKU", SqlDbType.VarChar) { Value = Material.SKU },
                      new SqlParameter("BPCS", SqlDbType.VarChar) { Value = Material.BPCS },
                      new SqlParameter("Brand", SqlDbType.VarChar) { Value = Material.Brand },
                      new SqlParameter("Group", SqlDbType.VarChar) { Value = Material.Group },
                      new SqlParameter("NamePort", SqlDbType.VarChar) { Value = Material.NamePort },
                      new SqlParameter("NameEsp", SqlDbType.VarChar) { Value = Material.NameEsp },
                      new SqlParameter("Volume", SqlDbType.Decimal) { Value = Material.Volume },
                      new SqlParameter("Weight", SqlDbType.Decimal) { Value = Material.Weight },
                      new SqlParameter("Hierachy", SqlDbType.VarChar) { Value = Material.Hierachy }
                    ).ToList();



            return data;

            //try
            //{
            //    List<MaterialLogXmlDto> results = new List<MaterialLogXmlDto>();
            //    var con = ConnectionStrings.BelcorpCore;

            //    //using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            //    string conn = ConfigurationManager.AppSettings["nsCore"];
            //    using (SqlConnection connection = new SqlConnection("Data Source=10.12.6.187;Initial Catalog=BelcorpBRACore;Persist Security Info=True;Integrated Security=false;Application Name=GMP;MultipleActiveResultSets=True;uid=usrencorebrasilqas;pwd=Belcorp2014"))
            //    {

            //        connection.Open();
            //        var parameters = new SqlParameter[] {
            //                        new SqlParameter("@SKU",  Material.SKU ),
            //                        new SqlParameter("@BPCS", Material.BPCS),
            //                        new SqlParameter("@Brand", Material.Brand ),
            //                        new SqlParameter("@Group", Material.Group  ),
            //                        new SqlParameter("@NamePort", Material.NamePort ),
            //                        new SqlParameter("@NameEsp", Material.NameEsp ),
            //                        new SqlParameter("@Volume",  Material.Volume ),
            //                        new SqlParameter("@Weight", Material.Weight ),
            //                        new SqlParameter("@Hierachy", Material.Hierachy )
            //    };
            //        SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[upsInsertMaterialsServ]", connection, parameters);
            //        while (dataReader.Read())
            //        {


            //            results.Add(new MaterialLogXmlDto()
            //            {
            //                CampoError = dataReader.IsDBNull(0) ? "" : dataReader.GetString(0),
            //                DescError = dataReader.IsDBNull(1) ? "" : dataReader.GetString(1)
            //            });

            //        }

            //    }
            //    return results;
            //}
            //catch (Exception ex)
            //{
            //    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            //}
        }

        /// <summary>
        /// Developed By KTC - CSTI
        /// BR-B200
        /// </summary>
        /// <param name="MatCenter">MaterialCentersXmlDto</param>
        /// <returns>MaterialLogXmlDto</returns>
        public MaterialLogXmlDto InsertWarehouseMaterialsDto(MaterialCentersXmlDto MatCenter)
        {
            MaterialLogXmlDto data = new MaterialLogXmlDto();


            data = DataAccess.ExecWithStoreProcedureListParam<MaterialLogXmlDto>(ConnectionStrings.BelcorpCore,
                                                                                                        "upsInsertWarehouseMaterialsServ",
                                                                                                         new SqlParameter("SKU", SqlDbType.VarChar) { Value = MatCenter.SKU },
                                                                                                         new SqlParameter("CostAvarage", SqlDbType.Decimal) { Value = MatCenter.Costo },
                                                                                                         new SqlParameter("ExternalCode", SqlDbType.VarChar) { Value = MatCenter.Centro }
                                                                                                         ).ToList().FirstOrDefault();

            return data;

            //List<MaterialLogXmlDto> data = new List<MaterialLogXmlDto>();
            //try
            //{

            //    //data = DataAccess.ExecWithStoreProcedureListParam<MaterialLogXmlDto>(ConnectionStrings.BelcorpCore,
            //    //                                                                                            "upsInsertWarehouseMaterialsServ",
            //    //                                                                                             new SqlParameter("SKU", SqlDbType.VarChar) { Value = MatCenter.SKU },
            //    //                                                                                             new SqlParameter("CostAverage", SqlDbType.Decimal) { Value = MatCenter.Costo },
            //    //                                                                                             new SqlParameter("Centro", SqlDbType.VarChar) { Value = MatCenter.Centro }
            //    //                                                                                             ).ToList();

            //    //if (data == null)
            //    //    throw new Exception("Selected accounts not found");
            //    string conn = ConfigurationManager.AppSettings["nsCore"];
            //    using (SqlConnection connection = new SqlConnection("Data Source=10.12.6.183;Initial Catalog=BelcorpBRACore;Persist Security Info=True;Integrated Security=false;Application Name=GMP;MultipleActiveResultSets=True;uid=usrencorebrasilqas;pwd=Belcorp2014"))
            //    {

            //        connection.Open();
            //        var parameters = new SqlParameter[] {
            //                        new SqlParameter("@SKU",  MatCenter.SKU ),
            //                        new SqlParameter("@CostAvarage", MatCenter.Costo),
            //                        new SqlParameter("@ExternalCode", MatCenter.Centro )
            //    };
            //        SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[upsInsertWarehouseMaterialsServ]", connection, parameters);
            //        while (dataReader.Read())
            //        {


            //            data.Add(new MaterialLogXmlDto()
            //            {
            //                CampoError = dataReader.IsDBNull(0) ? "" : dataReader.GetString(0),
            //                DescError = dataReader.IsDBNull(1) ? "" : dataReader.GetString(1)
            //            });

            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            //}
            //return data.FirstOrDefault();

        }
        #region InsSAPEncoreFacturas
        /// <summary>
        /// Developed By KLC - CSTI
        /// BR-B070
        /// </summary>
        /// <param name="facturas"></param>
        /// <returns></returns>
        public List<BalancesBillOrdersXml> InsSAPEncoreFacturas(BalancesBillOrdersXmlDto OrderInvoice, BalancesBillOrdersItemsXmlDto InvoiceDetail)
        {
            try
            {
                List<BalancesBillOrdersXml> InsSAPEncoreFacturas = DataAccess.ExecWithStoreProcedureListParam<BalancesBillOrdersXml>(ConnectionStrings.BelcorpCore, "SAPEncoreFacturas",
                new SqlParameter("OrderNumber", SqlDbType.VarChar) { Value = OrderInvoice.OrderNumber.TrimStart('0') },
                new SqlParameter("InvoiceNumber", SqlDbType.VarChar) { Value = OrderInvoice.InvoiceNumber.TrimStart('0') },
                new SqlParameter("InvoiceSerie", SqlDbType.VarChar) { Value = OrderInvoice.InvoiceSerie },
                new SqlParameter("DateInvoice", SqlDbType.DateTime) { Value = OrderInvoice.DateInvoice },
                new SqlParameter("InvoiceType", SqlDbType.VarChar) { Value = OrderInvoice.InvoiceType },
                new SqlParameter("SortLine", SqlDbType.VarChar) { Value = InvoiceDetail.SortLine.TrimStart('0') },
                new SqlParameter("ChaveNFe", SqlDbType.VarChar) { Value = OrderInvoice.ChaveNFe },
                new SqlParameter("Boxes", SqlDbType.Int) { Value = OrderInvoice.Boxes.TrimStart('0') },
                new SqlParameter("Weight", SqlDbType.Decimal) { Value = OrderInvoice.Weight.Replace('.', this.DecimalSeparator) },
                new SqlParameter("Quantity", SqlDbType.Decimal) { Value = InvoiceDetail.Quantity.Replace('.', this.DecimalSeparator) },
                new SqlParameter("ICMS", SqlDbType.Money) { Value = InvoiceDetail.ICMS },
                new SqlParameter("ICMS_ST", SqlDbType.Money) { Value = InvoiceDetail.ICMS_ST },
                new SqlParameter("IPI", SqlDbType.Money) { Value = InvoiceDetail.IPI },
                new SqlParameter("PIS", SqlDbType.Money) { Value = InvoiceDetail.PIS },
                new SqlParameter("COFINS", SqlDbType.Money) { Value = InvoiceDetail.COFINS },
                new SqlParameter("InvoiceValue", SqlDbType.Money) { Value = InvoiceDetail.InvoiceUnitValue },
                new SqlParameter("InvoicePath", SqlDbType.VarChar) { Value = OrderInvoice.InvoicePath },
                    /// Modificación: Campos nuevos 
                    /// Fecha: 17/03/2016 
                    /// Author: MAM - CSTI
                new SqlParameter("WeightLiq", SqlDbType.Decimal) { Value = OrderInvoice.WeightLiq.Replace('.', this.DecimalSeparator) },
                new SqlParameter("HeadICMSBase", SqlDbType.Money) { Value = OrderInvoice.HeadICMSBase },
                new SqlParameter("HeadICMSValue", SqlDbType.Money) { Value = OrderInvoice.HeadICMSValue },
                new SqlParameter("HeadICMSSTBase", SqlDbType.Money) { Value = OrderInvoice.HeadICMSSTBase },
                new SqlParameter("HeadICMSSTValue", SqlDbType.Money) { Value = OrderInvoice.HeadICMSSTValue },
                new SqlParameter("HeadIPIBase", SqlDbType.Money) { Value = OrderInvoice.HeadIPIBase },
                new SqlParameter("HeadIPIValue", SqlDbType.Money) { Value = OrderInvoice.HeadIPIValue },
                new SqlParameter("QuantityPicked", SqlDbType.Int) { Value = InvoiceDetail.QuantityPicked },
                new SqlParameter("ICMSAliq", SqlDbType.Decimal) { Value = InvoiceDetail.ICMSAliq.Replace('.', this.DecimalSeparator) },
                new SqlParameter("IPIAliq", SqlDbType.Decimal) { Value = InvoiceDetail.IPIAliq.Replace('.', this.DecimalSeparator) },
                new SqlParameter("CFOP", SqlDbType.VarChar) { Value = InvoiceDetail.CFOP }
                    /// Fin Modificación.

                ).ToList();
                return InsSAPEncoreFacturas;


            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        } 
        #endregion

        #region InsSAPEncoreFacturasUpdateCvValue
        /// <summary>
        /// Developed By MAM - CSTI
        /// BR-B070
        /// </summary>
        /// <param name="facturas"></param>
        /// <returns></returns>
        public List<BalancesBillOrdersXml> InsSAPEncoreFacturasUpdateCvValue(string orderNumber)
        {
            try
            {
                List<BalancesBillOrdersXml> InsSAPEncoreFacturas = DataAccess.ExecWithStoreProcedureListParam<BalancesBillOrdersXml>(ConnectionStrings.BelcorpCore, "SAPEncoreFacturasUpdateCVValue",
                new SqlParameter("OrderNumber", SqlDbType.NVarChar) { Value = orderNumber}
                ).ToList();
                return InsSAPEncoreFacturas;


            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        } 
        #endregion

        #region InsSAPEncorePicking
        /// <summary>
        /// Developed By KLC - CSTI
        /// BR-B055
        /// </summary>
        /// <param name="facturas"></param>
        /// <returns></returns>
        public List<BalancesBillOrdersXml> InsSAPEncorePicking(BalancesBillOrdersXmlDto OrderInvoice, BalancesBillOrdersItemsXmlDto InvoiceDetail)
        {
            try
            {
                List<BalancesBillOrdersXml> InsSAPEncorePicking = DataAccess.ExecWithStoreProcedureListParam<BalancesBillOrdersXml>(ConnectionStrings.BelcorpCore, "SAPEncorePicking",
                new SqlParameter("OrderNumber", SqlDbType.VarChar) { Value = OrderInvoice.OrderNumber.TrimStart('0') },
                new SqlParameter("InvoiceNumber", SqlDbType.VarChar) { Value = OrderInvoice.InvoiceNumber.TrimStart('0') },
                new SqlParameter("InvoiceSerie", SqlDbType.VarChar) { Value = OrderInvoice.InvoiceSerie },
                new SqlParameter("DateInvoice", SqlDbType.DateTime) { Value = OrderInvoice.DateInvoice },
                new SqlParameter("InvoiceType", SqlDbType.VarChar) { Value = OrderInvoice.InvoiceType },
                new SqlParameter("SortLine", SqlDbType.VarChar) { Value = InvoiceDetail.SortLine.TrimStart('0') },
                new SqlParameter("ChaveNFe", SqlDbType.VarChar) { Value = OrderInvoice.ChaveNFe },
                new SqlParameter("Boxes", SqlDbType.Int) { Value = OrderInvoice.Boxes.TrimStart('0') },
                new SqlParameter("Weight", SqlDbType.Decimal) { Value = OrderInvoice.Weight.Replace('.', this.DecimalSeparator) },
                new SqlParameter("Quantity", SqlDbType.Decimal) { Value = InvoiceDetail.Quantity.Replace('.', this.DecimalSeparator) },
                new SqlParameter("ICMS", SqlDbType.Money) { Value = InvoiceDetail.ICMS },
                new SqlParameter("ICMS_ST", SqlDbType.Money) { Value = InvoiceDetail.ICMS_ST },
                new SqlParameter("IPI", SqlDbType.Money) { Value = InvoiceDetail.IPI },
                new SqlParameter("PIS", SqlDbType.Money) { Value = InvoiceDetail.PIS },
                new SqlParameter("COFINS", SqlDbType.Money) { Value = InvoiceDetail.COFINS },
                new SqlParameter("InvoiceValue", SqlDbType.Money) { Value = InvoiceDetail.InvoiceUnitValue },
                new SqlParameter("InvoicePath", SqlDbType.VarChar) { Value = OrderInvoice.InvoicePath },
                    /// Modificación: Campos nuevos 
                    /// Fecha: 17/03/2016 
                    /// Author: MAM - CSTI
                new SqlParameter("WeightLiq", SqlDbType.Decimal) { Value = OrderInvoice.WeightLiq.Replace('.', this.DecimalSeparator) },
                new SqlParameter("HeadICMSBase", SqlDbType.Money) { Value = OrderInvoice.HeadICMSBase },
                new SqlParameter("HeadICMSValue", SqlDbType.Money) { Value = OrderInvoice.HeadICMSValue },
                new SqlParameter("HeadICMSSTBase", SqlDbType.Money) { Value = OrderInvoice.HeadICMSSTBase },
                new SqlParameter("HeadICMSSTValue", SqlDbType.Money) { Value = OrderInvoice.HeadICMSSTValue },
                new SqlParameter("HeadIPIBase", SqlDbType.Money) { Value = OrderInvoice.HeadIPIBase },
                new SqlParameter("HeadIPIValue", SqlDbType.Money) { Value = OrderInvoice.HeadIPIValue },
                new SqlParameter("QuantityPicked", SqlDbType.Int) { Value = InvoiceDetail.QuantityPicked },
                new SqlParameter("ICMSAliq", SqlDbType.Decimal) { Value = InvoiceDetail.ICMSAliq.Replace('.', this.DecimalSeparator) },
                new SqlParameter("IPIAliq", SqlDbType.Decimal) { Value = InvoiceDetail.IPIAliq.Replace('.', this.DecimalSeparator) },
                new SqlParameter("CFOP", SqlDbType.VarChar) { Value = InvoiceDetail.CFOP }
                    /// Fin Modificación.

                ).ToList();
                return InsSAPEncorePicking;


            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion

        #region InsSAPEncorePickingGenerateResidual
        /// <summary>
        /// Developed By MAM - CSTI
        /// BR-B055
        /// </summary>
        /// <param name="facturas"></param>
        /// <returns></returns>
        public List<BalancesBillOrdersXml> InsSAPEncorePickingGenerateResidual(string orderNumber)
        {
            try
            {
                List<BalancesBillOrdersXml> InsSAPEncoreFacturas = DataAccess.ExecWithStoreProcedureListParam<BalancesBillOrdersXml>(ConnectionStrings.BelcorpCore, "SAPEncorePickingGenerateResidual",
                new SqlParameter("OrderNumber", SqlDbType.NVarChar) { Value = orderNumber}
                ).ToList();
                return InsSAPEncoreFacturas;


            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion

        /* @01 A01*/
        #region [E020]

        /// <summary>
        /// Valida integridad de Nota Fiscal
        /// </summary>
        public Tuple<bool, bool, int, int> ValidarNotaFiscal(int NumeroNotaFiscal, int NumeroSerie)
        {
            Tuple<bool, bool, int, int> result = new Tuple<bool, bool, int, int>(false, false, -1, -1);

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[ValidarNotaFiscal]";
                cmd.Parameters.AddWithValue("@NumeroNotaFiscal", NumeroNotaFiscal);
                cmd.Parameters.AddWithValue("@NumeroSerie", NumeroSerie);
                cmd.Parameters.AddWithValue("@ValOrderInvoice", result.Item1);
                cmd.Parameters.AddWithValue("@ValOrder", result.Item2);
                cmd.Parameters.AddWithValue("@OrderID", result.Item3);
                cmd.Parameters.AddWithValue("@OrderCustomerID", result.Item4);
                cmd.Parameters["@ValOrderInvoice"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ValOrder"].Direction = ParameterDirection.Output;
                cmd.Parameters["@OrderID"].Direction = ParameterDirection.Output;
                cmd.Parameters["@OrderCustomerID"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Tuple.Create(Convert.ToBoolean(cmd.Parameters["@ValOrderInvoice"].Value),
                                      Convert.ToBoolean(cmd.Parameters["@ValOrder"].Value),
                                      Convert.ToInt32(cmd.Parameters["@OrderID"].Value),
                                      Convert.ToInt32(cmd.Parameters["@OrderCustomerID"].Value));
            }
            return result;
        }

        /// <summary>
        /// Updates Orders status to Shipped
        /// </summary>
        public void UpdateOrderStatusToShipped(List<int> ListOrderID)
        {
            try
            {
                DataTable dtListOrderID = new DataTable();
                dtListOrderID.Columns.Add("OrderID");

                foreach (var orderID in ListOrderID)
                {
                    dtListOrderID.Rows.Add(orderID);
                }

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[UpdateOrderStatusToShipped]";
                    cmd.Parameters.AddWithValue("@ListOrderID", dtListOrderID).SqlDbType = SqlDbType.Structured;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        /* @01 A01*/

        //E030 - LIB
        #region [E030]

        public bool ValidarCnpj(string cnpjValue)
        {

            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {

                connection.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[uspValidarCnpj]";
                cmd.Parameters.AddWithValue("@cpnjValue", cnpjValue);
                cmd.Parameters.AddWithValue("@isValid", result);
                cmd.Parameters["@isValid"].Direction = ParameterDirection.Output;


                cmd.ExecuteNonQuery();
                result = Convert.ToBoolean(cmd.Parameters["@isValid"].Value);

            }

            return result;

        }


        public void InsertOrderShipmentTracking(int orderCustomerID, int situacao, string observacao, string dataOcorrencia)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {

                    connection.Open();
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[uspInsertOrderShipmentTracking]";
                    cmd.Parameters.AddWithValue("@OrderCustomerID", orderCustomerID);
                    cmd.Parameters.AddWithValue("@Situacao", situacao);
                    cmd.Parameters.AddWithValue("@Observacao", observacao);
                    cmd.Parameters.AddWithValue("@DataOcorrencia", dataOcorrencia);
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Req BR010 Pedidos Encore - SAP 3PL  Dev. SvG G&S
        public List<ClientXmlDto> GetClientOrder(int LoteID)
        {
            List<ClientXmlDto> result = new List<ClientXmlDto>();
            ClientXmlDto objClientXmlDto = new ClientXmlDto();
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() 
                        {  
                                { "@LoteID", LoteID }                                                                                              
                        };

                SqlDataReader reader = DataAccess.GetDataReader("SpClientOrder", parameters, "Core");
                if (reader.HasRows)
                {
                    #region Cargar data
                    while (reader.Read())
                    {
                        objClientXmlDto = new ClientXmlDto();
                        if (!Convert.IsDBNull(reader["CEP"]))
                        {
                            objClientXmlDto.CEP = reader["CEP"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["OrderNumber"]))
                        {
                            objClientXmlDto.OrderNumber = reader["OrderNumber"].ToString();
                        }

                        if (!Convert.IsDBNull(reader["CEPRecebedor"]))
                        {
                            objClientXmlDto.CEPRecebedor = reader["CEPRecebedor"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["Cidade"]))
                        {
                            objClientXmlDto.Cidade = reader["Cidade"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["CidadeRecebedor"]))
                        {
                            objClientXmlDto.CidadeRecebedor = reader["CidadeRecebedor"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["ClienteID"]))
                        {
                            objClientXmlDto.ClienteID = Convert.ToInt32(reader["ClienteID"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["CPF"]))
                        {
                            objClientXmlDto.CPF = Convert.ToString(reader["CPF"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["Email"]))
                        {
                            objClientXmlDto.Email = Convert.ToString(reader["Email"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["EmailRecebedor"]))
                        {
                            objClientXmlDto.EmailRecebedor = Convert.ToString(reader["EmailRecebedor"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["ENovo"]))
                        {
                            objClientXmlDto.ENovo = Convert.ToInt32(reader["ENovo"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["Nome"]))
                        {
                            objClientXmlDto.Nome = Convert.ToString(reader["Nome"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["NumeroRua"]))
                        {
                            objClientXmlDto.NumeroRua = Convert.ToString(reader["NumeroRua"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["NumeroRuaRecebedor"]))
                        {
                            objClientXmlDto.NumeroRuaRecebedor = Convert.ToString(reader["NumeroRuaRecebedor"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["Recebedor"]))
                        {
                            objClientXmlDto.Recebedor = Convert.ToString(reader["Recebedor"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["Regiao"]))
                        {
                            objClientXmlDto.Regiao = Convert.ToString(reader["Regiao"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["RegiaoRecebedor"]))
                        {
                            objClientXmlDto.RegiaoRecebedor = Convert.ToString(reader["RegiaoRecebedor"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["Rua"]))
                        {
                            objClientXmlDto.Rua = Convert.ToString(reader["Rua"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["RuaRecebedor"]))
                        {
                            objClientXmlDto.RuaRecebedor = Convert.ToString(reader["RuaRecebedor"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["SetorIndustrial"]))
                        {
                            objClientXmlDto.SetorIndustrial = Convert.ToInt32(reader["SetorIndustrial"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["OrderCustomerID"]))
                        {
                            objClientXmlDto.OrderCustomerID = Convert.ToInt32(reader["OrderCustomerID"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["NumeroPedido"]))
                        {
                            objClientXmlDto.NumeroPedido = Convert.ToString(reader["NumeroPedido"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["Barrio"]))
                        {
                            objClientXmlDto.Barrio = Convert.ToString(reader["Barrio"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["BairroRecebedor"]))
                        {
                            objClientXmlDto.BairroRecebedor = Convert.ToString(reader["BairroRecebedor"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["Sexo"]))
                        {
                            objClientXmlDto.Sexo = reader["Sexo"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["REFERENCIALOCL"]))
                        {
                            objClientXmlDto.REFERENCIALOCL = reader["REFERENCIALOCL"].ToString();
                        }

                        
                        #region Cliente rapidao
                        //if (!Convert.IsDBNull(reader["Bairro"]))
                        //{
                        //    objClientXmlDto.Bairro = Convert.ToString(reader["Bairro"].ToString());
                        //}
                        //if (!Convert.IsDBNull(reader["NumeroRua2"]))
                        //{
                        //    objClientXmlDto.NumeroRua2 = Convert.ToString(reader["NumeroRua2"].ToString());
                        //}
                        //if (!Convert.IsDBNull(reader["NumeroRuaRecebedor2"]))
                        //{
                        //    objClientXmlDto.NumeroRuaRecebedor2 = Convert.ToString(reader["NumeroRuaRecebedor2"].ToString());
                        //}
                        //if (!Convert.IsDBNull(reader["BairroRecebedor"]))
                        //{
                        //    objClientXmlDto.BairroRecebedor = Convert.ToString(reader["BairroRecebedor"].ToString());
                        //}
                        //if (!Convert.IsDBNull(reader["TelefoneFixo"]))
                        //{
                        //    objClientXmlDto.TelefoneFixo = Convert.ToString(reader["TelefoneFixo"].ToString());
                        //}
                        //if (!Convert.IsDBNull(reader["TelefoneCelular"]))
                        //{
                        //    objClientXmlDto.TelefoneCelular = Convert.ToString(reader["TelefoneCelular"].ToString());
                        //}
                        //if (!Convert.IsDBNull(reader["SituacaoCliente"]))
                        //{
                        //    objClientXmlDto.SituacaoCliente = Convert.ToString(reader["SituacaoCliente"].ToString());
                        //}
                        #endregion

                        result.Add(objClientXmlDto);
                    }
                    return result;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public List<AdiantamentoXmlDto> GetAdiantamentoOrder(int LoteID, CultureInfo CurrentCultureInfo)
        {
            List<AdiantamentoXmlDto> result = new List<AdiantamentoXmlDto>();
            AdiantamentoXmlDto objAdiantamentoXmlDto = null;
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() 
                        {  
                                { "@LoteID", LoteID }                                                                                              
                        };

                SqlDataReader reader = DataAccess.GetDataReader("UspSelAdiantamento", parameters, "Core");
                if (reader.HasRows)
                {
                    #region Cargar data
                    while (reader.Read())
                    {
                        objAdiantamentoXmlDto = new AdiantamentoXmlDto();

                        if (!Convert.IsDBNull(reader["BancoRecebedorBoleto"]))
                        {
                            objAdiantamentoXmlDto.BancoRecebedorBoleto = Convert.ToString(reader["BancoRecebedorBoleto"]);
                        }
                        if (!Convert.IsDBNull(reader["CreditoPedidoAnterior"]))
                        {
                            objAdiantamentoXmlDto.CreditoPedidoAnterior = Convert.ToDecimal(reader["CreditoPedidoAnterior"]);
                        }
                        if (!Convert.IsDBNull(reader["DataRecebimentoBoleto"]))
                        {
                            objAdiantamentoXmlDto.DataRecebimentoBoleto = Convert.ToDateTime(reader["DataRecebimentoBoleto"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["DebitoPedidoAnterior"]))
                        {
                            objAdiantamentoXmlDto.DebitoPedidoAnterior = Convert.ToDecimal(reader["DebitoPedidoAnterior"]);
                        }
                        if (!Convert.IsDBNull(reader["OperadoraCartaoCred"]))
                        {
                            objAdiantamentoXmlDto.OperadoraCartaoCred = Convert.ToInt32(reader["OperadoraCartaoCred"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["OrderCustomerID"]))
                        {
                            objAdiantamentoXmlDto.OrderCustomerID = Convert.ToInt32(reader["OrderCustomerID"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["PrimeiraParcelaBoleto"]))
                        {
                            objAdiantamentoXmlDto.PrimeiraParcelaBoleto = Convert.ToDecimal(reader["PrimeiraParcelaBoleto"]);
                        }
                        if (!Convert.IsDBNull(reader["RecebidoBoleto"]))
                        {
                            objAdiantamentoXmlDto.RecebidoBoleto = Convert.ToDecimal(reader["RecebidoBoleto"]);
                        }
                        //if (!Convert.IsDBNull(reader["ValorAdiantamento"]))
                        //{
                        //    objAdiantamentoXmlDto.ValorAdiantamento = Convert.ToDecimal(reader["ValorAdiantamento"].ToString());
                        //}
                        if (!Convert.IsDBNull(reader["ValorCobradoCartaoCred"]))
                        {
                            objAdiantamentoXmlDto.ValorCobradoCartaoCred = Convert.ToDecimal(reader["ValorCobradoCartaoCred"]);
                        }
                        if (!Convert.IsDBNull(reader["ValorFatura"]))
                        {
                            objAdiantamentoXmlDto.ValorFatura = Convert.ToDecimal(reader["ValorFatura"]);
                        }
                        if (!Convert.IsDBNull(reader["NumeroPedido"]))
                        {
                            objAdiantamentoXmlDto.NumeroPedido = reader["NumeroPedido"].ToString();
                        }
                         if (!Convert.IsDBNull(reader["NumeroTitulo"]))
                        {
                            objAdiantamentoXmlDto.NumeroTitulo = reader["NumeroTitulo"].ToString();
                        }
                         if (!Convert.IsDBNull(reader["NumeroParcelas"]))
                        {
                            objAdiantamentoXmlDto.NumeroParcelas = reader["NumeroParcelas"].ToString();
                        }
                        
                        
                        result.Add(objAdiantamentoXmlDto);
                    }
                    return result;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public List<PedidoXmlDto> GetPedidoOrder(int LoteID, CultureInfo CurrentCultureInfo)
        {
            List<PedidoXmlDto> result = new List<PedidoXmlDto>();
            PedidoXmlDto objPedidoXmlDto = null;
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() 
                        {  
                                { "@LoteID", LoteID }                                                                                              
                        };

                SqlDataReader reader = DataAccess.GetDataReader("SpSelPedidoOrden", parameters, "Core");
                if (reader.HasRows)
                {
                    #region Cargar data
                    while (reader.Read())
                    {
                        objPedidoXmlDto = new PedidoXmlDto();
                        if (!Convert.IsDBNull(reader["DataPedido"]))
                        {
                            objPedidoXmlDto.DataPedido = Convert.ToDateTime(reader["DataPedido"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["EmisordaOrdem"]))
                        {
                            objPedidoXmlDto.EmisordaOrdem = Convert.ToInt32(reader["EmisordaOrdem"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["Frete"]))
                        {
                            objPedidoXmlDto.Frete =Convert.ToDecimal( reader["Frete"].ToString() );                           
                        }
                        if (!Convert.IsDBNull(reader["Incoterm"]))
                        {
                            objPedidoXmlDto.Incoterm = reader["Incoterm"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["NumeroPedido"]))
                        {
                            objPedidoXmlDto.NumeroPedido = reader["NumeroPedido"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["RecebedorMercaderia"]))
                        {
                            objPedidoXmlDto.RecebedorMercaderia = reader["RecebedorMercaderia"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["TipoOrdem"]))
                        {
                            objPedidoXmlDto.TipoOrdem = reader["TipoOrdem"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["Trasportador"]))
                        {
                            objPedidoXmlDto.Trasportador = reader["Trasportador"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["Trasportador"]))
                        {
                            objPedidoXmlDto.Trasportador = reader["Trasportador"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["OrderCustomerID"]))
                        {
                            objPedidoXmlDto.OrderCustomerID = Convert.ToInt32(reader["OrderCustomerID"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["FormaPgto"]))
                        {
                            objPedidoXmlDto.FormaPgto = reader["FormaPgto"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["LoteTransporte"]))
                        {
                            objPedidoXmlDto.LoteTransporte =Convert.ToInt32( reader["LoteTransporte"].ToString());
                        }

                        
                        #region Rapidao
                        //if (!Convert.IsDBNull(reader["NomeRecebedor"]))
                        //{
                        //    objPedidoXmlDto.NomeRecebedor = Convert.ToString(reader["NomeRecebedor"].ToString());
                        //}
                        //if (!Convert.IsDBNull(reader["CEP"]))
                        //{
                        //    objPedidoXmlDto.CEP = Convert.ToString(reader["CEP"].ToString());
                        //}
                        //if (!Convert.IsDBNull(reader["Rua"]))
                        //{
                        //    objPedidoXmlDto.Rua = Convert.ToString(reader["NumeroRua"].ToString());
                        //}

                        //if (!Convert.IsDBNull(reader["NumeroRua"]))
                        //{
                        //    objPedidoXmlDto.NumeroRua = Convert.ToString(reader["NumeroRua"].ToString());
                        //}
                        //if (!Convert.IsDBNull(reader["NumeroRua2"]))
                        //{
                        //    objPedidoXmlDto.NumeroRua2 = Convert.ToString(reader["NumeroRua2"].ToString());
                        //}

                        #endregion
                        result.Add(objPedidoXmlDto);
                    }
                    return result;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public List<OrderItemsXmlDto> GetDetailOrder(int LoteID, int OrderInvoiceIDIniPOut, int OrderInvoiceIDFinPOut, CultureInfo CurrentCultureInfo)
        {
            List<OrderItemsXmlDto> result = new List<OrderItemsXmlDto>();
            OrderItemsXmlDto objOrderItemsXmlDto = new OrderItemsXmlDto();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() 
                        {  
                                { "@LoteID", LoteID }  ,
                                { "@OrderInvoiceIDIniPOut", OrderInvoiceIDIniPOut}  ,
                                { "@OrderInvoiceIDFinPOut", OrderInvoiceIDFinPOut}  ,
                                
                                                            
                        };

                SqlDataReader reader = DataAccess.GetDataReader("SpSelDetaillOrden", parameters, "Core");
                if (reader.HasRows)
                {
                    #region Cargar data
                    while (reader.Read())
                    {
                        objOrderItemsXmlDto = new OrderItemsXmlDto();
                        if (!Convert.IsDBNull(reader["OrderCustomerID"]))
                        {
                            objOrderItemsXmlDto.OrderCustomerID = Convert.ToInt32(reader["OrderCustomerID"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["CategoriaItem"]))
                        {
                            objOrderItemsXmlDto.CategoriaItem = Convert.ToInt32(reader["CategoriaItem"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["CentroDistribucao"]))
                        {
                            objOrderItemsXmlDto.CentroDistribucao = reader["CentroDistribucao"].ToString();
                        }
                        if (!Convert.IsDBNull(reader["Desconto"]))
                        {
                            objOrderItemsXmlDto.Desconto = Convert.ToDecimal(reader["Desconto"]);
                        }
                        if (!Convert.IsDBNull(reader["Linea"]))
                        {
                            objOrderItemsXmlDto.Linea = Convert.ToInt32(reader["Linea"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["Material"]))
                        {
                            objOrderItemsXmlDto.Material = Convert.ToString(reader["Material"].ToString());
                        }
                        if (!Convert.IsDBNull(reader["PresoPraticado"]))
                        {
                            objOrderItemsXmlDto.PresoPraticado = Convert.ToDecimal(reader["PresoPraticado"]);
                        }
                        if (!Convert.IsDBNull(reader["Quantidade"]))
                        {
                            objOrderItemsXmlDto.Quantidade = Convert.ToInt32(reader["Quantidade"].ToString());
                        }

                        result.Add(objOrderItemsXmlDto);
                    }
                    return result;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }
        #endregion

        #region Req BR010   Returned Order
        public IEnumerable<ReturnOrderHeaderXmlDto> GetHeaderReturnedOrder(int OrderID)
        {

            List<ReturnOrderHeaderXmlDto> data = DataAccess.ExecWithStoreProcedureListParam<ReturnOrderHeaderXmlDto>(ConnectionStrings.BelcorpCore,
                                                                                                       "DevolverCabeceraOrdenDevolucionXML",
                                                                                                        new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }).ToList();

            if (data == null)
                throw new Exception("Order header not found");

            return data;
        }
        public IEnumerable<ReturnOrderDetailXmlDto> GetDetailReturnedOrder(int OrderID)
        {
            List<ReturnOrderDetailXmlDto> data = DataAccess.ExecWithStoreProcedureListParam<ReturnOrderDetailXmlDto>(ConnectionStrings.BelcorpCore,
                                                                                                       "DevolverDetalleOrdenDevolucionXML",
                                                                                                        new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }).ToList();
            if (data == null)
                throw new Exception("Order detail not found");

            return data;
        }

        #endregion
    }
}
