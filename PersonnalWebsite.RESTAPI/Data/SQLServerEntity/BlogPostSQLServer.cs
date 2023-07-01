using PersonnalWebsite.RESTAPI.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonnalWebsite.RESTAPI.Data.SQLServerEntity
{
    public class BlogPostSQLServer
    {
        [Key]
        [Column("BlogPostID")]
        public Guid BlogPostID { get; set; }
        [Column("BlogPostLanguageID")]
        public int BlogPostLanguageID { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("Author")]
        public string Author { get; set; }
        [ForeignKey("User")]
        [Column("AuthorID")]
        public Guid AuthorID { get; set; }
        [Column("Content")]
        public string Content { get; set; }
        [Column("IsApproved")]
        public bool IsApproved { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedDate { get; set; }
        [Column("LastModifiedAt")]
        public DateTime UpdatedDate { get; set; }

        public BlogPostSQLServer() { }

        public BlogPostSQLServer(Guid blogPostID, int blogPostLanguageID, string title, string author, Guid authorID, string content, bool isApproved, DateTime createdDate, DateTime updatedDate)
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

        public BlogPostSQLServer(BlogPost blogpost)
        {
            this.BlogPostID = blogpost.BlogPostID;
            this.BlogPostLanguageID = blogpost.BlogPostLanguageID;
            this.Title = blogpost.Title;    
            this.Author = blogpost.Author;
            this.AuthorID = blogpost.AuthorID;
            this.Content = blogpost.Content;
            this.IsApproved = blogpost.IsApproved;
            this.CreatedDate = blogpost.CreatedDate;
            this.UpdatedDate = blogpost.UpdatedDate;
        }

        public BlogPost ToEntity()
        {
            return new BlogPost(this);
        }
    }
}
