using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Infrastructure.Repositories;
using Library.Application.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Library.Application.Interfaces.Security;
using Library.Infrastructure.Security;
using Library.Application.Interfaces;

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
