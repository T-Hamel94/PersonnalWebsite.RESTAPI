using PersonnalWebsite.RESTAPI.Entities;

namespace PersonnalWebsite.RESTAPI.Interfaces
{
    public interface IBlogPostRepo
    {
        public BlogPost GetBlogPostByID(Guid blogPostID);
        public BlogPost GetBlogPostsByUsername(string username);
        public IEnumerable<BlogPost> GetBlogPosts();
        public BlogPost CreateBlogPost(BlogPost blogPost);
        public BlogPost UpdateBlogPost(BlogPost blogPost);
        public void DeleteBlogPost(Guid blogPostID);
    }
}
