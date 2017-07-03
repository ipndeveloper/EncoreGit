using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Context;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
//Modificaciones:
//@1 20151607 BR-CC-012 GYS MD: Se implemento el metodo ApplyReceivedBankPayment
//@2 20151607 BR-CC-012 GYS MD: Se implemento el metodo updateForReceivedBankPayment
//@3 20151607 BR-CC-012 GYS MD: Se implemento el metodo OrderStatusUpdate
//@4 20151607 BR-CC-012 GYS MD: Se Movio el metodo ApplyCredit de OrderEntryController
//@05 20153108 BR-CC-014 GYS EFP: Creación de método que llama al que se conecta con la BD (SaveStructuredRule)
namespace NetSteps.Data.Entities.Business
{
    public class CTE
    {

        #region EFP - Singleton

        private static CTE _instance;

        public static CTE Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CTE();
                return _instance;
            }
        }


        #endregion

        //@01 A01 BAL

        /// <summary>
        /// Clase para inicializar el objeto PaymentsTable
        /// </summary>
        public static  List<PaymentsTable> paymentTables = null;
        public static CreditPaymentTable creditPayment = null;
        /// <summary>
        /// Contructor que inicializa la clase
        /// </summary>
        public CTE()
        {
            paymentTables = new List<PaymentsTable>();
            creditPayment = new CreditPaymentTable();
        }

       
        public static IOrderRepository OrderRepository { get { return Create.New<IOrderRepository>(); } }

        #region BrowseRules KLC-CSTI
        public static List<CTERulesSearchData> BrowseRules()
        {
            return CTERepository.BrowseRules();
        }

        public static PaginatedList<CTERulesSearchData> BrowseRules(CTERulesParameters searchParams)
        {
            try
            {
                return CTERepository.BrowseRules(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion
        #region BrowseRulesNegotiation KLC-CSTI
        public static List<CTERulesNegotiationData> BrowseRulesNegotiation()
        {
            return CTERepository.BrowseRulesNegotiation();
        }

        public static PaginatedList<CTERulesNegotiationData> BrowseRulesNegotiation(CTERulesParameters searchParams)
        {
            try
            {
                return CTERepository.BrowseRulesNegotiation(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion
        /// <summary>
        /// @1 Proceso ApplyReceivedBankPayment
        /// </summary>
        public static void ApplyReceivedBankPayment()
        {
            /* 
             * El proceso toma como ingreso la data de tabla BankPayments.
            En caso tal de no encontrar un registro en la tabla OrderPayment 
            se deberá crear un registro log del error en la tabla LogErrorsBankPayments.
             
            Si satisface los requisitos normales este proceso llama al proceso OrderStatusUpdate 
            para actualizar el estado de la orden.
             
            Si se determina que no cumple los requisitos y existe una diferencia considerable 
            entonces se invoca el proceso ApplyCredit
            
            Al final del proceso se invoca el método CREATE LOG 
            
             */
            bool canContinue = true;

            try
            {
                var vlBankPaymentsRepository = new BankPaymentsRepository();
                var vlOrderPaymentRepository = new OrderPaymentsRepository();
                // Disabled until we have DTC enabled - MD
                //using (TransactionScope scope = new TransactionScope())

                var listBankPayments = vlBankPaymentsRepository.BrowseBankPayments().Where(bankPayment => bankPayment.Applied != 1 &&
                                                                                                    bankPayment.BankPaymentType == 1).ToList();

                foreach (var bankPayment in listBankPayments)
                {
                    if (canContinue)
                    {
                        var orderPayments = vlOrderPaymentRepository.BrowseOrderPaymentsByTicketNumber(bankPayment.TicketNumber);

                        if (orderPayments.Any())
                        {
                            foreach (var orderPayment in orderPayments)
                            {
                                if (canContinue)
                                {
                                    if (bankPayment.Amount == orderPayment.TotalAmount)
                                    {
                                        canContinue = updateForReceivedBankPayment(orderPayment, bankPayment);
                                    }
                                    else
                                    {
                                        var Diff = orderPayment.TotalAmount - bankPayment.Amount;

                                        if (Diff < 0)
                                        {
                                            var Difference = Diff * -1;
                                            var vlPaymentConfigurationsRepository =
                                                new PaymentConfigurationsRepository();
                                            var vlPaymentConfiguration =
                                                vlPaymentConfigurationsRepository
                                                    .BrowsePaymentConfigurationByPaymentConfigurationID(
                                                        orderPayment.PaymentConfigurationID);

                                            double? vlTolerancePercentage = null;
                                            int? vlToleranceValue = null;


                                            if (vlPaymentConfiguration.ToleranceValue == null)
                                            {
                                                vlTolerancePercentage = vlPaymentConfiguration.TolerancePercentage;
                                                vlToleranceValue = 0;
                                            }
                                            if (vlPaymentConfiguration.TolerancePercentage == null)
                                            {
                                                vlTolerancePercentage = 0;
                                                vlToleranceValue = vlPaymentConfiguration.ToleranceValue;
                                            }

                                            if (vlTolerancePercentage == 0)
                                            {
                                                if (Difference <= vlToleranceValue)
                                                {
                                                    canContinue = updateForReceivedBankPayment(orderPayment, bankPayment);
                                                }
                                                else
                                                {
                                                    ApplyCredit(bankPayment.AccountCode, 9, 5, 12,
                                                        Convert.ToDecimal(Diff),
                                                        orderPayment.OrderID, orderPayment.OrderPaymentID);
                                                }
                                            }
                                            else if (vlToleranceValue == 0)
                                            {
                                                var TValue = orderPayment.TotalAmount *
                                                                Convert.ToDecimal(vlTolerancePercentage);

                                                if (Difference <= TValue)
                                                {
                                                    canContinue = updateForReceivedBankPayment(orderPayment, bankPayment);
                                                }
                                                else
                                                {
                                                    ApplyCredit(bankPayment.AccountCode, 9, 5, 12,
                                                        Convert.ToDecimal(Diff),
                                                        orderPayment.OrderID, orderPayment.OrderPaymentID);
                                                }
                                            }

                                        }
                                        else
                                        {
                                            canContinue = updateForReceivedBankPayment(orderPayment, bankPayment);
                                            if (canContinue)
                                                ApplyCredit(bankPayment.AccountCode, 9, 5, 12, Convert.ToDecimal(Diff),
                                                                orderPayment.OrderID, orderPayment.OrderPaymentID);
                                        }
                                    }

                                    if (canContinue)
                                    {
                                        OrderPaymentsLog vlOrderPaymentsLog = new OrderPaymentsLog()
                                        {
                                            OrderPaymentID = orderPayment.OrderPaymentID,
                                            InitialAmount = Convert.ToDecimal(orderPayment.InitialAmount),
                                            TotalAmount = Convert.ToDecimal(orderPayment.TotalAmount),
                                            ExpirationDateUTC = Convert.ToDateTime(orderPayment.ExpirationDateUTC),
                                            FineAmount = 0,
                                            ReasonID = 2,
                                            InterestAmount = 0,
                                            DateModifiedUTC = DateTime.Now,
                                            ModifiedByUserID = ApplicationContext.Instance.CurrentUserID,
                                        };
                                        OrderPaymentsLogExtensions.CreateLog(vlOrderPaymentsLog);
                                    }
                                } // Validator
                            }
                        }
                        else
                        {
                            if (canContinue)
                            {
                                var vlLogErrorLogErrorBankPaymentsRepository = new LogErrorBankPaymentsRepository();
                                vlLogErrorLogErrorBankPaymentsRepository.InserLogErrorBankPayments(
                                    new LogErrorBankPayments()
                                    {
                                        BankPaymentID = bankPayment.BankPaymentID,
                                        BankName = bankPayment.BankName,
                                        TicketNumber = bankPayment.TicketNumber,
                                        OrderNumber = bankPayment.OrderNumber,
                                        Date = DateTime.Now
                                    });
                            }
                        }
                    } // Validator
                }
                // Disabled until we have DTC enabled - MD
                //scope.Complete();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// @2 Actualiza BankPayment y OrderPayment 
        /// orderPayment.BankName = bankPayment.BankName
        /// orderPayment.ProcessOnDateUTC  = bankPayment.DateReceivedBank 
        /// orderPayment.ProcessedOnDateUTC  = bankPayment.DateApplied 
        /// orderPayment.Accepted  = true
        /// bankPayment .Applied   = true 
        /// </summary>
        /// <param name="orderPayment"></param>
        /// <param name="bankPayment"></param>
        private static bool updateForReceivedBankPayment(OrderPayments orderPayment, BankPayments bankPayment)
        {
            bool rpta = false;

            CTEParameters Parameter = new CTEParameters();
            Parameter.BankPaymentID = bankPayment.BankPaymentID;
            Parameter.OrderPaymentID = orderPayment.OrderPaymentID;
            Parameter.BankName = bankPayment.BankName;
            Parameter.ProcessOnDateUTC = bankPayment.DateReceivedBank;
            Parameter.ProcessedDateUTC = bankPayment.DateApplied;
            Parameter.Accepted = true;
            Parameter.Applied = true;

            rpta = new CTERepository().updateForReceivedBankPayment(Parameter);
            if (rpta)
                rpta = OrderStatusUpdate(orderPayment.OrderID);

            return rpta;
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// BR-CC-008 - Gestionar Cartera Pedidos - Cta Cte 
        /// </summary>
        /// <param name="param"></param>
        /// <returns>OrderStatusID</returns>
        public static int ApplyPayment(ApplyPaymentSearchData param)
        {
            try
            {
                List<ApplyPaymentSearchData.paymentYpe> PaymentTypes = PaymentsMethodsExtensions.ApplyPayment(param);
                
                //param.PaymentConfigurationID
                bool IsCreditCard = false;
                bool IsTicket = false;
                int orderStatusID = 0;

                PaymentConfigurationsRepository b = new PaymentConfigurationsRepository();
                var paymentConfiguration = b.BrowsePaymentConfigurationByPaymentConfigurationID(param.PaymentConfigurationID);
                
                foreach (var item in PaymentTypes)
                {
                    IsCreditCard = item.IsCreditCard;
                    IsTicket = item.IsTicket;
                }


                if (IsCreditCard)
                {
                    // Invocamos al Proceso ValidateCreditCardPayment
                    bool prosVal = ValidateCreditCardPayment(param);
                    if (prosVal == true)
                    {
                        orderStatusID = 4; //(Paid);
                    }
                    else
                    {
                        orderStatusID = 11; //(Credit Card Declined);
                    }
                }
                else
                {
                    orderStatusID = 18; //(Pending per paid confirmation);
                }


                int PaymentTypeID = Order.GetApplyPaymentType(int.Parse(param.PaymentConfigurationID.ToString())); 

                PaymentsTable objE = new PaymentsTable();
                objE.PaymentConfigurationID = param.PaymentConfigurationID;
                objE.PaymentStatusID = orderStatusID;
                objE.AppliedAmount = param.Amount;
                objE.ExpirationDate = ValidateFinalDate(DateTime.Now, paymentConfiguration.DaysForPayment);
                objE.NumberCuota = param.NumberCuota;
                objE.AutorizationNumber = param.AutorizationNumber;
                objE.OrderPaymentId = param.OrderPaymentId;
                objE.PaymentType = PaymentTypeID;
                objE.ubic = 1;
                objE.PreOrderID = param.PreOrderID;
                //paymentTables.Add(objE);
                PaymentsMethodsExtensions.UpdPaymentsTable(objE);
                return orderStatusID;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
      
             
        /// <summary>
        /// Developed By GASS - CSTI
        /// BR-CC-013 - Se Implementa metodo ApplyManualPayment  
        /// </summary>
        /// <param name="TicketNumber">Numero de Ticket</param>
        /// /// <param name="Amount">Monto, pero no se usa</param>
        /// <returns>affectedRows, cantidad de registros afectados</returns>
        public static void ApplyManualPayment(Int32 TicketNumber, Int32 BankID, Int32 UserID, DateTime ProcessOnDateUTC, string Tipollamado, int BankPaymentID)
        {
            try
            {
                CTERepository.ApplyManualPayment(TicketNumber, BankID, UserID, ProcessOnDateUTC, Tipollamado, BankPaymentID);
                
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static bool ValidateCreditCardPayment(ApplyPaymentSearchData param)
        {
            try
            {
                bool orderStatusID = true;

                return orderStatusID;
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        //@4
        public static void ApplyCredit(int AccountID, int EntryReasonID, int EntryOriginID, int EntryTypeID,
                decimal EntryAmount, int OrderID, int? OrderPaymentID)
        {
            /* 
                se invoca el proceso ApplyCredit que está descrito en el caso de uso: 
             * “BR-CT-007 - Gerenciar Cuenta Corriente”. 
             */
            ProductCreditLedgerParameters param = new ProductCreditLedgerParameters();
            param.AccountID = AccountID;
            param.EntryReasonID = EntryReasonID;
            param.EntryOriginID = EntryOriginID;
            param.EntryTypeID = EntryTypeID;
            param.UserID = ApplicationContext.Instance.CurrentUserID;
            param.EntryAmount = EntryAmount;
            param.CurrencyTypeID = OrderRepository.LoadByID(OrderID).CurrencyID;
            param.OrderID = OrderID;
            param.OrderPaymentID = OrderPaymentID;
            ProductCreditLedgerExtension.CreateProductCreditLedger(param);

            // METODO POR VALIDAR
        }

        public static void ProcedimientoCuentaCorriente(int OrderID, string TipoMovimiento, decimal MontoParcial, int UserID)
        {
            ProductCreditLedgerExtension.ProcedimientoCuentaCorriente(OrderID, TipoMovimiento, MontoParcial, UserID);
        }
        /// <summary>
        /// @3 OrderStatusUpdate El proceso actualiza el estatus de una orden a paid,
        /// si se encuentra que todos sus títulos ya han sido pagados.
        /// </summary>
        /// <param name="OrderID"></param>
        public static bool OrderStatusUpdate(int OrderID)
        {
            /* 
             * con OrderNumber se obtiene el @OrderID de tabla Ordes.
             Si y solo si, todos los registros de OrderPayments en los cuales el campo OrderID es igual a @OrderID 
             * y el campo OrderPaymentStatusID es igual a 2, entonces, en la tabla Orders, en el registro cuyo OrderID
             * es igual a @OrderID se cambiará el valor de su campo OrderStatusID por 4.
             * */
            try
            {
                return new CTERepository().OrderStatusUpdate(OrderID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static DateTime ValidateFinalDate(DateTime Date, int Days)
        {

            return new CTERepository().ValidateFinalDate(Date, Days);

        }

        #region Modification @05

        public bool SaveStructuredRule(string name, List<CTERulesNegotiationData> details, int FineAndInterestRulesID)
        {
            bool rpta = false;

            //if (details.Count() > 0)
                rpta = new CTERepository().SaveStructuredRule(name, details, FineAndInterestRulesID);

            return rpta;
        }

        #endregion


        public bool SaveAccountCredit(List<int> details,int secc, int UserID)
        {
            bool rpta = false;

            //if (details.Count() > 0)
            rpta = new CTERepository().SaveAccountCredit( details, secc,UserID);

            return rpta;
        }
        public  int UpdateSaldoAsignar(AccountCreditSearchData AccountCredit)
        {

            return new CTERepository().UpdateSaldoAsignar(AccountCredit);

        }
        


    }
}

