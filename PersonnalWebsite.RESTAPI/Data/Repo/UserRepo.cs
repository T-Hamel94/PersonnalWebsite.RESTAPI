using PersonnalWebsite.RESTAPI.Data.Context;
using Srv_PersonnalWebsite;
using Srv_PersonnalWebsite.Entity;

namespace PersonnalWebsite.RESTAPI.Data.Repo
{
    public class UserRepo : IUserRepo
    {
        private ApplicationDbContext _dbContext;

        public UserRepo()
        {
            _dbContext = DbContextGeneration.GetApplicationDBContext();
        }

        public void DeactivateUser(Guid user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public User GetUserByID(Guid userGuid)
        {
            User user = new User();

            if(userGuid != Guid.Empty)
            {
                user = _dbContext.Users.Where(u => u.UserID == userGuid).SingleOrDefault()?.ToEntity();
            }

            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            List<UserSQLDTO> usersSQLDTO = _dbContext.Users.ToList();

            return usersSQLDTO.Select(u => u.ToEntity()).ToList();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
