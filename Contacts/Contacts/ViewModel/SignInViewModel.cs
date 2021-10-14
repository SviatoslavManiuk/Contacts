using System.ComponentModel;
using System.Windows.Input;
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
        
        public SignInViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        #region --- Public Properties ---

        public ICommand SignUpLabelTapCommand => new Command(OnSignUpLabelTap);

        public ICommand SignInButtonTapCommand =>
            new DelegateCommand(OnSignInButtonTap).ObservesCanExecute(() => AllFieldsNotEmpty());
        
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

        #endregion
 
        #region --- Private Halpers ---

        private async void OnSignInButtonTap()
        {
            await _navigationService.NavigateAsync("/"+nameof(MainListView));
        }
        private async void OnSignUpLabelTap()
        {
            await _navigationService.NavigateAsync(nameof(SignUpView));
        }

        private bool AllFieldsNotEmpty()
        {
            return !(string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password));
        }

        #endregion
    }
}