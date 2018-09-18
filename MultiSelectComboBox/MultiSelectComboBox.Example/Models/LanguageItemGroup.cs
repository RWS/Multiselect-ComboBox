using Sdl.MultiSelectComboBox.API;

namespace Sdl.MultiSelectComboBox.Example.Models
{
	public class LanguageItemGroup : IItemGroup
	{
		public int Order { get; set; }
		public string Name { get; }

		public LanguageItemGroup(int index, string name)
		{
			Order = index;
			Name = name;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
