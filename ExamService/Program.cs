using ExamService.Contracts.RepositoryContracts;
using ExamService.Contracts.ServiceContracts;
using ExamService.Repositories;
using ExamService.Services;
using Microsoft.EntityFrameworkCore;
using UserService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services.
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<IExamService, ExamsService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IOptionService, OptionService>();




//Add repository
builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IOptionRepository, OptionRepository>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddDbContext<DBContext>(options => { 
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"));});
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
