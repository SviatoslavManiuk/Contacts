using System.ComponentModel;
using Contacts.Services.Settings;
using Prism.Mvvm;
using Prism.Navigation;

namespace Contacts.ViewModel
{
    public class SettingsViewModel:BindableBase, INavigationAware
    {
        private ISettingsManager _settingsManager;
        private INavigationService _navigationService;

        public SettingsViewModel(ISettingsManager settingsManager, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _settingsManager = settingsManager;
            _indexSelectedSort = _settingsManager.SelectedSort;
            switch (_indexSelectedSort)
            {
                case 0:
                    SelectedSort = "NickName";
                    break;
                case 1:
                    SelectedSort = "Name";
                    break;
                case 2:
                    SelectedSort = "Date";
                    break;
            }
        }

        #region --- Public Properties ---
        
        private object _selectedSort;

        public object SelectedSort
        {
            get => _selectedSort;
            set => SetProperty(ref _selectedSort, value);
        }
        
        private int _indexSelectedSort;
        public int IndexSelectedSort
        {
            get => _indexSelectedSort;
            set => SetProperty(ref _indexSelectedSort, value);
        }
        
        #endregion

        #region --- Public Methods ---
        
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            var parameter = new NavigationParameters();
            parameters.Add("SelectedSort", IndexSelectedSort);
            _navigationService.GoBackAsync(parameter);
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }
        
        #endregion
        
        #region --- Overrides
        
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(SelectedSort):
                    switch ((string) SelectedSort)
                    {
                        case "NickName":
                            _indexSelectedSort = 0;
                            break;
                        case "Name":
                            _indexSelectedSort = 1;
                            break;
                        case "Date":
                            _indexSelectedSort = 2;
                            break;
                    }
                    _settingsManager.SelectedSort = _indexSelectedSort;
                    break;
            }
        }
        
        #endregion
        
    }
}