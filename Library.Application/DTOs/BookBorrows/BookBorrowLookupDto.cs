using AutoMapper;
using Library.Application.Common.Mapping;
using Library.Application.DTOs.Books;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.BookBorrows
{
    public class BookBorrowLookupDto : IMapWith<BookBorrow>
    {
        public Guid Id { get; set; }

        public DateTime BorrowedAt { get; set; }
        public DateTime ReturnBy { get; set; }
        public bool IsReturned { get; set; }

        public Guid BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookGenre { get; set; } = string.Empty;
        public string? BookImagePath { get; set; }

        public Guid AuthorId { get; set; }
        public string AuthorFirstName { get; set; } = string.Empty;
        public string AuthorLastName { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BookBorrow, BookBorrowLookupDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.BookGenre, opt => opt.MapFrom(src => src.Book.Genre))
                .ForMember(dest => dest.BookImagePath, opt => opt.MapFrom(src => src.Book.ImagePath))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Book.Author.Id))
                .ForMember(dest => dest.AuthorFirstName, opt => opt.MapFrom(src => src.Book.Author.FirstName))
                .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src => src.Book.Author.LastName));
        }
    }
}
