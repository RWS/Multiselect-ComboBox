using System.Linq;
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
				var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
				
				Application.Current.MainWindow.Title = Application.Current.MainWindow.Title + " (" + fileVersionInfo.FileVersion
				                                       + " - " + GetTargetFramework() + ")";
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

		private string GetTargetFramework()
		{
			var targetFrameworkAttribute = Assembly.GetExecutingAssembly()
				.GetCustomAttributes(typeof(System.Runtime.Versioning.TargetFrameworkAttribute), false)
				.OfType<System.Runtime.Versioning.TargetFrameworkAttribute>()
				.FirstOrDefault();

			return targetFrameworkAttribute.FrameworkName;
		}
	}
}
