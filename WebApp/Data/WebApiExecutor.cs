using System.Text.Json;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApp.Data;

public class WebApiExecutor : IWebApiExecutor
{
    private const string apiName = "ShirtsApi";
    private readonly IHttpClientFactory _httpClientFactory;


    public WebApiExecutor(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // ? - we are aware that return type could be null
    public async Task<T?> InvokeGet<T>(string relativeUrl)
    // get method require diff way to handle error
    {
        var httpClient = _httpClientFactory.CreateClient(apiName);
        // return await httpClient.GetFromJsonAsync<T>(relativeUrl);
        var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
        var response = await httpClient.SendAsync(request);
        await HandlePotentialError(response);

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
    {
        var httpClient = _httpClientFactory.CreateClient(apiName);
        var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);

        // response.EnsureSuccessStatusCode();
        // this creates an unhandled error if our POST call is invalid

        /* extract to Handle Error Method
        if (!response.IsSuccessStatusCode)
        {
            var errorJson = await response.Content.ReadAsStringAsync();
            // var ErrorReponse = JsonSerializer.Deserialize<ErrorReponse>(errorJson);
            throw new WebApiException(errorJson);
        }
        */
        await HandlePotentialError(response);

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task InvokePut<T>(string relativeUrl, T obj)
    // PUT doesn't return any objects => return type is only Task
    {
        var httpClient = _httpClientFactory.CreateClient(apiName);
        var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);
        // response.EnsureSuccessStatusCode(); - using error handling below
        await HandlePotentialError(response);
    }

    public async Task InvokeDelete(string relativeUrl)
    {
        var httpClient = _httpClientFactory.CreateClient(apiName);
        var response = await httpClient.DeleteAsync(relativeUrl);
        // response.EnsureSuccessStatusCode(); - use error handling
        await HandlePotentialError(response);
    }

    public async Task HandlePotentialError(HttpResponseMessage httpResponse)
    {
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorJson = await httpResponse.Content.ReadAsStringAsync();
            throw new WebApiException(errorJson);
        }
    }
}