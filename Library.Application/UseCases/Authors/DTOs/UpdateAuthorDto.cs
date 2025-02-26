using AutoMapper;
using Library.Application.Common.Mapping;
using Library.Domain.Entities;

namespace Library.Application.UseCases.Authors.DTOs
{
    public class UpdateAuthorDto : IMapWith<Author>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Country { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAuthorDto, Author>();
        }
    }
}
