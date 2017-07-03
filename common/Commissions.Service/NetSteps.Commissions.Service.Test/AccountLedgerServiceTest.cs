using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Commissions.Service.LedgerEntryKinds;
using NetSteps.Commissions.Service.LedgerEntryOrigins;
using NetSteps.Commissions.Service.LedgerEntryReasons;
using NetSteps.Commissions.Service.AccountLedgerEntries;
using NetSteps.Commissions.Common;
using NetSteps.Commissions.Common.Models;
using System.Transactions;

namespace NetSteps.Commissions.Service.Test
{
	[TestClass]
	public class AccountLedgerServiceTest
	{
		private AccountLedgerService GetLedgerService()
		{
			return new AccountLedgerService
				(
					new AccountLedgerEntryService(
							new AccountLedgerEntryProvider(new AccountLedgerEntryRepository
							(
								new LedgerEntryKindProvider(new LedgerEntryKindRepository()),
								new LedgerEntryOriginProvider(new LedgerEntryOriginRepository()),
								new LedgerEntryReasonProvider(new LedgerEntryReasonRepository())
							)),
							new LedgerEntryKindProvider(new LedgerEntryKindRepository()),
							new LedgerEntryOriginProvider(new LedgerEntryOriginRepository()),
							new LedgerEntryReasonProvider(new LedgerEntryReasonRepository())
						),
					new LedgerEntryKindService(new LedgerEntryKindProvider(new LedgerEntryKindRepository())),
					new LedgerEntryOriginService(new LedgerEntryOriginProvider(new LedgerEntryOriginRepository())),
					new LedgerEntryReasonService(new LedgerEntryReasonProvider(new LedgerEntryReasonRepository()))
				);
		}

		[TestMethod]
		public void AccountLedgerService_should_GetLedgerEntryKind_by_LedgerEntryKindId()
		{
			var LedgerEntryKindId = 4;

			var ledgerService = GetLedgerService();
			var ledgerEntryKind = ledgerService.GetEntryKind(LedgerEntryKindId);
			Assert.IsNotNull(ledgerEntryKind);
			Assert.AreEqual(4, ledgerEntryKind.LedgerEntryKindId);
			Assert.AreEqual("GA", ledgerEntryKind.Code);
			Assert.IsTrue(ledgerEntryKind.IsEnabled);
			Assert.IsTrue(ledgerEntryKind.IsEditable);
			Assert.IsFalse(ledgerEntryKind.IsTaxable);
			Assert.AreEqual("General Adjustment (Non Taxable)", ledgerEntryKind.Name);
			Assert.AreEqual("GeneralAdjustment(NonTaxable)", ledgerEntryKind.TermName);
		}

		[TestMethod]
		public void AccountLedgerService_should_GetLedgerEntryKind_by_LedgerEntryKindCode()
		{
			var ledgerEntryKindCode = "GT";

			var ledgerService = GetLedgerService();
			var ledgerEntryKind = ledgerService.GetEntryKind(ledgerEntryKindCode);
			Assert.IsNotNull(ledgerEntryKind);
			Assert.AreEqual(9, ledgerEntryKind.LedgerEntryKindId);
			Assert.AreEqual("GT", ledgerEntryKind.Code);
			Assert.IsTrue(ledgerEntryKind.IsEnabled);
			Assert.IsTrue(ledgerEntryKind.IsEditable);
			Assert.IsTrue(ledgerEntryKind.IsTaxable);
			Assert.AreEqual("General Adjustment (Taxable)", ledgerEntryKind.Name);
			Assert.AreEqual("GeneralAdjustment(Taxable)", ledgerEntryKind.TermName);
		}

		[TestMethod]
		public void AccountLedgerService_should_AddLedgerEntry()
		{
			using (var scope = new TransactionScope())
			{
				IAccountLedgerEntry ledgerEntry = new AccountLedgerEntry()
				{
					AccountId = 1,
					BonusTypeId = 7,
					BonusValueId = null,
					CurrencyTypeId = 1,
					EffectiveDate = DateTime.Now,
					EndingBalance = 1500M,
					EntryAmount = 15M,
					EntryDate = DateTime.Now,
					EntryDescription = "Test Entry",
					EntryKind = new LedgerEntryKindProvider(new LedgerEntryKindRepository()).First(),
					EntryNotes = "New Note",
					EntryOrigin = new LedgerEntryOriginProvider(new LedgerEntryOriginRepository()).First(),
					EntryReason = new LedgerEntryReasonProvider(new LedgerEntryReasonRepository()).First(),
					UserId = 1
				};

				var ledgerService = GetLedgerService();
				var saved = ledgerService.AddLedgerEntry(ledgerEntry);
				Assert.IsNotNull(saved);
				Assert.AreEqual(ledgerEntry.AccountId, saved.AccountId);
				Assert.AreNotEqual(0, saved.EntryId);

				scope.Dispose();
			}
		}

		[TestMethod]
		public void AccountLedgerService_should_AddLedgerEntryByValues()
		{
			using (var scope = new TransactionScope())
			{
				var ledgerService = GetLedgerService();
				var saved = ledgerService.AddLedgerEntry(1, 10M, DateTime.UtcNow, "Test", 1, 1, 1, "notes", 1, 1);
				Assert.IsNotNull(saved);
				Assert.AreEqual(1, saved.AccountId);
				Assert.AreNotEqual(0, saved.EntryId);

				scope.Dispose();
			}
		}

