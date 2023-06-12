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
        public Guid AuthorID { get; set; }
        public string Content { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public BlogPost() { }

        public BlogPost(Guid blogPostID, int blogPostLanguageID, string title, string author, Guid authorID, string content, bool isApproved, DateTime createdDate, DateTime updatedDate)
        {
            BlogPostID = blogPostID;
            BlogPostLanguageID = blogPostLanguageID;
            Title = title;
            Author = author;
            AuthorID = authorID;
            Content = content;
            IsApproved = isApproved;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }

        public BlogPost(BlogPostSQLServer blogPostSqlServer)
        {
            BlogPostID = blogPostSqlServer.BlogPostID;
            BlogPostLanguageID = blogPostSqlServer.BlogPostLanguageID;
            Title = blogPostSqlServer.Title;
            Author = blogPostSqlServer.Author;
            AuthorID = blogPostSqlServer.AuthorID;
            Content = blogPostSqlServer.Content;
            IsApproved = blogPostSqlServer.IsApproved;
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
                AuthorID = this.AuthorID,
                Content = this.Content,
                IsApproved = this.IsApproved,
                CreatedDate = this.CreatedDate,
                UpdatedDate = this.UpdatedDate,
            };
        }
    }
}
