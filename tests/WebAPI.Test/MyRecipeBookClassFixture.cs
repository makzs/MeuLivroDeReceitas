﻿using Azure.Core;
using System.Net.Http.Json;

namespace WebAPI.Test;

public class MyRecipeBookClassFixture : IClassFixture<CustomWebApplicationFactory>
{

    private readonly HttpClient _httpClient;
    public MyRecipeBookClassFixture(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();

    }

    protected async Task<HttpResponseMessage> doPost(string method, Object request, string culture = "en")
    {
        ChangeRequestCulture(culture);

        return await _httpClient.PostAsJsonAsync(method, request);
    }

    private void ChangeRequestCulture(string culture)
    {
        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
    }
}
