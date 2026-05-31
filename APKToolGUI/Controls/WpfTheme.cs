using System.Windows;
using System.Windows.Media;

namespace APKToolGUI.Controls
{
    /// <summary>
    /// Shared dark/light palette for the WPF windows. Fills the <c>DynamicResource</c>
    /// brush keys consumed by <c>Themes/Controls.xaml</c> and the window XAML, so every
    /// converted window themes consistently. The dark palette mirrors the WinForms
    /// <see cref="DarkTheme"/> colors used by the not-yet-converted forms.
    /// </summary>
    public static class WpfTheme
    {
        /// <summary>
        /// Applies the dark or light palette to a window/element's resource dictionary.
        /// The merged <c>Themes/Controls.xaml</c> styles pick these up via DynamicResource.
        /// </summary>
        public static void Apply(FrameworkElement target, bool dark)
        {
            if (dark)
            {
                Set(target, "WindowBackground", 32, 32, 32);
                Set(target, "PrimaryText", 255, 255, 255);
                Set(target, "SecondaryText", 160, 160, 160);
                Set(target, "PanelBackground", 64, 64, 64);   // text input background
                Set(target, "ControlBackground", 51, 51, 51); // combo / checkbox box
                Set(target, "PanelBorder", 90, 90, 90);
                Set(target, "Accent", 0, 120, 215);
                Set(target, "LinkText", 30, 144, 255);        // DodgerBlue
                Set(target, "ButtonBackground", 51, 51, 51);
                Set(target, "ButtonBorder", 155, 155, 155);
                Set(target, "ButtonHover", 61, 61, 61);
                Set(target, "ButtonPressed", 42, 42, 42);
                Set(target, "ScrollTrack", 45, 45, 45);
                Set(target, "ScrollThumb", 85, 85, 85);
                Set(target, "TabBackground", 45, 45, 45);     // unselected tab
                Set(target, "MenuBar", 32, 32, 32);
                Set(target, "MenuPopup", 43, 43, 43);
                Set(target, "MenuHighlight", 61, 61, 61);
                Set(target, "LogBackground", 30, 30, 30);
            }
            else
            {
                Set(target, "WindowBackground", 240, 240, 240);
                Set(target, "PrimaryText", 0, 0, 0);
                Set(target, "SecondaryText", 105, 105, 105);  // DimGray
                Set(target, "PanelBackground", 255, 255, 255);
                Set(target, "ControlBackground", 255, 255, 255);
                Set(target, "PanelBorder", 171, 173, 179);
                Set(target, "Accent", 0, 120, 215);
                Set(target, "LinkText", 30, 144, 255);
                Set(target, "ButtonBackground", 225, 225, 225);
                Set(target, "ButtonBorder", 173, 173, 173);
                Set(target, "ButtonHover", 229, 241, 251);
                Set(target, "ButtonPressed", 204, 228, 247);
                Set(target, "ScrollTrack", 240, 240, 240);
                Set(target, "ScrollThumb", 180, 180, 180);
                Set(target, "TabBackground", 224, 224, 224);
                Set(target, "MenuBar", 240, 240, 240);
                Set(target, "MenuPopup", 250, 250, 250);
                Set(target, "MenuHighlight", 204, 228, 247);
                Set(target, "LogBackground", 255, 255, 255);
            }
        }

        private static void Set(FrameworkElement target, string key, byte r, byte g, byte b)
        {
            var brush = new SolidColorBrush(Color.FromRgb(r, g, b));
            brush.Freeze();
            target.Resources[key] = brush;
        }
    }
}
