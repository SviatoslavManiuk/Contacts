using Contacts.Services.Repository;
using Contacts.Services.Settings;
using Contacts.View;
using Contacts.ViewModel;
using Prism.Ioc;
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
            
            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignIn, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUp, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainList>();
            containerRegistry.RegisterForNavigation<AddEditProfile>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var settingsManager = new SettingsManager();
            if (settingsManager.IsAuthorized)
            {
                await NavigationService.NavigateAsync("NavigationPage/" + nameof(MainList));
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
