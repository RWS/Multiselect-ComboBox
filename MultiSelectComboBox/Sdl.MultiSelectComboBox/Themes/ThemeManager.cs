using System;
using System.Windows;

namespace Sdl.MultiSelectComboBox.Themes
{
    public static class ThemeManager
    {
        private static readonly Uri DefaultThemeUri = new Uri(
            "pack://application:,,,/Sdl.MultiSelectComboBox;component/Themes/Default.xaml",
            UriKind.Absolute);

        private static readonly Uri DarkThemeUri = new Uri(
            "pack://application:,,,/Sdl.MultiSelectComboBox;component/Themes/Dark.xaml",
            UriKind.Absolute);

        private static readonly Uri HighContrastThemeUri = new Uri(
            "pack://application:,,,/Sdl.MultiSelectComboBox;component/Themes/HighContrast.xaml",
            UriKind.Absolute);

        private static readonly ResourceDictionary SharedThemeDictionary = new ResourceDictionary();
        private static MultiSelectComboBoxTheme _requestedTheme = MultiSelectComboBoxTheme.Light;
        private static bool _initialized;

        public static void Startup(ResourceDictionary controlResources)
        {
            if (!_initialized)
            {
                ApplyTheme();

                SystemParameters.StaticPropertyChanged += (_, e) =>
                {
                    if (e.PropertyName == nameof(SystemParameters.HighContrast))
                    {
                        ApplyTheme();
                    }
                };

                _initialized = true;
            }

            if (!controlResources.MergedDictionaries.Contains(SharedThemeDictionary))
            {
                controlResources.MergedDictionaries.Insert(0, SharedThemeDictionary);
            }
        }

        /// <summary>
        /// Called by the host application to switch between Light and Dark. Has no effect when
        /// Windows High Contrast is active, since that always takes precedence. If never called,
        /// the control stays on <see cref="MultiSelectComboBoxTheme.Light"/>.
        /// </summary>
        public static void SetTheme(MultiSelectComboBoxTheme theme)
        {
            _requestedTheme = theme;
            ApplyTheme();
        }

        private static void ApplyTheme()
        {
            var targetUri = GetTargetThemeUri();

            SharedThemeDictionary.MergedDictionaries.Clear();
            SharedThemeDictionary.MergedDictionaries.Add(new ResourceDictionary { Source = targetUri });
        }

        private static Uri GetTargetThemeUri()
        {
            if (SystemParameters.HighContrast)
            {
                return HighContrastThemeUri;
            }

            return _requestedTheme == MultiSelectComboBoxTheme.Dark ? DarkThemeUri : DefaultThemeUri;
        }
    }
}
