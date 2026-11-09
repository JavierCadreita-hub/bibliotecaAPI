using BibliotecaAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Registro de Entity Framework Core (DbContext)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // DefaultConnection: Es el nombre de la cadena de conexión definida en el archivo appsettings.json
// Registrar el DbContext
builder.Services.AddDbContext<BibliotecaDbContext>(options => options.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "BibliotecaAPI v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
