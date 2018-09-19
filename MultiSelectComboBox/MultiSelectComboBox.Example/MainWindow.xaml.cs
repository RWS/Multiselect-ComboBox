using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Sdl.MultiSelectComboBox.Example.Models;

namespace Sdl.MultiSelectComboBox.Example
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			if (Application.Current.MainWindow != null)
			{
				Application.Current.MainWindow.Title = Application.Current.MainWindow.Title + " (" + Assembly.GetExecutingAssembly().GetName().Version + ")";
			}

			Loaded += MainWindow_Loaded;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			var model = new LanguageItems();
			DataContext = model;
		}

		private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			if (sender is TextBox textBox && textBox.LineCount > 0)
			{
				textBox.ScrollToLine(textBox.LineCount - 1);
			}
		}
	}
}
