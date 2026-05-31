using APKToolGUI;
using APKToolGUI.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace APKToolGUI.Utils
{
    /// <summary>
    /// simple logging wrapper class
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Active log sink. Set by the main window (<c>MainWindow.ToLog</c>) at startup.
        /// </summary>
        public static Action<ApktoolEventType, string> Output;

        private static void Send(ApktoolEventType type, string s)
        {
            Output?.Invoke(type, s);
        }

        /// <summary>log message with level VERBOSE (may be disabled)</summary>
        public static void v(string s)
        {
            if (!Settings.Default.DebugMode) return;
            Send(ApktoolEventType.None, s);
        }

        /// <summary>log message with level DEBUG (may be disabled)</summary>
        public static void d(string s)
        {
            if (!Settings.Default.DebugMode) return;
            Send(ApktoolEventType.Infomation, s);
        }

        /// <summary>log message with level INFO</summary>
        public static void i(string s) => Send(ApktoolEventType.Infomation, s);

        /// <summary>log message with level WARNING</summary>
        public static void w(string s) => Send(ApktoolEventType.Warning, s);

        /// <summary>log message with level ERROR</summary>
        public static void e(string s) => Send(ApktoolEventType.Error, s);
    }
}
