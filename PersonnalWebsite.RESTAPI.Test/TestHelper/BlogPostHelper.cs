
using PersonnalWebsite.RESTAPI.Entities;

namespace PersonnalWebsite.RESTAPI.Test.TestHelper
{
    public class BlogPostHelper
    {
        public static BlogPost GenerateBlogPost()
        {
            return new BlogPost 
            { 
                BlogPostID = Guid.NewGuid(),
                BlogPostLanguageID = 1,
                Author = "Nietzschy",
                AuthorID = Guid.NewGuid(),
                Title = "The rise of the Unit tester",
                Content = "There once was a crazy dev who was passionate by writting Unit Tests, he would unit test everything he could in his life. One day...", 
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
        }

        public static List<BlogPost> GenerateBlogPosts()
        {
            List<BlogPost> blogPosts = new List<BlogPost>();

            BlogPost post1 = new BlogPost
            {
                BlogPostID = Guid.NewGuid(),
                BlogPostLanguageID = 1,
                Author = "JKRowling",
                AuthorID = Guid.NewGuid(),
                Title = "The rise of the Unit tester",
                Content = "There once was a crazy dev who was passionate about writing Unit Tests. He would unit test everything he could in his life. One day...",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            blogPosts.Add(post1);

            BlogPost post2 = new BlogPost
            {
                BlogPostID = Guid.NewGuid(),
                BlogPostLanguageID = 1,
                Author = "JaneDoe",
                AuthorID = Guid.NewGuid(),
                Title = "The Art of Debugging",
                Content = "Debugging is an essential skill for every developer. In this blog post, we will explore various debugging techniques and best practices.",
                CreatedDate = DateTime.Now.AddDays(-2),
                UpdatedDate = DateTime.Now.AddDays(-1)
            };
            blogPosts.Add(post2);

            BlogPost post3 = new BlogPost
            {
                BlogPostID = Guid.NewGuid(),
                BlogPostLanguageID = 1,
                Author = "JohnSmith",
                AuthorID = Guid.NewGuid(),
                Title = "Introduction to Machine Learning",
                Content = "Machine Learning is revolutionizing various industries. In this blog post, we will provide a beginner-friendly introduction to machine learning concepts and algorithms.",
                CreatedDate = DateTime.Now.AddDays(-5),
                UpdatedDate = DateTime.Now.AddDays(-3)
            };
            blogPosts.Add(post3);

            BlogPost post4 = new BlogPost
            {
                BlogPostID = Guid.NewGuid(),
                BlogPostLanguageID = 1,
                Author = "EmilyJones",
                AuthorID = Guid.NewGuid(),
                Title = "The Power of Networking",
                Content = "Networking is crucial for personal and professional growth. In this blog post, we will discuss the benefits of networking and provide tips for effective networking.",
                CreatedDate = DateTime.Now.AddDays(-8),
                UpdatedDate = DateTime.Now.AddDays(-6)
            };
            blogPosts.Add(post4);

            BlogPost post5 = new BlogPost
            {
                BlogPostID = Guid.NewGuid(),
                BlogPostLanguageID = 1,
                Author = "MarkWilson",
                AuthorID = Guid.NewGuid(),
                Title = "Web Development Best Practices",
                Content = "Building high-quality web applications requires following best practices. In this blog post, we will cover essential web development best practices and guidelines.",
                CreatedDate = DateTime.Now.AddDays(-10),
                UpdatedDate = DateTime.Now.AddDays(-9)
            };
            blogPosts.Add(post5);

            return blogPosts;
        }

    }
}
