using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using Sdl.MultiSelectComboBox.API;

namespace Sdl.MultiSelectComboBox.Example.Models
{
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
}
