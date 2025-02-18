using MyRecipeBook.Application.Services.Cryptography;

namespace CommonTestsUtilities.Cryptography;

public class PasswordEncripterBuilder
{
    public static PasswordEncripter Build() => new PasswordEncripter("ABC1234");
}
