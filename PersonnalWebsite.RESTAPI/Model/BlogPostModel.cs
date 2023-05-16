namespace PersonnalWebsite.RESTAPI.Model
{
    public class BlogPostModel
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
