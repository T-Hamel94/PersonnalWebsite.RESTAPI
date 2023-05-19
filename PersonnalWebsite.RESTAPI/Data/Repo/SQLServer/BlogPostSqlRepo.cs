using PersonnalWebsite.RESTAPI.Data.Context;
using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;

namespace PersonnalWebsite.RESTAPI.Data.Repo.SQLServer
{
    public class BlogPostSqlRepo : IBlogPostRepo
    {
        private ApplicationDbContext _dbContext;

        public BlogPostSqlRepo()
        {
            _dbContext = DbContextGeneration.GetApplicationDBContext();
        }

        public BlogPost CreateBlogPost(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }

        public void DeleteBlogPost(Guid blogPostID)
        {
            throw new NotImplementedException();
        }

        public BlogPost GetBlogPostByID(Guid blogPostID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BlogPost> GetBlogPosts()
        {
            IEnumerable<BlogPost> posts = _dbContext.BlogPosts.Select(bp => bp.ToEntity());
            return posts;
        }

        public BlogPost GetBlogPostsByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public BlogPost UpdateBlogPost(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
