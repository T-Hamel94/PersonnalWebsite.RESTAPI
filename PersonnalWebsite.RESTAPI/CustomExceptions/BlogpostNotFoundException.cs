namespace PersonnalWebsite.RESTAPI.CustomExceptions
{
    public class BlogpostNotFoundException : Exception
    {
        public BlogpostNotFoundException() : base()
        {
        }

        public BlogpostNotFoundException(string message) : base(message)
        {
        }

        public BlogpostNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
