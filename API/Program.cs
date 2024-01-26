using System.Text;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data.Repositories;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Middlewares;
using Infrastructure.Data.ViewModel;
using StackExchange.Redis;
using Infrastructure.Data.Extensions;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Controllers for handling HTTP requests.
builder.Services.AddControllers();

// Swagger/OpenAPI configuration for API documentation.
builder.Services.AddEndpointsApiExplorer();



// Database context for the main application.
builder.Services.AddDbContext<EntityDataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Database context for identity management.
builder.Services.AddDbContext<ApplicationIdentityDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("IdentityConnection"));
});

// Singleton for managing Redis connections.
builder.Services.AddSingleton<ConnectionMultiplexer>(c =>
{
    var config = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
    return ConnectionMultiplexer.Connect(config);
});
//builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JsonWebTokenKeys")); // Adjust if using a different configuration source

// Configuration for JWT token settings.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JsonWebTokenKeys"));

builder.Services.AddUnitOfWork();
// services.AddSyncData();
builder.Services.AddRepositories();
builder.Services.AddCustomServices();
// Scoped services for repositories and services.
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<ITokenGenerationService, TokenGenerationService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// AutoMapper for object mapping.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Identity configuration for user management.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

// Cross-Origin Resource Sharing (CORS) configuration.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200", "https://localhost:4200") // Replace with the actual Angular app URLs
           .AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});

builder.Services.AddJWTTokenServices(builder.Configuration);
// Authentication configuration for JWT tokens.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Albums_API", Version = "v1" });

    // Define the Bearer token security scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // Add the Bearer token as a requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();

    // Enable Swagger UI for API documentation in development mode.
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Albums_API v1");
    });
}


// Enable CORS, HTTPS redirection, and configure authentication and authorization.
app.UseCors("AllowAngularApp");
app.UseExceptionMiddleware();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
// Map controllers for handling HTTP requests.
app.MapControllers();

// Apply migrations and seed data during application startup.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>(); // Get logger

    try
    {
        var context = services.GetRequiredService<EntityDataContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            logger.LogInformation("Applying migrations...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Migrations applied successfully.");

            // Seed initial data for the main application.
            //await EcommerceContextSeed.SeedDataAsync(context);

            // Seed initial data for identity management.
            await ApplicationIdentityDbContextSeed.SeedAsync(services);
        }
    }
    catch (Exception ex)
    {
        // Log exceptions or handle them as needed
        // Consider logging this exception or handling it appropriately
        logger.LogError(ex, "An error occurred while applying migrations");
        // Optionally, you can handle the exception further, like rethrowing or terminating the application
    }
}

// Run the application.
app.Run();
