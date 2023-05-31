using Microsoft.EntityFrameworkCore;
using PersonnalWebsite.RESTAPI.Data.Context;
using PersonnalWebsite.RESTAPI.Data.Repo.SQLServer;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Service;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen();

// Db Context
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer());

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

// Dependency Injections
builder.Services.AddScoped<IUserRepo, UserSqlRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IBlogPostRepo, BlogPostSqlRepo>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();

WebApplication webApp = builder.Build();

// Configure the HTTP request pipeline.
if (webApp.Environment.IsDevelopment())
{
    webApp.UseSwagger();
    webApp.UseSwaggerUI();
}

webApp.UseCors("MyAllowSpecificOrigins");

webApp.UseHttpsRedirection();


webApp.UseAuthorization();

webApp.MapControllers();

webApp.Run();
