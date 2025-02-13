using Library.Application.Interfaces.Services;
using Library.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Library.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookBorrowService, BookBorrowService>();

            return services;
        }
    }
}
