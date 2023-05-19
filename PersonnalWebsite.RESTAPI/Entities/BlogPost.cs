using PersonnalWebsite.RESTAPI.Data.SQLServer;
using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Entities
{
    public class BlogPost
    {
        public Guid BlogPostID { get; set; }
        public int BlogPostLanguageID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public BlogPost() { }

        public BlogPost(Guid blogPostID, int blogPostLanguageID, string title, string author, string content, DateTime createdDate, DateTime updatedDate)
        {
            BlogPostID = blogPostID;
            BlogPostLanguageID = blogPostLanguageID;
            Title = title;
            Author = author;
            Content = content;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }

        public BlogPost(BlogPostSQLServer blogPostSqlServer)
        {
            BlogPostID = blogPostSqlServer.BlogPostID;
            BlogPostLanguageID = blogPostSqlServer.BlogPostLanguageID;
            Title = blogPostSqlServer.Title;
            Author = blogPostSqlServer.Author;
            Content = blogPostSqlServer.Content;
            CreatedDate = blogPostSqlServer.CreatedDate;
            UpdatedDate = blogPostSqlServer.UpdatedDate;
        }

        public BlogPostModel ToModel()
        {
            return new BlogPostModel()
            {
                BlogPostID = this.BlogPostID,
                BlogPostLanguageID = this.BlogPostLanguageID,
                Title = this.Title,
                Author = this.Author,
                Content = this.Content,
                CreatedDate = this.CreatedDate,
                UpdatedDate = this.UpdatedDate,
            };
        }
    }
}
