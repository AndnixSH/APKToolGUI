using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace APKToolGUI.Controls
{
    /// <summary>
    /// Application theme selection.
    /// <para>
    /// The integer values intentionally match the stored <c>Settings.Default.Theme</c>
    /// values and the order of the theme combo box (0 = Auto, 1 = Light, 2 = Dark),
    /// so existing config files keep working. This is a drop-in replacement for the
    /// former <c>Dark.Net.Theme</c> enum.
    /// </para>
    /// </summary>
    public enum Theme
    {
        Auto = 0,
        Light = 1,
        Dark = 2
    }

    /// <summary>
    /// Self-contained dark mode support for Windows 10 (1809 / build 17763 and newer)
    /// and Windows 11, written from scratch to replace the external <c>DarkNet</c> package.
    ///
    /// <list type="bullet">
    /// <item>Immersive dark title bars via <c>DwmSetWindowAttribute</c>.</item>
    /// <item>Dark Win32 popup/menu rendering via the undocumented uxtheme app-mode ordinals.</item>
    /// <item>Dark scroll bars and combo box drop-downs via <c>SetWindowTheme</c>.</item>
    /// </list>
    ///
    /// All methods are safe no-ops on operating systems that do not support these APIs,
    /// so callers do not need to guard them with an OS-version check.
    /// </summary>
    public static class NativeDarkMode
    {
        #region Native interop

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int pvAttribute, int cbAttribute);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        // uxtheme.dll ordinal #135:
        //   Windows 10 1809  -> AllowDarkModeForApp(bool)
        //   Windows 10 1903+ -> SetPreferredAppMode(PreferredAppMode)
        [DllImport("uxtheme.dll", EntryPoint = "#135")]
        private static extern int SetPreferredAppMode(int preferredAppMode);

        [DllImport("uxtheme.dll", EntryPoint = "#135")]
        private static extern bool AllowDarkModeForApp(bool allow);

        // uxtheme.dll ordinal #136: FlushMenuThemes()
        [DllImport("uxtheme.dll", EntryPoint = "#136")]
        private static extern void FlushMenuThemes();

        // RtlGetVersion always returns the true OS version, unlike Environment.OSVersion
        // / GetVersionEx which "lie" (report Windows 8 / 6.2) when the app has no manifest
        // declaring Windows 10 compatibility - which is exactly this project's case.
        [DllImport("ntdll.dll")]
        private static extern int RtlGetVersion(ref RTL_OSVERSIONINFOEX versionInfo);

        [StructLayout(LayoutKind.Sequential)]
        private struct RTL_OSVERSIONINFOEX
        {
            internal uint dwOSVersionInfoSize;
            internal uint dwMajorVersion;
            internal uint dwMinorVersion;
            internal uint dwBuildNumber;
            internal uint dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string szCSDVersion;
        }

        // DwmSetWindowAttribute "use immersive dark mode" attribute id.
        // It moved from 19 to 20 in Windows 10 build 19041 (20H1); we try the new
        // one first and fall back to the old one for 1809 / 1903 / 1909.
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        private enum PreferredAppMode
        {
            Default = 0,
            AllowDark = 1,
            ForceDark = 2,
            ForceLight = 3
        }

        private const int BUILD_1809 = 17763; // earliest build exposing the dark mode ordinals
        private const int BUILD_1903 = 18362; // ordinal #135 becomes SetPreferredAppMode(int)
        private const int BUILD_20H1 = 19041; // DWM immersive dark mode attribute becomes 20

        #endregion

        /// <summary>
        /// The true OS version, queried once via <c>RtlGetVersion</c> so it is correct
        /// even though this app has no Windows 10 compatibility manifest embedded.
        /// </summary>
        private static readonly Version _osVersion = GetRealOSVersion();

        private static Version GetRealOSVersion()
        {
            try
            {
                var info = new RTL_OSVERSIONINFOEX
                {
                    dwOSVersionInfoSize = (uint)Marshal.SizeOf(typeof(RTL_OSVERSIONINFOEX))
                };
                if (RtlGetVersion(ref info) == 0) // STATUS_SUCCESS
                    return new Version((int)info.dwMajorVersion, (int)info.dwMinorVersion, (int)info.dwBuildNumber);
            }
            catch
            {
                // ntdll unavailable - fall back to the (possibly lying) framework value.
            }
            return Environment.OSVersion.Version;
        }

        private static int WindowsBuild => _osVersion.Build;

        /// <summary>
        /// True when the running OS is Windows 10 1809 or newer, where the dark mode
        /// APIs used here exist.
        /// </summary>
        public static bool IsSupported =>
            _osVersion.Major >= 10 &&
            WindowsBuild >= BUILD_1809;

        /// <summary>
        /// Sets the process-wide app mode so that Win32 popup menus, context menus and
        /// common controls render dark. Call this once, before any window is created.
        /// Mirrors <c>DarkNet.SetCurrentProcessTheme</c>.
        /// </summary>
        public static void SetProcessTheme(Theme theme)
        {
            if (!IsSupported)
                return;

            try
            {
                bool dark = EffectiveIsDark(theme);
                if (WindowsBuild >= BUILD_1903)
                    SetPreferredAppMode((int)(dark ? PreferredAppMode.ForceDark : PreferredAppMode.ForceLight));
                else
                    AllowDarkModeForApp(dark);
                FlushMenuThemes();
            }
            catch
            {
                // These ordinals are undocumented; ignore failures on unexpected builds.
            }
        }

        /// <summary>
        /// Applies (or removes) the immersive dark title bar on a form. Safe to call
        /// before the window handle exists - it is applied as soon as the handle is
        /// created and re-applied once the window is shown. Mirrors
        /// <c>DarkNet.SetWindowThemeForms</c>.
        /// </summary>
        public static void ApplyTheme(Form form, Theme theme)
        {
            if (form == null || !IsSupported)
                return;

            bool dark = EffectiveIsDark(theme);

            if (form.IsHandleCreated)
                UseImmersiveDarkTitleBar(form.Handle, dark);
            else
                form.HandleCreated += (s, e) => UseImmersiveDarkTitleBar(form.Handle, dark);

            // Some builds only honour the attribute once the window is actually visible.
            form.Shown += (s, e) => UseImmersiveDarkTitleBar(form.Handle, dark);
        }

        /// <summary>
        /// Applies (or removes) the immersive dark title bar on a WPF window. Safe to
        /// call before the native handle exists - it is applied on
        /// <c>SourceInitialized</c>, which fires before the window is shown.
        /// </summary>
        public static void ApplyTheme(System.Windows.Window window, Theme theme)
        {
            if (window == null || !IsSupported)
                return;

            bool dark = EffectiveIsDark(theme);
            var helper = new System.Windows.Interop.WindowInteropHelper(window);

            if (helper.Handle != IntPtr.Zero)
                UseImmersiveDarkTitleBar(helper.Handle, dark);
            else
                window.SourceInitialized += (s, e) =>
                    UseImmersiveDarkTitleBar(new System.Windows.Interop.WindowInteropHelper(window).Handle, dark);
        }

        /// <summary>
        /// Toggles the immersive dark mode title bar for an arbitrary window handle.
        /// </summary>
        public static void UseImmersiveDarkTitleBar(IntPtr handle, bool enabled)
        {
            if (handle == IntPtr.Zero || !IsSupported)
                return;

            int useImmersiveDarkMode = enabled ? 1 : 0;
            int attribute = WindowsBuild >= BUILD_20H1
                ? DWMWA_USE_IMMERSIVE_DARK_MODE
                : DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;

            if (DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) != 0)
            {
                // Fall back to the other attribute id if the first one was not accepted.
                int fallback = attribute == DWMWA_USE_IMMERSIVE_DARK_MODE
                    ? DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1
                    : DWMWA_USE_IMMERSIVE_DARK_MODE;
                DwmSetWindowAttribute(handle, fallback, ref useImmersiveDarkMode, sizeof(int));
            }
        }

        /// <summary>
        /// Gives a control dark-themed scroll bars / drop-downs. Use
        /// <c>"DarkMode_Explorer"</c> for scroll bars and lists, <c>"DarkMode_CFD"</c>
        /// for combo boxes. Safe to call before the handle exists.
        /// </summary>
        public static void UseDarkControlTheme(Control control, string subAppName = "DarkMode_Explorer")
        {
            if (control == null || !IsSupported)
                return;

            if (control.IsHandleCreated)
                SetWindowTheme(control.Handle, subAppName, null);
            else
                control.HandleCreated += (s, e) => SetWindowTheme(control.Handle, subAppName, null);
        }

        /// <summary>
        /// Resolves a <see cref="Theme"/> to an effective dark/light decision,
        /// reading the Windows personalization setting for <see cref="Theme.Auto"/>.
        /// Mirrors <c>DarkNet.EffectiveCurrentProcessThemeIsDark</c>.
        /// </summary>
        public static bool EffectiveIsDark(Theme theme)
        {
            switch (theme)
            {
                case Theme.Dark:
                    return true;
                case Theme.Light:
                    return false;
                default:
                    return IsSystemUsingDarkMode();
            }
        }

        /// <summary>
        /// Reads whether the current user has selected the dark app theme in Windows.
        /// </summary>
        public static bool IsSystemUsingDarkMode()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    // AppsUseLightTheme: 0 = dark, 1 = light (absent means light).
                    if (key?.GetValue("AppsUseLightTheme") is int appsUseLightTheme)
                        return appsUseLightTheme == 0;
                }
            }
            catch
            {
                // Registry unavailable - assume light.
            }
            return false;
        }
    }
}
