using CinemaSystem.Interfaces;
using CinemaSystem.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace CinemaSystem.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly CinemaDBContext _db;
        private readonly IConfiguration _config;

        public AuthenticationService(IConfiguration config, CinemaDBContext db)
        {
            _db = db;
            _config = config;
        }

        public bool CheckUserExists(string email)
        {
            return _db.Users.Any(u => u.Email == email);
        }

        public bool CheckCorrectPassword(LoginInfo user)
        {
            return user.Password == _db.Users.FirstOrDefault(u => u.Email == user.Email).Password;
        }

        public bool CheckUserNameUsed(string userName)
        {
            return _db.Users.Any(u => u.UserName == userName);
        }

        public bool CheckCorrectEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }

        private string GenerateToken(string userEmail)
        {
            //I'm not sure i understand what to use as a key and issuer/audience
            const string key = "0d5b3235a8b403c3dab9c3f4f65c07fcalskd234n1k41230";
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new[] {
                new Claim(ClaimTypes.Name, userEmail),
                new Claim(ClaimTypes.Role, _db.Users.FirstOrDefault(u => u.Email == userEmail).Role),
                new Claim(ClaimTypes.NameIdentifier, _db.Users.FirstOrDefault(u => u.Email == userEmail).Id.ToString())
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

        public UserLoginInfo LoginUser(LoginInfo user)
        {
            return new UserLoginInfo
            {
                UserName = _db.Users.FirstOrDefault(u => u.Email == user.Email).UserName,
                Token = GenerateToken(user.Email)
            };
        }

        public string RegisterUser(RegistrationInfo user)
        {
            _db.Users.Add(new User
            {
                Email = user.Email,
                Password = user.Password,
                Role = "user",
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName
            });

            _db.SaveChanges();

            string Token = GenerateToken(user.Email);

            return Token;
        }
    }
}
