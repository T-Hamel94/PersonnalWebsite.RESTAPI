using Microsoft.IdentityModel.Tokens;
using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PersonnalWebsite.RESTAPI.Service
{
    // Uses User and return UserModel
    public class AuthService : IAuthService
    {
        private IUserRepo _userRepo;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepo userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _configuration = configuration;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) 
        { 
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public string Login(string email, string password)
        {
            User userLoginIn = _userRepo.GetUserByEmail(email);

            if(userLoginIn == null)
            {
                return null; // The controller should return bad request "User not found"
            }

            if (!VerifyPasswordHash(password, userLoginIn.PasswordHash, userLoginIn.PasswordSalt))
            {
                return null; // The controller should return bad request "Wrong password"
            }

            string JSWToken = CreateToken(userLoginIn);

            return JSWToken; // Return OK
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var JSWToken = new JwtSecurityTokenHandler().WriteToken(token);

            return JSWToken;
        }
    }
}