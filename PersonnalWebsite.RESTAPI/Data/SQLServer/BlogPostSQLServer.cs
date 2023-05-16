namespace PersonnalWebsite.RESTAPI.Data.SQLServer
{
    public class BlogPostSQLServer
    {
        public Guid BlogPostID { get; set; }
        public int BlogPostLanguageID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
