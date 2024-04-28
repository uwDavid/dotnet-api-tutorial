using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

namespace WebApp.Data;

public class WebApiException : Exception
{
    public ErrorReponse? ErrorReponse { get; }

    // constructor
    public WebApiException(string errorJson)
    {
        ErrorReponse = JsonSerializer.Deserialize<ErrorReponse>(errorJson);
    }
}