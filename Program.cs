using LibraryApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) Wszystkie dodania usług (AddDbContext, AddControllers, AddCors, AddSwagger itp.)
//    MUSZĄ znajdować się tutaj, przed builder.Build()

// Dodaj DbContext z SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodaj kontrolery
builder.Services.AddControllers();

// Dodaj CORS — na cały projekt
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// (Opcjonalnie) Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build(); // <— od tego momentu NIE wolno już robić builder.Services.AddX

// 2) Middleware — kolejność ma znaczenie

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Włącz CORS (korzystając z polityki zarejestrowanej powyżej)
app.UseCors();

// Umożliwiamy serwowanie statycznych plików (index.html i main.js w wwwroot)
app.UseStaticFiles();

app.MapControllers();

app.Run();

