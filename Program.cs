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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

