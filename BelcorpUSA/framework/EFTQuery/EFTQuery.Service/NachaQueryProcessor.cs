using System;
using System.Collections.Generic;
using EFTQuery.Common;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.Dto;
using NetSteps.Encore.Core.IoC;

namespace EFTQuery.Service
{
    [ContainerRegister(typeof(IEFTQueryProcessor), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class NachaQueryProcessor : IEFTQueryProcessor
    {
        protected IOrderPaymentRepository OrderPaymentRepository { get; private set; }

        public NachaQueryProcessor(IOrderPaymentRepository orderPaymentRepository)
        {
            OrderPaymentRepository = orderPaymentRepository;
        }

        public IEnumerable<IEFTQueryProcessorResult> GetTransfersByClassType(string classType)
        {
            var orderPaymentBusinessLogic = Create.New<IOrderPaymentBusinessLogic>();
            IEnumerable<IOrderPayment> payments = orderPaymentBusinessLogic.FilterByNachaClassType(OrderPaymentRepository, classType);
            var result = GetEFTQueryProcessorResultFromPayments(payments);
            return result;
        }

        public IEnumerable<IEFTQueryProcessorResult> GetTransfersByDateRangeAndClassType(DateTime startDate, DateTime endDate, string classType)
        {
            var orderPayment = Create.New<IOrderPayment>();
            var orderPaymentBusinessLogic = Create.New<IOrderPaymentBusinessLogic>();
            IEnumerable<IOrderPayment> payments = orderPaymentBusinessLogic.FilterByDateAndNachaClassType(OrderPaymentRepository, startDate, endDate, classType);
            var result = GetEFTQueryProcessorResultFromPayments(payments);
            return result;
        }

        public IEnumerable<IEFTQueryProcessorResult> GetTransfersByClassTypeAndCountryID(string classType, int countryID)
        {
            var orderPaymentBusinessLogic = Create.New<IOrderPaymentBusinessLogic>();
            IEnumerable<OrderPayment> payments = orderPaymentBusinessLogic.FilterByNachaClassTypeAndCountryID(OrderPaymentRepository, classType, countryID);
            var result = GetEFTQueryProcessorResultFromPayments(payments);
            return result;
        }

        private IEnumerable<IEFTQueryProcessorResult> GetEFTQueryProcessorResultFromPayments(IEnumerable<IOrderPayment> orderPayments)
        {
            var result = new List<IEFTQueryProcessorResult>();
            int counter = 0;
            using (var create = Create.SharedOrNewContainer())
            {
                foreach (var orderPayment in orderPayments)
                {
                    var payment = orderPayment;
                    int count = counter;
                    counter++;
                    var info = create.Mutation(create.New<IEFTQueryProcessorResult>(),
                        it =>
                        {
                            it.RecordTypeCode = "6";
                            it.TransactionCode = payment.BankAccountTypeID == 1 ? "27" : "37";
                            it.RoutingNumber = Convert.ToString(payment.RoutingNumber);
                            it.AccountNumber = payment.DecryptedAccountNumber;
                            it.Amount = payment.Amount.ToString("F");
                            it.IndividualName = payment.NameOnCard;
                            it.PaymentTypeCode = payment.BillingCountryID == 2 ? "CA" : "S";
                            it.TransactionNumber = count.ToString();
                            it.TransactionDate = payment.ProcessedDateUTC.ToString();
                            it.CountryCode = payment.BillingCountryID == 2 ? "CA" : "US"; 
                            it.NachaClassType = payment.NachaClassType.ToString();
                            it.BankAccountType = payment.BankAccountTypeID != null
                             ? BankAccountType.Load((short)payment.BankAccountTypeID).Name
                             : Constants.BankAccountTypeEnum.Checking.ToString();
                            it.OrderId = payment.OrderID.ToString();
                            it.OrderPaymentId = payment.OrderPaymentID.ToString();
                        });

                    result.Add(info);
                }
            }

            return result;
        }

        private IEnumerable<IEFTQueryProcessorResult> GetEFTQueryProcessorResultFromPayments(IEnumerable<OrderPayment> orderPayments)
        {
            var result = new List<IEFTQueryProcessorResult>();
            int counter = 0;
            using (var create = Create.SharedOrNewContainer())
            {
                foreach (var orderPayment in orderPayments)
                {
                    var payment = orderPayment;
                    int count = counter;
                    counter++;
                    var info = create.Mutation(create.New<IEFTQueryProcessorResult>(),
                        it =>
                        {
                            it.RecordTypeCode = "6";
                            it.TransactionCode = payment.BankAccountTypeID == 1 ? "27" : "37";
                            it.RoutingNumber = Convert.ToString(payment.RoutingNumber);
                            it.AccountNumber = payment.DecryptedAccountNumber;
                            it.Amount = payment.Amount.ToString("F");
                            it.IndividualName = payment.NameOnCard;
                            it.PaymentTypeCode = payment.BillingCountryID == 2 ? "CA" : "S";
                            it.TransactionNumber = count.ToString();
                            it.TransactionDate = payment.ProcessedDateUTC.ToString();
                            it.CountryCode = payment.BillingCountryID == 2 ? "CA" : "US";
                            it.NachaClassType = payment.NachaClassType.ToString();
                            it.BankAccountType = payment.BankAccountTypeID != null
                             ? BankAccountType.Load((short)payment.BankAccountTypeID).Name
                             : Constants.BankAccountTypeEnum.Checking.ToString();
                            it.OrderId = payment.OrderID.ToString();
                            it.OrderPaymentId = payment.OrderPaymentID.ToString();
                        });

                    result.Add(info);
                }
            }

            return result;
        }

        public IEnumerable<IOrderPaymentUpdateResult> UpdateNachaQueryProcessorResults(List<int> orderPaymentIds)
        {
            foreach (var orderPaymentId in orderPaymentIds)
            {
				var result = Create.Mutation(Create.New<IOrderPaymentUpdateResult>(), m => m.OrderPaymentId = orderPaymentId);
				try
                {
                    UpdateNachaSendDate(orderPaymentId);
					result.Success = true;
                }
                catch (Exception ex)
                {
					result.Success = false;
					result.Message = ex.Message;
                }
				yield return result;
            }
        }

        private void UpdateNachaSendDate(int orderPaymentId)
        {
            var orderPaymentBusinessLogic = Create.New<IOrderPaymentBusinessLogic>();
            var payment = orderPaymentBusinessLogic.LoadOrderPaymentByPaymentId(OrderPaymentRepository, orderPaymentId);
            if (payment != null)
            {
                payment.StartEntityTracking();
                payment.NachaSentDate = DateTime.Today;
                payment.Save();
                payment.StopEntityTracking();
            }
        }

        public IEFTQueryProcessorResult GetTransferByOrderId(int orderId)
        {
            var payment = OrderPaymentRepository.LoadEftOrderPaymentByOrderId(orderId);
            using (var create = Create.SharedOrNewContainer())
            {
                var info = create.Mutation(create.New<IEFTQueryProcessorResult>(),
                       it =>
                       {
                           it.RecordTypeCode = "6";
                           it.TransactionCode = payment.BankAccountTypeID == 1 ? "27" : "37";
                           it.RoutingNumber = Convert.ToString(payment.RoutingNumber);
                           it.AccountNumber = payment.DecryptedAccountNumber;
                           it.Amount = payment.Amount.ToString("F");
                           it.IndividualName = payment.NameOnCard;
                           it.PaymentTypeCode = "S";
                           it.TransactionDate = payment.ProcessedDateUTC.ToString();
                           it.CountryCode = Country.Load(payment.BillingCountryID != null ? (int)payment.BillingCountryID : 1).CountryCode;
                           it.NachaClassType = payment.NachaClassType ?? "Web";
                           it.BankAccountType = payment.BankAccountTypeID != null
                             ? BankAccountType.Load((short)payment.BankAccountTypeID).Name
                             : Constants.BankAccountTypeEnum.Checking.ToString();
                           it.OrderId = payment.OrderID.ToString();
                           it.OrderPaymentId = payment.OrderPaymentID.ToString();
                       });
                return info;
            }
        }
    }
}
