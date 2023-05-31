using PersonnalWebsite.RESTAPI.Entities;
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

        public IEnumerable<BlogPostModel> GetBlogPosts()
        {
            IEnumerable<BlogPostModel> blogPostsModel = _blogPostRepo.GetBlogPosts().Select(bp => bp.ToModel());
            return blogPostsModel;
        }

        public BlogPostModel GetBlogPostByID(Guid blogPostID)
        {
            BlogPost blogPostFound = _blogPostRepo.GetBlogPostByID(blogPostID);
            
            if (blogPostFound == null)
            {
                throw new Exception("Could not find the blogpost");
            }

            return blogPostFound.ToModel();
        }

        public IEnumerable<BlogPostModel> GetBlogPostsByUsername(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            IEnumerable<BlogPost> blogPostFound = _blogPostRepo.GetBlogPostsByUsername(username);

            return blogPostFound.Select(bp => bp.ToModel());
        }

        public BlogPostModel CreateBlogPost(BlogPostModel blogPost)
        {
            if (blogPost == null)
            {
                throw new ArgumentNullException(nameof(blogPost));
            }

            BlogPost blogPostToCreate = new BlogPost()
            {
                BlogPostID = Guid.NewGuid(),
                BlogPostLanguageID = blogPost.BlogPostLanguageID,
                Title = blogPost.Title,
                Author = blogPost.Author,
                Content = blogPost.Content,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            BlogPost createdBlogPost = _blogPostRepo.CreateBlogPost(blogPostToCreate);

            if (createdBlogPost == null)
            {
                throw new Exception("Blog post was not created");
            }

            return createdBlogPost.ToModel();
        }

        public BlogPostModel UpdateBlogPost(BlogPostModel blogPost)
        {
            if (blogPost == null)
            {
                throw new ArgumentNullException(nameof(blogPost));
            }

            BlogPost blogPostUpdated = _blogPostRepo.UpdateBlogPost(blogPost.ToEntity());

            return blogPostUpdated.ToModel();
        }

        public void DeleteBlogPost(Guid blogPostID)
        {
            _blogPostRepo.DeleteBlogPost(blogPostID);
        }
    }
}
