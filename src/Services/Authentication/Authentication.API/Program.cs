using Authentication.API.Extensions;
using Authentication.API.MiddlewareHandlers;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureDbContext(builder.Configuration); 
builder.Services.ConfigureIdentity();
builder.Services.AddAuthorization(); 
builder.Services.AddFluentValidationAutoValidation(); 
builder.Services.ConfigureProviders();
builder.Services.ConfigureRepositories(); 
builder.Services.ConfigureServices(); 
builder.Services.AddControllers(); 
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.ApplyMigrations(app.Services);

app.Run();