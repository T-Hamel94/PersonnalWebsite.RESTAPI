using PersonnalWebsite.RESTAPI.Entities;

namespace PersonnalWebsite.RESTAPI.Interfaces
{
    public interface IUserRepo
    {
        public IEnumerable<User> GetUsers();
        public User GetUserByID(Guid p_userGuid);
        public User GetUserByEmail(string email);
        public User GetUserByUsername(string username);
        public User CreateUser(User user);
        public User UpdateUser(User user);
        public void DeleteUser(Guid p_userGuid);
    }
}
