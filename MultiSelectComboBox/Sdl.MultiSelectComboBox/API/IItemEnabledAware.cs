namespace Sdl.MultiSelectComboBox.API
{
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
}
