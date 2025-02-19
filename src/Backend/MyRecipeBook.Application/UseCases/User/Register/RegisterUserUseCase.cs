using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Microsoft.Extensions.Options;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using System.Runtime.CompilerServices;
using MyRecipeBook.Domain.Repositories.User;
using AutoMapper;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    
    // Injeção de dependencia
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;
    private readonly PasswordEncripter _passwordEncripter;

    public RegisterUserUseCase(
        IUserWriteOnlyRepository userWriteOnlyRepository, 
        IUserReadOnlyRepository userReadOnlyRepository,
        IMapper mapper,
        PasswordEncripter passwordEncripter,
        IUnityOfWork unityOfWork
        )
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _unityOfWork = unityOfWork;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {

        // Validar a request
        await Validate(request);

        // Mapear a request em uma entidade
        /*var autoMapper = new AutoMapper.MapperConfiguration(options => {
            options.AddProfile(new Automapping());
        }).CreateMapper();*/

        var user = _mapper.Map<Domain.Entities.User>(request);

        // Criptografia da senha
        user.Password = _passwordEncripter.Encrypt(request.Password);

        // Salvar no banco de dados
        await _userWriteOnlyRepository.Add(user);
        await _unityOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var Validator = new RegisterUserValidator();

        var result = Validator.Validate(request);

        var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if(emailExist)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessageException.EMAIL_ALREADY_REGISTERED));
        }

        if(result.IsValid == false)
        {

            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }

}
