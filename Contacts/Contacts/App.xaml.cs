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
            
            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignIn, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUp>();
            containerRegistry.RegisterForNavigation<MainList>();
            containerRegistry.RegisterForNavigation<AddEditProfile>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/" + nameof(SignIn));
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
