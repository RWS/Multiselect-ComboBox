namespace Sdl.MultiSelectComboBox.API
{
	/// <summary>
	/// Service used to determine auto-complete values for items in the collection.
	/// </summary>
	public interface IAutoCompleteService
	{
		/// <summary>
		/// Returns the auto-complete value to use for the specified item from the collection.
		/// </summary>
		string GetAutoCompleteString(object item);
	}
}
