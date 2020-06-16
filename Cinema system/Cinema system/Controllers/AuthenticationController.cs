using Microsoft.AspNetCore.Mvc;
using CinemaSystem.Interfaces;
using CinemaSystem.Models;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authorizationService;

        private const string ErrorOfEmail = "This email is not valid.";
        private const string ErrorOfUserNonexisting = "This user is not exists.";
        private const string ErrorOfUserExisting = "This user is already exists.";
        private const string ErrorOfUserPasword = "Password entered incorrectly.";
        private const string ErrorOfUserName = "This name is already exists.";

        public AuthenticationController(IAuthenticationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost("[action]")]
        public IActionResult Login([FromBody] LoginInfo user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(ModelState));
            }

            if (!_authorizationService.CheckCorrectEmail(user.Email))
            {
                return BadRequest(new Response(ErrorOfEmail));
            }

            var findUser = _authorizationService.FindUser(user.Email);      

            if (findUser == null)
            {
                return BadRequest(new Response(ErrorOfUserNonexisting));
            }

            if (!_authorizationService.CheckCorrectPassword(user,findUser))
            {
                return BadRequest(new Response(ErrorOfUserPasword));
            }

            UserLoginInfo loginInfo = _authorizationService.LoginUser(findUser);

            return Ok(new LoginResponse(loginInfo));
        }

        [HttpPost("[action]")]
        public IActionResult Register([FromBody] RegistrationInfo user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(ModelState));
            }

            if (!_authorizationService.CheckCorrectEmail(user.Email))
            {
                return BadRequest(new Response(ErrorOfEmail));
            }

            var findUser = _authorizationService.FindUser(user.Email);

            if (findUser != null)
            {
                return BadRequest(new Response(ErrorOfUserExisting));
            }

            if (_authorizationService.CheckUserNameUsed(user.Email))
            {
                return BadRequest(new Response(ErrorOfUserName));
            }

            string token = _authorizationService.RegisterUser(user);

            return Ok(new LoginResponse(token));
        }
    }
}
