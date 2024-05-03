using ApiDemo.Data;
using ApiDemo.Filters.OperationFilter;
using Microsoft.OpenApi.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

var DbUrl = builder.Configuration.GetConnectionString("ShirtStoreManagement") ?? string.Empty;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(DbUrl);
});

builder.Services.AddControllers();

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    // use default version, if client didn't specify version number
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    // Read version from header
    // options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
    // report available versions
    options.ReportApiVersions = true; // response header will show list of versions
});

// Swagger middleware
// builder.Services.AddEndpointsApiExplorer(); replace this with AddVersionedApiExplorer
builder.Services.AddVersionedApiExplorer(
    options => options.GroupNameFormat = "'v'VVV");
// Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
// check dotnet/aspnet-api-versioning repo + wiki
builder.Services.AddSwaggerGen(c =>
{
    // add multiple version
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API v1", Version = "version 1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Web API v2", Version = "version 2" });

    c.OperationFilter<AuthorizationHeaderOperationFilter>(); // custom defined filter

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });  //specify id of scheme "Bearer" => all occurrence must be the same as Id in our filter above

});


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"); // update AddVersionedApiExplorer()
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApi v2");
        }
    );
}

// app.UseHttpsRedirection();
app.MapControllers();

app.Run();

