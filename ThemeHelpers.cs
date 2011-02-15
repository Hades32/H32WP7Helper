using System.Windows;
using System.Windows.Media;

namespace H32WP7Helper
{
    public static class ThemeHelper
    {
        private static bool? _useDarkTheme = null;
        public static bool UseDarkTheme
        {
            get
            {
                if (_useDarkTheme == null || _useDarkTheme.HasValue == false)
                {
                    if (Application.Current == null)
                        _useDarkTheme = true;
                    else
                        _useDarkTheme = (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible;
                }
                return _useDarkTheme.Value;
            }
        }

        public static string ThemeImagePath
        {
            get
            {
                if (UseDarkTheme)
                    return "/Images/Dark/";
                else
                    return "/Images/Light/";
            }
        }

        public static void createAdditionalAccentStyles()
        {
            Color accentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
            Color darkAccentColor = Color.FromArgb(accentColor.A,
                (byte)(accentColor.R / 2),
                (byte)(accentColor.G / 2),
                (byte)(accentColor.B / 2));
            Color lightAccentColor = Color.FromArgb(accentColor.A,
                (byte)((255 + accentColor.R) / 2),
                (byte)((255 + accentColor.G) / 2),
                (byte)((255 + accentColor.B) / 2));
            SolidColorBrush lightAccentBrush = new SolidColorBrush(lightAccentColor);
            SolidColorBrush darkAccentBrush = new SolidColorBrush(darkAccentColor);

            Application.Current.Resources.AddOrSet("DarkAccentColor", darkAccentColor);
            Application.Current.Resources.AddOrSet("DarkAccentBrush", darkAccentBrush);
            Application.Current.Resources.AddOrSet("LightAccentColor", lightAccentColor);
            Application.Current.Resources.AddOrSet("LightAccentBrush", lightAccentBrush);
        }
    }
}
