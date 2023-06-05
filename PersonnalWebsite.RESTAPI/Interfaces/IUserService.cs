using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Interfaces
{
    public interface IUserService
    {
        public IEnumerable<UserPublicModel> GetUsers();
        public UserModel GetUserByID(Guid p_userGuid);
        public UserModel GetUserByEmail(string email);
        public UserModel RegisterUser(UserRegistrationModel model);
        public UserModel CreateUser(Guid loggedInUserId, UserModel user);
        public UserModel UpdateUser(Guid loggedInUserId, UserModel user);
        public void DeleteUser(Guid loggedInUserId, Guid p_userGuid);
    }
}
