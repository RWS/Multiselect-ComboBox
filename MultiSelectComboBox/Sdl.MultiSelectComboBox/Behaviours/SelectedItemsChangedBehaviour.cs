using System.Windows;
using System.Windows.Input;

namespace Sdl.MultiSelectComboBox.Behaviours
{
	public class SelectedItemsChangedBehaviour
	{
		public static readonly DependencyProperty SelectedItemsChangedCommand = EventBehaviourFactory.CreateCommandExecutionEventBehaviour(
			Themes.Generic.MultiSelectComboBox.SelectedItemsChangedEvent, "SelectedItemsChanged", typeof(SelectedItemsChangedBehaviour));

		public static void SetSelectedItemsChanged(DependencyObject o, ICommand value)
		{
			o.SetValue(SelectedItemsChangedCommand, value);
		}

		public static ICommand GetSelectedItemsChanged(DependencyObject o)
		{
			return o.GetValue(SelectedItemsChangedCommand) as ICommand;
		}
	}
}
