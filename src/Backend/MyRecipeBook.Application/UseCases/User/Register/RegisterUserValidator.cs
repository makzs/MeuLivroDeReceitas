﻿using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessageException.NAME_EMPTY);
        RuleFor(user => user.Email).NotEmpty();
        RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6);
        When(user => string.IsNullOrEmpty(user.Email) == false, () => 
        {
            RuleFor(user => user.Email).EmailAddress();
        });
    }
}
