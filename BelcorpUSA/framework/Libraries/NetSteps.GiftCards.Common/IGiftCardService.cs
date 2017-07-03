using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Data.Common.Entities;

namespace NetSteps.GiftCards.Common
{
	[ContractClass(typeof(Contracts.GiftCardServiceContracts))]
	public interface IGiftCardService
	{
		void SetUniqueGiftCardCode(IGiftCardRepository repository, IGiftCard giftCard);
		string GenerateCode(int desiredLength);
		void GenerateGiftCardCodesForAllPurchasedGiftCards(IOrder order, DateTime? expirationDate = null);
		List<int> FindAllGiftCardProductIDs();
		bool IsUniqueCode(string code);
		int OrderItemGiftCardCount(int orderItemID);
		IGiftCard FindByCodeAndCurrency(string code, int currencyID);
		decimal? GetBalanceWithPendingPayments(string code, IOrder order);
		bool Update(IGiftCard giftCard);
		int GetCurrencyIDByCode(string currencyCode);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IGiftCardService))]
		abstract class GiftCardServiceContracts : IGiftCardService
		{
			public void SetUniqueGiftCardCode(IGiftCardRepository repository, IGiftCard giftCard)
			{
				throw new NotImplementedException();
			}

			public string GenerateCode(int desiredLength)
			{
				throw new NotImplementedException();
			}

			public void GenerateGiftCardCodesForAllPurchasedGiftCards(IOrder order, DateTime? expirationDate = null)
			{
				throw new NotImplementedException();
			}

			public List<int> FindAllGiftCardProductIDs()
			{
				throw new NotImplementedException();
			}

			public bool IsUniqueCode(string code)
			{
				throw new NotImplementedException();
			}

			public int OrderItemGiftCardCount(int orderItemID)
			{
				throw new NotImplementedException();
			}

			public IGiftCard FindByCodeAndCurrency(string code, int currencyID)
			{
				throw new NotImplementedException();
			}

			public decimal? GetBalanceWithPendingPayments(string code, IOrder order)
			{
				throw new NotImplementedException();
			}

			public bool Update(IGiftCard giftCard)
			{
				throw new NotImplementedException();
			}

			public int GetCurrencyIDByCode(string currencyCode)
			{
				Contract.Requires(!string.IsNullOrWhiteSpace(currencyCode), "currencyCode required");

				throw new NotImplementedException();
			}
		}
	}
}