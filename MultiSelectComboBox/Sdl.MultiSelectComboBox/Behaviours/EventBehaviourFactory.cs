using System;
using System.Windows;
using System.Windows.Input;

namespace Sdl.MultiSelectComboBox.Behaviours
{
	public static class EventBehaviourFactory
	{
		public static DependencyProperty CreateCommandExecutionEventBehaviour(RoutedEvent routedEvent, string propertyName, Type ownerType)
		{
			var property = DependencyProperty.RegisterAttached(propertyName, typeof(ICommand), ownerType,
				new PropertyMetadata(null, new ExecuteCommandOnRoutedEventBehaviour(routedEvent).PropertyChangedHandler));

			return property;
		}

		/// <summary>
		/// An internal class to handle listening for an event and executing a command,
		/// when a Command is assigned to a particular DependencyProperty
		/// </summary>
		private class ExecuteCommandOnRoutedEventBehaviour : ExecuteCommandBehaviour
		{
			private readonly RoutedEvent _routedEvent;

			public ExecuteCommandOnRoutedEventBehaviour(RoutedEvent routedEvent)
			{
				_routedEvent = routedEvent;
			}

			/// <summary>
			/// Handles attaching or Detaching Event handlers when a Command is assigned or unassigned
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="oldValue"></param>
			/// <param name="newValue"></param>
			protected override void AdjustEventHandlers(DependencyObject sender, object oldValue, object newValue)
			{
				if (!(sender is UIElement element))
				{
					return;
				}

				if (oldValue != null)
				{
					element.RemoveHandler(_routedEvent, new RoutedEventHandler(EventHandler));
				}

				if (newValue != null)
				{
					element.AddHandler(_routedEvent, new RoutedEventHandler(EventHandler));
				}
			}

			private void EventHandler(object sender, RoutedEventArgs e)
			{
				HandleEvent(sender, e);
			}
		}

		internal abstract class ExecuteCommandBehaviour
		{
			protected DependencyProperty Property;
			protected abstract void AdjustEventHandlers(DependencyObject sender, object oldValue, object newValue);

			protected void HandleEvent(object sender, System.EventArgs e)
			{
				var dp = sender as DependencyObject;

				if (!(dp?.GetValue(Property) is ICommand command))
				{
					return;
				}

				if (command.CanExecute(e))
				{
					command.Execute(e);
				}
			}

			/// <summary>
			/// Listens for a change in the DependencyProperty that we are assigned to, and
			/// adjusts the EventHandlers accordingly
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="e"></param>
			public void PropertyChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
			{
				// the first time the property changes,
				// make a note of which property we are supposed
				// to be watching
				if (Property == null)
				{
					Property = e.Property;
				}

				var oldValue = e.OldValue;
				var newValue = e.NewValue;

				AdjustEventHandlers(sender, oldValue, newValue);
			}
		}
	}
}
