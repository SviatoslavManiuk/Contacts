using System.ComponentModel;
using System.Windows.Input;
using Acr.UserDialogs;
using Contacts.Model;
using Contacts.Services.Authentication;
using Contacts.Services.Settings;
using Contacts.View;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Contacts.ViewModel
{
    public class SignInViewModel: BindableBase
    {
        private string _login;
        private string _password;

        private INavigationService _navigationService;
        private IAuthenticationService _authenticationService;
        
        public SignInViewModel(INavigationService navigationService, ISettingsManager settingsManager, IAuthenticationService authenticationService)
        {
            _navigationService = navigationService;
            _authenticationService = authenticationService;
        }
        
        #region --- Public Properties ---
        
        public ICommand SignUpLabelTapCommand => new Command(OnSignUpLabelTap);

        public ICommand SignInButtonTapCommand => new DelegateCommand(OnSignInButtonTap).ObservesCanExecute(()=>AllFieldsNotNull);
        
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

        #endregion

        #region --- Overrides ---
        
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(Login):
                case nameof(Password):
                    AllFieldsNotNull = !(string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password));
                    break;
            }
        }
        #endregion
 
        #region --- Private Halpers ---

        private async void OnSignInButtonTap()
        {
            UserModel user = await _authenticationService.SignInAsync(Login, Password);
            if (user != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add(nameof(UserModel), user);
                await _navigationService.NavigateAsync("/NavigationPage/"+nameof(MainList), parameters);
            }
            else
            {
                UserDialogs.Instance.Alert("Invalid login or password!");
            }
        }
        private async void OnSignUpLabelTap()
        {
            await _navigationService.NavigateAsync(nameof(SignUp));
        }

        #endregion
    }
}