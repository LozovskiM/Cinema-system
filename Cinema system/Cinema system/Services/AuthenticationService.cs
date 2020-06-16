using CinemaSystem.Interfaces;
using CinemaSystem.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CinemaSystem.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly CinemaDBContext _db;

        public AuthenticationService(CinemaDBContext db)
        {
            _db = db;
        }

        public User FindUser (string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool CheckCorrectPassword(LoginInfo user, User findUser)
        {
            return EncryptPassword(user.Password) == findUser.Password;
        }

        public bool CheckUserNameUsed(string userName)
        {
            return _db.Users.Any(u => u.UserName == userName);
        }

        public bool CheckCorrectEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private string EncryptPassword (string password)
        {
            using var sha256 = SHA256.Create();

            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            var hash = BitConverter.ToString(hashedBytes).ToLower();

            return hash;
        }

        private string GenerateToken(User findUser)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Key"]));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new[] {
                new Claim(ClaimTypes.Name, findUser.Email),
                new Claim(ClaimTypes.Role, findUser.Role),
                new Claim(ClaimTypes.NameIdentifier, findUser.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer:"CinemaSystemServer",
                audience:"ClientApp",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        public UserLoginInfo LoginUser(User findUser)
        {
            return new UserLoginInfo
            {
                UserName = findUser.UserName,
                Token = GenerateToken(findUser)
            };
        }

        public string RegisterUser(RegistrationInfo user)
        {
            var newUser = new User
            {
                Email = user.Email,
                Password = EncryptPassword(user.Password),
                Role = "user",
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            _db.Users.Add(newUser);

            _db.SaveChanges();

            string Token = GenerateToken(newUser);

            return Token;
        }
    }
}
