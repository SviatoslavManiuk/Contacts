using Xamarin.Essentials;

namespace Contacts.Services.Settings
{
    public class SettingsManager: ISettingsManager
    {
        public bool IsAuthorized
        {
            get => Preferences.Get(nameof(IsAuthorized), false);
            set => Preferences.Set(nameof(IsAuthorized), value);
        }
    }
}