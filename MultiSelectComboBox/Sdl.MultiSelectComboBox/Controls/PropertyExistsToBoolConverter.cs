using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Sdl.MultiSelectComboBox.Controls {
	internal class PropertyExistsToBoolConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			var type = value.GetType();
			if (CheckedTypeCache.TryGetValue(type, out var res))
				return res;
			var prop = type.GetProperty(PropertyToCheck);
			res = prop != null;
			CheckedTypeCache[type] = res;
			return res;
		}
		public string PropertyToCheck { get; set; }
		private ConcurrentDictionary<Type, bool> CheckedTypeCache = new ConcurrentDictionary<Type, bool>();

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
