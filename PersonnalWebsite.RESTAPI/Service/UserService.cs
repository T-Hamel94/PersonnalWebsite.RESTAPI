using PersonnalWebsite.RESTAPI.CustomExceptions;
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

        public IEnumerable<UserPublicModel> GetUsers()
        {
            IEnumerable<UserPublicModel> users = _userRepo.GetUsers().Select(u => u.ToPublicModel());

            return users.OrderBy(u => u.Username);
        }

        public UserModel GetUserByID(Guid userID)
        {
            User user = _userRepo.GetUserByID(userID);

            return user.ToModel();
        }

        public UserModel GetUserByEmail(string email)
        {
            User user = _userRepo.GetUserByEmail(email);

            return user.ToModel();
        }

        public UserModel RegisterUser(UserRegistrationModel model)
        {
            _passwordService.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User newUser = new User()
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

            _userRepo.CreateUser(newUser);

            return newUser.ToModel();
        }

        public UserModel CreateUser(Guid loggedInUserId, UserModel newUser)
        {
            User loggedInUser = GetUserByID(loggedInUserId)?.ToEntity();

            if (!loggedInUser.IsAdmin)
            {
                throw new UnauthorizedActionException("Only an admin user can create a user without using registration");
            }

            User createdUser = _userRepo.CreateUser(newUser.ToEntity());

            return createdUser.ToModel();
        }

        public UserModel UpdateUser(Guid loggedInUserId, UserModel user)
        {
            if (user.Id != loggedInUserId)
            {
                throw new UnauthorizedActionException("Logged in user and user to update ID's do not match");
            }

            User userToUpdate = _userRepo.UpdateUser(user.ToEntity());

            return userToUpdate.ToModel();
        }

        public void DeleteUser(Guid loggedInUserId, Guid userToDeleteID)
        {
            if (userToDeleteID != loggedInUserId)
            {
                throw new UnauthorizedActionException("Logged in user and user to delete ID's do not match");
            }

            _userRepo.DeleteUser(userToDeleteID);
        }
    }
}
