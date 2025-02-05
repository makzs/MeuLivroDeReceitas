using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Application.Services.Cryptography;

public class PasswordEncripter
{

    public string Encrypt(string password)
    {
        var chaveAdicional = "ABC";
        var newPassword = $"{password}{chaveAdicional}";

        var bytes = Encoding.UTF8.GetBytes(newPassword);
        var hashBytes = SHA512.HashData(bytes);

        return StringBytes(hashBytes);
    }

    // converte o array de bytes em string
    private static string StringBytes(Byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach(byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }

        return sb.ToString();
    }

}
