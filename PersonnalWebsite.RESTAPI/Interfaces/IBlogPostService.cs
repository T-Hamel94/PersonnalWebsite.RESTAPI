using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Interfaces
{
    public interface IBlogPostService
    {
        public BlogPostModel GetBlogPostByID(Guid blogPostID);
        public BlogPostModel GetBlogPostsByUsername(string username);
        public IEnumerable<BlogPostModel> GetBlogPosts();
        public BlogPostModel CreateBlogPost(BlogPostModel blogPost);
        public BlogPostModel UpdateBlogPost(BlogPostModel blogPost);
        public void DeleteBlogPost(Guid blogPostID);
    }
}
