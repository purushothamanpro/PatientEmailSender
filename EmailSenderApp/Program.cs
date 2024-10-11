using EmailSenderApp.Auth.Models;
using EmailSenderApp.Repository.Interface;
using EmailSenderApp.Repository;
using EmailSenderApp.Service.Interface;
using EmailSenderApp.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Bind OAuth settings from appsettings.json
builder.Services.Configure<OAuthSettings>(builder.Configuration.GetSection("OAuthSettings"));

builder.Services.AddControllers();
builder.Services.AddHttpClient<IFhirApiRepository, FhirApiRepository>();
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<IEmailService, EmailService>();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
