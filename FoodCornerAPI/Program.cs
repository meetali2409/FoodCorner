using AutoMapper;
using FoodCorner.DTO;
using FoodCornerAPI.Data;
using FoodCornerAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<FoodCornerDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("FoodCornerDb"))
        );

        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireDigit = true;
        })
        .AddEntityFrameworkStores<FoodCornerDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowUI", policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod());
        });

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        var app = builder.Build();

        app.MapGet("/", () => "FoodCorner API Running");

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        try
        {
            using var scope = app.Services.CreateScope();
            await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Role seeding failed: {ex.Message}");
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowUI");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}