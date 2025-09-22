using Colegio.Application.Services.Implementations;
using Colegio.Application.Services.Interfaces;
using Colegio.Infrastructure.Extensions;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Application & Validators
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssembly(typeof(Colegio.Application.AssemblyRef).Assembly);

// Infra (DbContext, repos, UoW)
builder.Services.AddInfrastructure(builder.Configuration);

//Services
builder.Services.AddScoped<ISubjectOfferingService, SubjectOfferingService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IAssessmentService, AssessmentService>();
builder.Services.AddScoped<IGradeService, GradeService>();

//Cors
const string FrontCors = "FrontCors";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(FrontCors, p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("http://127.0.0.1:5500")); 
});

var app = builder.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors(FrontCors);
app.Run();