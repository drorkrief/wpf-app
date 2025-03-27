using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace wpf_app
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool IsManualExit = false;
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Prevent restart if the user manually exited
            if (!IsManualExit)
            {
                RestartApp();
            }
        }

        private void RestartApp()
        {
            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
        }
    }

}
