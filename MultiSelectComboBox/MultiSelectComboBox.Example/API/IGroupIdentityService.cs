using System;

namespace Sdl.MultiSelectComboBox.Example.API
{
	public interface IGroupIdentityService
	{
		int Index { get; set; }

		string Name { get; }

		bool Contains(string item, StringComparison comparer = StringComparison.InvariantCultureIgnoreCase);
	}
}