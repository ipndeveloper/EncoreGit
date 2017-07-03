using System;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Interfaces;
using ExecutionContext = NetSteps.Common.Utility.ExecutionContext;

namespace NetSteps.Data.Entities.PaymentGateways
{
    public abstract class BasePaymentGateway : IPaymentGateway
    {
        #region Constructors

        public BasePaymentGateway(PaymentGatewaySection paymentGatewaySection)
        {
            _paymentGatewaySection = paymentGatewaySection;
        }

        public BasePaymentGateway(bool liveMode, bool testTransaction)
        {
            LiveMode = liveMode;
            TestTransaction = TestTransaction;
        }

        #endregion

        #region Members

        protected readonly PaymentGatewaySection _paymentGatewaySection;
        private PaymentAuthorizationResponse _result = null;
        private bool? _liveMode = null;
        private bool? _testTransaction = null;
        private string _alertEmailAddresses = null;
        private OrderPayment _currentOrderPayment = null;
        private OrderPaymentResult _currentOrderPaymentResult = null;
        private BasicResponse _validationResonse = null;

        public PaymentAuthorizationResponse Result
        {
            get
            {
                if (_result == null)
                {
                    _result = new PaymentAuthorizationResponse();
                }

                return _result;

            }
            private set
            {
                _result = value;
            }
        }

        public bool LiveMode
        {
            get
            {
                if (!_liveMode.HasValue)
                {
                    _liveMode = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.IsPaymentLiveMode, false);
                }

                return _liveMode.ToBool();

            }
            private set
            {
                _liveMode = value;
            }
        }

