using AutoMapper;
using Library.Application.Common.Mapping;
using Library.Domain.Entities;

namespace Library.Application.UseCases.Books.DTOs
{
    public class CreateBookDto : IMapWith<Book>
    {
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? ImagePath { get; set; }
        public Guid AuthorId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBookDto, Book>();
        }
    }
}
