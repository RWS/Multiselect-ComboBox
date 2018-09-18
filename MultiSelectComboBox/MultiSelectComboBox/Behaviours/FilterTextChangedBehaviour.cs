using System.Windows;
using System.Windows.Input;

namespace Sdl.MultiSelectComboBox.Behaviours
{
	public class FilterTextChangedBehaviour
	{
		public static readonly DependencyProperty FilterTextChangedCommand = EventBehaviourFactory.CreateCommandExecutionEventBehaviour(
		Themes.Generic.MultiSelectComboBox.FilterTextChangedEvent, "FilterTextChanged", typeof(FilterTextChangedBehaviour));

		public static void SetFilterTextChanged(DependencyObject o, ICommand value)
		{
			o.SetValue(FilterTextChangedCommand, value);
		}

		public static ICommand GetFilterTextChanged(DependencyObject o)
		{
			return o.GetValue(FilterTextChangedCommand) as ICommand;
		}
	}
}
