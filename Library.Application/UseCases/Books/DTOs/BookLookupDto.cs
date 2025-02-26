using AutoMapper;
using Library.Application.Common.Mapping;
using Library.Domain.Entities;

namespace Library.Application.UseCases.Books.DTOs
{
    public class BookLookupDto : IMapWith<Book>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int AvaliableQuantity { get; set; }
        public string? BookImagePath { get; set; }

        public Guid AuthorId { get; set; }
        public string AuthorFirstName { get; set; } = string.Empty;
        public string AuthorLastName { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Book, BookLookupDto>()
                .ForMember(dest => dest.AuthorFirstName, opt => opt.MapFrom(src => src.Author.FirstName))
                .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src => src.Author.LastName));
        }
    }
}
