using MyRecipeBook.Communication.Requests;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Shouldly;
using WebAPI.Test.InlineData;
using CommonTestsUtilities.Requests;
using MyRecipeBook.Exceptions;
using System.Globalization;

namespace WebAPI.Test.Login;

public class DoLoginTest : MyRecipeBookClassFixture
{
    private readonly string method = "login";

    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
    {

        _email = factory.GetEmail();
        _password = factory.GetPassword();
        _name = factory.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson{
            Email = _email,
            Password = _password
        };

        var response = await doPost(method, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldSatisfyAllConditions(
            name => name.ShouldNotBeNullOrWhiteSpace(),
            name => name.ShouldBe(_name)
        );

    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Invalid(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();

        var response = await doPost(method, request, culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessageException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        errors.ShouldHaveSingleItem();
        var errorMessage = errors.First().GetString();
        errorMessage.ShouldBe(expectedMessage);
    }

}
