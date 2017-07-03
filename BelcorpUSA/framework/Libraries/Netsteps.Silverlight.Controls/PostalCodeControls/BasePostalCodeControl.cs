using System.Windows;
using System.Windows.Controls;

namespace NetSteps.Silverlight.Controls
{
    public class BasePostalCodeControl : UserControl
    {
        public static readonly DependencyProperty PostalCodeProperty = DependencyProperty.Register("PostalCode", typeof(string), typeof(BasePostalCodeControl), null);
        public string PostalCode
        {
            get { return (string)GetValue(PostalCodeProperty); }
            set { SetValue(PostalCodeProperty, value); }
        }
    }
}
