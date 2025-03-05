using CommonTestsUtilities.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.ResponseCompression;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using WebAPI.Test.InlineData;

namespace WebAPI.Test.User.Register;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly string method = "user";

    private readonly HttpClient _httpClient;

    public RegisterUserTest(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await _httpClient.PostAsJsonAsync(method, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldSatisfyAllConditions(
            name => name.ShouldNotBeNullOrWhiteSpace(),
            name => name.ShouldBe(request.Name)
        );

    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

        var response = await _httpClient.PostAsJsonAsync(method, request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessageException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture) );

        errors.ShouldHaveSingleItem();
        var errorMessage = errors.First().GetString();
        errorMessage.ShouldBe(expectedMessage);


    }
}
