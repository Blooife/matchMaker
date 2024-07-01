using Profile.Application.Extensions;
using Profile.Infrastructure.Extensions;
using Profile.Infrastructure.Services;
using Profile.Presentation.Extensions;
using Profile.Presentation.MiddlewareHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.ConfigureApplication();
builder.Services.ConfigurePresentation(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.MapControllers();

app.UseRouting();

app.MapGrpcService<ProfileGrpcService>();

app.ApplyMigrations(app.Services);

app.Run();