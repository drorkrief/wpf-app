using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using wpf_app.Models;
namespace wpf_app.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _userName;
        private string _userEmail;
        private string _userCity;
        private string _errorText;
        private bool _isLoaderVisible;

        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }

        public string UserEmail
        {
            get => _userEmail;
            set { _userEmail = value; OnPropertyChanged(); }
        }

        public string UserCity
        {
            get => _userCity;
            set { _userCity = value; OnPropertyChanged(); }
        }

        public string ErrorText
        {
            get => _errorText;
            set { _errorText = value; OnPropertyChanged(); }
        }

        public bool IsLoaderVisible
        {
            get => _isLoaderVisible;
            set { _isLoaderVisible = value; OnPropertyChanged(); }
        }

        public ICommand ProcessCardCommand { get; }

        public MainWindowViewModel()
        {
            ProcessCardCommand = new RelayCommand<string>(async (cardData) => await ProcessCardAsync(cardData));
        }

        private async Task ProcessCardAsync(string cardData)
        {
            IsLoaderVisible = true;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"https://jsonplaceholder.typicode.com/users/1";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<User>(json);

                        UserName = $"👤 Name: {user.Name}";
                        UserEmail = $"📧 Email: {user.Email}";
                        UserCity = $"🌍 City: {user.Address.City}";
                        ErrorText = string.Empty;
                    }
                    else
                    {
                        ShowError("Failed to fetch user data.");
                    }
                }
            }
            catch
            {
                ShowError("An error occurred. Please try again.");
            }
            finally
            {
                IsLoaderVisible = false;
            }
        }

        private void ShowError(string message)
        {
            ErrorText = message;
            UserName = string.Empty;
            UserEmail = string.Empty;
            UserCity = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
