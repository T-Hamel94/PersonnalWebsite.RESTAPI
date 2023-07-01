namespace PersonnalWebsite.RESTAPI.Interfaces
{
    public interface IAuthService
    {
        public string Login(string email, string password);
    }
}
