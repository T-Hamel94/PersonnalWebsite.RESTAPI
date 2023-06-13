using PersonnalWebsite.RESTAPI.Entities;
using System.Collections.Generic;

namespace PersonnalWebsite.RESTAPI.Interfaces
{
    public interface IBlogPostRepo
    {
        public IEnumerable<BlogPost> GetBlogPosts();
        public IEnumerable<BlogPost> GetBlogPostsByUsername(string username);
        public BlogPost GetBlogPostByID(Guid blogPostID);
        public BlogPost CreateBlogPost(BlogPost blogPost);
        public BlogPost UpdateBlogPost(BlogPost blogPost);
        public BlogPost ApproveBlogPost(BlogPost blogPost);
        public void DeleteBlogPost(Guid blogPostID);
    }
}
