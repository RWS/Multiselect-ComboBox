using System;
using System.Collections;
using System.Linq;
using System.Windows.Input;
using Sdl.MultiSelectComboBox.EventArgs;
using Sdl.MultiSelectComboBox.Example.Models;

namespace Sdl.MultiSelectComboBox.Example.Commands
{
	public class FilterTextChangedCommand : ICommand
	{
		private readonly Action<string, string> _updateEventLog;
		private readonly Func<bool> _canExecute;

		public FilterTextChangedCommand(Action<string, string> updateEventLog, Func<bool> canExecute)
		{
			_updateEventLog = updateEventLog;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute() && parameter is FilterTextChangedEventArgs && _updateEventLog != null;
		}

		public void Execute(object parameter)
		{
			if (parameter is FilterTextChangedEventArgs args)
			{				
				var aggregatedText = GetAggregatedText(args.Items);

				var report = "Text: " + args.Text
					+ ", Items: " + args.Items?.Count + (!string.IsNullOrEmpty(aggregatedText) ? " (" + TrimToLength(aggregatedText, 100) + ") " : string.Empty);

				_updateEventLog?.Invoke("Filter Changed", report);
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
