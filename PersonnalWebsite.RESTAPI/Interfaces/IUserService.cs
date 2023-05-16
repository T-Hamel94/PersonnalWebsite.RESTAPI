using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Interfaces
{
    public interface IUserService
    {
        public UserModel GetUserByID(Guid p_userGuid);
        public UserModel GetUserByEmail(string email);
        public IEnumerable<UserModel> GetUsers();
        public UserModel RegisterUser(UserRegistrationModel model);
        public UserModel CreateUser(UserModel user);
        public UserModel UpdateUser(UserModel user);
        public void DeactivateUser(Guid p_user);
        public void DeleteUser(Guid p_userGuid);
    }
}
