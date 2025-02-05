using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase
{

    public ResponseRegisteredUserJson Execute(RequestRegisterUserJson request)
    {

        // Validar a request
        Validate(request);

        // Mapear a request em uma entidade

        // Criptografia da senha

        // Salvar no banco de dados

        return new ResponseRegisteredUserJson
        {
            Name = request.Name,
        };
    }

    public void Validate(RequestRegisterUserJson request)
    {
        var Validator = new RegisterUserValidator();

        var result = Validator.Validate(request);

        if(result.IsValid == false)
        {

            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }

}
