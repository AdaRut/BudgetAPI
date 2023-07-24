using BudgetAPI;
using BudgetAPI.Entities;
using BudgetAPI.Middleware;
using BudgetAPI.Models;
using BudgetAPI.Models.Validators;
using BudgetAPI.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{


    var builder = WebApplication.CreateBuilder(args);

    //Logging 

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.

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
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    var seeder = builder.Services.BuildServiceProvider().GetService<BudgerSeeder>();
    seeder.SeedData();

    // Configure the HTTP request pipeline.
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimeMiddleware>();

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
