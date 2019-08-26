using System.Windows;
using System.Windows.Controls;

namespace Sdl.MultiSelectComboBox.Controls
{
	public sealed class ContentItemsControl : ItemsControl
	{
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ContentControl();
		}
	}
}
