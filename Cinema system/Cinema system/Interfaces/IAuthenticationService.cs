using CinemaSystem.Models;

namespace CinemaSystem.Interfaces
{
    public interface IAuthenticationService
    {
        bool CheckUserExists(string email);
        bool CheckCorrectPassword(LoginInfo user);
        bool CheckUserNameUsed(string userName);
        bool CheckCorrectEmail(string email);
        UserLoginInfo LoginUser(LoginInfo user);
        string RegisterUser(RegistrationInfo user);
    }
}