using Microsoft.Extensions.Configuration;

namespace MessengerApi.Migrations.Helpers
{
    public class ConfigurationHelper
    {
        private IConfiguration configuration;

        public IConfiguration Configuration => configuration;

        public ConfigurationHelper()
        {
            configuration = BuildConfiguration();
        }

        private IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build();
        }
    }
}
