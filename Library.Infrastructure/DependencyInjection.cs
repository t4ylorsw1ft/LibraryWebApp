using Library.Infrastructure.Interfaces;
using Library.Domain.Interfaces.Repositories;
using Library.Application.Interfaces.Security;
using Library.Application.Interfaces.Services;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Security;
using Library.Infrastructure.Services;
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

            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookBorrowRepository, BookBorrowRepository>();

            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IJwtValidator, JwtValidator>();
            return services;
        }

    }
}
