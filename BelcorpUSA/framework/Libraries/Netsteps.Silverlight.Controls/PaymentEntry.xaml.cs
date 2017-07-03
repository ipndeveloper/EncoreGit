using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NetSteps.Silverlight.Controls
{
    public partial class PaymentEntry : UserControl
    {
        public static readonly DependencyProperty DataContextChangedProperty = DependencyProperty.Register("DataContextChanged", typeof(object), typeof(PaymentEntry), new PropertyMetadata(new PropertyChangedCallback((o, e) =>
        {
            PaymentEntry paymentEntry = (o as PaymentEntry);

            if ((o as PaymentEntry).DataContext is PaymentEntryModel)
            {
                PaymentEntryModel paymentEntryModel = ((o as PaymentEntry).DataContext as PaymentEntryModel);

                paymentEntryModel.ExpirationYear = (int)(paymentEntry.uxYears.SelectedItem);
                paymentEntryModel.ExpirationMonth = (int)((KeyValuePair<int, string>)paymentEntry.uxMonths.SelectedItem).Key;

                paymentEntryModel.PropertyChanged += (sender, a) =>
                {
                    if (a.PropertyName == "CreditCardNumberIsValid")
                    {
                        string original = paymentEntry.uxTxtCreditCardNumber.Text;
                        paymentEntry.uxTxtCreditCardNumber.Text = Guid.NewGuid().ToString();
                        paymentEntry.uxTxtCreditCardNumber.Text = original;
                    }
                };
            }
        })));

        public static readonly DependencyProperty MonthsProperty = DependencyProperty.Register("Months", typeof(Dictionary<int, string>), typeof(PaymentEntry), null);
        public Dictionary<int, string> Months
        {
            get
            {
                return (Dictionary<int, string>)this.GetValue(MonthsProperty);
            }
            set
            {
                this.SetValue(MonthsProperty, value);
                uxMonths.ItemsSource = Months;
            }
        }

        public static readonly DependencyProperty ExpirationMonthProperty = DependencyProperty.Register("ExpirationMonth", typeof(KeyValuePair<int, string>), typeof(PaymentEntry), null);
        public KeyValuePair<int, string> ExpirationMonth
        {
            get
            {
                return (KeyValuePair<int, string>)this.GetValue(ExpirationMonthProperty);
            }
            set
            {
                this.SetValue(ExpirationMonthProperty, value);

                if (this.DataContext is PaymentEntryModel)
                    (this.DataContext as PaymentEntryModel).ExpirationMonth = value.Key;
            }
        }

        public static readonly DependencyProperty YearsProperty = DependencyProperty.Register("Years", typeof(ObservableCollection<int>), typeof(PaymentEntry), null);
        public ObservableCollection<int> Years
        {
            get
            {
                return (ObservableCollection<int>)this.GetValue(YearsProperty);
            }
            set
            {
                this.SetValue(YearsProperty, value);
                uxYears.ItemsSource = Years;
            }
        }

        public static readonly DependencyProperty ExpirationYearProperty = DependencyProperty.Register("ExpirationYear", typeof(int), typeof(PaymentEntry), null);
        public int ExpirationYear
        {
            get
            {
                return (int)this.GetValue(ExpirationYearProperty);
            }
            set
            {
                this.SetValue(ExpirationYearProperty, value);

                if (this.DataContext is PaymentEntryModel)
                    (this.DataContext as PaymentEntryModel).ExpirationYear = value;
            }
        }

        public PaymentEntry()
        {
            InitializeComponent();

            uxMonths.SelectionChanged += new SelectionChangedEventHandler(uxMonths_SelectionChanged);
            uxYears.SelectionChanged += new SelectionChangedEventHandler(uxYears_SelectionChanged);

            Dictionary<int, string> months = new Dictionary<int, string>();
            foreach (int month in Enumerable.Range(1, 12))
            {
                string monthAsString = month.ToString().PadLeft(2, '0');
                months.Add(month, monthAsString + " (" + DateTime.ParseExact(monthAsString, "MM", System.Threading.Thread.CurrentThread.CurrentUICulture).ToString("MMMM") + ")");
            }

            ObservableCollection<int> years = new ObservableCollection<int>();
            foreach (int year in Enumerable.Range(DateTime.Now.Year, 10))
                years.Add(year);

            uxMonths.ItemsSource = months;
            uxYears.ItemsSource = years;

            uxMonths.SelectedIndex = 0;
            uxYears.SelectedIndex = 0;

            uxYears_SelectionChanged(this, null);
            uxMonths_SelectionChanged(this, null);
        }

        private void uxYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExpirationYear = (int)(uxYears.SelectedItem);
        }

        private void uxMonths_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExpirationMonth = (KeyValuePair<int, string>)uxMonths.SelectedItem;
        }

        private void PostalCodeControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.DataContext is PaymentEntryModel)
            {
                PaymentEntryModel model = this.DataContext as PaymentEntryModel;

                string postalCode = string.Empty;
                if ((sender as ContentControl).Content is TextBox)
                    postalCode = ((sender as ContentControl).Content as TextBox).Text;
                else if ((sender as ContentControl).Content is BasePostalCodeControl)
                    postalCode = ((sender as ContentControl).Content as BasePostalCodeControl).PostalCode;

                if (model.SelectedCountry != null && model.SelectedCountry.PostalCodeLookup != null)
                {
                    model.SelectedCountry.PostalCodeLookup(postalCode);
                }
            }
        }
    }

    public class PaymentEntryModel : INotifyPropertyChanged
    {
        private string _firstName, _lastName, _addressLine1, _addressLine2, _postalCode, _country, _city, _state, _creditCardNumber, _cvv2;
        private int _expirationMonth, _expirationYear;
        private bool _creditCardNumberIsValid = true;
        private CountryModel _selectedCountry;
        private ObservableCollection<string> _cities, _states;
        private ObservableCollection<CountryModel> _countries = new ObservableCollection<CountryModel>();

        public PaymentEntryModel()
        {
            Console.Write("test");
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("First Name is required.");
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("Last Name is required.");
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        public string AddressLine1
        {
            get { return _addressLine1; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("Address Line 1 is required.");
                if (_addressLine1 != value)
                {
                    _addressLine1 = value;
                    OnPropertyChanged("AddressLine1");
                }
            }
        }

        public string AddressLine2
        {
            get { return _addressLine2; }
            set
            {
                if (_addressLine2 != value)
                {
                    _addressLine2 = value;
                    OnPropertyChanged("AddressLine2");
                }
            }
        }

        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("Postal Code is required.");
                if (_postalCode != value)
                {
                    if (_selectedCountry != null)
                    {
                        if (_selectedCountry.PostalCodeValidation != null)
                            _selectedCountry.PostalCodeValidation(value);
                        //if (_selectedCountry.PostalCodeValidation != null && !_selectedCountry.PostalCodeValidation(value))
                        //    return;
                        //if (_selectedCountry.PostalCodeLookup != null)
                        //{
                        //ObservableCollection<string> cities, states;
                        //_selectedCountry.PostalCodeLookup(value, out cities, out states);
                        //Cities = cities;
                        //States = states;
                        //    _selectedCountry.PostalCodeLookup(value);
                        //}
                    }
                    _postalCode = value;
                    OnPropertyChanged("PostalCode");
                }
            }
        }

        public string Country
        {
            get { return _country; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("Country is required.");
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("City is required.");
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        public string State
        {
            get { return _state; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("State is required.");
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged("State");
                }
            }
        }

        public bool CreditCardNumberIsValid
        {
            get { return _creditCardNumberIsValid; }
            set
            {
                if (_creditCardNumberIsValid != value)
                {
                    _creditCardNumberIsValid = value;
                    OnPropertyChanged("CreditCardNumberIsValid");
                }
            }
        }

        public string CreditCardNumber
        {
            get { return _creditCardNumber; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("Credit Card Number is required.");
                if (_creditCardNumber != value)
                {
                    if (CreditCardValidation != null)
                        CreditCardValidation(value);
                    if (!CreditCardNumberIsValid)
                        throw new InvalidOperationException("Credit Card Number is not valid.");
                    _creditCardNumber = value;
                    OnPropertyChanged("CreditCardNumber");
                }
            }
        }

        public string AESEncryptedCardNumber
        {
            get
            {
                if (string.IsNullOrEmpty(CreditCardNumber))
                    CreditCardNumber = "";

                return Encryption.Encryption.EncryptAES(CreditCardNumber, Encryption.Encryption.key, Encryption.Encryption.salt);
            }
        }

        public string CVV2
        {
            get { return _cvv2; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("CVV2 is required.");
                if (_cvv2 != value)
                {
                    _cvv2 = value;
                    OnPropertyChanged("CVV2");
                }
            }
        }

        public int ExpirationMonth
        {
            get { return _expirationMonth; }
            set
            {
                if (value < 1 || value > 12)
                    throw new InvalidOperationException("Expiration Month is required.");
                if (_expirationMonth != value)
                {
                    _expirationMonth = value;
                    OnPropertyChanged("ExpirationMonth");
                }
            }
        }

        public int ExpirationYear
        {
            get { return _expirationYear; }
            set
            {
                if (value <= 0)
                    throw new InvalidOperationException("Expiration Year is invalid.");
                if (_expirationYear != value)
                {
                    _expirationYear = value;
                    OnPropertyChanged("ExpirationYear");
                }
            }
        }

        public CountryModel SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                if (_selectedCountry != value)
                {
                    if (_selectedCountry != null)
                    {
                        _postalCode = "";
                        _city = "";
                        _state = "";
                    }
                    _selectedCountry = value;
                    OnPropertyChanged("SelectedCountry");
                }
            }
        }

        public ObservableCollection<string> Cities
        {
            get { return _cities; }
            set
            {
                if (_cities != value)
                {
                    _cities = value;
                    OnPropertyChanged("Cities");
                }
            }
        }

        public ObservableCollection<string> States
        {
            get { return _states; }
            set
            {
                if (_states != value)
                {
                    _states = value;
                    OnPropertyChanged("States");
                }
            }
        }

        public ObservableCollection<CountryModel> Countries
        {
            get { return _countries; }
        }

        public Action<string> CreditCardValidation
        {
            get;
            set;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public delegate void LookupFunc<T, TResult>(T param1, out TResult outParam1, out TResult outParam2);

    public class CountryModel : INotifyPropertyChanged
    {
        private string _countryName;
        private int _countryId;
        private UIElement _postalCodeControl, _cityControl, _stateControl;

        public string CountryName
        {
            get { return _countryName; }
            set
            {
                if (_countryName != value)
                {
                    _countryName = value;
                    OnPropertyChanged("CountryName");
                }
            }
        }

        public int CountryId
        {
            get { return _countryId; }
            set
            {
                if (_countryId != value)
                {
                    _countryId = value;
                    OnPropertyChanged("CountryId");
                }
            }
        }

        public UIElement PostalCodeControl
        {
            get { return _postalCodeControl; }
            set
            {
                if (_postalCodeControl != value)
                {
                    _postalCodeControl = value;
                    OnPropertyChanged("PostalCodeControl");
                }
            }
        }

        public UIElement CityControl
        {
            get { return _cityControl; }
            set
            {
                if (_cityControl != value)
                {
                    _cityControl = value;
                    OnPropertyChanged("CityControl");
                }
            }
        }

        public UIElement StateControl
        {
            get { return _stateControl; }
            set
            {
                if (_stateControl != value)
                {
                    _stateControl = value;
                    OnPropertyChanged("StateControl");
                }
            }
        }

        //public Func<string, bool> PostalCodeValidation
        //{
        //    get;
        //    set;
        //}

        //public LookupFunc<string, ObservableCollection<string>> PostalCodeLookup
        //{
        //    get;
        //    set;
        //}

        public Action<string> PostalCodeValidation
        {
            get;
            set;
        }

        public Action<string> PostalCodeLookup
        {
            get;
            set;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
