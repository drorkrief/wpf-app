using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using wpf_app.Models;
namespace wpf_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int VK_LWIN = 0x5B; // Left Windows Key
        private const int VK_RWIN = 0x5C; // Right Windows Key

        private static IntPtr _hookID = IntPtr.Zero;

        //prevent minimizing the window
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal; // Prevent minimizing
            }
        }

        //focus on the input box when the window loads
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CardInputBox.Focus(); // Auto-focus on load
        }

        //handle the keydown event for the input box
        private void CardInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string cardData = CardInputBox.Text.Trim(); // Remove any extra spaces

                if (cardData.Length == 10 && long.TryParse(cardData, out _)) // Ensure exactly 10 digits
                {
                    ProcessCard(cardData);
                    CardInputBox.Clear(); // Clear input for the next scan
                }
                else
                {
                    MessageBox.Show("Invalid card data. Please try again.");
                    CardInputBox.Clear();
                }
            }
        }
        private void ToggleCardPanel()
        {
            CardPanel.Visibility = CardPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
        private void ProcessCard(string cardData)
        {
            //MessageBox.Show("Card Read: " + cardData);
            ToggleCardPanel();
            HandleLoaderVisibility(true);
            FetchUserAsync(1);
        }
        private async Task FetchUserAsync(int userId)
        {
            using (HttpClient client = new HttpClient())
            {
                MessageBox.Show("This is a quick debug check 1!" + client);
                try
                {
                    string url = "https://login-logout-api.onrender.com/posts";
                    //string url = "http://localhost:3033/posts";
                    client.DefaultRequestHeaders.Add("x-api-key", "sk-2e7a0b1c");
                    MessageBox.Show("This is a quick debug check 2!" + client);
                    HttpResponseMessage response = await client.GetAsync(url);
                    MessageBox.Show("This is a quick debug check 3!" + response);
                    Console.WriteLine("Response: " + response.StatusCode);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Success");
                        string json = await response.Content.ReadAsStringAsync();
                        var users = JsonConvert.DeserializeObject<List<User>>(json);
                        var user = users?.FirstOrDefault();
                        Console.WriteLine(user);
                        if (user != null)
                        {
                            UserNameText.Text = $"👤 Name: {user.Name}";
                            //UserEmailText.Text = $"📧 Email: {user.Email}";
                            //UserCityText.Text = $"🌍 City: {user.Address.City}";
                            ErrorText.Visibility = Visibility.Collapsed;
                            HandleLoaderVisibility(false);
                        }
                    }
                    else
                    {
                        ShowError();
                    }
                }

                catch
                {
                    ShowError();
                }
            }
        }
        private void ShowError()
        {
            HandleLoaderVisibility(false);
            ErrorText.Text = "❌ An error occurred. Check your password again.";
            ErrorText.Visibility = Visibility.Visible;
            UserNameText.Text = "";
            UserEmailText.Text = "";
            UserCityText.Text = "";
        }
        private void HandleLoaderVisibility(bool isVisible)
        {
            if (isVisible)
            {
                Loader.Visibility = Visibility.Visible;
            }
            else
            {
                Loader.Visibility = Visibility.Collapsed;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += SecretCloseKey;
            _hookID = SetHook(HookCallback);
        }

        private void SecretCloseKey(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Q)
            {
                App.IsManualExit = true;
                Application.Current.Shutdown();
            }
        }
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                if (vkCode == VK_LWIN || vkCode == VK_RWIN)
                {
                    return (IntPtr)1; // Suppress key press
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        protected override void OnClosed(EventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
            base.OnClosed(e);
        }
    }
}


