using Epsilon.Alpha.Configuration;
using Epsilon.Alpha.Controls;
using Epsilon.Alpha.Elsword;
using Epsilon.Alpha.Elsword.Buff;
using Epsilon.Alpha.Hooks;
using Epsilon.Alpha.Properties;

namespace Epsilon.Alpha
{
    internal static class Program
    {
        private static readonly string _configPath = Application.StartupPath + "config.json";

        private static EpsilonConfigurationController? _configController;
        private static KeyboardHook? _keyboardHook;
        private static ForegroundHook? _foregroundHook;
        private static ElswordBuffs? _buffs;
        private static ElswordProcessor? _processor;

        private static readonly string _creator = "UnovaEls";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        internal static void Main()
        {
            ApplicationConfiguration.Initialize();

            using (NotifyIcon notifyIcon = new NotifyIcon() { Icon = Resources.Epsilon })
            using (Overlay overlay = new Overlay())
            {
                _configController = new EpsilonConfigurationController(_configPath);
                _keyboardHook = new KeyboardHook();
                _foregroundHook = new ForegroundHook();
                _buffs = new ElswordBuffs();
                _processor = new ElswordProcessor(_configController, _keyboardHook, _foregroundHook, overlay, notifyIcon);

                Application.Run(overlay);
            }
        }
    }
}