using System.Collections;
using System.Windows;

namespace Sdl.MultiSelectComboBox.EventArgs
{
	/// <summary>
	/// Raised when the selected items collection is modified
	/// </summary>
	public class SelectedItemsChangedEventArgs : RoutedEventArgs
	{
		/// <summary>
		/// Items added to the collection
		/// </summary>
		public ICollection Added { get; }

		/// <summary>
		/// Items removed from the collection
		/// </summary>
		public ICollection Removed { get; }

		/// <summary>
		/// The selected items
		/// </summary>
		public ICollection Selected { get; }

		internal SelectedItemsChangedEventArgs(RoutedEvent routedEvent,
			ICollection added,
			ICollection removed,
			ICollection selected) : base(routedEvent)
		{
			Added = added;
			Removed = removed;
			Selected = selected;
		}
	}
}
