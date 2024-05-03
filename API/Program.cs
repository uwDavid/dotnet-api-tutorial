using ApiDemo.Data;
using ApiDemo.Filters.OperationFilter;
using Microsoft.OpenApi.Models;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var DbUrl = builder.Configuration.GetConnectionString("ShirtStoreManagement") ?? string.Empty;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(DbUrl);
});

builder.Services.AddControllers();
// Swagger middleware
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
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
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.MapControllers();

app.Run();

