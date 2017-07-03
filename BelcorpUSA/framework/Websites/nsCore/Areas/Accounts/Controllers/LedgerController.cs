using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using nsCore.Areas.Accounts.Models.Ledger;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities.Business.Logic;

namespace nsCore.Areas.Accounts.Controllers
{
	public class LedgerController : BaseAccountsController
	{
		private readonly IAccountLedgerService _accountLedgerService = Create.New<IAccountLedgerService>();
		private readonly IProductCreditLedgerService _productCreditLedgerService = Create.New<IProductCreditLedgerService>();
		private readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		#region Commission Ledger
		[FunctionFilter("Accounts-Commission Ledger", "~/Accounts/Overview")]
		public virtual ActionResult Index(string id)
		{
			try
			{
				AccountNum = id;
                Session["UbicLedgerPopup"] = "A";
				return View();
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[OutputCache(CacheProfile = "PagedGridData")]
		[FunctionFilter("Accounts-Commission Ledger", "~/Accounts/Overview")]
		public virtual ActionResult Get(int page, int pageSize)
		{
			try
			{
                //var accountLedgers = _accountLedgerService.RetrieveLedger(CoreContext.CurrentAccount.AccountID);
                //var entries = accountLedgers.OrderByDescending(le => le.EffectiveDate).ThenByDescending(le => le.EntryId);

                var entries = LedgerExtension.ListAccountLedger(CoreContext.CurrentAccount.AccountID);
				if (entries.Any())
				{
					var builder = new StringBuilder();

					var count = 0;
					foreach (var entry in entries.Skip(page * pageSize).Take(pageSize))
					{
						// The commissions team removed the CurrencyTypes table from the commissions database.
						// The Ledger tables now point to the Core database's Currencies table. 5/21/2013
						var currency = SmallCollectionCache.Instance.Currencies.FirstOrDefault(c => c.CurrencyID == entry.CurrencyTypeID);
						if (currency == null)
						{
                            throw new NullReferenceException(string.Format("failed to find currency {0}", entry.CurrencyTypeID));
						}

						var entryAmount = entry.EntryAmount;
						var description = entry.EntryDescription;


                        var reasonTerm = _productCreditLedgerService.GetEntryReason(entry.EntryReasonID).TermName;
                        var kindTerm = _productCreditLedgerService.GetEntryKind(entry.EntryTypeID).TermName;                     
                        var bonusKind = _commissionsService.GetBonusKind(entry.BonusTypeID.ToString());
                        var EntryOriginName = _productCreditLedgerService.GetEntryOrigin(entry.EntryOriginID).TermName;
						var bonusTypeTerm = bonusKind != null ? bonusKind.TermName : string.Empty;

                        // inicio 06042017 comentado por IPN la sentencia solo formate decimales para brazil
						//var endingBalance = entry.EndingBalance.ToString("C", currency.Culture);
                        //fin 060042017
                        var endingBalance = entry.EndingBalance.ToString("C", CoreContext.CurrentCultureInfo);
                        var entryDate = entry.EntryDate;
                        var reg = AccountPropertiesBusinessLogic.GetValueByID(7, entry.UserID);
                        string userName = reg == null ? "" : reg.name;


						builder
							.Append("<tr class=\"")
							.Append(count % 2 == 1 ? "Alt" : string.Empty)
                            .Append(entry.EndingBalance < 0 ? " Negative" : " Positive")
							.Append("\">")
							.AppendCell(description)
							.AppendCell(reasonTerm)
                            .AppendCell(EntryOriginName)
							.AppendCell(kindTerm)
                            .AppendCell(entry.EffectiveDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                            .AppendCell(userName)
                            // inicio 06042017 comentado por IPN la sentencia solo formate decimales para brazil
                            //.AppendCell(entryAmount.ToString("C", currency.Culture))
                            //fin 060042017

                            // inicio 06042017 agregado por IPN se agrego para formatear los decimales de acuerdo al tipo de idioma
                            .AppendCell(entryAmount.ToString("C", CoreContext.CurrentCultureInfo))
                            // fin  06042017


                         
                            .AppendCell(endingBalance)
                        
                            .AppendCell(entryDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))                          
                            .AppendCell(bonusTypeTerm)

							.Append("</td></tr>");
						++count;
					}
					return Json(new { totalPages = Math.Ceiling(entries.Count() / (double)pageSize), page = builder.ToString() });
				}
				return Json(new { totalPages = 0, page = "<tr><td colspan=\"7\">" + Translation.GetTerm("NoLedgerEntries", "No ledger entries.") + "</td></tr>" });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Accounts-Add Commission Ledger Entry", "~/Accounts/Ledger")]
		public virtual ActionResult Add(string _entryAmount, string effectiveDate, string entryDescription, int entryReason, int entryType, int? bonusType, string notes, string _currentEndingBalance, int orderID, string supportTicketNumber)
		{
			try
			{

                decimal entryAmount = 0;
                decimal currentEndingBalance = 0;

                var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
                if (KeyDecimals == "ES")
                {
                    var culture = CultureInfoCache.GetCultureInfo("En");
                    entryAmount = Convert.ToDecimal(_entryAmount, culture);
                    currentEndingBalance = Convert.ToDecimal(_currentEndingBalance, culture);
                }


                _commissionsService.SavePartialAccountToCommission(CoreContext.CurrentAccount.AccountID, CoreContext.CurrentAccount.AccountNumber);
                DateTime effectiveDate_ = DateTime.Now;
                var newEntry = Create.New<IAccountLedgerEntry>();
                newEntry.EntryAmount = entryAmount;
                newEntry.EffectiveDate = effectiveDate_;
                newEntry.EntryDescription = entryDescription;
                newEntry.EntryDate = DateTime.Now;
                newEntry.AccountId = CoreContext.CurrentAccount.AccountID;
                newEntry.EntryOrigin = _accountLedgerService.GetEntryOrigins().FirstOrDefault(x => x.Code == "ME"); //Manual Entry
                newEntry.EntryReason = _accountLedgerService.GetEntryReasons().FirstOrDefault(x => x.EntryReasonId == entryReason);
                newEntry.EntryKind = _accountLedgerService.GetEntryKinds().FirstOrDefault(x => x.LedgerEntryKindId == entryType);
                newEntry.BonusTypeId = bonusType;
                newEntry.EntryNotes = notes;
                newEntry.CurrencyTypeId = 1;
                var mainAddress = CoreContext.CurrentAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Main);
                if (mainAddress == null)
                {
                    throw new NetSteps.Common.Exceptions.NetStepsApplicationException("No main address")
                    {
                        PublicMessage = Translation.GetTerm("NoMainAddress", "No main address")
                    };
                }
                var currency = SmallCollectionCache.Instance.Currencies.GetById(SmallCollectionCache.Instance.Countries.GetById(mainAddress.CountryID).CurrencyID);

              
                _accountLedgerService.AddLedgerEntry(newEntry);


                return Json(new
                {
                    result = true,
                    entryAmount = entryAmount.ToString("C", currency.Culture),
                    endingBalance = (currentEndingBalance + entryAmount).ToString("C", currency.Culture)
                });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Accounts-Add Commission Ledger Entry", "~/Accounts/Ledger")]
		public virtual ActionResult EntryForm()
		{
			var model = new LedgerEntryFormModel();
			return PartialView(model);
		}
		#endregion

		#region Product Credit Ledger

		[FunctionFilter("Accounts-Product Credit Ledger", "~/Accounts/Ledger")]
		public virtual ActionResult ProductCredit(string id)
		{
			AccountNum = id;
            Session["UbicLedgerPopup"] = "P";
			return View();
		}

		[OutputCache(CacheProfile = "PagedGridData")]
		[FunctionFilter("Accounts-Product Credit Ledger", "~/Accounts/Ledger")]
		public virtual ActionResult GetProductCredit(int page, int pageSize)
		{
			try
			{
				var accountLedgers = _productCreditLedgerService.RetrieveLedger(CoreContext.CurrentAccount.AccountID);
                IEnumerable<IProductCreditLedgerEntry> entries = accountLedgers.OrderByDescending(le => le.EntryId);//.ThenByDescending(le => le.EntryId);
				if (entries.Any())
				{
					var builder = new StringBuilder();

					// TODO: AccountLedger should have a CurrencyID for formatting currency, right? - JHE
					int count = 0;
					foreach (var entry in entries.Skip(page * pageSize).Take(pageSize))
					{
						// The commissions team removed the CurrencyTypes table from the commissions database.
						// The Ledger tables now point to the Core database's Currencies table. 5/21/2013
						var currency = SmallCollectionCache.Instance.Currencies.FirstOrDefault(c => c.CurrencyID == entry.CurrencyTypeId);
						var reason = _productCreditLedgerService.GetEntryReason(entry.EntryReason.EntryReasonId);
						var entryKind = _productCreditLedgerService.GetEntryKind(entry.EntryKind.LedgerEntryKindId);
                        //entryKind=entryKind.repl
						var bonusKind = _commissionsService.GetBonusKind(entry.BonusTypeId.GetValueOrDefault());
                        var origin = _productCreditLedgerService.GetEntryOrigin(entry.EntryOrigin.EntryOriginId);
                        var reg = AccountPropertiesBusinessLogic.GetValueByID(7, entry.UserId);
                        string userName = reg == null ? "" : reg.name;

                        var cultureInfoApp = CoreContext.CurrentCultureInfo;

						builder.Append("<tr class=\"")
							.Append(count % 2 == 1 ? "Alt" : "")
							//.Append(entry.EntryAmount < 0 ? " Negative" : " Positive")
                             .Append(entry.EndingBalance.ToDecimal() < 0 ? " Negative" : " Positive")
							.Append("\">")
							 .AppendCell(entry.EntryDescription)
							 .AppendCell(reason != null ? reason.Name : String.Empty)
                             .AppendCell(origin != null ? origin.Name : String.Empty)
							 .AppendCell(entryKind != null ? entryKind.Name : String.Empty)
                            .AppendCell(entry.EffectiveDate.ToString())

                            // .AppendCell(entry.EffectiveDate.ToString("dd/MM/yyyy"))//.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                              //username
                             .AppendCell(userName)

                             //  15042017 comentado por IPN
                                //.AppendCell(entry.EntryAmount.ToString("C", currency.Culture), style: "width:60px")
                                //.AppendCell(entry.EndingBalance.ToDecimal().ToString("C", currency.Culture), style: "width:60px")
                             // fin 15042017
                            .AppendCell(entry.EntryAmount.ToString("C", cultureInfoApp), style: "width:60px")
                            .AppendCell(entry.EndingBalance.ToDecimal().ToString("C", cultureInfoApp), style: "width:60px")
                             .AppendCell(entry.OrderPaymentId.ToString())
                             .AppendCell(entry.OrderId.ToString())
                             .AppendCell(bonusKind != null ? bonusKind.TermName : String.Empty)
                             //.AppendCell(entry.tic
							 .Append("</td></tr>");
						++count;
					}
					return Json(new { totalPages = Math.Ceiling(entries.Count() / (double)pageSize), page = builder.ToString() });
				}
				return Json(new { totalPages = 0, page = "<tr><td colspan=\"7\">" + Translation.GetTerm("NoLedgerEntries", "No ledger entries.") + "</td></tr>" });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Accounts-Add Product Credit Ledger Entry", "~/Accounts/Ledger")]
        public virtual ActionResult AddProductCredit(string _entryAmount, string effectiveDate, string entryDescription, int entryReason,
            int entryType, int? bonusType, string notes, string _currentEndingBalance, int orderID, string supportTicketNumber)
		{
			try
			{
                decimal entryAmount = 0; decimal currentEndingBalance = 0;
                var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
                if (KeyDecimals == "ES")
                {
                    var culture = CultureInfoCache.GetCultureInfo("En");
                    entryAmount = Convert.ToDecimal(_entryAmount, culture);
                    currentEndingBalance = Convert.ToDecimal(_currentEndingBalance, culture);
                }
               

				var mainAddress = CoreContext.CurrentAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Main);
				if (mainAddress == null)
				{
					//If no main address, find the sponsor for an address
					var sponsorAccount = NetSteps.Data.Entities.Account.LoadByAccountNumberFull(CoreContext.CurrentAccount.SponsorInfo.AccountNumber);
					mainAddress = sponsorAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Main);
					if (mainAddress == null)
					{
						throw new NetSteps.Common.Exceptions.NetStepsApplicationException("No main address")
						{
							PublicMessage = Translation.GetTerm("NoMainAddress", "No main address")
						};
					}
				}
				var currency = SmallCollectionCache.Instance.Currencies.GetById(SmallCollectionCache.Instance.Countries.GetById(mainAddress.CountryID).CurrencyID);

				_commissionsService.SavePartialAccountToCommission(CoreContext.CurrentAccount.AccountID, CoreContext.CurrentAccount.AccountNumber);
                //_productCreditLedgerService.AddLedgerEntry(
                //    CoreContext.CurrentAccount.AccountID
                //    , entryAmount
                //    , effectiveDate
                //    , entryDescription
                //    , entryReason
                //    , entryType
                //    , bonusType.GetValueOrDefault()
                //    , notes
                //    , currency.CurrencyID
                //    , CoreContext.CurrentAccount.UserID.GetValueOrDefault());
                DateTime effectiveDate_ = DateTime.Now;
                var EntryOrigin   = _accountLedgerService.GetEntryOrigins().FirstOrDefault(x => x.Code == "ME").EntryOriginId; //Manual Entry
                LedgerExtension.ProductCreditLedger model = new LedgerExtension.ProductCreditLedger();
                model.AccountID = CoreContext.CurrentAccount.AccountID;
                model.EntryDescription = entryDescription;
                model.EntryReasonID = entryReason;
                model.EntryOriginID = EntryOrigin;
                model.EntryTypeID = entryType;
                //model.UserID = CoreContext.CurrentAccount.UserID.GetValueOrDefault();
                model.EntryAmount = entryAmount;
                model.EntryDate =  effectiveDate_;
                model.EffectiveDate = effectiveDate_;
                model.BonusTypeID = bonusType;
                model.OrderID = orderID;
                model.SupportTicketID = supportTicketNumber.ToInt();// LedgerExtension.GetSupporTicketByID(supportTicketNumber);
                model.UserID = CoreContext.CurrentUser.UserID;

                var ProductCreditLedgerVal = LedgerExtension.InsProductCreditLedger(model);


				return Json(new
				{
					result = true,
					entryAmount = entryAmount.ToString("C", currency.Culture),
					endingBalance = (currentEndingBalance + entryAmount).ToString("C", currency.Culture)
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion
	}
}