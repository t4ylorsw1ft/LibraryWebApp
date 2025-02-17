using AutoMapper;
using Library.Application.Common.Mapping;
using Library.Domain.Entities;

namespace Library.Application.DTOs.Books
{
    public class UpdateBookDto : IMapWith<Book>
    {
        public Guid Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImagePath { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBookDto, Book>();
        }
    }
}
