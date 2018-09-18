namespace Sdl.MultiSelectComboBox.API
{
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
}
