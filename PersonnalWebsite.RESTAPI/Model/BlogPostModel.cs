﻿using PersonnalWebsite.RESTAPI.Entities;

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

        public BlogPostModel() { }

        public BlogPostModel(Guid blogPostID, int blogPostLanguageID, string title, string author, string content, DateTime createdDate, DateTime updatedDate)
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
            return new BlogPost() 
            {
                BlogPostID = this.BlogPostID,
                BlogPostLanguageID = this.BlogPostLanguageID,
                Title = this.Title,
                Author = this.Author,
                Content = this.Content,
                CreatedDate = this.CreatedDate,
                UpdatedDate = this.UpdatedDate
            };

        }
    }

}
