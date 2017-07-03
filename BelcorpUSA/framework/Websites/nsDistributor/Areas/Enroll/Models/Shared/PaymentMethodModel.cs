using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Extensions;

namespace nsDistributor.Areas.Enroll.Models.Shared
{
    public class PaymentMethodModel
    {
        #region Values
        [NSDisplayName("PaymentType", "Payment Type")]
        public virtual int PaymentTypeID { get; set; }

        [NSDisplayName("NameOnCard", "Name on Card")]
        [NSRequired(Condition = "EnableCreditCardValidation")]
        public virtual string NameOnCard { get; set; }

        [NSDisplayName("CardNumber", "Card Number")]
        [NSRequired(Condition = "EnableCreditCardValidation")]
        [NSCreditCard(Condition = "EnableCreditCardValidation", DisableClientValidation = true)]
        // Added DisableClientValidation to prevent client side brute-force validation for CC # check
        // Long-term we would like to authorize the card for $1 to prevent fraudulent card #'s and re-enable 
        // client-side validations for CC#
        public virtual string CreditCardNumber { get; set; }

        [NSDisplayName("ExpirationDate", "Expiration Date")]
        [NSRange(1, 12, TermName = "ErrorExpirationDateInvalid", ErrorMessage = "Expiration Date is invalid.", Condition = "EnableCreditCardValidation")]
        [NSRequired(Condition = "EnableCreditCardValidation")]
        public virtual int? CreditCardMonth { get; set; }

        [NSDisplayName("ExpirationDate", "Expiration Date")]
        [NSRange(1900, 2999, TermName = "ErrorExpirationDateInvalid", ErrorMessage = "Expiration Date is invalid.", Condition = "EnableCreditCardValidation")]
        [NSRequired(Condition = "EnableCreditCardValidation")]
        public virtual int? CreditCardYear { get; set; }

        [NSDisplayName("CardNumber", "Card Number")]
        public virtual string MaskedCreditCardNumber
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.CreditCardNumber) ? "" : this.CreditCardNumber.MaskString(4);
            }
        }

		public virtual int LanguageID { get; set; }


		public virtual System.Globalization.CultureInfo EnrollmentCultureInfo
		{
			get
			{
				var language = NetSteps.Data.Entities.Cache.SmallCollectionCache.Instance.Languages.FirstOrDefault(x => x.LanguageID == this.LanguageID);
				if (language == null)
				{
					language = CoreContext.CurrentLanguage;
				}
				return language.Culture;
			}
		}
        #endregion

        #region Resources
        public virtual IEnumerable<SelectListItem> PaymentTypes { get; set; }

        public virtual IEnumerable<SelectListItem> CreditCardMonths
        {
            get
            {
                return Enumerable.Range(1, 12)
                    .Select(x => new SelectListItem
                    {
                        Value = x.ToString(),
                        Text = string.Format("{0:D2} - {1}",
                               x,
                               CoreContext.CurrentCultureInfo.TextInfo.ToTitleCase(new DateTime(1900, x, 1).ToString("MMMM", this.EnrollmentCultureInfo)))
                    });
            }
        }

        protected IEnumerable<SelectListItem> _creditCardYears;
        public virtual IEnumerable<SelectListItem> CreditCardYears
        {
            get
            {
                if (_creditCardYears == null)
                {
                    _creditCardYears = Enumerable.Range(DateTime.Now.Year, 10)
                        .Select(x => new SelectListItem
                        {
                            Value = x.ToString(),
                            Text = x.ToString()
                        });
                }
                return _creditCardYears;
            }
        }
        #endregion

        #region Infrastructure
        public PaymentMethodModel()
        {
            // Value type defaults - will be overridden by LoadValues() and model binder.
            this.PaymentTypeID = (int)Constants.PaymentType.CreditCard;
        }

        /// <summary>
        /// Validation Rules
        /// </summary>
        public static readonly Predicate<PaymentMethodModel>
            EnableCreditCardValidation = m => m.PaymentTypeID == (int)Constants.PaymentType.CreditCard;

        /// <summary>
        /// Combines <see cref="CreditCardMonth"/> and <see cref="CreditCardYear"/> into a <see cref="DateTime"/> value.
        /// </summary>
        [NSFutureValidDate(TermName = "ErrorExpirationDateInvalid", ErrorMessage = "Expiration Date is invalid.", Condition = "EnableCreditCardValidation")]
        public virtual DateTime? CreditCardExpirationDate
        {
            get
            {
                if (this.CreditCardMonth == null || this.CreditCardYear == null)
                {
                    return null;
                }

                return new DateTime(this.CreditCardYear.Value, this.CreditCardMonth.Value, DateTime.DaysInMonth(this.CreditCardYear.Value, this.CreditCardMonth.Value));
            }
        }

        public virtual PaymentMethodModel LoadValues(
            IPayment paymentMethod,
			int languageID)
        {
            this.PaymentTypeID = paymentMethod.PaymentTypeID;
			this.LanguageID = languageID;

            if (this.PaymentTypeID == (int)Constants.PaymentType.CreditCard)
            {
                this.NameOnCard = paymentMethod.NameOnCard;
                this.CreditCardNumber = paymentMethod.DecryptedAccountNumber;
                if (paymentMethod.ExpirationDate.HasValue)
                {
                    this.CreditCardMonth = paymentMethod.ExpirationDate.Value.Month;
                    this.CreditCardYear = paymentMethod.ExpirationDate.Value.Year;
                }
                else
                {
                    this.CreditCardMonth = this.CreditCardYear = null;
                }
            }

            return this;
        }

        public virtual PaymentMethodModel LoadResources(
            IEnumerable<PaymentType> paymentTypes)
        {
            this.PaymentTypes = paymentTypes
                .Select(x => new SelectListItem
                {
                    Text = x.GetTerm(),
                    Value = x.PaymentTypeID.ToString()
                })
                .OrderBy(x => x.Text);

            return this;
        }

        public virtual PaymentMethodModel ApplyTo(
            IPayment paymentMethod)
        {
            paymentMethod.PaymentTypeID = this.PaymentTypeID;
            if (this.PaymentTypeID == (int)Constants.PaymentType.CreditCard)
            {
                paymentMethod.NameOnCard = this.NameOnCard;
                paymentMethod.DecryptedAccountNumber = this.CreditCardNumber.RemoveNonNumericCharacters();
                paymentMethod.ExpirationDate = this.CreditCardExpirationDate;
            }
            else
            {
                paymentMethod.NameOnCard = "";
                paymentMethod.DecryptedAccountNumber = "";
                paymentMethod.ExpirationDate = null;
            }

            return this;
        }
        #endregion
    }
}