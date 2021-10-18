using System.Threading.Tasks;
using Contacts.DAL;
using Contacts.Model;
using Contacts.Services.Settings;

namespace Contacts.Services.Authentication
{
    public class AuthenticationWithLogin : IAuthenticationService
    {
        private UserService _userService;
        private ISettingsManager _settingsManager;

        public AuthenticationWithLogin(UserService userService, ISettingsManager settingsManager)
        {
            _userService = userService;
            _settingsManager = settingsManager;
        }
        
        public async Task<UserModel> SignInAsync(string login, string password)
        {
            UserModel user = await _userService.FindByLoginAsync(login);
            if (user == null || user.Password != password)
                return null;

            _settingsManager.UserId = user.Id;
            return user;
        }

        public async Task<bool> SignUpAsync(string login, string password)
        {
            UserModel res = await _userService.FindByLoginAsync(login);
            if (res == null)
            {
                UserModel user = new UserModel() {Login = login, Password = password};
                await _userService.InsertAsync(user);
                return true;
            }
            
            return false;
        }
    }
}