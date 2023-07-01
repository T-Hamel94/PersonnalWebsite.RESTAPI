using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Interfaces
{
    public interface IUserService
    {
        public IEnumerable<UserPublicModel> GetUsers();
        public UserModel GetUserByID(Guid userID);
        public UserModel GetUserByEmail(string email);
        public UserModel RegisterUser(UserRegistrationModel userToRegister);
        public UserModel CreateUser(Guid loggedInUserId, UserModel userToCreateID);
        public UserModel UpdateUser(Guid loggedInUserId, UserModel userToUpdateID);
        public void DeleteUser(Guid loggedInUserId, Guid userToDeleteID);
    }
}
