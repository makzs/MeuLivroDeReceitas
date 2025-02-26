using CommonTestsUtilities.Cryptography;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
    }
    
    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        var result = useCase.Execute(request);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => result);
    
        exception.ErrorsMessages.ShouldHaveSingleItem()
            .ShouldBe(ResourceMessageException.EMAIL_ALREADY_REGISTERED);
    }    

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(request.Name);

        var result = useCase.Execute(request);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => result);
    
        exception.ErrorsMessages.ShouldHaveSingleItem()
            .ShouldBe(ResourceMessageException.NAME_EMPTY);
    }

    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnityOfWorkBuilder.Build();
        var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

        if (string.IsNullOrEmpty(email) == false)
            readRepositoryBuilder.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(writeRepository, readRepositoryBuilder.Build(), mapper, passwordEncripter, unitOfWork);
    }

}
