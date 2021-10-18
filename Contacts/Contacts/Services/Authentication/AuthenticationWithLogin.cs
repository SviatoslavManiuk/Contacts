using System.Threading.Tasks;
using Contacts.DAL;
using Contacts.Model;
using Contacts.Services.Settings;

namespace Contacts.Services.Authentication
{
    public class AuthenticationWithLogin : IAuthenticationService
    {
        private UserDAO _userDao;
        private ISettingsManager _settingsManager;

        public AuthenticationWithLogin(UserDAO userDao, ISettingsManager settingsManager)
        {
            _userDao = userDao;
            _settingsManager = settingsManager;
        }
        
        public async Task<UserModel> SignInAsync(string login, string password)
        {
            UserModel user = await _userDao.FindByLoginAsync(login);
            if (user == null || user.Password != password)
                return null;

            _settingsManager.UserId = user.Id;
            return user;
        }

        public async Task<bool> SignUpAsync(string login, string password)
        {
            UserModel res = await _userDao.FindByLoginAsync(login);
            if (res == null)
            {
                UserModel user = new UserModel() {Login = login, Password = password};
                await _userDao.InsertAsync(user);
                return true;
            }
            
            return false;
        }
    }
}