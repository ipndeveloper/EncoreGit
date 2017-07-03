using System;
using System.Globalization;
using System.Windows.Data;

namespace NetSteps.Silverlight.Converters
{
    public class UserToolboxGreetingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string greeting = DateTime.Now.Hour < 12 ? Translation.GetTerm("ToolBox_Greeting_Morning", "Good Morning") : (DateTime.Now.Hour < 17 ? Translation.GetTerm("ToolBox_Greeting_Afternoon", "Good Afternoon") : Translation.GetTerm("ToolBox_Greeting_Evening", "Good Evening"));

            string result = string.Format("{0} {1}!", greeting, value);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
