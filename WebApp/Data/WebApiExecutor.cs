using System.Text.Json;

using Microsoft.Extensions.Diagnostics.HealthChecks;

using System.Net.Http.Headers;

using Newtonsoft.Json;

namespace WebApp.Data;

public class WebApiExecutor : IWebApiExecutor
{
    private const string apiName = "ShirtsApi";
    private const string authApiName = "AuthorityApi";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public WebApiExecutor(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    // ? - we are aware that return type could be null
    public async Task<T?> InvokeGet<T>(string relativeUrl)
    // get method require diff way to handle error
    {
        var httpClient = _httpClientFactory.CreateClient(apiName);
        await AddJwtToHeader(httpClient);

        // return await httpClient.GetFromJsonAsync<T>(relativeUrl);
        var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
        var response = await httpClient.SendAsync(request);
        //   await HandlePotentialError(response);

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
    {
        var httpClient = _httpClientFactory.CreateClient(apiName);
        await AddJwtToHeader(httpClient);

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
        await AddJwtToHeader(httpClient);

        var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);
        // response.EnsureSuccessStatusCode(); - using error handling below
        await HandlePotentialError(response);
    }

    public async Task InvokeDelete(string relativeUrl)
    {
        var httpClient = _httpClientFactory.CreateClient(apiName);
        await AddJwtToHeader(httpClient);

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

    // authenticate + add jwt to header
    private async Task AddJwtToHeader(HttpClient httpClient)
    {
        JwtToken? token = null;
        string? strToken = _httpContextAccessor.HttpContext?.Session.GetString("access_token");
        if (!string.IsNullOrWhiteSpace(strToken))
        {
            token = JsonConvert.DeserializeObject<JwtToken>(strToken);
        }

        if (token == null || token.ExpiresAt <= DateTime.UtcNow)
        {
            var clientId = _configuration.GetValue<string>("ClientId");
            Console.WriteLine(clientId);
            var secret = _configuration.GetValue<string>("Secret");
            Console.WriteLine(secret);

            // 1-authentication
            var authClient = _httpClientFactory.CreateClient(authApiName);
            // POST to /auth
            var response = await authClient.PostAsJsonAsync("auth", new AppCredential
            {
                ClientId = clientId,
                Secret = secret
            });

            response.EnsureSuccessStatusCode(); // if not correct => throws exception

            // 2-get jwt
            strToken = await response.Content.ReadAsStringAsync();
            token = JsonConvert.DeserializeObject<JwtToken>(strToken);

            // save token to session
            _httpContextAccessor.HttpContext?.Session.SetString("access_token", strToken);
        }

        // 3-pass jwt to headers
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", token?.AccessToken
        );
    }
}