using AutoMapper;
using Library.Application.Common.Mapping;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Authors.DTOs
{
    public class AuthorDetailsDto : IMapWith<Author>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Country { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Author, AuthorDetailsDto>();
        }
    }
}
