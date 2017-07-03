using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class OrderPaymentsBusinessLogic
    {

        public static dynamic Insert(OrderPaymentsParameters model)
        {
            var table = new OrderPayments();

            try
            {
                 table.Insert(new
                {
                    PaymentConfigurationID = model.PaymentConfigurationID,
                    FineAndInterestsRulesID = model.FineAndInterestRulesID,
                    //TicketNumber = model.TicketNumber,
                    OrderID = model.OrderID,
                    OrderCustomerID = model.OrderCustomerID,
                    CurrencyID = model.CurrencyID,
                    OrderPaymentStatusID = model.OrderPaymentStatusID,
                    OriginalExpirationDate = model.OriginalExpirationDateUTC,
                    CurrentExpirationDateUTC = model.CurrentExpirationDateUTC,
                    InitialAmount = model.InitialAmount,
                    TotalAmount = model.TotalAmount,
                    DateLastTotalAmountUTC = model.DateLastTotalAmountUTC,
                    ExpirationStatusID = model.ExpirationStatusID,
                    NegotiationLevelID = model.NegotiationLevelID,
                    IsDeferred = model.IsDeferred,
                    ProcessOnDateUTC = model.ProcessOnDateUTC,
                    ProcessedDateUTC = model.ProcessedDateUTC,
                    TransactionID = model.TransactionID,
                    ModifiedByUserID = model.ModifiedByUserID,
                    PaymentGatewayID = model.PaymentGatewayID,
                    DateCreatedUTC = model.DateCreatedUTC,
                    DateLastModifiedUTC = model.DateLastModifiedUTC,
                    Amount = 0,
                    BillingPostalCode = "",
                    PaymentTypeID = model.PaymentTypeID
                });

                table = new OrderPayments();

                var ID = table.All().OrderByDescending(x => x.OrderPaymentID).FirstOrDefault().ID;

                table = new OrderPayments();
                table.Update(new
                {
                    TicketNumber = ID
                },
                   ID
                 );

                return ID;
            }
            catch
            {
                return null;
            }
        }

     
        public bool Update(dynamic model)
        {
            var table = new AccountProperties();

            try
            {
                if (model.AccountPropertyValueID > 0)
                {
                    table.Update(new
                    {
                        AccountID = model.AccountID,
                        AccountPropertyTypeID = model.AccountPropertyTypeID,
                        AccountPropertyID = model.AccountPropertyID,
                        AccountPropertyValueID = model.AccountPropertyValueID, //  solo cuando es el valor de un combo                 
                        Active = model.Active,
                    },
                    model.AccountPropertyID
                    );

                }
                else {
                    table.Update(new
                    {
                        AccountID = model.AccountID,
                        AccountPropertyTypeID = model.AccountPropertyTypeID,
                        AccountPropertyID = model.AccountPropertyID,
                        PropertyValue = model.PropertyValue,
                        Active = model.Active,
                    },
                       model.AccountPropertyID
                       );
                }


                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
