using Srv_PersonnalWebsite.Entity;

namespace Srv_PersonnalWebsite
{
    public interface IUserRepo
    {
        public User GetUserByID(Guid p_userGuid);
        public IEnumerable<User> GetUsers();
        public void UpdateUser(User user);
        public void DeactivateUser(Guid p_user);
        public void DeleteUser(Guid p_userGuid);
    }
}