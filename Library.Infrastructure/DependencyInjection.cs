using Library.Application.Interfaces;
using Library.Application.Interfaces.Repositories;
using Library.Application.Interfaces.Security;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration["DbConnection"];
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(connectionString));

            services.AddScoped<IAppDbContext>(provider =>
                provider.GetService<AppDbContext>());

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookBorrowRepository, BookBorrowRepository>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJWTProvider, JWTProvider>();
            return services;
        }

    }
}
