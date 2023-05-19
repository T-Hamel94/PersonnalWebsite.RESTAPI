using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Service
{
    public class BlogPostService : IBlogPostService
    {
        private IBlogPostRepo _blogPostRepo;

        public BlogPostService(IBlogPostRepo blogPostRepo)
        {
            _blogPostRepo = blogPostRepo;
        }
        public BlogPostModel CreateBlogPost(BlogPostModel blogPost)
        {
            throw new NotImplementedException();
        }

        public void DeleteBlogPost(Guid blogPostID)
        {
            throw new NotImplementedException();
        }

        public BlogPostModel GetBlogPostByID(Guid blogPostID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BlogPostModel> GetBlogPosts()
        {
            IEnumerable<BlogPostModel> blogPostsModel = _blogPostRepo.GetBlogPosts().Select(bp => bp.ToModel());
            return blogPostsModel;
        }

        public BlogPostModel GetBlogPostsByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public BlogPostModel UpdateBlogPost(BlogPostModel blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
