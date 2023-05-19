using Microsoft.IdentityModel.Tokens;
using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PersonnalWebsite.RESTAPI.Service
{
    // Uses User and return UserModel
    public class AuthService : IAuthService
    {
        private IUserRepo _userRepo;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;

        public AuthService(IUserRepo userRepo, IConfiguration configuration, IPasswordService passwordService)
        {
            _userRepo = userRepo;
            _configuration = configuration;
            _passwordService = passwordService; 
        }

        public string Login(string email, string password)
        {
            User userLoginIn = _userRepo.GetUserByEmail(email);

            if(userLoginIn == null)
            {
                // Log the error
                // Add custom exceptions here
                throw new Exception("The user trying to log in was not found");
            }

            if (!_passwordService.VerifyPasswordHash(password, userLoginIn.PasswordHash, userLoginIn.PasswordSalt))
            {
                // Log the error
                // Add custom exceptions here
                throw new Exception("The password hash did not match");
            }

            string JWT = CreateToken(userLoginIn);

            return JWT; 
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:TokenKey").Value));

            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            var JWT = new JwtSecurityTokenHandler().WriteToken(token);

            return JWT;
        }
    }
}