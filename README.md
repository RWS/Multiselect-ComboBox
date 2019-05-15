# Sdl.MultiSelectComboBox (WPF Custom Control)

[![NuGet Stats](https://img.shields.io/nuget/v/Sdl.MultiSelectComboBox.svg)](https://www.nuget.org/packages/Sdl.MultiSelectComboBox/)

## Overview
The multi selection combo box is a WPF custom control with multiple [item selection](#item-selection) capabilities, along with customizable features to [group](#item-group), [sort](#item-sorting) and [filter](#filter-criteria) items in the collection.

![Usage Demonstration Gif](https://github.com/sdl/Multiselect-ComboBox/blob/master/Resources/SDL.MultiSelectComboBox.Usage.gif)

## Components and Features
![Components Image](https://github.com/sdl/Multiselect-ComboBox/blob/master/Resources/Sdl.MultiSelectComboBox.Components.png)


## Selected Items Panel
The [selected items](#selecteditems) are displayed like tags with a remove button  They can be added or removed from the [selected items](#selecteditems) collection, by selecting them from the items present in the [Dropdown Menu](#dropdown-menu) list.  Additionally, items can also be removed by interacting with them directly from the *Selected Items Panel*, as follows:

| Device | Action | Description |
| ------ | ------ | ------ |
| Keyboard | Backspace or Delete key | If the Filter Criteria is empty, then the last item in the selected items collection is removed. |
| Mouse | Remove Item Button | The item is removed from the selected items collection |


### Visual States
The control has two visual states, [Readonly](#readonlymode) and [EditMode](#editmode), which is identified by the [IsEditMode](#iseditmode) property.  When the control is in readonly mode, the items cannot be edited from the view.  To switch to edit mode, select the control in the view or hit (*F2*) when the control has focus.  Once the control is in edit mode, the items can be filtered, selected or removed.  To switch back to readonly, move focus away from the control by selecting any other control in the view or by deactivating the parent window.

| Visual state | Description | Example |
| ------ | ------ | ------ |
| <a name="readonlymode"></a>Readonly | The [Dropdown Menu](#dropdown-menu) is collapsed and items present in the [Selected Items Panel](#selected-items-panel) cannot be edited. | ![Readonly Image](https://github.com/sdl/Multiselect-ComboBox/blob/master/Resources/Sdl.MultiSelectComboBox.Readonly.png) |
| <a name="editmode"></a>EditMode | The [Dropdown Menu](#dropdown-menu) can be expanded or collapsed when the button is clicked and items present in the [Selected Items Panel](#selected-items-panel) can be edited. | ![EditMode Image](https://github.com/sdl/Multiselect-ComboBox/blob/master/Resources/Sdl.MultiSelectComboBox.EditMode.png) |


### Filter Criteria
When you type a character in the text area, the control applies a filter on the collection and suggests a list of items matching that criteria in the [Dropdown Menu](#dropdown-menu) list.  The developer can override the default [filter service](#filterservice), based on their own business logic requirements, that implements [IFilterService](#ifilterservice).

![Filter Criteria Image](https://github.com/sdl/Multiselect-ComboBox/blob/master/Resources/Sdl.MultiSelectComboBox.Filter.png)

Depending on whether or not the [ClearFilterOnDropdownClosing](#clearfilterondropdownclosing) property is set to true and [Dropdown Menu](#dropdown-menu) list as keyboard focus, the *Filter Criteria* is cleared automatically as the [Dropdown Menu](#dropdown-menu) is closing.

| Keyboard focus | Action |
| ------ | ------ |
| True | The *Filter Criteria* is cleared as the [Dropdown Menu](#dropdown-menu) is closing. |
| False | No attempt is made to change the current filter. |


## Dropdown Menu
Presents a list of suggestions that can be selected by the user.  If no [Filter Criteria](#filter-criteria) is applied, then all items in the collection are displayed


### Visibility
The [Dropdown Menu](#dropdown-menu) can be expanded only when the control is in EditMode.
<table>
  <tbody>
    <tr>
      <th>Visibility</th>
      <th>Actions</th>
    </tr>
    <tr>
      <td>Expand</td>
      <td>
        <ul>
          <li>Left mouse click anywhere within the control area, with exception to the <strong>Remove Item Button</strong> of the item.</li>
          <li>Down arrow key on the keyboard when the control has focus.</li>
          <li>Change the *Filter Criteria*, by typing characters in the text area.</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td>Collapse</td>
      <td>
        <ul>
          <li>Left mouse click anywhere within the control area, with exception to the <strong>Remove Item Button</strong> of the item.</li>
          <li>Return key.
            <ul>
              <li>The item that has focus in the list is selected</li>
              <li>The *Filter Criteria* is removed</li>
            </ul>
          </li>
          <li>Esc key</li>
          <li>Move focus away from the control by selecting any other control in the view or by deactivating the parent window.</li>
        </ul>
      </td>
    </tr>
  </tbody>
</table>


### Item Group
The items in the collection can be grouped by implementing [IItemGroupAware](#iitemgroupaware).  In addition to the header name, this interface exposes a property to manage the order in which the group headers are displayed. 
![Item Group Image](https://github.com/sdl/Multiselect-ComboBox/blob/master/Resources/Sdl.MultiSelectComboBox.ItemGroup.png)


### Item Sorting
The sort order is based on the order of the items in the collection that was received when the [ItemsSource](#itemssource) is set.


### Item Selection
Items can be added or removed from the [selected items](#selecteditems) collection, by selecting them from the items present in the [Dropdown Menu](#dropdown-menu) list.

| Device | Actions | Description |
| ------ | ------ | ------ |
| Mouse | Left mouse click | The item that has selection focus in the list is selected/unselected. |
| Mouse | Shift + Left mouse click | All items between the previous and current item that has focus are selected. |
| Keyboard | Return key |- The item that has focus in the list is selected.<br/>- The [Filter Criteria](#filter-criteria) is removed.<br/>- The [Dropdown Menu](#dropdown-menu) is closed |
| Keyboard | Space key | The item that has focus in the list is selected/unselected. |
| Keyboard | Shift + Up or Down key | The item that has focus in the list is selected/unselected. |


### Disabled Item
Implement [IItemEnabledAware](#iitemenabledaware) to identify whether or not the items are enabled.  When an item is not enabled, then it will not be selectable from the [Dropdown Menu](#dropdown-menu) list and removed from the [Selected Items Panel](#selected-items-panel) automatically.


### Selected Item
Items in the [Dropdown Menu](#dropdown-menu) list can be selected/unselected via the Mouse or Keyboard.  When the item is selected, the style is updated to reflect a selected state.


## Example Project
The example project demonstrates how to implement the *Sdl.MultiSelectComboBox* custom control to your project, and provides good examples in understanding the controls behaviours.
![Example Project Image](https://github.com/sdl/Multiselect-ComboBox/blob/master/Resources/Sdl.MultiSelectComboBox.ExampleProject.png)


## Examples
The following example creates an *Sdl.MultiSelectComboBox*.  The example populates the control by binding the [ItemsSource](#itemssource) property to a collection of type object, that implements [IItemEnabledAware](#iitemenabledaware) and [IItemGroupAware](#iitemgroupaware).  The example also binds the [SelectedItemsChangedBehaviour](#selecteditemschangedbehaviour) to a command to receive a notification of [SelectedItemsChangedEventArgs](#selecteditemschangedeventargs) whenever the [selected items](#selecteditems) collection changes and then display those results in a TextBlock on the view.
Additionally, both the selected and dropdown list item templates are customized to display an image along with the item name.

**Example.xaml**
```c#
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
```c#
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
```c#
<DataTemplate x:Key="MultiSelectComboBox.SelectedItems.ItemTemplate" DataType="models:LanguageItem">
    <StackPanel Orientation="Horizontal" Margin="0,-4">
        <Image Style="{StaticResource MultiSelectComboBox.Image.Style}" Margin="2,0,4,-1"/>
        <TextBlock Text="{Binding Path=Name}" Style="{DynamicResource MultiSelectComboBox.DefaultTextBlock.Style}" Margin="2,0" />
    </StackPanel>
</DataTemplate>
```
**Style: MultiSelectComboBox.Image.Style**
```c#
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
```c#
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

The following example defines the data context (i.e. *models:LanguageItems*) that the *Sdl.MultiSelectComboBox* binds to from the previous example.

**LanguageItems : INotifyPropertyChanged**
```c#
public class LanguageItems : INotifyPropertyChanged
{
    private List<LanguageItem> _items;   
 
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
**LanguageItem : [IItemEnabledAware](#iitemenabledaware), [IItemGroupAware](#iitemgroupaware), INotifyPropertyChanged**
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
**LanguageItemGroup : [IItemGroup](#iitemgroup)**
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
The *Sdl.MultiSelectComboBox* allows the user to select an item from a [Dropdown Menu](#dropdown-menu) list and optionally apply a filter on the list by typing text in the text box of the [Selected Items Panel](#selected-items-panel). The [SelectionMode](#selectionmode) and [IsEditable](#iseditable) properties specify how the [Item Selection](#item-selection) behaves, especially when the user is interacting with items from the [Selected Items Panel](#selected-items-panel), as follows:

|  | SelectionMode.**Multiple** _(default)_ | SelectionMode.**Single** |
| ------ | ------ | ------ |
| [IsEditable](#iseditable) is **true** _(default)_ | Multiple items can be selected/unselected from the [Dropdown Menu](#dropdown-menu) list<br/><br/>The **Remove Item Button** is displayed in each of the selected items in the [Selected Items Panel](#selected-items-panel).<br/><br/>Items in the [Selected Items Panel](#selected-items-panel) can be removed when the user hits the Delete or Back key | A single item can be selected from the [Dropdown Menu](#dropdown-menu) list.  When a new item is selected, it substitutes the previously selected item in the [Selected Items Panel](#selected-items-panel).<br/><br/>The **Remove Item Button** is displayed in each of the selected items in the [Selected Items Panel](#selected-items-panel).<br/><br/>Items in the [Selected Items Panel](#selected-items-panel) can be removed when the user hits the Delete or Back key |
| [IsEditable](#iseditable) is **false** | Multiple items can be selected/unselected from the [Dropdown Menu](#dropdown-menu) list.<br/><br/>The **Remove Item Button** is **not** displayed in each of the selected items in the [Selected Items Panel](#selected-items-panel).<br/><br/>Items in the [Selected Items Panel](#selected-items-panel) **cannot** be removed when the user hits the Delete or Back key | A single item can be selected from the [Dropdown Menu](#dropdown-menu) list.  When a new item is selected, it substitutes the previously selected item in the [Selected Items Panel](#selected-items-panel).<br/><br/>The **Remove Item Button** is **not** displayed in each of the selected items in the [Selected Items Panel](#selected-items-panel).<br/><br/>Items in the [Selected Items Panel](#selected-items-panel) **cannot** be removed when the user hits the Delete or Back key |


### Customizing the Style and template
You can modify the default [Style](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.style) and [ControlTemplate](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.controltemplate) to give the control a unique appearance. For information about modifying a controls style and template, see [Customizing the Appearance of an Existing Control by Creating a ControlTemplate](https://docs.microsoft.com/en-us/dotnet/framework/wpf/controls/customizing-the-appearance-of-an-existing-control?view=netframework-4.7.2) and [Styling controls](https://msdn.microsoft.com/windows/uwp/controls-and-patterns/styling-controls). The default style, template, and resources that define the look of the control are included in the generic.xaml file.

This table shows the resources used by the *Sdl.MultiSelectComboBox* control.

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
MultiSelectComboBox.SelectedItem.Border|	Selected item border brush that surrounds the selected item in the [Selected Items Panel](#selected-items-panel)
MultiSelectComboBox.SelectedItem.Button.Foreground |	Selected Item button foreground color
MultiSelectComboBox.SelectedItem.Button.Hover.Background |	Selected Item button foreground when the mouse if hovering over it
MultiSelectComboBox.SelectedItem.Button.Light.Foreground |	Selected Item button light foreground color
MultiSelectComboBox.SelectedItemsPanel.Border |	[Selected Items Panel](#selected-items-panel) border brush
MultiSelectComboBox.Text.Disabled.Foreground |	Default text foreground color when disabled
MultiSelectComboBox.Text.FontFamily |	Default FontFamily for all text in the control
MultiSelectComboBox.Text.Foreground |	Default text foreground color

The sample project includes an example of a customized version of control template style for the *Sdl.MultiSelectComboBox* control, where the [item selection](#item-selection) styles are modified and a popup control is displayed with the CultureInfo properties when hovering over the language items in the [Selected Items Panel](#selected-items-panel).  Make reference to the following.
![Custom Control Template image](https://github.com/sdl/Multiselect-ComboBox/blob/master/Resources/Sdl.MultiSelectComboBox.CustomControlTemplate.png)


## API
<a name="ifilterservice"></a>**interface IFilterService**
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
<a name="iitemenabledaware"></a>**interface IItemEnabledAware**
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
<a name="iitemgroup"></a>**interface IItemGroup**
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
<a name="iitemgroupaware"></a>**interface IItemGroupAware**
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
<a name="filtertextchangedeventargs"></a>**FilterTextChangedEventArgs : RoutedEventArgs**
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
<a name="selecteditemschangedeventargs"></a>**SelectedItemsChangedEventArgs : RoutedEventArgs**
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
AutoCompleteBackgroundProperty | Identified the [AutoCompleteBackground](#autocompletebackground) dependency property.
AutoCompleteForegroundProperty | Identified the [AutoCompleteForeground](#autocompleteforeground) dependency property.
AutoCompleteMaxLengthProperty | Identified the [AutoCompleteMaxLength](#autocompletemaxlength) dependency property.
ClearFilterOnDropdownClosingProperty | Identifies the [ClearFilterOnDropdownClosing](#clearfilterondropdownclosing) dependency property.
DropdownItemTemplateProperty | Identifies the [DropdownItemTemplate](#dropdownitemtemplate) dependency property.
EnableAutoCompleteProperty | Identified the [EnableAutoComplete](#enableautocomplete) dependency property.
EnableFilteringProperty | Identifies the [EnableFiltering](#enablefiltering) dependency property.
EnableGroupingProperty | Identifies the [EnableGrouping](#enablegrouping) dependency property.
FilterServiceProperty |	Identifies the [FilterService](#filterservice) dependency property.
IsDropDownOpenProperty | Identifies the [IsDropDownOpen](#isdropdownopen) dependency property.
IsEditableProperty | Identifies the [IsEditable](#iseditable) dependency property.
IsEditModeProperty | Identifies the [IsEditMode](#iseditmode) dependency property.
ItemsSourceProperty | Identifies the [ItemsSource](#itemssource) dependency property.
MaxDropDownHeightProperty | Identifies the [MaxDropDownHeight](#maxdropdownheight) dependency property.
SelectedItemsProperty | Identifies the [SelectedItems](#selecteditems) dependency property.
SelectedItemTemplateProperty | Identifies the [SelectedItemTemplate](#selecteditemtemplate) dependency property.
SelectionModeProperty | Identified the [SelectionMode](#selectionmode) dependency property.


## Properties
| Property | Description |
| ------ | ------ |
AllowDrop |	Gets or sets a value indicating whether this element can be used as the target of a drag-and-drop operation. This is a dependency property.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
<a name="autocompletebackground"></a>**AutoCompleteBackground** | Gets or sets background brush used when displaying the autocomplete content for the Filter Criteria
<a name="autocompleteforeground"></a>**AutoCompleteForeground** | Gets or sets foreground brush used when displaying the autocomplete content for the Filter Criteria
<a name="autocompletemaxlength"></a>**AutoCompleteMaxLength** | Gets or sets the maximum length of autocomplete content displayed for the Filter Criteria
Background	| Gets or sets a brush that describes the background of a control.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
BindingGroup | Gets or sets the [BindingGroup](https://docs.microsoft.com/en-us/dotnet/api/system.windows.data.bindinggroup?view=netframework-4.7.2) that is used for the element.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
BorderBrush	| Gets or sets a brush that describes the border background of a control.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
BorderThickness | Gets or sets the border thickness of a control.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
<a name="clearfilterondropdownclosing"></a>**ClearFilterOnDropdownClosing** | Gets or sets whether the Filter Criteria is cleared as the [Dropdown Menu](#dropdown-menu) is closing while it has keyboard focus. (default) true
Clip | Gets or sets the geometry used to define the outline of the contents of an element. This is a dependency property.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
ClipToBounds | Gets or sets a value indicating whether to clip the content of this element (or content coming from the child elements of this element) to fit into the size of the containing element. This is a dependency property.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
CommandBindings | Gets a collection of [CommandBinding](https://docs.microsoft.com/en-us/dotnet/api/system.windows.input.commandbinding?view=netframework-4.7.2) objects associated with this element. A CommandBinding enables command handling for this element, and declares the linkage between a command, its events, and the handlers attached by this element.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
ContextMenu | Gets or sets the context menu element that should appear whenever the context menu is requested through user interface (UI) from within this element.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
Cursor | Gets or sets the cursor that displays when the mouse pointer is over this element.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
DataContext | Gets or sets the data context for an element when it participates in data binding.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
<a name="dropdownitemtemplate"></a>**DropdownItemTemplate** | Gets or sets the [DataTemplate](https://docs.microsoft.com/en-us/dotnet/api/system.windows.datatemplate?view=netframework-4.7.2) used to display each item in the [Dropdown Menu](#dropdown-menu) list
<a name="enableautocomplete"></a>**EnableAutoComplete** | Gets or sets whether autocomplete feature is enabled for the Filter Criteria
<a name="enablefiltering"></a>**EnableFiltering** | Gets or sets whether filtering is enabled
<a name="enablegrouping"></a>**EnableGrouping** | Gets or sets whether grouping is enabled
Effect | Gets or sets the bitmap effect to apply to the [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2). This is a dependency property.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
<a name="filterservice"></a>**FilterService** |	Gets or sets a custom *filter service* that is used when filtering items in the collection.  If the *filter service* is null, then a default service is used that applies a filter on the override string ToString()
FlowDirection | Gets or sets the direction that text and other user interface (UI) elements flow within any parent element that controls their layout.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
Focusable | Gets or sets a value that indicates whether the element can receive focus. This is a dependency property.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
FocusVisualStyle | Gets or sets a property that enables customization of appearance, effects, or other style characteristics that will apply to this element when it captures keyboard focus.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
FontFamily | Gets or sets the font family of the control.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
FontSize | Gets or sets the font size.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
FontStretch | Gets or sets the degree to which a font is condensed or expanded on the screen.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
FontWeight | Gets or sets the weight or thickness of the specified font.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
ForceCursor | Gets or sets a value that indicates whether this [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2) should force the user interface (UI) to render the cursor as declared by the [Cursor](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement.cursor?view=netframework-4.7.2#System_Windows_FrameworkElement_Cursor) property.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
Foreground | Gets or sets a brush that describes the foreground color.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
Height | Gets or sets the suggested height of the element.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
HorizontalAlignment | Gets or sets the horizontal alignment characteristics applied to this element when it is composed within a parent element, such as a panel or items control.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
HorizontalContentAlignment | Gets or sets the horizontal alignment of the control's content.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
InputBindings | Gets the collection of input bindings associated with this element.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
InputScope | Gets or sets the context for input used by this FrameworkElement.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
<a name="isdropdownopen"></a>**IsDropDownOpen** | Gets or sets a value that indicates whether the [Dropdown Menu](#dropdown-menu) is currently open.
<a name="iseditable"></a>**IsEditable** | Gets or sets a value that indicates whether the user can edit items in the [Selected Items Panel](#selected-items-panel). The default value is true.
<a name="iseditmode"></a>**IsEditMode** | Gets the value identifying the Visual State of the control (i.e. Readonly or EditMode)
IsEnabled | Gets or sets a value indicating whether this element is enabled in the user interface (UI). This is a dependency property.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
IsFocused | Gets a value that determines whether this element has logical focus. This is a dependency property.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
IsHitTestVisible | Gets or sets a value that declares whether this element can possibly be returned as a hit test result from some portion of its rendered content. This is a dependency property.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
IsTabStop | Gets or sets a value that indicates whether a control is included in tab navigation.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
<a name="itemssource"></a>**ItemsSource** | Gets or sets the collection used to generate the content of the listbox [ItemsControl](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.itemscontrol?view=netframework-4.7.2).
Language | Gets or sets localization/globalization language information that applies to an element.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
LayoutTransform	| Gets or sets a graphics transformation that should apply to this element when layout is performed.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
Margin | Gets or sets the outer margin of an element.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
<a name="maxdropdownheight"></a>**MaxDropDownHeight** | Gets or sets the maximum height of the Sdl.MultiSelectComboBox dropdown menu
MaxHeight | Gets or sets the maximum height constraint of the element.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
MaxWidth | Gets or sets the maximum width constraint of the element.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
MinHeight | Gets or sets the minimum height constraint of the element.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
MinWidth | Gets or sets the minimum width constraint of the element.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
Name | Gets or sets the identifying name of the element. The name provides a reference so that code-behind, such as event handler code, can refer to a markup element after it is constructed during processing by a XAML processor.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
Opacity | Gets or sets the opacity factor applied to the entire [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2) when it is rendered in the user interface (UI). This is a dependency property.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
OpacityMask | Gets or sets an opacity mask, as a [Brush](https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.brush?view=netframework-4.7.2) implementation that is applied to any alpha-channel masking for the rendered content of this element. This is a dependency property.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
OverridesDefaultStyle | Gets or sets a value that indicates whether this element incorporates style properties from theme styles.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
Padding | Gets or sets the padding inside a control.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
RenderSize | Gets (or sets) the final render size of this element.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
RenderTransform | Gets or sets transform information that affects the rendering position of this element. This is a dependency property.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
RenderTransformOrigin | Gets or sets the center point of any possible render transform declared by [RenderTransform](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement.rendertransform?view=netframework-4.7.2#System_Windows_UIElement_RenderTransform), relative to the bounds of the element. This is a dependency property.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
Resources | Gets or sets the locally-defined resource dictionary.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
<a name="selecteditems"></a>**SelectedItems** | Gets or sets the selected items.
<a name="selecteditemtemplate"></a>**SelectedItemTemplate** | Gets or sets the [DataTemplate](https://docs.microsoft.com/en-us/dotnet/api/system.windows.datatemplate?view=netframework-4.7.2) used to display each item in the [Selected Items Panel](#selected-items-panel)
<a name="selectionmode"></a>**SelectionMode** |	Gets or sets the SelectionMode value. The SelectionMode property determines how many items a user can select at one time. You can set the property to Multiple (the default) or Single
SnapsToDevicePixels	| Gets or sets a value that determines whether rendering for this element should use device-specific pixel settings during rendering. This is a dependency property.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
Style | Gets or sets the style used by this element when it is rendered.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
TabIndex | Gets or sets a value that determines the order in which elements receive focus when the user navigates through controls by using the TAB key.(Inherited from Control)
Tag | Gets or sets an arbitrary object value that can be used to store custom information about this element.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
Template | Gets or sets a control template.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
ToolTip | Gets or sets the tool-tip object that is displayed for this element in the user interface (UI).<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
Triggers | Gets the collection of triggers established directly on this element, or in child elements.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
UseLayoutRounding | Gets or sets a value that indicates whether layout rounding should be applied to this element's size and position during layout.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
VerticalAlignment | Gets or sets the vertical alignment characteristics applied to this element when it is composed within a parent element such as a panel or items control.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))
VerticalContentAlignment | Gets or sets the vertical alignment of the control's content.<br/>(Inherited from [Control](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control?view=netframework-4.7.2))
Visibility | Gets or sets the user interface (UI) visibility of this element. This is a dependency property.<br/>(Inherited from [UIElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement?view=netframework-4.7.2))
Width | Gets or sets the width of the element.<br/>(Inherited from [FrameworkElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement?view=netframework-4.7.2))


## Events
| Property | Description |
| ------ | ------ |
<a name="filtertextchanged"></a>**FilterTextChanged** |	Occurs when the criteria of the filter text has changed. The [FilterTextChangedEventArgs](#filtertextchangedeventargs) are passed with the event, identifying the filter criteria and list of items currently filtered.
<a name="selecteditemschanged"></a>**SelectedItemsChanged** | Occurs when items in the selected items collection has changed.  The [SelectedItemsChangedEventArgs](#selecteditemschangedeventargs) are passed with the event, identifying the added, removed and currently selected items.

### Event Behaviours
We have introduced the following event behaviors to accommodate binding events to a command. This way, the bound command is invoked like an event handler when the event is raised.

| Property | Description |
| ------ | ------ |
<a name="filtertextchangedbehaviour"></a>**FilterTextChangedBehaviour**.FilterTextChanged | Occurs when the criteria of the filter text has changed. The [FilterTextChangedEventArgs](#filtertextchangedeventargs) are passed with the event, identifying the filter criteria and list of items currently filtered.
<a name="selecteditemschangedbehaviour"></a>**SelectedItemsChangedBehaviour**.SelectedItemsChanged | Occurs when items in the selected items collection has changed. The [SelectedItemsChangedEventArgs](#selecteditemschangedeventargs) are passed with the event, identifying the added, removed and currently selected items.
