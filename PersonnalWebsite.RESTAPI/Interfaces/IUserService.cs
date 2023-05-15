using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Interfaces
{
    public interface IUserService
    {
        public UserModel GetUserByID(Guid p_userGuid);
        public IEnumerable<UserModel> GetUsers();
        public void UpdateUser(UserModel user);
        public void DeactivateUser(Guid p_user);
        public void DeleteUser(Guid p_userGuid);
    }
}
