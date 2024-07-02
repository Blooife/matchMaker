using Authentication.API.Extensions;
using Authentication.API.MiddlewareHandlers;
using Authentication.BusinessLogic.Extensions;
using Authentication.DataLayer.Extensions;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(); 
builder.Services.AddFluentValidationAutoValidation(); 
builder.Services.AddControllers(); 
builder.Services.ConfigureDataLayer(builder.Configuration);
builder.Services.ConfigureBusinessLogic(builder.Configuration);
builder.Services.ConfigureApi(builder.Configuration);

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