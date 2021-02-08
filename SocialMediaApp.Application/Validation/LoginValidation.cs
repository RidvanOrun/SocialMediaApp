using FluentValidation;
using SocialMediaApp.Application.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Application.Services.Validation
{
    public class LoginValidation:AbstractValidator<LoginDTO>
    {
        public LoginValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Enter a username");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Enter a username");
        }
    }
}
