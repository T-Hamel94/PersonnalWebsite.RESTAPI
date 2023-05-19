using PersonnalWebsite.RESTAPI.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonnalWebsite.RESTAPI.Data.SQLServer
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
        [Column("Content")]
        public string Content { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedDate { get; set; }
        [Column("LastModifiedAt")]
        public DateTime UpdatedDate { get; set; }

        public BlogPostSQLServer() { }

        public BlogPostSQLServer(Guid blogPostID, int blogPostLanguageID, string title, string author, string content, DateTime createdDate, DateTime updatedDate)
        {
            BlogPostID = blogPostID;
            BlogPostLanguageID = blogPostLanguageID;
            Title = title;
            Author = author;
            Content = content;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }

        public BlogPost ToEntity()
        {
            return new BlogPost(this);
        }
    }
}
