using Common.Dto;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Entities;
using Service.Interfaces;
using Service.servicess;
using Mock;
using Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ה webapi קורא ל AddServices שהוא קורא להזרקת תלויות של AddRepository וכן מבצע את ההזרקת תלויות שלו.

builder.Services.AddServices();

//הגדרת התלויות
//builder.Services.AddScoped<IService<ManagerDto>,ManagerService>();//עבור כל גולש
//builder.Services.AddScoped<IRepository<Manager>,ManagerRepository>();

//builder.Services.AddScoped<IService<CandidateDto>, CandidateService>();
//builder.Services.AddScoped<IRepository<Candidate>, CandidateRepository>();

//builder.Services.AddScoped<IService<JobDto>, JobService>();
//builder.Services.AddScoped<IRepository<Job>, JobRepository>();

//builder.Services.AddScoped<IService<RequirementsDto>, RequirementsService>();
//builder.Services.AddScoped<IRepository<Requirements>, RequirementsRepository>();

//builder.Services.AddScoped<IService<SkillsDto>, SkillsService>();
//builder.Services.AddScoped<IRepository<Skills>, SkillsRepository>();

//builder.Services.AddAutoMapper(typeof(MyMapper));
builder.Services.AddDbContext<IContext, SmartMatchDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
