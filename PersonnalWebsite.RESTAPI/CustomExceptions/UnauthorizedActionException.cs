namespace PersonnalWebsite.RESTAPI.CustomExceptions
{
    public class UnauthorizedActionException : Exception
    {
        public UnauthorizedActionException() : base()
        {
        }

        public UnauthorizedActionException(string message) : base(message)
        {
        }

        public UnauthorizedActionException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
