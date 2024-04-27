using ApiDemo.Data;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var DbUrl = builder.Configuration.GetConnectionString("ShirtStoreManagement") ?? string.Empty;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(DbUrl);
});

builder.Services.AddControllers();

var app = builder.Build();


// app.UseHttpsRedirection();
app.MapControllers();

app.Run();

