using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace Contacts.ViewModel
{
    public class SignUpViewModel: BindableBase
    {
        private string _login;
        private string _password;
        private string _confirmPassword;

        private INavigationService _navigationService;
        
        public SignUpViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        #region --- Public Properties ---

        public ICommand SignUpButtonTapCommand => new DelegateCommand(OnSignUpButtonTap).ObservesCanExecute(()=>AllFieldsNotNull);
        
        private bool _allFieldsNotNull;
        public bool AllFieldsNotNull
        {
            get => _allFieldsNotNull;
            set => SetProperty(ref _allFieldsNotNull, value);
        }
        
        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }
        
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        #endregion

        #region --- Overrides ---
        
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(Login):
                case nameof(Password):
                case nameof(ConfirmPassword):
                    AllFieldsNotNull = !(string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password)
                                                                          || string.IsNullOrWhiteSpace(ConfirmPassword));
                    break;
            }
        }
        #endregion
 
        #region --- Private Halpers ---

        private async void OnSignUpButtonTap()
        {
            if (IsValid())
            {
                await _navigationService.GoBackAsync();
            }
        }

        private bool IsValid()
        {
            if (!Regex.IsMatch(Login, @"^((?=\D+.*).{4,16})$"))
            {
                UserDialogs.Instance.Alert("Login must be at least 4 and at most 16 symbols" +
                                           " and must not start with digits!");
                return false;
            }

            if (Password != ConfirmPassword)
            {
                UserDialogs.Instance.Alert("Password and password confirmation must match!");
                return false;
            }
            
            if (!Regex.IsMatch(Password, @"^((?=.*\d)(?=.*[A-Z])(?=.*[a-z]).{8,16})$"))
            {
                UserDialogs.Instance.Alert("Password must be at least 8 and at most 16 symbols" +
                                           " and must contain at least one digit, one uppercase and one lowercase letter!");
                return false;
            }

            return true;
        }
        
        #endregion
    }
}