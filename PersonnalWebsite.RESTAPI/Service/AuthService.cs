using PersonnalWebsite.RESTAPI.Interfaces;
using System.Security.Cryptography;

namespace PersonnalWebsite.RESTAPI.Service
{
    // Uses User and return UserModel
    public class AuthService : IAuthService
    {
        //public AuthService()
        //{

        //}

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

        //public string Login(User request)
        //{
        //    if (_userRepo.GetUsers().Any(u => u.Username != request.Username))
        //    {
        //        return null; // The controller should return bad request "User not found"
        //    }

        //    if (!VerifyPasswordHash(request.Password, request.PasswordSalt, request.PasswordSalt))
        //    {
        //        return null; // The controller should return bad request "Wrong password"
        //    }

        //    return "MY CRAZY TOKEN"; // Return OK
        //}
    }
}
