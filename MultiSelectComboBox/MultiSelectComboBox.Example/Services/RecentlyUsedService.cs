using System;
using System.Collections.Generic;
using System.Linq;
using Sdl.MultiSelectComboBox.Example.API;

namespace Sdl.MultiSelectComboBox.Example.Services
{
	public class RecentlyUsedService : IGroupIdentityService
	{
		private readonly IEnumerable<string> _items;

		public RecentlyUsedService(IEnumerable<string> items)
		{
			Index = 0;
			Name = StringResources.ItemsGroupService_RecentlyUsedItems;

			_items = items;
		}

		public int Index { get; set; }

		public string Name { get; }

		public bool Contains(string item, StringComparison comparer = StringComparison.InvariantCultureIgnoreCase)
		{
			return _items.Any(a => string.Compare(a, item, comparer) == 0);
		}
	}
}
