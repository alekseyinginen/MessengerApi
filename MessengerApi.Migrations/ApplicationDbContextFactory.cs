using MessengerApi.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MessengerApi.Migrations.Helpers;
using Microsoft.Extensions.Configuration;

namespace MessengerApi.Migrations
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationHelper().Configuration;

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ApplicationContext"), b => b.MigrationsAssembly("MessengerApi.Migrations"));

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
