using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Extensions;

namespace nsDistributor.Areas.Enroll.Models.Shared
{
    using NetSteps.Auth.UI.Common;
    using NetSteps.Data.Entities.Services;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.Enrollment.Common.Models.Context;
    using NetSteps.Commissions.Common.Models;
    using NetSteps.Commissions.Common;

    public class SummaryModel
    {
        #region Resources
        public virtual bool ShowSponsorEditLink { get; set; }
        public virtual bool ShowEditLinks { get; set; }
        public virtual bool ShowShippingEditLink { get; set; }
        public virtual bool ShowSponsor { get; set; }
        public virtual bool ShowDisbursementProfiles { get; protected set; }

        public virtual string UserName { get; set; }
        public virtual string FullName { get; set; }
        public virtual string MailAccountEmail { get; set; }
        public virtual string PersonalEmail { get; set; }
        public virtual string Language { get; set; }
        public virtual string PWSUrl { get; set; }
        public virtual HtmlString SponsorPhotoHtml { get; set; }
        public virtual string SponsorFullName { get; set; }
        public virtual string SponsorText { get; set; }
        public virtual Account EnrollingAccount { get; set; }

        public virtual MvcHtmlString MainAddressHtml { get; set; }
        public virtual MvcHtmlString ShippingAddressHtml { get; set; }
        public virtual MvcHtmlString BillingAddressHtml { get; set; }

        public virtual string NameOnCard { get; set; }
        public virtual DateTime? CreditCardExpirationDate { get; set; }
        public virtual string MaskedCreditCardNumber { get; set; }

        public virtual int CurrencyID { get; set; }
        public virtual Order InitialOrder { get; set; }
        public virtual AutoshipOrder AutoshipOrder { get; set; }
        public virtual AutoshipOrder SiteSubscriptionAutoshipOrder { get; set; }

        //public virtual IList<Commissions.DisbursementProfile> DisbursementProfiles { get; protected set; }
        public virtual MvcHtmlString DisbursementHtml { get; protected set; }

        public virtual string EditInitialOrderController { get; set; }
        public virtual string EditInitialOrderAction { get; set; }
        public virtual string EditAutoshipController { get; set; }
        public virtual string EditAutoshipAction { get; set; }
        public virtual string EditDisbursementProfiles { get; set; }
        #endregion

        #region Helpers
        public virtual bool ShowInitialOrder
        {
            get
            {
                return this.InitialOrder != null;
            }
        }

        public virtual bool ShowAutoshipOrder
        {
            get
            {
                return this.AutoshipOrder != null;
            }
        }

        public virtual bool ShowSubscriptionOrder
        {
            get
            {
                return this.SiteSubscriptionAutoshipOrder != null;
            }
        }

        public virtual bool ShowOrderSummary
        {
            get
            {
                return ShowInitialOrder || ShowAutoshipOrder || ShowSubscriptionOrder;
            }
        }

        public virtual bool ShowEditInitialOrderLink
        {
            get
            {
                return this.ShowEditLinks
                    && !string.IsNullOrWhiteSpace(this.EditInitialOrderController)
                    && !string.IsNullOrWhiteSpace(this.EditInitialOrderAction);
            }
        }

        public virtual bool ShowEditAutoshipLink
        {
            get
            {
                return this.ShowEditLinks
                    && !string.IsNullOrWhiteSpace(this.EditAutoshipController)
                    && !string.IsNullOrWhiteSpace(this.EditAutoshipAction);
            }
        }

		public virtual bool DisplayUsername
		{
			get
			{
				var authUIService = Create.New<IAuthenticationUIService>();
			    return authUIService.GetConfiguration().ShowUsernameFormFields;
			}
		}
        #endregion

