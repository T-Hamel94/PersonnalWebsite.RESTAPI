using PersonnalWebsite.RESTAPI.Data.Context;
using PersonnalWebsite.RESTAPI.Data.SQLServer;
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
            if(blogPost == null)
            {
                throw new ArgumentNullException(nameof(blogPost));
            }

            BlogPostSQLServer blogPostSQLServer = new BlogPostSQLServer(blogPost);

            _dbContext.Add(blogPostSQLServer);
            _dbContext.SaveChanges();
            return blogPost;
        }

        public BlogPost GetBlogPostByID(Guid blogPostID)
        {
            if (blogPostID == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(blogPostID));
            }

            BlogPostSQLServer blogPost = _dbContext.BlogPosts.Find(blogPostID);
            return blogPost.ToEntity();
        }

        public IEnumerable<BlogPost> GetBlogPosts()
        {
            IEnumerable<BlogPost> posts = _dbContext.BlogPosts.Select(bp => bp.ToEntity());
            return posts;
        }

        public IEnumerable<BlogPost> GetBlogPostsByUsername(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            IEnumerable<BlogPostSQLServer> blogPostsSqlServer = _dbContext.BlogPosts.Where(bp => bp.Author == username);
            IEnumerable<BlogPost> blogPosts = blogPostsSqlServer.Select(bp => bp.ToEntity());

            return blogPosts;
        }

        public BlogPost UpdateBlogPost(BlogPost blogPost)
        {
            if (blogPost == null)
            {
                throw new ArgumentNullException(nameof(blogPost));
            }

            BlogPostSQLServer existingBlogPost = _dbContext.BlogPosts.Find(blogPost.BlogPostID);

            if (existingBlogPost == null)
            {
                throw new ArgumentException("BlogPost with given id doesn't exist", nameof(blogPost));
            }

            existingBlogPost.BlogPostLanguageID = blogPost.BlogPostLanguageID;
            existingBlogPost.Title = blogPost.Title;
            existingBlogPost.Content = blogPost.Content;
            existingBlogPost.UpdatedDate = DateTime.Now;

            _dbContext.BlogPosts.Update(existingBlogPost);
            _dbContext.SaveChanges();

            return existingBlogPost.ToEntity();
        }

        public void DeleteBlogPost(Guid blogPostID)
        {
            if (blogPostID == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(blogPostID));
            }

            BlogPost postToDelete = GetBlogPostByID(blogPostID);

            if (postToDelete == null)
            {
                throw new Exception("Could not find the blog post to delete");
            }

            _dbContext.Remove(new BlogPostSQLServer(postToDelete));
            _dbContext.SaveChanges();
        }
    }
}
