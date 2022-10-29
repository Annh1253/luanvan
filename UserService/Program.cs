using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserService.AsyncDataServices;
using UserService.Contracts.InterfaceContracts;
using UserService.Contracts.RepositoryContracts;
using UserService.Data;
using UserService.Repositories;
using UserService.Services;
using Swashbuckle.AspNetCore.Filters;
using UserService.Helpers;
using UserService.Middlewares;

using UserService.AsyncDataService;
using UserService.EventProcessing;
using Microsoft.AspNetCore.Mvc.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
// Add repositories to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Add services to the container.
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Add message bus util
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();


// Add mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Add helpers to container
builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();
builder.Services.AddSingleton<IAuthorizationFilter, AuthorizeAttribute>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<DBContext>(options => { 
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"));});
// if(builder.Environment.IsDevelopment())
// {
//     builder.Services.AddDbContext<DBContext>(option => option.UseInMemoryDatabase("InMem"));
// }else
// {
//     builder.Services.AddDbContext<DBContext>(options => { 
//     options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrings"));
//     options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
// });
// }
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Seed Data
PrepDb.PrepPopulation(app);

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
