using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories;
using Talabat.Repository;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            Services.AddAutoMapper(typeof(MappingProfiles));
            #region Error Handling
            Services.Configure<ApiBehaviorOptions>(Options =>
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
        
            return Services;
        
        
        }
    }
}
