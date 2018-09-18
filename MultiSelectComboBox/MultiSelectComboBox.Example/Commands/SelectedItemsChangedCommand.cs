using System;
using System.Collections;
using System.Linq;
using System.Windows.Input;
using Sdl.MultiSelectComboBox.EventArgs;
using Sdl.MultiSelectComboBox.Example.Models;

namespace Sdl.MultiSelectComboBox.Example.Commands
{
	public class SelectedItemsChangedCommand : ICommand
	{
		private readonly Action<ICollection> _updateSelectedItems;
		private readonly Action<string, string> _updateEventLog;
		private readonly Func<bool> _canExecute;

		public SelectedItemsChangedCommand(Action<string, string> updateEventLog, Action<ICollection> updateSelectedItems, Func<bool> canExecute)
		{
			_updateEventLog = updateEventLog;
			_updateSelectedItems = updateSelectedItems;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute() && parameter is SelectedItemsChangedEventArgs && _updateEventLog != null;
		}

		public void Execute(object parameter)
		{
			if (parameter is SelectedItemsChangedEventArgs args)
			{
				_updateSelectedItems?.Invoke(args.Selected);

				var addedItems = GetAggregatedText(args.Added);
				var removedItems = GetAggregatedText(args.Removed);
				var selectedItems = GetAggregatedText(args.Selected);

				var report = "Added: " + args.Added?.Count + (!string.IsNullOrEmpty(addedItems) ? " (" + TrimToLength(addedItems, 100) + ") " : string.Empty)
								+ ", Removed: " + args.Removed?.Count + (!string.IsNullOrEmpty(removedItems) ? " (" + TrimToLength(removedItems, 100) + ") " : string.Empty)
								+ ", Selected: " + args.Selected?.Count + (!string.IsNullOrEmpty(selectedItems) ? " (" + TrimToLength(selectedItems, 100) + ") " : string.Empty);

				_updateEventLog?.Invoke("Selected Changed", report);
			}
		}

		public event EventHandler CanExecuteChanged;

		private static string TrimToLength(string text, int length)
		{
			if (text?.Length > length)
			{
				text = text.Substring(0, length) + "...";
			}

			return text;
		}

		private static string GetAggregatedText(IEnumerable items)
		{
			var itemsText = string.Empty;
			return items?.Cast<LanguageItem>().Aggregate(itemsText, (current, item) => current + ((!string.IsNullOrEmpty(current) ? ", " : string.Empty) + item.Id));
		}
	}
}
