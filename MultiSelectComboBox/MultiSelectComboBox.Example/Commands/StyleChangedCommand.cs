using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sdl.MultiSelectComboBox.Example.Commands
{
	public class StyleChangedCommand : ICommand
	{
		private readonly Action<string, string> _updateEventLog;

		public StyleChangedCommand(Action<string, string> updateEventLog)
		{
			_updateEventLog = updateEventLog;

		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			if (parameter is RadioButton args)
			{
				_updateEventLog?.Invoke("Style Changed", args.Content.ToString());
			}
		}

		public event EventHandler CanExecuteChanged;
	}
}
