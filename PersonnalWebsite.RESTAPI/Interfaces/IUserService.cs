using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Interfaces
{
    public interface IUserService
    {
        public IEnumerable<UserModel> GetUsers();
        public UserModel GetUserByID(Guid p_userGuid);
        public UserModel GetUserByEmail(string email);
        public UserModel RegisterUser(UserRegistrationModel model);
        public UserModel CreateUser(UserModel user);
        public UserModel UpdateUser(UserModel user);
        public void DeleteUser(Guid p_userGuid);
    }
}
