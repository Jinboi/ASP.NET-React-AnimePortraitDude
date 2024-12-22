using AnimePortraitDudeASPReact.Data;
using AnimePortraitDudeASPReact.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Allow React frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!context.Products.Any())
    {
        var products = new[]
        {
            new Product { Name = "Sample Product 1", Price = 10.99M },
            new Product { Name = "Sample Product 2", Price = 20.99M }
        };
        context.Products.AddRange(products);
        context.SaveChanges();

        Console.WriteLine("Seeded products:");
        foreach (var product in products)
        {
            Console.WriteLine($"- {product.Name}: {product.Price}");
        }
    }
    else
    {
        Console.WriteLine("Products already seeded.");
    }
}


app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
