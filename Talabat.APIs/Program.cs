using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;

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
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
                                    .SelectMany(P => P.Value.Errors)
                                    .Select(E => E.ErrorMessage).ToArray();
                    var ValidationErrorRespone = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorRespone);
                };
            });

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
                await StoreContextSeed.SeedAsync(DbContext);
            }
            catch(Exception ex) 
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex,"An Error Occurred While Migrating the Database.");
            }
            #endregion

            app.UseMiddleware<ExceptionMiddleWare>();
            #region Configure - Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
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
