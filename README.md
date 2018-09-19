# Sdl.MultiSelectComboBox (WPF Custom Control)

## Overview
The multi selection combo box is a WPF custom control with multiple item selection capabilities, along with customizable features to group, sort and filter items in the collection.
![Usage Demonstration](https://github.com/sdl/Multiselect-ComboBox/blob/master/Resources/SDL.MultiSelectComboBox.Usage.gif)

## Components and Features
![Components](https://github.com/sdl/Multiselect-ComboBox/blob/master/Resources/Sdl.MultiSelectComboBox.Components.png)

## Selected Items Panel
The selected items are displayed like tags with a remove button  They can be added or removed from the selected items collection, by selecting them from the items present in the Dropdown Menu list.  Additionally, items can also be removed by interacting with them directly from the Selected Items Panel, as follows:

| Device | Action | Description |
| ------ | ------ | ------ |
| Keyboard | Backspace or Delete key | If the Filter Criteria is empty, then the last item in the selected items collection is removed. |
| Mouse | Remove Item Button | The item is removed from the selected items collection |

### Visual States
The control has two visual states, Readonly and EditMode, which is identified by the IsEditMode property.  When the control is in readonly mode, the items cannot be edited from the view.  To switch to edit mode, select the control in the view or hit (F2) when the control has focus.  Once the control is in edit mode, the items can be filtered, selected or removed.  To switch back to readonly, move focus away from the control by selecting any other control in the view or by deactivating the parent window.

| Visual state | Description | Example |
| ------ | ------ | ------ |
| Readonly | The Dropdown Menu is collapsed and items present in the Selected Items Panel cannot be edited. | TODO: image |
| EditMode | The Dropdown Menu can be expanded or collapsed when the button is clicked and items present in the Selected Items Panel can be edited. | TODO: image |

### Filter Criteria
When you type a character in the text area, the control applies a filter on the collection and suggests a list of items matching that criteria in the Dropdown Menu list.  The developer can override the default filter service, based on their own business logic requirements, that implements IFilterService.
TODO: image
Depending on whether or not the ClearFilterOnDropdownClosing property is set to true and Dropdown Menu list as keyboard focus, the filter criteria is cleared automatically as the Dropdown Menu is closing.
| Keyboard focus | Action |
| ------ | ------ |
| True | The filter criteria is cleared as the Dropdown Menu is closing. |
| False | No attempt is made to change the current filter. |

## Dropdown Menu

Presents a list of suggestions that can be selected by the user.  If no Filter Criteria is applied, then all items in the collection are displayed
Visibility

The Dropdown Menu can be expanded only when the control is in EditMode.
| Visibility | Actions |
| ------ | ------ |
| Expand | * Left mouse click anywhere within the control area, with exception to the Remove Item Button of the item.<br/> *  Down arrow key on the keyboard when the control has focus.<br/> * Change the filter criteria, by typing characters in the text area. |
| Collapse | * Left mouse click anywhere within the control area, with exception to the Remove Item Button of the item. <br/> * Return key.<br/> -- The item that has focus in the list is selected.<br/> -- The filter criteria is removed <br/> * Esc key<br/> * Move focus away from the control by selecting any other control in the view or by deactivating the parent window.|

### Item Group
The items in the collection can be grouped by implementing IItemGroupAware.  In addition to the header name, this interface exposes a property to manage the order in which the group headers are displayed. 
TODO: image

### Item Sorting
The sort order is based on the order of the items in the collection that was received when the ItemsSource is set.

### Item Selection
Items can be added or removed from the selected items collection, by selecting them from the items present in the Dropdown Menu list.
| Device | Actions | Description |
| ------ | ------ | ------ |
| Mouse | Left mouse click | The item that has selection focus in the list is selected/unselected. |
| Mouse | Shift + Left mouse click | All items between the previous and current item that has focus are selected. |
| Keyboard | Return key | * The item that has focus in the list is selected.<br/> * The filter criteria is removed.<br/> * The Dropdown Menu is closed |
| Keyboard | Space key | The item that has focus in the list is selected/unselected. |
| Keyboard | Shift + Up or Down key | The item that has focus in the list is selected/unselected. |

### Disabled Item
Implement IItemEnabledAware to identify whether or not the items are enabled.  When an item is not enabled, then it will not be selectable from the Dropdown Menu list and removed from the Selected Items automatically.

### Selected Item
Items in the Dropdown Menu list can be selected/unselected via the Mouse or Keyboard.  When the item is selected, the style is updated to reflect a selected state.


## Example Project
The example project demonstrates how to implement the Sdl.MultiSelectComboBox custom control to your project, and provides good examples in understanding the controls behaviours.
TODO: image

## Examples
The following example creates an Sdl.MultiSelectComboBox.  The example populates the control by binding the ItemsSource property to a collection of type object, that implements IItemEnabledAware and IItemGroupAware.  The example also binds the SelectedItemsChangedBehaviour to a command to receive a notification of SelectedItemsChangedEventArgs whenever the selected items collection changes and then display those results in a TextBlock on the view.
Additionally, both the selected and dropdown list item templates are customized to display an image along with the item name.

**Example.xaml**
```html
<Window x:Class="MultiSelectComboBoxExample.Example"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:sdl="http://schemas.sdl.com/xaml"
        xmlns:models="clr-namespace:MultiSelectComboBoxExample.Models"
        d:DataContext="{d:DesignInstance {x:Type models:LanguageItems}}"
        mc:Ignorable="d"
        Title="Example" Height="250" Width="400">
    <Window.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Resources/MultiSelectComboBox.Resources.xaml"/>              
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Window.Resources>
    <Grid>
        <StackPanel Orientation="Vertical">
            <sdl:MultiSelectComboBox Height="100"
                ItemsSource="{Binding Items}"
                IsEditable="true"
                SelectionMode="Multiple"
                SelectedItemTemplate="{StaticResource MultiSelectComboBox.SelectedItems.ItemTemplate}"
                DropdownItemTemplate="{StaticResource MultiSelectComboBox.Dropdown.ListBox.ItemTemplate}"
                sdl:SelectedItemsChangedBehaviour.SelectedItemsChanged="{Binding SelectedItemsChangedCommand}"/>
            <TextBlock Text="{Binding Path=EventLog}" Height="100"/>
        </StackPanel>
    </Grid>
</Window>
```

**DataTemplate: MultiSelectComboBox.Dropdown.ListBox.ItemTemplate**
```html
<DataTemplate x:Key="MultiSelectComboBox.Dropdown.ListBox.ItemTemplate" DataType="models:LanguageItem">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="*"/>
            <ColumnDefinition Width="30"/>
        </Grid.ColumnDefinitions>
 
        <Image Grid.Column="0" Style="{StaticResource MultiSelectComboBox.Image.Style}"/>
        <TextBlock Grid.Column="1" Text="{Binding Path=Name}" Style="{StaticResource MultiSelectComboBox.DefaultTextBlock.Style}"/>
        <TextBlock Grid.Column="2" Text="{Binding Path=SelectedOrder}" Margin="5,0"
                Foreground="{StaticResource MultiSelectComboBox.DropDown.ListBoxItem.SelectedOrder.Foreground}"
                VerticalAlignment="Center" HorizontalAlignment="Center">
            <TextBlock.Style>
                <Style TargetType="TextBlock">
                    <Style.Triggers>
                        <DataTrigger Binding="{Binding Path=SelectedOrder}" Value="-1">
                            <Setter Property="Visibility" Value="Hidden" />
                        </DataTrigger>
                    </Style.Triggers>
                </Style>
            </TextBlock.Style>
        </TextBlock>
    </Grid>
</DataTemplate>
```
**DataTemplate: MultiSelectComboBox.SelectedItems.ItemTemplate**
```html
<DataTemplate x:Key="MultiSelectComboBox.SelectedItems.ItemTemplate" DataType="models:LanguageItem">
    <StackPanel Orientation="Horizontal" Margin="0,-4">
        <Image Style="{StaticResource MultiSelectComboBox.Image.Style}" Margin="2,0,4,-1"/>
        <TextBlock Text="{Binding Path=Name}" Style="{DynamicResource MultiSelectComboBox.DefaultTextBlock.Style}" Margin="2,0" />
    </StackPanel>
</DataTemplate>
```
**Style: MultiSelectComboBox.Image.Style**
```html
<Style x:Key="MultiSelectComboBox.Image.Style" TargetType="Image">
    <Setter Property="Stretch" Value="Fill"/>
    <Setter Property="Source" Value="{Binding Path=Image}"/>
    <Setter Property="RenderOptions.BitmapScalingMode" Value="NearestNeighbor"/>
    <Setter Property="VerticalAlignment" Value="Center"/>
    <Setter Property="RenderOptions.EdgeMode" Value="Aliased"/>
    <Setter Property="Margin" Value="0,0,4,0"/>
    <Setter Property="Width" Value="{Binding Path=ImageSize.Width}"/>
    <Setter Property="Height" Value="{Binding Path=ImageSize.Height}"/>
</Style>
```
**Style: MultiSelectComboBox.DefaultTextBlock.Style**
```html
<Style x:Key="MultiSelectComboBox.DefaultTextBlock.Style" TargetType="TextBlock">
    <Setter Property="FontFamily" Value="{StaticResource MultiSelectComboBox.Text.FontFamily}"/>
    <Setter Property="FontSize" Value="11"/>
    <Setter Property="HorizontalAlignment" Value="Stretch"/>
    <Setter Property="VerticalAlignment" Value="Center"/>
    <Setter Property="Control.VerticalContentAlignment" Value="Center"/>
    <Setter Property="TextWrapping" Value="NoWrap"/>
    <Setter Property="TextTrimming" Value="CharacterEllipsis"/>
    <Setter Property="Padding" Value="1"/>
    <Setter Property="Margin" Value="1"/>
    <Setter Property="Foreground" Value="{StaticResource MultiSelectComboBox.Text.Foreground}"/>
    <Setter Property="TextAlignment" Value="Left"/>
    <Style.Triggers>
        <DataTrigger Binding="{Binding IsEnabled, RelativeSource={RelativeSource AncestorType={x:Type ToggleButton}}}" Value="False">
            <Setter Property="Foreground" Value="{StaticResource MultiSelectComboBox.Text.Disabled.Foreground}" />
        </DataTrigger>
    </Style.Triggers>
</Style>
```

The following example defines the data context (i.e. models:LanguageItems) that the Sdl.MultiSelectComboBox binds to from the previous example.

**LanguageItems : INotifyPropertyChanged**
```c#
public class LanguageItems : INotifyPropertyChanged
{
    private List<LanguageItem> _items;
    private ObservableCollection<LanguageItem> _selectedItems;
 
    public LanguageItems()
    {
        SelectedItemsChangedCommand = new SelectedItemsChangedCommand(UpdateEventLog, UpdateSelectedItems);
 
        var group = new LanguageItemGroup(0, "All Items");
        Items = new List<LanguageItem>
        {
            new LanguageItem
            {
                Id = "en-US",
                Name= "English (United States)",
                Group = group
            },
            new LanguageItem
            {
                Id = "it-IT",
                Name= "Italian (Italy)",
                Group = group
            },
            new LanguageItem
            {
                Id = "de-DE",
                Name= "German (Germany)",
                Group = group
            }
        };
    }
     
    public ICommand SelectedItemsChangedCommand { get; }
 
    public List<LanguageItem> Items
    {
        get => _items ?? (_items = new List<LanguageItem>());
        set => _items = value;
    }
 
    public ObservableCollection<LanguageItem> SelectedItems
    {
        get => _selectedItems ?? (_selectedItems = new ObservableCollection<LanguageItem>());
        set
        {
            _selectedItems = value;
            OnPropertyChanged(nameof(SelectedItems));
        }
    }
 
    public string EventLog { get; set; }
 
    private void UpdateEventLog(string action, string text)
    {
        EventLog += action + " => " + text + "\r\n";
        OnPropertyChanged(nameof(EventLog));
    }
 
    private void UpdateSelectedItemsCount(int count)
    {
        SelectedItemsCount = count;
        OnPropertyChanged(nameof(SelectedItemsCount));
    }
 
    private void UpdateSelectedItems(ICollection selected)
    {           
        foreach (var item in _items)
        {
            var selectedItemIndex = selected.Cast<LanguageItem>().ToList().IndexOf(item);
            item.SelectedOrder = selectedItemIndex > -1 ? selectedItemIndex + 1 : -1;
        }
 
        UpdateSelectedItemsCount(selected.Count);
    }
 
    public event PropertyChangedEventHandler PropertyChanged;
 
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }       
 }
```
**LanguageItem : IItemEnabledAware, IItemGroupAware, INotifyPropertyChanged**
```c#
public class LanguageItem : IItemEnabledAware, IItemGroupAware, INotifyPropertyChanged
{
    private string _id;
    private string _name;
    private bool _isEnabled;
    private int _selectedOrder;
    private IItemGroup _group;
    private BitmapImage _image;
    private Size _imageSize;
 
    public LanguageItem()
    {
        _isEnabled = true;
        _selectedOrder = -1;
    }
 
    /// <summary>
    /// Unique id in the collection
    /// </summary>
    public string Id
    {
        get => _id;
        set
        {
            if (_id != null && string.Compare(_id, value, StringComparison.InvariantCulture) == 0)
            {
                return;
            }
 
            _id = value;
            OnPropertyChanged(nameof(Id));
        }
    }
 
    /// <summary>
    /// The item name.
    ///
    /// The filter criteria is applied on this property when using the default filter service.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (_name != null && string.Compare(_name, value, StringComparison.InvariantCulture) == 0)
            {
                return;
            }
 
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
 
    /// <summary>
    /// Identifies whether the item is enabled or not.
    ///
    /// When the item is not enabled, then it will not be selectable from the dropdown list and removed
    /// from the selected items automatically.
    /// </summary>
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (_isEnabled.Equals(value))
            {
                return;
            }
 
            _isEnabled = value;
            OnPropertyChanged(nameof(IsEnabled));
        }
    }
 
    /// <summary>
    /// The order in which the items are added to the selected collection. 
    /// 
    /// This order is independent to the group and sort order of the items in the collection. This selected
    /// order is visible in each of the selected items from the dropdown list and visually represented by
    /// the order of the items in the Selected Items Panel.
    /// </summary>
    public int SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            if (_selectedOrder.Equals(value))
            {
                return;
            }
 
            _selectedOrder = value;
            OnPropertyChanged(nameof(SelectedOrder));
        }
    }
 
    /// <summary>
    /// Identifies the name and order of the group header
    /// </summary>
    public IItemGroup Group
    {
        get => _group;
        set
        {
            if (_group != null && _group.Equals(value))
            {
                return;
            }
 
            _group = value;
            OnPropertyChanged(nameof(Group));
        }
    }
 
    /// <summary>
    /// The item Image.
    ///
    /// Use the ImageSize to identify the space required to display the image in the view.
    /// </summary>
    public BitmapImage Image
    {
        get => _image;
        set
        {
            _image = value;
            OnPropertyChanged(nameof(Image));
        }
    }
 
    /// <summary>
    /// The image size.
    ///
    /// Measures the width and height that is required to display the image.
    /// </summary>
    public Size ImageSize
    {
        get => _imageSize;
        set
        {
            if (_imageSize.Equals(value))
            {
                return;
            }
 
            _imageSize = value;
            OnPropertyChanged(nameof(ImageSize));
        }
    }
 
    public CultureInfo CultureInfo { get; set; }
 
    public override string ToString()
    {
        return Name;
    }
 
    public event PropertyChangedEventHandler PropertyChanged;
 
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```
**SelectedItemsChangedCommand : ICommand**
```c#
public class SelectedItemsChangedCommand : ICommand
{
    private readonly Action<string, string> _updateEventLog;
    private readonly Action<ICollection> _updateSelectedItems;
 
    public SelectedItemsChangedCommand(Action<string, string> updateEventLog, Action<ICollection>)
    {
        _updateEventLog = updateEventLog;
        _updateSelectedItems = updateSelectedItems;
    }
 
    public bool CanExecute(object parameter)
    {
        return parameter is SelectedItemsChangedEventArgs && _updateEventLog != null;
    }
 
    public void Execute(object parameter)
    {
        if (parameter is SelectedItemsChangedEventArgs args)
        {
            _updateSelectedItems?.Invoke(args.Selected);
 
            var addedItems = GetAggregatedText(args.Added);
            var removedItems = GetAggregatedText(args.Removed);
            var selectedItems = GetAggregatedText(args.Selected);
 
            var report = "Added - " + args.Added?.Count + (!string.IsNullOrEmpty(addedItems) ? " (" + TrimToLength(addedItems, 100) + ") " : string.Empty)
							+ ", Removed - " + args.Removed?.Count + (!string.IsNullOrEmpty(removedItems) ? " (" + TrimToLength(removedItems, 100) + ") " : string.Empty)
							+ ", Selected - " + args.Selected?.Count + (!string.IsNullOrEmpty(selectedItems) ? " (" + TrimToLength(selectedItems, 100) + ") " : string.Empty);

				_updateEventLog?.Invoke("Selected Changed", report);
        }
    }
         
    public event EventHandler CanExecuteChanged;
 
    private string TrimToLength(string text, int length)
    {
        if (text?.Length > length)
        {
            text = text.Substring(0, length) + "...";
        }
 
        return text;
    }
 
    private static string GetAggregatedText(ICollection items)
    {
        var itemsText = string.Empty;
        return items?.Cast<LanguageItem>().Aggregate(itemsText, (current, item) => current + ((!string.IsNullOrEmpty(current) ? ", " : string.Empty) + item.Id));
    }
 }
```
**LanguageItemGroup : IItemGroup**
```c#
public class LanguageItemGroup : IItemGroup
{
    public int Order { get; set; }
    public string Name { get; }
 
    public LanguageItemGroup(int index, string name)
    {
        Order = index;
        Name = name;
    }
 
    public override string ToString()
    {
        return Name;
    }
}
```

## Remarks
The Sdl.MultiSelectComboBox allows the user to select an item from a Dropdown Menu list and optionally apply a filter on the list by typing text in the text box of the Selected Items Panel. The SelectionMode and IsEditable properties specify how the Item Selection behaves, especially when the user is interacting with items from the Selected Items Panel, as follows:
|  | SelectionMode.Multiple (default) | SelectionMode.Single |
| ------ | ------ | ------ |
| IsEditable is true (default) | Multiple items can be selected/unselected from the Dropdown Menu list<br/>The Remove Item Button is displayed in each of the selected items in the Selected Items Panel.<br/>Items in the Selected Items Panel can be removed when the user hits the Delete or Back key | A single item can be selected from the Dropdown Menu list.  When a new item is selected, it substitutes the previously selected item in the Selected Items Panel.<br/>The Remove Item Button is displayed in each of the selected items in the Selected Items Panel.<br/>Items in the Selected Items Panel can be removed when the user hits the Delete or Back key |
| IsEditable is false | Multiple items can be selected/unselected from the Dropdown Menu list.<br/>The Remove Item Button is not displayed in each of the selected items in the Selected Items Panel.<br/>Items in the Selected Items Panel cannot be removed when the user hits the Delete or Back key | A single item can be selected from the Dropdown Menu list.  When a new item is selected, it substitutes the previously selected item in the Selected Items Panel.<br/>The Remove Item Button is not displayed in each of the selected items in the Selected Items Panel.<br/>Items in the Selected Items Panel cannot be removed when the user hits the Delete or Back key |

### Customizing the Style and template
You can modify the default Style and ControlTemplate to give the control a unique appearance. For information about modifying a control's style and template, see Customizing the Appearance of an Existing Control by Creating a ControlTemplate and Styling controls. The default style, template, and resources that define the look of the control are included in the generic.xaml file.

This table shows the resources used by the Sdl.MultiSelectComboBox control.

| Resource key | Description |
| ------ | ------ |
MultiSelectComboBox.DropDown.Button.Background |	Drop down button background color at rest
MultiSelectComboBox.DropDown.Button.Border |	Drop down button border brush at rest
MultiSelectComboBox.DropDown.Button.Disabled.Background |	Drop down button background color when disabled
MultiSelectComboBox.DropDown.Button.Disabled.Border |	Drop down button border brush when disabled
MultiSelectComboBox.DropDown.Button.Disabled.Foreground |	Drop down button foreground color when disabled
MultiSelectComboBox.DropDown.Button.MouseOver.Background |	Drop down button background color when the mouse is hovering
MultiSelectComboBox.DropDown.Button.MouseOver.Border |	Drop down button border brush when the mouse is hovering
MultiSelectComboBox.DropDown.Button.Pressed.Background |	Drop down button background color when the button is pressed
MultiSelectComboBox.DropDown.Button.Pressed.Border |	Drop down button border brush when the button is pressed
MultiSelectComboBox.DropDown.ListBox.GroupHeader.Background |	Drop down list group header background color
MultiSelectComboBox.DropDown.ListBox.GroupHeader.Foreground |	Drop down list group header foreground color
MultiSelectComboBox.DropDown.ListBoxItem.Selector.Background |	Drop down list item background color of the item when it has selection focus
MultiSelectComboBox.DropDown.ListBoxItem.Selector.Border |	Drop down list item border brush of the item when it has selection focus
MultiSelectComboBox.DropDown.ListBoxItem.Selected.Background |	Drop down list item background color of the item when it is selected
MultiSelectComboBox.DropDown.ListBoxItem.Selected.Border |	Drop down list item border brush of the item when it is selected
MultiSelectComboBox.SelectedItem.Border|	Selected item border brush that surrounds the selected item in the Selected Items Panel
MultiSelectComboBox.SelectedItem.Button.Foreground |	Selected Item button foreground color
MultiSelectComboBox.SelectedItem.Button.Hover.Background |	Selected Item button foreground when the mouse if hovering over it
MultiSelectComboBox.SelectedItem.Button.Light.Foreground |	Selected Item button light foreground color
MultiSelectComboBox.SelectedItemsPanel.Border |	Selected Items Panel border brush
MultiSelectComboBox.Text.Disabled.Foreground |	Default text foreground color when disabled
MultiSelectComboBox.Text.FontFamily |	Default FontFamily for all text in the control
MultiSelectComboBox.Text.Foreground |	Default text foreground color

The sample project includes an example of a customized version of control template style for the Sdl.MultiSelectComboBox control, where the item selection styles are modified and a popup control is displayed with the CultureInfo properties when hovering over the language items in the Selected Items Panel.  Make reference to the following.
TODO: image

## API

**interface IFilterService**
```c#
/// <summary>
/// The filter service that is used to apply a custom filter on the items that are displayed
/// from the collection.
/// </summary>
public interface IFilterService
{
    /// <summary>
    /// The filter criteria should be set before applying the Filter
    /// </summary>
    /// <param name="criteria">The filter criteria that is applied</param>
    void SetFilter(string criteria);

    /// <summary>
    /// Gets or sets a callback used to determine if an item is suitable for inclusion in the view.
    /// </summary>
    Predicate<object> Filter { get; set; }
}
```
**interface IItemEnabledAware**
```c#
/// <summary>
/// IEnabledAware - Identifies whether the item is enabled or not.
/// </summary>
public interface IItemEnabledAware
{
    /// <summary>
    /// Identifies whether the item is enabled or not.
    /// 
    /// When the item is not enabled, then it will not be selectable from the dropdown list and removed
    /// from the selected items automatically.
    /// </summary>
    bool IsEnabled { get; set; }
}
```
**interface IItemGroup**
```c#
/// <summary>
/// Identifies the name and order of the group header
/// </summary>
public interface IItemGroup
{
    /// <summary>
    /// The group name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The display order of the group headers.
    /// </summary>
    int Order { get; set; }
}
```
**interface IItemGroupAware**
```c#
/// <summary>
/// IGroupAware - provides support for grouping the items
/// </summary>
public interface IItemGroupAware
{
    /// <summary>
    /// Used to identify the name and order of the group header
    /// </summary>
    IItemGroup Group { get; set; }
}
```
### EventArgs
**FilterTextChangedEventArgs : RoutedEventArgs**
```c#
/// <summary>
/// Raised when the filter criteria has changed
/// </summary>
public class FilterTextChangedEventArgs : RoutedEventArgs
{    
    /// <summary>
    /// The filter critera applied on the collection of items
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// The filtered list of items
    /// </summary>
    public ICollection Items { get; }        
}
```
**SelectedItemsChangedEventArgs : RoutedEventArgs**
```c#
/// <summary>
/// Raised when the selected items collection is modified
/// </summary>
public class SelectedItemsChangedEventArgs : RoutedEventArgs
{        
    /// <summary>
    /// Items added to the collection
    /// </summary>
    public ICollection Added { get; }

    /// <summary>
    /// Items removed from the collection
    /// </summary>
    public ICollection Removed { get; }

    /// <summary>
    /// The selected items
    /// </summary>
    public ICollection Selected { get; }        
}
```

## Fields
| Dependency Property | Description |
| ------ | ------ |
AutoCompleteBackgroundProperty |	Identified the [AutoCompleteBackground](#autocompletebackground) dependency property.
AutoCompleteForegroundProperty |	Identified the AutoCompleteForeground dependency property.
AutoCompleteMaxLengthProperty |	Identified the AutoCompleteMaxLength dependency property.
ClearFilterOnDropdownClosingProperty |	Identifies the ClearFilterOnDropdownClosing dependency property.
DropdownItemTemplateProperty |	Identifies the DropdownItemTemplate dependency property.
EnableAutoCompleteProperty |	Identified the EnableAutoComplete dependency property.
EnableFilteringProperty |	Identifies the EnableFiltering dependency property.
EnableGroupingProperty |	Identifies the EnableGrouping dependency property.
FilterServiceProperty |	Identifies the FilterService dependency property.
IsDropDownOpenProperty |	Identifies the IsDropDownOpen dependency property.
IsEditableProperty |	Identifies the IsEditable dependency property.
IsEditModeProperty |	Identifies the IsEditMode dependency property.
ItemsSourceProperty |	Identifies the ItemsSource dependency property.
MaxDropDownHeightProperty |	Identifies the MaxDropDownHeight dependency property.
SelectedItemsProperty |	Identifies the SelectedItems dependency property.
SelectedItemTemplateProperty |	Identifies the SelectedItemTemplate dependency property.
SelectionModeProperty |	Identified the SelectionMode dependency property.

## Properties
| Property | Description |
| ------ | ------ |
AllowDrop |	Gets or sets a value indicating whether this element can be used as the target of a drag-and-drop operation. This is a dependency property. (Inherited from UIElement)
<a name="autocompletebackground"></a>AutoCompleteBackground | Gets or sets background brush used when displaying the autocomplete content for the Filter Criteria
AutoCompleteForeground | Gets or sets foreground brush used when displaying the autocomplete content for the Filter Criteria
AutoCompleteMaxLength | Gets or sets the maximum length of autocomplete content displayed for the Filter Criteria
Background	| Gets or sets a brush that describes the background of a control.(Inherited from Control)
BindingGroup | Gets or sets the BindingGroup that is used for the element.(Inherited from FrameworkElement)
BorderBrush	| Gets or sets a brush that describes the border background of a control.(Inherited from Control)
BorderThickness | Gets or sets the border thickness of a control.(Inherited from Control)
ClearFilterOnDropdownClosing | Gets or sets whether the Filter Criteria is cleared as the Dropdown Menu is closing while it has keyboard focus. (default) true
Clip | Gets or sets the geometry used to define the outline of the contents of an element. This is a dependency property.(Inherited from UIElement)
ClipToBounds | Gets or sets a value indicating whether to clip the content of this element (or content coming from the child elements of this element) to fit into the size of the containing element. This is a dependency property.(Inherited from UIElement)
CommandBindings | Gets a collection of CommandBinding objects associated with this element. A CommandBinding enables command handling for this element, and declares the linkage between a command, its events, and the handlers attached by this element.(Inherited from UIElement)
ContextMenu | Gets or sets the context menu element that should appear whenever the context menu is requested through user interface (UI) from within this element.(Inherited from FrameworkElement)
Cursor | Gets or sets the cursor that displays when the mouse pointer is over this element.(Inherited from FrameworkElement)
DataContext | Gets or sets the data context for an element when it participates in data binding.(Inherited from FrameworkElement)
DropdownItemTemplate | Gets or sets the DataTemplate used to display each item in the Dropdown Menu list
EnableAutoComplete | Gets or sets whether autocomplete feature is enabled for the Filter Criteria
EnableFiltering | Gets or sets whether filtering is enabled
EnableGrouping | Gets or sets whether grouping is enabled
Effect | Gets or sets the bitmap effect to apply to the UIElement. This is a dependency property.(Inherited from UIElement)
FilterService |	Gets or sets a custom filter service that is used when filtering items in the collection.  If the filter service is null, then a default service is used that applies a filter on the override string ToString()
FlowDirection | Gets or sets the direction that text and other user interface (UI) elements flow within any parent element that controls their layout.(Inherited from FrameworkElement)
Focusable | Gets or sets a value that indicates whether the element can receive focus. This is a dependency property.(Inherited from UIElement)
FocusVisualStyle | Gets or sets a property that enables customization of appearance, effects, or other style characteristics that will apply to this element when it captures keyboard focus.(Inherited from FrameworkElement)
FontFamily | Gets or sets the font family of the control.(Inherited from Control)
FontSize | Gets or sets the font size.(Inherited from Control)
FontStretch | Gets or sets the degree to which a font is condensed or expanded on the screen.(Inherited from Control)
FontWeight | Gets or sets the weight or thickness of the specified font.(Inherited from Control)
ForceCursor | Gets or sets a value that indicates whether this FrameworkElement should force the user interface (UI) to render the cursor as declared by the Cursor property.(Inherited from FrameworkElement)
Foreground | Gets or sets a brush that describes the foreground color.(Inherited from Control)
Height | Gets or sets the suggested height of the element.(Inherited from FrameworkElement)
HorizontalAlignment | Gets or sets the horizontal alignment characteristics applied to this element when it is composed within a parent element, such as a panel or items control.(Inherited from FrameworkElement)
HorizontalContentAlignment | Gets or sets the horizontal alignment of the control's content.(Inherited from Control)
InputBindings | Gets the collection of input bindings associated with this element.(Inherited from UIElement)
InputScope | Gets or sets the context for input used by this FrameworkElement.(Inherited from FrameworkElement)
IsDropDownOpen | Gets or sets a value that indicates whether the Dropdown Menu is currently open.
IsEditable | Gets or sets a value that indicates whether the user can edit items in the Selected Items Panel. The default value is true.
IsEditMode | Gets the value identifying the Visual State of the control (i.e. Readonly or EditMode)
IsEnabled | Gets or sets a value indicating whether this element is enabled in the user interface (UI). This is a dependency property.(Inherited from UIElement)
IsFocused | Gets a value that determines whether this element has logical focus. This is a dependency property.(Inherited from UIElement)
IsHitTestVisible | Gets or sets a value that declares whether this element can possibly be returned as a hit test result from some portion of its rendered content. This is a dependency property.(Inherited from UIElement)
IsTabStop | Gets or sets a value that indicates whether a control is included in tab navigation.(Inherited from Control)
ItemsSource | Gets or sets the collection used to generate the content of the listbox ItemsControl.
Language | Gets or sets localization/globalization language information that applies to an element.(Inherited from FrameworkElement)
LayoutTransform	| Gets or sets a graphics transformation that should apply to this element when layout is performed.(Inherited from FrameworkElement)
Margin | Gets or sets the outer margin of an element.(Inherited from FrameworkElement)
MaxDropDownHeight | Gets or sets the maximum height of the Sdl.MultiSelectComboBox dropdown menu
MaxHeight | Gets or sets the maximum height constraint of the element.(Inherited from FrameworkElement)
MaxWidth | Gets or sets the maximum width constraint of the element.(Inherited from FrameworkElement)
MinHeight | Gets or sets the minimum height constraint of the element.(Inherited from FrameworkElement)
MinWidth | Gets or sets the minimum width constraint of the element.(Inherited from FrameworkElement)
Name | Gets or sets the identifying name of the element. The name provides a reference so that code-behind, such as event handler code, can refer to a markup element after it is constructed during processing by a XAML processor.(Inherited from FrameworkElement)
Opacity | Gets or sets the opacity factor applied to the entire UIElement when it is rendered in the user interface (UI). This is a dependency property.(Inherited from UIElement)
OpacityMask | Gets or sets an opacity mask, as a Brush implementation that is applied to any alpha-channel masking for the rendered content of this element. This is a dependency property.(Inherited from UIElement)
OverridesDefaultStyle | Gets or sets a value that indicates whether this element incorporates style properties from theme styles.(Inherited from FrameworkElement)
Padding | Gets or sets the padding inside a control.(Inherited from Control)
RenderSize | Gets (or sets) the final render size of this element.(Inherited from UIElement)
RenderTransform | Gets or sets transform information that affects the rendering position of this element. This is a dependency property.(Inherited from UIElement)
RenderTransformOrigin | Gets or sets the center point of any possible render transform declared by RenderTransform, relative to the bounds of the element. This is a dependency property.(Inherited from UIElement)
Resources | Gets or sets the locally-defined resource dictionary.(Inherited from FrameworkElement)
SelectedItems | Gets or sets the selected items.
SelectedItemTemplate | Gets or sets the DataTemplate used to display each item in the Selected Items Panel
SelectionMode |	Gets or sets the SelectionMode value. The SelectionMode property determines how many items a user can select at one time. You can set the property to Multiple (the default) or Single
SnapsToDevicePixels	| Gets or sets a value that determines whether rendering for this element should use device-specific pixel settings during rendering. This is a dependency property.(Inherited from UIElement)
Style | Gets or sets the style used by this element when it is rendered.(Inherited from FrameworkElement)
TabIndex | Gets or sets a value that determines the order in which elements receive focus when the user navigates through controls by using the TAB key.(Inherited from Control)
Tag | Gets or sets an arbitrary object value that can be used to store custom information about this element.(Inherited from FrameworkElement)
Template | Gets or sets a control template.(Inherited from Control)
ToolTip | Gets or sets the tool-tip object that is displayed for this element in the user interface (UI).(Inherited from FrameworkElement)
Triggers | Gets the collection of triggers established directly on this element, or in child elements.(Inherited from FrameworkElement)
UseLayoutRounding | Gets or sets a value that indicates whether layout rounding should be applied to this element's size and position during layout.(Inherited from FrameworkElement)
VerticalAlignment | Gets or sets the vertical alignment characteristics applied to this element when it is composed within a parent element such as a panel or items control.(Inherited from FrameworkElement)
VerticalContentAlignment | Gets or sets the vertical alignment of the control's content.(Inherited from Control)
Visibility | Gets or sets the user interface (UI) visibility of this element. This is a dependency property.(Inherited from UIElement)
Width | Gets or sets the width of the element.(Inherited from FrameworkElement)

## Events
| Property | Description |
| ------ | ------ |
FilterTextChanged |	Occurs when the criteria of the filter text has changed. The FilterTextChangedArgs are passed with the event, identifying the filter criteria and list of item ids currently filtered.
SelectedItemsChanged | Occurs when items in the selected items collection has changed.  The SelectedItemsChangedEventArgs are passed with the event, identifying the added, removed and currently selected item ids.

### Event Behaviours
We have introduced the following event behaviors to accommodate binding events to a command. This way, the bound command is invoked like an event handler when the event is raised.
| Property | Description |
| ------ | ------ |
FilterTextChangedBehaviour.FilterTextChanged | Occurs when the criteria of the filter text has changed. The FilterTextChangedEventArgs are passed with the event, identifying the filter criteria and list of item ids currently filtered.
SelectedItemsChangedBehaviour.SelectedItemsChanged | Occurs when items in the selected items collection has changed. The SelectedItemsChangedEventArgs are passed with the event, identifyin