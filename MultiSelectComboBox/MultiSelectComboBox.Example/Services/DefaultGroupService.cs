using System.Collections.Generic;
using Sdl.MultiSelectComboBox.API;
using Sdl.MultiSelectComboBox.Example.API;
using Sdl.MultiSelectComboBox.Example.Models;

namespace Sdl.MultiSelectComboBox.Example.Services
{
	public class DefaultGroupService
	{
		private readonly IGroupIdentityService _service;

		private readonly List<IItemGroup> _itemGroups;

		public DefaultGroupService(IGroupIdentityService service)
		{
			_service = service;

			_itemGroups = new List<IItemGroup> { new LanguageItemGroup(-1, StringResources.ItemsGroupService_AllItems) };

			if (_service != null && !_itemGroups.Exists(a => a.Name == _service.Name))
			{
				if (_service.Index < _itemGroups.Count)
				{
					_itemGroups.Insert(_service.Index, new LanguageItemGroup(_service.Index, _service.Name));
				}
				else
				{
					_service.Index = _itemGroups.Count;
					_itemGroups.Add(new LanguageItemGroup(_service.Index, _service.Name));
				}

				for (var index = 0; index < _itemGroups.Count; index++)
				{
					_itemGroups[index].Order = index;
				}
			}
		}

		public IItemGroup GetItemGroup(string id)
		{
			var exists = _service?.Contains(id);

			return exists != null && exists.Value 
				? _itemGroups[_service.Index] 
				: _itemGroups[_service != null ? _service.Index == 0 ? 1 : 0 : 0];
		}
	}
}
