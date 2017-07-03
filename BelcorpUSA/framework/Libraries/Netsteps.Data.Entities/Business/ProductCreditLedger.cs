using System;
using System.Collections.Generic;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Commissions
{
    public partial class ProductCreditLedger
    {
        public static List<ProductCreditLedger> LoadByAccountID(int accountID)
        {
            try
            {
                return Repository.LoadByAccountID(accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void SaveTemporaryAccountToCommission(int accountID, string accountNumber)
        {
            try
            {
                Repository.SaveTemporaryAccountToCommission(accountID, accountNumber);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static decimal GetCurrentBalance(int accountID)
        {
            try
            {
				return Repository.GetBalance(accountID, null, null, null);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static decimal GetCurrentBalance(int accountID, LedgerEntryType entryType)
        {
            try
            {
                return Repository.GetBalance(accountID, null, null, entryType.LedgerEntryTypeID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static decimal GetCurrentBalance(int accountID, DateTime? effectiveDate, int? entryID)
        {
            try
            {
                return Repository.GetBalance(accountID, effectiveDate, entryID, null);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static decimal GetCurrentBalanceLessPendingPayments(int accountID)
        {
            try
            {
                return Repository.GetBalanceLessPendingPayments(accountID, null, null);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static decimal GetCurrentBalanceLessPendingPayments(int accountID, DateTime? effectiveDate, int? entryID, int? ignoreOrderPaymentID = null)
        {
            try
            {
                return Repository.GetBalanceLessPendingPayments(accountID, effectiveDate, entryID, ignoreOrderPaymentID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void AddProductCredit(int accountID, Address mainAddress, decimal entryAmount, DateTime effectiveDate, string entryDescription, int entryReason, int entryType, int? bonusType, string notes, decimal currentEndingBalance, int? orderID = null, int? orderPaymentID = null, int? userID = null, int? currencyTypeID = null)
        {
            ProductCreditLedger newEntry = new ProductCreditLedger();
            newEntry.StartEntityTracking();
            newEntry.EntryAmount = entryAmount;
            newEntry.EffectiveDate = effectiveDate;
            newEntry.EntryDescription = entryDescription;
            newEntry.EntryDate = DateTime.Now;
            newEntry.AccountID = accountID;
            newEntry.EntryOriginID = SmallCollectionCache.Instance.LedgerEntryOrigins.GetByCode("ME").EntryOriginID; //Manual Entry
            newEntry.EntryReasonID = entryReason;
            newEntry.EntryTypeID = entryType;
            newEntry.BonusTypeID = bonusType;
            newEntry.EntryNotes = notes;
            newEntry.OrderID = orderID;
            newEntry.OrderPaymentID = orderPaymentID;
            newEntry.UserID = userID.ToInt();
            newEntry.CurrencyTypeID = currencyTypeID
				?? SmallCollectionCache.Instance.Countries.GetById(
						SmallCollectionCache.Instance.Markets.GetById(
							Entities.Account.Load(accountID).MarketID)
						.GetDefaultCountryID())
					.CurrencyID;

            if (mainAddress == null)
                throw new NetSteps.Common.Exceptions.NetStepsApplicationException("No main address")
                {
                    PublicMessage = Translation.GetTerm("NoMainAddress", "No main address")
                };

			var currency = SmallCollectionCache.Instance.Currencies.GetById(SmallCollectionCache.Instance.Countries.GetById(mainAddress.CountryID).CurrencyID);
			// The commissions team removed the CurrencyTypes table from the commissions database.
			// The Ledger tables now point to the Core database's Currencies table. 5/21/2013
			newEntry.CurrencyTypeID = currency.CurrencyID;

            try
            {
                newEntry.Save();
            }
            catch (Exception ex)
            {
                throw new NetSteps.Common.Exceptions.NetStepsApplicationException("Could Not Add Product Credit", ex)
                {
                    PublicMessage = Translation.GetTerm("CouldNotAddProductCredit", "Could Not Add Product Credit")
                };
            }
        }
    }
}
