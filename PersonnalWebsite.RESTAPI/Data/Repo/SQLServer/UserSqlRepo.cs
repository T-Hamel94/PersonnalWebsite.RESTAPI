using PersonnalWebsite.RESTAPI.Data.Context;
using PersonnalWebsite.RESTAPI.Data.SQLServer;
using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;

namespace PersonnalWebsite.RESTAPI.Data.Repo.SQLServer
{
    // Uses UserSqlDTO and returns User
    public class UserSqlRepo : IUserRepo
    {
        private ApplicationDbContext _dbContext;

        public UserSqlRepo()
        {
            _dbContext = DbContextGeneration.GetApplicationDBContext();
        }

        public User GetUserByID(Guid userGuid)
        {
            if (userGuid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userGuid));
            }

            UserSQLServer user = _dbContext.Users.Where(u => u.UserID == userGuid).SingleOrDefault();

            return user?.ToEntity();
        }

        public IEnumerable<User> GetUsers()
        {
            List<UserSQLServer> usersSQLDTO = _dbContext.Users.ToList();

            return usersSQLDTO.Select(u => u.ToEntity()).ToList();
        }

        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            UserSQLServer userSQLDTO = _dbContext.Users.Where(u => u.Email == email).FirstOrDefault();

            return userSQLDTO?.ToEntity();
        }

        public User CreateUser(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            UserSQLServer userSQLDTO = new UserSQLServer(user);
            _dbContext.Users.Add(userSQLDTO);
            _dbContext.SaveChanges();

            return userSQLDTO.ToEntity();
        }

        public User UpdateUser(User userUpdate)
        {
            if(userUpdate == null || userUpdate.Id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userUpdate));
            }

            UserSQLServer existingUser = _dbContext.Users.Find(userUpdate.Id);  

            if(existingUser == null)
            {
                throw new ArgumentException("User Not Found");
            }

            _dbContext.Users.Update(new UserSQLServer(userUpdate));
            _dbContext.SaveChanges();

            return userUpdate;
        }

        public void DeleteUser(Guid userGuid)
        {
            if(userGuid == Guid.Empty)
            {
                throw new ArgumentNullException();
            }

            UserSQLServer existingUser = _dbContext.Users.Find(userGuid);

            if(existingUser == null)
            {
                throw new ArgumentException("User not found");
            }

            _dbContext.Users.Remove(existingUser);
            _dbContext.SaveChanges();
        }
    }
}
