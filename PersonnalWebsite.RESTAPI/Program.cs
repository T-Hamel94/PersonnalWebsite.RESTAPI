using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonnalWebsite.RESTAPI.Data.Context;
using PersonnalWebsite.RESTAPI.Data.Repo.SQLServer;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Service;
using System.Reflection;
using System.Text;
using System.Xml;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:TokenKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };      
    });

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

// Log4Net
var log4NetConfig = new XmlDocument();
log4NetConfig.Load(File.OpenRead("log4net.config"));

var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

log4net.Config.XmlConfigurator.Configure(repo, log4NetConfig["log4net"]);


// Configure the HTTP request pipeline.
if (webApp.Environment.IsDevelopment())
{
    webApp.UseSwagger();
    webApp.UseSwaggerUI();
}

webApp.UseCors("MyAllowSpecificOrigins");

webApp.UseHttpsRedirection();

webApp.UseAuthentication();

webApp.UseAuthorization();

webApp.MapControllers();

webApp.Run();
