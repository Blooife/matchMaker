using Profile.Application.Extensions;
using Profile.Infrastructure.Extensions;
using Profile.Infrastructure.Services;
using Profile.Presentation.Extensions;
using Profile.Presentation.MiddlewareHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.ConfigureApplication(builder.Configuration);
builder.Services.ConfigurePresentation(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("MyCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();
app.MapGrpcService<ProfileGrpcService>();

app.ApplyMigrations(app.Services);

app.Run();