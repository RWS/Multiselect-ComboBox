using System;
using System.Windows.Input;

namespace Sdl.MultiSelectComboBox.Example.Commands
{
	public class ClearSelectedItemsCommand : ICommand
	{
		private readonly Action<string, string> _updateEventLog;		

		public ClearSelectedItemsCommand(Action<string, string> updateEventLog)
		{
			_updateEventLog = updateEventLog;			
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_updateEventLog?.Invoke("Clear items", string.Empty);
		}

		public event EventHandler CanExecuteChanged;	
	}
}
