using System.Collections.Generic;
using NetSteps.Data.Common.Entities;

namespace NetSteps.GiftCards.Common
{
    public interface IGiftCardRepository
    {
        List<int> FindAllGiftCardProductIDs();
        bool IsUniqueCode(string code);
        int OrderItemGiftCardCount(int orderItemID);
        IGiftCard FindByCodeAndCurrency(string code, int currencyID);
        decimal? GetBalanceWithPendingPayments(string code, IOrder order);
        bool Update(IGiftCard giftCard);
    }
}