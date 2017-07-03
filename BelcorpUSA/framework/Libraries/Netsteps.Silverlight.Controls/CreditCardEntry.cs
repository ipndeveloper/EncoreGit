using System.Windows;
using System.Windows.Controls;

namespace NetSteps.Silverlight.Controls
{
    public class CreditCardEntry : Control
    {
        public CreditCardEntry()
            : base()
        {
            this.DataContext = this;
            this.DefaultStyleKey = typeof(CreditCardEntry);
        }

        public static readonly DependencyProperty FirstNameProperty = DependencyProperty.Register("FirstName", typeof(string), typeof(CreditCardEntry), null);
        public string FirstName
        {
            get
            {
                return (string)GetValue(FirstNameProperty);
            }
            set
            {
                SetValue(FirstNameProperty, value);
            }
        }

        public static readonly DependencyProperty LastNameProperty = DependencyProperty.Register("LastName", typeof(string), typeof(CreditCardEntry), null);
        public string LastName
        {
            get
            {
                return (string)GetValue(LastNameProperty);
            }
            set
            {
                SetValue(LastNameProperty, value);
            }
        }

        public static readonly DependencyProperty ExpirationMonthProperty = DependencyProperty.Register("ExpirationMonth", typeof(string), typeof(CreditCardEntry), null);
        public string ExpirationMonth
        {
            get
            {
                return (string)GetValue(ExpirationMonthProperty);
            }
            set
            {
                SetValue(ExpirationMonthProperty, value);
            }
        }

        public static readonly DependencyProperty ExpirationYearProperty = DependencyProperty.Register("ExpirationYear", typeof(string), typeof(CreditCardEntry), null);
        public string ExpirationYear
        {
            get
            {
                return (string)GetValue(ExpirationYearProperty);
            }
            set
            {
                SetValue(ExpirationYearProperty, value);
            }
        }

        public static readonly DependencyProperty CVV2Property = DependencyProperty.Register("CVV2", typeof(string), typeof(CreditCardEntry), null);
        public string CVV2
        {
            get
            {
                return (string)GetValue(CVV2Property);
            }
            set
            {
                SetValue(CVV2Property, value);
            }
        }

        public string AESEncryptedCardNumber
        {
            get
            {
                if (CreditCardNumber == null)
                    CreditCardNumber = "";

                return Encryption.Encryption.EncryptAES(CreditCardNumber, Encryption.Encryption.key, Encryption.Encryption.salt);
            }
        }

        public static readonly DependencyProperty CreditCardNumberProperty = DependencyProperty.Register("CreditCardNumber", typeof(string), typeof(CreditCardEntry), null);
        public string CreditCardNumber
        {
            private get
            {
                return (string)GetValue(CreditCardNumberProperty);
            }
            set
            {
                SetValue(CreditCardNumberProperty, value);
            }
        }

        public static readonly DependencyProperty Address1Property = DependencyProperty.Register("Address1", typeof(string), typeof(CreditCardEntry), null);
        public string Address1
        {
            get
            {
                return (string)GetValue(Address1Property);
            }
            set
            {
                SetValue(Address1Property, value);
            }
        }

        public static readonly DependencyProperty Address2Property = DependencyProperty.Register("Address2", typeof(string), typeof(CreditCardEntry), null);
        public string Address2
        {
            get
            {
                return (string)GetValue(Address2Property);
            }
            set
            {
                SetValue(Address2Property, value);
            }
        }

        public static readonly DependencyProperty CityProperty = DependencyProperty.Register("City", typeof(string), typeof(CreditCardEntry), null);
        public string City
        {
            get
            {
                return (string)GetValue(CityProperty);
            }
            set
            {
                SetValue(CityProperty, value);
            }
        }

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(string), typeof(CreditCardEntry), null);
        public string State
        {
            get
            {
                return (string)GetValue(StateProperty);
            }
            set
            {
                SetValue(StateProperty, value);
            }
        }

        public static readonly DependencyProperty PostalCodeProperty = DependencyProperty.Register("PostalCode", typeof(string), typeof(CreditCardEntry), null);
        public string PostalCode
        {
            get
            {
                return (string)GetValue(PostalCodeProperty);
            }
            set
            {
                SetValue(PostalCodeProperty, value);
            }
        }

