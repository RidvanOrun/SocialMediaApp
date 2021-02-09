using FluentValidation;
using SocialMediaApp.Application.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Application.Services.Validation
{
    public class RegisterValidation:AbstractValidator<RegisterDTO>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Enter e mail Adress").EmailAddress().WithMessage("Please type into a valid e mail adress");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Enter e mail Adress");
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x=>x.Password).WithMessage("Password do not macth");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Password do not macth");
        }

    }
}
