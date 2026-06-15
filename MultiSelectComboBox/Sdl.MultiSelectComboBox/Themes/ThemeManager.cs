using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sdl.MultiSelectComboBox.Themes
{
    public static class ThemeManager
    {
        private static readonly Uri DefaultThemeUri = new Uri(
            "pack://application:,,,/Sdl.MultiSelectComboBox;component/Themes/Default.xaml",
            UriKind.Absolute);

        private static readonly Uri HighContrastThemeUri = new Uri(
            "pack://application:,,,/Sdl.MultiSelectComboBox;component/Themes/HighContrast.xaml",
            UriKind.Absolute);

        private static ResourceDictionary _controlResources;


        public static void Startup(ResourceDictionary controlResources)
        {
            _controlResources = controlResources;

            ApplyTheme();

            SystemParameters.StaticPropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(SystemParameters.HighContrast))
                {
                    ApplyTheme();
                }
            };
        }

        private static void ApplyTheme()
        {
            var targetUri = SystemParameters.HighContrast ? HighContrastThemeUri : DefaultThemeUri;
            var merged = _controlResources.MergedDictionaries;

            for (int i = merged.Count - 1; i >= 0; i--)
            {
                var src = merged[i].Source;
                if (src == DefaultThemeUri || src == HighContrastThemeUri)
                {
                    merged.RemoveAt(i);
                }
            }

            merged.Insert(0, new ResourceDictionary { Source = targetUri });
        }
    }
}
