using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Service
{
    // Uses User and returns UserModel
    public class UserService : IUserService
    {
        private IUserRepo _userRepo;
        private IPasswordService _passwordService;

        public UserService(IUserRepo userRepo, IPasswordService passwordService)
        {
            _userRepo = userRepo;
            _passwordService = passwordService;
        }

        public UserModel RegisterUser(UserRegistrationModel model)
        {
            _passwordService.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Email = model.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Birthdate = model.Birthdate,
                IsAdmin = false,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };

            _userRepo.CreateUser(user);

            return user.ToModel();
        }

        public UserModel CreateUser(UserModel user)
        {
            User createdUser = _userRepo.CreateUser(user.ToEntity());

            return createdUser.ToModel();
        }

        public UserModel GetUserByID(Guid p_userGuid)
        {
            UserModel user = _userRepo.GetUserByID(p_userGuid).ToModel();

            return user;
        }

        public UserModel GetUserByEmail(string email)
        {
            User user = _userRepo.GetUserByEmail(email);

            if (user == null)
            {
                throw new Exception("Could not find User with given email");
            }

            return user.ToModel();
        }

        public IEnumerable<UserModel> GetUsers()
        {
            IEnumerable<UserModel> users = _userRepo.GetUsers().Select(u => u.ToModel());

            return users;
        }

        public UserModel UpdateUser(UserModel user)
        {
            User userToUpdate = _userRepo.UpdateUser(user.ToEntity());

            return userToUpdate.ToModel();
        }


        public void DeleteUser(Guid userGuid)
        {
            _userRepo.DeleteUser(userGuid);
        }
    }
}
