using Microsoft.EntityFrameworkCore;
using PersonnalWebsite.RESTAPI.Data.Context;
using PersonnalWebsite.RESTAPI.Data.Repo;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Service;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen();

//Db Context
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer());

//Dependency Injections
builder.Services.AddScoped<IUserRepo, UserSqlRepo>();
builder.Services.AddScoped<IUserService, UserManipulation>();

WebApplication webApp = builder.Build();

// Configure the HTTP request pipeline.
if (webApp.Environment.IsDevelopment())
{
    webApp.UseSwagger();
    webApp.UseSwaggerUI();
}

webApp.UseHttpsRedirection();

webApp.UseAuthorization();

webApp.MapControllers();

webApp.Run();
