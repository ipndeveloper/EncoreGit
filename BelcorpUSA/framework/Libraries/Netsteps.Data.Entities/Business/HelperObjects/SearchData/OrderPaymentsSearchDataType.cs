using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.SqlServer.Server;
using System.Data;

namespace NetSteps.Data.Entities.Business
{

    public class OrderPaymentsSearchDataType : List<OrderPaymentNegotiationData>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var OrderPaymentsSearchType = new SqlDataRecord(
                //new SqlMetaData("OrderPaymentID", SqlDbType.Int),
                                        new SqlMetaData("OrderID", SqlDbType.Int),
                                        new SqlMetaData("OrderCustomerID", SqlDbType.Int),
                                        new SqlMetaData("PaymentTypeID", SqlDbType.Int),
                                        new SqlMetaData("CurrencyID", SqlDbType.Int),
                                        new SqlMetaData("OrderPaymentStatusID", SqlDbType.SmallInt),
                                        new SqlMetaData("CreditCardTypeID", SqlDbType.SmallInt),
                                        new SqlMetaData("NameOnCard", SqlDbType.NVarChar, 100),
                                        new SqlMetaData("AccountNumber", SqlDbType.VarChar, 200),
                                        new SqlMetaData("BillingFirstName", SqlDbType.NVarChar, 100),
                                        new SqlMetaData("BillingLastName", SqlDbType.NVarChar, 100),
                                        new SqlMetaData("BillingName", SqlDbType.NVarChar, 200),
                                        new SqlMetaData("BillingAddress1", SqlDbType.NVarChar, 200),
                                        new SqlMetaData("BillingAddress2", SqlDbType.NVarChar, 200),
                                        new SqlMetaData("BillingAddress3", SqlDbType.NVarChar, 200),
                                        new SqlMetaData("BillingCity", SqlDbType.NVarChar, 200),
                                        new SqlMetaData("BillingCounty", SqlDbType.NVarChar, 100),
                                        new SqlMetaData("BillingState", SqlDbType.NVarChar, 100),
                                        new SqlMetaData("BillingStateProvinceID", SqlDbType.Int),
                                        new SqlMetaData("BillingPostalCode", SqlDbType.NVarChar, 20),
                                        new SqlMetaData("BillingCountryID", SqlDbType.Int),
                                        new SqlMetaData("BillingPhoneNumber", SqlDbType.NVarChar, 50),
                                        new SqlMetaData("IdentityNumber", SqlDbType.NVarChar, 100),
                                        new SqlMetaData("IdentityState", SqlDbType.NVarChar, 100),
                                        new SqlMetaData("Amount", SqlDbType.Money),
                                        new SqlMetaData("RoutingNumber", SqlDbType.NVarChar, 50),
                                        new SqlMetaData("IsDeferred", SqlDbType.Bit),
                                        new SqlMetaData("ProcessOnDateUTC", SqlDbType.DateTime),
                                        new SqlMetaData("ProcessedDateUTC", SqlDbType.DateTime),
                                        new SqlMetaData("TransactionID", SqlDbType.NVarChar, 50),
                                        new SqlMetaData("DeferredAmount", SqlDbType.Money),
                                        new SqlMetaData("DeferredTransactionID", SqlDbType.NVarChar, 100),
                                        new SqlMetaData("ExpirationDateUTC", SqlDbType.DateTime),
                //new SqlMetaData("DataVersion", SqlDbType.Timestamp),
                                        new SqlMetaData("ModifiedByUserID", SqlDbType.Int),
                                        new SqlMetaData("Request", SqlDbType.VarChar, 250),
                                        new SqlMetaData("AccountNumberLastFour", SqlDbType.NVarChar, 10),
                                        new SqlMetaData("PaymentGatewayID", SqlDbType.SmallInt),
                                        new SqlMetaData("SourceAccountPaymentMethodID", SqlDbType.Int),
                                        new SqlMetaData("BankAccountTypeID", SqlDbType.SmallInt),
                                        new SqlMetaData("BankName", SqlDbType.NVarChar, 100),
                                        new SqlMetaData("NachaClassType", SqlDbType.NVarChar, 10),
                                        new SqlMetaData("NachaSentDate", SqlDbType.DateTime),
                                        new SqlMetaData("ETLNaturalKey", SqlDbType.NVarChar, 256),
                                        new SqlMetaData("ETLHash", SqlDbType.NVarChar, 50),
                                        new SqlMetaData("ETLPhase", SqlDbType.NVarChar, 50),
                                        new SqlMetaData("ETLDate", SqlDbType.DateTime),
                                        new SqlMetaData("DateCreatedUTC", SqlDbType.DateTime),
                                        new SqlMetaData("DateLastModifiedUTC", SqlDbType.DateTime),
                                        new SqlMetaData("BillingStreet", SqlDbType.NVarChar, 100),
                                        new SqlMetaData("NegotiationLevelID", SqlDbType.Int),
                                        new SqlMetaData("OrderExpirationStatusID", SqlDbType.Int),
                                        new SqlMetaData("PaymentConfigurationID", SqlDbType.Int),
                                        new SqlMetaData("FineAndInterestsRulesID", SqlDbType.Int),
                                        new SqlMetaData("TicketNumber", SqlDbType.Int),
                                        new SqlMetaData("OriginalExpirationDate", SqlDbType.DateTime),
                                        new SqlMetaData("CurrentExpirationDateUTC", SqlDbType.DateTime),
                                        new SqlMetaData("InitialAmount", SqlDbType.Money),
                                        new SqlMetaData("FinancialAmount", SqlDbType.Money),
                                        new SqlMetaData("DiscountedAmount", SqlDbType.Money),
                                        new SqlMetaData("TotalAmount", SqlDbType.Money),
                                        new SqlMetaData("DateLastTotalAmountUTC", SqlDbType.DateTime),
                                        new SqlMetaData("Accepted", SqlDbType.Bit),
                                        new SqlMetaData("Forefit", SqlDbType.Bit),
                                        new SqlMetaData("ExpirationStatusID", SqlDbType.Int),
                                        new SqlMetaData("OrderPaymentID", SqlDbType.Int),
                                        new SqlMetaData("DateValidity", SqlDbType.Int),
                                        new SqlMetaData("RenegotiationConfigurationID", SqlDbType.Int),
                                        new SqlMetaData("ExpirationDays", SqlDbType.Int)
                                      );