		[TestMethod]
		public void AccountLedgerService_should_GetCurrentBalance()
		{
			using (var scope = new TransactionScope())
			{
				IAccountLedgerEntry dummyLedgerEntry = new AccountLedgerEntry()
				{
					AccountId = 1,
					BonusTypeId = 7,
					BonusValueId = null,
					CurrencyTypeId = 1,
					EffectiveDate = DateTime.Now,
					EndingBalance = 1M,
					EntryAmount = 15M,
					EntryDate = DateTime.Now,
					EntryDescription = "Test Entry",
					EntryKind = new LedgerEntryKindProvider(new LedgerEntryKindRepository()).First(),
					EntryNotes = "New Note",
					EntryOrigin = new LedgerEntryOriginProvider(new LedgerEntryOriginRepository()).First(),
					EntryReason = new LedgerEntryReasonProvider(new LedgerEntryReasonRepository()).First(),
					UserId = 1
				};
				var accountId = 1;

				var ledgerService = GetLedgerService();

				try
				{
					ledgerService.AddLedgerEntry(dummyLedgerEntry);
				}
				catch { }

				var balance = ledgerService.GetCurrentBalance(accountId);
				Assert.AreEqual(dummyLedgerEntry.EntryAmount, balance);
				scope.Dispose();
			}
		}

		[TestMethod]
		public void AccountLedgerService_should_GetEntryOrigin()
		{
			var entryOriginId = 1;

			var ledgerService = GetLedgerService();
			var origin = ledgerService.GetEntryOrigin(entryOriginId);
			Assert.IsNotNull(origin);
			Assert.AreEqual("Global Management Portal", origin.Name);
			Assert.AreEqual("GlobalManagementPortal", origin.TermName);
			Assert.AreEqual("GMP", origin.Code);
			Assert.IsTrue(origin.IsEnabled);
			Assert.IsFalse(origin.IsEditable);
		}

		[TestMethod]
		public void AccountLedgerService_should_GetEntryOrigins()
		{
			var ledgerService = GetLedgerService();
			var ledgerEntryOrigins = ledgerService.GetEntryOrigins();
			foreach (var ledgerEntryOrigin in ledgerEntryOrigins)
			{
				Assert.IsNotNull(ledgerEntryOrigin);
				Assert.AreNotEqual(0, ledgerEntryOrigin.EntryOriginId);
			}
			Assert.AreEqual(4, ledgerEntryOrigins.Count());
		}

		[TestMethod]
		public void AccountLedgerService_should_GetEntryReason()
		{
			var entryReasonId = 3;

			var ledgerService = GetLedgerService();
			var reason = ledgerService.GetEntryReason(entryReasonId);
			Assert.IsNotNull(reason);
			Assert.AreEqual("Data Conversion/Import", reason.Name);
			Assert.AreEqual("DataConversion/Import", reason.TermName);
			Assert.AreEqual("DC", reason.Code);
			Assert.IsTrue(reason.IsEnabled);
			Assert.IsFalse(reason.IsEditable);
		}

		[TestMethod]
		public void AccountLedgerService_should_RetrieveLedger()
		{
			using (var scope = new TransactionScope())
			{
				IAccountLedgerEntry dummyLedgerEntry = new AccountLedgerEntry()
				{
					AccountId = 1,
					BonusTypeId = 7,
					BonusValueId = null,
					CurrencyTypeId = 1,
					EffectiveDate = DateTime.Now,
					EndingBalance = 1M,
					EntryAmount = 15M,
					EntryDate = DateTime.Now,
					EntryDescription = "Test Entry",
					EntryKind = new LedgerEntryKindProvider(new LedgerEntryKindRepository()).First(),
					EntryNotes = "New Note",
					EntryOrigin = new LedgerEntryOriginProvider(new LedgerEntryOriginRepository()).First(),
					EntryReason = new LedgerEntryReasonProvider(new LedgerEntryReasonRepository()).First(),
					UserId = 1
				};
				var accountId = 1;

				var ledgerService = GetLedgerService();

				try
				{
					ledgerService.AddLedgerEntry(dummyLedgerEntry);
				}
				catch { }

				var ledger = ledgerService.RetrieveLedger(accountId);
				foreach (var ledgerEntry in ledger)
				{
					Assert.IsNotNull(ledgerEntry);
					Assert.AreNotEqual(0, ledgerEntry.EntryId);
				}
				Assert.AreEqual(1, ledger.Count());
				scope.Dispose();
			}
		}

		[TestMethod]
		public void AccountLedgerService_should_GetEntryReasons()
		{
			var ledgerService = GetLedgerService();
			var ledgerEntryReasons = ledgerService.GetEntryReasons();
			foreach (var ledgerEntryReason in ledgerEntryReasons)
			{
				Assert.IsNotNull(ledgerEntryReason);
				Assert.AreNotEqual(0, ledgerEntryReason.EntryReasonId);
			}
			Assert.AreEqual(8, ledgerEntryReasons.Count());
		}


		[TestMethod]
		public void AccountLedgerService_should_GetEntryKinds()
		{
			var ledgerService = GetLedgerService();
			var ledgerEntryKinds = ledgerService.GetEntryKinds();
			foreach (var ledgerEntryKind in ledgerEntryKinds)
			{
				Assert.IsNotNull(ledgerEntryKind);
				Assert.AreNotEqual(0, ledgerEntryKind.LedgerEntryKindId);
			}
			Assert.AreEqual(11, ledgerEntryKinds.Count());
		}
	}
}
