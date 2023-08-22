using AutoMapper;
using BudgetAPI;
using BudgetAPI.Authorization;
using BudgetAPI.DAL;
using BudgetAPI.DAL.Entities;
using BudgetAPI.Middleware;
using BudgetAPI.Services;
using BudgetAPI.Services.Interfaces;
using BudgetAPI.Services.Mapper;
using BudgetAPI.Services.Models.Budget;
using BudgetAPI.Services.Models.User;
using BudgetAPI.Services.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System.Text;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{


    var builder = WebApplication.CreateBuilder(args);

    //Logging 

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.

    var authenticationSettings = new AuthenticationSettings();
    builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
    builder.Services.AddSingleton(authenticationSettings);

    builder.Services.AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = "Bearer";
        option.DefaultScheme = "Bearer";
        option.DefaultChallengeScheme = "Bearer";
    }).AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = authenticationSettings.JwtIssuer,
            ValidAudience = authenticationSettings.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))

        };
    });
    builder.Services.AddAuthorization(option =>
    {
        option.AddPolicy("HasUsernameADMIN123", builder =>  builder.RequireClaim("Username", "ADMIN123")  );
        option.AddPolicy("AtLeast18Years", builder => builder.AddRequirements(new MinimumAgeRequirement(18)));
        option.AddPolicy("Minimum2RestaurantsCreated", builder => builder.AddRequirements(new MinimumAddedRestaurantsRequirement(2)));
    });

    builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, MinimumAddedRestaurantsRequirementHandler>();

    builder.Services.AddDbContext<BudgetDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("BudgetDbConnection"));
    });
    builder.Services.AddScoped<BudgetSeeder>();
    builder.Services.AddScoped<IBudgetService, BudgetService>();
    builder.Services.AddScoped<IGroupService, GroupService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IUserContextService, UserContextService>();


    builder.Services.AddSingleton<Profile, BudgetProfile>();
    builder.Services.AddSingleton<Profile, GroupProfile>();
    builder.Services.AddSingleton<Profile, GroupItemProfile>();
    builder.Services.AddSingleton<AutoMapper.IConfigurationProvider, AutoMapperConfiguration>(p =>
                    new AutoMapperConfiguration(p.GetServices<Profile>()))
                    .AddSingleton<IMapper, Mapper>();


    builder.Services.AddScoped<ErrorHandlingMiddleware>();
    builder.Services.AddScoped<RequestTimeMiddleware>();
    builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
    builder.Services.AddScoped<IValidator<BudgetQuery>, BudgetQueryValidator>();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSwaggerGen();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHealthChecks();
    builder.Services.AddControllers();


    builder.Services.AddCors(options =>
    {
        options.AddPolicy("FrontendApp", policyBuilder =>
        {
            policyBuilder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(builder.Configuration["AllowedOrigins"]);
        });
    });

    var app = builder.Build();

    app.UseResponseCaching();
    app.UseStaticFiles();
    app.UseCors("FrontendApp");

    var seeder = builder.Services.BuildServiceProvider().GetService<BudgetSeeder>();
    seeder.SeedData();

    // Configure the HTTP request pipeline.
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimeMiddleware>();
    app.UseAuthentication();
    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Budget API");
    });

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
