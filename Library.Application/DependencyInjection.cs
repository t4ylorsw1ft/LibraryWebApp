using FluentValidation;
using FluentValidation.AspNetCore;
using Library.Application.Validators.Authors;
using Library.Application.Validators.Books;
using Library.Application.Validators.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Library.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssemblyContaining<CreateAuthorDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateAuthorDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateBookDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateBookDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();   

            return services;
        }
    }
}
