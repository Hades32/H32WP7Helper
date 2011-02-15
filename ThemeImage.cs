using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace H32WP7Helper
{
    public class ThemeImage
    {
        /// <summary>
        /// Source Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached("Source", typeof(string), typeof(ThemeImage),
                new PropertyMetadata((string)null,
                    new PropertyChangedCallback(OnSourceChanged)));

        /// <summary>
        /// Gets the Source property. This dependency property 
        /// indicates the image.
        /// </summary>
        public static string GetSource(DependencyObject d)
        {
            return (string)d.GetValue(SourceProperty);
        }

        /// <summary>
        /// Sets the Source property. This dependency property 
        /// indicates the image.
        /// </summary>
        public static void SetSource(DependencyObject d, string value)
        {
            d.SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Handles changes to the Source property.
        /// </summary>
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)d.GetValue(SourceProperty);
            if (string.IsNullOrEmpty(newValue) == false)
            {
                var themeimg = new BitmapImage(new Uri(ThemeHelper.ThemeImagePath + newValue, UriKind.Relative));
                if (d is Image)
                {
                    var img = (Image)d;
                    img.Source = themeimg;
                }
                else if (d is ImageBrush)
                {
                    var imgbrush = (ImageBrush)d;
                    imgbrush.ImageSource = themeimg;
                }
            }
        }
    }
}
