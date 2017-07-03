using System;
using System.ComponentModel;
using System.Windows.Data;

namespace NetSteps.Silverlight.Controls
{
    public partial class CanadianPostalCodeControl : BasePostalCodeControl, INotifyPropertyChanged
    {
        public CanadianPostalCodeControl()
        {
            InitializeComponent();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class CanadianPostalCodeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                string[] postalCodePieces = value.ToString().Split(' ');
                if (postalCodePieces.Length == 2)
                {
                    switch (parameter.ToString())
                    {
                        case "FirstHalf":
                            return postalCodePieces[0];
                        case "SecondHalf":
                            return postalCodePieces[1];
                    }
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
