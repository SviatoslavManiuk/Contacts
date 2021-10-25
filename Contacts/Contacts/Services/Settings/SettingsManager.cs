using Xamarin.Essentials;

namespace Contacts.Services.Settings
{
    public class SettingsManager: ISettingsManager
    {
        public int UserId
        {
            get => Preferences.Get(nameof(UserId), -1);
            set => Preferences.Set(nameof(UserId), value);
        }

        public int SelectedSort
        {
            get => Preferences.Get(nameof(SelectedSort), 0);
            set => Preferences.Set(nameof(SelectedSort), value);
        }
    }
}