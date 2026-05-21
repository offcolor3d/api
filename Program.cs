using api.Data;
using api.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

// Load .env vars.
Env.Load();

var builder = WebApplication.CreateBuilder(args);

//CORS para proteccion.
builder.Services.AddCors(options =>
{
    //Politica una vez desplegada la api.
    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4321",
            "https://offcolors3d.github.io"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

    //Politica para desarrollo.
    options.AddPolicy("DevelopmentPolicy", policy =>
    {
       policy.AllowAnyOrigin()
       .AllowAnyHeader()
       .AllowAnyMethod();
    });
});

// Configure database connection string.
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

builder.Services.AddScoped<IProductService, ProductService>();

//Agrega el contexto de la base de datos.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(databaseUrl));

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevelopmentPolicy");
}
else
{   
    app.UseCors("ProductionPolicy");
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
