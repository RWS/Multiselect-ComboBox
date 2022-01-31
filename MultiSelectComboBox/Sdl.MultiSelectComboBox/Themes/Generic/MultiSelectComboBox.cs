using Sdl.MultiSelectComboBox.API;
using Sdl.MultiSelectComboBox.Controls;
using Sdl.MultiSelectComboBox.EventArgs;
using Sdl.MultiSelectComboBox.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Sdl.MultiSelectComboBox.Themes.Generic
{
	[TemplatePart(Name = PART_MultiSelectComboBox, Type = typeof(Grid))]
	[TemplatePart(Name = PART_MultiSelectComboBox_Dropdown, Type = typeof(Popup))]
	[TemplatePart(Name = PART_MultiSelectComboBox_Dropdown_ListBox, Type = typeof(ListBox))]
	[TemplatePart(Name = PART_MultiSelectComboBox_Dropdown_Button, Type = typeof(Button))]
	[TemplatePart(Name = PART_MultiSelectComboBox_SelectedItemsPanel_ItemsControl, Type = typeof(ItemsControl))]
	[TemplatePart(Name = PART_MultiSelectComboBox_SelectedItemsPanel_Filter_TextBox, Type = typeof(TextBox))]
	[TemplatePart(Name = PART_MultiSelectComboBox_SelectedItemsPanel_Filter_AutoComplete_TextBox, Type = typeof(TextBox))]
	[TemplatePart(Name = PART_MultiSelectComboBox_SelectedItemsPanel_RemoveItem_Button, Type = typeof(Button))]
	public class MultiSelectComboBox : Control, IDisposable
	{
		private const string PART_MultiSelectComboBox = "PART_MultiSelectComboBox";
		private const string PART_MultiSelectComboBox_Dropdown = "PART_MultiSelectComboBox_Dropdown";
		private const string PART_MultiSelectComboBox_Dropdown_ListBox = "PART_MultiSelectComboBox_Dropdown_ListBox";
		private const string PART_MultiSelectComboBox_Dropdown_Button = "PART_MultiSelectComboBox_Dropdown_Button";
		private const string PART_MultiSelectComboBox_SelectedItemsPanel_ItemsControl = "PART_MultiSelectComboBox_SelectedItemsPanel_ItemsControl";
		private const string PART_MultiSelectComboBox_SelectedItemsPanel_Filter_TextBox = "PART_MultiSelectComboBox_SelectedItemsPanel_Filter_TextBox";
		private const string PART_MultiSelectComboBox_SelectedItemsPanel_Filter_AutoComplete_TextBox = "PART_MultiSelectComboBox_SelectedItemsPanel_Filter_AutoComplete_TextBox";
		private const string PART_MultiSelectComboBox_SelectedItemsPanel_RemoveItem_Button = "PART_MultiSelectComboBox_SelectedItemsPanel_RemoveItem_Button";

		private const string MultiSelectComboBox_SelectedItems_ItemTemplate = "MultiSelectComboBox.SelectedItems.ItemTemplate";
		private const string MultiSelectComboBox_SelectedItems_Searchable_ItemTemplate = "MultiSelectComboBox.SelectedItems.Searchable.ItemTemplate";
		private const string MultiSelectComboBox_Dropdown_ListBox_ItemTemplate = "MultiSelectComboBox.Dropdown.ListBox.ItemTemplate";

		public MultiSelectComboBox()
		{
			Loaded += MultiSelectComboBox_Loaded;
			Unloaded += MultiSelectComboBox_Unloaded;
		}

		private void MultiSelectComboBox_Loaded(object sender, RoutedEventArgs e)
		{
			InitializeSelectedItemsNotifyingCollection();
		}

		private void MultiSelectComboBox_Unloaded(object sender, RoutedEventArgs e)
		{
			CleanUpSelectedItemsNotifyingCollection();
		}

		private Window _controlWindow;
		private Window ControlWindow
		{
			get => _controlWindow;
			set
			{
				if (_controlWindow != null)
				{
					_controlWindow.LocationChanged -= ControlWindowLocationChanged;
					_controlWindow.Deactivated -= ControlWindowDeactivated;
				}

				_controlWindow = value;

				if (_controlWindow != null)
				{
					_controlWindow.LocationChanged += ControlWindowLocationChanged;
					_controlWindow.Deactivated += ControlWindowDeactivated;
				}
			}
		}

		private Grid _multiSelectComboBoxGrid;
		private Grid MultiSelectComboBoxGrid
		{
			get => _multiSelectComboBoxGrid;
			set
			{
				if (_multiSelectComboBoxGrid != null)
				{
					_multiSelectComboBoxGrid.PreviewMouseDown -= MultiSelectComboBoxOnPreviewMouseDown;
					_multiSelectComboBoxGrid.GotFocus -= MultiSelectComboBoxGotFocus;
					_multiSelectComboBoxGrid.LostFocus -= MultiSelectComboBoxLostFocus;
					_multiSelectComboBoxGrid.KeyUp -= MultiSelectComboBoxKeyUp;
					_multiSelectComboBoxGrid.SizeChanged -= MultiSelectComboBoxGridSizeChanged;

					PreviewKeyUp -= MultiSelectComboBox_PreviewKeyUp;
				}

				_multiSelectComboBoxGrid = value;

				if (_multiSelectComboBoxGrid != null)
				{
					_multiSelectComboBoxGrid.PreviewMouseDown += MultiSelectComboBoxOnPreviewMouseDown;
					_multiSelectComboBoxGrid.GotFocus += MultiSelectComboBoxGotFocus;
					_multiSelectComboBoxGrid.LostFocus += MultiSelectComboBoxLostFocus;
					_multiSelectComboBoxGrid.KeyUp += MultiSelectComboBoxKeyUp;
					_multiSelectComboBoxGrid.SizeChanged += MultiSelectComboBoxGridSizeChanged;

					PreviewKeyUp += MultiSelectComboBox_PreviewKeyUp;
				}
			}
		}

		private Popup _dropdownMenu;
		private Popup DropdownMenu
		{
			get => _dropdownMenu;
			set
			{
				if (_dropdownMenu != null)
				{
					_dropdownMenu.Closed -= DropdownMenuClosed;
					_dropdownMenu.Opened -= DropdownMenuOpened;
				}

				_dropdownMenu = value;

				if (_dropdownMenu != null)
				{
					_dropdownMenu.Closed += DropdownMenuClosed;
					_dropdownMenu.Opened += DropdownMenuOpened;
				}
			}
		}

		private ListBox _dropdownListBox;
		private ListBox DropdownListBox
		{
			get => _dropdownListBox;
			set
			{
				if (_dropdownListBox != null)
				{
					_dropdownListBox.SelectionChanged -= DropdownListBoxSelectionChanged;
					_dropdownListBox.PreviewMouseUp -= DropdownListBoxPreviewMouseUp;
					_dropdownListBox.PreviewKeyDown -= DropdownListBoxPreviewKeyDown;
					_dropdownListBox.ItemContainerGenerator.StatusChanged -= DropDownListBoxItemContainerGenerator_StatusChanged;
					_dropdownListBox.RemoveHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(DropDownListBoxScrolled));
				}

				_dropdownListBox = value;

				if (_dropdownListBox != null)
				{
					if (DropdownItemTemplate == null)
					{
						DropdownItemTemplate = _dropdownListBox.FindResource(MultiSelectComboBox_Dropdown_ListBox_ItemTemplate) as DataTemplate;
					}

					DropdownItemTemplateSelector = new DropdownItemTemplateService(DropdownItemTemplate);

					// this should always be set to Single; multiple selection feature is managed separatly.
					_dropdownListBox.SelectionMode = System.Windows.Controls.SelectionMode.Single;
					_dropdownListBox.ItemsSource = ItemsCollectionViewSource?.View;

					_dropdownListBox.SelectionChanged += DropdownListBoxSelectionChanged;
					_dropdownListBox.PreviewMouseUp += DropdownListBoxPreviewMouseUp;
					_dropdownListBox.PreviewKeyDown += DropdownListBoxPreviewKeyDown;
					_dropdownListBox.ItemContainerGenerator.StatusChanged += DropDownListBoxItemContainerGenerator_StatusChanged;
					_dropdownListBox.AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(DropDownListBoxScrolled));
				}
			}
		}

		private CollectionViewSource _itemsCollectionViewSource;
		private CollectionViewSource ItemsCollectionViewSource
		{
			get => _itemsCollectionViewSource;
			set
			{
				_itemsCollectionViewSource = value;

				if (ItemsCollectionViewSource != null && ItemsSource != null)
				{
					if (EnableGrouping)
					{
						// check that the items are groupable before adding a default group definition
						if (ItemsCollectionViewSource.GroupDescriptions.Count == 0)
						{
							var isGenericTypeGroupable = ItemsSource.GetType().IsGenericType
								&& typeof(IItemGroupAware).IsAssignableFrom(ItemsSource.GetType().GetGenericArguments()[0]);
							if (isGenericTypeGroupable || ItemsSource.Count > 0 && ItemsSource[0] is IItemGroupAware)
							{
								ItemsCollectionViewSource.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
							}
						}

						foreach (var groupDescription in ItemsCollectionViewSource.GroupDescriptions)
						{
							groupDescription.CustomSort = GroupComparerService;
						}
					}
					else
					{
						ItemsCollectionViewSource?.GroupDescriptions.Clear();
					}

					CurrentFilterService = FilterService ?? new DefaultFilterService();
					CurrentFilterService.SetFilter(EnableFiltering ? SelectedItemsFilterTextBox?.Text : string.Empty);
				}

				InitializeInternalElements();
			}
		}

		private ItemsControl _selectedItemsControl;
		private ItemsControl SelectedItemsControl
		{
			get => _selectedItemsControl;
			set
			{
				if (_selectedItemsControl != null)
				{
					_selectedItemsControl.Items.CurrentChanged -= SelectedItemsControl_CurrentChanged;
					_selectedItemsControl.PreviewMouseDown -= SelectedItemsControl_OnPreviewMouseDown;
					_selectedItemsControl.KeyUp -= SelectedItemsControl_OnKeyUp;
				}

				_selectedItemsControl = value;

				if (_selectedItemsControl != null)
				{
					_selectedItemsControl.ItemsSource = SelectedItemsInternal;

					if (SelectedItemTemplate == null)
					{
						SelectedItemTemplate = _selectedItemsControl.FindResource(MultiSelectComboBox_SelectedItems_ItemTemplate) as DataTemplate;
					}

					SelectedItemTemplateSelector = new SelectedItemTemplateService(SelectedItemTemplate, _selectedItemsControl.FindResource(MultiSelectComboBox_SelectedItems_Searchable_ItemTemplate) as DataTemplate);
					_selectedItemsControl.Items.CurrentChanged += SelectedItemsControl_CurrentChanged;
					_selectedItemsControl.PreviewMouseDown += SelectedItemsControl_OnPreviewMouseDown;
					_selectedItemsControl.KeyUp += SelectedItemsControl_OnKeyUp;
				}
			}
		}

		private TextBox _selectedItemsFilterTextBox;
		private TextBox SelectedItemsFilterTextBox
		{
			get => _selectedItemsFilterTextBox ?? (SelectedItemsFilterTextBox =
					   VisualTreeService.FindVisualChild<TextBox>(SelectedItemsControl, PART_MultiSelectComboBox_SelectedItemsPanel_Filter_TextBox));
			set
			{
				if (_selectedItemsFilterTextBox != null)
				{
					_selectedItemsFilterTextBox.PreviewTextInput -= SelectedItemsFilterTextBoxPreviewTextInput;
					_selectedItemsFilterTextBox.TextChanged -= SelectedItemsFilterTextBoxTextChanged;
				}

				_selectedItemsFilterTextBox = value;

				if (_selectedItemsFilterTextBox != null)
				{
					_selectedItemsFilterTextBox.PreviewTextInput += SelectedItemsFilterTextBoxPreviewTextInput;
					_selectedItemsFilterTextBox.TextChanged += SelectedItemsFilterTextBoxTextChanged;
				}
			}
		}

		private TextBox _selectedItemsFilterAutoCompleteTextBox;
		private TextBox SelectedItemsFilterAutoCompleteTextBox
		{
			get => _selectedItemsFilterAutoCompleteTextBox ?? (SelectedItemsFilterAutoCompleteTextBox =
					   VisualTreeService.FindVisualChild<TextBox>(SelectedItemsControl, PART_MultiSelectComboBox_SelectedItemsPanel_Filter_AutoComplete_TextBox));
			set
			{
				_selectedItemsFilterAutoCompleteTextBox = value;

				if (AutoCompleteForeground != null && _selectedItemsFilterAutoCompleteTextBox != null)
				{
					_selectedItemsFilterAutoCompleteTextBox.Foreground = AutoCompleteForeground;
				}
			}
		}

		private IComparer _groupComparerService;
		private IComparer GroupComparerService => _groupComparerService ?? (_groupComparerService = new GroupComparerService());

		private IFilterService _currentFilterService;
		private IFilterService CurrentFilterService
		{
			get => _currentFilterService ?? (_currentFilterService = new DefaultFilterService());
			set => _currentFilterService = value;
		}

		private ObservableCollection<object> _selectedItemsInternal;
		private ObservableCollection<object> SelectedItemsInternal
		{
			get => _selectedItemsInternal ?? (_selectedItemsInternal = new ObservableCollection<object>());
			set => _selectedItemsInternal = value;
		}

		static MultiSelectComboBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(typeof(MultiSelectComboBox)));
			EventManager.RegisterClassHandler(typeof(MultiSelectComboBox), Mouse.MouseEnterEvent, new MouseEventHandler(OneMouseEnter), true);
			EventManager.RegisterClassHandler(typeof(MultiSelectComboBox), Mouse.MouseLeaveEvent, new MouseEventHandler(OneMouseLeave), true);
			EventManager.RegisterClassHandler(typeof(MultiSelectComboBox), Mouse.PreviewMouseDownOutsideCapturedElementEvent, new MouseButtonEventHandler(OnPreviewMouseDownOutside), true);
		}

		private object _previousSelectedValue;
		private static void OneMouseLeave(object sender, MouseEventArgs e)
		{
			var comboBox = sender as MultiSelectComboBox;
			if (comboBox.IsDropDownOpen && !comboBox.IsMouseCaptured)
			{
				Mouse.Capture(comboBox, CaptureMode.SubTree);
			}
		}
		private static void OneMouseEnter(object sender, MouseEventArgs e)
		{

			var comboBox = sender as MultiSelectComboBox;
			if (comboBox.IsDropDownOpen && comboBox.IsMouseCaptured)
			{
				comboBox.CaptureMouse();
				comboBox.ReleaseMouseCapture();
			}
		}

		private DateTime _suggestionProviderLastRequest;

		private void DropDownListBoxScrolled(object sender, RoutedEventArgs e)
		{
			var suggestionProvider = SuggestionProvider;
			if (_dropdownListBox == null || suggestionProvider == null)
				return;
			if (DateTime.Now.Subtract(_suggestionProviderLastRequest).TotalSeconds < 0.2)
				return;
			var scrollViewer = VisualTreeService.FindVisualChild<ScrollViewer>(_dropdownListBox, null);
			if (scrollViewer == null || scrollViewer.ContentVerticalOffset / scrollViewer.ScrollableHeight < 0.85)
				return;
			_suggestionProviderLastRequest = DateTime.Now;
			if (!suggestionProvider.HasMoreSuggestions)
				return;
			IsLoadingSuggestions = true;
			DropDownListBoxScrolledAsync().ContinueWith(t => IsLoadingSuggestions = false, TaskContinuationOptions.ExecuteSynchronously);
		}

		private async Task DropDownListBoxScrolledAsync()
		{
			var suggestionProvider = SuggestionProvider;
			var items = await suggestionProvider.GetSuggestionsAsync(_suggestionProviderToken.Token);
			await Dispatcher.BeginInvoke(new Action(() =>
			{
				foreach (var item in items)
					ItemsSource.Add(item);
				_suggestionProviderLastRequest = _suggestionProviderLastRequest.AddSeconds(-1);
			}));
		}

		private static void OnPreviewMouseDownOutside(object sender, MouseButtonEventArgs e)
		{

			MultiSelectComboBox comboBox = sender as MultiSelectComboBox;
			if (comboBox != null)
			{
				comboBox.CloseDropdownMenu(comboBox.ClearFilterOnDropdownClosing, false);
				comboBox.CaptureMouse();
				comboBox.ReleaseMouseCapture();
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			MultiSelectComboBoxGrid = GetTemplateChild(PART_MultiSelectComboBox) as Grid;

			if (MultiSelectComboBoxGrid != null)
			{
				ControlWindow = Window.GetWindow(MultiSelectComboBoxGrid);

				// We expect internal SelectedItemsControl to have its template applied upon InitializeInternalElements.
				ApplyInternalTemplates(MultiSelectComboBoxGrid);
			}

			InitializeInternalElements();
		}

		private void ApplyInternalTemplates(FrameworkElement parent)
		{
			if (parent == null)
				return;

			parent.ApplyTemplate();

			int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < childrenCount; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
				ApplyInternalTemplates(child);
			}
		}

		private void InitializeInternalElements()
		{
			if (SelectedItemsControl == null && MultiSelectComboBoxGrid != null)
			{
				SelectedItemsControl = VisualTreeService.FindVisualChild<ItemsControl>(MultiSelectComboBoxGrid, PART_MultiSelectComboBox_SelectedItemsPanel_ItemsControl);
			}

			if (DropdownListBox == null && MultiSelectComboBoxGrid != null)
			{
				if (DropdownMenu == null)
				{
					DropdownMenu = VisualTreeService.FindVisualChild<Popup>(MultiSelectComboBoxGrid, PART_MultiSelectComboBox_Dropdown);
				}

				if (DropdownMenu != null)
				{
					DropdownListBox = VisualTreeService.FindVisualChild<ListBox>(DropdownMenu.Child, PART_MultiSelectComboBox_Dropdown_ListBox);
				}
			}

			if (ItemsSource != null)
			{
				if (ItemsCollectionViewSource?.Source != ItemsSource)
				{
					ItemsCollectionViewSource = new CollectionViewSource
					{
						Source = ItemsSource
					};
				}

				if (DropdownListBox != null)
				{
					DropdownListBox.ItemsSource = ItemsCollectionViewSource?.View;
				}

				if (ItemsSource.Count > 0)
				{
					UpdateSelectedItemsContainer(ItemsSource);
				}
			}
		}

		public enum SelectionModes
		{
			Multiple = 0,
			Single
		}

		public static readonly DependencyProperty EnableAutoCompleteProperty =
			DependencyProperty.Register("EnableAutoComplete", typeof(bool), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public bool EnableAutoComplete
		{
			get => (bool)GetValue(EnableAutoCompleteProperty);
			set => SetValue(EnableAutoCompleteProperty, value);
		}

		public static readonly DependencyProperty AutoCompleteBackgroundProperty =
			DependencyProperty.Register("AutoCompleteBackground", typeof(Brush), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(Brushes.Gainsboro, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Brush AutoCompleteBackground
		{
			get => (Brush)GetValue(AutoCompleteBackgroundProperty);
			set => SetValue(AutoCompleteBackgroundProperty, value);
		}

		public static readonly DependencyProperty AutoCompleteForegroundProperty =
			DependencyProperty.Register("AutoCompleteForeground", typeof(Brush), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Brush AutoCompleteForeground
		{
			get => (Brush)GetValue(AutoCompleteForegroundProperty);
			set => SetValue(AutoCompleteForegroundProperty, value);
		}

		public static readonly DependencyProperty AutoCompleteMaxLengthProperty =
			DependencyProperty.Register("AutoCompleteMaxLength", typeof(int), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public int AutoCompleteMaxLength
		{
			get => (int)GetValue(AutoCompleteMaxLengthProperty);
			set => SetValue(AutoCompleteMaxLengthProperty, value);
		}

		public static readonly RoutedEvent FilterTextChangedEvent =
			EventManager.RegisterRoutedEvent("FilterTextChanged", RoutingStrategy.Direct,
				typeof(EventHandler<FilterTextChangedEventArgs>), typeof(MultiSelectComboBox));

		public event EventHandler<FilterTextChangedEventArgs> FilterTextChanged
		{
			add => AddHandler(FilterTextChangedEvent, value);
			remove => RemoveHandler(FilterTextChangedEvent, value);
		}

		public static readonly RoutedEvent SelectedItemsChangedEvent =
			EventManager.RegisterRoutedEvent("SelectedItemsChanged", RoutingStrategy.Direct,
				typeof(EventHandler<SelectedItemsChangedEventArgs>), typeof(MultiSelectComboBox));

		public event EventHandler<SelectedItemsChangedEventArgs> SelectedItemsChanged
		{
			add => AddHandler(SelectedItemsChangedEvent, value);
			remove => RemoveHandler(SelectedItemsChangedEvent, value);
		}

		public static readonly DependencyProperty EnableGroupingProperty =
			DependencyProperty.Register("EnableGrouping", typeof(bool), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None, EnableGroupingPropertyChangedCallback));

		public bool EnableGrouping
		{
			get => (bool)GetValue(EnableGroupingProperty);
			set => SetValue(EnableGroupingProperty, value);
		}

		private static void EnableGroupingPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var control = dependencyObject as MultiSelectComboBox;

			if (control?.MultiSelectComboBoxGrid != null)
			{
				control.ItemsCollectionViewSource = control.ItemsCollectionViewSource;
			}
		}

		public static readonly DependencyProperty EnableFilteringProperty =
			DependencyProperty.Register("EnableFiltering", typeof(bool), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None, EnableFilteringPropertyChangedCallback));

		public bool EnableFiltering
		{
			get => (bool)GetValue(EnableFilteringProperty);
			set => SetValue(EnableFilteringProperty, value);
		}

		private static void EnableFilteringPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var control = dependencyObject as MultiSelectComboBox;

			if (control?.MultiSelectComboBoxGrid != null)
			{
				control.ItemsCollectionViewSource = control.ItemsCollectionViewSource;
			}
		}

		public static readonly DependencyProperty FilterServiceProperty =
			DependencyProperty.Register("FilterService", typeof(IFilterService), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, FilterServicePropertyChangedCallback));

		public IFilterService FilterService
		{
			get => (IFilterService)GetValue(FilterServiceProperty);
			set => SetValue(FilterServiceProperty, value);
		}

		private static void FilterServicePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var control = dependencyObject as MultiSelectComboBox;

			if (control?.MultiSelectComboBoxGrid != null)
			{
				control.ItemsCollectionViewSource = control.ItemsCollectionViewSource;
			}
		}



		public static readonly DependencyProperty IsDropDownOpenProperty =
			DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public bool IsDropDownOpen
		{
			get => (bool)GetValue(IsDropDownOpenProperty);
			set => SetValue(IsDropDownOpenProperty, value);
		}

		public static readonly DependencyProperty SelectionModeProperty =
			DependencyProperty.Register("SelectionMode", typeof(SelectionModes), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(SelectionModes.Multiple, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectionModePropertyChangedCallback));

		private static void SelectionModePropertyChangedCallback(DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var control = dependencyObject as MultiSelectComboBox;
			if (control?.MultiSelectComboBoxGrid != null)
			{
				control.UpdateSelectedItemsContainer(control.ItemsSource);
			}
		}

		public SelectionModes SelectionMode
		{
			get => (SelectionModes)GetValue(SelectionModeProperty);
			set => SetValue(SelectionModeProperty, value);
		}

		public static readonly DependencyProperty MaxDropDownHeightProperty =
			DependencyProperty.Register("MaxDropDownHeight", typeof(int), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(360, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public int MaxDropDownHeight
		{
			get => (int)GetValue(MaxDropDownHeightProperty);
			set => SetValue(MaxDropDownHeightProperty, value);
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(IList), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ItemsPropertyChangedCallback));

		public IList ItemsSource
		{
			get => (IList)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		private static void ItemsPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			if (!(dependencyObject is MultiSelectComboBox control))
			{
				return;
			}

			control.InitializeInternalElements();
		}

		public static readonly DependencyProperty SelectedItemsProperty =
			DependencyProperty.Register("SelectedItems", typeof(IList), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedItemsPropertyChangedCallback));

		public bool ClearSelectionOnFilterChanged
		{
			get => (bool)GetValue(ClearSelectionOnFilterChangedProperty);
			set => SetValue(ClearSelectionOnFilterChangedProperty, value);
		}

		public static readonly DependencyProperty ClearSelectionOnFilterChangedProperty =
			DependencyProperty.Register("ClearSelectionOnFilterChanged", typeof(bool), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		private static void RemoveSelectedItems(IList from, IList basedOn, ref Collection<object> itemsRemoved)
		{
			for (var i = from.Count - 1; i >= 0; i--)
			{
				var item = from[i];
				if (RemoveSelectedItem(from, i, basedOn))
				{
					itemsRemoved.Add(item);
				}
			}
		}

		private static bool RemoveSelectedItem(IList from, int index, IList basedOn)
		{
			if (from[index] != null && !basedOn.Contains(from[index]))
			{
				from.RemoveAt(index);

				return true;
			}

			return false;
		}

		private static void AddSelectedItems(IList to, IList basedOn, ref Collection<object> itemsAdded, MultiSelectComboBox control)
		{
			foreach (var item in basedOn)
			{
				if (AddSelectedItem(to, item))
				{
					itemsAdded.Add(item);
					if (control.SelectionMode == SelectionModes.Single)
					{
						control._previousSelectedValue = item;
					}

				}
			}
		}

		private static bool AddSelectedItem(IList to, object item)
		{
			if (to.Contains(item))
			{
				return false;
			}

			if (to.Count > 0 && to[to.Count - 1] == null)
			{
				to.Insert(to.Count - 1, item);
			}
			else
			{
				to.Add(item);
			}

			return true;
		}

		private static void SelectedItemsPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			if (dependencyObject is MultiSelectComboBox control)
			{
				control.SelectedItemsPropertyChangedCallback();
			}
		}

		private void SelectedItemsPropertyChangedCallback()
		{
			CleanUpSelectedItemsNotifyingCollection();
			HandleSelectedItemsChanged();
			InitializeSelectedItemsNotifyingCollection();
		}

		private void CleanUpSelectedItemsNotifyingCollection()
		{
			if (_selectedItemsNotifyingCollection != null)
			{
				_selectedItemsNotifyingCollection.CollectionChanged -= SelectedItemsNotifyingCollection_CollectionChanged;
			}
			_selectedItemsNotifyingCollection = null;
		}
		private void InitializeSelectedItemsNotifyingCollection()
		{
			_selectedItemsNotifyingCollection = SelectedItems as INotifyCollectionChanged;
			if (_selectedItemsNotifyingCollection != null)
			{
				_selectedItemsNotifyingCollection.CollectionChanged += SelectedItemsNotifyingCollection_CollectionChanged;
			}
		}

		private INotifyCollectionChanged _selectedItemsNotifyingCollection;

		private void SelectedItemsNotifyingCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			// Allow client code to perform multiple changes and handle them only once at the end of the message execution cycle.
			if (!_isWaitingToHandleSelectedItemsChanged)
			{
				_isWaitingToHandleSelectedItemsChanged = true;
				Dispatcher.BeginInvoke((Action)delegate
				{
					HandleSelectedItemsChanged();
					_isWaitingToHandleSelectedItemsChanged = false;
				}, DispatcherPriority.ContextIdle);
			}
		}

		private bool _isWaitingToHandleSelectedItemsChanged;

		private void HandleSelectedItemsChanged()
		{
			if (SelectedItems == null)
			{
				return;
			}

			var itemsAdded = new Collection<object>();
			var itemsRemoved = new Collection<object>();

			RemoveSelectedItems(SelectedItemsInternal, SelectedItems, ref itemsRemoved);
			AddSelectedItems(SelectedItemsInternal, SelectedItems, ref itemsAdded, this);

			ToggleDropdownListItemsCheckState(itemsAdded, true);
			ToggleDropdownListItemsCheckState(itemsRemoved, false);

			if (itemsAdded.Count > 0 || itemsRemoved.Count > 0)
			{
				RaiseSelectedItemsChangedEvent(itemsAdded, itemsRemoved, SelectedItemsInternal.Where(a => a != null).ToList());
			}
		}

		public IList SelectedItems
		{
			get => (IList)GetValue(SelectedItemsProperty);
			set => SetValue(SelectedItemsProperty, value);
		}

		public static readonly DependencyProperty ClearFilterOnDropdownClosingProperty =
			DependencyProperty.Register("ClearFilterOnDropdownClosing", typeof(bool), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public bool ClearFilterOnDropdownClosing
		{
			get => (bool)GetValue(ClearFilterOnDropdownClosingProperty);
			set => SetValue(ClearFilterOnDropdownClosingProperty, value);
		}

		public static readonly DependencyProperty DropdownItemTemplateProperty =
			DependencyProperty.Register("DropdownItemTemplate", typeof(DataTemplate), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public DataTemplate DropdownItemTemplate
		{
			get => (DataTemplate)GetValue(DropdownItemTemplateProperty);
			set => SetValue(DropdownItemTemplateProperty, value);
		}

		public static readonly DependencyProperty SelectedItemTemplateProperty =
			DependencyProperty.Register("SelectedItemTemplate", typeof(DataTemplate), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public DataTemplate SelectedItemTemplate
		{
			get => (DataTemplate)GetValue(SelectedItemTemplateProperty);
			set => SetValue(SelectedItemTemplateProperty, value);
		}

		public static readonly DependencyProperty IsEditableProperty =
			DependencyProperty.Register("IsEditable", typeof(bool), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public bool IsEditable
		{
			get => (bool)GetValue(IsEditableProperty);
			set => SetValue(IsEditableProperty, value);
		}

		private static readonly DependencyPropertyKey IsEditModePropertyKey =
			DependencyProperty.RegisterReadOnly("IsEditMode", typeof(bool),
				typeof(MultiSelectComboBox), new PropertyMetadata(false));

		public static readonly DependencyProperty IsEditModeProperty = IsEditModePropertyKey.DependencyProperty;

		public bool IsEditMode => (bool)GetValue(IsEditModeProperty);

		public SelectedItemTemplateService SelectedItemTemplateSelector { get; private set; }

		public DropdownItemTemplateService DropdownItemTemplateSelector { get; private set; }

		public bool DisableFilterUpdateOnDropDownItemSelectionChange
		{
			get => (bool)GetValue(ClearSelectionOnFilterChangedProperty);
			set => SetValue(ClearSelectionOnFilterChangedProperty, value);
		}

		public static readonly DependencyProperty DisableFilterUpdateOnDropDownItemSelectionChangeProperty =
			DependencyProperty.Register("DisableFilterUpdateOnDropDownItemSelectionChange", typeof(bool), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public bool SetFocusOnFirstSelectedItemOnDropDown
		{
			get => (bool)GetValue(ClearSelectionOnFilterChangedProperty);
			set => SetValue(ClearSelectionOnFilterChangedProperty, value);
		}

		public static readonly DependencyProperty SetFocusOnFirstSelectedItemOnDropDownProperty =
			DependencyProperty.Register("SetFocusOnFirstSelectedItemOnDropDown", typeof(bool), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		private string FilterTextApplied { get; set; }

		private bool MultiSelectComboBoxHasFocus { get; set; }


		private bool IsSelectedItem(object item)
		{
			return SelectedItemsInternal.Contains(item);
		}

		private ExtendedListBoxItem GetListViewItem(object item)
		{
			return DropdownListBox?.ItemContainerGenerator.ContainerFromItem(item) as ExtendedListBoxItem;
		}

		private void UpdateSelectedItemsContainer(IList comboBoxItems)
		{
			if (comboBoxItems == null)
			{
				return;
			}

			if (DropdownListBox?.SelectedItem != null)
			{
				UpdateAutoCompleteFilterText(FilterTextApplied, null);
			}

			var itemsAdded = new Collection<object>();
			var itemsRemoved = new Collection<object>();

			foreach (var comboBoxItem in comboBoxItems)
			{
				var listBoxItem = GetListViewItem(comboBoxItem);
				var isSelectedItem = IsSelectedItem(comboBoxItem);
				var enableAwareItem = comboBoxItem as IItemEnabledAware;

				if (enableAwareItem == null || enableAwareItem.IsEnabled)
				{
					if (isSelectedItem && listBoxItem != null && !listBoxItem.IsChecked)
					{
						SelectedItemsInternal.Remove(comboBoxItem);
						itemsRemoved.Add(comboBoxItem);
					}
					else if (!isSelectedItem && listBoxItem != null && listBoxItem.IsChecked)
					{
						if (AddSelectedItem(SelectedItemsInternal, comboBoxItem))
						{
							itemsAdded.Add(comboBoxItem);
						}
					}
				}
				else if (isSelectedItem)
				{
					SelectedItemsInternal.Remove(comboBoxItem);
					itemsRemoved.Add(comboBoxItem);
				}
			}

			ConfigureSingleSelectionMode(ref itemsRemoved);

			var selectedItems = SelectedItemsInternal.Where(a => a != null).ToList();

			UpdateSelectedItems(selectedItems);

			if (itemsAdded.Count > 0 || itemsRemoved.Count > 0)
			{
				RaiseSelectedItemsChangedEvent(itemsAdded, itemsRemoved, selectedItems);
			}

			// Add a placeholder for the filter
			if (!SelectedItemsInternal.Contains(null))
			{
				SelectedItemsInternal.Add(null);
			}
		}

		private void ConfigureSingleSelectionMode(ref Collection<object> itemsRemoved)
		{
			if (SelectionMode != SelectionModes.Single || SelectedItemsInternal.Count(a => a != null) <= 1)
			{
				return;
			}

			var lastSelectedItem = SelectedItemsInternal.LastOrDefault(a => a != null);

			for (var i = SelectedItemsInternal.Count - 1; i >= 0; i--)
			{
				var selectedComboBoxItem = SelectedItemsInternal[i];
				if (selectedComboBoxItem == null || selectedComboBoxItem == lastSelectedItem)
					continue;
				var selectedListBoxItem = GetListViewItem(selectedComboBoxItem);
				if (selectedListBoxItem != null)
				{
					selectedListBoxItem.IsChecked = false;
				}

				SelectedItemsInternal.RemoveAt(i);
				itemsRemoved.Add(selectedComboBoxItem);
			}
		}

		private void AttemptToRemoveSelectedItem(object comboBoxItem)
		{
			var listBoxItem = GetListViewItem(comboBoxItem);
			if (listBoxItem != null)
			{
				listBoxItem.IsChecked = false;
			}

			if (IsDropDownOpen && listBoxItem != null)
			{
				UpdateSelectedItemsContainer(ItemsSource);
			}
			else
			{
				SelectedItemsInternal.Remove(comboBoxItem);

				var selectedItems = SelectedItemsInternal.Where(a => a != null).ToList();
				UpdateSelectedItems(selectedItems);

				RaiseSelectedItemsChangedEvent(new List<object>(), new List<object> { comboBoxItem }, selectedItems);
			}
		}

		private void UpdateSelectedItems(IList selectedItems)
		{
			if (SelectedItems != null)
			{
				for (var i = SelectedItems.Count - 1; i >= 0; i--)
				{
					if (!selectedItems.Contains(SelectedItems[i]))
					{
						SelectedItems.RemoveAt(i);
					}
				}

				foreach (var item in selectedItems)
				{
					if (!SelectedItems.Contains(item))
					{
						SelectedItems.Add(item);
						if (SelectionMode == SelectionModes.Single && _previousSelectedValue != item)
						{
							_previousSelectedValue = item;
						}
					}
				}
			}
		}

		private void RaiseFilterTextChangedEvent()
		{
			Dispatcher.BeginInvoke(new Action(
				delegate
				{
					var args = new FilterTextChangedEventArgs(FilterTextChangedEvent, SelectedItemsFilterTextBox?.Text, DropdownListBox?.Items.Cast<object>().ToList());
					RaiseEvent(args);
				}));
		}

		private void RaiseSelectedItemsChangedEvent(ICollection added, ICollection removed, ICollection selected)
		{
			Dispatcher.BeginInvoke(new Action(
				delegate
				{
					var args = new SelectedItemsChangedEventArgs(SelectedItemsChangedEvent, added, removed, selected);
					RaiseEvent(args);
				}));
		}

		private void MultiSelectComboBox_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			// allows the user to switch to edit mode when control as focus and typing F2 (similar to excel cell behaviour)
			if (e.Key == Key.F2 && !IsEditMode)
			{
				AssignIsEditMode();
			}
		}

		private void MultiSelectComboBoxKeyUp(object sender, KeyEventArgs e)
		{
			if ((e.Key != Key.Down && e.Key != Key.Up) || !IsEditMode || DropdownListBox == null || DropdownListBox.IsKeyboardFocusWithin)
			{
				return;
			}

			IsDropDownOpen = true;

			if (DropdownListBox.Items.Count > 0)
			{
				SetVisualFocusOnItem(DropdownListBox.SelectedItem);

				Dispatcher.BeginInvoke(DispatcherPriority.Input,
					new Action(delegate
					{
						SetKeyBoardFocusOnItem(DropdownListBox.SelectedItem);
					}));
			}
		}

		public bool OpenDropDownListAlsoWhenNotInEditMode
		{
			get => (bool)GetValue(OpenDropDownListAlsoWhenNotInEditModeProperty);
			set => SetValue(OpenDropDownListAlsoWhenNotInEditModeProperty, value);
		}

		public static readonly DependencyProperty OpenDropDownListAlsoWhenNotInEditModeProperty =
			DependencyProperty.Register("OpenDropDownListAlsoWhenNotInEditMode", typeof(bool), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public static readonly DependencyProperty SuggestionProviderProperty =
			DependencyProperty.Register("SuggestionProvider", typeof(ISuggestionProvider), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, SuggestionProviderPropertyChangedCallback));

		private static void SuggestionProviderPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			if (!(dependencyObject is MultiSelectComboBox control))
				return;

			control.UpdateItems(string.Empty);
		}

		public ISuggestionProvider SuggestionProvider
		{
			get => (ISuggestionProvider)GetValue(SuggestionProviderProperty);
			set => SetValue(SuggestionProviderProperty, value);
		}

		public static readonly DependencyProperty IsLoadingSuggestionsProperty =
			DependencyProperty.Register("IsLoadingSuggestions", typeof(bool), typeof(MultiSelectComboBox),
				new FrameworkPropertyMetadata(false));

		public bool IsLoadingSuggestions
		{
			get => (bool)GetValue(IsLoadingSuggestionsProperty);

			set => SetValue(IsLoadingSuggestionsProperty, value);
		}

		private void MultiSelectComboBoxOnPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{


			if (IsScrollBar(e) || IsRemoveItemButton(e) || IsComboBoxItemDataContext(e))
			{
				if (IsComboBoxItemDataContext(e))
				{
					UpdateAutoCompleteFilterText(FilterTextApplied, null);
				}
				return;
			}

			if (!IsEditMode && OpenDropDownListAlsoWhenNotInEditMode == false)
			{
				e.Handled = true;
			}
			else if (IsDropdownButton(e) || IsItemsControl(e))
			{
				if (ClearFilterOnDropdownClosing && DropdownListBox != null && DropdownListBox.IsKeyboardFocusWithin)
				{
					CloseDropdownMenu(true, false);
				}
				else
				{
					IsDropDownOpen = !IsDropDownOpen;

					if (!IsDropDownOpen)
					{
						UpdateAutoCompleteFilterText(FilterTextApplied, null);
					}
				}

				e.Handled = true;
			}

			AssignIsEditMode();
		}

		private void MultiSelectComboBoxGotFocus(object sender, RoutedEventArgs e)
		{
			MultiSelectComboBoxHasFocus = true;
		}

		private void MultiSelectComboBoxLostFocus(object sender, RoutedEventArgs e)
		{
			MultiSelectComboBoxHasFocus = false;

			AttemptToCloseEditMode();
		}

		private void SelectedItemsControl_CurrentChanged(object sender, System.EventArgs e)
		{
			FocusCursorOnFilterTextBox();
		}

		private void SelectedItemsControl_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (IsScrollBar(e))
			{
				return;
			}

			// show/hide the popup listview, when the user clicks into the items control
			if (IsEditMode)
			{
				if (IsEditable && IsRemoveItemButton(e))
				{
					var element = e.OriginalSource as FrameworkElement;
					if (element?.DataContext is object item)
					{
						AttemptToRemoveSelectedItem(item);
					}
				}
				else
				{
					IsDropDownOpen = !IsDropDownOpen;
				}
			}

			AssignIsEditMode();
		}

		private void SelectedItemsControl_OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.OriginalSource is TextBox textBox && IsEditMode)
			{
				var perviousFilterText = FilterTextApplied;
				FilterTextApplied = textBox.Text.Trim();
				textBox.Focus();

				switch (e.Key)
				{
					case Key.Delete:
					case Key.Back when textBox.CaretIndex == 0 && string.IsNullOrEmpty(perviousFilterText):
						if (e.Key == Key.Delete && !string.IsNullOrEmpty(FilterTextApplied))
						{
							textBox.Text = string.Empty;
							FilterTextApplied = string.Empty;

							UpdateItems(string.Empty);
						}
						else if (IsEditable)
						{
							UnSelectComboBoxItem();
						}
						break;
					case Key.Return:
						if (IsDropDownOpen)
						{
							SelectComboBoxItem();
							IsDropDownOpen = false;
						}

						SelectedItemsFilterTextBox.Text = string.Empty;
						FilterTextApplied = string.Empty;

						UpdateItems(string.Empty);
						break;
					case Key.Escape:
						IsDropDownOpen = false;
						UpdateAutoCompleteFilterText(string.Empty, null);
						break;
					default:
						UpdateItems(textBox.Text);

						if (!IsDropDownOpen && EnableFiltering)
						{
							IsDropDownOpen = true;
						}
						break;
				}
			}
		}

		private void DropdownMenuClosed(object sender, System.EventArgs e)
		{
			FocusCursorOnFilterTextBox();
		}

		private void DropdownMenuOpened(object sender, System.EventArgs e)
		{
			if (SelectedItems?.Count > 0 && SetFocusOnFirstSelectedItemOnDropDown)
			{
				SetVisualFocusOnItem(SelectedItems[0]);
			}
			else if (DropdownListBox?.Items.Count > 0)
			{
				SetVisualFocusOnItem(DropdownListBox.Items[0]);
			}
			Mouse.Capture(this, CaptureMode.SubTree);
		}

		private void ControlWindowLocationChanged(object sender, System.EventArgs e)
		{
			ResetDropdownMenu();
		}

		private void ControlWindowDeactivated(object sender, System.EventArgs e)
		{
			if (DropdownMenu != null)
			{
				DropdownMenu.IsOpen = false;
			}
		}

		private void MultiSelectComboBoxGridSizeChanged(object sender, SizeChangedEventArgs e)
		{
			ResetDropdownMenu();
		}

		private void DropdownListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if ((SelectionMode == SelectionModes.Single && SelectedItems != null && SelectedItems.Count == 0 && DisableFilterUpdateOnDropDownItemSelectionChange) || !DisableFilterUpdateOnDropDownItemSelectionChange)
			{
				if (e.AddedItems.Count > 0 && e.AddedItems[0] is object comboBoxItemAdded)
				{
					UpdateAutoCompleteFilterText(FilterTextApplied, comboBoxItemAdded);
				}
				else if (e.RemovedItems.Count > 0 && e.RemovedItems[0] is object comboBoxItemRemoved)
				{
					UpdateAutoCompleteFilterText(FilterTextApplied, comboBoxItemRemoved);
				}
			}

		}

		private void DropdownListBoxPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (DropdownListBox != null && DropdownListBox.SelectedItem is object item)
			{
				switch (e.Key)
				{
					case Key.Space:
						var listBoxItem = GetListViewItem(item);
						listBoxItem.IsChecked = !listBoxItem.IsChecked;

						UpdateSelectedItemsContainer(ItemsSource);

						break;
					case Key.Return:
						SelectComboBoxItem();
						IsDropDownOpen = false;

						SelectedItemsFilterTextBox.Text = string.Empty;
						FilterTextApplied = string.Empty;

						UpdateItems(string.Empty);

						break;
					case Key.Escape:
						if (ClearFilterOnDropdownClosing && DropdownListBox != null && DropdownListBox.IsKeyboardFocusWithin)
						{
							CloseDropdownMenu(true, false);
						}
						else
						{
							IsDropDownOpen = false;
						}

						break;
				}

				if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0 && (e.Key == Key.Down || e.Key == Key.Up))
				{
					var originalSource = e.OriginalSource as FrameworkElement;
					if (originalSource?.DataContext is object comboBoxItem)
					{
						var listBoxItem = GetListViewItem(comboBoxItem);
						listBoxItem.IsChecked = !listBoxItem.IsChecked;

						UpdateSelectedItemsContainer(ItemsSource);

					}
				}
			}
		}

		private void DropdownListBoxPreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var originalSource = e.OriginalSource as FrameworkElement;
			if (DropdownListBox.SelectedItem is object comboBoxItemFrom && (Keyboard.Modifiers & ModifierKeys.Shift) != 0)
			{
				if (originalSource?.DataContext is object comboBoxItemTo)
				{
					var listBoxItemFrom = (IInputElement)DropdownListBox.ItemContainerGenerator.ContainerFromItem(DropdownListBox.SelectedItem);
					var listBoxItemTo = (IInputElement)DropdownListBox.ItemContainerGenerator.ContainerFromItem(comboBoxItemTo);

					if (listBoxItemFrom != null && listBoxItemTo != null)
					{
						var itemIndexFrom = -1;
						var itemIndexTo = -1;

						GetComboBoxItemIndexes(comboBoxItemFrom, ref itemIndexFrom, comboBoxItemTo, ref itemIndexTo);

						if (itemIndexTo > itemIndexFrom && itemIndexTo - itemIndexFrom > 1)
						{
							for (var i = itemIndexFrom + 1; i <= itemIndexTo - 1; i++)
							{
								if (DropdownListBox.Items[i] is object item)
								{
									var listBoxItem = GetListViewItem(item);
									listBoxItem.IsChecked = !listBoxItem.IsChecked;
								}
							}
						}
						else if (itemIndexFrom - itemIndexTo > 1)
						{
							for (var i = itemIndexFrom - 1; i >= itemIndexTo + 1; i--)
							{
								if (DropdownListBox.Items[i] is object item)
								{
									var listBoxItem = GetListViewItem(item);
									listBoxItem.IsChecked = !listBoxItem.IsChecked;
								}
							}
						}
					}

					SetKeyBoardFocusOnItem(comboBoxItemTo);
					UpdateSelectedItemsContainer(ItemsSource);
				}
			}

			if (originalSource?.DataContext is object comboBoxItem)
			{
				var listBoxItem = GetListViewItem(comboBoxItem);
				if (listBoxItem != null)
				{
					if (SelectionMode != SelectionModes.Single || !listBoxItem.IsChecked)
					{
						listBoxItem.IsChecked = !listBoxItem.IsChecked;

						SetKeyBoardFocusOnItem(comboBoxItem);
						UpdateSelectedItemsContainer(ItemsSource);
					}

					if (SelectionMode == SelectionModes.Single)
					{
						CloseDropdownMenu(true, false);
					}
				}
			}
		}

		private void DropDownListBoxItemContainerGenerator_StatusChanged(object sender, System.EventArgs e)
		{
			foreach (var item in SelectedItemsInternal)
			{
				if (item != null && _dropdownListBox.ItemContainerGenerator.ContainerFromItem(item) is ExtendedListBoxItem listBoxItem)
				{
					listBoxItem.IsChecked = true;
				}
			}
		}

		private void SetKeyBoardFocusOnItem(object comboBoxItem)
		{
			if (comboBoxItem != null)
			{
				ItemsCollectionViewSource.View.MoveCurrentTo(comboBoxItem);
				DropdownListBox.Items.MoveCurrentTo(comboBoxItem);

				var listBoxItemTo = (IInputElement)DropdownListBox.ItemContainerGenerator.ContainerFromItem(comboBoxItem);
				if (listBoxItemTo != null)
				{
					listBoxItemTo.Focus();
					DropdownListBox.SelectedItem = listBoxItemTo;
				}
			}
		}

		private void SetVisualFocusOnItem(object comboBoxItem)
		{
			if (DropdownListBox?.Items.Count > 0)
			{
				Dispatcher.BeginInvoke(DispatcherPriority.Input,
					new Action(delegate
					{
						var isEnableAware = comboBoxItem is IItemEnabledAware;
						if (isEnableAware)
						{
							DropdownListBox.SelectedItem = ((IItemEnabledAware)comboBoxItem).IsEnabled
								? comboBoxItem
								: DropdownListBox.Items.Cast<object>().FirstOrDefault(a => ((IItemEnabledAware)a).IsEnabled);
						}
						else
						{
							DropdownListBox.SelectedItem = comboBoxItem;
						}


						if (DropdownListBox.SelectedItem == null)
						{
							ItemsCollectionViewSource.View.MoveCurrentTo(comboBoxItem);
							DropdownListBox.Items.MoveCurrentTo(comboBoxItem);
						}

						if (DropdownListBox.SelectedItem != null)
						{
							DropdownListBox.ScrollIntoView(DropdownListBox.SelectedItem);

							UpdateAutoCompleteFilterText(FilterTextApplied, comboBoxItem);
						}
					}));
			}
		}

		private void GetComboBoxItemIndexes(object comboBoxItemFrom, ref int itemIndexFrom, object comboBoxItemTo, ref int itemIndexTo)
		{
			for (var i = 0; i < DropdownListBox.Items.Count; i++)
			{
				if (!(DropdownListBox.Items[i] is object item))
				{
					continue;
				}

				if (item.Equals(comboBoxItemFrom))
				{
					itemIndexFrom = i;
				}
				else if (item.Equals(comboBoxItemTo))
				{
					itemIndexTo = i;
				}
			}
		}

		private void SelectedItemsFilterTextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!EnableFiltering && !string.IsNullOrEmpty(e.Text))
			{
				e.Handled = true;
			}
		}

		private void SelectedItemsFilterTextBoxTextChanged(object sender, TextChangedEventArgs e)
		{
			var criteria = ((TextBox)e.OriginalSource).Text;

			if (ClearSelectionOnFilterChanged && !string.IsNullOrEmpty(criteria) && SelectionMode == SelectionModes.Single)
			{
				SelectedItems.Clear();
				SetValue(SelectedItemsProperty, SelectedItems);
			}

			UpdateAutoCompleteFilterText(criteria, DropdownListBox != null && DropdownListBox.Items.Count > 0 ? DropdownListBox.Items[0] : null);
		}

		private void ResetDropdownMenu()
		{
			if (DropdownMenu == null)
			{
				return;
			}

			var offset = DropdownMenu.HorizontalOffset;
			DropdownMenu.HorizontalOffset = offset + 0.001;
			DropdownMenu.HorizontalOffset = offset;
		}

		private CancellationTokenSource _suggestionProviderToken;

		private void UpdateItems(string criteria)
		{
			if (SuggestionProvider == null)
			{
				ApplyItemsFilter(criteria);
				return;
			}
			IsLoadingSuggestions = true;
			LoadSuggestionsAsync(criteria).ContinueWith(t => IsLoadingSuggestions = false, TaskContinuationOptions.ExecuteSynchronously);
		}

		private async Task LoadSuggestionsAsync(string criteria)
		{
			var suggestionProvider = SuggestionProvider;
			_suggestionProviderToken?.Cancel(true);

			var suggestionProviderToken = _suggestionProviderToken = new CancellationTokenSource();
			var items = await suggestionProvider.GetSuggestionsAsync(criteria, _suggestionProviderToken.Token);
			await Dispatcher.BeginInvoke(new Action(() =>
			{
				if (suggestionProviderToken.IsCancellationRequested)
				{
					return;
				}

				ItemsSource.Clear();
				foreach (var item in items)
				{
					ItemsSource.Add(item);
				}

				if (!suggestionProviderToken.IsCancellationRequested)
				{
					ApplyItemsFilter(criteria);
				}
			}));
		}

		private void ApplyItemsFilter(string criteria)
		{
			if (EnableFiltering && ItemsCollectionViewSource?.View != null)
			{
				ItemsCollectionViewSource.View.Filter = CurrentFilterService.Filter;
				CurrentFilterService.SetFilter(criteria);

				ItemsCollectionViewSource.View.Refresh();

				if (DropdownListBox?.Items.Count > 0)
				{
					var item = DropdownListBox.Items[0];
					SetVisualFocusOnItem(item);

					UpdateAutoCompleteFilterText(criteria, item);
				}
				else
				{
					UpdateAutoCompleteFilterText(criteria, null);
				}

				RaiseFilterTextChangedEvent();
			}
			else
			{
				UpdateAutoCompleteFilterText(criteria, null);
			}
		}

		private void UpdateAutoCompleteFilterText(string criteria, object item)
		{
			if (EnableAutoComplete && IsDropDownOpen && item != null && !IsSelectedItem(item) && SelectedItemsFilterAutoCompleteTextBox != null)
			{
				var index = criteria?.Length > 0 ? item.ToString().IndexOf(criteria, StringComparison.InvariantCultureIgnoreCase) : 0;
				var autoCompleteText = index > -1 ? item.ToString().Substring(index + (criteria?.Length ?? 0)) : string.Empty;

				if (AutoCompleteMaxLength > 0 && autoCompleteText.Length >= AutoCompleteMaxLength)
				{
					autoCompleteText = autoCompleteText.Substring(0, AutoCompleteMaxLength) + "...";
				}

				SelectedItemsFilterAutoCompleteTextBox.Text = autoCompleteText;

				SelectedItemsFilterAutoCompleteTextBox.Background = AutoCompleteBackground;
			}
			else if (SelectedItemsFilterAutoCompleteTextBox != null)
			{
				SelectedItemsFilterAutoCompleteTextBox.Text = string.Empty;
				SelectedItemsFilterAutoCompleteTextBox.Background = Brushes.Transparent;
			}
		}

		private void AssignIsEditMode()
		{
			SetValue(IsEditModePropertyKey, true);

			FocusCursorOnFilterTextBox();
		}

		private void UnSelectComboBoxItem()
		{
			if (SelectedItemsInternal?.Count > 1)
			{
				// we take the second last item; understanding that the last item is always the searchable textbox
				var item = SelectedItemsInternal[SelectedItemsInternal.Count - 2];
				if (item != null)
				{
					AttemptToRemoveSelectedItem(item);
				}
			}
		}

		private void SelectComboBoxItem()
		{
			if (DropdownListBox.SelectedItem == null && DropdownListBox.Items.Count > 0)
			{
				DropdownListBox.SelectedItem = DropdownListBox.Items[0];
			}

			if (DropdownListBox.SelectedItem != null)
			{
				var selectedItem = DropdownListBox.SelectedItem;

				var listBoxItem = GetListViewItem(selectedItem);
				listBoxItem.IsChecked = true;

				UpdateSelectedItemsContainer(ItemsSource);
			}
		}

		private void FocusCursorOnFilterTextBox()
		{
			if (IsEditMode)
			{
				Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(
					delegate
					{
						if (SelectedItemsControl != null && SelectedItemsFilterTextBox != null)
						{
							SelectedItemsFilterTextBox.Visibility = Visibility.Visible;
							SelectedItemsFilterTextBox.Focus();
							SelectedItemsFilterTextBox.ForceCursor = true;
							SelectedItemsFilterTextBox.ScrollToEnd();
							SelectedItemsFilterTextBox.CaretIndex = SelectedItemsFilterTextBox.Text.Trim().Length;
						}
					}));
			}
		}

		private bool IsComboBoxItemDataContext(RoutedEventArgs e)
		{
			var inline = e.OriginalSource as FrameworkContentElement;
			if (inline?.DataContext != null)
			{
				return true;
			}

			var source = e.OriginalSource as FrameworkElement;
			if (source?.DataContext != null)
			{
				return true;
			}

			var sourceParent = source?.Parent as FrameworkElement;
			if (sourceParent?.DataContext != null)
			{
				return true;
			}

			return false;
		}

		private bool IsItemsControl(RoutedEventArgs e)
		{
			var itemsControl = VisualTreeService.FindVisualTemplatedParent<ItemsControl>(e.OriginalSource as FrameworkElement, PART_MultiSelectComboBox_SelectedItemsPanel_ItemsControl);
			return itemsControl != null;
		}

		private bool IsDropdownButton(RoutedEventArgs e)
		{
			var button = VisualTreeService.FindVisualTemplatedParent<Button>(e.OriginalSource as FrameworkElement, PART_MultiSelectComboBox_Dropdown_Button);
			return button != null;
		}

		private bool IsRemoveItemButton(RoutedEventArgs e)
		{
			var button = VisualTreeService.FindVisualTemplatedParent<Button>(e.OriginalSource as FrameworkElement, PART_MultiSelectComboBox_SelectedItemsPanel_RemoveItem_Button);
			return button != null;
		}

		private bool IsScrollBar(RoutedEventArgs e)
		{
			var source = e.OriginalSource as FrameworkElement;
			if (source?.TemplatedParent?.GetType() == typeof(ScrollBar))
			{
				return true;
			}

			var sourceParent = source?.TemplatedParent as FrameworkElement;
			if (sourceParent?.TemplatedParent?.GetType() == typeof(ScrollBar))
			{
				return true;
			}

			return false;
		}
		private void RestorePreviousSelection()
		{
			SelectedItems.Clear();
			SelectedItems.Add(_previousSelectedValue);
			SetValue(SelectedItemsProperty, SelectedItems);
			SelectedItemsFilterTextBox.Text = string.Empty;
			SelectedItemsFilterAutoCompleteTextBox.Text = string.Empty;
			FilterTextApplied = string.Empty;
			UpdateItems(string.Empty);
		}
		private void AttemptToCloseEditMode()
		{
			if (SelectedItemsControl != null)
			{
				var task = Task.Run(
					delegate
					{
						System.Threading.Thread.Sleep(500);
					});

				task.ContinueWith(
					delegate
					{
						if (CanCloseEditMode())
						{
							Dispatcher.BeginInvoke(
								new Action(delegate
								{
									CloseDropdownMenu(true, true);
								}));
						}
					}
				);
			}
		}

		public void CloseDropdownMenu(bool clearFilter, bool moveFocus)
		{
			if (clearFilter)
			{
				if (SelectedItemsFilterTextBox != null)
				{
					SelectedItemsFilterTextBox.Text = string.Empty;
				}

				FilterTextApplied = string.Empty;
				UpdateItems(string.Empty);
			}

			if (moveFocus)
			{
				if (SelectedItemsFilterTextBox != null)
				{
					SelectedItemsFilterTextBox.Visibility = Visibility.Hidden;
				}

				SetValue(IsEditModePropertyKey, false);
			}

			if (IsDropDownOpen && _previousSelectedValue != null && SelectedItems != null && SelectedItems.Count == 0)

			{
				RestorePreviousSelection();
			}

			IsDropDownOpen = false;
		}



		private bool CanCloseEditMode()
		{
			return !MultiSelectComboBoxHasFocus;
		}

		public void Dispose()
		{
			PreviewKeyUp -= MultiSelectComboBox_PreviewKeyUp;

			if (MultiSelectComboBoxGrid != null)
			{
				MultiSelectComboBoxGrid.PreviewMouseDown -= MultiSelectComboBoxOnPreviewMouseDown;
				MultiSelectComboBoxGrid.GotFocus -= MultiSelectComboBoxGotFocus;
				MultiSelectComboBoxGrid.LostFocus -= MultiSelectComboBoxLostFocus;
				MultiSelectComboBoxGrid.KeyUp -= MultiSelectComboBoxKeyUp;
				MultiSelectComboBoxGrid.SizeChanged -= MultiSelectComboBoxGridSizeChanged;
			}

			if (ControlWindow != null)
			{
				ControlWindow.LocationChanged -= ControlWindowLocationChanged;
				ControlWindow.Deactivated -= ControlWindowDeactivated;
			}

			if (DropdownMenu != null)
			{
				DropdownMenu.Closed -= DropdownMenuClosed;
				DropdownMenu.Opened -= DropdownMenuOpened;
			}

			if (DropdownListBox != null)
			{
				DropdownListBox.SelectionChanged -= DropdownListBoxSelectionChanged;
				DropdownListBox.PreviewMouseUp -= DropdownListBoxPreviewMouseUp;
				DropdownListBox.PreviewKeyDown -= DropdownListBoxPreviewKeyDown;
				DropdownListBox.ItemContainerGenerator.StatusChanged -= DropDownListBoxItemContainerGenerator_StatusChanged;
			}

			if (SelectedItemsControl != null)
			{
				SelectedItemsControl.Items.CurrentChanged -= SelectedItemsControl_CurrentChanged;
				SelectedItemsControl.PreviewMouseDown -= SelectedItemsControl_OnPreviewMouseDown;
				SelectedItemsControl.KeyUp -= SelectedItemsControl_OnKeyUp;
			}

			if (SelectedItemsFilterTextBox != null)
			{
				SelectedItemsFilterTextBox.PreviewTextInput -= SelectedItemsFilterTextBoxPreviewTextInput;
				SelectedItemsFilterTextBox.TextChanged -= SelectedItemsFilterTextBoxTextChanged;
			}
		}

		private void ToggleDropdownListItemsCheckState(IList items, bool isChecked)
		{
			var listItems = items
				.Cast<object>()
				.Select(GetListViewItem)
				.Where(e => e != null && (e as IItemEnabledAware)?.IsEnabled != false);

			foreach (var item in listItems)
			{
				item.IsChecked = isChecked;
			}
		}
	}
}
