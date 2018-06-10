using MessengerApi.DAL.EF;
using MessengerApi.DAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MessengerApi.Util;
using AutoMapper;
using MessengerApi.BLL.Interfaces;
using MessengerApi.BLL.Services;
using MessengerApi.DAL.Interfaces;
using MessengerApi.DAL.Repositories;
//using Microsoft.Extensions.Hosting;
using MessengerApi.TcpServer.Core;
using MessengerApi.Hubs;

namespace MessengerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            MapperConfiguration configMapper = new MapperConfiguration(
               cfg => { cfg.AddProfile(new AutoMapperProfile()); }
           );
            AutomapperConfiguration.Configure();
            services.AddSingleton(ctx => configMapper.CreateMapper());
            services.AddSingleton(Configuration);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IGroupService, GroupServise>();
            services.AddScoped<IGroupUserService, GroupUserServise>();

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ApplicationContext"), b => b.MigrationsAssembly("MessengerApi.Migrations")));

            services.AddIdentity<ApplicationUser, IdentityRole>(opts => { opts.User.RequireUniqueEmail = true; })
               .AddEntityFrameworkStores<ApplicationContext>()
               .AddDefaultTokenProviders();

            //services.AddSingleton<IHostedService, ServerObject>();

            services.AddMvc();

            services.AddSignalR();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chathub");
            });

            app.UseMvc();
        }
    }
}
