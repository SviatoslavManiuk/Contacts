using System.Threading.Tasks;
using Contacts.Model;

namespace Contacts.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<UserModel> SignInAsync(string login, string password);

        Task<bool> SignUpAsync(string login, string password);
    }
}