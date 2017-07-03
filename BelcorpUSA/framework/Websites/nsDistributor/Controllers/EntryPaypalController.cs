using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Context;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using nsDistributor.Models.Paypal;
using PayPal.Validator;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Collections.Generic;
using NetSteps.Encore.Core.IoC;
using OrderRules.Service.Interface;
using OrderRules.Core.Model;
using OrderRules.Service.DTO;
using OrderRules.Service.DTO.Converters;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Generated;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace nsDistributor.Controllers
{

    public class EntryPaypalController : BaseOrderContextController
    {
        
        public LoadPayment loadPayment = new LoadPayment();

        public ActionResult PreparePayment()
        {
            return PartialView();
        }


        public ActionResult FormPayPal()
        {
            PayPal_Tokens ppTokens = new PayPal_Tokens();
            PayPalLog paypalLog = new PayPalLog();

            try
            {
                OrderContext.Order.AsOrder().OrderPayments.Each(n => n.ExpirationStatusID = (int)ConstantsGenerated.ExpirationStatuses.Unexpired);
                OrderContext.Order.Save(); //Cambio Solicitado por KTorres para el funcionamiento de Save (OrderContext.Order.AsOrder().Save();)
                OrderContext.Order.AsOrder().IsTemplate = true;
                //Se retira esta linea KTorres lo implementa en metodo Save() (Order.UpdatePreOrder(OrderContext.Order.AsOrder().OrderID, Convert.ToInt32(Session["PreOrder"]));)
                var amount = "";

                PaymentsTable objE = new PaymentsTable();
                objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                var ct = Order.GetPaymentsTable(objE);

                if (OrderContext.Order.AsOrder().OrderPayments.Count > 1)
                {
                    for (int i = 0; i < OrderContext.Order.AsOrder().OrderPayments.Count; i++)
                    {
                        var paymentTable = ct[i];

                        if (OrderContext.Order.AsOrder().OrderPayments[i].PaymentTypeID == 1)
                        {
                            amount = paymentTable.AppliedAmount.ToString();
                        }
                    }
                }
                else
                {
                    
                    var paymentTable = ct[0];
                    amount = paymentTable.AppliedAmount.ToString();
                }

                var orderID = OrderContext.Order.AsOrder().OrderNumber;
                var accountID = "" + OrderContext.Order.AsOrder().OrderCustomers[0].AccountID;
                //Se Agrega Campo de Telefono para Pagos PayPal -- CSTI Juan Morales 15-07-2016
                var addressPayPal = DataAccess.ExecWithStoreProcedureScalarType<AddressPayPal>("Core", "PhoneAccountID", new SqlParameter("AccountID", SqlDbType.VarChar) { Value = accountID },
                                                                                                                       new SqlParameter("PostalCode", SqlDbType.VarChar) { Value = OrderContext.Order.AsOrder().OrderShipments[0].PostalCode });
                var cur = DataAccess.ExecWithStoreProcedureListParam<CurrencyModel>("Core", "spCurrencySym", new SqlParameter("CurrencyID", SqlDbType.VarChar) { Value = OrderContext.Order.AsOrder().CurrencyID }).ToList().First();

                ppTokens.Total = amount.Replace(",", ".");
                ppTokens.Currency = cur.CurrencyCode;
                ppTokens.OrderID = orderID;
                ppTokens.AccountID = accountID;
                ppTokens.CancelUrl = ConfigurationManager.AppSettings["urlApp"];
                ppTokens.ReturnUrl = ConfigurationManager.AppSettings["urlApp"];
                ppTokens.Experience_id = ConfigurationManager.AppSettings["experience_id"];
                ppTokens.Mode = ConfigurationManager.AppSettings["mode"];

                //Datos para preparar la autorización de Pago de PayPal
                cur.CultureInfo = cur.CultureInfo.Replace("-", "_");
                cur.CurrencyCode = cur.CurrencyCode.Substring(0, 2);

                //Agregar Valores para la dirección  -- CSTI Juan Morales 15-07-2016
                ppTokens.City = OrderContext.Order.AsOrder().OrderShipments[0].City;
                ppTokens.Country_code = cur.CurrencyCode;
                ppTokens.Line1 = addressPayPal.Street + " " + OrderContext.Order.AsOrder().OrderShipments[0].Address1 +
                                                        " " + OrderContext.Order.AsOrder().OrderShipments[0].Address2;
                ppTokens.Line2 = OrderContext.Order.AsOrder().OrderShipments[0].County != null ? OrderContext.Order.AsOrder().OrderShipments[0].County : "";
                ppTokens.Postal_code = OrderContext.Order.AsOrder().OrderShipments[0].PostalCode;
                ppTokens.State = OrderContext.Order.AsOrder().OrderShipments[0].State;
                ppTokens.Recipient_name = OrderContext.Order.AsOrder().ConsultantInfo.FirstName + " " +
                                          OrderContext.Order.AsOrder().ConsultantInfo.LastName;


                loadPayment.chargePayment(ref ppTokens);

                ViewData["language"] = cur.CultureInfo;
                ViewData["country"] = cur.CurrencyCode;
                ViewData["curr"] = cur.CurrencySymbol;
                ViewData["mode"] = ConfigurationManager.AppSettings["mode"];
                ViewData["error_formpaypal"] = "";
                //Se Agrega Campo de Telefono para Pagos PayPal -- CSTI Juan Morales 15-07-2016
                ViewData["phonenumber"] = addressPayPal.PhoneNumber;


            }
            catch (NullReferenceException e)
            {
                ViewData["approval_url"] = "{error:ERROR_PAYPALFORM}";
                ViewData["error_formpaypal"] = "ERROR: (" + e.Message + ")";

                //Guarda una traza en PayPal_Log
                paypalLog.Site = "PWS";
                paypalLog.OrderID = ppTokens.OrderID.ToInt();
                paypalLog.AccountId = ppTokens.AccountID.ToInt();
                paypalLog.PayPal_Response = "{error:ERROR_PAYPALFORM} - " + ppTokens.CreatedPayment;
                paypalLog.PayPal_Error = "ERROR: (" + e.Message + ")";
                paypalLog.PayPal_Process = "ChargePayment";
                paypalLog.PayPal_Status = "ERROR";
                paypalLog.PayPal_STS_Term = 0;
                paypalLog.PayPal_AmountPay = ppTokens.Total;

                InsertPayPal_Log(paypalLog);
                return PartialView();
            }

            //Datos para preparar la autorización de Pago de PayPal
            ViewData["approval_url"] = ppTokens.Approval_url;
            ViewData["execute_url"] = ppTokens.Execute_url;
            ViewData["token_id"] = ppTokens.Token_id;

            Session["ppTokens"] = ppTokens;

            //Guarda una traza en PayPal_Log
            paypalLog.Site = "PWS";
            paypalLog.OrderID = ppTokens.OrderID.ToInt();
            paypalLog.AccountId = ppTokens.AccountID.ToInt();
            paypalLog.PayPal_Response = ppTokens.CreatedPayment;
            paypalLog.PayPal_Error = ViewData["error_formpaypal"].ToString();
            paypalLog.PayPal_Process = "ChargePayment";
            paypalLog.PayPal_Status = "CHARGE";
            paypalLog.PayPal_STS_Term = 0;
            paypalLog.PayPal_AmountPay = ppTokens.Total;

            InsertPayPal_Log(paypalLog);

            return PartialView();
        }

        public string executePayment(string payer_Id)
        {
            string result = null;
            string message = string.Empty;
            String amountTotal = string.Empty;
            PayPalLog paypalLog = new PayPalLog();
            PayPal_Tokens ppTokens = new PayPal_Tokens();

            try
            {
                ppTokens = (PayPal_Tokens)Session["ppTokens"];
                ppTokens.Payer_Id = payer_Id;

                var lstVerifiedExecute = DataAccess.ExecWithStoreProcedureListParam<PayPalLog>("Core", "sp_VerifiedExecutePayment",
                                                                                             new SqlParameter("Site", SqlDbType.NVarChar) { Value = "DWS" },
                                                                                             new SqlParameter("OrderID", SqlDbType.Int) { Value = ppTokens.OrderID },
                                                                                             new SqlParameter("AccountId", SqlDbType.Int) { Value = ppTokens.AccountID },
                                                                                             new SqlParameter("PayPal_Process", SqlDbType.VarChar) { Value = "ExecutePayment" },
                                                                                             new SqlParameter("PayPal_Status", SqlDbType.VarChar) { Value = "APPROVED" }
                                                                                                ).ToList();

                PayPalLog verifiedExecute = null;
                if (lstVerifiedExecute.Count > 0)
                    verifiedExecute = lstVerifiedExecute.First<PayPalLog>();

                if (verifiedExecute == null)
                {
                    result = loadPayment.executePayment(ppTokens);
                }
                else
                {
                    return (Json(new { result = true, message = "", name = "OTHER_EXECUTE" })).ToString();
                }

                if (result.IndexOf("approved") > -1 || (result.IndexOf("PAYMENT_ALREADY_DONE") > -1 && verifiedExecute == null))
                {
                    //Guarda una traza en PayPal_Log
                    paypalLog.Site = "PWS";
                    paypalLog.OrderID = ppTokens.OrderID.ToInt();
                    paypalLog.AccountId = ppTokens.AccountID.ToInt();
                    paypalLog.PayPal_Response = result;
                    paypalLog.PayPal_Error = "";
                    paypalLog.PayPal_Process = "ExecutePayment";
                    paypalLog.PayPal_Status = "APPROVED";
                    paypalLog.PayPal_STS_Term = 1;
                    paypalLog.PayPal_AmountPay = ppTokens.Total;

                    InsertPayPal_Log(paypalLog);
                    
                    string payID = result.Substring(0, result.IndexOf(",")).ToString();
                    payID = Regex.Replace(payID, @"[^\w\.@-]", "", RegexOptions.None);
                    payID = payID.Replace("id", "");

                    PaymentsTable objE = new PaymentsTable();
                    objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                    var ct = Order.GetPaymentsTable(objE);
                    if (OrderContext.Order.AsOrder().OrderPayments.Count > 1)
                    {
                        for (int i = 0; i < OrderContext.Order.AsOrder().OrderPayments.Count; i++)
                        {
                            
                            var paymentTable = ct[i];
                            
                            if (paymentTable.PaymentStatusID == 4)
                            {
                                OrderContext.Order.AsOrder().OrderPayments[i].OrderPaymentStatusID = 2;
                            }
                            else if (paymentTable.PaymentStatusID == 11 || paymentTable.PaymentStatusID == 18)
                            {
                                OrderContext.Order.AsOrder().OrderPayments[i].OrderPaymentStatusID = 1;
                            }

                            OrderContext.Order.AsOrder().OrderPayments[i].ExpirationDateUTC = paymentTable.ExpirationDate;
                            OrderContext.Order.AsOrder().OrderPayments[i].DateLastModifiedUTC = DateTime.Now;
                            OrderContext.Order.AsOrder().OrderPayments[i].TransactionID = paymentTable.AutorizationNumber;
                            OrderContext.Order.AsOrder().OrderPayments[i].DeferredAmount = (paymentTable.NumberCuota.HasValue) ? paymentTable.NumberCuota : 0;
                            
                            if (OrderContext.Order.AsOrder().OrderPayments[i].PaymentTypeID == 1)
                            {
                                OrderContext.Order.AsOrder().OrderPayments[i].TransactionID = payID;
                                OrderContext.Order.AsOrder().OrderPayments[i].OrderPaymentStatusID = 2;                                
                            }
                        }

                    }
                    else
                    {

                        var paymentTable = ct[0];

                        if (paymentTable.PaymentStatusID == 4)
                        {
                            OrderContext.Order.AsOrder().OrderPayments[0].OrderPaymentStatusID = 2;
                        }
                        else if (paymentTable.PaymentStatusID == 11 || paymentTable.PaymentStatusID == 18)
                        {
                            OrderContext.Order.AsOrder().OrderPayments[0].OrderPaymentStatusID = 1;
                        }

                        OrderContext.Order.AsOrder().OrderPayments[0].ExpirationDateUTC = paymentTable.ExpirationDate;
                        OrderContext.Order.AsOrder().OrderPayments[0].DateLastModifiedUTC = DateTime.Now;
                        OrderContext.Order.AsOrder().OrderPayments[0].TransactionID = paymentTable.AutorizationNumber;
                        OrderContext.Order.AsOrder().OrderPayments[0].DeferredAmount = (paymentTable.NumberCuota.HasValue) ? paymentTable.NumberCuota : 0;


                        OrderContext.Order.AsOrder().OrderPayments[0].TransactionID = payID;
                        OrderContext.Order.AsOrder().OrderPayments[0].OrderPaymentStatusID = 2;
                        amountTotal = "" + OrderContext.Order.AsOrder().OrderPayments[0].Amount;
                    }               
                }
                else
                {
                    //Guarda una traza en PayPal_Log
                    paypalLog.Site = "PWS";
                    paypalLog.OrderID = ppTokens.OrderID.ToInt();
                    paypalLog.AccountId = ppTokens.AccountID.ToInt();
                    paypalLog.PayPal_Response = result;
                    paypalLog.PayPal_Error = "ERROR";
                    paypalLog.PayPal_Process = "ExecutePayment";
                    paypalLog.PayPal_Status = "PAYPAL_ERROR";
                    paypalLog.PayPal_STS_Term = 0;
                    paypalLog.PayPal_AmountPay = ppTokens.Total;

                    InsertPayPal_Log(paypalLog);
                }




            }
            catch (Exception ex)
            {
                message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;

                //Guarda una traza en PayPal_Log
                paypalLog.Site = "PWS";
                paypalLog.OrderID = ppTokens.OrderID.ToInt();
                paypalLog.AccountId = ppTokens.AccountID.ToInt();
                paypalLog.PayPal_Response = result;
                paypalLog.PayPal_Error = message;
                paypalLog.PayPal_Process = "ExecutePayment";
                paypalLog.PayPal_Status = "ERROR";
                paypalLog.PayPal_STS_Term = 0;
                paypalLog.PayPal_AmountPay = ppTokens.Total;

                InsertPayPal_Log(paypalLog);

                return (Json(new { result = result, message = message })).ToString();
            }

            ViewData["amount"] = amountTotal;
            Session["ppTokens"] = ppTokens;

            return result;
        }

        public ActionResult Pay_Approved(string name, string amount, string orderID, string numCuotas)
        {
            PayPal_Tokens ppTokens = (PayPal_Tokens)Session["ppTokens"];

            //Actualiza Term de Transacción Aprobada por PayPal
            PayPalLog paypalLog = new PayPalLog();
            paypalLog.Site = "PWS";
            paypalLog.OrderID = ppTokens.OrderID.ToInt();
            paypalLog.AccountId = ppTokens.AccountID.ToInt();
            paypalLog.PayPal_Response = "";
            paypalLog.PayPal_Error = "";
            paypalLog.PayPal_Process = "ExecutePayment";
            paypalLog.PayPal_Status = "APPROVED";
            paypalLog.PayPal_AmountPay = ppTokens.Total;
            paypalLog.PayPal_STS_Term = 2;

            UpdatePayPal_Log(paypalLog);

            ViewData["name"] = name;
            ViewData["numPedido"] = orderID;
            ViewData["numcuota"] = numCuotas;
            ViewData["valorpedido"] = amount;

            return PartialView();
        }

        public ActionResult validaPaymentGatewayID(string paymentConfigurationID, string paymentGatewayID)
        {
            bool result = false;
            string message = string.Empty;

            try
            {
                var messageRule = ValidateMesageRule();
                if (!messageRule.IsEmpty())
                {

                    return Json(new { result = false, validrule = true, message = messageRule,
                        totals = Totals, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()) });
                }

                //Se agrega esto para validar cuando la Residual es mayor al monto total a pagar -- 10/08/2016 JM
                PaymentsTable objE = new PaymentsTable();
                objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                var ct = Order.GetPaymentsTable(objE);
                var paymentTable = ct[0];

                if (paymentConfigurationID != null)
                {
                    if (paymentConfigurationID.Equals("0"))
                        paymentConfigurationID = paymentTable.PaymentConfigurationID.ToString();
                }
                else
                    paymentConfigurationID = paymentTable.PaymentConfigurationID.ToString();
                //Fin del cambio -- 10/08/2016 JM

                var PayGatewayID = AccountPropertiesBusinessLogic.GetValueByID(10, paymentConfigurationID.ToInt()).PaymentGatewayID;

                if (PayGatewayID == paymentGatewayID.ToInt())
                    result = true;

            }
            catch (Exception ex)
            {
                message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;
                return Json(new { result = false, validrule = true, message = message, totals = Totals, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()) });
            }

            return Json(new { result = result, message = message });
        }
       

        public int InsertPaymentDeclined(PaymentDeclinedModel paymentdeclined)
        {
            var statusID = DataAccess.ExecWithStoreProcedureSave("Core", "InsertPaymentDeclined",
                                                                new SqlParameter("TypeError", SqlDbType.VarChar) { Value = paymentdeclined.TypeError },
                                                                new SqlParameter("OrderID", SqlDbType.Int) { Value = paymentdeclined.OrderID },
                                                                new SqlParameter("PaymentDecMon", SqlDbType.Decimal) { Value = paymentdeclined.PaymentDecMon },
                                                                new SqlParameter("PaymentDecCuo", SqlDbType.Int) { Value = paymentdeclined.PaymentDecCuo },
                                                                new SqlParameter("AccountId", SqlDbType.Int) { Value = paymentdeclined.AccountId },
                                                                new SqlParameter("PaymentGatewayID", SqlDbType.SmallInt) { Value = paymentdeclined.PaymentGatewayID });


            return statusID;
        }

        public int InsertPayPal_Log(PayPalLog paypalLog)
        {
            var statusID = DataAccess.ExecWithStoreProcedureSave("Core", "InsertPayPal_Log",
                                                                new SqlParameter("Site", SqlDbType.VarChar) { Value = paypalLog.Site },
                                                                new SqlParameter("OrderID", SqlDbType.Int) { Value = paypalLog.OrderID },
                                                                new SqlParameter("AccountId", SqlDbType.Int) { Value = paypalLog.AccountId },
                                                                new SqlParameter("PayPal_Response", SqlDbType.NVarChar) { Value = paypalLog.PayPal_Response },
                                                                new SqlParameter("PayPal_Error", SqlDbType.NVarChar) { Value = paypalLog.PayPal_Error },
                                                                new SqlParameter("PayPal_Process", SqlDbType.VarChar) { Value = paypalLog.PayPal_Process },
                                                                new SqlParameter("PayPal_Status", SqlDbType.VarChar) { Value = paypalLog.PayPal_Status },
                                                                new SqlParameter("PayPal_STS_Term", SqlDbType.Int) { Value = paypalLog.PayPal_STS_Term },
                                                                new SqlParameter("PayPal_AmountPay", SqlDbType.NVarChar) { Value = paypalLog.PayPal_AmountPay });
            return statusID;
        }

        public int UpdatePayPal_Log(PayPalLog paypalLog)
        {
            var statusID = DataAccess.ExecWithStoreProcedureSave("Core", "UpdatePayPal_Log",
                                                                new SqlParameter("Site", SqlDbType.VarChar) { Value = paypalLog.Site },
                                                                new SqlParameter("OrderID", SqlDbType.Int) { Value = paypalLog.OrderID },
                                                                new SqlParameter("AccountId", SqlDbType.Int) { Value = paypalLog.AccountId },
                                                                new SqlParameter("PayPal_Process", SqlDbType.VarChar) { Value = paypalLog.PayPal_Process },
                                                                new SqlParameter("PayPal_Status", SqlDbType.VarChar) { Value = paypalLog.PayPal_Status });
            return statusID;
        }

        #region [Validaciones de Datos]
        public ActionResult EmailValidation(string email)
        {
            bool result = false;
            string message = string.Empty;

            try
            {
                Regex RxEmail = new Regex(@"^$|^([\w\-\.]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$");

                if (!RxEmail.IsMatch(email))
                {
                    message = Translation.GetTerm("EmailAccountInvalid", "Provided e-mail is invalid.");
                }
                else
                {
                    result = true;

                }
            }
            catch (Exception ex)
            {
                message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;
            }

            return Json(new { result = result, message = message });
        }


        public ActionResult DocumentValidation(int DocumentType, string DocumentValue)
        {
            bool result = false;


            try
            {
                switch (DocumentType)
                {
                    case 1: //CPF
                        result = swValidarCPF(DocumentValue);
                        break;
                    case 2: //CNPJ
                        result = swValidarCNPJ(DocumentValue);
                        break;
                }
            }
            catch (Exception ex)
            {
                //   message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;
            }

            return Json(new { result = result });
        }

        #region [Document Validations]

        public bool swValidarCPF(string CPFTextoInput)
        {

            CPFTextoInput = (CPFTextoInput == null ? "" : CPFTextoInput);

            Boolean Resulado = true;
            if (CPFTextoInput.Length < 11 || CPFTextoInput.Length < 9)
                Resulado = false;


            //Dictionary<string, string> dcResultado = new Dictionary<string, string>();
            bool dcResultado = false;
            string NuevePrimerosDigitos = CPFTextoInput.Substring(0, 9);

            string PrimerDigito = string.Empty;

            string SegundoDigito = string.Empty;
            if (CPFTextoInput.Length > 9)
            {
                SegundoDigito = CPFTextoInput.Substring(10, 1);
                PrimerDigito = CPFTextoInput.Substring(9, 1);
            }

            int PrimerDigitoValidar = ValidarPrimerDigitoCPF(NuevePrimerosDigitos);
            int SegundoDigitoValidar = ValidarSegundoDigitoCPF(NuevePrimerosDigitos + PrimerDigitoValidar.ToString());

            if (CPFTextoInput.Length > 9)
            {
                Resulado = (Convert.ToByte(PrimerDigito) == PrimerDigitoValidar & Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);

            }

            if (Resulado)
            {
                dcResultado = ValidarCPF(NuevePrimerosDigitos + PrimerDigitoValidar + SegundoDigitoValidar);
            }

            return dcResultado;

        }


        #region validaciones CPF
        static bool ValidarCPF(string TextoInput)
        {
            if (TextoInput.Length < 11 || TextoInput == "")
                return false;

            string NuevePrimerosDigitos = TextoInput.Substring(0, 9);
            string PrimerDigito = TextoInput.Substring(9, 1);
            string SegundoDigito = TextoInput.Substring(10, 1);
            int PrimerDigitoValidar = ValidarPrimerDigitoCPF(NuevePrimerosDigitos);
            int SegundoDigitoValidar = ValidarSegundoDigitoCPF(NuevePrimerosDigitos + PrimerDigitoValidar.ToString());
            return (Convert.ToByte(PrimerDigito) == PrimerDigitoValidar & Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);
        }

        static int ValidarPrimerDigitoCPF(string TextoValidar)
        {
            int[] Multiplicadores = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[9];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }

            total = Resultados.Sum();
            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }

        static int ValidarSegundoDigitoCPF(string TextoValidar)
        {
            int[] Multiplicadores = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[10];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }

            total = Resultados.Sum();
            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }
        #endregion

        public bool swValidarCNPJ(string CNPJTextoInput)
        {

            CNPJTextoInput = (CNPJTextoInput == null ? "" : CNPJTextoInput);

            Boolean Resultado = true;
            if (CNPJTextoInput.Length < 14 || CNPJTextoInput.Length < 12)
                Resultado = false;


            //Dictionary<string, string> dcResultado = new Dictionary<string, string>();
            bool dcResultado = false;
            string DocePrimerosDigitos = CNPJTextoInput.Substring(0, 12);

            string PrimerDigito = string.Empty;

            string SegundoDigito = string.Empty;
            if (CNPJTextoInput.Length > 12)
            {
                SegundoDigito = CNPJTextoInput.Substring(13, 1);
                PrimerDigito = CNPJTextoInput.Substring(12, 1);
            }

            int PrimerDigitoValidar = ValidarPrimerDigitoCNPJ(DocePrimerosDigitos);
            int SegundoDigitoValidar = ValidarSegundoDigitoCNPJ(DocePrimerosDigitos + PrimerDigitoValidar.ToString());

            if (CNPJTextoInput.Length > 12)
            {
                Resultado = (Convert.ToByte(PrimerDigito) == PrimerDigitoValidar & Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);
            }

            if (Resultado)
            {
                dcResultado = ValidarCNPJ(DocePrimerosDigitos + PrimerDigitoValidar + SegundoDigitoValidar);
            }

            return dcResultado;

        }



        #region validaciones CNPJ
        static bool ValidarCNPJ(string TextoInput)
        {
            if (TextoInput.Length < 14 || TextoInput == "")
                return false;

            string DocePrimerosDigitos = TextoInput.Substring(0, 12);
            string PrimerDigito = TextoInput.Substring(12, 1);
            string SegundoDigito = TextoInput.Substring(13, 1);
            int PrimerDigitoValidar = ValidarPrimerDigitoCNPJ(DocePrimerosDigitos);
            int SegundoDigitoValidar = ValidarSegundoDigitoCNPJ(DocePrimerosDigitos + PrimerDigitoValidar.ToString());
            return (Convert.ToByte(PrimerDigito) == PrimerDigitoValidar & Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);
        }

        static int ValidarPrimerDigitoCNPJ(string TextoValidar)
        {

            int[] Multiplicadores = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[12];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }

            total = Resultados.Sum();
            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }

        static int ValidarSegundoDigitoCNPJ(string TextoValidar)
        {
            int[] Multiplicadores = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[13];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }

            total = Resultados.Sum();
            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }
        #endregion

        #endregion
        #endregion

        #region ManejoErrores

        public ActionResult PayPal_Error(string state, string message, string cause, string paymentConfigID)
        {
            if (state == null)
                state = "ERROR_SUBMIT_ORDER";

            if (state.Equals(""))
                state = "ERROR_SUBMT_ORDER";

            if (message == null) message = "Error en proceso de guardar la Orden";
            if (cause == null) cause = "";
            if (paymentConfigID == null) paymentConfigID = "";

            state = state.ToUpper();

            if (!state.Equals("ONERROR")) cause = "";


            var paypalError = DataAccess.ExecWithStoreProcedureListParam<PayPalMessageError>("Core", "uspGetMessagePayPalError",
                                                                                         new SqlParameter("PP_Cause", SqlDbType.VarChar) { Value = state },
                                                                                         new SqlParameter("PP_Action", SqlDbType.VarChar) { Value = cause }
                                                                                         ).ToList().First();
            try
            {

                /*if (state.Equals("INSTRUMENT_DECLINED") ||
                    state.Equals("ONERROR") ||
                    state.Equals("ERROR_SERVER_NOT_FOUND") ||
                    state.Equals("INVALID_OR_EXPIRED_TOKEN") ||
                    state.Equals("INTERNAL_ERROR") ||
                    state.Equals("INVALID_DATA_FIRST_NAME") ||
                    state.Equals("INVALID_DATA_LAST_NAME") ||
                    state.Equals("INVALID PAYERTAXID") ||
                    state.Equals("INVALID PAYERFIRSTNAME") ||
                    state.Equals("INVALID PAYERLASTNAME") ||
                    state.Equals("INVALID PAYEREMAIL"))
                {*/

                    PaymentDeclinedModel paymentDecline = new PaymentDeclinedModel();

                    var paymentConfigurationID = paymentConfigID.ToInt();
                    var paymentGateway = AccountPropertiesBusinessLogic.GetValueByID(10, paymentConfigurationID).PaymentGatewayID;

                    paymentDecline.TypeError = "{Type:" + state + ", Message: [" + message + "]-[" + paypalError.PP_Message + "], Cause:" + cause + "}";
                    paymentDecline.PaymentGatewayID = paymentGateway;

                    PaymentsTable objE = new PaymentsTable();
                    objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                    var ct = Order.GetPaymentsTable(objE);

                    if (OrderContext.Order.AsOrder().OrderPayments.Count > 1)
                    {
                        for (int i = 0; i < OrderContext.Order.AsOrder().OrderPayments.Count; i++)
                        {
                            var paymentTable = ct[i];

                            if (OrderContext.Order.AsOrder().OrderPayments[i].PaymentTypeID == 1)
                            {
                                paymentDecline.PaymentDecMon = paymentTable.AppliedAmount.ToString();
                                paymentDecline.PaymentDecCuo = paymentTable.NumberCuota.ToInt();

                            }
                        }
                    }
                    else
                    {
                        var paymentTable = ct[0];
                        paymentDecline.PaymentDecMon = paymentTable.AppliedAmount.ToString();
                        paymentDecline.PaymentDecCuo = paymentTable.NumberCuota.ToInt();
                    }

                    paymentDecline.OrderID = OrderContext.Order.AsOrder().OrderNumber!=null?OrderContext.Order.AsOrder().OrderNumber.ToInt():0;
                    var result = InsertPaymentDeclined(paymentDecline);

                //}
            }
            catch (Exception ex)
            {
                message = paypalError.PP_Message + "( " + ex.Message + " )";
            }


            ViewData["state"] = state;

            /*if(!paypalError.PP_Message.Equals(message))
                ViewData["message"] = paypalError.PP_Message+"["+message+"]";
            else
                ViewData["message"] = paypalError.PP_Message;*/

            if (paypalError.PP_Message.Equals("") && !message.Equals(""))
                ViewData["message"] = "[" + message + "]";
            else
                ViewData["message"] = paypalError.PP_Message;


            return PartialView();
        }

        //Metodo para controlar el error que envia en IFrame de PayPal -- CSTI Juan Morales 17-07-2016
        public void SaveErroPayPalIFrame(string msgPayPal)
        {
            var response = "{ result = ERROR_IFRAME_PAYPAL, message = " + msgPayPal + "}";

            PayPalLog paypalLog = new PayPalLog();

            paypalLog.Site = "PWS";
            paypalLog.PayPal_Response = response;
            paypalLog.PayPal_Process = "PayPal_IFrame_Validator";
            paypalLog.PayPal_Status = "ERROR_PAYPAL_IFRAME";
            paypalLog.PayPal_Error = "ERROR_PAYPAL_IFRAME";
            paypalLog.PayPal_STS_Term = 0;

            PayPal_Tokens ppTokens = (PayPal_Tokens)Session["ppTokens"];

            paypalLog.OrderID = ppTokens.OrderID.ToInt();
            paypalLog.AccountId = ppTokens.AccountID.ToInt();
            paypalLog.PayPal_AmountPay = ppTokens.Total;

            InsertPayPal_Log(paypalLog);
        }

        //Metodo para controlar el boton cancelar de ambos formularios --- CSTI Juan Morales 17-07-2016
        public void CancelPay(string botonCancel)
        {

            var response = "{ result = Press_Cancel_" + botonCancel + ", message = User press cancel buttom in GUI " +
                           (botonCancel.Equals("1") ? "Pre-Payment" : "PayPal IFrame") + " }";

            PayPalLog paypalLog = new PayPalLog();

            paypalLog.Site = "PWS";
            paypalLog.PayPal_Response = response;
            paypalLog.PayPal_Process = "PressButtomCancel_" + botonCancel;
            paypalLog.PayPal_Status = "CANCEL_BUTTOM_" + botonCancel;
            paypalLog.PayPal_Error = "";
            paypalLog.PayPal_STS_Term = 0;

            var amount = "";

            #region Carga Datos Pago
            PaymentsTable objE = new PaymentsTable();
            objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
            var ct = Order.GetPaymentsTable(objE);

            if (OrderContext.Order.AsOrder().OrderPayments.Count > 1)
            {
                for (int i = 0; i < OrderContext.Order.AsOrder().OrderPayments.Count; i++)
                {
                    var paymentTable = ct[i];

                    if (OrderContext.Order.AsOrder().OrderPayments[i].PaymentTypeID == 1)
                    {
                        amount = paymentTable.AppliedAmount.ToString();
                    }
                }
            }
            else
            {
                var paymentTable = ct[0];
                amount = paymentTable.AppliedAmount.ToString();
            }
            #endregion

            if (botonCancel.Equals("1"))
            {
                var orderID = OrderContext.Order.AsOrder().OrderNumber;
                var accountID = OrderContext.Order.AsOrder().OrderCustomers[0].AccountID;

                paypalLog.OrderID = orderID.ToInt();
                paypalLog.AccountId = accountID;
                paypalLog.PayPal_AmountPay = amount.Replace(",", ".");
            }
            else
            {
                PayPal_Tokens ppTokens = (PayPal_Tokens)Session["ppTokens"];

                paypalLog.OrderID = ppTokens.OrderID.ToInt();
                paypalLog.AccountId = ppTokens.AccountID.ToInt();
                paypalLog.PayPal_AmountPay = ppTokens.Total;
            }

            InsertPayPal_Log(paypalLog);
        }

        #endregion

        private string ValidateMesageRule()
        {
            var messageRule = string.Empty;

            if (Session["PreOrder"] == null) Session["PreOrder"] = Order.getPreOrder(OrderContext.Order.AsOrder().ConsultantID, 4, OrderContext.Order.AsOrder().OrderID);

            PaymentsTable objE = new PaymentsTable();
            objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
            var ct = Order.GetPaymentsTable(objE);
            decimal Amount = Order.GetProductCreditByAccount(OrderContext.Order.AsOrder().ConsultantID);

            int numOrdePay = 0;
            if (OrderContext.Order.AsOrder().OrderPayments != null) numOrdePay = OrderContext.Order.AsOrder().OrderPayments.Count();
            decimal balancedue = getValor("balanceDue");
            decimal grandtotal = getValor("grandTotal");

            bool istotal = true;

            if (Amount > 0 && balancedue >= 0 && Amount >= grandtotal && (ct.Count() != 1 || numOrdePay != 1))
            {
                ApplyPaymentPreviosBalance();
                OrderService.UpdateOrder(OrderContext);
                Session["Order"] = OrderContext.Order.AsOrder();
                istotal = false;
                messageRule = Translation.GetTerm("NoPaymentSelect02", "Efectuar nuevamente el pago.");
            }

            if (istotal && (ct.Count() == 0) || (numOrdePay == 0) || (Amount != 0 && balancedue <= 0 && (grandtotal != Amount) && (ct.Count() != 2 || numOrdePay != 2)))
            {
                ApplyPaymentPreviosBalance();
                OrderService.UpdateOrder(OrderContext);
                Session["Order"] = OrderContext.Order.AsOrder();
                //return Json(new { result = false, validrule = true, message = Translation.GetTerm("NoShippingRatesAvailable", "No shipping rates available. Please enter a shipping address if one is not yet added to order or adjust existing address or order contents.") });
                messageRule = Translation.GetTerm("NoPaymentSelect", "Seleccione nuevamente el metodo de pago.");
            }

            // csti(mescoobar)-EB-486-Inicio
            OrderCustomer orderCustomer = OrderContext.Order.AsOrder().OrderCustomers[0];
            var addedItemOperationID = (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem;
            var nonPromotionalItems = orderCustomer.ParentOrderItems.Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == addedItemOperationID));

            /*Reglas de Salida*/
            decimal qvRule = 0;
            decimal retailRule = 0;
            decimal subtotalRule = (decimal)OrderContext.Order.Subtotal;
            /*Reglas de Entrada*/
            List<int> productsRule = new List<int>();
            List<int> productTypesRule = new List<int>();
            int constQV = (int)NetSteps.Data.Entities.Constants.ProductPriceType.QV;
            int constRetail = (int)NetSteps.Data.Entities.Constants.ProductPriceType.Retail;
            foreach (OrderItem orderItem in nonPromotionalItems)
            {
                var productInfo = Product.Load((int)orderItem.ProductID);
                productsRule.Add(productInfo.ProductID);
                productTypesRule.Add(ProductBase.Load(productInfo.ProductBaseID).ProductTypeID);
                qvRule += (orderItem.Quantity * (decimal)orderItem.OrderItemPrices
                                .Where(x => x.ProductPriceTypeID == constQV)
                                    .Select(y => y.OriginalUnitPrice).FirstOrDefault());
                retailRule += (orderItem.Quantity * (decimal)orderItem.OrderItemPrices
                                .Where(x => x.ProductPriceTypeID == constRetail)
                                    .Select(y => y.OriginalUnitPrice).FirstOrDefault());
            }
            int storeFrontRule = ApplicationContext.Instance.StoreFrontID;
            short orderTypeRule = OrderContext.Order.OrderTypeID;
            int accountRule = orderCustomer.AccountID;
            short accountTypeRule = orderCustomer.AccountTypeID;
            var ruleBasicFilter = Create.New<IOrderRulesService>().GetRules().Where(x => x.RuleStatus == (int)RuleStatus.Active &&
                                                                 (x.StartDate.IsNullOrEmpty() ? true : x.StartDate <= DateTime.Now ? true : false) &&
                                                                 (x.EndDate.IsNullOrEmpty() ? true : x.EndDate >= DateTime.Now ? true : false)).ToList();

            List<RulesDTO> dtoRuleComparer = new List<RulesDTO>();
            var ordeRuleConverter = Create.New<OrderRuleConverter<Rules, RulesDTO>>();
            foreach (var item in ruleBasicFilter)
            {
                dtoRuleComparer.Add(ordeRuleConverter.Convert(item));
            }

            /*Filtrar Reglas a las que aplica la order*/
            var appliedRules = dtoRuleComparer.Where(x => (x.RuleValidationsDTO.Where(y => (y.AccountIDs.Count == 0 ? true : y.AccountIDs.Contains(accountRule) ? true : false)
                                                                                && (y.AccountTypeIDs.Count == 0 ? true : y.AccountTypeIDs.Contains(accountTypeRule) ? true : false)
                                                                                && (y.OrderTypeIDs.Count == 0 ? true : y.OrderTypeIDs.Contains(orderTypeRule) ? true : false)
                                                                                && (y.StoreFrontIDs.Count == 0 ? true : y.StoreFrontIDs.Contains(storeFrontRule) ? true : false)
                                                                                && (y.ProductIDs.Count == 0 ? true : productsRule.Distinct().Intersect(y.ProductIDs).Any() ? true : false)
                                                                                && (y.ProductTypeIDs.Count == 0 ? true : productTypesRule.Distinct().Intersect(y.ProductTypeIDs).Any() ? true : false)).Any())).ToList();

            /*Filtrar Reglas no cumplidas*/
            var unfulfilledRules = appliedRules.Where(x => (x.RuleValidationsDTO.Where(y => (y.CustomerPriceSubTotalDTO.Count == 0 ? false : y.CustomerPriceSubTotalDTO.FirstOrDefault().MinimumAmount > subtotalRule ? true : false)
                                                                                        || (y.CustomerPriceTotalDTO.Count == 0 ? false : (y.CustomerPriceTotalDTO
                                                                                        .Where(z => z.ProductPriceTypeID == constQV && z.MinimumAmount >= qvRule).Any() || y.CustomerPriceTotalDTO
                                                                                        .Where(z => z.ProductPriceTypeID == constRetail && z.MinimumAmount >= retailRule).Any()))).Any())).ToList();

            /*Concatenar mensajes*/
         
            foreach (var faildRule in unfulfilledRules)
            {
                TermTranslation translation = TermTranslation.Repository.FirstOrDefault(tt => tt.TermName == faildRule.TermName && tt.LanguageID == CoreContext.CurrentLanguageID);
                if (translation == default(TermTranslation))
                {
                    translation = TermTranslation.Repository.FirstOrDefault(tt => tt.TermName == faildRule.TermName);
                    messageRule += translation.Term;
                }
                else
                {
                    messageRule += translation.Term;
                }
            }


            if (Convert.ToBoolean(Session["ExisShippingMetods"]) == false)
            {
                //return Json(new { result = false, validrule = true, message = Translation.GetTerm("NoShippingRatesAvailable", "No shipping rates available. Please enter a shipping address if one is not yet added to order or adjust existing address or order contents.") });
                messageRule += Translation.GetTerm("NoShippingRatesAvailable", "No shipping rates available. Please enter a shipping address if one is not yet added to order or adjust existing address or order contents.");
            }

            // csti(mescoobar)-EB-486-Fin

            OrderContext.Order.AsOrder().StartEntityTracking();
            // Validate that a shipping address has been selected and contains enough information to ship the order
            if (!OrderContext.Order.AsOrder().OrderShipments.Any())
            {
                //throw new Exception(Translation.GetTerm("NoShippingAddress", "There is no shipping address.  Please choose an address to ship to."));
                messageRule += Translation.GetTerm("NoShippingAddress", "There is no shipping address.  Please choose an address to ship to.");
            }
            foreach (OrderShipment shipment in OrderContext.Order.AsOrder().OrderShipments)
            {
                if (string.IsNullOrEmpty(shipment.Address1) ||
                    string.IsNullOrEmpty(shipment.City) ||
                    string.IsNullOrEmpty(shipment.State) ||
                    string.IsNullOrEmpty(shipment.PostalCode))
                {
                    //throw new Exception(Translation.GetTerm("InvalidShippingAddress", "The shipping address is invalid."));
                    messageRule += Translation.GetTerm("InvalidShippingAddress", "The shipping address is invalid.");
                }
            }

            return messageRule;
            ////csti(mescoobar)-EB-486-Fin

        }

        #region ValidateAmounts
        //Todos estos metodos (CalculateQualificationTotal,Totals,RemovePaymentsTable,ApplyPaymentPreviosBalance)  son copia de los que ya existen  en ProductsCotnroller.

        private void CalculateQualificationTotal()
        {
            decimal sum = 0;
            foreach (var item in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
            {
                foreach (var price in item.OrderItemPrices)
                {
                    if (price.ProductPriceTypeID == 21) sum += price.UnitPrice * item.Quantity;
                }
            }
            OrderContext.Order.AsOrder().QualificationTotal = sum;
        }
        protected virtual object Totals
        {
            get
            {
                Order order = OrderContext.Order.AsOrder();
                CalculateQualificationTotal();
                if (order == null)
                    return null;
                decimal paymentTotal = order.OrderCustomers[0].OrderPayments.Where(p => p.OrderPaymentStatusID != NetSteps.Data.Entities.Constants.OrderPaymentStatus.Cancelled.ToShort()).Sum(p => p.Amount);
                decimal shippin = 0;
                shippin = (order.ShippingTotalOverride.HasValue) ? order.HandlingTotal.ToDecimal() : 0;

                decimal totalAPagar = (paymentTotal + shippin);
                decimal balance = order.GrandTotal.GetRoundedNumber() - totalAPagar;
                if (paymentTotal > order.GrandTotal.GetRoundedNumber())
                {
                    if (balance < 0)
                        balance = balance * (-1);
                }
                else
                {
                    if (balance > 0)
                        balance = balance * (-1);
                }

                return new
                {
                    subtotalAdjusted = order.OrderCustomers[0].AdjustedSubTotal.ToString(order.CurrencyID),
                    subtotal = order.Subtotal.ToDecimal().GetRoundedNumber().ToString(order.CurrencyID),
                    commissionableTotal = order.CommissionableTotal.ToDecimal().ToString(order.CurrencyID),
                    qualificationTotal = order.QualificationTotal.ToDecimal().ToString(),
                    taxTotal = (order.TaxAmountTotalOverride != null) ? order.TaxAmountTotalOverride.ToString(order.CurrencyID) : order.TaxAmountTotal.ToString(order.CurrencyID),
                    shippingTotal = (order.ShippingTotalOverride != null) ? order.ShippingTotalOverride.ToString(order.CurrencyID) : order.ShippingTotal.ToString(order.CurrencyID),
                    handlingTotal = order.HandlingTotal.ToString(order.CurrencyID),
                    grandTotal = order.GrandTotal.ToDecimal(),
                    //grandTotal = order.GrandTotal.ToDecimal().ToString(order.CurrencyID),
                    paymentTotal = paymentTotal.ToString(order.CurrencyID),
                    balanceDue = balance,
                    //balanceDue = balance.ToString(order.CurrencyID),
                    balanceAmount = balance,
                    numberOfItems = (order.OrderCustomers.Sum(oc => oc.ParentOrderItems.Sum(oi => oi.Quantity)))
                };
            }
        }
        public void RemovePaymentsTable(int Indice)
        {
            PaymentsTable objE = new PaymentsTable();
            objE.ubic = 0;
            objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
            objE.OrderPaymentId = Indice;
            PaymentsMethodsExtensions.UpdPaymentsTable(objE);
        }
        public virtual ActionResult ApplyPaymentPreviosBalance()
        {
            try
            {
                RemovePaymentsTable(0);
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                decimal Amount = Order.GetProductCreditByAccount(OrderContext.Order.AsOrder().ConsultantID);
                ViewBag.Balance = Amount.ToString(OrderContext.Order.CurrencyID);

                int exitPB = 0;
                exitPB = OrderContext.Order.AsOrder().OrderPayments.Where(x => x.BankName == "Product Credit").ToList().Count();

                if (Amount == 0 || exitPB > 0)
                {
                    return Json(new { result = false, message = "" });
                }
                else
                {
                    PaymentTypeModel paymentTypeModel = new PaymentTypeModel();
                    paymentTypeModel.PaymentMethodID = 60;
                    paymentTypeModel.NameOnCard = "Product Credit";
                    paymentTypeModel.Amount = Amount;

                    IPayment payment = new Payment()
                    {
                        DecryptedAccountNumber = paymentTypeModel.AccountNumber,
                        CVV = paymentTypeModel.Cvv,
                        PaymentType = ConstantsGenerated.PaymentType.EFT,
                        NameOnCard = paymentTypeModel.AccountNumber,
                        BankName = paymentTypeModel.NameOnCard
                    };

                    payment.PaymentTypeID = Order.GetApplyPaymentType(paymentTypeModel.PaymentMethodID.Value);

                    BasicResponseItem<OrderPayment> response = OrderContext.Order.AsOrder().ApplyPaymentToCustomerPreviosBalance(paymentTypeModel.PaymentType, paymentTypeModel.Amount, payment, user: CoreContext.CurrentUser);

                    if (response.Success)
                    {
                        OrderService.UpdateOrder(OrderContext);
                    }

                    ApplyPaymentSearchData objE = new ApplyPaymentSearchData();
                    objE.OrderPaymentId = response.Item.OrderPaymentID;
                    objE.Amount = paymentTypeModel.Amount;
                    objE.PaymentConfigurationID = paymentTypeModel.PaymentMethodID;
                    objE.NameOnCard = "";
                    objE.PaymentConfigurationID = paymentTypeModel.PaymentMethodID;

                    if (payment.PaymentTypeID > 1)
                    {
                        objE.NumberCuota = null;
                    }
                    else
                    {
                        if (paymentTypeModel.NumberCuota != null)
                            objE.NumberCuota = paymentTypeModel.NumberCuota.Value;

                    }
                    NetSteps.Data.Entities.Business.CTE.paymentTables = new List<PaymentsTable>();
                    objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                    NetSteps.Data.Entities.Business.CTE.ApplyPayment(objE);

                    //Order.GetApplyPayment(objE);

                    return Json(new { result = false, message = "" });
                }

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        /// <summary>
        /// getValor : retorna un valor del Totals de un campo especifico.
        /// </summary>
        /// <param name="campo">nomble del valor del campo a retornar</param>
        /// <returns>decimal</returns>
        private decimal getValor(string campo)
        {
            var getBalanceDue = Totals.GetType().GetProperty(campo).GetValue(Totals, null);
            //string[] strBalanceDue = Convert.ToString(getBalanceDue).Split('$');
            decimal balancDue = 0;
            balancDue = Convert.ToDecimal(getBalanceDue, CoreContext.CurrentCultureInfo);
            //balancDue = NetSteps.Data.Entities.Extensions.DecimalExtensions.FormatGlobalizationDecimal(Convert.ToDecimal(strBalanceDue[1].ToString()));
            return balancDue;
        }
        #endregion


    }
}
