using System;
using System.Windows;
using System.Windows.Data;

namespace Sdl.MultiSelectComboBox.Example.Converters
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is bool b && b)
			{
				return Visibility.Visible;
			}
			return Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is Visibility visibility && visibility == Visibility.Visible)
			{
				return true;
			}
			return false;
		}
	}
}
