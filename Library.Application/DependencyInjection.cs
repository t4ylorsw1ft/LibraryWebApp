using FluentValidation;
using FluentValidation.AspNetCore;

using Library.Application.Interfaces.Services;
using Library.Application.Services;
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

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookBorrowService, BookBorrowService>();
            services.AddScoped<IFileService, FileService>();

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