        #region Infrastructure
        public virtual SummaryModel LoadResources(
            bool showSponsorEditLink,
            bool showEditLinks,
            bool showSponsor,
            IEnrollmentContext enrollmentContext,
            string pwsUrl,
            bool showShippingEditLink = true,
            bool showInitialOrder = true,
            bool showAutoshipOrder = true,
            bool showSubscriptionAutoshipOrder = true,
            bool showDisbursementProfiles = false,
            string editInitialOrderController = null,
            string editInitialOrderAction = null,
            string editAutoshipController = null,
            string editAutoshipAction = null,
            string editDisbursementProfiles = null)
        {
            var account = (Account)enrollmentContext.EnrollingAccount;
            
            this.ShowEditLinks = showEditLinks;
            this.ShowShippingEditLink = showShippingEditLink;
            this.ShowSponsor = showSponsor;
            this.ShowDisbursementProfiles = showDisbursementProfiles;

            this.FullName = account.FullName;
            this.UserName = (account.User == null) ? string.Empty : account.User.Username;
            if (account.MailAccounts != null && account.MailAccounts.Count > 0)
                this.MailAccountEmail = account.MailAccounts[0].EmailAddress;
            else
                this.MailAccountEmail = string.Empty;
            this.PersonalEmail = account.EmailAddress;
            this.Language = SmallCollectionCache.Instance.Languages.GetById(account.DefaultLanguageID).GetTerm();
            this.PWSUrl = pwsUrl;
            this.EnrollingAccount = account;
            // Get sponsor's photo
            try
            {
                // TODO: Check sponsor for null
                var sponsorSite = Site.LoadByAccountID(account.SponsorID.Value).FirstOrDefault();
                if (sponsorSite != null)
                {
                    var photoHtmlSection = sponsorSite.GetHtmlSectionByName("MyPhoto");
                    if (photoHtmlSection != null)
                    {
                        this.SponsorPhotoHtml = new HtmlString(photoHtmlSection.ToDisplay(sponsorSite, Constants.ViewingMode.Production));
                    }
                }
            }
            catch (Exception ex)
            {
                // Log and continue without photo
                ex.Log();
            }

            this.SponsorFullName = ((Account)enrollmentContext.Sponsor).FullName;
            this.ShowSponsorEditLink = showSponsorEditLink;
            this.SponsorText = showSponsorEditLink
                ? Translation.GetTerm("IsCurrentlySelectedAsYourSponsor", "is currently selected as your sponsor")
                : Translation.GetTerm("IsYourSponsor", "is your sponsor");

            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main);
            if (mainAddress != null)
                this.MainAddressHtml = mainAddress.ToDisplay(IAddressExtensions.AddressDisplayTypes.Web).ToMvcHtmlString();
            var shippingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);
            if (shippingAddress != null)
                this.ShippingAddressHtml = shippingAddress.ToDisplay(IAddressExtensions.AddressDisplayTypes.Web).ToMvcHtmlString();
            var billingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Billing);
            if (billingAddress != null)
                this.BillingAddressHtml = billingAddress.ToDisplay(IAddressExtensions.AddressDisplayTypes.Web).ToMvcHtmlString();

            var accountPaymentMethod = account.AccountPaymentMethods
                .OrderByDescending(x => x.IsDefault)
                .FirstOrDefault();
            if (accountPaymentMethod != null)
            {
                this.NameOnCard = accountPaymentMethod.NameOnCard;
                this.CreditCardExpirationDate = accountPaymentMethod.ExpirationDate;
                this.MaskedCreditCardNumber = accountPaymentMethod.MaskedAccountNumber;
            }

            // Set currency
            if (enrollmentContext.InitialOrder != null)
            {
                this.CurrencyID = enrollmentContext.InitialOrder.CurrencyID;
            }
            else if (enrollmentContext.AutoshipOrder != null && ((AutoshipOrder)enrollmentContext.AutoshipOrder).Order != null)
            {
                this.CurrencyID = ((AutoshipOrder)enrollmentContext.AutoshipOrder).Order.CurrencyID;
            }
            else this.CurrencyID = enrollmentContext.CurrencyID;

            if (showInitialOrder
                && enrollmentContext.InitialOrder != null
                && enrollmentContext.InitialOrder.OrderCustomers.Any()
                && ((Order)enrollmentContext.InitialOrder).OrderCustomers[0].OrderItems.Any())
            {
            //string OrderNumber = PreOrderExtension.GetOrderNumberByAccount(account.AccountID, 11, 4);
            this.InitialOrder = Order.LoadByOrderNumberFull(Convert.ToString(enrollmentContext.InitialOrder.OrderID));  
            }
            else
            {
                this.InitialOrder = null;
            }

            if (showAutoshipOrder
                && enrollmentContext.AutoshipOrder != null
                && ((AutoshipOrder)enrollmentContext.AutoshipOrder).Order != null
                && ((AutoshipOrder)enrollmentContext.AutoshipOrder).Order.OrderCustomers.Any()
                && ((AutoshipOrder)enrollmentContext.AutoshipOrder).Order.OrderCustomers[0].OrderItems.Any())
            {
                this.AutoshipOrder = (AutoshipOrder)enrollmentContext.AutoshipOrder;
            }
            else
            {
                this.AutoshipOrder = null;
            }

            if (showSubscriptionAutoshipOrder
                && enrollmentContext.SiteSubscriptionAutoshipOrder != null
                && ((AutoshipOrder)enrollmentContext.SiteSubscriptionAutoshipOrder).Order != null
                && ((AutoshipOrder)enrollmentContext.SiteSubscriptionAutoshipOrder).Order.OrderCustomers.Any()
                && ((AutoshipOrder)enrollmentContext.SiteSubscriptionAutoshipOrder).Order.OrderCustomers[0].OrderItems.Any())
            {
                this.SiteSubscriptionAutoshipOrder = (AutoshipOrder)enrollmentContext.SiteSubscriptionAutoshipOrder;
            }
            else
            {
                this.SiteSubscriptionAutoshipOrder = null;
            }

            if (showDisbursementProfiles)
            {
                var service = Create.New<ICommissionsService>();
                var disbursementProfiles = service.GetDisbursementProfilesByAccountId(account.AccountID).Where(x => x.IsEnabled == true).ToList();
                if (disbursementProfiles.Count > 0)
                {
                    var disbursementBuilder = new StringBuilder();

                    var disbursementMethod = disbursementProfiles[0].DisbursementMethod;
                    if (disbursementMethod == DisbursementMethodKind.Check)
                    {
                        var checkDisbursement = (ICheckDisbursementProfile)disbursementProfiles[0];
                        disbursementBuilder.AppendLine(string.Format("<span>{0} Check <br /></span>", Translation.GetTerm("EnrollmentReview_DisbursementInfoType", "Type: ")));
                        disbursementBuilder.Append(Address.Load(checkDisbursement.AddressId).ToDisplay(IAddressExtensions.AddressDisplayTypes.Web));
                    }
                    else if (disbursementMethod == DisbursementMethodKind.EFT)
                    {
                        var eftDisbursement = (IEFTDisbursementProfile)disbursementProfiles[0];
                        disbursementBuilder.AppendLine(string.Format("<span class=\"block mb5\">{0} EFT</span>", Translation.GetTerm("EnrollmentReview_DisbursementInfoType", "Type: ")));
                        foreach (var profile in disbursementProfiles)
                        {
                            disbursementBuilder.AppendLine("<div class=\"mb5 eftAcctReview\">");
                            disbursementBuilder.AppendLine(string.Format("{0} <br />", eftDisbursement.BankName));
                            disbursementBuilder.AppendLine(string.Format("{0} {1}", Translation.GetTerm("EnrollmentReview_DisbursementInfoAccount", "Account:"), eftDisbursement.AccountNumber.MaskString(4)));
                            disbursementBuilder.AppendLine("</div>");
                        }
                    }

                    this.DisbursementHtml = disbursementBuilder.ToString().ToMvcHtmlString();
                }
                else
                {
                    this.DisbursementHtml = string.Empty.ToMvcHtmlString();
                }
            }

            this.EditInitialOrderController = editInitialOrderController;
            this.EditInitialOrderAction = editInitialOrderAction;
            this.EditAutoshipController = editAutoshipController;
            this.EditAutoshipAction = editAutoshipAction;
            this.EditDisbursementProfiles = editDisbursementProfiles;

            return this;
        }
        #endregion
    }
}