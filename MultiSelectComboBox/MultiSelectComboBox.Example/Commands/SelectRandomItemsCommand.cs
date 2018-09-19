using System;
using System.Windows.Input;

namespace Sdl.MultiSelectComboBox.Example.Commands
{
	public class SelectRandomItemsCommand : ICommand
	{
		private readonly Action<int> _selectRandomItems;
		
		public SelectRandomItemsCommand(Action<int> selectRandomItems)
		{
			_selectRandomItems = selectRandomItems;			
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_selectRandomItems(20);
		}

		public event EventHandler CanExecuteChanged;	
	}
}
