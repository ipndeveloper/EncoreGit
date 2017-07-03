using System;
using System.Globalization;
using System.Windows.Data;

namespace NetSteps.Silverlight.Converters
{
    public class TaskStatusToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((int)value == 1)
                    return "/Images/flag_red.png";
                else if ((int)value == 2)
                    return "/Images/flag_blue.png";
                else if ((int)value == 3)
                    return "/Images/flag_green.png";
            }
            return "/Images/flag_white.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TaskAlertToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Boolean)value) return "/Images/AlertIcon.png";
            return "/Images/blank.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class TaskStatusToImageStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "TaskGridDeletedImage";
            if ((int)value == 1) return "TaskGridToDoImage";
            if ((int)value == 2) return "TaskGridInProgressImage";
            if ((int)value == 3) return "TaskGridCompletedImage";
            return "TaskGridDeletedImage";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TaskAlertToImageStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Boolean)value) return "TaskGridAlertImage";
            return "TaskGridTaskImage";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TaskStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((int)value == 1) return Translation.GetTerm("Mod_Tasks_Center_Status_drpdwn_ToDo", "To Do");
                if ((int)value == 2) return Translation.GetTerm("Mod_Tasks_Center_Status_drpdwn_InProgress", "In Progress");
                if ((int)value == 3) return Translation.GetTerm("Mod_Tasks_Center_Status_drpdwn_Completed", "Completed");
                return Translation.GetTerm("Mod_Tasks_Center_Status_drpdwn_Deleted", "Deleted");
            }
            return Translation.GetTerm("Mod_Tasks_Center_Status_drpdwn_Deleted", "Deleted");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


}
