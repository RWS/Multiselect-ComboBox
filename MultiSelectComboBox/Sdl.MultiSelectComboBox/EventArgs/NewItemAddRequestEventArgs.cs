using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sdl.MultiSelectComboBox.EventArgs {
	public class NewItemAddRequestEventArgs : RoutedEventArgs {
		public string TypedText { get; }

		internal NewItemAddRequestEventArgs(RoutedEvent routedEvent, string TypedText) : base(routedEvent) {
			this.TypedText = TypedText;
		}
	}
}
