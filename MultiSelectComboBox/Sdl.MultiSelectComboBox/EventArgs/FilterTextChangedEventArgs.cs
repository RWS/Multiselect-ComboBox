using System.Collections;
using System.Windows;

namespace Sdl.MultiSelectComboBox.EventArgs
{
	/// <summary>
	/// Raised when the filter criteria has changed
	/// </summary>
	public class FilterTextChangedEventArgs : RoutedEventArgs
	{	
		/// <summary>
		/// The filter critera applied on the collection of items
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// The filtered list of items
		/// </summary>
		public ICollection Items { get; }
		
		internal FilterTextChangedEventArgs(RoutedEvent routedEvent, string text, ICollection items) : base(routedEvent)
		{
			Text = text;
			Items = items;
		}
	}
}
