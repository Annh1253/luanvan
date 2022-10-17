using Microsoft.EntityFrameworkCore;
using UserService.Contracts.InterfaceContracts;
using UserService.Contracts.RepositoryContracts;
using UserService.Data;
using UserService.Repositories;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add repositories to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Add services to the container.
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContext>(options => { 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));});
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
