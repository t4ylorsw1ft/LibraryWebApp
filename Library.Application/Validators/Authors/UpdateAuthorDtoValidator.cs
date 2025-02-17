﻿using FluentValidation;
using Library.Application.DTOs.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Validators.Authors
{
    public class UpdateAuthorDtoValidator : AbstractValidator<UpdateAuthorDto>
    {
        public UpdateAuthorDtoValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty().WithMessage("Author ID is required");

            RuleFor(a => a.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

            RuleFor(a => a.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

            RuleFor(a => a.BirthDate)
                .LessThan(DateTime.Today).WithMessage("Birth date must be in the past");

            RuleFor(a => a.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(100).WithMessage("Country name must not exceed 100 characters");
        }
    }
}
