namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : MyRecipeBookException
{
    public IList<string> ErrorsMessages { get; set; }

    public ErrorOnValidationException(IList<string> erros)
    {
        ErrorsMessages = erros;
    }
}
