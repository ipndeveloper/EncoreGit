using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;
using NetSteps.GiftCards.Common;


namespace NetSteps.GiftCards.Service
{
    [ContainerRegister(typeof(IGiftCardService), RegistrationBehaviors.Default)]
    public class GiftCardService : IGiftCardService
    {
        #region Members
        /// <summary>
        /// Modified version found at: https://www.grc.com/ppp
        /// !#%+23456789:=?@ABCDEFGHJKLMNPRSTUVWXYZabcdefghijkmnopqrstuvwxyz
        /// </summary>
        private const string _possibleCodeCharacters = "23456789ABCDEFGHJKLMNPRSTUVWXYZ";

        private const int _numberOfTriesForUniqueCodeBeforeError = 300;
        #endregion

        public void SetUniqueGiftCardCode(IGiftCardRepository repository, IGiftCard giftCard)
        {
            giftCard.Code = String.Empty;

            for (int i = 1; i <= _numberOfTriesForUniqueCodeBeforeError; i++)
            {
                giftCard.Code = GenerateCode(10);

                if (repository.IsUniqueCode(giftCard.Code))
                {
                    break;
                }
                if (i == _numberOfTriesForUniqueCodeBeforeError)
                {
                    giftCard.Code = String.Empty;

                    throw new NetStepsBusinessException("Unable to generate a unique gift card code")
                    {
                        PublicMessage = Translation.GetTerm("Unable to generate a unique gift card code")
                    };
                }
            }
        }

        public string GenerateCode(int desiredLength)
        {
            string code = string.Empty;

            for (var i = 0; i < desiredLength; i++)
            {
                code += _possibleCodeCharacters[NetSteps.Common.Random.Next(0, _possibleCodeCharacters.Length - 1)];
            }

            return code;
        }

        public void GenerateGiftCardCodesForAllPurchasedGiftCards(IOrder order, DateTime? expirationDate = null)
        {
            var repository = Create.New<IGiftCardRepository>();

            if (order != null && order.OrderStatusID != (int)Constants.OrderStatus.Pending
                && order.OrderStatusID != (int)Constants.OrderStatus.PendingError)
            {
                var inventory = Create.New<InventoryBaseRepository>();

                var giftCardProductIDs = repository.FindAllGiftCardProductIDs();
                var giftCardOrderItems =
                    order.OrderCustomers.SelectMany(oc => oc.OrderItems).Where(
                        oi => oi.ProductID.HasValue && giftCardProductIDs.Contains(oi.ProductID.Value)).ToList();

                foreach (OrderItem oi in giftCardOrderItems)
                {
                    var orderItemGiftCardCount = repository.OrderItemGiftCardCount(oi.OrderItemID);
                    var product = inventory.GetProduct(oi.ProductID.ToInt());
                    ProductPrice price = null;
                    if (product != null && product.Prices != null)
                    {
                        price =
                            product.Prices.FirstOrDefault(
                                pp =>
                                pp.ProductPriceTypeID == (int)Constants.ProductPriceType.Retail
                                && pp.CurrencyID == order.CurrencyID);
                    }
                    decimal gcAmount = (price == null ? 0 : price.Price);

                    if (orderItemGiftCardCount < oi.Quantity && gcAmount > 0)
                    {
                        int numberOfGCs = oi.Quantity - orderItemGiftCardCount;
                        for (int i = 0; i < numberOfGCs; i++)
                        {
                            var newGC = Create.New<IGiftCard>();
                            newGC.InitialAmount = gcAmount;
                            newGC.Balance = gcAmount;
                            newGC.ExpirationDate = expirationDate;
                            newGC.CurrencyID = order.CurrencyID;
                            newGC.OriginOrderItemID = oi.OrderItemID;
                            newGC.Code = GenerateCode(10);
                            Update(newGC);
                            oi.GiftCards.Add((GiftCard)newGC);
                        }

                        //newGC.SetUniqueGiftCardCode();
                        //newGC.StartTracking();
                        //newGC.Save();
                        //oi.StartTracking();
                        //oi.GiftCards.Add(newGC);
                    }
                }
            }
        }

        public List<int> FindAllGiftCardProductIDs()
        {
            var repo = Create.New<IGiftCardRepository>();
            return repo.FindAllGiftCardProductIDs();
        }

        public bool IsUniqueCode(string code)
        {
            var repo = Create.New<IGiftCardRepository>();
            return repo.IsUniqueCode(code);
        }

        public int OrderItemGiftCardCount(int orderItemID)
        {
            var repo = Create.New<IGiftCardRepository>();
            return repo.OrderItemGiftCardCount(orderItemID);
        }

        public IGiftCard FindByCodeAndCurrency(string code, int currencyID)
        {
            var repo = Create.New<IGiftCardRepository>();
            return repo.FindByCodeAndCurrency(code, currencyID);
        }

        public decimal? GetBalanceWithPendingPayments(string code, IOrder order)
        {
            var repo = Create.New<IGiftCardRepository>();
            return repo.GetBalanceWithPendingPayments(code, order);
        }

        public bool Update(IGiftCard giftCard)
        {
            var repo = Create.New<IGiftCardRepository>();
            return repo.Update(giftCard);
        }

        //this really belongs in some kind of currency service or extension elsewhere, but its ending up here for the time being
        public int GetCurrencyIDByCode(string currencyCode)
        {
            var currency = SmallCollectionCache.Instance.Currencies.FirstOrDefault(c => c.CurrencyCode.EqualsIgnoreCase(currencyCode));
            if (currency == null)
            {
                throw new ArgumentException(string.Format("currencyCode '{0}' not found", currencyCode));
            }
            return currency.CurrencyID;
        }
    }
}