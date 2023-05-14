using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;

namespace PersonnalWebsite.RESTAPI.Data.Context
{
    public class DbContextGeneration
    {
        private static DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private static PooledDbContextFactory<ApplicationDbContext> _pooledDbContext;

        static DbContextGeneration()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("PersonnalWebsiteConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
#if DEBUG
                .LogTo(message => Debug.WriteLine(message), LogLevel.Information).EnableSensitiveDataLogging()
#endif
                .Options;
            _pooledDbContext = new PooledDbContextFactory<ApplicationDbContext>(_dbContextOptions);
        }
        public static ApplicationDbContext GetApplicationDBContext()
        {
            return _pooledDbContext.CreateDbContext();
        }
    }
}
