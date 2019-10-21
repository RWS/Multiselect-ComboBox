using System.Windows;
using System.Windows.Controls;

namespace Sdl.MultiSelectComboBox.Controls
{
	public class ExtendedListBox : ListBox
	{		
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is ExtendedListBoxItem;
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ExtendedListBoxItem();
		}

		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item as ExtendedListBoxItem);
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item as ExtendedListBoxItem);
		}		 
	}
}
