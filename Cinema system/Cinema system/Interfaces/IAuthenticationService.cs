using CinemaSystem.Models;

namespace CinemaSystem.Interfaces
{
    public interface IAuthenticationService
    {
        User FindUser(string email);
        bool CheckCorrectPassword(LoginInfo user, User findUser);
        bool CheckUserNameUsed(string userName);
        bool CheckCorrectEmail(string email);
        UserLoginInfo LoginUser(User findUser);
        string RegisterUser(RegistrationInfo user);
    }
}