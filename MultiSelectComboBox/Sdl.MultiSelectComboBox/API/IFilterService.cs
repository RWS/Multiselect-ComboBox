using System;

namespace Sdl.MultiSelectComboBox.API
{
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
}
