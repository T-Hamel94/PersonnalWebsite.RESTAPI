using PersonnalWebsite.RESTAPI.CustomExceptions;
using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Service
{
    public class BlogPostService : IBlogPostService
    {
        private IBlogPostRepo _blogPostRepo;
        private IUserRepo _userRepo;

        public BlogPostService(IBlogPostRepo blogPostRepo, IUserRepo userRepo)
        {
            _blogPostRepo = blogPostRepo;
            _userRepo = userRepo;
        }

        public IEnumerable<BlogPostModel> GetBlogPosts()
        {
            IEnumerable<BlogPostModel> blogPostsModel = _blogPostRepo.GetBlogPosts().Select(bp => bp.ToModel());

            return blogPostsModel;
        }

        public BlogPostModel GetBlogPostByID(Guid blogPostID)
        {
            BlogPost blogPostFound = _blogPostRepo.GetBlogPostByID(blogPostID);

            return blogPostFound.ToModel();
        }

        public IEnumerable<BlogPostModel> GetBlogPostsByUsername(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            User userFound = _userRepo.GetUserByUsername(username);

            IEnumerable<BlogPost> blogPostFound = _blogPostRepo.GetBlogPostsByUsername(userFound.Username);

            return blogPostFound.Select(bp => bp.ToModel());
        }

        public BlogPostModel CreateBlogPost(Guid loggedInUserId, BlogPostModel blogPost)
        {
            if (blogPost == null)
            {
                throw new ArgumentNullException(nameof(blogPost));
            }

            if (blogPost.AuthorID != loggedInUserId)
            {
                throw new UnauthorizedActionException("The current logged in user cannot post an article under another user");
            }

            User authorFound = _userRepo.GetUserByID(loggedInUserId);

            if (authorFound == null)
            {
                throw new UserNotFoundException($"Could not find user with ID {loggedInUserId}");
            }

            BlogPost blogPostToCreate = new BlogPost()
            {
                BlogPostID = Guid.NewGuid(),
                BlogPostLanguageID = blogPost.BlogPostLanguageID,
                Title = blogPost.Title,
                Author = authorFound.Username,
                AuthorID = authorFound.Id,
                Content = blogPost.Content,
                IsApproved = false,
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

        public BlogPostModel UpdateBlogPost(Guid loggedInUserId, BlogPostModel blogPost)
        {
            if (blogPost == null)
            {
                throw new ArgumentNullException(nameof(blogPost));
            }

            Guid authorId = _userRepo.GetUserByUsername(blogPost.Author).Id;
            if (authorId != loggedInUserId)
            {
                throw new UnauthorizedActionException($"User trying to updated blogpost {blogPost.BlogPostID} is not authorized");
            }

            BlogPost blogPostUpdated = _blogPostRepo.UpdateBlogPost(blogPost.ToEntity());

            return blogPostUpdated.ToModel();
        }

        public void DeleteBlogPost(Guid loggedInUserId, Guid blogPostID)
        {
            Guid originalAuthorID = _blogPostRepo.GetBlogPostByID(blogPostID).AuthorID;
            
            if (originalAuthorID != loggedInUserId)
            {
                throw new UnauthorizedActionException($"User trying to delete {blogPostID} is not authorized");
            }

            _blogPostRepo.DeleteBlogPost(blogPostID);
        }
    }
}
