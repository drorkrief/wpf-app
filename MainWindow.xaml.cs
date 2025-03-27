using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal; // Prevent minimizing
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += SecretCloseKey;
        }
        private void CalculateDaysPassed(object sender, RoutedEventArgs e)
        {
            if (BirthDatePicker.SelectedDate.HasValue)
            {
                DateTime birthDate = BirthDatePicker.SelectedDate.Value;
                int daysPassed = (DateTime.Today - birthDate).Days;
                ResultText.Text = $"Days passed since birth: {daysPassed} days";
            }
            else
            {
                ResultText.Text = "Please select a valid birthdate.";
            }
        }
        private void SecretCloseKey(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Q)
            {
                App.IsManualExit = true;
                Application.Current.Shutdown();
            }
        }
    }
}