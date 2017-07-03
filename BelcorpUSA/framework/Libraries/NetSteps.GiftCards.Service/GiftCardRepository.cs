using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;
using NetSteps.GiftCards.Common;

namespace NetSteps.GiftCards.Service
{

    [ContainerRegister(typeof(IGiftCardRepository), RegistrationBehaviors.Default)]
    public class GiftCardRepository : IGiftCardRepository
    {
        public List<int> FindAllGiftCardProductIDs()
        {
            return ExceptionHandledDataAction.Run(
                new ExecutionContext(this),
                () =>
                    {
                        using (var context = Create.New<NetStepsEntities>())
                        {
                            var result = (from pt in context.ProductTypes
                                          join pb in context.ProductBases on pt.ProductTypeID equals pb.ProductTypeID
                                          join p in context.Products on pb.ProductBaseID equals p.ProductBaseID
                                          where pt.ProductTypeID == (int)Constants.ProductType.GiftCard
                                          select p.ProductID).ToList();
                            return result;
                        }
                    });
        }

        public bool IsUniqueCode(string code)
        {
            return ExceptionHandledDataAction.Run(
                new ExecutionContext(this),
                () =>
                    {
                        using (var context = Create.New<NetStepsEntities>())
                        {
                            var result =
                                !(from gc in context.GiftCards
                                  where gc.Code.ToLower() == code.ToLower()
                                  select gc.GiftCardID).Any();
                            return result;
                        }
                    });
        }

        public int OrderItemGiftCardCount(int orderItemID)
        {
            return ExceptionHandledDataAction.Run(
                new ExecutionContext(this),
                () =>
                    {
                        using (var context = Create.New<NetStepsEntities>())
                        {
                            var result =
                                (from gc in context.GiftCards
                                 where gc.OriginOrderItemID == orderItemID
                                 select gc.GiftCardID).Count();
                            return result;
                        }
                    });
        }

        public IGiftCard FindByCodeAndCurrency(string code, int currencyID)
        {
            return ExceptionHandledDataAction.Run(
                new ExecutionContext(this),
                () =>
                    {
                        using (var context = Create.New<NetStepsEntities>())
                        {
                            var result =
                                context.GiftCards.FirstOrDefault(
                                    gc => gc.CurrencyID == currencyID && gc.Code.ToLower() == code.ToLower());
                            return result == null ? null : Create.AsIf<IGiftCard>(result);
                        }
                    });
        }

        public decimal? GetBalanceWithPendingPayments(string code, IOrder order)
        {
            decimal? result = null;

            var orderObject = (Order)order;
            var gc = this.FindByCodeAndCurrency(code, order.CurrencyID);
            if (gc != null)
            {
                result = gc.Balance ?? 0m;
                decimal existingPaymentsSum =
                    orderObject.OrderPayments.Concat(orderObject.OrderCustomers.SelectMany(oc => oc.OrderPayments)).
                        Where(
                            op =>
                            op.PaymentTypeID == (int)Constants.PaymentType.GiftCard
                            && op.OrderPaymentStatusID == (int)Constants.OrderPaymentStatus.Pending
                            && op.DecryptedAccountNumber == code).Distinct().Sum(op => op.Amount);
                result -= existingPaymentsSum;
            }

            return result;
        }

        public bool Update(IGiftCard giftCard)
        {
            var gcUpdate = Create.New<GiftCard>();

            ExceptionHandledDataAction.Run(
                new ExecutionContext(this),
                () =>
                    {
                        using (var context = Create.New<NetStepsEntities>())
                        {
                            gcUpdate =
                                context.GiftCards.FirstOrDefault(
                                    gc =>
                                    gc.CurrencyID == giftCard.CurrencyID && gc.Code.ToLower() == giftCard.Code.ToLower())
                                ?? new GiftCard { CurrencyID = giftCard.CurrencyID, Code = giftCard.Code };
                            gcUpdate.StartEntityTracking();
                            gcUpdate.Balance = giftCard.Balance;
                            gcUpdate.InitialAmount = giftCard.InitialAmount;
                            gcUpdate.ExpirationDate = giftCard.ExpirationDate;
                            gcUpdate.OriginOrderItemID = giftCard.OriginOrderItemID;
                            gcUpdate.Save();
                        }
                    });

            return true;
        }
    }
}