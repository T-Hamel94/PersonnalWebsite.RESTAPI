using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonnalWebsite.RESTAPI.Data.SQLServer;

namespace PersonnalWebsite.RESTAPI.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserSQLServer> Users { get; set; }
        public DbSet<BlogPostSQLServer> BlogPosts { get; set; }
    }
}
