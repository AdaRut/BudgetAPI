using BudgetAPI;
using BudgetAPI.Authorization;
using BudgetAPI.Entities;
using BudgetAPI.Middleware;
using BudgetAPI.Models;
using BudgetAPI.Models.Validators;
using BudgetAPI.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    builder.Services.AddControllers().AddFluentValidation();
    builder.Services.AddDbContext<BudgetDbContext>();
    builder.Services.AddScoped<BudgerSeeder>();
    builder.Services.AddScoped<IBudgetService, BudgetService>();
    builder.Services.AddScoped<IGroupService, GroupService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddScoped<ErrorHandlingMiddleware>();
    builder.Services.AddScoped<RequestTimeMiddleware>();
    builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
    builder.Services.AddScoped<IUserContextService, UserContextService>();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    var seeder = builder.Services.BuildServiceProvider().GetService<BudgerSeeder>();
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
