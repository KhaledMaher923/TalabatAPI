using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services Add to the Container
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });
            
            builder.Services.AddApplicationServices();

            builder.Services.AddIdentityServices();
            #endregion

            var app = builder.Build();

            #region Update-Databsae
            using var scope = app.Services.CreateScope();
            // Get the StoreContext instance from the service provider
            var services = scope.ServiceProvider;
            // Get the StoreContext instance

            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var DbContext = services.GetRequiredService<StoreContext>();
                await DbContext.Database.MigrateAsync();

                var IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync();
                var UserManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);
                await StoreContextSeed.SeedAsync(DbContext);
            }
            catch(Exception ex) 
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex,"An Error Occurred While Migrating the Database.");
            }
            #endregion

            #region Configure - Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleWare>();
                app.UseSwaggerMiddlewares();
            }
            app.UseStatusCodePagesWithReExecute("errors/{0}");

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
