using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Sdl.MultiSelectComboBox.Controls {
	internal class NumberComparisonToBoolConverter : IValueConverter {
		public int CompareToAmount { get; set; }
		public bool LessThanNotGreater { get; set; }
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			var num= System.Convert.ToInt32(value);
			if (LessThanNotGreater)
				return num < CompareToAmount;
			else
				return num > CompareToAmount;

		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => throw new NotImplementedException();
	}
}
