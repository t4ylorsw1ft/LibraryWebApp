using AutoMapper;
using Library.Application.Common.Mapping;
using Library.Application.DTOs.Books;
using Library.Domain.Entities;

namespace Library.Application.DTOs.Authors
{
    public class AuthorLookupDto : IMapWith<Author>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Author, AuthorLookupDto>();
        }
    }
}
