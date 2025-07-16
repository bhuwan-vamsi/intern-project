using APIPractice.Data;
using APIPractice.Repositories;
using APIPractice.Services;
using APIPractice.Extensions;
using Microsoft.EntityFrameworkCore;

namespace APIPractice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            Console.WriteLine("Using DB: " + builder.Configuration.GetConnectionString("DefaultConnection"));

            // Register Repositories and Services
            builder.Services.AddDependencies();

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Middleware pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowReactApp");
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
