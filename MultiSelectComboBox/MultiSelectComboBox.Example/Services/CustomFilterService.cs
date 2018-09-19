using System;
using System.Text.RegularExpressions;
using Sdl.MultiSelectComboBox.API;
using Sdl.MultiSelectComboBox.Example.Models;

namespace Sdl.MultiSelectComboBox.Example.Services
{
	public class CustomFilterService: IFilterService
	{		
		private readonly Regex _toMatchDash = new Regex(@"^(((([A-Z])|([a-z])){2})\-(([A-Z])|([a-z])){0,3})");
		private readonly Regex _toMatchSpace = new Regex(@"^(((([A-Z])|([a-z])){2})\s(([A-Z])|([a-z])){0,3})");

		private string _filterText;
		private string _auxiliaryText = string.Empty;

		public void SetFilter(string criteria)
		{
			_filterText = criteria;

			ConfigureFilter();
		}

		public Predicate<object> Filter { get; set; }

		private bool FilteringByName(object item)
		{
			return string.IsNullOrEmpty(_filterText) || ((LanguageItem)item).ToString().ToLower().Contains(_filterText.ToLower());
		}

		private bool FilteringById(object item)
		{
			return string.IsNullOrEmpty(_filterText) || ((LanguageItem)item).Id.ToLower().Contains(_filterText.ToLower());
		}

		private bool FilteringByComposedId(object item)
		{
			return string.IsNullOrEmpty(_auxiliaryText) || ((LanguageItem)item).Id.ToLower().Contains(_auxiliaryText.ToLower());
		}

		private void ConfigureFilter()
		{
			if (_filterText != null && _toMatchSpace.IsMatch(_filterText))
			{
				var inputString = _filterText;
				_auxiliaryText = inputString.Replace(' ', '-');

				Filter = FilteringByComposedId;
			}
			else if (_filterText != null && _toMatchDash.IsMatch(_filterText))
			{
				Filter = FilteringById;
			}
			else
			{
				Filter = FilteringByName;
			}
		}
	}
}
