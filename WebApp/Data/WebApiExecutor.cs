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
    {
        var httpClient = _httpClientFactory.CreateClient(apiName);
        return await httpClient.GetFromJsonAsync<T>(relativeUrl);
    }

    public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
    {
        var httpClient = _httpClientFactory.CreateClient(apiName);
        var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task InvokePut<T>(string relativeUrl, T obj)
    // PUT doesn't return any objects => return type is only Task
    {
        var httpClient = _httpClientFactory.CreateClient(apiName);
        var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);
        response.EnsureSuccessStatusCode();
    }

    public async Task InvokeDelete(string relativeUrl)
    {
        var httpClient = _httpClientFactory.CreateClient(apiName);
        var response = await httpClient.DeleteAsync(relativeUrl);
        response.EnsureSuccessStatusCode();
    }
}