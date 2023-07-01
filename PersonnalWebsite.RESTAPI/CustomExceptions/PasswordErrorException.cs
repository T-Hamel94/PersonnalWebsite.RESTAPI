namespace PersonnalWebsite.RESTAPI.CustomExceptions
{
    public class PasswordErrorException : Exception
    {
        public PasswordErrorException() : base()
        {
        }

        public PasswordErrorException(string message) : base(message)
        {
        }

        public PasswordErrorException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