            foreach (OrderPaymentNegotiationData OrderPayment in this)
            {
                OrderPaymentsSearchType.SetInt32(0, OrderPayment.OrderID);
                OrderPaymentsSearchType.SetInt32(1, OrderPayment.OrderCustomerID);
                OrderPaymentsSearchType.SetInt32(2, OrderPayment.PaymentTypeID);
                OrderPaymentsSearchType.SetInt32(3, OrderPayment.CurrencyID);
                OrderPaymentsSearchType.SetInt16(4, OrderPayment.OrderPaymentStatusID);
                OrderPaymentsSearchType.SetInt16(5, OrderPayment.CreditCardTypeID);
                OrderPaymentsSearchType.SetString(6, OrderPayment.NameOnCard);
                OrderPaymentsSearchType.SetString(7, OrderPayment.AccountNumber);
                OrderPaymentsSearchType.SetString(8, OrderPayment.BillingFirstName);
                OrderPaymentsSearchType.SetString(9, OrderPayment.BillingLastName);
                OrderPaymentsSearchType.SetString(10, OrderPayment.BillingName);
                OrderPaymentsSearchType.SetString(11, OrderPayment.BillingAddress1);
                OrderPaymentsSearchType.SetString(12, OrderPayment.BillingAddress2);
                OrderPaymentsSearchType.SetString(13, OrderPayment.BillingAddress3);
                OrderPaymentsSearchType.SetString(14, OrderPayment.BillingCity);
                OrderPaymentsSearchType.SetString(15, OrderPayment.BillingCounty);
                OrderPaymentsSearchType.SetString(16, OrderPayment.BillingState);
                OrderPaymentsSearchType.SetInt32(17, OrderPayment.BillingStateProvinceID);
                OrderPaymentsSearchType.SetString(18, OrderPayment.BillingPostalCode);
                OrderPaymentsSearchType.SetInt32(19, OrderPayment.BillingCountryID);
                OrderPaymentsSearchType.SetString(20, OrderPayment.BillingPhoneNumber);
                OrderPaymentsSearchType.SetString(21, OrderPayment.IdentityNumber);
                OrderPaymentsSearchType.SetString(22, OrderPayment.IdentityState);
                OrderPaymentsSearchType.SetDecimal(23, OrderPayment.Amount);
                OrderPaymentsSearchType.SetString(24, OrderPayment.RoutingNumber);
                OrderPaymentsSearchType.SetBoolean(25, OrderPayment.IsDeferred);
                OrderPaymentsSearchType.SetDateTime(26, OrderPayment.ProcessOnDateUTC);
                OrderPaymentsSearchType.SetDateTime(27, OrderPayment.ProcessedDateUTC);
                OrderPaymentsSearchType.SetString(28, OrderPayment.TransactionID);
                OrderPaymentsSearchType.SetDecimal(29, OrderPayment.DeferredAmount);
                OrderPaymentsSearchType.SetString(30, OrderPayment.DeferredTransactionID);
                OrderPaymentsSearchType.SetDateTime(31, OrderPayment.ExpirationDateUTC);
                //OrderPaymentsSearchType.SetTimeSpan(32, OrderPayment.DataVersion);
                OrderPaymentsSearchType.SetInt32(32, OrderPayment.ModifiedByUserID);
                OrderPaymentsSearchType.SetString(33, OrderPayment.Request);
                OrderPaymentsSearchType.SetString(34, OrderPayment.AccountNumberLastFour);
                OrderPaymentsSearchType.SetInt16(35, OrderPayment.PaymentGatewayID);
                OrderPaymentsSearchType.SetInt32(36, OrderPayment.SourceAccountPaymentMethodID);
                OrderPaymentsSearchType.SetInt16(37, OrderPayment.BankAccountTypeID);
                OrderPaymentsSearchType.SetString(38, OrderPayment.BankName);
                OrderPaymentsSearchType.SetString(39, OrderPayment.NachaClassType);
                OrderPaymentsSearchType.SetDateTime(40, OrderPayment.NachaSentDate);
                OrderPaymentsSearchType.SetString(41, OrderPayment.ETLNaturalKey);
                OrderPaymentsSearchType.SetString(42, OrderPayment.ETLHash);
                OrderPaymentsSearchType.SetString(43, OrderPayment.ETLPhase);
                OrderPaymentsSearchType.SetDateTime(44, OrderPayment.ETLDate);
                OrderPaymentsSearchType.SetDateTime(45, OrderPayment.DateCreatedUTC);
                OrderPaymentsSearchType.SetDateTime(46, OrderPayment.DateLastModifiedUTC);
                OrderPaymentsSearchType.SetString(47, OrderPayment.BillingStreet);
                OrderPaymentsSearchType.SetInt32(48, OrderPayment.NegotiationLevelID);
                OrderPaymentsSearchType.SetInt32(49, OrderPayment.OrderExpirationStatusID);
                OrderPaymentsSearchType.SetInt32(50, OrderPayment.PaymentConfigurationID);
                OrderPaymentsSearchType.SetInt32(51, OrderPayment.FineAndInterestsRulesID);
                OrderPaymentsSearchType.SetInt32(52, OrderPayment.TicketNumber);
                OrderPaymentsSearchType.SetDateTime(53, OrderPayment.OriginalExpirationDate);
                OrderPaymentsSearchType.SetString(54, OrderPayment.CurrentExpirationDateUTC);
                OrderPaymentsSearchType.SetString(55, OrderPayment.InitialAmount);
                OrderPaymentsSearchType.SetDecimal(56, OrderPayment.FinancialAmount);
                OrderPaymentsSearchType.SetDecimal(57, OrderPayment.DiscountedAmount);
                OrderPaymentsSearchType.SetString(58, OrderPayment.TotalAmount);
                OrderPaymentsSearchType.SetDateTime(59, OrderPayment.DateLastTotalAmountUTC);
                OrderPaymentsSearchType.SetBoolean(60, OrderPayment.Accepted);
                OrderPaymentsSearchType.SetBoolean(61, OrderPayment.Forefit);
                OrderPaymentsSearchType.SetInt32(62, OrderPayment.ExpirationStatusID);
                OrderPaymentsSearchType.SetInt32(63, OrderPayment.OrderPaymentID);
                OrderPaymentsSearchType.SetString(64, OrderPayment.DateValidity);
                OrderPaymentsSearchType.SetInt32(65, OrderPayment.RenegotiationConfigurationID);
                OrderPaymentsSearchType.SetInt32(66, OrderPayment.ExpirationDays);

                yield return OrderPaymentsSearchType;
            }
        }
    }
}
