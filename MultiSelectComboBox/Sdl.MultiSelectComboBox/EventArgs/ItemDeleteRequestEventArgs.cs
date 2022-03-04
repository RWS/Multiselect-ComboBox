using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sdl.MultiSelectComboBox.EventArgs {
	public class ItemDeleteRequestEventArgs : RoutedEventArgs {
		public ICollection ItemsRemoveRequestedOn { get; }
		internal ItemDeleteRequestEventArgs(RoutedEvent routedEvent,
			ICollection items
			) : base(routedEvent) {
			ItemsRemoveRequestedOn = items;
		}
	}
}
