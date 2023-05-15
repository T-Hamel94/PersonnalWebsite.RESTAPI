using PersonnalWebsite.RESTAPI.Data.Repo;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Service
{
    // Uses User and returns UserModel
    public class UserManipulation : IUserService
    {
        private IUserRepo _userRepo;

        public UserManipulation(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public void DeactivateUser(Guid p_user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(Guid p_userGuid)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUserByID(Guid p_userGuid)
        {
            UserModel user = _userRepo.GetUserByID(p_userGuid).ToModel();

            // Eventual business logic ?

            return user;
        }

        public IEnumerable<UserModel> GetUsers()
        {
            IEnumerable<UserModel> users = _userRepo.GetUsers().Select(u => u.ToModel());
            
            // Eventual business logic ?

            return users;
        }

        public void UpdateUser(UserModel user)
        {
            throw new NotImplementedException();
        }
    }
}
