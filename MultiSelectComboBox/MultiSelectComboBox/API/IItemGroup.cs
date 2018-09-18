namespace Sdl.MultiSelectComboBox.API
{
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
}
