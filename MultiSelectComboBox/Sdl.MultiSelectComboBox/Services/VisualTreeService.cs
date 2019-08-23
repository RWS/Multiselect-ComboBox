using System.Windows;
using System.Windows.Media;

namespace Sdl.MultiSelectComboBox.Services
{
	internal class VisualTreeService
	{
		public static T FindVisualChild<T>(DependencyObject parent, string name)
			where T : DependencyObject
		{
			if (parent == null)
			{
				return null;
			}
					
			T foundFrameworkElement = null;
			
			var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (var i = 0; i < childrenCount; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);

				if (!(child is T))
				{
					foundFrameworkElement = FindVisualChild<T>(child, name);

					if (foundFrameworkElement != null)
					{
						break;
					}
				}
				else if (!string.IsNullOrEmpty(name))
				{
					if (child is FrameworkElement frameworkElement && frameworkElement.Name == name)
					{
						foundFrameworkElement = (T)child;
						break;
					}
				}
				else
				{
					foundFrameworkElement = (T)child;
					break;
				}
			}

			return foundFrameworkElement;
		}

		public static T FindVisualTemplatedParent<T>(DependencyObject dependencyObject, string name) where T : DependencyObject
		{
			var source = dependencyObject as FrameworkElement;
			var parent = source?.Parent;
			var templatedParent = source?.TemplatedParent;

			if (source == null)
			{
				return null;
			}

			T foundFrameworkElement;

			var templatedParentElement = templatedParent as FrameworkElement;
			var templatedParentParentElement = templatedParentElement?.Parent as FrameworkElement;

			if (!(parent is T) && !(templatedParent is T) && !(templatedParentElement?.Parent is T))
			{
				foundFrameworkElement = FindVisualTemplatedParent<T>(templatedParent, name);
			}
			else if (!string.IsNullOrEmpty(name))
			{
				if (parent is FrameworkElement parentElement && parentElement.Name == name)
				{
					foundFrameworkElement = (T)parent;
				}
				else if (templatedParentElement != null && templatedParentElement.Name == name)
				{
					foundFrameworkElement = (T)templatedParent;
				}
				else if (templatedParentParentElement != null && templatedParentParentElement.Name == name)
				{
					foundFrameworkElement = (T)templatedParentElement.Parent;
				}
				else
				{
					foundFrameworkElement = FindVisualTemplatedParent<T>(templatedParent, name);
				}
			}
			else
			{
				foundFrameworkElement = parent is T
					? (T)parent
					: ((T)templatedParent is T
						? (T)templatedParent
						: (T)templatedParentElement?.Parent);
			}

			return foundFrameworkElement;
		}
	}
}
