using FluentValidation;
using FluentValidation.AspNetCore;

namespace YunusExpress_MVC
{
    public static class ServicesRegistiration
    {
        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining(typeof(ServicesRegistiration));
            return services;
        }
    }
}