        public bool TestTransaction
        {
            get
            {
                if (!_testTransaction.HasValue)
                {
                    _testTransaction = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.IsPaymentTestTransaction, true);
                }

                return _testTransaction.ToBool();
            }
            private set
            {
                _testTransaction = value;
            }
        }

        public string AlertEmailAddresses
        {
            get
            {
                if (_alertEmailAddresses == null)
                {
                    _alertEmailAddresses = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.AlertEmailAddresses);
                }

                return _alertEmailAddresses;
            }
            private set
            {
                _alertEmailAddresses = value;
            }
        }

        public OrderPayment CurrentOrderPayment
        {
            get
            {
                if (_currentOrderPayment == null)
                {
                    _currentOrderPayment = new OrderPayment();
                }

                return _currentOrderPayment;
            }
            set
            {
                _currentOrderPayment = value;
            }
        }

        public OrderPaymentResult CurrentOrderPaymentResult
        {
            get
            {
                if (_currentOrderPaymentResult == null)
                {
                    _currentOrderPaymentResult = new OrderPaymentResult(CurrentOrderPayment);

                    SetBaseCurrentOrderPaymentResultValues();
                }

                return _currentOrderPaymentResult;
            }
            private set
            {
                _currentOrderPaymentResult = value;
            }
        }

        public BasicResponse ValidationResponse
        {
            get
            {
                if (_validationResonse == null)
                {
                    _validationResonse = new BasicResponse();
                }

                return _validationResonse;
            }
            set
            {
                _validationResonse = value;
            }
        }

        #endregion

        #region Methods

        private void SetBaseCurrentOrderPaymentResultValues()
        {
            _currentOrderPaymentResult.CurrencyID = CurrentOrderPayment.CurrencyID;
            _currentOrderPaymentResult.ModifiedByUserID = ApplicationContext.Instance.CurrentUserID.ToIntNullable();
            _currentOrderPaymentResult.DateAuthorized = DateTime.Now;
            _currentOrderPaymentResult.Amount = CurrentOrderPayment.Amount;
            _currentOrderPaymentResult.OrderID = CurrentOrderPayment.OrderID;
            _currentOrderPaymentResult.OrderPaymentID = CurrentOrderPayment.OrderPaymentID;
            _currentOrderPaymentResult.IsTesting = TestTransaction;

            _currentOrderPaymentResult.ApprovalCode = string.Empty;
            _currentOrderPaymentResult.ResponseReasonCode = string.Empty;
            _currentOrderPaymentResult.ResponseCode = string.Empty;
            _currentOrderPaymentResult.ResponseSubCode = string.Empty;
            _currentOrderPaymentResult.ResponseReasonText = string.Empty;
            _currentOrderPaymentResult.AuthorizeType = string.Empty;
            _currentOrderPaymentResult.CardCodeResponse = string.Empty;
            _currentOrderPaymentResult.ErrorMessage = string.Empty;
            _currentOrderPaymentResult.Response = string.Empty;
            _currentOrderPaymentResult.AVSResult = string.Empty;
            _currentOrderPaymentResult.TransactionID = string.Empty;

            if (CurrentOrderPayment.OrderCustomerID != null && CurrentOrderPayment.OrderCustomerID != 0)
            {
                _currentOrderPaymentResult.OrderCustomerID = CurrentOrderPayment.OrderCustomerID ?? 0;
            }

            if (_paymentGatewaySection != null)
            {
                _currentOrderPaymentResult.PaymentGatewayID = _paymentGatewaySection.PaymentGatewayID.ToShortNullable();
                _currentOrderPayment.PaymentGatewayID = _paymentGatewaySection.PaymentGatewayID.ToShortNullable();
            }
        }

        public virtual void ResetValues()
        {
            _result = null;
            _liveMode = null;
            _testTransaction = null;
            _alertEmailAddresses = null;
            _currentOrderPayment = null;
            _currentOrderPaymentResult = null;
            _validationResonse = null;
        }

        public virtual PaymentAuthorizationResponse Charge(OrderPayment orderPayment)
        {
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
                ResetValues();

                SetCurrentOrderPayment(orderPayment);

                decimal currentBalance = 0;
                ValidationResponse = ValidateCharge(ref currentBalance);

                SetResultStatusFromValidationResponse();

                ChargePayment();

                SetTransactionChargeOrderPaymentResult();

                RecordTransaction();

                SaveCurrentOrderPayment();

                return Result;
            }
        }

        public virtual void RecordTransaction()
        {
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
                try
                {
                    CurrentOrderPaymentResult.Save();
                    CurrentOrderPayment.OrderPaymentResults.Add(CurrentOrderPaymentResult);
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsPaymentGatewayException);
                }
            }
        }

        public virtual PaymentAuthorizationResponse Refund(OrderPayment orderPayment, decimal refundAmount)
        {
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
                SetCurrentOrderPayment(orderPayment);

                decimal currentBalance = 0;
                ValidationResponse = ValidateRefund(ref currentBalance);

                SetResultStatusFromValidationResponse();

                RefundPayment(refundAmount);

                SetRecordTransactionRefundOrderPaymentResult();

                RecordTransaction();

                SaveCurrentOrderPayment();

                return Result;
            }
        }

        private void SetResultStatusFromValidationResponse()
        {
            Result.Success = ValidationResponse.Success;
            Result.Message = ValidationResponse.Message;
        }

        protected BasicResponse ValidateCharge(ref decimal currentBalance)
        {
            return ValidateCharge(CurrentOrderPayment, ref currentBalance);
        }

        public virtual BasicResponse ValidateCharge(OrderPayment orderPayment, ref decimal currentBalance)
        {
            BasicResponse response = new BasicResponse();

            response.Success = true;
            response.Message = string.Empty;

            Result.Success = true;

            return response;
        }

        protected BasicResponse ValidateRefund(ref decimal currentBalance)
        {
            return ValidateRefund(CurrentOrderPayment, ref currentBalance);
        }

        public virtual BasicResponse ValidateRefund(OrderPayment orderPayment, ref decimal currentBalance)
        {
            BasicResponse response = new BasicResponse();

            response.Success = true;
            response.Message = string.Empty;

            Result.Success = true;

            return response;
        }

        private void SaveCurrentOrderPayment()
        {
            CurrentOrderPayment.ProcessedDate = CurrentOrderPayment.ProcessOnDate = DateTime.Now;
            CurrentOrderPayment.Save();
        }

        private void SetCurrentOrderPayment(OrderPayment orderPayment)
        {
            CurrentOrderPayment = orderPayment;

            if (!CurrentOrderPayment.ChangeTracker.ChangeTrackingEnabled)
            {
                CurrentOrderPayment.StartTracking();
            }
        }

        public virtual string GetPaymentGatewayUrl(bool isLiveMode)
        {
            if (isLiveMode)
            {
                return !String.IsNullOrWhiteSpace(_paymentGatewaySection.LiveUrl)
                          ? _paymentGatewaySection.LiveUrl
                          : String.Empty;

            }

            return !String.IsNullOrWhiteSpace(_paymentGatewaySection.TestUrl)
                       ? _paymentGatewaySection.TestUrl
                       : String.Empty;
        }

        public virtual string GetPaymentGatewayTerminalId(bool isLiveMode)
        {
            if (isLiveMode)
            {
                return !String.IsNullOrWhiteSpace(_paymentGatewaySection.LiveTerminalId)
                          ? _paymentGatewaySection.LiveTerminalId
                          : String.Empty;

            }

            return !String.IsNullOrWhiteSpace(_paymentGatewaySection.TestTerminalId)
                       ? _paymentGatewaySection.TestTerminalId
                       : String.Empty;
        }

        public virtual string TestAuthorizeID()
        {
            return !String.IsNullOrWhiteSpace(_paymentGatewaySection.TestLogin)
                       ? _paymentGatewaySection.TestLogin
                       : String.Empty;
        }

        public virtual string TestAuthorizePin()
        {
            return !String.IsNullOrWhiteSpace(_paymentGatewaySection.TestPassword)
                       ? _paymentGatewaySection.TestPassword
                       : String.Empty;
        }

        public virtual decimal TestMinAmount()
        {
            return !String.IsNullOrWhiteSpace(_paymentGatewaySection.TestMinAmount)
                       ? _paymentGatewaySection.TestMinAmount.ToDecimal()
                       : Decimal.Zero;
        }

        public virtual decimal TestFailAmount()
        {
            return !String.IsNullOrWhiteSpace(_paymentGatewaySection.TestFailAmount)
                       ? _paymentGatewaySection.TestFailAmount.ToDecimal()
                       : Decimal.Zero;
        }

        public virtual string TestFailCreditCardNumber()
        {
            return !String.IsNullOrWhiteSpace(_paymentGatewaySection.TestFailCreditCardNumber)
                       ? _paymentGatewaySection.TestFailCreditCardNumber
                       : String.Empty;
        }

        protected abstract PaymentAuthorizationResponse ChargePayment();

        protected abstract PaymentAuthorizationResponse RefundPayment(decimal refundAmount);

        protected abstract void SetTransactionChargeOrderPaymentResult();

        protected abstract void SetRecordTransactionRefundOrderPaymentResult();

        #endregion
    }
}
