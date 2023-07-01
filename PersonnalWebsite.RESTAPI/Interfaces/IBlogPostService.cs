using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Interfaces
{
    public interface IBlogPostService
    {
        public IEnumerable<BlogPostModel> GetBlogPosts();
        public IEnumerable<BlogPostModel> GetUnapprovedBlogPosts();
        public BlogPostModel GetBlogPostByID(Guid blogPostID);
        public IEnumerable<BlogPostModel> GetBlogPostsByUsername(string username);
        public BlogPostModel CreateBlogPost(Guid loggedInUserId, BlogPostModel blogPost);
        public BlogPostModel UpdateBlogPost(Guid loggedInUserId, BlogPostModel blogPost);
        public BlogPostModel ApproveBlogPost(Guid loggedInUserId, Guid blogPostID);
        public void DeleteBlogPost(Guid loggedInUserId, Guid blogPostID);
    }
}
