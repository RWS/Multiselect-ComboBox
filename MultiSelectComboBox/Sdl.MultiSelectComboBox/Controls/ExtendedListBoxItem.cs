using System.Windows;
using System.Windows.Controls;

namespace Sdl.MultiSelectComboBox.Controls
{
	public class ExtendedListBoxItem: ListBoxItem
	{		
		public static readonly DependencyProperty IsCheckedProperty =
			DependencyProperty.Register("IsChecked", typeof(bool), typeof(ExtendedListBoxItem),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public bool IsChecked
		{
			get => (bool)GetValue(IsCheckedProperty);
			set => SetValue(IsCheckedProperty, value);
		}
	}
}
