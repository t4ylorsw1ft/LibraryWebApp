using AutoMapper;
using Library.Application.Common.Mapping;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Users.DTOs
{
    public class UserDetailsDto : IMapWith<User>
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDetailsDto>();
        }
    }
}
