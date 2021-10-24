using Contacts.DAL;
using Contacts.Model;
using Contacts.Services.Authentication;
using Contacts.Services.Repository;
using Contacts.Services.Settings;
using Contacts.View;
using Contacts.ViewModel;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using Xamarin.Forms;

namespace Contacts
{
    public partial class App : PrismApplication
    {
        public App()
        {
        }

        #region --- Overrides ---

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAuthenticationService>(Container.Resolve<AuthenticationWithLogin>());
            containerRegistry.RegisterInstance<UserService>(Container.Resolve<UserService>());
            containerRegistry.RegisterInstance<ContactService>(Container.Resolve<ContactService>());
            

            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignIn, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUp, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainList, MainListViewModel>();
            containerRegistry.RegisterForNavigation<AddEditProfile, AddEditProfileViewModel>();
            containerRegistry.RegisterForNavigation<Settings>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var settingsManager = new SettingsManager();
            int userId = settingsManager.UserId;
            if (userId != -1)
            {
                var parameter = new NavigationParameters();
                parameter.Add("userId", userId);
                await NavigationService.NavigateAsync("NavigationPage/" + nameof(MainList), parameter);
            }
            else
            {
                await NavigationService.NavigateAsync("NavigationPage/" + nameof(SignIn));
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #endregion
    }
}
