using System;
using System.Windows.Data;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;

namespace ErrorLogViewer.Converters
{
	public class IdToNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int id = System.Convert.ToInt32(value);
			if (id != 0)
			{
				// TODO: Use reflection to lookup values from SmallCollectionCache - JHE

				if (parameter.ToString().ToCleanString().ToUpper() == "Applications".ToUpper())
					return SmallCollectionCache.Instance.Applications.GetById(id.ToShort()).Name;
				else
					return string.Empty;
			}
			else
				return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
