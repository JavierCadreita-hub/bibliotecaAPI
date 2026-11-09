using BibliotecaAPI.Data;
using BibliotecaAPI.DTO;
using BibliotecaAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controladores + formato de los errores de validación de los DTOs:
// cada mensaje "código|texto" se convierte en un ErrorResponse { code, description }
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errorList = context.ModelState
                .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                .SelectMany(entry => entry.Value!.Errors.Select(error =>
                {
                    // Separamos "22|Mensaje de error" por la barra
                    var parts = error.ErrorMessage.Split('|');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int parsedCode))
                    {
                        return new ErrorResponse(parsedCode, parts[1]);
                    }
                    // Si el mensaje no lleva código, ponemos 9999
                    return new ErrorResponse(9999, error.ErrorMessage);
                }))
                .ToList();

            return new BadRequestObjectResult(errorList);
        };
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Registro de Entity Framework Core (DbContext)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // DefaultConnection: Es el nombre de la cadena de conexión definida en el archivo appsettings.json
// Registrar el DbContext
builder.Services.AddDbContext<BibliotecaDbContext>(options => options.UseSqlServer(connectionString));

// Repositorios: Scoped = uno nuevo por cada petición HTTP, igual que el DbContext
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<BookRepository>();


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