        public static readonly DependencyProperty CountryProperty = DependencyProperty.Register("Country", typeof(string), typeof(CreditCardEntry), null);
        public string Country
        {
            get
            {
                return (string)GetValue(CountryProperty);
            }
            set
            {
                SetValue(CountryProperty, value);
            }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(CreditCardEntry), null);
        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }
            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }

        public static readonly DependencyProperty PaymentInformationLabelProperty = DependencyProperty.Register("PaymentInformationLabel", typeof(string), typeof(CreditCardEntry), null);
        public string PaymentInformationLabel
        {
            get
            {
                return (string)GetValue(PaymentInformationLabelProperty);
            }
            set
            {
                SetValue(PaymentInformationLabelProperty, value);
            }
        }

        public static readonly DependencyProperty FirstNameLabelProperty = DependencyProperty.Register("FirstNameLabel", typeof(string), typeof(CreditCardEntry), null);
        public string FirstNameLabel
        {
            get
            {
                return (string)GetValue(FirstNameLabelProperty);
            }
            set
            {
                SetValue(FirstNameLabelProperty, value);
            }
        }

        public static readonly DependencyProperty LastNameLabelProperty = DependencyProperty.Register("LastNameLabel", typeof(string), typeof(CreditCardEntry), null);
        public string LastNameLabel
        {
            get
            {
                return (string)GetValue(LastNameLabelProperty);
            }
            set
            {
                SetValue(LastNameLabelProperty, value);
            }
        }

        public static readonly DependencyProperty Address1LabelProperty = DependencyProperty.Register("Address1Label", typeof(string), typeof(CreditCardEntry), null);
        public string Address1Label
        {
            get
            {
                return (string)GetValue(Address1LabelProperty);
            }
            set
            {
                SetValue(Address1LabelProperty, value);
            }
        }

        public static readonly DependencyProperty Address2LabelProperty = DependencyProperty.Register("Address2Label", typeof(string), typeof(CreditCardEntry), null);
        public string Address2Label
        {
            get
            {
                return (string)GetValue(Address2LabelProperty);
            }
            set
            {
                SetValue(Address2LabelProperty, value);
            }
        }

        public static readonly DependencyProperty CityLabelProperty = DependencyProperty.Register("CityLabel", typeof(string), typeof(CreditCardEntry), null);
        public string CityLabel
        {
            get
            {
                return (string)GetValue(CityLabelProperty);
            }
            set
            {
                SetValue(CityLabelProperty, value);
            }
        }

        public static readonly DependencyProperty StateLabelProperty = DependencyProperty.Register("StateLabel", typeof(string), typeof(CreditCardEntry), null);
        public string StateLabel
        {
            get
            {
                return (string)GetValue(StateLabelProperty);
            }
            set
            {
                SetValue(StateLabelProperty, value);
            }
        }

        public static readonly DependencyProperty PostalCodeLabelProperty = DependencyProperty.Register("PostalCodeLabel", typeof(string), typeof(CreditCardEntry), null);
        public string PostalCodeLabel
        {
            get
            {
                return (string)GetValue(PostalCodeLabelProperty);
            }
            set
            {
                SetValue(PostalCodeLabelProperty, value);
            }
        }

        public static readonly DependencyProperty CountryLabelProperty = DependencyProperty.Register("CountryLabel", typeof(string), typeof(CreditCardEntry), null);
        public string CountryLabel
        {
            get
            {
                return (string)GetValue(CountryLabelProperty);
            }
            set
            {
                SetValue(CountryLabelProperty, value);
            }
        }

        public static readonly DependencyProperty CreditCardNumberLabelProperty = DependencyProperty.Register("CreditCardNumberLabel", typeof(string), typeof(CreditCardEntry), null);
        public string CreditCardNumberLabel
        {
            get
            {
                return (string)GetValue(CreditCardNumberLabelProperty);
            }
            set
            {
                SetValue(CreditCardNumberLabelProperty, value);
            }
        }

        public static readonly DependencyProperty CVV2LabelProperty = DependencyProperty.Register("CVV2Label", typeof(string), typeof(CreditCardEntry), null);
        public string CVV2Label
        {
            get
            {
                return (string)GetValue(CVV2LabelProperty);
            }
            set
            {
                SetValue(CVV2LabelProperty, value);
            }
        }

        public static readonly DependencyProperty ExpirationMonthLabelProperty = DependencyProperty.Register("ExpirationMonthLabel", typeof(string), typeof(CreditCardEntry), null);
        public string ExpirationMonthLabel
        {
            get
            {
                return (string)GetValue(ExpirationMonthLabelProperty);
            }
            set
            {
                SetValue(ExpirationMonthLabelProperty, value);
            }
        }

        public static readonly DependencyProperty ExpirationYearLabelProperty = DependencyProperty.Register("ExpirationYearLabel", typeof(string), typeof(CreditCardEntry), null);
        public string ExpirationYearLabel
        {
            get
            {
                return (string)GetValue(ExpirationYearLabelProperty);
            }
            set
            {
                SetValue(ExpirationYearLabelProperty, value);
            }
        }
    }
}