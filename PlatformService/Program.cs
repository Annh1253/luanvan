using System.Data;
using System.Text;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System;
using System.Collections.Immutable;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PlatformService.Contracts.RepositoryContracts;
using PlatformService.Data;
using PlatformService.Repository;
using PlatformService.SyncDataServices.Http;
using PlatformService.AsyncDataServices;

var builder = WebApplication.CreateBuilder(args);

//Register Repository
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
if(builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDBContext>(option => option.UseInMemoryDatabase("InMem"));
}else
{
    builder.Services.AddDbContext<AppDBContext>(options => { 
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConn"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
}

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Console.WriteLine($"---> Command Service: {builder.Configuration["CommandService"]}");

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
