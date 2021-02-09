using FluentValidation;
using SocialMediaApp.Application.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Application.Validation
{
    public class TweetValidation:AbstractValidator<AddTweetDTO>
    {
        public TweetValidation() => RuleFor(x => x.Text).NotEmpty().WithMessage("Can not't be empty").MaximumLength(256).WithMessage("Must be less then 256 character");

    }
}
