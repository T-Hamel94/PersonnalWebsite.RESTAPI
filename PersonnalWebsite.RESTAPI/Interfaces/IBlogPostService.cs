using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Interfaces
{
    public interface IBlogPostService
    {
        public IEnumerable<BlogPostModel> GetBlogPosts();
        public BlogPostModel GetBlogPostByID(Guid blogPostID);
        public IEnumerable<BlogPostModel> GetBlogPostsByUsername(string username);
        public BlogPostModel CreateBlogPost(BlogPostModel blogPost);
        public BlogPostModel UpdateBlogPost(BlogPostModel blogPost);
        public void DeleteBlogPost(Guid blogPostID);
    }
}
